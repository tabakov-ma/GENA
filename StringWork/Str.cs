using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace WorkString
{
   public class Str : IComparer<string[]>
   {
      public int Compare(string[] o1, string[] o2)
      {
         return string.Compare(o1[0], o2[0]);
      }
      /// <summary>
      /// Строка базовых типов 
      /// </summary>
      public const string BaseTypeNames = "|Bool|Byte|Char|Double|Decimal|DInt|DWord|Float|Int|Long|Real|SByte|Short|String|UInt|ULong|UShort|Word|";
      /// <summary>
      /// Список объектов
      /// </summary>
      public static List<string[]> ObjectsNames = new List<string[]>();
      /// <summary>
      /// Преобразует строку подключения к SQL в двумерный массив из ключей и данных
      /// </summary>
      /// <param name="str"> Строка подключения к SQL </param>
      /// <returns></returns>
      public string[,] StrSqlConToArr(string str)
      {
         // Разделение по параметрам
         string[] sep1 = str.Split(new char[] { ';'});
         // Определение размеров результирующего размера
         string[,] strResult = new string[sep1.Length-1 , 2];
         // Проход по массиву параметров
         for (int i = 0; i < sep1.Length - 1; i++)
         {
            string[] sep2 = sep1[i].Split(new char[] { '=' }); // Разделение параметров на ключи и данные
            strResult[i, 0] = sep2[0].Trim(); // Ключ параметра
            strResult[i, 1] = sep2[1].Trim(); // Данные параметра
         }
         return strResult;
      }

      /// <summary>
      /// Добавить в конце строки строку, если этой строки нет в конце
      /// </summary>
      /// <param name="strDest"></param>
      /// <param name="strAdd"></param>
      /// <returns></returns>
      public static string AddStrEnd(string strDest, string strAdd)
      {
         int lenStrDest = strDest.Length;
         int lenStrAdd = strAdd.Length;
         int indexAdd = strDest.IndexOf(strAdd);
         if(indexAdd + lenStrAdd == lenStrDest) return strDest;
         return strDest + strAdd;
      }

      /// <summary>
      /// Копирование массива строк из одного массива в другой
      /// </summary>
      /// <param name="source"> Массив источник </param>
      /// <param name="dest"> Целевой массив </param>
      /// <param name="start"> Начальный элемент копирования и вставки </param>
      /// <param name="fillEmpty"> Включение заполнения пустых </param>
      /// <param name="fillText"> Текст для заполнения пустых </param>
      public static string[] CopyArrStr(string[] source, string[] dest=null, int start = 0, bool fillEmpty = false, string fillText = "", bool NotNull=false)
      {
         List<string> result = new List<string>();
         int destLen = dest!=null ? dest.Length : source.Length;
         int sourLen = source.Length;
         for(int i = start; i < destLen; i++)
         {
            if (i > sourLen - 1)
            {
               if (fillEmpty)
               {
                  if(dest != null) dest[i] = fillText;
                  result.Add(fillText);
               }
            }
            else
            {
               if (NotNull)
               {
                  if (dest != null) dest[i] = source[i];
                  if (source[i]!=null) result.Add(source[i]);
               }
               else
               {
                  if(source[i]!=null)
                     result.Add(source[i]);
               }
            }
         }
         return result.ToArray();
      }

      /// <summary>
      /// Копирование словаря [string, sttring]
      /// </summary>
      /// <param name="source"></param>
      /// <param name="dest"></param>
      /// <returns></returns>
      public static Dictionary<string, string> CopyDict2Str(Dictionary<string, string> source)
      {
         Dictionary<string, string> result = new Dictionary<string, string>();
         foreach(var entry in source)
         {
            result.Add(key: entry.Key, value: entry.Value);
         }
         return result;
      }

      public static void CopyLstStr(List<string> source, List<string> dest, int start = 0, bool fillEmpty = false, string fillText = "")
      {
         int destLen = dest.Capacity;
         int sourLen = source.Count;
         for (int i = start; i < destLen; i++)
         {
            if (i > sourLen - 1)
            {
               if (fillEmpty) dest.Add(fillText);
            }
            else dest.Add(source[i]);
         }
      }

      /// <summary>
      /// Ищет соответствие списков.
      /// lstEqual - найденные соответствия;
      /// lst1_rem - остаток от списка lst1, которых нет в lst2;
      /// lst2_rem - остаток от списка lst2, которых нет в lst1;
      /// </summary>
      /// <param name="lst1"></param>
      /// <param name="lst2"></param>
      /// <param name="lstEqual"></param>
      /// <param name="lst1_rem"></param>
      /// <param name="lst2_rem"></param>
      /// <returns></returns>
      public static bool Find(List<string> lst1, List<string> lst2, 
                              out List<string> lstEqual, 
                              out List<string> lst1_rem, out List<string> lst2_rem )
      {
         lstEqual = new List<string>();
         lst1_rem = new List<string>();
         lst2_rem = new List<string>();
         lst1_rem.AddRange(lst1.ToArray());
         lst2_rem.AddRange(lst2.ToArray());
         string str_lst1 = "|" + string.Join("|", lst1.ToArray()) + "|";
         foreach(string str in lst2){
            if (str_lst1.Contains("|"+str+"|"))
            {
               lstEqual.Add(str);
               lst1_rem.Remove(str);
               lst2_rem.Remove(str);
            }
         }
         return lstEqual.Count>0;
      }

      /// <summary>
      /// Выполняет поиск значения 'FindVal' в списке массивов строк
      /// 'index:' -1 поиск во всём массиве, >= 0 в конкретном индексе
      /// </summary>
      /// <param name="sourse"></param>
      /// <param name="FindVal"></param>
      /// <param name="FindRow"></param>
      /// <param name="index"></param>
      /// <returns></returns>
      public static bool Find(List<string[]> sourse, string FindVal, out string[] FindRow, int index = -1 )
      {
         FindRow = null;
         foreach(string[] row in sourse)
         {
            if(index >= 0)
            {
               try
               {
                  if (row[index] == FindVal) 
                  {
                     FindRow = new string[row.Length];
                     row.CopyTo(FindRow,0);
                     return true; }
               }
               catch{ MessageBox.Show(text: "Переданный индекс 'index:' " + index + " больше длинны массива: " + row.Length,
                                        caption: "Ошибка",
                                        icon: MessageBoxIcon.Error,
                                        buttons: MessageBoxButtons.OK); }
            }
            else{
               foreach(string cell in row)
               {
                  if (cell == FindVal)
                  {
                     FindRow = new string[row.Length];
                     row.CopyTo(FindRow, 0);
                     return true;
                  }
               }
            }
         }
         return false;
      }

      /// <summary>
      /// Делегат
      /// </summary>
      /// <param name="str1"> Первая строка </param>
      /// <param name="str2"> Вторая страка </param>
      /// <returns></returns>
      delegate bool CompareStr(string str1, string str2);
      delegate bool CompareInt(int int1, int sint2);
      /// <summary>
      /// Сортировка списка строк
      /// </summary>
      /// <param name="source"> Исходный список строк </param>
      /// <param name="type"> true/false - по возростанию/убываню </param>
      /// <returns></returns>
      public static List<string> Sort(List<string> source, bool type = true)
      {
         string tempStr="";
         string[] result = new string[source.Count];
         source.CopyTo(result);

         CompareStr compare;
         if (type) 
            compare = (str1, str2) => string.Compare(str1, str2) > 0; 
         else 
            compare = (str1, str2) => string.Compare(str1, str2) < 0;

         for (int i=0; i< result.Length-1; i++)
         {
            for (int m = 0; m < result.Length-1; m++)
            {
               if(compare(result[m], result[m+1]))
               {
                  tempStr = result[m + 1];
                  result[m + 1] = result[m];
                  result[m] = tempStr;
               }
            }
         }
         return new List<string>(result);
      }

      /// <summary>
      /// Чтение таблицы из троки
      /// </summary>
      /// <param name="table"></param>
      /// <param name="sep_head"></param>
      /// <param name="sep_data"></param>
      /// <returns></returns>
      public static DataTable GetTableFromStr(string data, string sep_head = "=", string sep_data = ";", bool MsgYes=true)
      {
         DataTable result = new DataTable();
         if (data != null && data != "" && data.Contains(sep_head) && data.Contains(sep_data))
         {
            try
            {
               // Заголовок таблицы
               string[] head = data.Split(sep_head.ToCharArray())[0].Split(';');
               foreach (string NameColumn in head)
               {
                  DataColumn column = new DataColumn(NameColumn);
                  result.Columns.Add(column);
               }
               // Данные таблицы
               string[] data_rows = data.Split(sep_head.ToCharArray())[1].Split(';');
               int num = 0;
               DataRow row = null;
               foreach (string data_row in data_rows)
               {
                  if (num == 0) row = result.NewRow();
                  row[num] = data_row; num++;
                  if (num >= row.Table.Columns.Count)
                  {
                     result.Rows.Add(row);
                     num = 0;
                  }
               }
               if (num > 0) result.Rows.Add(row);
            }
            catch (Exception ex)
            {
               if (MsgYes) MessErr(ex);
            }
         }
         else
         {
            if (MsgYes) MessErr(error: "Входная строка не корректна: '" + data +"'");
         }
         return result;
      }
      public static string GetTimeNow(string format = "hh:mm:ss:fff")
      {
         return DateTime.Now.ToString(format);
      }

      /// <summary>
      /// Сохранение таблицы в троку
      /// </summary>
      /// <param name="table"></param>
      /// <param name="sep_head"></param>
      /// <param name="sep_data"></param>
      /// <returns></returns>
      public static string SetStrFromTable(DataTable table, string sep_head = "=", string sep_data = ";")
      {
         string result = null;
         try
         {
            // Заголовок таблицы
            foreach (DataColumn column in table.Columns){
            result = result + column.ColumnName + sep_data;
         } 
         result = result.TrimEnd(sep_data.ToCharArray()) + sep_head;
         // Данные таблицы
         
            foreach (DataRow row in table.Rows)
            {
               for (int i = 0; i < row.Table.Columns.Count; i++)
               {
                  result = result + row[i] + sep_data;
               }
            }
            result = result.TrimEnd(sep_data.ToCharArray());
         }
         catch (Exception ex)
         {
            MessErr(ex);
         }
         return result;
      }

      /// <summary>
      /// Сообщение об ошибки
      /// </summary>
      /// <param name="ex"></param>
      /// <param name="caption"></param>
      /// <param name="error"></param>
      /// <param name="buttons"></param>
      /// <param name="icon"></param>
      /// <returns></returns>
      private static DialogResult MessErr(
         Exception ex = null, string caption = "Ошибка", string error = "",
         MessageBoxButtons buttons = MessageBoxButtons.OK,
         MessageBoxIcon icon = MessageBoxIcon.Error)
      {
         string exInfo = "";
         if (ex != null)
         {
            exInfo = "Message: " + ex.Message + "\n" +
                     "Source: " + ex.Source + "\n" +
                     "StackTrace: " + ex.StackTrace + "\n" +
                     "TargetSite: " + ex.TargetSite + "\n" +
                     "HelpLink: " + ex.HelpLink;
         }
         error = error != "" ? "Error: " + error + "\n" : error;
         return MessageBox.Show(
            text: error + exInfo,
            caption: caption,
            buttons: buttons,
            icon: icon);
      }

      public static Dictionary<int,string> Sort(Dictionary<int, string> source, bool type = true)
      {
         string tempStr = "";
         int tempInt = 0;
         int[] Keys = new int[source.Count];
         string[] Values = new string[source.Count];
         int n = 0;
         foreach(var pair in source)   { Keys[n] = pair.Key; Values[n] = pair.Value; n++; }
        
         CompareInt compare;
         if (type)
            compare = (int1, int2) => int1 > int2;
         else
            compare = (int1, int2) => int1 < int2;

         for (int i = 0; i < Keys.Length - 1; i++)
         {
            for (int m = 0; m < Keys.Length - 1; m++)
            {
               if (compare(Keys[m], Keys[m + 1]))
               {
                  tempStr = Values[m + 1];
                  Values[m + 1] = Values[m];
                  Values[m] = tempStr;

                  tempInt = Keys[m + 1];
                  Keys[m + 1] = Keys[m];
                  Keys[m] = tempInt;
               }
            }
         }

         Dictionary<int, string> Result = new Dictionary<int, string>(Keys.Length);
         for(int i = 0; i< Keys.Length; i++)
         {
            Result.Add(Keys[i], Values[i]);
         }
         return Result;
      }

      public static List<List<string>> Sort(List<List<string>> source, bool type = true)
      {
         string tempStr = "";
         string[] Col = new string[source.Count];
         string[] Row = new string[source.Count];
         int n = 0;
         for (int i = 0; i < source.Count; i++) { Col[n] = source[i][0]; Row[n] = source[i][1]; n++; }

         CompareInt compare;
         if (type)
            compare = (int1, int2) => int1 > int2;
         else
            compare = (int1, int2) => int1 < int2;

         for (int i = 0; i < Col.Length - 1; i++)
         {
            for (int m = 0; m < Col.Length - 1; m++)
            {
               if (compare(int.Parse(Col[m]), int.Parse(Col[m + 1])))
               {
                  tempStr = Row[m + 1];
                  Row[m + 1] = Row[m];
                  Row[m] = tempStr;

                  tempStr = Col[m + 1];
                  Col[m + 1] = Col[m];
                  Col[m] = tempStr;
               }
            }
         }

         List<List<string>> Result = new List<List<string>>(Col.Length);
         for (int i = 0; i < Col.Length; i++)
         {
            List<string> lst = new List<string>();
            lst.Add(Col[i]);
            lst.Add(Row[i]);
            Result.Add(lst);
         }
         return Result;
      }

      public static List<List<string>> GetDataTypeObjInListList(List<List<string>> DataList, List<string> types, string type )
     {
         List<List<string>> ResultList;
         List<List<string>> IndexTypes = new List<List<string>>(types.Count);
         bool ColumnFind = false;
         try
         {
            // Построение словаря типов и их адресации из DataList
            for (int col = 0; col < DataList.Count; col++)
            {
               foreach (string typ in types)
               {
                  int index = DataList[col].IndexOf(typ);
                  // Найден тип
                  if (index >= 0)
                  {
                     // Справа от найденного типа: цифра (ID объекта) или пусто
                     int index2 = 0;
                     if (int.TryParse(DataList[col + 1][index], out index2) || DataList[col + 1][index] == "")
                     {
                        List<string> lstTemp = new List<string>();
                        lstTemp.Add(index.ToString());
                        lstTemp.Add(typ);
                        IndexTypes.Add(lstTemp);

                        ColumnFind = true; // Найден столбец с данными
                     }
                  }
               }
               if (ColumnFind) break;
            }
            IndexTypes = Sort(IndexTypes);
         
            // Нахождение диапазона данных
            int RowStart = -1;
            int RowEnd = -1;
            int RowCount = -1;
            for (int i = 0; i < IndexTypes.Count; i++)
            {
               if (type == IndexTypes[i][1])
               {
                  RowStart = int.Parse(IndexTypes[i][0]) + 1;
                  if (i != IndexTypes.Count - 1) // Если не последняя запись в словаре
                     RowEnd = int.Parse(IndexTypes[i + 1][0]);
                  else
                     RowEnd = DataList[0].Count;
                  break;
               }
            }

            if (RowStart < 0 || RowEnd < 0) return null;

            // Копирование данных по найденному диапазону
            RowCount = RowEnd - RowStart;
            ResultList = new List<List<string>>(RowCount);
            for (int col = 0; col < DataList.Count; col++)
            {
               string[] arrTemp = new string[RowCount];
               DataList[col].CopyTo(RowStart, arrTemp, 0, RowCount);
               List<string> lst = new List<string>(arrTemp);
               ResultList.Add(lst);
            }
            return ResultList;
         }
         catch (Exception ex)
         { MessageBox.Show(ex.Message); return null; }

         
     }

     /// <summary>
     /// Получить массив строк из словаря <string, string>
     /// </summary>
     /// <param name="Dic"></param>
     /// <param name="key"></param>
     /// <returns></returns>
     public static string[] GetArrStrFromDic(Dictionary<string, string> Dic, bool key = true)
     {
         string[] result = null;
         if (Dic == null || Dic.Count == 0) return result;

         result = new string[Dic.Count];
         int i = 0;
         if (key)
         {
            foreach (string Key in Dic.Keys)
            {
               result[i] = Key;
               i++;
            }
         }
         else
         {
            foreach (string Value in Dic.Values)
            {
               result[i] = Value;
               i++;
            }
         }
         return result;
     }
      /// <summary>
      /// Возвращает значение (int) из строки содержащей значение между разделителями (..[int]..) или второе значение после разделителя [start..int]
      /// </summary>
      /// <param name="val"> Входная строка </param>
      /// <param name="start"> Выходное значение int идущее первым до разделителя [start..int] </param>
      /// <param name="separ"> Разделители, по умолчанию "[", ".." и "]" </param>
      /// <returns></returns>
     public static int GetArraySizeFromStr(string val, out int start, string[] separ = null )
     {
         int num = start = - 1;
         if (separ == null)
            separ = new string[] { "[", "..","]" };
         if (val.IndexOf(separ[0]) >=0 & val.IndexOf(separ[2]) >= 0)
         {
            string[] result = val.Split(separ, StringSplitOptions.None);
            if (result.Length == 3)
            {
               int.TryParse(result[1], out num);
            }
            else if (result.Length == 4)
            {
               int.TryParse(result[1], out start);
               int.TryParse(result[2], out num);
            }
         }
         return num;
     }

      /// <summary>
      /// Возвращает значение (string) из строки содержащей значение между разделителями (..[string]..)
      /// </summary>
      /// <param name="val"> Входная строка </param>
      /// <param name="separ"> Разделители, по умолчанию '[' и ']' </param>
      /// <returns></returns>
      public static string GetStrFromStrBySepar(string val, char[] separ = null)
      {
         string str = "";
         if (separ == null)
            separ = new char[] { '[', ']' };
         if (val.IndexOf(separ[0]) >= 0 & val.IndexOf(separ[1]) >= 0)
         {
            str = val.Split(separ[0])[1].Split(separ[1])[0];
         }
         return str;
      }

      /// <summary>
      /// Возвращает значение (string) из строки без разделителей и что между ними
      /// </summary>
      /// <param name="val"> Входная строка </param>
      /// <param name="separ"> Разделители, по умолчанию '[' и ']' </param>
      /// <returns></returns>
     public static string GetStrWithoutSepar(string val, char[] separ = null)
     {
         string str = val;
         if (separ == null)
            separ = new char[] { '[', ']' };
         if (val.IndexOf(separ[0]) >= 0 & val.IndexOf(separ[1]) >= 0)
         {
            str = val.Split(separ[0])[0] + val.Split(separ[1])[1];
         }
            return str;
     }

    
     /// <summary>
     /// Определение является ли передаваемая строка, строкой говорящей что это объект 
     /// (Возврыщает:
     /// 0 - базовый тип
     /// 1 - графический объект
     /// 2 - контроллерный объект
     /// -1 - ошибка, не загружен списов объектов проекта)
     /// </summary>
     /// <param name="val">Входная строка</param>
     /// <param name="equal">Флаг строгости соответсвия</param>
     /// <returns></returns>
     public static int IsObject(string val)
     {
         int result = 0;

         if(ObjectsNames == null)
         {
            MessageBox.Show(text: "Список объектов проекта не загружен,\n" +
                                  "функция IsObject не может проверить значение " + val,
                                  caption: "Ошибка",
                                  icon: MessageBoxIcon.Error,
                                  buttons: MessageBoxButtons.OK);
            return -1;
         }

         foreach(string[] name in ObjectsNames)
         {
            if (name[0] == val)
            {
               if(name[1] != "" && name[1] != "0")
               {
                  result = 1;
               }
               else
               {
                  result = 2;
               }
                  
            }
         }
         return result;
     }

      /// <summary>
      /// 0 - не является массивом:       (нет элементов [ и ])
      /// 1 - является массивом:          (Name[0..64])
      /// 2 - является элементом массива: (Name[#])
      /// 3 - является призноком массива: (Name[])
      /// </summary>
      /// <param name="val">Входная строка</param>
      /// <returns></returns>
      public static int IsArray(string val) 
     {
         int result = 0;
         // 0 - не является массивом:       (нет элементов [ и ])
         // 1 - является массивом:          (Name[0..64])
         // 2 - является элементом массива: (Name[#])
         // 3 - является призноком массива: (Name[])
         int start = val.IndexOf("["); int end = val.IndexOf("]"); 
         
         if (start >= 0 & end >= 0)
         {
            int count;
            if((end - start) == 1)
            {
               result = 3;
            }
            else if (int.TryParse(val.Substring(start+1, end-start-1), out count))
            {
               result = 2;
            }
            else
            {
               if(val.Substring(start + 1, end - start - 1).IndexOf("..")>=0)
               {
                  result = 1;
               }
            }
         }
         return result;
     }

      public static string GetNameArray(string val)
      {
         string result = "";
         if(val.IndexOf("[")>0)
         {
            result = val.Split(new string[] { "["}, StringSplitOptions.None)[0];
         }
         return result;
      }

      public static bool Compare(string in_str, string[] arr_str, bool register = false)
      {
         bool result = false;
         if (arr_str.Length > 0)
         {
            foreach (string str in arr_str)
            {
               if ((!register && str.ToLower() == in_str.ToLower()) ||
                   ( register && str == in_str))
               {
                  result = true;
                  break;
               }
            }
         }
         return result;
      }

      public static bool Compare(string in_str, List<string> arr_str, bool register = false)
      {
         bool result = false;
         if (arr_str.Count > 0)
         {
            foreach (string str in arr_str)
            {
               if ((!register && str.ToLower() == in_str.ToLower()) ||
                   (register && str == in_str))
               {
                  result = true;
                  break;
               }
            }
         }
         return result;
      }
      
      public static string EmptyDel(string val)
      {
         return new string(val.Where(c => !char.IsControl(c)).ToArray());
      }

   }
}
