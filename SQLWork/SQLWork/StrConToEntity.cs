using System.Collections;

namespace SQLWork
{
    public class StrConToEntity
    {
        public StrConToEntity(string strCon)
        {
            Fill(strCon);
        }
        public int State = 0;
        public string Connect_Timeout { get; set; }
        public string Data_Source { get; set; }
        public string Initial_Catalog { get; set; }
        public string Integrated_Security { get; set; }
        public string Password { get; set; }
        public string User_ID { get; set; }

        private void Fill (string str)
        {
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
                            Connect_Timeout = strArr3[i, 1]; bitArray.Set(0,true);
                            break;
                        case "Data Source":
                        case "Server":
                            Data_Source = strArr3[i, 1]; bitArray.Set(1, true);
                            break;
                        case "Initial Catalog":
                        case "Database":
                            Initial_Catalog = strArr3[i, 1]; bitArray.Set(2, true);
                            break;
                        case "Integrated Security":
                        case "Trusted_Connection":
                            Integrated_Security = strArr3[i, 1]; bitArray.Set(3, true);
                            break;
                        case "Password":
                            Password = strArr3[i, 1]; bitArray.Set(4, true);
                            break;
                        case "User ID":
                            User_ID = strArr3[i, 1]; bitArray.Set(5, true);
                            break;
                    }
                }
                int[] array = new int[1];
                bitArray.CopyTo(array, 0);
                State = array[0];
            }
            catch { State=-1; }
            
        }

    public override string ToString()
    {
      string StrConnect = "Server=" + Data_Source.ToString() + ";Database=" + Initial_Catalog.ToString() + ";Connect Timeout=" + Connect_Timeout.ToString() + ";";
      string res = Integrated_Security.ToString().ToLower();
      if (res == "true") StrConnect = StrConnect + "Trusted_Connection = True;";
      else StrConnect = StrConnect + "User ID=" + User_ID.ToString() + ";" + "Password=" + Password.ToString() + ";";
      return StrConnect;
    }
  }
}
