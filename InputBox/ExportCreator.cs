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
using WorkString;

namespace WorkBox
{
   public partial class ExportCreator : Form
   {
      DataTable[] dts;
      DataTable dtTune;
      DataTable dtRelation_File_Obj;
      DataTable dtRelation_File_Sys;
      Query[] qs;
      Query qTRelation;
      Query query;
      BindingSource[] bss;
      enum dtName{ OBJECT, HMI, PLC, FILE }

      bool DataProgChang = true;
      public ExportCreator(Query query)
      {
         InitializeComponent();
         this.query = query;
         qTRelation = new Query(query.GetStrConnect());
         UpDate(qTRelation);

         //string dataTune = AccessProp.GetPropStr("ExportCreator_Tune");
         //dtTune = Str.GetTableFromStr(dataTune, MsgYes: false);
         //try
         //{
         //   tsmTune_AutoSize.Checked = bool.Parse(dtTune.Rows[0]["tsmTune_AutoSize"].ToString());
         //   tsmTune_SaveToServer.Checked = bool.Parse(dtTune.Rows[0]["tsmTune_SaveToServer"].ToString());

         //   this.Top = int.Parse(dtTune.Rows[0]["this.Top"].ToString());
         //   this.Left = int.Parse(dtTune.Rows[0]["this.Left"].ToString());
         //   this.Height = int.Parse(dtTune.Rows[0]["this.Height"].ToString());
         //   this.Width = int.Parse(dtTune.Rows[0]["this.Width"].ToString());
         //}
         //catch { }
      }
      #region INIT
      private void Init(Control.ControlCollection objects, Query query, bool AutoSize, bool LocalData = true)
      {
         DataProgChang = true;
         int num = 0;
         foreach (object objTLP in objects)
         {
            if (objTLP is TableLayoutPanel)
            {
               TableLayoutPanel tlp = objTLP as TableLayoutPanel;
               foreach (object objDGV in tlp.Controls)
               {
                  if (objDGV is DataGridView)
                  {
                     num++;
                  }
               }
            }
         }
         dts = new DataTable[num];
         qs = new Query[num];
         bss = new BindingSource[num];
         num--;
         foreach (object objTLP in objects)
         {
            if (objTLP is TableLayoutPanel)
            {
               TableLayoutPanel tlp = objTLP as TableLayoutPanel;
               foreach (object objDGV in tlp.Controls)
               {
                  if (objDGV is DataGridView)
                  {
                     DataGridView dgv = objDGV as DataGridView;
                     switch (dgv.Name)
                     {
                        case "dgvOBJECT":
                           {
                              num = (int)dtName.OBJECT;
                              qs[num] = new Query(query.GetStrConnect());
                              qs[num].GetTable(NameTable: "InstHead",
                                          table: out dts[num],
                                          select: "EN,NAME,PATH",
                                          order: "NAME");

                              if (LocalData)
                              {
                                 string strLocal = AccessProp.GetPropStr("ExportCreator_OBJECT");
                                 DataTable tabLocal = Str.GetTableFromStr(strLocal);
                                 Ctrl.CopyDataTable(tabSource: tabLocal, tabDest: ref dts[num], NumColSourceMerge: 1,
                                                    type: Ctrl.CopyDataTableType.byDest);
                              }
                              bss[num] = new BindingSource();
                              bss[num].DataSource = dts[num];
                              dgv.DataSource = bss[num];
                              break;
                           }
                        case "dgvHMI":
                           {
                              num = (int)dtName.HMI;
                              qs[num] = new Query(query.GetStrConnect());
                              qs[num].GetTable(NameTable: "ListHMI",
                                          table: out dts[num],
                                          select: "EN,NAME",
                                          order: "NAME");
                              if (LocalData)
                              {
                                 string strLocal = AccessProp.GetPropStr("ExportCreator_HMI");
                                 DataTable tabLocal = Str.GetTableFromStr(strLocal);
                                 Ctrl.CopyDataTable(tabSource: tabLocal, tabDest: ref dts[num], NumColSourceMerge: 1,
                                                    type: Ctrl.CopyDataTableType.byDest);
                              }
                              bss[num] = new BindingSource();
                              bss[num].DataSource = dts[num];
                              dgv.DataSource = bss[num];
                              dgv.Columns["HMI_PATH_BUTTON"].Visible = false;
                              dgv.Columns["HMI_PATH_TEXT"].Visible = false;
                              break;
                           }
                        case "dgvPLC":
                           {
                              num = (int)dtName.PLC;
                              qs[num] = new Query(query.GetStrConnect());
                              qs[num].GetTable(NameTable: "ListPLC",
                                          table: out dts[num],
                                          select: "EN,NAME",
                                          order: "NAME");
                              if (LocalData)
                              {
                                 string strLocal = AccessProp.GetPropStr("ExportCreator_PLC");
                                 DataTable tabLocal = Str.GetTableFromStr(strLocal);
                                 Ctrl.CopyDataTable(tabSource: tabLocal, tabDest: ref dts[num], NumColSourceMerge: 1,
                                                    type: Ctrl.CopyDataTableType.byDest);
                              }
                              bss[num] = new BindingSource();
                              bss[num].DataSource = dts[num];
                              dgv.DataSource = bss[num];
                              dgv.Columns["PLC_PATH_BUTTON"].Visible = false;
                              dgv.Columns["PLC_PATH_TEXT"].Visible = false;
                              break;
                           }
                        case "dgvFILE":
                           {
                              num = (int)dtName.FILE;
                              qs[num] = new Query(query.GetStrConnect());
                              qs[num].GetTable(NameTable: "ListFILE",
                                          table: out dts[num],
                                          select: "EN,NAME,PATH",
                                          order: "NAME");
                              if (LocalData)
                              {
                                 string strLocal = AccessProp.GetPropStr("ExportCreator_FILE");
                                 DataTable tabLocal = Str.GetTableFromStr(strLocal);
                                 Ctrl.CopyDataTable(tabSource: tabLocal, tabDest: ref dts[num], NumColSourceMerge: 1,
                                                    type: Ctrl.CopyDataTableType.byDest);
                              }
                              bss[num] = new BindingSource();
                              bss[num].DataSource = dts[num];
                              dgv.DataSource = bss[num];
                              break;
                           }

                     }
                     if (AutoSize)
                     {
                        dgv.Parent.Width = Ctrl.GetColumsWidth(dgv, MaxWidth: -1);
                        dgv.Parent.Height = Ctrl.GetRowsHeight(dgv, MaxHeight: -1);
                     }
                     else
                     {
                        dgv.Parent.Width = 291;
                        dgv.Parent.Height = 284;
                     }
                  }
               }
            }
         }
         DataProgChang = false;
      }
      #endregion
      #region DGV CLICK
      private void dgv_Click(object sender, DataGridViewCellEventArgs e)
      {
         try
         {
            DataGridView dgvTable = sender as DataGridView;
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;
            DataGridViewCell dgvCell = dgvTable.Rows[e.RowIndex].Cells[e.ColumnIndex];

            // CheckBox
            if (dgvTable.Columns[e.ColumnIndex].Name.Contains("_EN"))
            {
               if (dgvTable.Name == "dgvPLC")
               {
                  bool val = ((dgvCell.Value == null) ? false : bool.Parse(dgvCell.Value.ToString()));
                  dgvTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !val;
                  if (!val)
                     foreach (DataGridViewRow row in dgvTable.Rows)
                     {
                        if (row.Index == e.RowIndex) continue;
                        row.Cells[e.ColumnIndex].Value = false;
                     }
               }
            }
            // Button
            if (dgvTable.Columns[e.ColumnIndex].Name.Contains("_BUTTON"))
            {
               OpenFileDialog OFD = new OpenFileDialog();
               if (OFD.ShowDialog() == DialogResult.OK)
                  dgvTable.Rows[e.RowIndex].Cells[dgvTable.Name.Replace("dgv", "") + "_PATH_TEXT"].Value = OFD.FileName;
            }
         }
         catch (Exception ex) { Ctrl.MessErr(ex); }
      }
      #endregion

