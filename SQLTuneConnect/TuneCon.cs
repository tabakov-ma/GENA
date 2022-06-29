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
using WorkSQL;
using WorkString;
using WorkBox;
using WorkControl;
using ExcelWork;
using System.Configuration;

namespace SQLTuneCon
{
   public partial class TuneCon  : Form
   {
      List<ComboBox> gbxTuneServer_cbxs;
      List<TextBox> gbxTuneServer_tbxs;
      List<TextBox> gbxCreateNewDB_tbxs;
      
      Query sql;
      string StrConnect;
      public string ResultStrCon = "";
      int barTimCon = 0;
      int barLoadDataMax = 0;
      Dictionary<string, string> dicNameServer;
      Dictionary<string, string> dicNameDB;
      Dictionary<string, string> dicTimeConnect;
      Dictionary<string, string> dicUserName;
      InputBox inputBox;
      OutBox outBox;
      List<string> sResult = new List<string>(); // Список результатов выполнения функций
      public TuneCon()
      {
         InitializeComponent();
         InitControl();
         this.Left = AccessProp.GetPropInt("X");
         this.Top = AccessProp.GetPropInt("Y");
      }
      

      private void chbWinUserAccess_CheckedChanged(object sender, EventArgs e)
      {
         cbxUserName.Enabled = !cbxUserName.Enabled;
         tbxUserPass.Enabled = !tbxUserPass.Enabled;
         CheckBox cbx = (CheckBox)sender;
         if (cbx.Checked)
         { cbxUserName.Text = " "; tbxUserPass.Text = " "; }
         else
         { cbxUserName.Text = ""; tbxUserPass.Text = ""; }
      }

