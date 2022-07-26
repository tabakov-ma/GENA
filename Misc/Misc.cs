using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkMisc
{
    public class Misc
    {
      /// <summary>
      /// Получить значение указанного (num) номера бита в числе (val)
      /// </summary>
      /// <param name="val"></param>
      /// <param name="num"></param>
      /// <returns></returns>
      public static bool GetBit(int val, int num = 0)
      {
         return (val & (1 << num)) != 0;
      }

      /// <summary>
      /// Получает ширину монитора, где форма отображается
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      public static int GetScreenWidth(Control obj)
      {
         Screen screen = Screen.FromControl(obj);
         return screen.Bounds.Size.Width;
      }

      public static bool GetBit(byte val, int num = 0)
      {
         return (val & (1 << num)) != 0;
      }

      /// <summary>
      /// Установить значение указанного (num) номера бита в числе (val)
      /// </summary>
      /// <param name="val"></param>
      /// <param name="num"></param>
      /// <param name="set_val"></param>
      /// <returns></returns>
      public static int SetBit(int val, int num = 0, bool set_val = false)
      {
         if (set_val)
         {
            return val | (1 << num);
         }
         else
         {
            return val & ~(1 << num);
         }
      }

      public static byte SetBit(byte val, int num = 0, bool set_val = false)
      {
         if (set_val)
         {
            return (byte)(val | (1 << num));
         }
         else
         {
            return (byte)(val & ~(1 << num));
         }
      }

      public static int SetBit(int val, string num = "bool0", bool set_val = false)
      {
         try
         {
            int numBit = int.Parse(num.ToLower().Replace("bool", ""));
            if (set_val)
            {
               return val | (1 << numBit);
            }
            else
            {
               return val & ~(1 << numBit);
            }
         }
         catch{ return val; }
      }

      int _RememberBit = 0;
      /// <summary>
      /// Запоминатель новых битов, до первого повторного бита
      /// </summary>
      /// <param name="val"></param>
      /// <param name="NewBit"></param>
      /// <param name="num"></param>
      /// <param name="set_val"></param>
      /// <returns></returns>
      public int RememberBit(int val, out bool NewBit,  
                             string num = "bool0", bool set_val = false)
      {
         NewBit = false;
         try
         {
            int numBit = int.Parse(num.ToLower().Replace("bool", ""));
            if ( (_RememberBit & (1 << numBit)) > 0 )
            {
               _RememberBit = val =0;
            }
            else
            {
               NewBit = true;
            }
            _RememberBit = _RememberBit | (1 << numBit);
            return SetBit(val, numBit, set_val);
         }
         catch { _RememberBit = 0;  return val; }
      }

      /// <summary>
      /// Получить данные из внутреннего словаря для преобразования текстовых строк
      /// </summary>
      /// <param name="val"></param>
      /// <param name="ToUpper"></param>
      /// <param name="ToLower"></param>
      /// <param name="IgnorReg"></param>
      /// <returns></returns>
      public static string GetDataFromDict(string val, bool ToUpper = false, bool ToLower = false, bool IgnorReg = false)
      {
         string result = IgnorReg ? val.ToLower() : val;

         switch ( result )
         {
            case "desc": result = "description"; break;

            default: result = val; break;
         }

         return ToUpper ? result.ToUpper() : ToLower ? result.ToLower() : result;
      }

      /// <summary>
      /// Перестановка байт в массиве байт по указанному порядку
      /// </summary>
      /// <param name="InputBytes"></param>
      /// <param name="BytesOrder"></param>
      /// <returns></returns>
      public static byte[] ByteSwap(byte[] InputBytes, string BytesOrder = "0123")
      {
         if (BytesOrder == "0123") return InputBytes;
         byte[] ResBytes = new byte[BytesOrder.Length];
         try
         {
            for (int i = 0; i < BytesOrder.Length; i++)
            {
               ResBytes[i] = InputBytes[int.Parse(BytesOrder[i].ToString())];
            }
         }
         catch(Exception ex)
         {
            MessageBox.Show(text: ex.Message + "\n" + ex.StackTrace,
                            caption: "Ошибка",
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Error);
         }
         return ResBytes;
      }

      /// <summary>
      /// Преобразовывает строку числа HEX или DEC в int
      /// </summary>
      /// <param name="val"></param>
      /// <param name="hex"></param>
      /// <returns></returns>
      public static int? HexConverterStrToInt(string val, bool hex = true, bool errNull = true)
      {
         int? result = 0;
         if (errNull) result = null;
         if (val!="")
         {
            try
            {
               result = hex 
                            ? int.Parse(val, System.Globalization.NumberStyles.HexNumber) 
                            : int.Parse(val.ToString());
            }
            catch{}
         }
         return result;
      }

      public static byte? HexConverterStrToByte(string val, bool hex = true, bool errNull = true)
      {
         byte? result = 0;
         if (errNull) result = null;
         if (val != "")
         {
            try
            {
               result = hex
                            ? byte.Parse(val, System.Globalization.NumberStyles.HexNumber)
                            : byte.Parse(val.ToString());
            }
            catch { }
         }
         return result;
      }

      public static string HexConverterStrToStr(string val, bool hex = true)
      {
         string result = "";
         if (val != "")
         {
            try
            {
               result = hex
                            ? (int.Parse(val)).ToString("X") 
                            : val;
            }
            catch { }
         }
         return result.Length == 1 & hex ? "0"+ result : result;
      }

      public static string HexConverterByteToStr(byte val, bool hex = true)
      {
         string result = "";
         try
         {
            result = hex
                           ? val.ToString("X")
                           : val.ToString();
         }
         catch { }
         return result.Length == 1 & hex ? "0" + result : result;
      }

      public static string ReverseString(string s)
      {
         char[] arr = s.ToCharArray();
         Array.Reverse(arr);
         return new string(arr);
      }

      public static string GetBitsState(float valF)
      {
         string result = "";
         byte[] valB = BitConverter.GetBytes(valF);
         foreach (byte val in valB)
         {
            for(int i=0;i<8;i++)
            {
               result = (GetBit(val,i)?"1":"0") +result;
            }
            result = "-" + result;
         }
         return result.Substring(1);
      }

      public static string GetBitsState(int valI)
      {
         string result = "";
         byte[] valB = BitConverter.GetBytes(valI);
         foreach (byte val in valB)
         {
            for (int i = 0; i < 8; i++)
            {
               result = (GetBit(val, i) ? "1" : "0") + result;
            }
            result = "-" + result;
         }
         return result.Substring(1);
      }

      public static string[] GetUnique(string[] arrStr)
      {
         Dictionary<string, string> dicStr = new Dictionary<string, string>();
         foreach (string data in arrStr)
         {
            try
            {
               dicStr.Add(data, "");
            }
            catch { }
         }
         return dicStr.Keys.ToArray();
      }

      public static string ArrIntToString(int[] arrInt)
      {
         string result = "";
         foreach(int Int in arrInt)
            result = result + Int.ToString() + ",";
         return result==""?"X":result;
      }

   }
}
