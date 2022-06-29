using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkSQL;
using WorkControl;
using System.Text.RegularExpressions;

namespace WorkBox
{
   // =========================================================================================
   // Настройка отображения колонок в таблицах баз данных (порядок следования и видимость)
   // Считывание и сохранение настроек производиться в Extendend Properties в формате строки:
   // (TabNam=...,ColNam=...,ColVis,...)(...)
   // При открытии окна, настройки считываются из базы данных,
   // если данных нет, то считывается все таблицы в текущей базе данных
   // при закрытии окна, нажатии кнопки сохранить или переключении между таблицами происходит
   // сохранение 
   // =========================================================================================

   public partial class FormTuneTableColumn : Form
   {
      private WorkSQL.Query query;
      private TuneViewTable tuneViewTable;
      public Dictionary<string, string> StrTuneTabCol
      {
         get
         {
            return tuneViewTable.DicStrTuneTabCol;
         }
      }
      private void Connect(Query objQuery = null)
      {
         if(objQuery != null) query = objQuery;
         try
         {
            if (query == null)
            {
               MessageBox.Show(text: "Нет подключения к БД.",
                               caption: "Предупреждение",
                               buttons: MessageBoxButtons.OK,
                               icon: MessageBoxIcon.Warning,
                               defaultButton: MessageBoxDefaultButton.Button1);
               this.Close();
               return;
            }

            // Объект настройки данных
            tuneViewTable = new TuneViewTable(this.dgv_TuneColumn,
                                              new List<ComboBox> { this.cbx_TableName },
                                              query,
                                              new List<Button> { this.btn_Save, this.btn_SaveAll });
            if (tuneViewTable.DicStrTuneTabCol == null || tuneViewTable.DicStrTuneTabCol.Count == 0)
            {
               MessageBox.Show(text: "Нет ранее сохранённых настроек.",
                               caption: "Предупреждение",
                               buttons: MessageBoxButtons.OK,
                               icon: MessageBoxIcon.Warning,
                               defaultButton: MessageBoxDefaultButton.Button1);

               tuneViewTable.NeedSaveData = true;
            }
            dgv_TuneColumn.DefaultCellStyle.Font = new Font("Tahoma", 10);
         }
         catch (Exception ex)
         {
            MessageBox.Show(text: ex.Message + "\n" + ex.TargetSite,
                                  caption: "Ошибка",
                                  buttons: MessageBoxButtons.OK,
                                  icon: MessageBoxIcon.Error,
                                  defaultButton: MessageBoxDefaultButton.Button1);
         }
      }

      /// <summary>
      /// Конструктор формы
      /// </summary>
      /// <param name="strTuneTableColumn"></param>
      /// <param name="objQuery"></param>
      public FormTuneTableColumn( Query objQuery,string CurrTableName = "")
      {
         InitializeComponent();
         this.Text = objQuery.BaseName + ". Настройка таблиц";
         Connect(objQuery);
         this.toolTip.SetToolTip(btn_Reset, "Сбросить настройки текущей таблицы");
         this.toolTip.SetToolTip(btn_SaveAll, "Сохранить все настройки таблиц в текущей базе");
         this.toolTip.SetToolTip(btn_Save, "Сохранить настройки текущей таблицы в текущей базе");
         this.cbx_TableName.Text = CurrTableName;
      }

      /// <summary>
      /// Сохранение настроек в БД
      /// </summary>
      /// <param name="change"></param>
      private bool SaveDataChange(bool SaveAll, string NameTable)
      {
         bool result = false;
         if ( tuneViewTable != null && tuneViewTable.NeedSaveData )
         {
            string TextTable = SaveAll ? "" : "." + NameTable;
            DialogResult res = MessageBox.Show(text: "Сохранить конфигурацию \nотображения таблиц в базе данных: "+ 
                                                      tuneViewTable.NameBase + TextTable + "?",
                            caption: "Информация",
                            buttons: MessageBoxButtons.YesNo,
                            icon: MessageBoxIcon.Information,
                            defaultButton: MessageBoxDefaultButton.Button1);
            if (res == DialogResult.Yes)
            {
               // Получить настройки из таблицы
               tuneViewTable.GetDataGridView();
               tuneViewTable.SetStrTuneTabColFromSettings(NameTable);
               // Сохранить настройки отображения колонок (Порядок и видимость) из БД
               tuneViewTable.SetDBFromStrTuneTabCol(SaveAll, NameTable);
               result = true;
            }
         }
         return result;
      }
      private void cbx_TableName_SelectedIndexChanged(object sender, EventArgs e)
      {
         if ( tuneViewTable == null || tuneViewTable.ProgFillDataControl) return;
         if (SaveDataChange(false, tuneViewTable.NameTable))
            btn_Save.Enabled = false;
         tuneViewTable.NameTable = ((ComboBox)sender).SelectedItem.ToString();
      }

