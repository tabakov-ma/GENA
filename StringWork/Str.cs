using System.Collections.Generic;
namespace StringWork
{
   public class Str
   {
      public string[,] StrSqlConToArr(string str)
      {
      
         string[] sep1 = str.Split(new char[] { ';'});
         string[,] strResult = new string[sep1.Length-1 , 2];
         for (int i = 0; i < sep1.Length - 1; i++)
         {
            string[] sep2 = sep1[i].Split(new char[] { '=' });
            strResult[i, 0] = sep2[0]; strResult[i, 1] = sep2[1];
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
      public static void CopyArrStr(string[] source, string[] dest, int start = 0, bool fillEmpty = false, string fillText = "")
      {
         int destLen = dest.Length;
         int sourLen = source.Length;
         for(int i = start; i < destLen; i++)
         {
            if(i > sourLen - 1)
            {
               if (fillEmpty) dest[i] = fillText;
            }
            else  dest[i] = source[i];
         }
      }
      /// <summary>
      /// Делегат
      /// </summary>
      /// <param name="str1"> Первая строка </param>
      /// <param name="str2"> Вторая страка </param>
      /// <returns></returns>
      delegate bool Compare(string str1, string str2);
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

         Compare compare;
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
      
      
      public string[] Sort(string[] source)
      {
         return new string[3];
      }

      public int[] Sort(int[] source)
      {
         return new int[3];
      }

   }
}
