using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkSQL;
using WorkString;
using SQLTuneCon;
using ExcelWork;
using System.IO;
using WorkMisc;
using WorkFile;
using WorkBox;

namespace Test_Form
{
    public partial class Form1 : Form
    {
        Query sql;
        Query query;
        DataTable dt;
        public Form1()
        {
            
            InitializeComponent();
            tbx1.Text = @"Data Source=(local)\SQLEXPRESS;Initial Catalog=master; Connect Timeout=5; Integrated Security = True; " + Environment.NewLine;
            //sql = new SQLmgr(tbx1.Text);
            query = new Query("Server=.\\SQLEXPRESS;Database=test27;Trusted_Connection=True;");
            dt = new DataTable();
            //query.GetTable("TestCreateTable", ref dt);
            //dgvTest.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sql = new Query(tbx1.Text);
            Entity Entity = new Entity(tbx1.Text);
            //if (sql.Connect()=="") tbx1.Text = tbx1.Text + "Подключение к БД выполнено!" + Environment.NewLine;
            //else tbx1.Text = tbx1.Text + "Подключение к БД не выполнено!" + Environment.NewLine;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (sql.Close()) tbx1.Text = tbx1.Text + "Отключение от БД выполнено!" + Environment.NewLine;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbx1.Text = "";
        }

