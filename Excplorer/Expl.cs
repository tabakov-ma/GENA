using SQLTuneCon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkBox;
using WorkControl;
using WorkSQL;
using WorkString;
using WorkFile;


namespace Explorer
{
   public partial class Expl : Form
   {
      #region MembersClass

      Entity EntitySQL;             // 
      Query QuerySQL;               // 
      DataTable DataTable;          // 
      OutBox OutBox = new OutBox(); // 
      const int NOT_SELECT = -1;    // 
      string StrConnect = "";       // 
      bool AllowAddDellRow = false;
      bool AddInformChanged = false;
      /// <summary>
      /// Объект настройки таблиц
      /// </summary>
      TuneViewTable tuneViewTable;

      #endregion MembersClass

      /// <summary>
      /// Конструктор класса формы
      /// </summary>
      public Expl()
      {
         InitializeComponent();

         // Testing write from asus 4
         // Проверка всех мониторов и возможности открыть окно по сохранённым координатам
         Screen[] aryScreen;
         aryScreen = Screen.AllScreens;
         bool testX = false, testY = false;
         int X = AccessProp.GetPropInt("X");
         int Y = AccessProp.GetPropInt("Y");
         int Width = AccessProp.GetPropInt("Width");
         int Height = AccessProp.GetPropInt("Height");
         foreach (Screen sc in aryScreen)
         {
            if (X + 10 >= sc.Bounds.X && X < sc.Bounds.X + sc.Bounds.Width) testX = true;
            if (Y + 10 >= sc.Bounds.Y && Y < sc.Bounds.Y + sc.Bounds.Height) testY = true;
         }
         if (!testX || !testY)
         {
            foreach (Screen sc in aryScreen)
            {
               if (sc.Primary)
               {
                  X = sc.Bounds.X;
                  Y = sc.Bounds.Y;
                  Width = sc.Bounds.Width;
                  Height = sc.Bounds.Height;
               }
            }
         }

         // Задание положения и размеров
         {
            this.Left = X;
            this.Top = Y;
            this.Width = Width;
            this.Height = Height;
         }
         // Задание положения разделителей в контейнерах
         {
            try
            {
               spc_H.SplitterDistance = AccessProp.GetPropInt("spc_H_sd");
               spc_H2_H.SplitterDistance = AccessProp.GetPropInt("spc_H2_H_sd");
               spc_H2_H1_V.SplitterDistance = AccessProp.GetPropInt("spc_H2_H1_V_sd");
               spc_H2_H1_V1_H.SplitterDistance = AccessProp.GetPropInt("spc_H2_H1_V1_H_sd");
            }
            catch { }
         }
         // Сущность строки подключения
         EntitySQL = new Entity();
         EntitySQL.Change += Entity_Change;
         Init();

         // Подсказка
         toolTip1.SetToolTip(btnSafeChange, "Сохранить");

         if (spc_H.SplitterDistance >= 50)
         {
            //tsmTune_Tool.Checked = true;
            tsmTune_Tool.CheckState = CheckState.Checked;
         }
         else{
            //tsmTune_Tool.Checked = false;
            tsmTune_Tool.CheckState = CheckState.Unchecked;
         }
      }

      #region Functions

      /// <summary>
      /// Начальные установки (Инициализация)
      /// </summary>
      private void Init()
      {
         StrConnect = AccessProp.GetPropStr("StrConnect");
         if (StrConnect == "")
         {
            OutBox.ShowDialog("Нет сохранённой информации о подключении к БД", "Предупреждение", true);
            return;
         }
         // Сущность строки подключения
         EntitySQL.Fill(StrConnect);
         QuerySQL = new Query(StrConnect);
         SetTreeViewInit(ref trvObjects, QuerySQL);

         // Настройка таблицы
         Query queryTC = new Query(StrCon: QuerySQL.GetStrConnect());
         tuneViewTable = new TuneViewTable(query: queryTC);

         // Выделить первую базу в дереве объектов
         if(trvObjects.Nodes.Count > 0)
            trvObjects.SelectedNode = trvObjects.Nodes[0];
      }

