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
         query.SetDataTable();
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
         query.SetExtPropDB("test27", tbxPropName.Text, tbxPropVal.Text);
      }

      private void btnReadProp_Click(object sender, EventArgs e)
      {
         query = new Query("Server=.\\SQLEXPRESS;Database=test27;Trusted_Connection=True;");
         tbxPropVal.Text = query.GetExtPropDB("test27", tbxPropName.Text);
      }

      private void btnDelProp_Click(object sender, EventArgs e)
      {
         query = new Query("Server=.\\SQLEXPRESS;Database=test27;Trusted_Connection=True;");
         query.DelExtPropDB("test27", tbxPropName.Text);
      }
   }
}
