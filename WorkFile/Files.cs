using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkMisc;
using WorkString;

namespace WorkFile
{
    public class Files
    {
      /// <summary>
      /// Получить массив значений с плавующей точкой из hex-файла данных
      /// </summary>
      /// <param name="Path">Путь до файла</param>
      /// <param name="Addr">Начальный адрес значений</param>
      /// <param name="Count">Количество значений</param>
      /// <param name="ByteOrder">Порядок следования байт</param>
      /// <returns></returns>
      public Dictionary<int,float> GetFloatFromHexFile(string Path, 
                                         out List<byte[]> RawData,
                                         int Addr = 0, 
                                         int Count = 0, 
                                         string ByteOrder="0123",
                                         bool NotZero = false)
      {
         RawData = new List<byte[]>();
         byte[] BytesRead = null;
         if ( ! File.Exists( Path ))
         {
            MessageBox.Show(text: "Hex-файла нет, будет создан новый.",
                               caption: "Предупреждение",
                               buttons: MessageBoxButtons.OK,
                               icon: MessageBoxIcon.Information);
            return null;
         }
         using (FileStream fileStream = File.OpenRead(Path))
         using (BinaryReader binaryReader = new BinaryReader(fileStream))
         {
            if (Count == 0) Count = (int)binaryReader.BaseStream.Length / 4;
            BytesRead = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
         }

         if ( (( Addr + Count ) * 4 ) > (int)BytesRead.Length )
         {
            MessageBox.Show(text: "Указанный диапазон чтения\n вне прочитаных данных! ",
                               caption: "Ошибка",
                               buttons: MessageBoxButtons.OK,
                               icon: MessageBoxIcon.Error);
            return null;
         }

         Dictionary<int, float> FloatsResult = new Dictionary<int, float>();
         Addr = Addr * 4;
         int m = 0;
         try
         {
            for (int i = Addr; i < Addr + Count * 4; i = i + 4)
            {
               byte[] BytesTemp = new byte[4];
               Array.Copy(BytesRead, i, BytesTemp, 0, 4);
               if ( NotZero )
               {
                  if (BytesTemp[0] == 0 && BytesTemp[1] == 0 && BytesTemp[2] == 0 && BytesTemp[3] == 0) continue;
               }
               byte[] BytesSwap = WorkMisc.Misc.ByteSwap(BytesTemp, ByteOrder);
               RawData.Add(BytesSwap);
               FloatsResult[i] = BitConverter.ToSingle(BytesSwap, 0);
               m++;
            }
         }
         catch(Exception ex)
         {
            MessageBox.Show(text: ex.Message + "\n" + ex.StackTrace,
                               caption: "Ошибка",
                               buttons: MessageBoxButtons.OK,
                               icon: MessageBoxIcon.Error);
         }
         return FloatsResult;
      }
      /// <summary>
      /// Сохраняет в Hex-файл данные из DataTable
      /// </summary>
      /// <param name="PathHexFile"></param>
      /// <param name="table"></param>
      /// <param name="NameAndTypeColumns"></param>
      /// <param name="AddrHex"></param>
      /// <param name="ByteOrder"></param>
      /// <param name="NewFile"></param>
      /// <returns></returns>
      public bool SetHexFileFromDataTable(string PathHexFile,
                                        DataTable table,
                                        List<string> NameColumns = null,
                                        int AddrHex = 0,
                                        int SizeBlokFloat = 32,
                                        string ByteOrder = "0123",
                                        bool NewFile = true)
      {
         int BitCount = 0;
         int CurrBlokFloat = SizeBlokFloat;
         List<byte> ResultBytes = new List<byte>();
         List<string[]> NameAndTypeColumns = new List<string[]>();
         List<string[]> NameAndTypeColumnsBool = new List<string[]>();
         List<string[]> NameAndTypeColumnsFloat = new List<string[]>();

         if (NameColumns == null || NameColumns.Count == 0)
         {
            foreach(DataColumn col in table.Columns){
               if (col.Namespace.ToLower().Contains("bool"))
                  NameAndTypeColumnsBool.Add(new string[] { col.ColumnName, col.Namespace });
               else if (col.Namespace != "" && col.Namespace != table.Namespace )
                  NameAndTypeColumnsFloat.Add(new string[] { col.ColumnName, col.Namespace });
            }
         }
         else{
            foreach(string NameColumn in NameColumns){
               if (table.Columns[NameColumn].Namespace.ToLower().Contains("bool"))
                  NameAndTypeColumnsBool.Add(new string[] { table.Columns[NameColumn].ColumnName, table.Columns[NameColumn].Namespace });
               else if (table.Columns[NameColumn].Namespace != "" && 
                        table.Columns[NameColumn].Namespace != table.Namespace )
                  NameAndTypeColumnsFloat.Add(new string[] { table.Columns[NameColumn].ColumnName, table.Columns[NameColumn].Namespace });
            }
         }
         Str str = new Str();
         NameAndTypeColumnsBool.Sort(str);
         NameAndTypeColumnsFloat.Sort(str);
         BitCount = NameAndTypeColumnsBool.Count;
         NameAndTypeColumns.AddRange(NameAndTypeColumnsFloat);
         NameAndTypeColumns.AddRange(NameAndTypeColumnsBool);

         foreach (DataRow row in table.Rows)
         {
            int BitsTemp = 0;
            int BitCur = 0;
            byte[] BytesTemp;
            Misc rBit = new Misc();

            foreach (string[] NameAndTypeColumn in NameAndTypeColumns)
            {
               switch (NameAndTypeColumn[1].ToLower())
               {
                  case "bool":
                     bool valB = bool.Parse(row[NameAndTypeColumn[0]].ToString());
                     BitsTemp = Misc.SetBit(BitsTemp, BitCur, valB);
                     if (BitCur >= 31 || BitCount <= 1)
                     {
                        BytesTemp = BitConverter.GetBytes(BitsTemp);
                        ResultBytes.AddRange(BytesTemp);
                        BitsTemp = BitCur =0;
                        BitCount = NameAndTypeColumnsBool.Count;
                        CurrBlokFloat--;
                        break;
                     }
                     else{ BitCur++; }
                     BitCount--;
                     break;
                  case "byte": 
                     ResultBytes.Add(byte.Parse(row[NameAndTypeColumn[0]].ToString()));
                     CurrBlokFloat--;
                     break;
                  case "int16":
                     BytesTemp = BitConverter.GetBytes(short.Parse(row[NameAndTypeColumn[0]].ToString()));
                     ResultBytes.AddRange(BytesTemp);
                     CurrBlokFloat--;
                     break;
                  case "int":
                  case "int32":
                     BytesTemp = BitConverter.GetBytes(int.Parse(row[NameAndTypeColumn[0]].ToString()));
                     ResultBytes.AddRange(BytesTemp);
                     CurrBlokFloat--;
                     break;
                  case "float":
                  case "real":
                     BytesTemp = BitConverter.GetBytes(float.Parse(row[NameAndTypeColumn[0]].ToString().Replace(".",",")));
                     ResultBytes.AddRange(BytesTemp);
                     CurrBlokFloat--;
                     break;
               }
            }
            byte[] bytesTemp = new byte[4];
            for (;;)
            {
               if (CurrBlokFloat > 0)
               {
                  ResultBytes.AddRange(bytesTemp);
                  CurrBlokFloat--;
               }
               else { CurrBlokFloat = SizeBlokFloat; break; }
            }
         }
         return SetBytesInHexFile(PathHexFile, ResultBytes.ToArray(), Addr: 0, NewFile: true);
      }

