using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExchMemNET;
using SQLWork;
using StringWork;
using Box;
using ControlWork;

namespace SQLTuneConnect
{
  public partial class SQLTuneCon : Form
  {
    List<ComboBox> gbxTuneServer_cbxs;
    List<TextBox> gbxCreateNewDB_tbxs;
    Query sql;
    string StrConnect;
    int BarTimer = 0;
    Dictionary<string, string> dicNameServer;
    Dictionary<string, string> dicNameDB;
    Dictionary<string, string> dicTimeConnect;
    Dictionary<string, string> dicUserName;
    Dictionary<string, string> dicUserPass;
    InputBox inputBox;
    OutBox outBox;
    public SQLTuneCon()
    {
      InitializeComponent();
      init();

    }

    private void chbWinUserAccess_CheckedChanged(object sender, EventArgs e)
    {
      cbxUserName.Enabled = !cbxUserName.Enabled;
      cbxUserPass.Enabled = !cbxUserPass.Enabled;
      CheckBox cbx = (CheckBox)sender;
      if (cbx.Checked)
      { cbxUserName.Text = " "; cbxUserPass.Text = " "; }
      else
      { cbxUserName.Text = ""; cbxUserPass.Text = ""; }
    }

    private void btnTest_Click(object sender, EventArgs e)
    {

      
      if (!TestInputCbx(gbxTuneServer_cbxs)) return;

      StrConnect = "Server=" + cbxNameServer.Text + 
                   ";Database=" + cbxNameDB.Text + 
                   ";Connect Timeout=" + cbxTimeConnect.Text + ";";
      if (chbWinUserAccess.Checked) 
         StrConnect = StrConnect + "Trusted_Connection=True;";
      else 
         StrConnect = StrConnect + "User ID=" + cbxUserName.Text + ";" + "Password=" + cbxUserPass.Text + ";";

      sql = new Query(StrConnect);
      sql.TestCon();

      btnTest.Enabled = false;
      timer1.Enabled = true;
      BarTimer = int.Parse(cbxTimeConnect.Text);

    }
    private void InitComboBox(List<ComboBox> controls)
    {
      foreach (ComboBox cbx in controls)
      {
        cbx.BackColor = Color.White; cbx.ForeColor = Color.Black;
      }
    }
    private void InitTextBox(List<TextBox> controls)
      {
         foreach (TextBox cbx in controls)
         {
            cbx.BackColor = Color.White; cbx.ForeColor = Color.Black;
         }
     }
    private bool TestInputCbx(List<ComboBox> controls)
    {
      InitComboBox(controls);
      bool exit = true;
      foreach (ComboBox cbx in controls)
      {
        if (cbx.Text == "") { exit = false; cbx.BackColor = Color.Red; cbx.ForeColor = Color.White; }
      }
      return exit;
    }
    private bool TestInputTbx(List<TextBox> controls)
    {
      InitTextBox(controls);
      bool exit = true;
      foreach (TextBox tbx in controls)
      {
        if (tbx.Text == "") { exit = false; tbx.BackColor = Color.Red; tbx.ForeColor = Color.White; }
      }
      return exit;
    }
      private List<ComboBox> GetComboBox(Control.ControlCollection controls)
      {
         List<ComboBox> lstCbx = new List<ComboBox>();
         foreach (Control control in controls)
         {
            if (control is ComboBox)
               lstCbx.Add((ComboBox)control);
         }
         return lstCbx;
      }
      private List<TextBox> GetTextBox(Control.ControlCollection controls)
      {
         List<TextBox> lstCbx = new List<TextBox>();
         foreach (Control control in controls)
         {
            if (control is TextBox)
               lstCbx.Add((TextBox)control);
         }
         return lstCbx;
      }
      private List<TextBox> GetAllTextBox(Control.ControlCollection controlCollection)
    {
      List<TextBox> Controls = new List<TextBox>();
      foreach (Control control in controlCollection)
      {
        if (control is TextBox)
          Controls.Add((TextBox)control);
      }
      return Controls;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      SaveProp(StrConnect);
      Memory memory = new Memory();
      memory.SetStrData(StrConnect, "SQLConnectStr");
      MessageBox.Show(
        memory.GetStrData("SQLConnectStr"),
        "Сохранение строки подключения",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information);
      init();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (BarTimer <= 0) { pbrRun.Enabled = false; BarTimer = 0; }
      pbrRun.Minimum = 0;
      pbrRun.Maximum = int.Parse(cbxTimeConnect.Text);
      int val = int.Parse(cbxTimeConnect.Text) - BarTimer;
      if (int.Parse(cbxTimeConnect.Text) - BarTimer < pbrRun.Minimum) val = pbrRun.Minimum;
      if (int.Parse(cbxTimeConnect.Text) - BarTimer > pbrRun.Maximum) val = pbrRun.Maximum;
      if (sql.TestConOk == 1) // Успешное завершение
      { 
        val = pbrRun.Maximum; 
        btnSave.Enabled = true; 
        lblConnctYes.Visible = true;
        List<string> lstrNameDB = new List<string>();
        string result = sql.GetNamesDB(ref lstrNameDB);
        if (result != ""){ MessageBox.Show(result); return; }
        FillCbx(ref cbxNameDB, Str.Sort(lstrNameDB, false));
      }
      if (sql.TestConOk == 2) { val = pbrRun.Minimum; lblConnctYes.Visible = false; }
      if (sql.TestConOk > 0) { btnTest.Enabled = true; timer1.Enabled = false; }
      pbrRun.Value = val;
      BarTimer--;
    }