      /// <summary>
      /// Сохранить конфигурацию в БД
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      //private void btn_Save_Click(object sender, EventArgs e)
      //{
      //   if(SafeDataChange())
      //      btn_Save.Enabled = false;
      //}

      private void dgv_TuneColumn_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
         //if ( tuneViewTable == null || tuneViewTable.ProgFillDataControl ) return;
         //tuneViewTable.NeedSaveData = true;
      }

      private void TuneTableColumn_Shown(object sender, EventArgs e)
      {
         if (tuneViewTable.NeedSaveTables != "")
            tuneViewTable.NeedSaveTables = tuneViewTable.NeedSaveTables.TrimEnd(',');
         else
            return;
         foreach (string NameTable in tuneViewTable.NeedSaveTables.Split(','))
         {
            tuneViewTable.NeedSaveData = true;
            SaveDataChange(false, NameTable);
         }
         tuneViewTable.NeedSaveTables = "";
      }

      private void dgv_TuneColumn_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
      {
         if (tuneViewTable == null || tuneViewTable.ProgFillDataControl) return;
         tuneViewTable.NeedSaveData = true;
      }

      private void dgv_TuneColumn_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
      {
         DialogResult result = 
         MessageBox.Show(text: "Изменить все данные текущей строки?",
                         caption: "Информация",
                         icon: MessageBoxIcon.Question,
                         buttons: MessageBoxButtons.YesNo,
                         defaultButton: MessageBoxDefaultButton.Button1);
         if(result == DialogResult.Yes)
         {
            bool bool_state = (bool)((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value;
            foreach (DataGridViewCell cell in ((DataGridView)sender).Rows[e.RowIndex].Cells)
            {
               cell.Value =! bool_state;
            }
         }
      }

      private void FormTuneTableColumn_FormClosed(object sender, FormClosedEventArgs e)
      {
         SaveDataChange(true, tuneViewTable.NameTable);
      }

      private void btn_Reset_Click(object sender, EventArgs e)
      {
         DialogResult res = MessageBox.Show(text: "Сбросить конфигурацию отображения"+
                                                   "\nтаблицы: " + tuneViewTable.NameTable + 
                                                   "\nв базе данных: " + tuneViewTable.NameBase + "?",
                         caption: "Информация",
                         buttons: MessageBoxButtons.YesNo,
                         icon: MessageBoxIcon.Information,
                         defaultButton: MessageBoxDefaultButton.Button1);
         if (res == DialogResult.Yes)
         {
            tuneViewTable.DicStrTuneTabCol[tuneViewTable.NameTable] = "";
            tuneViewTable.SetDBFromStrTuneTabCol();
            tuneViewTable.UpDate(tuneViewTable.NameTable, tsmTune_Sort.CheckState== CheckState.Checked);
         }
      }

      private void btn_Save_Click(object sender, EventArgs e)
      {
         SaveDataChange(false, tuneViewTable.NameTable);
      }

      private void btn_SaveAll_Click(object sender, EventArgs e)
      {
         SaveDataChange(true, tuneViewTable.NameTable);
      }

      private void toolTip_Popup(object sender, PopupEventArgs e)
      {

      }

      private void dgv_TuneColumn_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
      {
         DataGridView dgView = sender as DataGridView;
         int Width = dgView.Columns[dgView.CurrentCell.ColumnIndex].Width;
         foreach (DataGridViewColumn column in dgView.SelectedColumns)
            column.Width = Width;
      }

      private void dgv_TuneColumn_CellClick(object sender, DataGridViewCellEventArgs e)
      {
         if (tuneViewTable == null || tuneViewTable.ProgFillDataControl) return;

         DataGridView table = ((DataGridView)sender);
         DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)table.CurrentCell;

         cell.Value = !(bool)cell.Value;
         table.EndEdit();
         tuneViewTable.NeedSaveData = true;
         //SendKeys.Send("{TAB}");
      }

      private void rbnRowSelect_CheckedChanged(object sender, EventArgs e)
      {
         dgv_TuneColumn.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
      }

      private void rbnColSelect_CheckedChanged(object sender, EventArgs e)
      {
         dgv_TuneColumn.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;

      }
      private bool tsmCheckState(object sender)
      {
         ToolStripMenuItem tsmItem = sender as ToolStripMenuItem;
         if (tsmItem.CheckState != CheckState.Checked) {
            tsmItem.CheckState = CheckState.Checked; 
            return true;}
         else { 
            tsmItem.CheckState = CheckState.Unchecked;
            return false;}
      }
      private void tsmTune_Sort_Click(object sender, EventArgs e)
      {
         tsmCheckState(sender);
      }

      private void tsmTune_SelCol_Click(object sender, EventArgs e)
      {
         if (tsmCheckState(sender))
         {
            dgv_TuneColumn.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
            tsmTune_SelRow.CheckState = CheckState.Unchecked;
         }
         else
         {
            dgv_TuneColumn.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            tsmTune_SelRow.CheckState = CheckState.Checked;
         }
      }

      private void tsmTune_SelRow_Click(object sender, EventArgs e)
      {
         if (tsmCheckState(sender))
         {
            dgv_TuneColumn.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            tsmTune_SelCol.CheckState = CheckState.Unchecked;
         }
         else
         {
            dgv_TuneColumn.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
            tsmTune_SelCol.CheckState = CheckState.Checked;
         }
      }
   }