      private void btnTest_Click(object sender, EventArgs e)
      {

      
         if (!TestInputCbx(gbxTuneServer_cbxs, chbWinUserAccess.Checked ? "cbxUserName" : "") | 
             !TestInputTbx(gbxTuneServer_tbxs)) return;

         Entity entity = new Entity(cbxNameServer.Text, cbxNameDB.Text, chbWinUserAccess.Checked.ToString(), cbxUserName.Text, tbxUserPass.Text, cbxTimeConnect.Text);
         StrConnect = entity.ToString();

         sql = new Query(StrConnect);
         sql.asyncTestCon();

         btnTest.Enabled = false;
         timConnect.Enabled = true;
         barTimCon = int.Parse(cbxTimeConnect.Text);

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
      private bool TestInputCbx(List<ComboBox> controls, string exception = "")
      {
         InitComboBox(controls);
         bool exit = true;
         foreach (ComboBox cbx in controls)
         {
            if (cbx.Text == "" & !exception.Contains(cbx.Name)) 
               { exit = false; cbx.BackColor = Color.Red; cbx.ForeColor = Color.White; }
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
      private void btnSave_Click(object sender, EventArgs e)
      {
         SaveProp(StrConnect);
         Memory memory = new Memory();
         memory.SetStrData(StrConnect, "SQLConnectStr");
         string temp_str = memory.GetStrData("SQLConnectStr").Replace(";", ";\n");
         outBox.ShowDialog(temp_str,
                          "Сохранение строки подключения",
                          true);
         //InitControl();
         ResultStrCon = StrConnect;
      }

      private string GetProp(string name)
      {
         try
         {
            return Properties.Settings.Default[name].ToString();
         }
         catch (Exception ex) { MessageBox.Show(ex.Message); return "";}
      }
      private string SetProp(string name, string val)
      {
         try
         {
            Properties.Settings.Default[name] = val;
            Properties.Settings.Default.Save();
            return "";
         }
         catch(Exception ex){ return ex.Message; }
      }
      /// <summary>
      /// Сохранение строки подключения в свойствах приложения
      /// </summary>
      /// <param name="StrConnect"></param>
      private void SaveProp(string StrConnect)
      {
         // Проверка пустых или аналогичных записей
         for(int n = 1; n < 6; n++)
         {
            string proper = GetProp("StrConnect" + n);
            if (proper == "" || proper == StrConnect)
            {
               SetProp("StrConnect" + n, StrConnect);
               return;
            }
         }
         // Добавление новой записи и здвиг всех записей с удалением старой
         for (int n = 1; n < 5; n++)
         {
            SetProp("StrConnect" + n, GetProp("StrConnect" + (n + 1).ToString()));
         }
         SetProp("StrConnect5", StrConnect);
      }

      private void InitControl()
      {
         dicNameServer = new Dictionary<string, string>();
         dicNameDB = new Dictionary<string, string>();
         dicTimeConnect = new Dictionary<string, string>();
         dicUserName = new Dictionary<string, string>();

         for(int n = 5; n > 0; n--)
         {
            string strTemp = GetProp("StrConnect" + n);
            if (strTemp != "") FillDic(strTemp);
         }
         FillCbxs();
         btnSave.Enabled = false;

         gbxTuneServer_cbxs = Ctrl.GetComboBoxs(this.gbxTuneServer.Controls);
         gbxTuneServer_tbxs = Ctrl.GetTextBoxs(this.gbxTuneServer.Controls);
         gbxCreateNewDB_tbxs = Ctrl.GetTextBoxs(this.gbxCreateNewDB.Controls);
         // Настройка формы. Блокировка изменения размера формы
         this.AutoSizeMode = AutoSizeMode.GrowAndShrink; 
         // Пользовательские окна ввода/вывода
         inputBox = new InputBox();
         outBox = new OutBox(new Font("Tahoma", 12, FontStyle.Regular), Color.Black);

         // Аутоидентификация по Windows user
         chbWinUserAccess.Checked = true;
         // Путь для сохранения БД
         if (Properties.Settings.Default.StrPatchCreateNewDB != "")
         tbxPatchNewDB.Text = Properties.Settings.Default.StrPatchCreateNewDB;
         // Начальные данные для поля максимального размера БД
         tbxSizeDB.Text = 11.ToString();

      }
      private void FillDic(string str)
      {
         Entity Entity = new Entity(str);

         if (Entity.Data_Source != null && !dicNameServer.ContainsKey(Entity.Data_Source)) 
            dicNameServer.Add(Entity.Data_Source, "");
         if (Entity.Initial_Catalog != null && !dicNameDB.ContainsKey(Entity.Initial_Catalog)) 
            dicNameDB.Add(Entity.Initial_Catalog, "");
         if (Entity.Connect_Timeout != null && !dicTimeConnect.ContainsKey(Entity.Connect_Timeout)) 
            dicTimeConnect.Add(Entity.Connect_Timeout, "");
         if (Entity.User_ID != null && !dicUserName.ContainsKey(Entity.User_ID)) 
            dicUserName.Add(Entity.User_ID, "");
      }
     
      private void FillCbxs()
      {
         Ctrl.Clear(this.gbxTuneServer.Controls);
         Ctrl.Fill(ref cbxNameServer, dicNameServer);
         Ctrl.Fill(ref cbxNameDB, dicNameDB);
         Ctrl.Fill(ref cbxTimeConnect, dicTimeConnect);
         Ctrl.Fill(ref cbxUserName, dicUserName);
   
      }
      
      /// <summary>
      /// Диалог выбора пути
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnFindPatch_Click(object sender, EventArgs e)
      {
         FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
         if(tbxPatchNewDB.Text != "")
            folderBrowserDialog.SelectedPath = tbxPatchNewDB.Text;
         if (folderBrowserDialog.ShowDialog() == DialogResult.Cancel) return;

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
         if(sql is null) 
            { MessageBox.Show("Необходимо проверить подключение к БД!"); return; }
         InitCreateDbObjectsType();
      }

    
      /// <summary>
      /// Событие. Ввод данных, ограничение вводимых типов символов
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tbxSizeDB_KeyPress(object sender, KeyPressEventArgs e)
      {
         char number = e.KeyChar;

         if (!Char.IsDigit(number) & !(number=='\b'))
         {
            e.Handled = true;
         }
      }


      /// <summary>
      /// Событие. Изменение пути до файлов БД, сохранение в свойствах приложения
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tbxPatchNewDB_TextChanged(object sender, EventArgs e)
      {
         SetProp("StrPatchCreateNewDB", ((TextBox)sender).Text);
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
         DialogResult result = outBox.ShowDialog("Подтверждение", "Удалить БД \"" + tbxNameNewDB.Text + "\"?");
         //DialogResult result = MessageBox.Show("Удалить БД \"" + tbxNameNewDB.Text + "\"?", 
         //                     "Подтверждение", 
         //                     MessageBoxButtons.OKCancel, 
         //                     MessageBoxIcon.Question, 
         //                     MessageBoxDefaultButton.Button2);
         if (result == DialogResult.Cancel) return;
         if (sql is null) { MessageBox.Show("Необходимо проверить подключение к БД!"); return; }
         if(!sql.DelDB(tbxNameNewDB.Text)) return;
         MessageBox.Show("БД " + tbxNameNewDB.Text + " удалена.", "Удаление БД");
      }

      private void cbxNameDB_SelectedIndexChanged(object sender, EventArgs e)
      {
      tbxNameNewDB.Text = ((ComboBox)sender).Text;
      }

      private void btnCreateTbl_Click(object sender, EventArgs e)
      {
         sql.CreateTable("TestCreateTable", new string[] { "FirsName", "LastName" });
      }

      private void btnDelTbl_Click(object sender, EventArgs e)
      {
         outBox.ShowDialog("Удалить БД \"" + tbxCurNameTbl.Text + "\"?", "Предупреждение");
         if (outBox.Result == DialogResult.OK)
         {
            if (sql == null)
            {
               outBox.ShowDialog("Нет подключения к БД!", "Предупреждение", true);
               return;
            }
            sql.DelTable(tbxCurNameTbl.Text);
         }
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
         sql.GetNamesTable(ref lstr); 
      }
      private void InitCreateDbObjectsType()
      {
         
         try
         {
            // Создание БД
            if(!sql.CreateDB(tbxNameNewDB.Text, int.Parse(tbxSizeDB.Text), tbxPatchNewDB.Text, true))return;
            string TableName = "TypesObjects";
            // Создание таблицы типов объектов
            if(!sql.CreateTable(TableName, new string[] { "NAME",
                                                         "OBJECT_ID",
                                                         "DESCRIPTION",
                                                         "PLC",
                                                         "PLC_IN",
                                                         "PLC_RTN",
                                                         "PLC_TAG",
                                                         "R_W",
                                                         "EPROM",
                                                         "UDT"}))return;
            // Наполнение таблицы объектов
            DataTable DT = new DataTable();
            if(!sql.GetTable(TableName, out DT)) return;
            {
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Exch", "0", "АРМ или Панели оператора", "Yes", "Yes", "", "Yes", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Eng", "4", "Электродвигатели/клапана", "Yes", "Yes", "Yes", "Yes", "Yes", "40000" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Dos", "7", "Дискретныe выходные сигналы", "Yes", "Yes", "Yes", "Yes", "Yes", "41100" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Dis", "2", "Дискретныe входные сигналы", "Yes", "Yes", "Yes", "Yes", "Yes", "41500" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Ana", "1", "Аналоговые сигналы", "Yes", "Yes", "Yes", "Yes", "Yes", "42000" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "stBindAddr", "", "Структура", "", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "FT_trig", "", "Тригер положительный", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "RT_trig", "", "Тригер отрецательный", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "MB_Request", "", "ModBus запросы", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Mdl", "3", "Модули ввода/вывода", "Yes", "Yes", "", "Yes", "", "46000" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "MsgH", "", "Сообщения", "Yes", "", "", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Pln", "8", "Система управления установкой", "Yes", "Yes", "", "Yes", "", "46100" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Reg", "6", "Контуры ПИД-регулирования", "Yes", "Yes", "", "Yes", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Timer_Tof", "", "Таймер на выключение", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Timer_Ton", "", "Таймер на включение", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Vlv", "5", "Электроприводные задвижки", "Yes", "Yes", "Yes", "Yes", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "BoolarAny", "", "", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Oth", "9", "Прочие теги", "Yes", "Yes", "", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "DateTime", "", "Дата и время", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Zon", "9", "Зоны пожаротушения", "Yes", "Yes", "", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Rsv", "10", "Контроль резервирования", "Yes", "Yes", "", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Tnk", "11", "Баки дозаторы", "Yes", "Yes", "", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Grp", "", "Группа резервирования", "", "Yes", "", "Yes" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "Lst", "", "Лист защит", "Yes", "Yes", "", "Yes", "", "", "137" }));
               DT.Rows.Add(
               Ctrl.SetDataRow(DT.NewRow(), new string[] { "List", "", "Общий список защит", "Yes", "", "", "", "", "", "140" }));
            }

            if(!sql.SetTable())return;
            // Загрузка исходных данных 
            // Выбрать путь и имя файла в диалоговом окне
            OpenFileDialog ofd = new OpenFileDialog();
            // Задаем расширение имени файла по умолчанию (открывается папка с программой)
            ofd.DefaultExt = "*.xls;*.xlsx";
            // Задаем строку фильтра имен файлов, которая определяет варианты
            ofd.Filter = "файл Excel (Spisok.xlsx)|*.xlsx";
            // Задаем заголовок диалогового окна
            ofd.Title = "Выберите файл для построения базы Objects";
            if (!(ofd.ShowDialog() == DialogResult.OK)) // если файл БД не выбран -> Выход
            return;
            //List<string> types = new List<string>() { "Ana", "Exch", "Vlv", "Reg", "Eng", "Dos", "Dis", "stBindAddr", "List" };
            Exc.GetDataStr(ofd.FileName);
            Exc.EndLoad += Exc_EndLoad;
            timLoadData.Enabled = true;
            barLoadDataMax = 0;

         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      
      }

      private void InitCreateDbObjects()
      {
         try
         {
            // Получение списка типов объектов
            DataTable dtObjectsType = new DataTable();
            if(!sql.GetTable("TypesObjects", out dtObjectsType))return;
            List<string> lTypesObj = Ctrl.GetListRowsStrInOneColumnTable(dtObjectsType, 1);
            lTypesObj = Ctrl.GetListRowsStrInOneColumnTable(dtObjectsType, 1);
            // Имя таблицы объектов
            string TableName = "Objects";
            // Список названий столбцов
            List<string> lHead = Exc.GetRowInListListData(0);
            // Добавление в список столбцов, которых нет в excel
            if(!lHead.Contains("NAME_TYPE")) lHead.Insert(0, "NAME_TYPE");
            if (!lHead.Contains("SAME")) lHead.Add("SAME");
            // Создание таблицы объектов
            if(!sql.CreateTable(TableName, lHead))return;
            //strRes.Add(sql.CreateTable(TableName, new string[] { "NAME_TYPE",    // 1
            //                                          "TYPE",         // 2
            //                                          "NAME",         // 3
            //                                          "DESCRIPTION",  // 4
            //                                          "ARRAY_SIZE",   // 5
            //                                          "PLC",          // 6
            //                                          "PLC_IN",       // 7
            //                                          "PLC_RTN",      // 8
            //                                          "PLC_TAG",      // 9
            //                                          "EXCH",         // 10
            //                                          "WRITE",        // 11
            //                                          "INIT",         // 12
            //                                          "LIST",         // 13
            //                                          "R_W",          // 14
            //                                          "EPROM",        // 15
            //                                          "OPTIONS",      // 16
            //                                          "SAME"}));      // 17


            // Наполнение таблицы объектов
            DataTable DT = new DataTable();
            if(!sql.GetTable(TableName, out DT))return;
            // Проход по типам объектов из таблицы типов
            for (int i = 0; i < lTypesObj.Count; i++)
            {
               List<List<string>> ListListDataObject = Str.GetDataTypeObjInListList(Exc.ListListData, lTypesObj, lTypesObj[i]);
               if (ListListDataObject == null) continue;
               // Проход по строкам объекта
               for (int m = 0; m < ListListDataObject[0].Count; m++)
               {
                  // Проход по данным в строке
                  List<string> row = Ctrl.GetListColumnsInOneRowTable(ListListDataObject, m);
                  row.Insert(0, lTypesObj[i]);
                  DT.Rows.Add(Ctrl.SetDataRow(DT.NewRow(), row));
               }
            }
            if(!sql.SetTable())return;
            //DT.Rows.Add(
            //sql.SetDataRow(DT.NewRow(), new string[] { /*1*/ "Ana", /*2*/ "Bool", /*3*/ "sHVD", /*4*/ "Недостоверное значение - КЗ", /*5*/ "", /*6*/ "", /*7*/ "Yes", /*8*/ "Yes", /*9*/ "Yes", /*10*/ "Yes", /*11*/ "Yes", /*12*/ "Yes", /*13*/ "Yes", /*14*/ "Yes", /*15*/ "Yes", /*16*/ "Yes", /*17*/ "Yes" }));
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         foreach (string mess in sResult)
         {
            if (mess != "") { MessageBox.Show(mess); return; }
         }
         MessageBox.Show("БД " + tbxNameNewDB.Text + " создана.", "Создание БД");
      }
      private void Exc_EndLoad()
      {
         timConnect.Enabled = false;
         InitCreateDbObjects();
      }

      private void timLoadData_Tick(object sender, EventArgs e)
      {
         if (barLoadDataMax == 0 && Exc.CellCount != 0) 
            barLoadDataMax = Exc.CellCount;
         if (barLoadDataMax == 0) return;
         barLoadData.Maximum = barLoadDataMax;
         barLoadData.Value = barLoadData.Maximum - Exc.CellCount;
         toolTip_barLoadData.SetToolTip(barLoadData, "Загрузка данных из Excel таблицы");
      }

      private void timConnect_Tick(object sender, EventArgs e)
      {
         if (barTimCon <= 0)
         {
            barConnect.Enabled = false;
            barTimCon = 0;
         }
         barConnect.Minimum = 0;
         barConnect.Maximum = int.Parse(cbxTimeConnect.Text);
         int val = int.Parse(cbxTimeConnect.Text) - barTimCon;
         if (int.Parse(cbxTimeConnect.Text) - barTimCon < barConnect.Minimum) val = barConnect.Minimum;
         if (int.Parse(cbxTimeConnect.Text) - barTimCon > barConnect.Maximum) val = barConnect.Maximum;
         if (sql.TestConOk == 1) // Успешное завершение
         {
            timConnect.Enabled = false;
            val = barConnect.Maximum;
            btnSave.Enabled = true;
            lblConnctYes.Visible = true;
            List<string> lstrNameDB = new List<string>();
            if(!sql.GetNamesDB(ref lstrNameDB, only_gen: true)) return;
            Ctrl.Fill(ref cbxNameDB, Str.Sort(lstrNameDB, false));
         }
         if (sql.TestConOk == 2)
         {
            val = barConnect.Minimum;
            lblConnctYes.Visible = false;
         }
         if (sql.TestConOk > 0)
         {
            btnTest.Enabled = true;
            timConnect.Enabled = false;
         }
         barConnect.Value = val;
         barTimCon--;
      }

      private void TuneCon_FormClosed(object sender, FormClosedEventArgs e)
      {
         AccessProp.SetProp("X", this.Left);
         AccessProp.SetProp("Y", this.Top);
      }
   }
}
