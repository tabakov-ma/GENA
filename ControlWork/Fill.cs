using System.Collections.Generic;
using System.Windows.Forms;

namespace ControlWork
{
    public class Fill
    {
      /// <summary>
      /// Наполнение данными из словаря, объекта ComboBox
      /// </summary>
      /// <param name="comboBox"> Объект ComboBox для наполнения </param>
      /// <param name="dic"> Словарь с данными для наполнения </param>
      /// <param name="index"> Индекс для начального отображения в ComboBox </param>
      public static void Control(ref ComboBox comboBox, Dictionary<string, string> dic, int index = 0)
      {
         foreach (string data in dic.Keys)
         {
            comboBox.Items.Add(data);
            comboBox.SelectedIndex = index;
         }
      }
   }
}