   /// <summary>
   /// Настройка отображения таблиц (Порядок, видимость столбцов)
   /// из ранее сохранённых настроек либо из данных текущего подключения к БД 
   /// </summary>
   public class TuneViewTable
   {
      public Query query;
      /// <summary>
      /// Словарь строковых настроек таблиц [Имя таблицы][Настройка]
      /// </summary>
      public Dictionary<string, string> DicStrTuneTabCol  = new Dictionary<string, string>();
      public Dictionary<string, string> DicStrColumsOrder = new Dictionary<string, string>();
      public List<SetViewBase> Settings = new List<SetViewBase>();
      public List<string> NamesBases = new List<string>();
      public List<string> NamesTables = new List<string>();
      public List<string> NamesColumns = new List<string>();
      public List<string> NamesColEmptyCellNotVis = new List<string>();
      /// <summary>
      /// Фдаг. Программное заполнение контролов данными
      /// </summary>
      public bool ProgFillDataControl { get { return fillDataControl; } }
      public string NeedSaveTables = "";
      private bool fillDataControl = false;
      private DataGridView dataGridView;
      private List<ComboBox> ComboBoxes;
      private List<Button> Buttons;
      private string nameBase = "";
      private string nameTable = "";
      private bool needSaveData = false;
      public bool NeedSaveData 
      {
         set
         {
            if (Buttons != null)
            {
               GetButton("Сохранить").Enabled = value;
               GetButton("Сохранить всё").Enabled = value;
            }
            needSaveData = value;
         }
         get{ return needSaveData; }
      }
      /// <summary>
      /// Чтение/задание текущей БД
      /// </summary>
      public string NameBase
      {
         get
         {
            return nameBase;
         }
         set
         {
            if (nameBase != value)
            {
               NamesTables.Clear();
               foreach (SetViewBase settingBase in Settings)
               {
                  if (settingBase.NameBase == value)
                  {
                     foreach (SetViewTable settingTable in settingBase.SetViewTables)
                     {
                        NamesTables.Add(settingTable.NameTable);
                     }
                     nameBase = value;
                     NameTable = NamesTables[0];
                     if (ComboBoxes != null)
                     {
                        fillDataControl = true;
                        ComboBox comboBox = ComboBoxes[0];
                        SetDataComboBox(ref comboBox);
                     }
                  }
               }
            }
         }
      }
      /// <summary>
      /// Чтение/задание текущей таблицы БД
      /// </summary>
      public string NameTable
      {
         get
         {
            return nameTable;
         }
         set
         {
            if (value != nameTable)
            {
               nameTable = value;
               SetNamesColumns();
               if (dataGridView != null)
               {
                  fillDataControl = true;
                  SetDataGridView();
               }
               //FillNamesColEmptyCell();
            }
         }
      }