      /// <summary>
      /// Проверка флага изменений данных ( в тексте звёздочка )
      /// </summary>
      private void ChangeSave(ref Label label, Query query, bool YesMessage = true)
      {
         if (label.Text.IndexOf("*") >= 0)
         {
            DialogResult result = DialogResult.None;
            if ( YesMessage )
               result = MessageBox.Show(
                           text:          "Данные были изменены, сохранить данные?",
                           caption:       "Предупреждение",
                           buttons:       MessageBoxButtons.OKCancel,
                           icon:          MessageBoxIcon.Warning,
                           defaultButton: MessageBoxDefaultButton.Button1);
            if ( !YesMessage || result == DialogResult.OK ){
               query.SetTable();
               label.Text = label.Text.Replace("*", "");
            }
         }
      }

      /// <summary>
      /// Установка данных таблице
      /// </summary>
      /// <param name="dgv"></param>
      /// <param name="label"></param>
      /// <param name="dt"></param>
      /// <param name="e"></param>
      private void SetDataGridView(DataGridView dgv, 
                                   Label label, 
                                   DataTable dt, 
                                   TreeViewEventArgs e,
                                   Query query,
                                   bool AllowAddDellRow = false)
      {
         Query queryTemp = new Query(query.GetStrConnect());
         Ctrl.Fill(DataGrid: ref dgv, query: queryTemp, table: ref dt);
         //Query query = new Query(StrCon: QuerySQL.GetStrConnect());
         //tuneViewTable = new TuneViewTable(query: query);

         //dataGrid.Columns.Clear();
         //dataGrid.DataSource = dataTable;
         // Настройка таблицы
         //Query queryTC = new Query(StrCon: QuerySQL.GetStrConnect());
         //tuneViewTable = new TuneViewTable(query: queryTC);
         ////tuneViewTable.NameBase = e.Node.Text;
         //if (dataTable?.Namespace != null)
         //   tuneViewTable.NameTable = dataTable.Namespace;
         ////Ctrl.TuneRowDataGridView(ref dgvObjectsDetails, NameColEmtyCellNotVis: tuneViewTable.NamesColEmptyCellNotVis);
         //Ctrl.TuneColumnDataGridView(dtView: ref dgvObjectsDetails, 
         //                            NameColumns: tuneViewTable.NamesColumns,
         //                            TuneType: 0,
         //                            ReadOnly: false,
         //                            AllowAddDellRow: AllowAddDellRow,
         //                            dataTable);

         label.Text = e.Node.Text + ":";
      }

      /// <summary>
      /// Начальное наполнение объекта TreeView
      /// </summary>
      /// <param name="treeView"></param>
      /// <param name="query"></param>
      /// <param name="name_table_gen"></param>
      public static void SetTreeViewInit(ref TreeView treeView, Query query, string name_table_gen = "TypeHead")
      {
         if (query == null) { MessageBox.Show("При вызове Ctrl.Fill_TreeView_Init, параметр query==null!"); return; }
         //1.1 Очистка TreeView
         treeView.Nodes.Clear();

         //1.2 Чтение всех баз данных c проверкой на наличие генераторной таблицы 'TypesObjects' в базе;
         List<string> lst_name_db = new List<string>();
         if (!query.GetNamesDB(ref lst_name_db, only_gen: true, name_table_gen)) return;

         //1.3 Построение узлов-систем из имён БД где есть генераторная таблица;
         foreach (string nameDB in lst_name_db)
         {
            // Создание узла БД системы
            TreeNode node_root = new TreeNode();
            node_root = new TreeNode(nameDB);
            // Коментарий из расширенных свойств база данных
            DataTable table;
            node_root.ToolTipText = query.GetExtProp(out table, nameDB, name_prop: "Caption");
            // Наполнение пустым узлом (для отображения плюсика)
            node_root.Nodes.Add("");
            // Добавление корневого узла дерева 
            treeView.Nodes.Add(node_root);
         }
      }

