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
using WorkMisc;
using WorkString;

namespace WorkBox
{
   public partial class TableCreator : Form
   {
      public enum TableCreatorTabControl{ CREATOR, DATA}
      DataTable dtTune;
      Query query;
      Query queryEPorLinks;
      DataTable dtNamesCol;
      DataTable dtPropOrLinks = new DataTable();
      object old_sender = new object();
      List<DataRow> DelRowsPropOrLink = new List<DataRow>();
      /// <summary>
      /// Флаг. Программное изменеие данных
      /// </summary>
      bool flagProgDataChange = false;
      bool flagPermitRun = true;
      bool flagAddRow = false;
      bool VisBottomPanel = false;
      int HeightBottomPanel;
      string NameAddTable;
      string TempNameTabText;
      List<string> lstNameTab;
      List<string> TempDataCell = new List<string>() {""};
      List<string> FOREIGN_TABLE = new List<string>();
      Dictionary<string, string[]> dicRenameColumns = new Dictionary<string, string[]>();
      /// <summary>
      /// Словарь добавленных столбцов key: [ColumnName], value: [TableName, OldColumnName, RefTableName, RefColumnName]
      /// </summary>
      Dictionary<string, string[]> dicAddColumns = new Dictionary<string, string[]>();
      Dictionary<string, string[]> dicDelColumns = new Dictionary<string, string[]>();
      Dictionary<string, string[]> dicRenameType = new Dictionary<string, string[]>();
      Dictionary<string, string[]> dicAddForeignKey = new Dictionary<string, string[]>();
      TuneViewTable tuneViewTable;
      public TableCreator(Query query, TuneViewTable tuneViewTable, TableCreatorTabControl tab = TableCreatorTabControl.CREATOR, string TabName="")
      {
         InitializeComponent();
         this.query = new Query(query.GetStrConnect());
         this.Text = query.BaseName + ". Конструктор таблиц";
         Init();
         //Refresh();
         flagProgDataChange = true;
         toolTip1.SetToolTip(btnSave, "Сохранить изменения");
         toolTip1.SetToolTip(btnRead, "Запросить данные по всем таблицам из БД");
         toolTip1.SetToolTip(btnCreate, "Добавить таблицу");
         toolTip1.SetToolTip(btnRemove, "Удалить таблицу");
         toolTip1.SetToolTip(btnRename, "Переименовать таблицу");
         toolTip1.SetToolTip(btnClear, "Очистить строки таблицы");
         toolTip1.SetToolTip(btnNew, "Создать пустую форму");
         //toolTip1.SetToolTip(btnRPropTable, "Прочитать данные Extended Properties Table (текущей таблицы)");
         //toolTip1.SetToolTip(btnRLinks, "Прочитать данные Relationship (взаимосвязи таблиц)");
         //toolTip1.SetToolTip(btnRPropColumn, "Прочитать данные Extended Properties Columns (всех колонок текущей таблицы)");
         toolTip1.SetToolTip(btnPropSave, "Сохранить изменения");
         toolTip1.SetToolTip(btnExtProp, "Отобразить панель Relationship (взаимосвязи) и Extended Properties");
         toolTip1.SetToolTip(chbOffTuneCol, "Отключить настройки порядка и отображения колонок таблицы");

         HeightBottomPanel = 35;
         this.tuneViewTable = tuneViewTable;
         this.tbcMain.SelectedIndex = (int)tab;
         if (TabName !="") 
            this.cbxNameTab.SelectedItem = TabName;
         else{
            string dataTune = AccessProp.GetPropStr("TableCreator_Tune");
            dtTune = Str.GetTableFromStr(dataTune, MsgYes: false);
            try
            {
               this.cbxNameTab.SelectedItem = dtTune.Rows[0]["NameTable"].ToString();
            }
            catch { }
         }
      }
      #region FUNCTION