    private void SaveProp(string StrConnect)
    {
      if (Properties.Settings.Default.StrConnect1 == "" | 
          Properties.Settings.Default.StrConnect1 == StrConnect) 
        { Properties.Settings.Default.StrConnect1  = StrConnect; Properties.Settings.Default.Save(); return; }
      if (Properties.Settings.Default.StrConnect2 == "" |
          Properties.Settings.Default.StrConnect2 == StrConnect) 
        { Properties.Settings.Default.StrConnect2  = StrConnect; Properties.Settings.Default.Save(); return; }
      if (Properties.Settings.Default.StrConnect3 == "" |
          Properties.Settings.Default.StrConnect3 == StrConnect) 
        { Properties.Settings.Default.StrConnect3  = StrConnect; Properties.Settings.Default.Save(); return; }
      if (Properties.Settings.Default.StrConnect4 == "" |
          Properties.Settings.Default.StrConnect4 == StrConnect)
        { Properties.Settings.Default.StrConnect4 = StrConnect; Properties.Settings.Default.Save(); return; }
      if (Properties.Settings.Default.StrConnect5 == "" |
          Properties.Settings.Default.StrConnect5 == StrConnect)
        { Properties.Settings.Default.StrConnect5 = StrConnect; Properties.Settings.Default.Save(); return; }

      Properties.Settings.Default.StrConnect1 =  Properties.Settings.Default.StrConnect2;
      Properties.Settings.Default.StrConnect2 =  Properties.Settings.Default.StrConnect3;
      Properties.Settings.Default.StrConnect3 =  Properties.Settings.Default.StrConnect4;
      Properties.Settings.Default.StrConnect4 =  Properties.Settings.Default.StrConnect5;
      Properties.Settings.Default.StrConnect5 =  StrConnect;
      Properties.Settings.Default.Save();
    }

      private void init()
      {
         dicNameServer = new Dictionary<string, string>();
         dicNameDB = new Dictionary<string, string>();
         dicTimeConnect = new Dictionary<string, string>();
         dicUserName = new Dictionary<string, string>();
         dicUserPass = new Dictionary<string, string>();
         if (Properties.Settings.Default.StrConnect5 != "") FillDic(Properties.Settings.Default.StrConnect5);
         if (Properties.Settings.Default.StrConnect4 != "") FillDic(Properties.Settings.Default.StrConnect4);
         if (Properties.Settings.Default.StrConnect3 != "") FillDic(Properties.Settings.Default.StrConnect3);
         if (Properties.Settings.Default.StrConnect2 != "") FillDic(Properties.Settings.Default.StrConnect2);
         if (Properties.Settings.Default.StrConnect1 != "") FillDic(Properties.Settings.Default.StrConnect1);
         FillCbxs();
         btnSave.Enabled = false;

         gbxTuneServer_cbxs = GetComboBox(this.gbxTuneServer.Controls);
         gbxCreateNewDB_tbxs = GetTextBox(this.gbxCreateNewDB.Controls);

         this.AutoSizeMode = AutoSizeMode.GrowAndShrink; // нельзя изменять размер формы
         inputBox = new InputBox();
         outBox = new OutBox(new Font("Tahoma", 40, FontStyle.Regular), Color.Black);

         // Аутоидентификация по Windows user
         chbWinUserAccess.Checked = true;

      }
    private void FillDic(string str)
    {
      Str strSep = new Str();
      string[,] strArr = strSep.StrSqlConToArr(str);
      
        for (int i = 0; i < strArr.Length / 2; i++)
        {
        try
        {
          switch (strArr[i, 0])
          {
            case "Server":
              dicNameServer.Add(strArr[i, 1], strArr[i, 0]);
              break;
            case "Database":
              dicNameDB.Add(strArr[i, 1], strArr[i, 0]);
              break;
            case "Connect Timeout":
              dicTimeConnect.Add(strArr[i, 1], strArr[i, 0]);
              break;
            case "User ID":
              dicUserName.Add(strArr[i, 1], strArr[i, 0]);
              break;
            case "Password":
              dicUserPass.Add(strArr[i, 1], strArr[i, 0]);
              break;
          }
        }
        catch { }
      }
      
    }
    private void FillCbx(ref ComboBox cbx, List<string> lstr)
    {
      cbx.Items.Clear();
      for (int i = 0; i < lstr.Count; i++)
      {
        cbx.Items.Add(lstr[i]);
      }
    }
    
