using System;

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
         return ( val & ( 1<<num ) ) > 0;
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
   }
}
