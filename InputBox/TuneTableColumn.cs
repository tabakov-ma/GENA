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
      public string StrTuneTableColumn
      {
         get
         {
            return tuneViewTable.strTuneTableColumn;
         }
      }

      /// <summary>
      /// Конструктор формы
      /// </summary>
      /// <param name="strTuneTableColumn"></param>
      /// <param name="objQuery"></param>
      public FormTuneTableColumn( Query objQuery)
      {
         InitializeComponent();
         query = objQuery;
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
                                              query);
            if (tuneViewTable.strTuneTableColumn == null || tuneViewTable.strTuneTableColumn == "")
            {
               MessageBox.Show(text: "Нет ранее сохранённых настроек.",
                               caption: "Предупреждение",
                               buttons: MessageBoxButtons.OK,
                               icon: MessageBoxIcon.Warning,
                               defaultButton: MessageBoxDefaultButton.Button1);
               
               tuneViewTable.NeedSaveData = true;
            }
            lblBaseName.Text = query.BaseName;
            dgv_TuneColumn.DefaultCellStyle.Font = new Font("Tahoma", 10);
         }
         catch(Exception ex)
         {
            MessageBox.Show(text: ex.Message + "\n" + ex.TargetSite,
                                  caption: "Ошибка",
                                  buttons: MessageBoxButtons.OK,
                                  icon: MessageBoxIcon.Error,
                                  defaultButton: MessageBoxDefaultButton.Button1);
         }
      }

      /// <summary>
      /// Сохранение настроек в БД
      /// </summary>
      /// <param name="change"></param>
      private bool SafeDataChange()
      {
         bool result = false;
         if (tuneViewTable.NeedSaveData)
         {
            DialogResult res = MessageBox.Show(text: "Сохранить конфигурацию \nотображения таблиц в базе данных: "+ tuneViewTable.NameBase + "?",
                            caption: "Информация",
                            buttons: MessageBoxButtons.YesNo,
                            icon: MessageBoxIcon.Information,
                            defaultButton: MessageBoxDefaultButton.Button1);
            if (res == DialogResult.Yes)
            {
               // Получить настройки из таблицы
               tuneViewTable.GetDataTable();
               // Сохранить настройки отображения колонок (Порядок и видимость) из БД
               tuneViewTable.SetSettingToDB();
               result = true;
            }
            
         }
         else
         {
            //MessageBox.Show(text: "Изменённых данных нет, \n сохранение данных не будет!",
            //                caption: "Предупреждение",
            //                buttons: MessageBoxButtons.YesNo,
            //                icon: MessageBoxIcon.Warning,
            //                defaultButton: MessageBoxDefaultButton.Button1);
         }
         return result;
      }
      private void cbx_TableName_SelectedIndexChanged(object sender, EventArgs e)
      {
         if ( tuneViewTable == null || tuneViewTable.ProgFillDataControl) return;
         SafeDataChange();
         tuneViewTable.NameTable = ((ComboBox)sender).SelectedItem.ToString();
      }

      /// <summary>
      /// Сохранить конфигурацию в БД
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btn_Save_Click(object sender, EventArgs e)
      {
         if(SafeDataChange())
            btn_Save.Enabled = false;
      }

      private void dgv_TuneColumn_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
         if ( tuneViewTable == null || tuneViewTable.ProgFillDataControl ) return;
         tuneViewTable.NeedSaveData = true;
         btn_Save.Enabled = true;
      }

      private void TuneTableColumn_Shown(object sender, EventArgs e)
      {

      }

      private void dgv_TuneColumn_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
      {
         if (tuneViewTable == null || tuneViewTable.ProgFillDataControl) return;
         tuneViewTable.NeedSaveData = true;
         btn_Save.Enabled = true;
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
         SafeDataChange();
      }

      private void btnRemoveConfig_Click(object sender, EventArgs e)
      {
         tuneViewTable.strTuneTableColumn = "";
      }
   }

   /// <summary>
   /// Настройка отображения таблиц (Порядок, видимость столбцов)
   /// из ранее сохранённых настроек либо из данных текущего подключения к БД 
   /// </summary>
   public class TuneViewTable
   {
      public Query query;
      public string strTuneTableColumn;
      public List<SettingViewBase> Settings = new List<SettingViewBase>();
      public List<string> NamesBases = new List<string>();
      public List<string> NamesTables = new List<string>();
      public List<string> NamesColumns = new List<string>();
      public List<string> NamesColEmptyCellNotVis = new List<string>();
      /// <summary>
      /// Фдаг. Программное заполнение контролов данными
      /// </summary>
      public bool ProgFillDataControl { get { return fillDataControl; } }
      private bool fillDataControl = false;
      private DataGridView dataGridView;
      private List<ComboBox> comboBoxes;
      private string nameBase = "";
      private string nameTable = "";
      private bool needSaveData = false;
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
               foreach (SettingViewBase settingBase in Settings)
               {
                  if (settingBase.Namebase == value)
                  {
                     foreach (SettingViewTable settingTable in settingBase.SettingTables)
                     {
                        NamesTables.Add(settingTable.NameTable);
                     }
                     nameBase = value;
                     NameTable = NamesTables[0];
                     if (comboBoxes != null)
                     {
                        fillDataControl = true;
                        ComboBox comboBox = comboBoxes[0];
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
               FillNamesColumns();
               if (dataGridView != null)
               {
                  fillDataControl = true;
                  SetDataTable();
               }
               //FillNamesColEmptyCell();
            }
         }
      }
      /// <summary>
      /// Флаг. Необходимо сохранить данные
      /// </summary>
      public bool NeedSaveData { get{ return needSaveData; } set{ needSaveData = value; } }

      /// <summary>
      /// Сохранение настроек в строковую переменную
      /// </summary>
      public void SetSettingToStr()
      {
         if (Settings == null || Settings.Count == 0) return;

         List<string> _lstTuneTableColumn = new List<string>();

         foreach (SettingViewBase settingBase in Settings)
         {
            string _strTuneTableColumn = "";
            _strTuneTableColumn = "{BasNam=" + settingBase.Namebase;
            foreach (SettingViewTable settingTable in settingBase.SettingTables)
            {
               _strTuneTableColumn = _strTuneTableColumn + "(TabNam=" + settingTable.NameTable;
               foreach (SettingViewColumn settingColumn in settingTable.SettingColumns)
               {
                  _strTuneTableColumn = _strTuneTableColumn + ",";
                  _strTuneTableColumn = _strTuneTableColumn + "ColNam=" + settingColumn.ColNam + ",";
                  _strTuneTableColumn = _strTuneTableColumn + "ColVis=" + settingColumn.ColVis.ToString().ToLower() + ",";
                  _strTuneTableColumn = _strTuneTableColumn + "RowVis=" + settingColumn.RowVis.ToString().ToLower();
               }
               _strTuneTableColumn = _strTuneTableColumn + ")";
            }
            _strTuneTableColumn = _strTuneTableColumn + "}";
            _lstTuneTableColumn.Add(_strTuneTableColumn);
         }

         // Проверка наличия в ранее сохранённых настройках
         if (strTuneTableColumn != "")
         {
            int i = 0;
            foreach (SettingViewBase settingViewBase in Settings)
            {
               Regex regexBases = new Regex(@"\{BasNam=" + settingViewBase.Namebase + @"[\w,=\(\)_]*\}");
               MatchCollection matchesBases = regexBases.Matches(strTuneTableColumn);

               if (matchesBases.Count > 0)
               {
                  strTuneTableColumn = regexBases.Replace(strTuneTableColumn, _lstTuneTableColumn[i]);
               }
               else
               {
                  foreach (string TTC in _lstTuneTableColumn)
                  {
                     strTuneTableColumn = strTuneTableColumn + TTC;
                  }
               }
               i++;
            }
         }
         else
         {
            foreach (string TTC in _lstTuneTableColumn)
            {
               strTuneTableColumn = strTuneTableColumn + TTC;
            }
         }
      }

      /// <summary>
      /// Сохранение настроек в свойство "TuneColumn" базы данных
      /// </summary>
      public void SetSettingToDB()
      {
         if(query.SetExtPropDB(name_prop: "TuneColumn", val: strTuneTableColumn))
         {
            NeedSaveData = false;
         }

      }

      /// <summary>
      /// Наполнение данными из сохранённых данных
      /// </summary>
      /// <param name="val"></param>
      public void FillSettingsFromStr()
      {
         Settings.Clear();
         if (strTuneTableColumn == null) return;

         Regex regexBases = new Regex(@"\{[\w,=\(\)_]*\}"); // Шаблон данных по базам
         MatchCollection matchesBases = regexBases.Matches(strTuneTableColumn);

         if (matchesBases.Count > 0)
         {
            SettingViewBase settingBase = new SettingViewBase(); // Настройки одной базы данных

            foreach (Match matchBase in matchesBases)
            {
               settingBase.SettingTables = new List<SettingViewTable>();

               Regex regexNameBase = new Regex(@"(?<=BasNam=)\w+"); // Шаблон по имени базы
               MatchCollection matchesNameBase = regexNameBase.Matches(matchBase.Value);
               if (matchesNameBase.Count > 0)
               {
                  foreach (Match matchNameBase in matchesNameBase)
                  {
                     settingBase.Namebase = matchNameBase.Value; // Имя базы данных
                  }
               }

               Regex regexTables = new Regex(@"TabNam=[\w\,=_]*\)"); // Шаблон данных по таблицам
               MatchCollection matchesTables = regexTables.Matches(matchBase.Value);
               if (matchesTables.Count > 0)
               {
                  foreach (Match matchTable in matchesTables) // 
                  {
                     SettingViewTable settingTable = new SettingViewTable(); // Настройка одной таблицы

                     Regex regexNameTable = new Regex(@"(?<=TabNam=)\w+"); // Шаблон по имени таблице
                     MatchCollection matchesNameTable = regexNameTable.Matches(matchTable.Value);
                     if (matchesNameTable.Count > 0)
                     {
                        foreach (Match matchNameTable in matchesNameTable)
                        {
                           settingTable.NameTable = matchNameTable.Value; // Имя таблицы данных
                        }
                     }

                     Regex regexColName = new Regex(@"(?<=ColNam=)\w+");
                     MatchCollection matchColNam = regexColName.Matches(matchTable.Value);
                     Regex regexColVis = new Regex(@"(?<=ColVis=)\w+");
                     MatchCollection matchColVis = regexColVis.Matches(matchTable.Value);
                     Regex regexRowVis = new Regex(@"(?<=RowVis=)\w+");
                     MatchCollection matchRowVis = regexRowVis.Matches(matchTable.Value);

                     for (int i = 0; i < matchColNam.Count; i++)
                     {
                        SettingViewColumn settingColumn = new SettingViewColumn();

                        settingColumn.ColNam = matchColNam[i].Value;
                        settingColumn.ColVis = matchColVis[i].Value == "true";
                        settingColumn.RowVis = matchRowVis[i].Value == "true";

                        settingTable.SettingColumns.Add((SettingViewColumn)settingColumn.Clone());
                     }
                     settingBase.SettingTables.Add((SettingViewTable)settingTable.Clone());
                  }
               }
               Settings.Add((SettingViewBase)settingBase.Clone());
            }
            NameBase = Settings[0].Namebase;
            NameTable = Settings[0].SettingTables[0].NameTable;
            FillNamesColumns();
         }
      }

      /// <summary>
      /// Наполнение данными из текущего подключения к базе данных
      /// </summary>
      /// <param name="query"></param>
      public void FillSettingFromDataSet()
      {
         Settings.Clear();
         DataSet dataSet = new DataSet();
         query.GetTablesBase(ref dataSet);
         SettingViewBase settingBase = new SettingViewBase();
         settingBase.Namebase = query.BaseName;
         foreach (DataTable dataTable in dataSet.Tables)
         {
            SettingViewTable settingTable = new SettingViewTable();
            settingTable.NameTable = dataTable.Namespace;
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
               SettingViewColumn settingColumn = new SettingViewColumn();
               settingColumn.ColNam = dataColumn.ColumnName;
               settingColumn.ColVis = true;
               settingColumn.RowVis = true;
               settingTable.SettingColumns.Add((SettingViewColumn)settingColumn.Clone());
            }
            settingBase.SettingTables.Add((SettingViewTable)settingTable.Clone());
         }
         Settings.Add(settingBase);
      }

      /// <summary>
      /// Конструктор
      /// </summary>
      /// <param name="strTune"></param>
      public TuneViewTable(DataGridView dataGridView = null, List<ComboBox> comboBox = null, Query query = null)
      {
         this.dataGridView = dataGridView;
         this.comboBoxes = comboBox;
         this.nameBase = query?.BaseName;
         this.query = query;
         this.strTuneTableColumn = query.GetExtPropDB(name_prop: "TuneColumn");
         bool set_control = false;
         if (strTuneTableColumn != null && strTuneTableColumn != "")
         {
            // Наполнение настроек из ранее сохранённых настроек
            FillSettingsFromStr();
            set_control = true;
         }
         else if (query != null)
         {
            // Наполнение настроек из текущей базы данных
            FillSettingFromDataSet();
            set_control = true;
         }
         else
         {
            //MessageBox.Show(text: "Нет сохранённых данных для настройки и нет подключения к БД!",
            //                caption: "Ошибка",
            //                buttons: MessageBoxButtons.OK,
            //                icon: MessageBoxIcon.Error,
            //                defaultButton: MessageBoxDefaultButton.Button1);
         }
         if (this.comboBoxes != null && set_control)
         {
            ComboBox box = comboBoxes[0];
            SetDataComboBox(ref box);
         }
         if (this.dataGridView != null && set_control)
         {
            SetDataTable();
         }
      }

      /// <summary>
      /// Задать данные в таблицу
      /// </summary>
      /// <param name="table"></param>
      public void SetDataTable()
      {
         // Выход если нет данных
         if (dataGridView == null || Settings == null || Settings.Count == 0 || NameBase == null || NameTable == null) return;
         // Программное редактирование
         fillDataControl = true;
         // Очистка данных
         dataGridView.Rows.Clear();
         dataGridView.Columns.Clear();
         // Проход по всем базам
         foreach (SettingViewBase settingBase in Settings)
         {
            // Нахождение выбранной базы
            if (settingBase.Namebase == NameBase)
            {
               // Проход по всем таблицам выбранной базы
               foreach (SettingViewTable settingTable in settingBase.SettingTables)
               {
                  // Нахождени выбранной таблицы
                  if (settingTable.NameTable == NameTable)
                  {
                     // Проход по всем столбцам выбранной таблицы
                     foreach (SettingViewColumn settingColumn in settingTable.SettingColumns)
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
                     for (int i = 0; i < settingTable.SettingColumns.Count; i++)
                     {
                        dataGridView.Rows[0].Cells[i].Value = settingTable.SettingColumns[i].ColVis;
                        dataGridView.Rows[1].Cells[i].Value = settingTable.SettingColumns[i].RowVis;
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
      public void GetDataTable()
      {
         SettingViewTable settingTableNew = new SettingViewTable();
         settingTableNew.NameTable = NameTable;
         for (int i = 0; i < dataGridView.Columns.Count; i++)
         {
            SettingViewColumn settingColumn = new SettingViewColumn();
            for (int m = 0; m < dataGridView.Columns.Count; m++)
            {
               if (i == dataGridView.Columns[m].DisplayIndex)
               {
                  settingColumn.ColNam = dataGridView.Columns[m].HeaderText;
                  settingColumn.ColVis = (bool)dataGridView.Rows[0].Cells[m].Value;
                  settingColumn.RowVis = (bool)dataGridView.Rows[1].Cells[m].Value;
                  settingTableNew.SettingColumns.Add(settingColumn);
               }
            }
         }
         foreach (SettingViewBase settingBase in Settings)
         {
            if (settingBase.Namebase == NameBase)
            {
               foreach (SettingViewTable settingTable in settingBase.SettingTables)
               {
                  if (settingTable.NameTable == NameTable)
                  {
                     settingTable.SettingColumns = settingTableNew.SettingColumns;
                  }
               }
            }
         }
         SetSettingToStr();
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
            foreach (SettingViewBase settingBase in Settings)
            {
               namesBase.Add(settingBase.Namebase);
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
            foreach (SettingViewBase settingBase in Settings)
            {
               if (settingBase.Namebase == NameBase)
               {
                  foreach (SettingViewTable settingTable in settingBase.SettingTables)
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
      private void FillNamesColumns()
      {
         if (Settings == null) return;
         foreach (SettingViewBase bas in Settings)
         {
            if (bas.Namebase == nameBase)
            {
               foreach (SettingViewTable tab in bas.SettingTables)
               {
                  if (tab.NameTable == nameTable)
                  {
                     NamesColumns.Clear();
                     NamesColEmptyCellNotVis.Clear();
                     foreach (SettingViewColumn col in tab.SettingColumns)
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
}