      private void Refresh(string NameTable = null, bool MsgYes=false, bool NewTable = false)
      {
         if (NameTable == null) NameTable = cbxNameTab.Text;
         flagProgDataChange = true;
         dgvData.Columns.Clear();
         dgvData.DataSource = null;
         dtNamesCol = new DataTable();
         DataTable dtData = new DataTable();
         if (tbcMain.SelectedTab.Name == "CREATOR")
         {

            if (this.query.GetTable(NameTable: cbxNameTab.Text,
                                    table: out dtData,
                                    MsgYes: MsgYes,
                                    AddConf: Query.GetTableAddConf.AllowDBNull) | NewTable)
            {
               dtNamesCol.Columns.Add("NAME_COLUMN");

               foreach (DataColumn column in dtData.Columns)
               {
                  //if(column.ColumnName != "ID")
                     dtNamesCol.Rows.Add(column.ColumnName);
               }
               dgvData.DataSource = dtNamesCol;

               DataGridViewCheckBoxColumn colCbx = new DataGridViewCheckBoxColumn();
               colCbx.Name = colCbx.HeaderText = "PRIMARY";
               colCbx.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
               colCbx.FalseValue = false;
               colCbx.TrueValue = true;
               colCbx.IndeterminateValue = false;
               dgvData.Columns.Add(colCbx);

               DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
               col.Name = col.HeaderText = "DATA_TYPE";
               col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
               col.DataSource = Misc.GetUnique(query.TypeSQL.Values.ToArray());
               dgvData.Columns.Add(col);

               colCbx = new DataGridViewCheckBoxColumn();
               colCbx.Name = colCbx.HeaderText = "ALLOW_NULL";
               colCbx.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
               colCbx.FalseValue = false;
               colCbx.TrueValue = true;
               colCbx.IndeterminateValue = false;
               dgvData.Columns.Add(colCbx);

               col = new DataGridViewComboBoxColumn();
               col.Name = col.HeaderText = "FOREIGN_TABLE";
               col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
               col.DataSource = lstNameTab;
               dgvData.Columns.Add(col);
            }
            // Определение ключевого столбца
            Query queryKey = new Query(query.GetStrConnect());
            string KeyNameCol = queryKey.GetPrimKeyTable(NameTable, MsgYes: false);
            // Определение связей 
            Query queryLink = new Query(query.GetStrConnect());
            DataTable table;
            List<string[]> LinkTable = queryLink.GetForeignKey(table: out table, NameTable: NameTable, type: Query.GetLinkTableType.CURR);
            foreach(string[] data in LinkTable)
            {
               if (NameTable == data[0])
               {
                  foreach (DataGridViewRow row in dgvData.Rows)
                  {
                     if (row.Cells["NAME_COLUMN"].Value?.ToString() == data[1])
                     {
                        flagProgDataChange = flagPermitRun = false;

                        row.Cells["FOREIGN_TABLE"].Value = data[2];
                        flagProgDataChange = flagPermitRun = true;
                        row.Cells[data[2].ToUpper() + "_FOREIGN_COLUMN"].Value = data[3];
                     }
                  }
               }
            }
            // Установка значений в таблицу
            bool old_ProgDataChange = flagProgDataChange;
            flagProgDataChange = true;
            int num = -1;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
               num++;
               if (row.Cells["NAME_COLUMN"].Value == null) continue;
               if (row.Cells["NAME_COLUMN"].Value.ToString() == KeyNameCol)
                  row.Cells["PRIMARY"].Value = true;
               row.Cells["DATA_TYPE"].Value = query.TypeSQL[dtData.Columns[num].DataType.Name];
               row.Cells["ALLOW_NULL"].Value = dtData.Columns[row.Cells["NAME_COLUMN"].Value.ToString()].Namespace;
            }
            flagProgDataChange = old_ProgDataChange;
         }
         else{ // Вкладка DATA
            dgvData.Name = cbxNameTab.Text;

            tuneViewTable.NameBase = query.BaseName;
            tuneViewTable.NameTable = cbxNameTab.Text;

            Ctrl.Fill(ref dgvData, query, ref dtNamesCol, 
           lstOrderColumns: chbOffTuneCol.Checked ? null : tuneViewTable.NamesColumns,
                     where: tbxWhere.Text);
         }
         flagProgDataChange = true;
         Ctrl.SetNumRowsDataGrid(ref dgvData);

         dgvData.Refresh();
         lblOut.Text = "Row count: " + (dgvData.Rows.Count-1).ToString();
         FormWidth();
         flagProgDataChange = false;
      }

      private void Init()
      {
         flagProgDataChange = true;
         lstNameTab = new List<string>();
         query.GetNamesTable(ref lstNameTab);
         
         Ctrl.Fill(ref cbxNameTab, lstNameTab, true);
         if (NameAddTable != null)
         {
            cbxNameTab.Text = NameAddTable;
         }

         ResSave();
         Ctrl.ButtonStar(btnCreate, false);
         flagProgDataChange = false;
      }