      /// <summary>
      /// Получить настройки таблицы (SetViewTable) из настроек базы (SetViewBase) по имени таблицы
      /// </summary>
      /// <param name="NameTable"></param>
      /// <returns></returns>
      private SetViewTable GetSetViewTabFromSetViewBase(string NameTable)
      {
         SetViewTable result = null;
         foreach(SetViewBase setViewBase in Settings)
         {
            if (setViewBase.NameBase == NameBase)
            {
               foreach(SetViewTable setViewTable in setViewBase.SetViewTables)
               {
                  if (setViewTable.NameTable == NameTable )
                  {
                     return setViewTable;
                  }
               }
            }
         }
         return result;
      }

       
      /// <summary>
      /// Сохраняет строку настройки указанной таблицы в словарь строковых настроек таблиц
      /// </summary>
      /// <param name="NameTable"></param>
      public void SetStrTuneTabColFromSettings(string NameTable)
      {
         if ( DicStrTuneTabCol.ContainsKey(NameTable) )
         {
            DicStrTuneTabCol[NameTable] = GetStrTuneFromSetViewTab(GetSetViewTabFromSetViewBase(NameTable));
         }
      }

      private SetViewBase GetSetBaseFromSettings(string NameBase)
      {
         foreach(SetViewBase setViewBase in Settings)
         {
            if (setViewBase.NameBase == NameBase) return setViewBase;
         }
         return null;
      }

      private SetViewTable GetSetTabFromSettings(string NameTable)
      {
         foreach (SetViewBase setViewBase in Settings)
         {
            if (setViewBase.NameBase == nameBase)
            {
               foreach (SetViewTable setViewTable in setViewBase.SetViewTables)
               {
                  if (setViewTable.NameTable == NameTable) return setViewTable;
               }
            }
         }
         return null;
      }

      public bool UpDate(string NameTable, bool sort = false)
      {
         bool result = true;

         DataTable table = new DataTable();
         query.GetTable(NameTable, out table);
         SetViewTable setViewTable = GetSetTabFromDataTab(table, sort);
         GetSetTabFromSettings(NameTable).SetViewColumns = setViewTable.SetViewColumns;
         SetDataGridView();
         NeedSaveData = true;

         return result;
      }

      private Button GetButton( string NameButton)
      {
         Button button = new Button();
         foreach(Button btn in Buttons)
         {
            if (btn.Text == NameButton) return btn;
         }
         return button;
      }

      /// <summary>
      /// Сохранение настроек в свойство "TuneColumn" таблиц базы данных
      /// </summary>
      public void SetDBFromStrTuneTabCol(bool SaveAll = false, string NameTable=null)
      {
         if(SaveAll)
         {
            foreach(var setting in DicStrTuneTabCol)
            {
               if (query.SetExtProp(name_tab: setting.Key, name_prop: "TuneColumn" , val: setting.Value))
               {
                  NeedSaveData = false;
               }
            }
         }
         else
         {
            if (NameTable == null) NameTable = nameTable;
            if (query.SetExtProp(name_tab: NameTable, name_prop: "TuneColumn", val: DicStrTuneTabCol[NameTable]))
            {
               NeedSaveData = false;
            }
         }
         
      }

      /// <summary>
      /// Наполнение данными из сохранённых данных
      /// </summary>
      /// <param name="val"></param>
      public void SetSettingsFromStr()
      {
         Settings.Clear();
         if (DicStrTuneTabCol == null || DicStrTuneTabCol.Count == 0 ) return;

         SetViewBase SetBase = new SetViewBase();
         SetBase.NameBase = query.BaseName;
         foreach ( var data in DicStrTuneTabCol )
         {
            SetViewTable SetTable = GetSetTabFromStr(data.Value);
            if (SetTable.NameTable != null && SetTable.SetViewColumns.Count > 0)
            {
               if (CompareConfigAndTable(ref SetTable))
                  NeedSaveTables = NeedSaveTables + SetTable.NameTable + ",";
               if (SetTable.SetViewColumns.Count > 0)
                  SetBase.SetViewTables.Add(SetTable);
               else
               {
                  SetBase.SetViewTables.Remove(SetTable);
               }
            }
         }
         Settings.Add((SetViewBase)SetBase.Clone());

         NameBase = Settings[0].NameBase;
         NameTable = Settings[0].SetViewTables[0].NameTable;

         AddSetting(CheckNewTable());
         SetNamesColumns();
      }
      /// <summary>
      /// Сравнить состав колонок конфигурации с существующей таблицей,
      /// добавить, удалить при необходимости в конфигурацию.
      /// Возвращает 'true' если есть разница и были сделаны изменения в SVT
      /// </summary>
      /// <param name="SVT"></param>
      /// <returns></returns>
      private bool CompareConfigAndTable(ref SetViewTable SVT)
      {
         bool result = false;
         Query queryCurTab = new Query(query.GetStrConnect());
         DataTable tableCurTab = new DataTable();
         List<string> lstCurColNames = new List<string>();
         queryCurTab.GetNamesColumn(out lstCurColNames, SVT.NameTable);
         string strCurColNames = string.Join("|", lstCurColNames.ToArray());
         string strConfColNames="";
         // Удаление
         for (; ; )
         {
            bool ExitFor = true;
            foreach (SetViewColumn set in SVT.SetViewColumns)
            {
               if (!strCurColNames.Contains(set.ColNam))
               {
                  SVT.SetViewColumns.Remove(set);
                  result = true;
                  ExitFor = false;
                  break;
               }
               strConfColNames = strConfColNames +"|" + set.ColNam;
            }
            if (ExitFor || SVT.SetViewColumns.Count==0) break;
         }
         // Добавление
         foreach(string ColName in lstCurColNames){
            if(!strConfColNames.Contains(ColName)){
               SetViewColumn col = new SetViewColumn() { ColNam= ColName, ColVis=false, RowVis=true };
               SVT.SetViewColumns.Add(col);
               result = true;
            }
         }
         return result;
      }