      #region DGV CHANGED
      private void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
         Ctrl.ButtonStar(btnSave, !DataProgChang);
         DataGridView dgv = sender as DataGridView;
         if (dts != null && 
             dgv.Name != "dgvFILE" && 
             dgv.Columns[dgv.CurrentCell.ColumnIndex].Name.Contains("_EN"))
         {
            UpRelatFS(UpRelatFO());
         }
      }
      #endregion

      public List<string> UpRelatFO()
      {
         List<string> SelObj = Ctrl.GetListDataTable(dts[(int)dtName.OBJECT], "True", 0, 1);
         List<string> PermFile = new List<string>();
         foreach (string obj in SelObj)
         {
            PermFile.AddRange(Ctrl.GetListDataTable(dtRelation_File_Obj, "Yes", FinNameCol: obj, RetNameCol: "NAME"));
         }
         Ctrl.SetVisRowDgv(ref dgvFILE, PermFile.ToArray(), NameCol: "FILE_NAME");
         return PermFile;
      }

      public void UpRelatFS(List<string> ObjPermFile)
      {
         List<string> SelSys = Ctrl.GetListDataTable(dts[(int)dtName.HMI], "True", 0, 1);
         SelSys.AddRange(Ctrl.GetListDataTable(dts[(int)dtName.PLC], "True", 0, 1));
         List<string> PermFile = new List<string>();
         foreach (string sys in SelSys)
         {
            PermFile.AddRange(Ctrl.GetListDataTable(table: dtRelation_File_Sys, FinVal: "None", FinNameCol: sys, RetNameCol: "NAME", FinNotVal: true));
         }
         Str.Find(lst1: ObjPermFile, 
                  lst2: PermFile, 
              lstEqual: out List<string> lstEqual, 
              lst1_rem: out List<string> lst1_rem, 
              lst2_rem: out List<string> lst2_rem);
         Ctrl.SetVisRowDgv(ref dgvFILE, lstEqual.ToArray(), NameCol: "FILE_NAME");
      }
      private void btnSave_Click(object sender, EventArgs e)
      {
         if (tsmTune_SaveToServer.Checked)
         {
            DialogResult result = MessageBox.Show(
               text: "Настройки будут сохранены локально,\n "+
                     "сохранить настройки экспорта так же и на сервере?",
               caption: "Информация",
               icon: MessageBoxIcon.Question,
               buttons: MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
               foreach (Query q in qs)
               {
                  if (q != null) q.SetTable();
               }
            }
         }
         AccessProp.SetProp("ExportCreator_OBJECT", Str.SetStrFromTable(dts[(int)dtName.OBJECT]));
         AccessProp.SetProp("ExportCreator_HMI",    Str.SetStrFromTable(dts[(int)dtName.HMI]));
         AccessProp.SetProp("ExportCreator_PLC",    Str.SetStrFromTable(dts[(int)dtName.PLC]));
         AccessProp.SetProp("ExportCreator_FILE",   Str.SetStrFromTable(dts[(int)dtName.FILE]));
         Ctrl.ButtonStar(btnSave, false);
      }