         private void btnCreate_Click(object sender, EventArgs e)
         {
         sql = new Query(tbx1.Text);
         sql.CreateDB("Test2", 10, Environment.CurrentDirectory + @"\");
         }

    private void btnAddStr_Click(object sender, EventArgs e)
    {
      tbxDest.Text = Str.AddStrEnd(tbxSourse.Text, tbxAdd.Text);
    }

      private void btnSaveDgv_Click(object sender, EventArgs e)
      {
         query.SetTable();
         query.GetTable("TestCreateTable", ref dt);
         dgvTest.DataSource = dt;
      }

      private void button1_Click_1(object sender, EventArgs e)
      {
         TuneCon sQLTuneCon = new TuneCon();
         sQLTuneCon.Show();
      }

      private void btnImport_Click(object sender, EventArgs e)
      {
         // Выбрать путь и имя файла в диалоговом окне
         OpenFileDialog ofd = new OpenFileDialog();
         // Задаем расширение имени файла по умолчанию (открывается папка с программой)
         ofd.DefaultExt = "*.xls;*.xlsx";
         // Задаем строку фильтра имен файлов, которая определяет варианты
         ofd.Filter = "файл Excel (Spisok.xlsx)|*.xlsx";
         // Задаем заголовок диалогового окна
         ofd.Title = "Выберите файл базы данных";
         if (!(ofd.ShowDialog() == DialogResult.OK)) // если файл БД не выбран -> Выход
            return;
         List<string> types = new List<string>() { "Ana", "Exch", "Vlv", "Reg", "Eng", "Dos", "Dis", "stBindAddr", "List" };
         Exc.GetDataStr(ofd.FileName);
         Exc.EndLoad += Exc_EndLoad;
         timer1.Enabled = true;
 
      }

      private void Exc_EndLoad()
      {
         //throw new NotImplementedException();
         List<string> types = new List<string>() { "Ana", "Exch", "Vlv", "Reg", "Eng", "Dos", "Dis", "stBindAddr", "List" };
         List<List<string>> list = Str.GetDataTypeObjInListList(Exc.ListListData, types, "Dis");
      }

      private void timer1_Tick(object sender, EventArgs e)
      {
         lblCountCell.Text = Exc.CellCount.ToString();
         if (Exc.ResultGetDataStr == "OK")
         {
         }
      }

      private void btnImportStop_Click(object sender, EventArgs e)
      {
         Exc.EndGetDataStr();
      }

      private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
      {

      }

      private void groupBox1_Enter(object sender, EventArgs e)
      {

      }

      private void btnWrProp_Click(object sender, EventArgs e)
      {
         query = new Query("Server=.\\SQLEXPRESS;Database=test27;Trusted_Connection=True;");
         query.SetExtProp(name_bas:"test27", 
                          name_tab: txbTabName.Text == "-1" ? null : txbTabName.Text, 
                          name_col: txbColName.Text == "-1" ? null : txbColName.Text, 
                          name_prop:txbPropName.Text, 
                          val: txbPropVal.Text=="-1"?null: txbPropVal.Text);
      }

      private void btnReadProp_Click(object sender, EventArgs e)
      {
         query = new Query("Server=.\\SQLEXPRESS;Database=test27;Trusted_Connection=True;");
         DataTable table;
         txbPropVal.Text 
            = query.GetExtProp(table_result: out table,
                               name_bas:  txbBasName.Text  == "null" ? null : txbBasName.Text, 
                               name_tab:  txbTabName.Text  == "null" ? null : txbTabName.Text,
                               name_col:  txbColName.Text  == "null" ? null : txbColName.Text,
                               name_prop: txbPropName.Text == "null" ? null : txbPropName.Text);
         lblRowCount.Text = table.Rows.Count.ToString();

         query.GetTable(txbTabName.Text,ref table);
         query.SetDataTabColNamespaceFromExtProp(ref table, txbPropName.Text);
      }

      private void btnDelProp_Click(object sender, EventArgs e)
      {
         query = new Query("Server=.\\SQLEXPRESS;Database=test27;Trusted_Connection=True;");
         query.SetExtProp(name_bas:"test27", name_prop: txbPropName.Text, val: null);
      }

      private void button2_Click(object sender, EventArgs e)
      {
         byte[] BytesResult = null;
         using (FileStream fileStream = File.OpenRead("G:\\00 Work\\GENERATOR\\Rtn\\ANA1"))
         using (BinaryReader binaryReader = new BinaryReader(fileStream))
         BytesResult = binaryReader.ReadBytes(256);
         Dictionary<int,float> FloatsResult = new Dictionary<int, float>();
         int m = 0;
         tbx1.Clear();

         Files files = new Files();
         List<byte[]> RawData = new List<byte[]>();
         FloatsResult = files.GetFloatFromHexFile("G:\\00 Work\\GENERATOR\\Rtn\\ANA1", out RawData, 0, 0, txtOrderBytes.Text);
         if (FloatsResult != null) 
         {
            int i = 0;
            foreach ( var fResult in FloatsResult)
            {
               if (FloatsResult[i] != 0)
               {
                  tbx1.Text = tbx1.Text + "Address: [" + fResult.Key.ToString() + "] = ";
                  tbx1.Text = tbx1.Text + fResult.Value.ToString() + Environment.NewLine;
               }
               i++;
            }
         }
         
      }

      private void btnWriteHex_Click(object sender, EventArgs e)
      {
         string InputText = tbx1.Text.Replace("Address: [","");
         string[] LinesText = InputText.Split('\n');
         Dictionary<int, float> Data = new Dictionary<int, float>();
         foreach(string LineText in LinesText)
         {
            if (LineText == "") continue;
            string[] temp = LineText.Replace(" = ", "").Split(']');
            Data.Add(key: int.Parse(temp[0]), value: float.Parse(temp[1].Replace(".", ",")));
         }
         Files files = new Files();
         int[] num = new int[Data.Count];
         float[] val = new float[Data.Count];
         int i = 0;
         foreach (var dat in Data)
         {
            num[i] = dat.Key;
            val[i] = dat.Value;
            i++;
         }
         files.SetFloatInHexFile("G:\\00 Work\\GENERATOR\\Rtn\\ANA1", val, num[0], txtOrderBytes.Text);

      }

      private void btnOpenHexRW_Click(object sender, EventArgs e)
      {
         HexFileRW hexFileRW = new HexFileRW();
         hexFileRW.Show();
      }

      private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
      {
         DataGridView table = (DataGridView)sender;
         DataGridViewCell cell = table.CurrentCell;
         DataGridViewCheckBoxCell boxCell = (DataGridViewCheckBoxCell)table.CurrentCell;
      }

      private void button2_Click_1(object sender, EventArgs e)
      {
         HexFileToCSV hexFileToCSV = new HexFileToCSV();
         hexFileToCSV.Show();
      }

      private void textBox1_TextChanged(object sender, EventArgs e)
      {

      }
   }
}