      private bool AddSetting(DataSet dataSet)
      {
         bool result = false;
         if (dataSet.Tables.Count > 0)
         {
            Settings.Add(GetSetBaseFromDataSet(dataSet));
            result = true;
         }
         return result;
      }

      /// <summary>
      /// Проверка наличия новых таблиц, которых нет в ранее сохранённых настройках 
      /// </summary>
      /// <returns></returns>
      private DataSet CheckNewTable()
      {
         DataSet CurDataSet = new DataSet();
         DataSet NewDataSet = new DataSet();
         query.GetTablesBase(ref CurDataSet);
         foreach (DataTable dataTable in CurDataSet.Tables)
         {
            foreach (SetViewBase setting in Settings)
            {
               if (setting.NameBase == NameBase)
               {
                  bool NewTable = true;
                  foreach ( SetViewTable table in setting.SetViewTables )
                  {
                     if (table.NameTable == dataTable.Namespace)
                     {
                        NewTable = false;
                     }
                  }
                  if (NewTable)
                  {
                     NewDataSet.Tables.Add(Ctrl.CopyDataTable(dataTable));
                  }
               }
            }
         }
         return NewDataSet;
      }

      /// <summary>
      /// Получить строку настройки таблицы из структуры настройки(SettingViewTable)
      /// </summary>
      /// <param name="setting"></param>
      /// <returns></returns>
      private string GetStrTuneFromSetViewTab(SetViewTable setting)
      {
         string result = "";

         if (setting == null ) return result;

         result = "(TabNam=" + setting.NameTable;
         foreach (SetViewColumn settingColumn in setting.SetViewColumns)
         {
            result = result + ",";
            result = result + "ColNam=" + settingColumn.ColNam + ",";
            result = result + "ColVis=" + settingColumn.ColVis.ToString().ToLower() + ",";
            result = result + "RowVis=" + settingColumn.RowVis.ToString().ToLower();
         }
         return result + ")";
      }
      /// <summary>
      /// Получить строку порядка столбцов таблицы из структуры настройки(SettingViewTable)
      /// </summary>
      /// <param name="setting"></param>
      /// <returns></returns>
      private string GetStrColumnsOrderFromSetViewTab(SetViewTable setting)
      {
         string result = "";

         if (setting == null) return result;
         foreach (SetViewColumn settingColumn in setting.SetViewColumns)
         {
            result = result + settingColumn.ColNam + ";";
         }
         result.Remove(result.Length-1,1);

         return result;
      }