    /// <summary>
    /// Наполнение данными объекта ComboBox
    /// </summary>
    /// <param name="comboBox"></param>
    /// <param name="dic"></param>
    /// <param name="index"></param>
    private void FillControl(ref ComboBox comboBox, Dictionary<string,string> dic, int index = 0)
    {
         foreach (string data in dic.Keys)
         {
            comboBox.Items.Add(data);
            comboBox.SelectedIndex = index;
         }
    }
    private void FillCbxs()
    {
      ClearCbx();
         Fill.Control(ref cbxNameServer, dicNameServer);
         Fill.Control(ref cbxNameDB, dicNameDB);
         Fill.Control(ref cbxTimeConnect, dicTimeConnect);
         Fill.Control(ref cbxUserName, dicUserName);
         Fill.Control(ref cbxUserPass, dicUserPass);
    }
    private void ClearCbx()
    {
      cbxNameServer.Items.Clear();
      cbxNameDB.Items.Clear();
      cbxTimeConnect.Items.Clear();
      cbxUserName.Items.Clear();
      cbxUserPass.Items.Clear();
    }

    private void btnFindPatch_Click(object sender, EventArgs e)
    {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        if(Properties.Settings.Default.StrPatchCreateNewDB!="")
        folderBrowserDialog.SelectedPath = Properties.Settings.Default.StrPatchCreateNewDB;
        if (folderBrowserDialog.ShowDialog() == DialogResult.Cancel)
        return;
        tbxPatchNewDB.Text = Str.AddStrEnd(folderBrowserDialog.SelectedPath, @"\");
    }

    /// <summary>
    /// Создание начальной базы данных
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnCreateDB_Click(object sender, EventArgs e)
      {
        // Проверка данных ввода  
        
        if (!TestInputTbx(gbxCreateNewDB_tbxs)) return;
        if(sql is null) { MessageBox.Show("Необходимо проверить подключение к БД!"); return; }

         InitCreateDB();
      }

    private void tbxSizeDB_KeyPress(object sender, KeyPressEventArgs e)
    {
      char number = e.KeyChar;

      if (!Char.IsDigit(number) & !(number=='\b'))
      {
        e.Handled = true;
      }
    }

    private void tbxPatchNewDB_TextChanged(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox)sender;
      Properties.Settings.Default.StrPatchCreateNewDB = textBox.Text;
      Properties.Settings.Default.Save();

    }

    private void cbxTimeConnect_KeyPress(object sender, KeyPressEventArgs e)
    {
      char number = e.KeyChar;

      if (!Char.IsDigit(number) & !(number == '\b'))
      {
        e.Handled = true;
      }
    }

    private void btnDeleteDB_Click(object sender, EventArgs e)
    {
      DialogResult result = MessageBox.Show("Удалить БД \"" + tbxNameNewDB.Text + "\"?", 
                                            "Подтверждение", 
                                            MessageBoxButtons.OKCancel, 
                                            MessageBoxIcon.Question, 
                                            MessageBoxDefaultButton.Button2/*,
                                            MessageBoxOptions.DefaultDesktopOnly*/);
      if (result == DialogResult.Cancel) return;
      if (sql is null) { MessageBox.Show("Необходимо проверить подключение к БД!"); return; }
      string strRes = sql.DeleteDB(tbxNameNewDB.Text);
      if (strRes != "") MessageBox.Show(strRes);
      else MessageBox.Show("БД " + tbxNameNewDB.Text + " удалена.", "Удаление БД");
    }

    private void cbxNameDB_SelectedIndexChanged(object sender, EventArgs e)
    {
      tbxNameNewDB.Text = ((ComboBox)sender).Text;
    }