      private void ExportCreator_Shown(object sender, EventArgs e)
      {
         string dataTune = AccessProp.GetPropStr("ExportCreator_Tune");
         dtTune = Str.GetTableFromStr(dataTune, MsgYes: false);
         try
         {
            tsmTune_AutoSize.Checked = bool.Parse(dtTune.Rows[0]["tsmTune_AutoSize"].ToString());
            tsmTune_SaveToServer.Checked = bool.Parse(dtTune.Rows[0]["tsmTune_SaveToServer"].ToString());

            this.Top = int.Parse(dtTune.Rows[0]["this.Top"].ToString());
            this.Left = int.Parse(dtTune.Rows[0]["this.Left"].ToString());
            this.Height = int.Parse(dtTune.Rows[0]["this.Height"].ToString());
            this.Width = int.Parse(dtTune.Rows[0]["this.Width"].ToString());
         }
         catch { }

         bool LocalData = true;
         if ( tsmTune_SaveToServer.Checked )
         {
            DialogResult result = MessageBox.Show(
               text: "Использовать настройки экспорта сохранённые на сервере?",
               caption: "Информация",
               icon: MessageBoxIcon.Question,
               buttons: MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) LocalData = false;
         }
         Init(flp_H1.Controls, query, tsmTune_AutoSize.Checked, LocalData: LocalData);
         UpRelatFS(UpRelatFO());
      }

      private void tsmTune_AutoSize_Click(object sender, EventArgs e)
      {
         ToolStripMenuItem item = sender as ToolStripMenuItem;
         item.Checked = !item.Checked;
         bool LocalData = true;
         if (tsmTune_SaveToServer.Checked)
         {
            DialogResult result = MessageBox.Show(
               text: "Использовать настройки экспорта сохранённые на сервере?",
               caption: "Информация",
               icon: MessageBoxIcon.Question,
               buttons: MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) LocalData = false;
         }
         Init(flp_H1.Controls, query, item.Checked, LocalData: LocalData);
      }