      /// <summary>
      /// Получить настройки таблицы (SettingViewTable) из строки
      /// </summary>
      /// <param name="StrTuneColumn"></param>
      /// <returns></returns>
      private SetViewTable GetSetTabFromStr(string StrTuneColumn)
      {
         SetViewTable settingTable = new SetViewTable();

         Regex regexNameTable = new Regex(@"(?<=TabNam=)\w+"); // Шаблон по имени таблице
         MatchCollection matchesNameTable = regexNameTable.Matches(StrTuneColumn);
         if (matchesNameTable.Count > 0)
         {
            foreach (Match matchNameTable in matchesNameTable)
            {
               settingTable.NameTable = matchNameTable.Value; // Имя таблицы данных
            }
         }

         Regex regexColName = new Regex(@"(?<=ColNam=)[^\,]+");
         MatchCollection matchColNam = regexColName.Matches(StrTuneColumn);
         Regex regexColVis = new Regex(@"(?<=ColVis=)\w+");
         MatchCollection matchColVis = regexColVis.Matches(StrTuneColumn);
         Regex regexRowVis = new Regex(@"(?<=RowVis=)\w+");
         MatchCollection matchRowVis = regexRowVis.Matches(StrTuneColumn);

         for (int i = 0; i < matchColNam.Count; i++)
         {
            SetViewColumn settingColumn = new SetViewColumn();

            settingColumn.ColNam = matchColNam[i].Value;
            settingColumn.ColVis = matchColVis[i].Value == "true";
            settingColumn.RowVis = matchRowVis[i].Value == "true";

            settingTable.SetViewColumns.Add((SetViewColumn)settingColumn.Clone());
         }
         return settingTable;
      }

      /// <summary>
      /// Получает начальные настройки таблицы (SettingViewTable) из таблицы SQL
      /// </summary>
      /// <param name="dataTable"></param>
      /// <returns></returns>
      private SetViewTable GetSetTabFromDataTab(DataTable dataTable, bool sort = false)
      {
         SetViewTable SVT = new SetViewTable();
         // Признок экземплярной таблицы
         bool InstBody = dataTable.Namespace.IndexOf("InstBody") >= 0;
         // Список базовых столбцов для отображения
         List<string> BaseNameCol = new List<string>() { "Num", "Descript", "Pos"};

         SVT.NameTable = dataTable.Namespace;
         List<string> NamesCol = new List<string>();
         foreach (DataColumn dataColumn in dataTable.Columns){
            NamesCol.Add(dataColumn.ColumnName);
         }  
         if (sort) NamesCol.Sort();

         foreach (string NameCol in NamesCol)
         {
            SetViewColumn SVC = new SetViewColumn();
            SVC.ColNam = NameCol;
            bool ColVis = BaseNameCol.Contains(NameCol);
            SVC.ColVis = !InstBody | (ColVis & InstBody);
            SVC.RowVis = true;
            SVT.SetViewColumns.Add((SetViewColumn)SVC.Clone());
         }
         return SVT;
      }

      /// <summary>
      /// Получить настройки таблиц базы (SettingViewTable) из набора таблиц
      /// </summary>
      /// <param name="dataSet"></param>
      /// <returns></returns>
      private SetViewBase GetSetBaseFromDataSet(DataSet dataSet)
      {
         SetViewBase settingBase = new SetViewBase();
         settingBase.NameBase = query.BaseName;
         foreach (DataTable dataTable in dataSet.Tables)
         {
            SetViewTable settingTable = GetSetTabFromDataTab(dataTable);
            settingBase.SetViewTables.Add((SetViewTable)settingTable.Clone());
         }
         return settingBase;
      }
      /// <summary>
      /// Наполнение данными из текущего подключения к базе данных
      /// </summary>
      /// <param name="query"></param>
      public void SetSettingsFromDataSet()
      {
         Settings.Clear();
         DataSet dataSet = new DataSet();
         query.GetTablesBase(ref dataSet);
         Settings.Add(GetSetBaseFromDataSet(dataSet));
      }

      /// <summary>
      /// Наполняет словарь строковых настроек отображения столбцов таблиц
      /// </summary>
      /// <returns></returns>
      private bool SetStrTuneTabCol()
      {
         bool result = false;
         // Очистить словарь
         DicStrTuneTabCol.Clear();
         // Список таблиц одной базы данных
         List<string> NameTables = new List<string>();
         query.GetNamesTable(ref NameTables);

         foreach ( string NameTable in NameTables )
         {
            try
            {
               DataTable dataTable;
               string val = query.GetExtProp(out dataTable, name_tab: NameTable, name_prop: "TuneColumn");
               SetViewTable SetTable = GetSetTabFromStr(val);

               // Проверка на достоверную конфигурацию
               if (SetTable.NameTable != null && SetTable.SetViewColumns.Count > 0)
               {
                  // После переименований таблиц осталась старая конфигурация
                  if (SetTable.NameTable != NameTable)
                  {
                     SetTable.NameTable = NameTable;
                     val = GetStrTuneFromSetViewTab(SetTable);
                     NeedSaveTables = NeedSaveTables + SetTable.NameTable + ",";
                  }
                  DicStrTuneTabCol.Add(NameTable, val);
                  result = true;
               }
               else{
                  DataTable table = new DataTable();
                  query.GetTable(NameTable, out table);
                  val = GetStrTuneFromSetViewTab(GetSetTabFromDataTab(table));
                  DicStrTuneTabCol.Add(NameTable, val);
                  nameTable = NameTable;
                  SetDBFromStrTuneTabCol();
                  result = true;
                  //NeedSaveData = true;
               }
            }
            catch 
            {}
         }
         return result;
      }
      