      /// <summary>
      /// Наполнение выделенного корневого узла (системы) контрола TreeView узлами
      /// </summary>
      /// <param name="TreeView">Контрол</param>
      /// <param name="query">Запрос-соединение</param>
      public void SetTreeViewDB(ref TreeView TreeView, ref Query query,
                                       string NameBase, out DataTable dataTable, string NameTableGen = "TypeHead", bool instance = false)
      {
         TreeNode node_root = new TreeNode();

         // Очистка дерева
         foreach (TreeNode treeNode in TreeView.Nodes)
         {
            if (treeNode.Text == NameBase)
            {
               query.BaseName = NameBase;
               //treeView.Nodes.Remove(treeNode);
               treeNode.Nodes.Clear();
               node_root = treeNode;
               break;
            }
         }
         // Чтение таблицы TypesObjects из выделенной системы с выборкой по одному столбцу и сортировкой
         dataTable = new DataTable();
         tuneViewTable.NameTable = dataTable.Namespace = NameTableGen;
         
         if (!query.GetTable(NameTable:NameTableGen, 
                             table: out dataTable, 
                             NameBase: NameBase, 
                             order: "NAME",
                             select: string.Join(",", tuneViewTable.NamesColumns.ToArray()))) return;
         Str.ObjectsNames.Clear();
         foreach (DataRow row in dataTable.Rows)
         {
            string name = row["NAME"].ToString();
            bool BaseType = Str.BaseTypeNames.ToLower().Contains(name.ToLower());
            TreeNode addNode = new TreeNode(name);
            addNode.Tag = name;
            if (instance)
            {
               Query queryChCntRow = new Query(query.GetStrConnect());
               if(queryChCntRow.GetCntRowTable(NameTable: "InstBody" + name)>0)
                  addNode.Nodes.Add("");
            }
            else
               if( !BaseType ) addNode.Nodes.Add("");
            addNode.ToolTipText = row["DESCRIPTION"].ToString();
            node_root.Nodes.Add(addNode);
            // Если не базовый тип данных
            if( !BaseType )
               Str.ObjectsNames.Add(new string[] { addNode.Text, row["OBJECT_ID"].ToString() });
         }
      }

      /// <summary>
      /// Формирование данных по выделенному узлу
      /// </summary>
      /// <param name="treeView"></param>
      /// <param name="query"></param>
      /// <param name="node_select"></param>
      /// <param name="data_table"></param>
      public static void SetObjTreeViewNode(ref TreeView treeView, ref Query query,
                                            TreeViewEventArgs node_select, out DataTable data_table,
                                             out bool AllowAddDellRow)
      {
         // Очистка узла
         node_select.Node.Nodes.Clear();
         TreeNode treeNode;

         // Инициализация
         data_table = null;

         // Получение информации о выделенно узле
         string[] result = node_select.Node.FullPath.Split(new char[] { '\\' });
         string obj_db = result[0];                                    // База данных (Система)
         string obj_parText = node_select.Node.Parent.Text;            // Имя родителя
         string obj_parType = node_select.Node.Parent.Tag?.ToString(); // Тип родителя
         string obj_name = node_select.Node.Text;
         string obj_type = node_select.Node.Tag?.ToString();

         // Переключение на выбронную базу данных
         query.BaseName = obj_db;
         // Таблица с данными
         data_table = new DataTable();
         data_table.Namespace = "TypeBody";

         if (Str.IsObject(obj_name) > 0 )
         {
            query.GetTable("TypeBody", out data_table, where: "WHERE NAME_TYPE = '" + obj_name + "'", order: "NAME");
         }
         else if (Str.IsObject(obj_type) > 0)
         {
            query.GetTable("TypeBody", out data_table, where: "WHERE NAME_TYPE = '" + obj_type + "'", order: "NAME");
         }
         else // Базовый тип
         {
            //query.GetTable("TypeBody", ref data_table, where: "WHERE NAME_TYPE = '" + obj_parText + "' AND NAME = '" + obj_name + "'");
            query.GetTable("TypeBody", out data_table, where: "WHERE NAME_TYPE = '" + obj_parType + "' AND NAME = '" + obj_name + "'");
         }
         foreach (DataRow row in data_table.Rows)
         {
            string type_name = row["TYPE"].ToString();
            treeNode = new TreeNode(row["NAME"].ToString());
            treeNode.Tag = type_name;
            treeNode.ToolTipText = row["DESCRIPTION"].ToString();
            // Если тип входит в СО добавить вложенный пустой узел;
            if (Str.IsObject(type_name) > 0)
            {
               treeNode.Nodes.Add("");
            }
            if (Str.IsObject(obj_type) > 0)
            {
               node_select.Node.Nodes.Add(treeNode);
            }
         }
         AllowAddDellRow = data_table.Rows.Count > 1 ? true : false;
         return;
      }