      # endregion FUNCTION
      private void btnCreate_Click(object sender, EventArgs e)
      {
         Button button = sender as Button;
         DialogResult result;
         result = MessageBox.Show(text: "Создать новую таблицу "+
                                  cbxNameTab.Text + "?",
                         caption: "Предупреждение",
                         buttons: MessageBoxButtons.YesNo,
                         icon: MessageBoxIcon.Question);
         
         if (result != DialogResult.Yes) return;


         List<string[]> lstTuneCol
            = Ctrl.GetListRowsFromTable(table: dgvData,
                                    AddColName: new string[] { "NAME_COLUMN", "DATA_TYPE", "PRIMARY", "ALLOW_NULL" },
                                    DataYes: 2,
                                    NotNull: false);

      List<string[]> lstForeign
         = Ctrl.GetListRowsFromTable(dgvData, ExcColName: new string[] { "PRIMARY","DATA_TYPE" }, DataYes: 3, NotNull: true);
      query.CreateTable(NameTable: cbxNameTab.Text,
                        TuneColumns: lstTuneCol,
                        ForeignColumns: lstForeign);

         Init();
         //Refresh(tbcMain.SelectedTab.Name, true);
         
      }

      private void btnRemove_Click(object sender, EventArgs e)
      {
         Button button = sender as Button;
         DialogResult result;
         result = MessageBox.Show(text: "Удалить таблицу " +
                                  cbxNameTab.Text + "?",
                         caption: "Предупреждение",
                         buttons: MessageBoxButtons.YesNo,
                         icon: MessageBoxIcon.Question);

         if (result != DialogResult.Yes) return;

         if (query.DelTable(cbxNameTab.Text))
         {
            NameAddTable = "";
            Init();
            Refresh(tbcMain.SelectedTab.Name);
         }
      }
      private void DoSave()
      {
         try
         {
            if (tbcMain.SelectedTab.Name == "CREATOR")
            {
               foreach (var dic in dicAddColumns)
               {
                  string[] row = Ctrl.GetListColumnsInOneRowTable(dgvData, dic.Key, GetNumColNull : 1);
                  string colName = row.Length > 0 ? row[0] : null;
                  string colPrim = row.Length > 1 ? row[1] : null;
                  string colType = row.Length > 2 ? row[2] : null;
                  string colNull = row.Length > 3 ? (row[3] == null ? "false" : row[3].ToString()) : "false";
                  string colFTName = row.Length > 4 ? row[4] : null;
                  string colFCName = row.Length > 5 ? row[5] : null;
                  if (!query.AddColumn(tabName: cbxNameTab.Text,
                                       colName: colName,
                                       colType: colType,
                                       NullYes: bool.Parse(colNull),
                                 ForeignTabCol: new string[] { colName,
                                                            colFTName,
                                                            colFCName }))
                     Refresh();
                  dicRenameColumns.Remove(dic.Key);
                  dicRenameType.Remove(dic.Key);
                  dicAddForeignKey.Remove(dic.Key);
                  dicDelColumns.Remove(dic.Key);
               }
               foreach (var dic in dicRenameColumns)
               {
                  if (!query.RenameColumn(dic.Value[0], dic.Value[1], dic.Key))
                     Refresh();
               }
               foreach (var dic in dicRenameType)
               {
                  if (!query.RenameColumnType(dic.Value[0], dic.Key, dic.Value[1]))
                     Refresh();
               }
               foreach (var dic in dicAddForeignKey)
               {
                  string[] row = Ctrl.GetListColumnsInOneRowTable(dgvData, dic.Key, GetNumColNull : -2, GetStartCol: 4);
                  if (!query.AddForeignKey(dic.Value[0], dic.Key, row[0], row[1]))
                     Refresh();
               }
               foreach (var dic in dicDelColumns)
               {
                  if (!query.DelColumn(dic.Value[0], dic.Key, null,true,true,true))
                     Refresh();
               }
               flagAddRow = false;
            }
            else
            {
               query.SetTable();
            }
         }
         catch(Exception ex){ Ctrl.MessErr(ex); }
         ResSave();

      }
      private void btnSave_Click(object sender, EventArgs e)
      {
         DoSave();
      }

      private void tbcMain_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (tbcMain.SelectedTab == null) return;
         