      /// <summary>
      /// Конструктор
      /// </summary>
      /// <param name="strTune"></param>
      public TuneViewTable(DataGridView DataGridView = null, 
                           List<ComboBox> ComboBoxs = null, 
                           Query query = null,
                           List<Button> Buttons = null)
      {
         this.dataGridView = DataGridView;
         this.ComboBoxes = ComboBoxs;
         this.Buttons = Buttons;
         this.nameBase = query?.BaseName;
         this.query = query;
        
         bool set_control = false;
         SetStrTuneTabCol();
         if (DicStrTuneTabCol != null && DicStrTuneTabCol.Count > 0)
         {
            // Наполнение настроек из ранее сохранённых настроек
            SetSettingsFromStr();
            set_control = true;
         }
         else if (query != null)
         {
            // Наполнение настроек из текущей базы данных
            SetSettingsFromDataSet();
            set_control = true;
         }
         else
         {
            MessageBox.Show(text: "Нет сохранённых данных для настройки и нет подключения к БД!",
                            caption: "Ошибка",
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Error,
                            defaultButton: MessageBoxDefaultButton.Button1);
         }
         if (this.ComboBoxes != null && set_control)
         {
            ComboBox box = ComboBoxes[0];
            SetDataComboBox(ref box);
         }
         if (this.dataGridView != null && set_control)
         {
            SetDataGridView();
         }
      }

      /// <summary>
      /// Задать данные в таблицу
      /// </summary>
      /// <param name="table"></param>
      public void SetDataGridView()
      {
         // Выход если нет данных
         if (dataGridView == null || Settings == null || Settings.Count == 0 || NameBase == null || NameTable == null) return;
         // Программное редактирование
         fillDataControl = true;
         // Очистка данных
         dataGridView.Rows.Clear();
         dataGridView.Columns.Clear();
         // Проход по всем базам
         foreach (SetViewBase settingBase in Settings)
         {
            // Нахождение выбранной базы
            if (settingBase.NameBase == nameBase)
            {
               // Проход по всем таблицам выбранной базы
               foreach (SetViewTable settingTable in settingBase.SetViewTables)
               {
                  // Нахождени выбранной таблицы
                  if (settingTable.NameTable == nameTable)
                  {
                     // Проход по всем столбцам выбранной таблицы
                     foreach (SetViewColumn settingColumn in settingTable.SetViewColumns)
                     {
                        // Добавляем столбцы
                        var column = new DataGridViewCheckBoxColumn();
                        column.Name = settingColumn.ColNam; // Имя
                        column.HeaderText = settingColumn.ColNam; // Имя
                        column.ValueType = typeof(bool);
                        column.CellTemplate = new DataGridViewCheckBoxCell();// Тип
                        column.DefaultCellStyle.Font = new Font("Tahoma", 10);
                        dataGridView.Columns.Add(column);
                     }
                     // Добавляем строки
                     var row = new DataGridViewRow();
                     row.HeaderCell.Value = "Видимость столбца";
                     dataGridView.Rows.Add(row);
                     row = new DataGridViewRow();
                     row.HeaderCell.Value = "Видимость строки при пустом значении ячейки";
                     dataGridView.Rows.Add(row);

                     // Добавление данных (инициализация)
                     for (int i = 0; i < settingTable.SetViewColumns.Count; i++)
                     {
                        dataGridView.Rows[0].Cells[i].Value = settingTable.SetViewColumns[i].ColVis;
                        dataGridView.Rows[1].Cells[i].Value = settingTable.SetViewColumns[i].RowVis;
                     }
                  }
               }
            }
            fillDataControl = false;
         }

         // Настройка таблицы
         dataGridView.RowHeadersWidth = 360;
         dataGridView.AllowUserToAddRows = false;
         dataGridView.RowHeadersDefaultCellStyle.Font = new Font("Tahoma", 10);

         // Программное редактирование
         fillDataControl = false;
      }