      /// <summary>
      /// Формирование данных по выделенному узлу
      /// </summary>
      /// <param name="treeView"></param>
      /// <param name="query"></param>
      /// <param name="node_select"></param>
      /// <param name="data_table"></param>
      public static void SetInstTreeViewNode(ref TreeView treeView, ref Query query,
                                             TreeViewEventArgs node_select, out DataTable data_table,
                                             out bool AllowAddDellRow)
      {
         // Очистка узла
         node_select.Node.Nodes.Clear();
         TreeNode treeNode;

         // Инициализация
         data_table = null;

         // Получение информации о выделенно узле
         string[] result = node_select.Node.FullPath.Split(new char[] { '\\' });
         string obj_db = result[0];                                    // База данных (Система)
         string obj_parText = node_select.Node.Parent.Text;            // Имя родителя
         string obj_parType = node_select.Node.Parent.Tag?.ToString(); // Тип родителя
         string obj_name = node_select.Node.Text;
         string obj_type = node_select.Node.Tag?.ToString();

         // Переключение на выбронную базу данных
         query.BaseName = obj_db;
         // Таблица с данными
         data_table = new DataTable();
         
         // Проверить Выделенный Узел, входит ли в Список Объектов (входит в таблицу TypesObjects):
         if (Str.IsObject(obj_name) > 0)
         {
            data_table.Namespace = "InstBody" + obj_name;
            query.GetTable("InstBody" + obj_name, out data_table, order: "Num");
            AllowAddDellRow = true;
            foreach (DataRow row in data_table.Rows)
            {
               string type_name = row["Num"].ToString();
               treeNode = new TreeNode(row["Pos"].ToString());
               treeNode.Tag = type_name;
               treeNode.ToolTipText = row["Descript"].ToString();
               node_select.Node.Nodes.Add(treeNode);
            }
         }
         else // Базовый тип
         {
            data_table.Namespace = "InstBody" + obj_parText;
            query.GetTable("InstBody" + obj_parText, out data_table, order: "Num", where: "Pos = '" + obj_name + "'");
            AllowAddDellRow = false;
         }
         return;
      }
      /// <summary>
      /// Вставка из буфера обмена в таблицу данных
      /// </summary>
      private void Clip()
      {
         Ctrl ctrl = new Ctrl();
         foreach (DataGridViewCell cell in dgvObjectsDetails.SelectedCells)
         {
            dgvObjectsDetails.CurrentCell = cell;
            ctrl.PasteInDataGridViewFromClipboard(ref dgvObjectsDetails, AllowAddRows: true);
         }
         dgvObjectsDetails.Refresh();
      }

      #endregion Functions

      #region Events