      private void tsmTune_InitRelation_Click(object sender, EventArgs e)
      {
         DialogResult result = MessageBox.Show(caption: "Информация",
                         text: "Выбирите действия над таблицей\n" +
                               "настройки экспорта (RelationExport).\n" +
                               "'Да' - полная инициализация (удаление, создание)\n" +
                               "'Нет' - обновление данных (столбцов и строк)\n" +
                               "'Отмена' - отмена действий",
                         buttons: MessageBoxButtons.YesNoCancel,
                         icon: MessageBoxIcon.Question);

         if (result == DialogResult.Cancel) return; // Отмена        

         if (result == DialogResult.Yes) // Полная инициализация 
         {
            InitDate(qTRelation);
         }
         if (result == DialogResult.No) // Обновление
         {
            UpDate(qTRelation);
         }
         MessageBox.Show(text: "Проверка завершена.",
                                        caption: "Информация",
                                        buttons: MessageBoxButtons.OK,
                                        icon: MessageBoxIcon.Information);
      }
      public void DelTab(Query query, string[] NamesTab)
      {
         foreach(string NameTab in NamesTab){
            if (query.AvailabTable(NameTab))
               query.DelTable(NameTab);
         }
      }
      public void InitDate(Query qTRelation, string NameTabRelFS = "Relation_File_Sys",
                                             string NameTabDatFS = "Relation_File_Sys_Data",
                                             string NameTabRelFO = "Relation_File_Obj",
                                             string NameTabDatFO = "Relation_File_Obj_Data")
      {
         List<string> lstNameSys;
         List<string> lstNameFile;
         List<string> lstNameObj;
         List<string[]> TuneColumn;
         List<string[]> ForeColumn;
         List<string[]> DataRows;
         // Список систем
         lstNameSys = GetDataTable(new string[] { "ListHMI", "ListPLC"});
         // Список файлов
         lstNameFile = GetDataTable(new string[] { "ListFILE" });
         // Список объектов
         lstNameObj = GetDataTable(new string[] { "TypeHead" });

         // Удаление таблиц
         DelTab(qTRelation, new string[] { NameTabRelFS, NameTabDatFS, NameTabRelFO, NameTabDatFO });

         #region Создание таблицы данных для таблицы взаимосвязей файлов и систем
         qTRelation.CreateTable(NameTable: NameTabDatFS, nameColumns: new List<string> { "NAME", "DESCRIPT" }, PrimKey: "NAME");
         DataRows = new List<string[]>();
         DataRows.Add(new string[] { "Yes", "Файл потдерживается и независет ни от чего" });
         DataRows.Add(new string[] { "PLC", "Необходимо чтобы был выбран хотя бы один контроллер" });
         DataRows.Add(new string[] { "PLC&File", "Необходимо чтобы был выбран хотя бы один контроллер и путь к файлу экспорта" });
         DataRows.Add(new string[] { "File", "Необходимо чтобы был выбран путь к файлу экспорта" });
         DataRows.Add(new string[] { "FileObj", "Необходимо чтобы был выбран путь к файлу шаблон объекта" });
         DataRows.Add(new string[] { "None", "Файл для выбранной системы не потдерживается" });

         foreach (string[] DataRow in DataRows)
            qTRelation.AddDataTable(nameTable: NameTabDatFS,
                                  nameColumns: new string[] { "NAME", "DESCRIPT" },
                                       values: DataRow);
         #endregion Создание таблицы данных для таблицы взаимосвязей файлов и систем

         #region Создание таблицы данных для таблицы взаимосвязей файлов и объектов
         qTRelation.CreateTable(NameTable: NameTabDatFO, nameColumns: new List<string> { "NAME", "DESCRIPT" }, PrimKey: "NAME");
         DataRows = new List<string[]>();
         DataRows.Add(new string[] { "Yes", "Файл потдерживается и независет ни от чего" });
         DataRows.Add(new string[] { "None", "Файл для выбранного объекта не потдерживается" });

         foreach (string[] DataRow in DataRows)
            qTRelation.AddDataTable(nameTable: NameTabDatFO,
                                  nameColumns: new string[] { "NAME", "DESCRIPT" },
                                       values: DataRow);
         #endregion Создание таблицы данных для таблицы взаимосвязей файлов и объектов

         #region Создание таблицы взаимосвязей файлов и систем
         CreateTable(NameTabRelFS, NameTabDatFS, lstNameSys, lstNameFile);
         #endregion Создание таблицы взаимосвязей файлов и систем
         #region Создание таблицы взаимосвязей файлов и объектов
         CreateTable(NameTabRelFO, NameTabDatFO, lstNameObj, lstNameFile);
         #endregion Создание таблицы взаимосвязей файлов и объектов
      }
      public void CreateTable(string strNameTableCreate, string strNameTableData, List<string> lstNameColumn, List<string> lstNameFile)
      {
         List<string[]> TuneColumn = new List<string[]>();
         List<string[]> ForeColumn = new List<string[]>();
         TuneColumn.Add(new string[] { "NAME", null, "true", "false", null });
         ForeColumn.Add(new string[] { "NAME", "ListFILE", "NAME" });
         foreach (string NameColumn in lstNameColumn)
         {
            TuneColumn.Add(new string[] { NameColumn, null, null, "false", "None" });
            ForeColumn.Add(new string[] { NameColumn, strNameTableData, "NAME" });
         }
         qTRelation.CreateTable(NameTable: strNameTableCreate, TuneColumns: TuneColumn, ForeignColumns: ForeColumn);
         foreach (string NameFile in lstNameFile)
            qTRelation.AddDataTable(nameTable: strNameTableCreate,
                                  nameColumns: new string[] { "NAME" },
                                       values: new string[] { NameFile });
      }
      public void UpDate(Query qTRelation, string NameTabRelFS = "Relation_File_Sys",
                                             string NameTabDatFS = "Relation_File_Sys_Data",
                                             string NameTabRelFO = "Relation_File_Obj")
      {
         // Получение таблицы данных отношений файлов и систем
         qTRelation.GetTable(NameTabRelFS, out dtRelation_File_Sys);
         // Получение таблицы данных отношений файлов и объектов
         qTRelation.GetTable(NameTabRelFO, out dtRelation_File_Obj);
         // Проверка столбцов (систем)
         // Список имён столбцов текущей таблице
         qTRelation.GetNamesColumn(out List<string> lst_Name_Col_Cur, NameTabRelFS);
         lst_Name_Col_Cur.Remove("NAME");
         // Список имён столбцов HMI и PLC
         List<string> lst_Col_Source = GetDataTable(new string[]{"ListHMI", "ListPLC" });
         // Строка имён столбцов HMI и PLC
         string str_Col_Source = string.Join("|", lst_Col_Source.ToArray());
         string str_Col_Cur = "";
         // Проверка на отсутствие в источниках (удалена)
         foreach (string str_Name_Col in lst_Name_Col_Cur)
         {
            if (!str_Col_Source.Contains(str_Name_Col))
            {
               DialogResult Dialog_Result =
                  MessageBox.Show(text: "Системы " + str_Name_Col + "\nнет в исходных таблицах (ListHMI, ListPLC)" +
                                         "\n   Да - удалить систему из таблицы RelationExport;" +
                                         "\n   Нет - добавить информацию в исходные таблицы;" +
                                         "\n   Отмена - оставить без изменений",
                                  caption: "Информация",
                                  buttons: MessageBoxButtons.YesNoCancel,
                                  icon: MessageBoxIcon.Question);
               if (Dialog_Result == DialogResult.Yes)
               {
                  qTRelation.DelColumn(NameTabRelFS, str_Name_Col, null, true, true, true);
               }
               else if (Dialog_Result == DialogResult.No)
               {
                  DialogResult Dialog_Result_2 =
                  MessageBox.Show(text: "Выбирите исходную таблицу для добавления" +
                                         "\n   Да - ListHMI;" +
                                         "\n   Нет - ListPLC;",
                                  caption: "Информация",
                                  buttons: MessageBoxButtons.YesNo,
                                  icon: MessageBoxIcon.Question);
                  if (Dialog_Result_2 == DialogResult.Yes)
                     qTRelation.AddDataTable("ListHMI", new string[] { "NAME" }, new string[] { str_Name_Col });
                  else if (Dialog_Result_2 == DialogResult.No)
                     qTRelation.AddDataTable("ListPLC", new string[] { "NAME" }, new string[] { str_Name_Col });
               }
            }
            str_Col_Cur = str_Col_Cur + "|" + str_Name_Col;
         }
         // Проверка на новые в источниках (добавлена)
         foreach (string str_Col in lst_Col_Source)
         {
            if (!str_Col_Cur.Contains(str_Col))
            {
               DialogResult Dialog_Result_3 =
                  MessageBox.Show(text: "Системы " + str_Col + "\nнет в таблице взаимоотношений " + NameTabRelFS +
                                         "\n   Добавить?",
                                  caption: "Информация",
                                  buttons: MessageBoxButtons.YesNo,
                                  icon: MessageBoxIcon.Question);
               if (Dialog_Result_3 == DialogResult.Yes)
               {
                  qTRelation.AddColumn(NameTabRelFS, str_Col, ForeignTabCol: new string[] { str_Col, NameTabDatFS, "NAME" }, DefaultData: "None");
               }
            }
         }
         // Проверка файлов (строк)
         List<string> lst_Rows_Dest = GetDataTable(new string[] { "Relation_File_sys" });
         string str_Rows_Dest = string.Join("|", lst_Rows_Dest.ToArray());
         List<string> lst_Rows_Sour = GetDataTable(new string[] { "ListFILE" });
         string str_Rows_Sour = string.Join("|", lst_Rows_Sour.ToArray());
         foreach (string str_Row in lst_Rows_Sour)
         {
            if (!str_Rows_Dest.Contains(str_Row))
            {
               DialogResult Dialog_Result_4 =
                     MessageBox.Show(text: "Файла " + str_Row + "\nнет в таблице взаимоотношений " + NameTabRelFS +
                                            "\n   Добавить?;",
                                     caption: "Информация",
                                     buttons: MessageBoxButtons.YesNo,
                                     icon: MessageBoxIcon.Question);
               if (Dialog_Result_4 == DialogResult.Yes)
               {
                  qTRelation.AddDataTable(NameTabRelFS, new string[] { "NAME" }, new string[] { str_Row });
               }
            }
         }
         foreach (string str_Row in lst_Rows_Dest)
         {
            if (!str_Rows_Sour.Contains(str_Row))
            {
               DialogResult Dialog_Result_4 =
                     MessageBox.Show(text: "Файла " + str_Row + "\nнет в исходных таблицах (ListHMI, ListPLC)" +
                                            "\n   Удалить из \""+ NameTabRelFS + "\"?",
                                     caption: "Информация",
                                     buttons: MessageBoxButtons.YesNo,
                                     icon: MessageBoxIcon.Question);
               if (Dialog_Result_4 == DialogResult.Yes)
               {
                  qTRelation.DelDataTable(NameTabRelFS, where: "NAME = " + str_Row);
               }
            }
         }
      }
      public List<string> GetDataTable(string[] NamesTab)
      {
         List<string>[] lstTabNameColumns = new List<string>[NamesTab.Length];
         List<string> result = new List<string>();
         Query qTRelation = new Query(query.GetStrConnect());
         int i = 0;
         foreach(string NameTab in NamesTab)
         {
            qTRelation.GetTable(NameTab, out DataTable table, select: "NAME");
            lstTabNameColumns[i] = Ctrl.GetListRowsInOneColumnTable(table, "NAME");
            result.AddRange(lstTabNameColumns[i]);
            i++;
         }
         return result;
      }