         Refresh();
         btnExtProp.Enabled = btnNew.Enabled = btnClear.Enabled = btnRename.Enabled = btnRemove.Enabled = btnCreate.Enabled = tbcMain.SelectedTab.Name == "CREATOR";
         chbOffTuneCol.Enabled = tbxWhere.Enabled = tbcMain.SelectedTab.Name != "CREATOR";
      }

      private void cbxNameTab_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (flagProgDataChange) return;
         NameAddTable = cbxNameTab.Text;
         Refresh();
         if (VisBottomPanel)
            ReadLinksAndProp();
      }
      private void dgvSelChekBox(object sender)
      {
         try
         {
            DataGridViewCheckBoxCell cell = ((DataGridView)sender).SelectedCells[0] as DataGridViewCheckBoxCell;
            if (cell == null) return;
            if (cell.DataGridView.Columns[cell.ColumnIndex].Name == "PRIMARY")
            {
               //   //if (cell.Value == null)
               //   //   cell.Value = true;
               //   //else 
               //   //   cell.Value = null;
               //   //cell.Value = cell.Selected;
               if ( cell.Value == null ) return;
               if ((bool)cell.Value == true)
               {
                  flagProgDataChange = true;
                  foreach (DataGridViewRow row in dgvData.Rows)
                     row.Cells["PRIMARY"].Value = false;
                  cell.Value = true;
                  flagProgDataChange = false;
               }
            }
         }
         catch{ }
      }
      private void dgvData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
      {
      }

      private void dgvData_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
      {
         DataGridView dGV= sender as DataGridView;
         TempDataCell.Add(dGV.SelectedCells[0].Value != null ? dGV.SelectedCells[0].Value.ToString() : "");
      }

      private void dgvData_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
      {
         if (flagProgDataChange) return;
         flagAddRow = true;
      }

      private void dgvData_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
      {
         if (flagProgDataChange) return;
         foreach (string data in TempDataCell)
         {
            try
            {
               dicDelColumns.Add(data, new string[] { cbxNameTab.Text, "" });
            }
            catch { }
            Ctrl.ButtonStar(btnSave);
         }
      }

      private void dgvData_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
      {

      }

      private void dgvData_SelectionChanged(object sender, EventArgs e)
      {
         if (flagProgDataChange) return;

         DataGridView dataGrid = sender as DataGridView;
         TempDataCell.Clear();
         foreach (DataGridViewCell cell in dataGrid.SelectedCells)
            if(cell!=null)
               if (dataGrid.Columns[cell.ColumnIndex].Name == "NAME_COLUMN")
                  TempDataCell.Add(cell.Value?.ToString());
      }

      private void TableCreator_Shown(object sender, EventArgs e)
      {
         Refresh();
         stc_H_SplitDist_Litle();
         flagProgDataChange = false;

         string dataTune = AccessProp.GetPropStr("TableCreator_Tune");
         dtTune = Str.GetTableFromStr(dataTune, MsgYes: false);
         try
         {
            this.Top = int.Parse(dtTune.Rows[0]["this.Top"].ToString());
            this.Left = int.Parse(dtTune.Rows[0]["this.Left"].ToString());
            this.Height = int.Parse(dtTune.Rows[0]["this.Height"].ToString());
            this.Width = int.Parse(dtTune.Rows[0]["this.Width"].ToString());
            //this.cbxNameTab.Text = dtTune.Rows[0]["NameTable"].ToString();
         }
         catch { }
      }

      private void btnRead_Click(object sender, EventArgs e)
      {
         Init();
         Refresh();
         if (VisBottomPanel)
            ReadLinksAndProp();
      }

      private void tbcMain_Click(object sender, EventArgs e)
      {
         CheckSave();
      }
      private void ResSave()
      {
         dicRenameColumns.Clear();
         dicRenameType.Clear();
         dicAddColumns.Clear();
         dicDelColumns.Clear();
         dicAddForeignKey.Clear();
         Ctrl.ButtonStar(btnSave, false);
      }
      private void CheckSave()
      {
         if (btnSave.Text.Contains("*"))
         {
            DialogResult result
               = MessageBox.Show(text: "Сохранить внесённые данные?",
                              caption: "Предупреждение",
                              buttons: MessageBoxButtons.YesNo,
                                 icon: MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
               DoSave();
            else
            {
               ResSave();
               Refresh();
            }
         }
      }

      private void cbxNameTab_Click(object sender, EventArgs e)
      {
         CheckSave();
         TempNameTabText = ((ComboBox)sender).Text;
      }

      private void tbcMain_Deselected(object sender, TabControlEventArgs e)
      {
         CheckSave();
      }

      private void dgvData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
         if (flagProgDataChange) return;
         dgvSelChekBox(sender);

         DataGridView dataGridView = sender as DataGridView;
         DataGridViewCell cell = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

         if (dataGridView == null || cell == null) return;

         if (tbcMain.SelectedTab.Name == "CREATOR")
         {
            if (dataGridView.Columns[cell.ColumnIndex].Name == "NAME_COLUMN")
            {
               if (TempDataCell[0] != cell.Value?.ToString())
               {
                  try
                  {
                     if (flagAddRow)
                     {
                        dicAddColumns.Add(cell.Value.ToString(),
                                  new string[] { cbxNameTab.Text,
                                                (dgvData.Rows[cell.RowIndex].Cells["DATA_TYPE"].Value==null ? null : dgvData.Rows[cell.RowIndex].Cells["DATA_TYPE"].Value.ToString()),
                                                (dgvData.Rows[cell.RowIndex].Cells["FOREIGN_TABLE"].Value==null ? null : dgvData.Rows[cell.RowIndex].Cells["FOREIGN_TABLE"].Value.ToString()),
                                               ((dgvData.Rows[cell.RowIndex].Cells["FOREIGN_TABLE"].Value==null || dgvData.Rows[cell.RowIndex].Cells[dgvData.Rows[cell.RowIndex].Cells["FOREIGN_TABLE"].Value.ToString().ToUpper()+"_FOREIGN_COLUMN"].Value==null)?null:dgvData.Rows[cell.RowIndex].Cells[dgvData.Rows[cell.RowIndex].Cells["FOREIGN_TABLE"].Value.ToString().ToUpper()+"_FOREIGN_COLUMN"].Value.ToString())}); 
                        flagAddRow = false;
                     }
                     else
                        dicRenameColumns.Add(cell.Value.ToString(), new string[] { cbxNameTab.Text, TempDataCell[0] });

                     Ctrl.ButtonStar(btnSave);
                  }
                  catch
                  { cell.Value = TempDataCell[0]; }
               }
            }
            // Добавление колонки со списком столбцов
            else if (dataGridView.Columns[cell.ColumnIndex].Name == "FOREIGN_TABLE")
            {
               string NamesTable = "";
               // Удаляем ненужные столбцы _FOREIGN_COLUMN
               foreach (DataGridViewRow row in dataGridView.Rows)
               {
                  if (row.Cells["FOREIGN_TABLE"].Value != null)
                     NamesTable = NamesTable + row.Cells["FOREIGN_TABLE"].Value.ToString().ToUpper()+"_"; 
               }
               for (; ; )
               {
                  bool exit = false;
                  int count = dataGridView.Columns.Count;
                  int curr = 0;
                  foreach (DataGridViewColumn column in dataGridView.Columns)
                  {
                     if (column.Name.Contains("_FOREIGN_COLUMN"))
                     {
                        if (!NamesTable.Contains(column.Name.Replace("FOREIGN_COLUMN", "")))
                        {
                           dataGridView.Columns.Remove(column);
                           break;
                        }
                     }
                     curr++;
                     if(count==curr) exit = true;
                  }
                  if (exit) break;
               }
               // Добавление столбцов
               DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
               col.HeaderText = col.Name = cell.Value.ToString().ToUpper() + "_FOREIGN_COLUMN";
               col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
               Query new_query = new Query(query.GetStrConnect());
               List<string> lstColumns = new List<string>();
               new_query.GetNamesColumn(out lstColumns, dgvData.Rows[cell.RowIndex].Cells["FOREIGN_TABLE"].Value.ToString());
               col.DataSource = lstColumns;
               dgvData.Columns.Add(col);

               // Изменение внешнего ключа
               string nameCol = dgvData.Rows[cell.RowIndex].Cells["NAME_COLUMN"].Value?.ToString();
               if (nameCol!=null && !dicAddColumns.ContainsKey(nameCol) && flagPermitRun)
               {
                  if(!dicAddForeignKey.ContainsKey(nameCol))
                     dicAddForeignKey.Add(nameCol, new string[] { cbxNameTab.Text });
                  Ctrl.ButtonStar(btnSave);
               }
            }
            // Изменение типа данных
            else if (dataGridView.Columns[cell.ColumnIndex].Name == "DATA_TYPE")
            {
               dicRenameType.Add(dataGridView.Rows[cell.RowIndex].Cells["NAME_COLUMN"].Value.ToString(),
                                 new string[] { cbxNameTab.Text, cell.Value.ToString()});
               Ctrl.ButtonStar(btnSave);
            }
         }
         else
         {
            if (TempDataCell.Count == 0 || TempDataCell[0] != cell.Value?.ToString())
            {
               if (!btnSave.Text.Contains("*")) btnSave.Text = btnSave.Text + "*";
            }
         }
      }

      private void dgvData_KeyPress(object sender, KeyPressEventArgs e)
      {
         if (e.KeyChar == 22)
         {
            flagProgDataChange = true;

            Ctrl ctrl = new Ctrl();
            ctrl.PasteInDataTableFromClipboard(ref dtNamesCol,
                                                CurRow: dgvData.SelectedCells[0].RowIndex,
                                                CurCol: dgvData.SelectedCells[0].ColumnIndex,
                                                AllowAddRows: true);
            
            Ctrl.ButtonStar(btnSave);
            Ctrl.SetNumRowsDataGrid(ref dgvData);
            dgvData.Refresh();
            
            flagProgDataChange = false;
         }
      }



      private void cbxNameTab_TextChanged(object sender, EventArgs e)
      {

      }

      private void btnRename_Click(object sender, EventArgs e)
      {
         DialogResult result = MessageBox.Show(text: "Переименовать таблицу " + TempNameTabText + " в " + cbxNameTab.Text + "?",
                         caption: "Предупреждение",
                         icon: MessageBoxIcon.Question,
                         buttons: MessageBoxButtons.YesNo);
         if (result == DialogResult.Yes)
         {
            Query queryRename = new Query(query.GetStrConnect());
            queryRename.RenameTable(curName: TempNameTabText, newName: cbxNameTab.Text);
            Init();
         }
      }

      private void btnClear_Click(object sender, EventArgs e)
      {
         flagProgDataChange = true;
         for (; ; )
         {
            bool End = true;
            try
            {
               int count = dgvData.Rows.Count;
               for (int num=0;num < count; num++)
               {
                  dgvData.Rows.RemoveAt(num);
               }
            }
            catch{ if (dgvData.Rows.Count > 1) End = false; else End = true; }
            if (End) break;
         }
         flagProgDataChange = false;
         Ctrl.ButtonStar(btnSave);
      }

      private void lblHelp_Click(object sender, EventArgs e)
      {
         List<string> lstHelpData = new List<string>();
         lstHelpData.Add("   Создание простых и связанных таблиц\n");
         lstHelpData.Add("   Удаление таблиц\n");
         lstHelpData.Add("   Переименование таблиц\n");
         lstHelpData.Add("   Добавление простых и связанных столбцов в таблицу \n");
         lstHelpData.Add("   Удаление столбцов из таблицы \n");
         lstHelpData.Add("   Переименование столбцов в таблице \n");
         lstHelpData.Add("   Просмотр и редактирование данных в таблицах \n");
         lstHelpData.Add("   Вставка данных из буфера в таблицу данных\n");
         lstHelpData.Add("   Фильтрация (WHERE) в таблицах \n");
         lstHelpData.Add("   Просмотр и редактирование расширенных свойств \n");
         lstHelpData.Add("   Просмотр связей таблиц \n");
         lstHelpData.Add("   Изменение типа данных столбцов \n");
         lstHelpData.Add("   Просмотр ошибок при работе с данным \n");
         lstHelpData.Sort();
         string strHelpData = string.Join("",lstHelpData.ToArray());

         MessageBox.Show(caption: "Help",
                         text: "Функции конструктора таблиц:\n"+
                               "------------------------------------------\n"+
                               strHelpData,
                         icon: MessageBoxIcon.Information,
                         buttons: MessageBoxButtons.OK);
      }


      private void tbcMain_Deselected_1(object sender, TabControlEventArgs e)
      {
         CheckSave();
      }

      private void TableCreator_Load(object sender, EventArgs e)
      {

      }

      private void btnNew_Click(object sender, EventArgs e)
      {
         cbxNameTab.Text = "NEW_NAME_TABLE";
         Ctrl.ButtonStar(btnCreate);
         btnSave.Enabled = false;
         cbxNameTab.SelectAll();
         Refresh(NewTable: true);
      }

      private void dgvData_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
      {

      }
      private void FormWidth()
      {
         int Width = 120; foreach (DataGridViewColumn column in dgvData.Columns) if (column.Visible) Width = Width + column.Width;
         // Ограничение автоширины формы
         if (Width < chbOffTuneCol.Location.X + chbOffTuneCol.Width) Width = chbOffTuneCol.Location.X + chbOffTuneCol.Width;
         int width = Misc.GetScreenWidth(this);
         if (Width > width) Width = width;
         this.Width = Width;
      }

      private void btnPropTable_Click(object sender, EventArgs e)
      {
         if (Ctrl.CheckNeedSave(btnPropSave))
            SavePropOrLinks();
         old_sender = sender;
         ReadProp();
      }
      private void ReadLinksAndProp()
      {
         if (Ctrl.CheckNeedSave(btnPropSave))
            SavePropOrLinks();

         if (rbxLink.Checked)
         {
            ReadLinks();
         }
         else
         {
            ReadProp();
         }
      }

      private void ReadProp()
      {

         flagProgDataChange = true;
         dgvProp.ReadOnly = false;
         dgvProp.RowsDefaultCellStyle.BackColor = Color.White;
         string name_col=null;
         if (rbxEPcol.Checked) name_col = "*";
         
         if (queryEPorLinks == null) queryEPorLinks = new Query(query.GetStrConnect());
         queryEPorLinks.GetExtProp(table_result: out dtPropOrLinks, 
                                       name_tab: chbCurTabLinks.Checked? cbxNameTab.Text :"*", 
                                       name_col: name_col);
         dtPropOrLinks.Namespace = "Prop";
         
         BindingSource source = new BindingSource();
         source.DataSource = dtPropOrLinks;
         DelRowsPropOrLink.Clear();
         DelRowsPropOrLink.Add(dtPropOrLinks.NewRow());
         DelRowsPropOrLink[0] = dtPropOrLinks.NewRow();
         dgvProp.DataSource = source;
         Ctrl.SetNumRowsDataGrid(ref dgvProp);
         Ctrl.ButtonStar(btnPropSave, false);
         flagProgDataChange = false;
      }
      private void btnPropColumn_Click(object sender, EventArgs e)
      {
         if (Ctrl.CheckNeedSave(btnPropSave))
            SavePropOrLinks();
         old_sender = sender;
         ReadProp();
      }

      private void SavePropOrLinks()
      {
         if (queryEPorLinks == null) return;
         if (dtPropOrLinks.Namespace == "Prop")
         {
            bool ColumnProp = false;
            foreach (DataColumn column in dtPropOrLinks.Columns)
               if (column.ColumnName == "ColumnName")
                  ColumnProp = true;

            foreach (DataRow row in dtPropOrLinks.Rows)
            {
               if (row.RowState == DataRowState.Deleted)
               {
                  for (int num = 1; num < DelRowsPropOrLink.Count; num++)
                  {
                     queryEPorLinks.SetExtProp(name_tab: DelRowsPropOrLink[num]["TableName"].ToString(),
                                        name_col: ColumnProp ? DelRowsPropOrLink[num]["ColumnName"].ToString() : null,
                                       name_prop: DelRowsPropOrLink[num]["ExtendedPropertyName"].ToString(),
                                             val: null);
                  }
                  DelRowsPropOrLink.Clear();
               }
               else if (row.RowState != DataRowState.Unchanged)
                  queryEPorLinks.SetExtProp(name_tab: row["TableName"].ToString(),
                                     name_col: ColumnProp ? row["ColumnName"].ToString() : null,
                                    name_prop: row["ExtendedPropertyName"].ToString(),
                                          val: row["ExtendedPropertyValue"].ToString());
            }
         }
         else if (dtPropOrLinks.Namespace == "Links")
         {
            int NumBuffer = 0;
            foreach(DataRow row in DelRowsPropOrLink)
            {
               if (NumBuffer == 0) { NumBuffer++; continue; }
               queryEPorLinks.DelForeignKey(row["FK_TABLE_NAME"].ToString(), row["CONSTRAINT_NAME"].ToString(), MsgYes: false);
            }
         }
         Ctrl.ButtonStar(btnPropSave, false);
      }
      private void btnPropSave_Click(object sender, EventArgs e)
      {
         SavePropOrLinks();
      }

      private void dgvProp_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
      {
         DataGridView dataGrid = sender as DataGridView;

         DataRow row = DelRowsPropOrLink[0];
         foreach (DataGridViewCell cell in dataGrid.SelectedRows[0].Cells) row[cell.ColumnIndex] = cell.Value;
         DelRowsPropOrLink[0] = row;
      }

      private void dgvProp_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
      {
         if (flagProgDataChange) return;
         if (Ctrl.YesDelRowDataTAble(dtPropOrLinks))
         {
            DelRowsPropOrLink.Add(Ctrl.CopyDataRow(DelRowsPropOrLink[0]));
            Ctrl.ButtonStar(btnPropSave);
         }
      }

      private void btnExtProp_Click(object sender, EventArgs e)
      {
         int Height = spc_H.Size.Height - spc_H.SplitterDistance;
         if (!VisBottomPanel)
         {
            spc_H.SplitterDistance = (spc_H.Size.Height - 200) > 0 ? spc_H.Size.Height - 200 : 0;
            ReadLinksAndProp();
            VisBottomPanel = true;
         }
         else
         {
            spc_H.SplitterDistance = (spc_H.Size.Height - HeightBottomPanel) > 0 ? spc_H.Size.Height - HeightBottomPanel : 0;
            VisBottomPanel = false;
         }
      }

      private void TableCreator_Resize(object sender, EventArgs e)
      {
         stc_H_SplitDist_Litle();
      }
      private void stc_H_SplitDist_Litle()
      {
         try
         {
            spc_H.SplitterDistance = (spc_H.Size.Height - HeightBottomPanel) > 0 ? spc_H.Size.Height - HeightBottomPanel : spc_H.Panel1MinSize;
         }
         catch{ }
         VisBottomPanel = false;
      }

      private void dgvProp_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
         if (flagProgDataChange) return;
         Ctrl.ButtonStar(btnPropSave);
      }
      

      private void tbcMain_DrawItem(object sender, DrawItemEventArgs e)
      {
         TabControl tab = sender as TabControl;
         TabPage page = tbcMain.TabPages[e.Index];
         page.ForeColor = (e.State == DrawItemState.Selected) ? Color.White : Color.Black;
         Color col = (e.State == DrawItemState.Selected) ? Color.DarkOliveGreen : Color.LightGray;
         e.Graphics.FillRectangle(new SolidBrush(col), e.Bounds);

         Rectangle paddedBounds = e.Bounds;
         int yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
         paddedBounds.Offset(1, yOffset);
         TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, page.ForeColor);
      }

      private void dgvProp_KeyPress(object sender, KeyPressEventArgs e)
      {
         if (e.KeyChar == 22)
         {
            flagProgDataChange = true;
            Ctrl ctrl = new Ctrl();
            ctrl.PasteInDataTableFromClipboard(ref dtPropOrLinks,
                                                CurRow: dgvProp.SelectedCells[0].RowIndex,
                                                CurCol: dgvProp.SelectedCells[0].ColumnIndex,
                                                AllowAddRows: true);
            dgvProp.Refresh();
            flagProgDataChange = false;
         }
      }

      private void ReadLinks()
      {
         if (Ctrl.CheckNeedSave(btnPropSave))
            SavePropOrLinks();
         flagProgDataChange = true;
         BindingSource bindingSource = new BindingSource();
         string NameTable = null;

         if (queryEPorLinks == null) queryEPorLinks = new Query(query.GetStrConnect());
         if (chbCurTabLinks.Checked) NameTable = cbxNameTab.Text;
         queryEPorLinks.GetForeignKey(table: out dtPropOrLinks,
                                 NameTable: NameTable);

         dtPropOrLinks.Namespace = "Links";
         bindingSource.DataSource = dtPropOrLinks;
         dgvProp.DataSource = bindingSource;
         DelRowsPropOrLink.Clear();
         DelRowsPropOrLink.Add(dtPropOrLinks.NewRow());
         DelRowsPropOrLink[0] = dtPropOrLinks.NewRow();

         dgvProp.ReadOnly = true;
         dgvProp.RowsDefaultCellStyle.BackColor = Color.LightGray;
         Ctrl.SetNumRowsDataGrid(ref dgvProp);
         flagProgDataChange = false;
      }
      private void btnRLinks_Click(object sender, EventArgs e)
      {
         ReadLinks();
      }

      private void dgvData_DataError(object sender, DataGridViewDataErrorEventArgs e)
      {
         string Text = "row:" + e.RowIndex.ToString() + " " +
                       "column:" + e.ColumnIndex.ToString() + " " +
                       "error:" + e.Exception +
                        Environment.NewLine;

         if (!tbxError.Text.Contains(Text))
            tbxError.Text = tbxError.Text + Text;
      }

      private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
      {

      }

      private void button1_Click(object sender, EventArgs e)
      {
         tbxError.Clear();
      }

      private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
      {

      }

      private void chbCurTabLinks_CheckedChanged(object sender, EventArgs e)
      {
         if (dtPropOrLinks.Namespace == "Links")
            ReadLinks();
         else
            ReadProp();
      }


      private void rbx_Click(object sender, EventArgs e)
      {
         ReadLinksAndProp();
      }

      private void rbxEPtab_CheckedChanged(object sender, EventArgs e)
      {

      }

      private void TableCreator_FormClosed(object sender, FormClosedEventArgs e)
      {
         dtTune = new DataTable("Tune");
         DataColumn column;
         column = new DataColumn("this.Top"); dtTune.Columns.Add(column);
         column = new DataColumn("this.Left"); dtTune.Columns.Add(column);
         column = new DataColumn("this.Height"); dtTune.Columns.Add(column);
         column = new DataColumn("this.Width"); dtTune.Columns.Add(column);
         column = new DataColumn("NameTable"); dtTune.Columns.Add(column);
         DataRow row;
         row = dtTune.NewRow();
         row["this.Top"] = this.Top;
         row["this.Left"] = this.Left;
         row["this.Height"] = this.Height;
         row["this.Width"] = this.Width;
         row["NameTable"] = this.cbxNameTab.Text;
         dtTune.Rows.Add(row);
         AccessProp.SetProp("TableCreator_Tune", Str.SetStrFromTable(dtTune));
      }
   }
}