      #region TREEVIEW // =========================================================
      /// <summary>
      /// Выбор элемента в дереве экземпляров
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void trvInstances_AfterSelect(object sender, TreeViewEventArgs e)
      {
         ChangeSave(ref this.lblNameTableDetail, QuerySQL);
         bool allowAddDellRow = false;

         if (e.Node.Parent == null)
         {
            SetTreeViewDB(ref trvInstances, ref QuerySQL, e.Node.Text, out DataTable, "InstHead", instance: true);
         }
         else
         {
            SetInstTreeViewNode(ref trvInstances, ref QuerySQL, e, 
                                out DataTable,  AllowAddDellRow: out allowAddDellRow);
         }
         Query qEP = new Query(StrCon: QuerySQL.GetStrConnect());
         qEP.SetDataTabColNamespaceFromExtProp(ref DataTable, "Type");

         SetDataGridView(this.dgvObjectsDetails,
                         label: lblNameTableDetail,
                         dt: DataTable,
                         e,
                         QuerySQL,
                         AllowAddDellRow: allowAddDellRow | AllowAddDellRow);
      }

      /// <summary>
      /// Выбор элемента в дереве типов
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void trvObjects_AfterSelect(object sender, TreeViewEventArgs e)
      {
         bool allowAddDellRow = false;
         Query qEP;
         ChangeSave(ref this.lblNameTableDetail, QuerySQL);

         if (e.Node.Parent == null)
         {
            SetTreeViewDB(ref trvObjects, ref QuerySQL, e.Node.Text, out DataTable);
            allowAddDellRow = true;

            qEP = new Query(StrCon: QuerySQL.GetStrConnect());
            txbAddInform.Text = qEP.GetExtProp(table_result: out DataTable dt, name_prop: "Caption");
         }
         else
         {
            SetObjTreeViewNode(ref trvObjects, ref QuerySQL, e, out DataTable, out allowAddDellRow);
         }
         qEP = new Query(StrCon: QuerySQL.GetStrConnect());
         qEP.SetDataTabColNamespaceFromExtProp(ref DataTable, "Type");
         SetDataGridView(this.dgvObjectsDetails, lblNameTableDetail, DataTable, e, QuerySQL,
                         AllowAddDellRow: allowAddDellRow | AllowAddDellRow);
      }
      /// <summary>
      /// Событие перед раскрытием узла
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      void trvBeforeExpand(object sender, TreeViewCancelEventArgs e)
      {
         ((TreeView)sender).SelectedNode = e.Node;
      }
      #endregion TREEVIEW // =========================================================
      /// <summary>
      /// Переключение между вкладками ( Типы | Экземпляры )
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tabMenu_SelectedIndexChanged(object sender, EventArgs e)
      {
         ChangeSave(ref this.lblNameTableDetail, QuerySQL);

         switch (((TabControl)sender).SelectedIndex)
         {
            case 0:
               SetTreeViewInit(ref trvObjects, QuerySQL);   // Типы
               break;
            case 1:
               SetTreeViewInit(ref trvInstances, QuerySQL, "InstHead"); // Экземпляры
               break;
         }
      }

      private void tabExplorer_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (((TabControl)sender).SelectedIndex == 2)
         {
            // Начальная загрузка [Fill_TreeView_Init]
            //Ctrl.Fill_TreeView_Init(ref trvObjects, qObjects);
         }
      }



      private void btnSafeChange_Click_1(object sender, EventArgs e)
      {
         ChangeSave(ref lblNameTableDetail, QuerySQL, false);
         if (AddInformChanged)
         {
            Query qEP = new Query(StrCon: QuerySQL.GetStrConnect());
            qEP.SetExtProp( name_prop: "Caption", val: txbAddInform.Text);
            AddInformChanged = false;
         }
      }

      /// <summary>
      /// Закрытие формы
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void Explorer_FormClosed(object sender, FormClosedEventArgs e)
      {
         AccessProp.SetProp("X", this.Left);
         AccessProp.SetProp("Y", this.Top);
         AccessProp.SetProp("Width", this.Width);
         AccessProp.SetProp("Height", this.Height);
         AccessProp.SetProp("spc_H_sd", this.spc_H.SplitterDistance);
         AccessProp.SetProp("spc_H2_H_sd", this.spc_H2_H.SplitterDistance);
         AccessProp.SetProp("spc_H2_H1_V_sd", this.spc_H2_H1_V.SplitterDistance);
         AccessProp.SetProp("spc_H2_H1_V1_H_sd", this.spc_H2_H1_V1_H.SplitterDistance);
         AccessProp.SetProp("StrConnect", QuerySQL.GetStrConnect());
      }

