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
using SQLWork;
using StringWork;

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
            query = new Query("Server=.\\SQLEXPRESS;Database=test;Trusted_Connection=True;");
            dt = new DataTable();
            query.GetDataTable("TestCreateTable", ref dt);
            dgvTest.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sql = new Query(tbx1.Text);
            StrConToEntity Entity = new StrConToEntity(tbx1.Text);
            //if (sql.Connect()=="") tbx1.Text = tbx1.Text + "Подключение к БД выполнено!" + Environment.NewLine;
            //else tbx1.Text = tbx1.Text + "Подключение к БД не выполнено!" + Environment.NewLine;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (sql.Close()=="") tbx1.Text = tbx1.Text + "Отключение от БД выполнено!" + Environment.NewLine;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbx1.Text = "";
        }

  private void btnCreate_Click(object sender, EventArgs e)
  {
     sql = new Query(tbx1.Text);
   string strRes = sql.CreateDB("Test2", 10, Environment.CurrentDirectory + @"\");
   if (strRes != "")  MessageBox.Show(strRes);
  }

    private void btnAddStr_Click(object sender, EventArgs e)
    {
      tbxDest.Text = Str.AddStrEnd(tbxSourse.Text, tbxAdd.Text);
    }

      private void btnSaveDgv_Click(object sender, EventArgs e)
      {
         query.SetDataTable();
         query.GetDataTable("TestCreateTable", ref dt);
         dgvTest.DataSource = dt;
      }
   }
}