      private void ExportCreator_FormClosed(object sender, FormClosedEventArgs e)
      {
         dtTune = new DataTable("Tune");
         DataColumn column;
         column = new DataColumn(tsmTune_AutoSize.Name); dtTune.Columns.Add(column);
         column = new DataColumn(tsmTune_SaveToServer.Name); dtTune.Columns.Add(column);
         column = new DataColumn("this.Top"); dtTune.Columns.Add(column);
         column = new DataColumn("this.Left"); dtTune.Columns.Add(column);
         column = new DataColumn("this.Height"); dtTune.Columns.Add(column);
         column = new DataColumn("this.Width"); dtTune.Columns.Add(column);
         DataRow row;
         row = dtTune.NewRow();
         row[tsmTune_AutoSize.Name] = tsmTune_AutoSize.Checked;
         row[tsmTune_SaveToServer.Name] = tsmTune_SaveToServer.Checked;
         row["this.Top"] = this.Top;
         row["this.Left"] = this.Left;
         row["this.Height"] = this.Height;
         row["this.Width"] = this.Width;
         dtTune.Rows.Add(row);
         AccessProp.SetProp("ExportCreator_Tune", Str.SetStrFromTable(dtTune));
      }

      private void tsmTune_SaveToServer_Click(object sender, EventArgs e)
      {
         ToolStripMenuItem item = sender as ToolStripMenuItem;
         item.Checked = !item.Checked;
      }