      private void Entity_Change()
      {
         lblStrCon.Text = EntitySQL.ToStringHidePass();
      }




      #endregion Events // ======================================================================

      #region SPLIT // ===================================================================
      // ======================================================================
      // Контейнеры. Изменение разделителей
      // ======================================================================
      private void spc_H_SplitMov(object sender, SplitterEventArgs e)
      {
         AccessProp.SetProp("SplitterDistanceTop", ((SplitContainer)sender).SplitterDistance);
         if (((SplitContainer)sender).SplitterDistance > 50) ((SplitContainer)sender).SplitterDistance = 50;
      }
      private void spc_H2_H_SplitMov(object sender, SplitterEventArgs e)
      {
         AccessProp.SetProp("SplitterDistanceBottom", ((SplitContainer)sender).SplitterDistance);
      }
      private void spc_H2_H1_V_SplitMov(object sender, SplitterEventArgs e)
      {
         AccessProp.SetProp("SplitterDistanceMain", ((SplitContainer)sender).SplitterDistance);
      }
      private void spc_H2_H1_V1_H_SplitMov(object sender, SplitterEventArgs e)
      {
         AccessProp.SetProp("SplitterDistanceMenuTbp1", ((SplitContainer)sender).SplitterDistance);
      }
      #endregion SPLIT // ===================================================================
      #region DATAGRIDVIEW // ===================================================================
      /// <summary>
      /// Вставка из буфера обмена
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void dgvObjectsDetails_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
      {
         if (e.Button == MouseButtons.Right)
         {
            if (Clipboard.ContainsText() == true)
            {
               Paste paste = new Paste(Cursor.Position.X, Cursor.Position.Y);
               if (paste.ShowDialog() == DialogResult.Yes)
               {
                  Ctrl ctrl = new Ctrl();
                  DataGridView table = (DataGridView)sender;
                  ctrl.PasteInDataGridViewFromClipboard(ref table);
               }
            }
         }
      }
      private void dgvObjectsDetails_KeyPress(object sender, KeyPressEventArgs e)
      {
         // Ctrl + v
         if (e.KeyChar == 22)
            Clip();
         // Backspace
         if (e.KeyChar == 8)
            Ctrl.Clear(sender);
      }
      /// <summary>
      /// Изменение данных в таблице
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void dgvObjectsDetails_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
      {
         if (lblNameTableDetail.Text.IndexOf('*') == -1)
            lblNameTableDetail.Text = lblNameTableDetail.Text + "*";
      }
      #endregion DATAGRIDVIEW // ===================================================================






      private void Expl_KeyPress(object sender, KeyPressEventArgs e){}

      private void txbAddInform_TextChanged(object sender, EventArgs e)
      {
         AddInformChanged = true;
      }

      private void spc_H_Resize(object sender, EventArgs e){}