      public bool SetDataTableFromHexFile(string PathHexFile,
                                        DataTable table,
                                        List<string> NameColumns = null,
                                        int AddrHex = 0,
                                        int SizeBlokFloat = 32,
                                        string ByteOrder = "0123",
                                        bool NewFile = true)
      {
         int BitCount = 0;
         int CurrBlokFloat = SizeBlokFloat;
         List<byte> ResultBytes = new List<byte>();
         List<string[]> NameAndTypeColumns = new List<string[]>();
         List<string[]> NameAndTypeColumnsBool = new List<string[]>();
         List<string[]> NameAndTypeColumnsFloat = new List<string[]>();

         if (NameColumns == null || NameColumns.Count == 0)
         {
            foreach (DataColumn col in table.Columns)
            {
               if (col.Namespace.ToLower().Contains("bool"))
                  NameAndTypeColumnsBool.Add(new string[] { col.ColumnName, col.Namespace });
               else if (col.Namespace != "" && col.Namespace != table.Namespace)
                  NameAndTypeColumnsFloat.Add(new string[] { col.ColumnName, col.Namespace });
            }
         }
         else
         {
            foreach (string NameColumn in NameColumns)
            {
               if (table.Columns[NameColumn].Namespace.ToLower().Contains("bool"))
                  NameAndTypeColumnsBool.Add(new string[] { table.Columns[NameColumn].ColumnName, table.Columns[NameColumn].Namespace });
               else if (table.Columns[NameColumn].Namespace != "" &&
                        table.Columns[NameColumn].Namespace != table.Namespace)
                  NameAndTypeColumnsFloat.Add(new string[] { table.Columns[NameColumn].ColumnName, table.Columns[NameColumn].Namespace });
            }
         }
         Str str = new Str();
         NameAndTypeColumnsBool.Sort(str);
         NameAndTypeColumnsFloat.Sort(str);
         BitCount = NameAndTypeColumnsBool.Count;
         NameAndTypeColumns.AddRange(NameAndTypeColumnsFloat);
         NameAndTypeColumns.AddRange(NameAndTypeColumnsBool);

         foreach (DataRow row in table.Rows)
         {
            int BitsTemp = 0;
            int BitCur = 0;
            byte[] BytesTemp;
            Misc rBit = new Misc();

            foreach (string[] NameAndTypeColumn in NameAndTypeColumns)
            {
               switch (NameAndTypeColumn[1].ToLower())
               {
                  case "bool":
                     bool valB = bool.Parse(row[NameAndTypeColumn[0]].ToString());
                     BitsTemp = Misc.SetBit(BitsTemp, BitCur, valB);
                     if (BitCur >= 31 || BitCount <= 1)
                     {
                        BytesTemp = BitConverter.GetBytes(BitsTemp);
                        ResultBytes.AddRange(BytesTemp);
                        BitsTemp = BitCur = 0;
                        BitCount = NameAndTypeColumnsBool.Count;
                        CurrBlokFloat--;
                        break;
                     }
                     else { BitCur++; }
                     BitCount--;
                     break;
                  case "byte":
                     ResultBytes.Add(byte.Parse(row[NameAndTypeColumn[0]].ToString()));
                     CurrBlokFloat--;
                     break;
                  case "int16":
                     BytesTemp = BitConverter.GetBytes(short.Parse(row[NameAndTypeColumn[0]].ToString()));
                     ResultBytes.AddRange(BytesTemp);
                     CurrBlokFloat--;
                     break;
                  case "int":
                  case "int32":
                     BytesTemp = BitConverter.GetBytes(int.Parse(row[NameAndTypeColumn[0]].ToString()));
                     ResultBytes.AddRange(BytesTemp);
                     CurrBlokFloat--;
                     break;
                  case "float":
                  case "real":
                     BytesTemp = BitConverter.GetBytes(float.Parse(row[NameAndTypeColumn[0]].ToString().Replace(".", ",")));
                     ResultBytes.AddRange(BytesTemp);
                     CurrBlokFloat--;
                     break;
               }
            }
            byte[] bytesTemp = new byte[4];
            for (; ; )
            {
               if (CurrBlokFloat > 0)
               {
                  ResultBytes.AddRange(bytesTemp);
                  CurrBlokFloat--;
               }
               else { CurrBlokFloat = SizeBlokFloat; break; }
            }
         }
         return SetBytesInHexFile(PathHexFile, ResultBytes.ToArray(), Addr: 0, NewFile: true);
      }
      public bool SetFloatInHexFile(string Path,
                                    float[] Data,
                                    int Addr = 0,
                                    string ByteOrder = "0123",
                                    bool NewFile = true)
      {
         try
         {
            foreach (float data in Data)
            {
               byte[] TempButes = WorkMisc.Misc.ByteSwap(BitConverter.GetBytes(data), ByteOrder);
               SetBytesInHexFile(Path, TempButes, Addr*4, NewFile);
               Addr++;
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(text: ex.Message + "\n" + ex.StackTrace,
                                 caption: "Ошибка",
                                 buttons: MessageBoxButtons.OK,
                                 icon: MessageBoxIcon.Error);
            return false;
         }
         return true;
      }

      public bool SetBytesInHexFile(string Path,
                                    byte[] Data,
                                    int Addr = 0,
                                    bool NewFile = true)
      {
         if (!NewFile && !File.Exists(Path))
         {
            MessageBox.Show(text: "Hex-файла нет!",
                               caption: "Ошибка",
                               buttons: MessageBoxButtons.OK,
                               icon: MessageBoxIcon.Error);
            return false;
         }

         using (FileStream fileStream = File.OpenWrite(Path))
            try
            {
               using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
               {
                  binaryWriter.Seek((int)Addr, SeekOrigin.Begin);

                  binaryWriter.Write(Data);

               }
            }
            catch (Exception ex)
            {
               MessageBox.Show(text: ex.Message + "\n" + ex.StackTrace,
                                  caption: "Ошибка",
                                  buttons: MessageBoxButtons.OK,
                                  icon: MessageBoxIcon.Error);
               return false;
            }


         return true;
      }

      public byte[] GetBytesFromHexFile(string Path,
                                         int Addr = 0,
                                         int Count = 0)
      {
         byte[] BytesRead = null;
         if (!File.Exists(Path))
         {
            MessageBox.Show(text: "Hex-файла нет!",
                               caption: "Предупреждение",
                               buttons: MessageBoxButtons.OK,
                               icon: MessageBoxIcon.Information);
            return null;
         }
         using (FileStream fileStream = File.OpenRead(Path))
         using (BinaryReader binaryReader = new BinaryReader(fileStream))
         {
            if (Count == 0) Count = (int)binaryReader.BaseStream.Length;
            BytesRead = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
         }

         if ((Addr + Count) > (int)BytesRead.Length)
         {
            MessageBox.Show(text: "Указанный диапазон чтения\n вне прочитаных данных! ",
                               caption: "Ошибка",
                               buttons: MessageBoxButtons.OK,
                               icon: MessageBoxIcon.Error);
            return null;
         }
         byte[] BytesResult = new byte[Count];
         try
         {
            Array.Copy(BytesRead, Addr, BytesResult, 0, Count);
         }
         catch (Exception ex)
         {
            MessageBox.Show(text: ex.Message + "\n" + ex.StackTrace,
                               caption: "Ошибка",
                               buttons: MessageBoxButtons.OK,
                               icon: MessageBoxIcon.Error);
         }
         return BytesResult;
      }

      public int SetDataCsv(string Path, List<string[]> Data)
      {
         int result = 0;
         try
         {
            using (var sw = new StreamWriter(Path, false, Encoding.Default))
            {
               foreach (string[] data in Data)
               {
                  string row = "";
                  foreach (string dat in data)
                  {
                     row = row + dat + @";";
                  }
                  row.TrimEnd(new char[] { ';' });
                  sw.WriteLine(row);
                  result++;
               }
            }
         }
         catch { result = -1; }
         return result;
      }

      public bool AppendDataCsv(string Path, string[] Data)
      {
         try
         {
            using (FileStream fs = new FileStream(Path, FileMode.Append, FileAccess.Write))
            {
               byte[] buff = Encoding.Default.GetBytes(String.Concat(Data));
               fs.Write(buff, 0, Data.Length);
               fs.Flush();
               fs.Close();
            }
         }
         catch { return false; }
         return true;
      }
   }
}