      /// <summary>
      /// Получить данные из таблицы
      /// </summary>
      public void GetDataGridView()
      {
         SetViewTable settingTableNew = new SetViewTable();
         settingTableNew.NameTable = NameTable;
         for (int i = 0; i < dataGridView.Columns.Count; i++)
         {
            SetViewColumn settingColumn = new SetViewColumn();
            for (int m = 0; m < dataGridView.Columns.Count; m++)
            {
               if (i == dataGridView.Columns[m].DisplayIndex)
               {
                  settingColumn.ColNam = dataGridView.Columns[m].HeaderText;
                  settingColumn.ColVis = (bool)dataGridView.Rows[0].Cells[m].Value;
                  settingColumn.RowVis = (bool)dataGridView.Rows[1].Cells[m].Value;
                  settingTableNew.SetViewColumns.Add(settingColumn);
               }
            }
         }
         foreach (SetViewBase settingBase in Settings)
         {
            if (settingBase.NameBase == NameBase)
            {
               foreach (SetViewTable settingTable in settingBase.SetViewTables)
               {
                  if (settingTable.NameTable == NameTable)
                  {
                     settingTable.SetViewColumns = settingTableNew.SetViewColumns;
                     break;
                  }
               }
            }
         }
      }

      /// <summary>
      /// Задать данные в ComboBox
      /// </summary>
      /// <param name="comboBox"></param>
      private void SetDataComboBox(ref ComboBox comboBox)
      {
         if (comboBox == null) return;
         if (comboBox.Name == "cbx_BaseName")
         {
            List<string> namesBase = new List<string>();
            foreach (SetViewBase settingBase in Settings)
            {
               namesBase.Add(settingBase.NameBase);
            }
            Ctrl.Fill(ref comboBox, namesBase, clear: true);
            string namebase = comboBox.SelectedItem.ToString();
            try
            {
               NameBase = namebase;
            }
            catch (Exception ex)
            {
               MessageBox.Show(text: ex.Message,
                                  caption: "Ошибка",
                                  buttons: MessageBoxButtons.OKCancel,
                                  icon: MessageBoxIcon.Error,
                                  defaultButton: MessageBoxDefaultButton.Button1);
            }
         }
         else if (comboBox.Name == "cbx_TableName")
         {
            List<string> namesTable = new List<string>();
            foreach (SetViewBase settingBase in Settings)
            {
               if (settingBase.NameBase == NameBase)
               {
                  foreach (SetViewTable settingTable in settingBase.SetViewTables)
                  {
                     namesTable.Add(settingTable.NameTable);
                  }
               }
            }
            Ctrl.Fill(ref comboBox, namesTable, clear: true);
            NameTable = comboBox.SelectedItem?.ToString();
            fillDataControl = false;
         }
      }

      /// <summary>
      /// Наполнение списка имён колонок
      /// </summary>
      private void SetNamesColumns()
      {
         if (Settings == null) return;
         foreach (SetViewBase bas in Settings)
         {
            if (bas.NameBase == nameBase)
            {
               foreach (SetViewTable tab in bas.SetViewTables)
               {
                  if (tab.NameTable == nameTable)
                  {
                     NamesColumns.Clear();
                     NamesColEmptyCellNotVis.Clear();
                     foreach (SetViewColumn col in tab.SetViewColumns)
                     {
                        if (col.ColVis)
                           NamesColumns.Add(col.ColNam);
                        if (!col.RowVis)
                           NamesColEmptyCellNotVis.Add(col.ColNam);
                     }
                  }
               }
            }
         }
      }
   }

   /// <summary>
   /// Структура настройки столбца
   /// </summary>
   public class SetViewColumn : ICloneable
   {
      public string ColNam;
      public bool ColVis;
      public bool RowVis;
      public SetViewColumn(SetViewColumn SVC)
      {
         ColNam = SVC.ColNam;
         ColVis = SVC.ColVis;
         RowVis = SVC.RowVis;
      }
      public object Clone()
      {
         return MemberwiseClone();
      }
      public SetViewColumn()
      {
      }
   }

   /// <summary>
   /// Структура настройки таблицы
   /// </summary>
   public class SetViewTable : ICloneable
   {
      public string NameTable;
      public List<SetViewColumn> SetViewColumns = new List<SetViewColumn>();
      public object Clone()
      {
         return MemberwiseClone();
      }
   }

   /// <summary>
   /// Структура настройки БД
   /// </summary>
   public class SetViewBase : ICloneable
   {
      public string NameBase;
      public List<SetViewTable> SetViewTables = new List<SetViewTable>();
      public object Clone()
      {
         return MemberwiseClone();
      }
   }
}
