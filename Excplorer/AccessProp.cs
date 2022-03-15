using System;
using System.Windows.Forms;

namespace Explorer
{
   class AccessProp
   {
      public static string GetPropStr(string name)
      {
         try
         {
            return Properties.Settings.Default[name].ToString();
         }
         catch (Exception ex) { MessageBox.Show(ex.Message); return ""; }
      }

      public static int GetPropInt(string name)
      {
         try
         {
            return int.Parse(Properties.Settings.Default[name].ToString());
         }
         catch (Exception ex) { MessageBox.Show(ex.Message); return 0; }
      }
      public static string SetProp(string name, string val)
      {
         try
         {
            Properties.Settings.Default[name] = val;
            Properties.Settings.Default.Save();
            return "";
         }
         catch (Exception ex) { return ex.Message; }
      }

      public static string SetProp(string name, int val)
      {
         try
         {
            Properties.Settings.Default[name] = val;
            Properties.Settings.Default.Save();
            return "";
         }
         catch (Exception ex) { return ex.Message; }
      }
   }
}