      private void tsmTune_TableCreator_Click(object sender, EventArgs e)
      {
         // Настройка таблицы
         Query queryTC = new Query(StrCon: query.GetStrConnect());
         TuneViewTable tuneViewTable = new TuneViewTable(query: queryTC);

         TableCreator TabCreat = new TableCreator(query, tuneViewTable, TableCreator.TableCreatorTabControl.DATA);
         TabCreat.ShowDialog();
      }

      private void btnSartExport_Click(object sender, EventArgs e)
      {
         TestDataForExport(ref txbExportErr, ref txbExportWarning, ref txbExportOut, out List<string[]> TaskExport);
      }
      private void dgvColorDef()
      {
         foreach(DataGridView dgv in new DataGridView[]{dgvFILE, dgvHMI,dgvOBJECT, dgvPLC}){
            foreach(DataGridViewRow row in dgv.Rows){
               row.Cells[dgv.Name.Replace("dgv", "") + "_EN"].Style.BackColor = Color.FromArgb(255, 255, 255);
               row.Cells[dgv.Name.Replace("dgv", "") + "_PATH_TEXT"].Style.BackColor = Color.FromArgb(244, 244, 244);
            }
         }
      }
      private bool TestDataForExport(ref TextBox tbErr, ref TextBox tbWar, ref TextBox tbOut, out List<string[]> TaskExport)
      {
         TaskExport = new List<string[]>();
         dgvColorDef();
         int countErr = 0;
         int countWar = 0;
         int countOut = 0;
         var NL = Environment.NewLine;
         string TErr = "";
         string TWar = "";
         string TOut = "";
         bool result = false;
         TOut = TOut + Str.GetTimeNow() + " -> Старт проверки конфигурации экспорта" + NL;

         List<string> lstObjSel = Ctrl.GetListDataTable(dgvOBJECT, "True", FinNameCol: "OBJECT_EN", RetNameCol: "OBJECT_NAME", VisOnly: true);
         TOut = TOut + Str.GetTimeNow() + " -> Выбраны объекты экспорта: " + string.Join(", ", lstObjSel.ToArray()) + NL;

         List<string> lstSysSel = Ctrl.GetListDataTable(dgvHMI, "True", FinNameCol: "HMI_EN", RetNameCol: "HMI_NAME", VisOnly: false);
         lstSysSel.AddRange(Ctrl.GetListDataTable(dgvPLC, "True", FinNameCol: "PLC_EN", RetNameCol: "PLC_NAME", VisOnly: false));
         TOut = TOut + Str.GetTimeNow() + " -> Выбраны системы экспорта: " + string.Join(", ", lstSysSel.ToArray()) + NL;

         List<string> lstFileSel = Ctrl.GetListDataTable(dgvFILE, "True", FinNameCol: "FILE_EN", RetNameCol: "FILE_NAME", VisOnly: true);
         TOut = TOut + Str.GetTimeNow() + " -> Выбраны файлы экспорта: " + string.Join(", ", lstFileSel.ToArray()) + NL;
 
         TOut = TOut + Str.GetTimeNow() + " -> Чтение таблицы взаимоотношений экспорта: Relation_File_Sys" + NL;
         DataTable Relation_File_Sys;
         Query qRelat = new Query(StrCon: query.GetStrConnect());
         qRelat.GetTable("Relation_File_Sys", out Relation_File_Sys);
         TOut = TOut + Str.GetTimeNow() + " -> Чтение таблицы типов взаимоотношений экспорта: Relation_File_Sys_Data" + NL;
         DataTable dtRelatType;
         qRelat.GetTable("Relation_File_Sys_Data", out dtRelatType);
         TOut = TOut + Str.GetTimeNow() + " -> Анализ таблицы: Relation_File_Sys по выбранным файлам экспорта" + NL;
         foreach (string nameFile in lstFileSel){

            List<string> lstSysPermit = Ctrl.GetListColDataTable(Relation_File_Sys, nameFile, FinNameCol: "NAME", NotVal: "Yes");
            Str.Find(lst1: lstSysPermit, lst2: lstSysSel, out List<string> lstSys, out List<string> lst1_rem, out List<string> lst2_rem);

            foreach (string nameSys in lstSys) {
               List<string> dataPermit = Ctrl.GetListDataTable(Relation_File_Sys, nameFile, FinNameCol: "NAME", RetNameCol: nameSys);
               
               bool printErr = false;
               bool printWar = false;
               bool printOut = false;

               switch (dataPermit[0])
               {
                  case "File":
                  case "PLC":
                  case "PLC&File":
                     {
                        if (dataPermit[0] == "PLC" || dataPermit[0] == "PLC&File")
                        {
                           // Проверка выбранный контроллер
                           // Если не выбран ни один контроллер
                           List<string> lstPLC = Ctrl.GetListDataTable(dts[(int)dtName.PLC], "True", FinNameCol: "EN");
                           if (lstPLC.Count == 0)
                           {
                              if (!Ctrl.CheckDataTable(ref dgvPLC, nameSys, "PLC_NAME", "PLC_EN", Color.White, Color.Red)) printErr = true;
                              else printOut = true;
                           }
                           else
                           {
                              // Проверка выбора на действительность
                              //if (!Str.Find(lst1: lstSysSel, lst2: lstPLC, lstEqual: out List<string> lstEqual, lst1_rem: out List<string> lst1_rem, lst2_rem: out List<string> lst2_rem))
                              //{
                                 if (!Ctrl.CheckDataTable(ref dgvPLC, nameSys, "PLC_NAME", "PLC_EN", Color.White, Color.Red)) printErr = true;
                                 else printOut = true;
                              //}
                           }
                        }
                        if (dataPermit[0] == "File" || dataPermit[0] == "PLC&File")
                        {
                           // Проверяем файл для экспорта
                           if (!Ctrl.CheckDataTable(ref dgvFILE, nameFile, "FILE_NAME", "FILE_PATH_TEXT", Color.FromArgb(244, 244, 244), Color.Red)) printErr = true;
                           else printOut = true;
                        }
                        break;
                     }
                  case "HMI":
                     {
                        // Проверка системы HMI для экспорта
                        List<string> lstHMI = Ctrl.GetListDataTable(dts[(int)dtName.HMI], "True", FinNameCol: "EN");
                        if (lstHMI.Count == 0)
                        {
                           if (!Ctrl.CheckDataTable(ref dgvHMI, nameSys, "HMI_NAME", "HMI_EN", Color.White, Color.Red)) printErr = true;
                           else printOut = true;
                        }
                        else
                        {
                           // Проверка выбора на действительность
                           //if (!Str.Find(lst1: lstSysSel, lst2: lstHMI, lstEqual: out List<string> lstEqual, lst1_rem: out List<string> lst1_rem, lst2_rem: out List<string> lst2_rem))
                           //{
                              if (!Ctrl.CheckDataTable(ref dgvHMI, nameSys, "HMI_NAME", "HMI_EN", Color.White, Color.Red)) printErr = true;
                              else printOut = true;
                           //}
                        }
                        break;
                     }
                  case "FileObj":
                     {
                        // Проверка выбора файлов для объектов
                        if (Ctrl.CheckDataTable(ref dgvHMI, nameSys, "HMI_NAME", "HMI_EN", Color.White, Color.White))
                           if (!Ctrl.CheckDataTable(ref dgvOBJECT, "True", "OBJECT_EN", "OBJECT_PATH_TEXT", Color.FromArgb(244, 244, 244), Color.Red)) printErr = true;
                           else printOut = true;
                        break;
                     }
                  case "None":
                     {
                        if (Ctrl.CheckDataTable(ref dgvHMI, nameSys, "HMI_NAME", "HMI_EN", Color.White, Color.White)) printWar = true;
                        if (Ctrl.CheckDataTable(ref dgvPLC, nameSys, "PLC_NAME", "PLC_EN", Color.White, Color.White)) printWar = true;
                        break;
                     }
               }
               if (printErr)
               {
                  countErr++;
                  TErr = TErr + Str.GetTimeNow() + " -> " + nameFile + " - " + nameSys + " : " + dataPermit[0] + " = " + Ctrl.GetListDataTable(dtRelatType, dataPermit[0], FinNameCol: "NAME", RetNameCol: "DESCRIPT")[0] + NL;
               }
               if (printWar)
               {
                  countWar++;
                  TWar = TWar + Str.GetTimeNow() + " -> " + nameFile + " - " + nameSys + " : " + dataPermit[0] + " = " + Ctrl.GetListDataTable(dtRelatType, dataPermit[0], FinNameCol: "NAME", RetNameCol: "DESCRIPT")[0] + NL;
               }
               if (printOut)
               {
                  foreach (string nameObj in lstObjSel)
                  {
                     TaskExport.Add(new string[] { nameObj, nameSys, nameFile });
                     TOut = TOut + Str.GetTimeNow() + " -> " + string.Join("--", TaskExport[TaskExport.Count - 1].ToArray()) + NL;
                     countOut++;
                  }
               }
            }
         }

         TOut = TOut + Str.GetTimeNow() + " -> Окончание проверки конфигурации экспорта" + NL;

         tbErr.Text = TErr + NL + "Error count: " + countErr;
         tbErr.SelectionStart = tbErr.TextLength;
         tbErr.ScrollToCaret();

         tbWar.Text = TWar + NL + "Warning count: " + countWar;
         tbWar.SelectionStart = tbWar.TextLength;
         tbWar.ScrollToCaret();

         tbOut.Text = TOut + NL + "Out count: " + countOut; ;
         tbOut.SelectionStart = tbOut.TextLength;
         tbOut.ScrollToCaret();

         return result;
      }
      private void btnFileExport_Click(object sender, EventArgs e)
      {
         SaveFileDialog saveFileDialog = new SaveFileDialog();
         saveFileDialog.Filter = "All files(*.*)|*.*";
         if (saveFileDialog.ShowDialog() == DialogResult.OK){
            lblFileExport.Text = saveFileDialog.FileName;
         }
      }
   }

   class AccessProp
   {
      public static string GetPropStr(string name)
      {
         try
         {
            return Properties.Settings.Default[name].ToString();
         }
         catch (Exception ex) { MessageBox.Show(ex.Message); return ""; }
      }

      public static int GetPropInt(string name)
      {
         try
         {
            return int.Parse(Properties.Settings.Default[name].ToString());
         }
         catch (Exception ex) { MessageBox.Show(ex.Message); return 0; }
      }
      public static string SetProp(string name, string val)
      {
         try
         {
            Properties.Settings.Default[name] = val;
            Properties.Settings.Default.Save();
            return "";
         }
         catch (Exception ex) { return ex.Message; }
      }

      public static string SetProp(string name, int val)
      {
         try
         {
            Properties.Settings.Default[name] = val;
            Properties.Settings.Default.Save();
            return "";
         }
         catch (Exception ex) { return ex.Message; }
      }
   }
}