      #region MENU
      // ======================================================================
      // МЕНЮ
      // ======================================================================
      /// <summary>
      /// Меню. Файл. Сохранить
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tsmSafe_MouseUp(object sender, MouseEventArgs e)
      {
         //qTypes.SetDataTable();
         //QuerySQL.SetDataTable();
      }
      /// <summary>
      /// Меню. Файл. Выход
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tsmExit_MouseUp(object sender, MouseEventArgs e)
      {
         Application.Exit();
      }
      /// <summary>
      /// Меню. БД. Подключение
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tsmTuneConnect_MouseUp(object sender, MouseEventArgs e)
      {
         TuneCon con = new TuneCon();
         con.ShowDialog();
         if (con.ResultStrCon != "")
         {
            AccessProp.SetProp("StrConnect", con.ResultStrCon);
            EntitySQL.Fill(con.ResultStrCon);
            Init();
         }
      }
      private void tsmSafe_Click(object sender, EventArgs e)
      {
         ChangeSave(ref lblNameTableDetail, QuerySQL, false);
      }
      /// <summary>
      /// Меню. БД. Упрваление экземплярами
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tsmInstance_MouseUp(object sender, MouseEventArgs e)
      {
         // Открытие окна настройки экземпляров
         WorkBox.TuneInstance tuneInstance = new TuneInstance(QuerySQL);
         tuneInstance.ShowDialog();
      }
      /// <summary>
      /// Tool Strip Menu Item
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tsmHEXeditor_Click(object sender, EventArgs e)
      {
         HexFileRW hexFileRW = new HexFileRW();
         hexFileRW.Show();
      }
      private void tsmHEXconvertor_Click(object sender, EventArgs e)
      {
         HexFileToCSV hexFileToCSV = new HexFileToCSV();
         hexFileToCSV.Show();
      }
      private void tsmImportCurrTableHexFile_Click(object sender, EventArgs e)
      {

      }
      private void tsmTuneViewTableCol_Click(object sender, EventArgs e)
      {
         try
         {
            // Получить настройки отображения колонок (Порядок и видимость) из БД
            WorkBox.FormTuneTableColumn tuneTableColumn = new FormTuneTableColumn(QuerySQL, DataTable.Namespace);
            tuneTableColumn.ShowDialog();
            // Обновить настройки
            Query query = new Query(StrCon: QuerySQL.GetStrConnect());
            tuneViewTable = new TuneViewTable(query: query);

         }
         catch (Exception ex)
         {
            MessageBox.Show(text: ex.Message + "\n" + ex.StackTrace,
                            caption: "Ошибка",
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Error,
                            defaultButton: MessageBoxDefaultButton.Button1);
         }
      }
      private void tsmTune_Tool_Click(object sender, EventArgs e)
      {
         if (!((System.Windows.Forms.ToolStripMenuItem)sender).Checked)
         {
            spc_H.SplitterDistance = 50;
            ((System.Windows.Forms.ToolStripMenuItem)sender).CheckState = CheckState.Checked;
         }
         else
         {
            spc_H.SplitterDistance = 20;
            ((System.Windows.Forms.ToolStripMenuItem)sender).CheckState = CheckState.Unchecked;
         }
      }
      private void tsmTune_AllowAddRow_Click(object sender, EventArgs e)
      {
         if (!((System.Windows.Forms.ToolStripMenuItem)sender).Checked)
         {
            AllowAddDellRow = true;
            ((System.Windows.Forms.ToolStripMenuItem)sender).CheckState = CheckState.Checked;
         }
         else
         {
            AllowAddDellRow = false;
            ((System.Windows.Forms.ToolStripMenuItem)sender).CheckState = CheckState.Unchecked;
         }
      }

      /// <summary>
      /// Экспорт активной таблицы
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tsmExportCurrTableHexFile_Click(object sender, EventArgs e)
      {
         SaveFileDialog dialog = new SaveFileDialog();
         if (dialog.ShowDialog() == DialogResult.Cancel || DataTable == null || DataTable.Rows.Count == 0) return;
         Files files = new Files();
         QuerySQL.SetDataTabColNamespaceFromExtProp(ref DataTable, "Type");
         files.SetHexFileFromDataTable(PathHexFile: dialog.FileName,
                                       table: DataTable);
      }
      // ======================================================================
      #endregion MENU

      private void tsmTuneTableCreator_Click(object sender, EventArgs e)
      {
         // Настройка таблицы
         Query queryTC = new Query(StrCon: QuerySQL.GetStrConnect());
         tuneViewTable = new TuneViewTable(query: queryTC);

         TableCreator tuneGeneral = new TableCreator(QuerySQL, tuneViewTable);
         tuneGeneral.ShowDialog();
      }

      private void tsmExportCreator_Click(object sender, EventArgs e)
      {
         ExportCreator ExCr = new ExportCreator(QuerySQL);
         ExCr.ShowDialog();
      }
   }
}