    private void btnCreateTbl_Click(object sender, EventArgs e)
    {
      string strResCreateTab1 = sql.CreateTable("TestCreateTable", new string[] { "FirsName", "LastName" });
         if (strResCreateTab1 !="") MessageBox.Show(strResCreateTab1);
    }

    private void btnDelTbl_Click(object sender, EventArgs e)
    {
      if (outBox.ShowDialog()) MessageBox.Show("Удалил)");
      //if(outBox.ShowDialog("Сообщение", "Удалить БД?")) MessageBox.Show("Удалил)");
      //string strResDelTab = sql.DeleteTable("TestCreateTable");
    }

    private void btnRenameTbl_Click(object sender, EventArgs e)
    {
      inputBox.ShowDialog("Ввод данных", "Введите новое имя таблицы:");
      string result = inputBox.Result;
      if (result == "") return;
      sql.RenameTable(tbxCurNameTbl.Text, result);
    }

    private void btnListTbl_Click(object sender, EventArgs e)
    {
         List<string> lstr = new List<string>();
         string result = sql.GetNamesTable(ref lstr);
         if (result != "") outBox.ShowDialog("Ошибка", result);  
    }
    private void InitCreateDB()
    {
         List<string> strRes = new List<string>();
         try
         {
            // Создание БД
            strRes.Add(sql.CreateDB(tbxNameNewDB.Text, int.Parse(tbxSizeDB.Text), tbxPatchNewDB.Text, true));
            // Создание таблиц объектов
            strRes.Add(sql.CreateTable("ObjectsType", new string[] { "NAME",
                                                                     "OBJECT_ID",
                                                                     "DESCRIPTION",
                                                                     "PLC",
                                                                     "PLC_IN",
                                                                     "PLC_RTN",
                                                                     "PLC_TAG",
                                                                     "R_W",
                                                                     "EPROM",
                                                                     "UDT"}));
            // Наполнение таблицы объектов
            DataTable DT = new DataTable();
            strRes.Add(sql.GetDataTable("ObjectsType", ref DT));

            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Exch", "0", "АРМ или Панели оператора", "Yes", "Yes", "", "Yes", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Eng", "4", "Электродвигатели/клапана", "Yes", "Yes", "Yes", "Yes", "Yes", "40000" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Dos", "7", "Дискретныe выходные сигналы", "Yes", "Yes", "Yes", "Yes", "Yes", "41100" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Dis", "2", "Дискретныe входные сигналы", "Yes", "Yes", "Yes", "Yes", "Yes", "41500" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Ana", "1", "Аналоговые сигналы", "Yes", "Yes", "Yes", "Yes", "Yes", "42000" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "stBindAddr", "", "Структура", "", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "FT_trig", "", "Тригер положительный", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "RT_trig", "", "Тригер отрецательный", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "MB_Request", "", "ModBus запросы", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Mdl", "3", "Модули ввода/вывода", "Yes", "Yes", "", "Yes", "", "46000" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "MsgH", "", "Сообщения", "Yes", "", "", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Pln", "8", "Система управления установкой", "Yes", "Yes", "", "Yes", "", "46100" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Reg", "6", "Контуры ПИД-регулирования", "Yes", "Yes", "", "Yes", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Timer_Tof", "", "Таймер на выключение", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Timer_Ton", "", "Таймер на включение", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Vlv", "5", "Электроприводные задвижки", "Yes", "Yes", "Yes", "Yes", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "BoolarAny", "", "", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Oth", "9", "Прочие теги", "Yes", "Yes", "", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "DateTime", "", "Дата и время", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Zon", "9", "Зоны пожаротушения", "Yes", "Yes", "", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Rsv", "10", "Контроль резервирования", "Yes", "Yes", "", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Tnk", "11", "Баки дозаторы", "Yes", "Yes", "", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Grp", "", "Группа резервирования", "", "Yes", "", "Yes" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "Lst", "", "Лист защит", "Yes", "Yes", "", "Yes", "", "", "137" }));
            DT.Rows.Add(
               sql.SetDataRow(DT.NewRow(), new string[] { "List", "", "Общий список защит", "Yes", "", "", "", "", "", "140" }));


            strRes.Add(sql.SetDataTable());

         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         foreach (string mess in strRes)
         {
            if (mess != "") { MessageBox.Show(mess); return; }
         }
         MessageBox.Show("БД " + tbxNameNewDB.Text + " создана.", "Создание БД");
      }
  }
}
