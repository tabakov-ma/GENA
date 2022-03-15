using System.Collections;

namespace WorkSQL
{
   /// <summary>
   /// Объект содержащий настройки из строки подключения к SQL Server
   /// </summary>
   public class Entity
   {
      public delegate void change();
      public event change Change;
      public Entity(string strCon)
      {
         Fill(strCon);
      }
      public Entity()
      {
      }
      public Entity(string server, string db, string InterSecur = "true", string user = "", string pass = "", string timeout = "5" )
      {
         data_Source = server;
         initial_Catalog = db;
         connect_Timeout = timeout;
         integrated_Security = InterSecur;
         password = pass;
         user_ID = user;
         Change?.Invoke();
      }
      string connect_Timeout ="";
      string data_Source = "";
      string initial_Catalog = "";
      string integrated_Security = "";
      string password = "";
      string user_ID = "";

      public int State = 0;
      
      /// <summary>
      /// Время подключения
      /// </summary>
      public string Connect_Timeout 
      { 
         get 
         { 
            return connect_Timeout; 
         } 
         set 
         {
            int num;
            if (value != connect_Timeout && int.TryParse(value, out num)) 
            { 
               connect_Timeout = value; Change?.Invoke(); 
            } 
         } 
      }
      /// <summary>
      /// Сервер SQL
      /// </summary>
      public string Data_Source
      {
         get
         {
            return data_Source;
         }
         set
         {
            if (value != data_Source)
            {
               data_Source = value; Change?.Invoke();
            }
         }
      }
      
      /// <summary>
      /// База данных
      /// </summary>
      public string Initial_Catalog
      {
         get
         {
            return initial_Catalog;
         }
         set
         {
            if (value != initial_Catalog)
            {
               initial_Catalog = value; Change?.Invoke();
            }
         }
      }
      public bool Integrated_Security
      {
         get
         {
            return integrated_Security.ToLower()=="true" ? true : false;
         }
         set
         {
            if (value != (integrated_Security.ToLower() == "true" ? true : false))
            {
               integrated_Security = value ? "true" : "false"; 
               Change?.Invoke();
            }
         }
      }
      public string Password
      {
         get
         {
            return "***";
         }
         set
         {
            if (value != password)
            {
               password = value; Change?.Invoke();
            }
         }
      }
      public string User_ID
      {
         get
         {
            return user_ID;
         }
         set
         {
            if (value != user_ID)
            {
               user_ID = value; Change?.Invoke();
            }
         }
      }

      public void Fill (string str)
      {
         if (str == null || str == "") return;
         try
         {
            // Разделение по ;
            string[] strArr1 = str.Split(new char[] { ';' });
            string[,] strArr3 = new string[strArr1.Length - 1, 2];
            // Разделяем  по =
            for (int i=0;i<strArr1.Length-1;i++) // -1 запятая в конце
            {
               string[] strArr2 = strArr1[i].Split(new char[] { '=' });
               strArr3[i, 0] = strArr2[0];
               strArr3[i, 1] = strArr2[1];
            }
            BitArray bitArray = new BitArray(32);
            // Удаление пробелов в начале и в конце строки
            for(int i=0;i<strArr3.Length/2;i++)
            {
               strArr3[i, 0] = strArr3[i, 0].Trim();
               strArr3[i, 1] = strArr3[i, 1].Trim();
               // Наполнение сущности
               switch (strArr3[i, 0])
               {
                  case "Connect Timeout":
                     connect_Timeout = strArr3[i, 1]; bitArray.Set(0,true);
                     break;
                  case "Data Source":
                  case "Server":
                     data_Source = strArr3[i, 1]; bitArray.Set(1, true);
                     break;
                  case "Initial Catalog":
                  case "Database":
                     initial_Catalog = strArr3[i, 1]; bitArray.Set(2, true);
                     break;
                  case "Integrated Security":
                  case "Trusted_Connection":
                     integrated_Security = strArr3[i, 1]; bitArray.Set(3, true);
                     break;
                  case "Password":
                     password = strArr3[i, 1]; bitArray.Set(4, true);
                     break;
                  case "User ID":
                     user_ID = strArr3[i, 1]; bitArray.Set(5, true);
                     break;
               }
            }
            int[] array = new int[1];
            bitArray.CopyTo(array, 0);
            State = array[0];
            Change?.Invoke();
         }
         catch { State=-1; }
            
      }

      /// <summary>
      /// Возвращает строку SQL подключения
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return ResultStrCon();
      }
      public string ToStringHidePass()
      {
         return ResultStrCon("***");
      }
      private string ResultStrCon(string PassHide = "")
      {
         string pass = PassHide == "" ? password.ToString() : PassHide;

         string StrConnect = "Server = " + data_Source.ToString() + "; Database = " + initial_Catalog.ToString() + "; Connect Timeout = " + connect_Timeout.ToString() + ";";
   
         if  (Integrated_Security) 
            StrConnect = StrConnect + " Trusted_Connection = True;";
         else
            StrConnect = StrConnect + " Trusted_Connection = False; User ID = " + user_ID.ToString() + "; Password = " + pass + ";";
         return StrConnect;
      }
   }
}
