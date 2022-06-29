using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Threading;
using WorkSQL;
using WorkString;
using WorkStruct;
using WorkMisc;


namespace WorkControl
{
   public class Tag
   {
      public int AccessLevel = 0;
      public List<string> DataString = new List<string>();
   }
   public class Ctrl
   {
      /// <summary>
      /// Поиск строки в таблице по значению в указанном столбце
      /// </summary>
      /// <param name="table"></param>
      /// <param name="NameColumn"></param>
      /// <param name="FindValue"></param>
      /// <returns></returns>
      public static DataGridViewRow Find(DataGridView table, string NameColumn, string FindValue)
      {
         DataGridViewRow result = null;
         if (table != null)
         {
            string strNamesColumn = string.Join("|",GetColumsNames(table));
            if (strNamesColumn.Contains(NameColumn))
            {
               foreach (DataGridViewRow row in table.Rows)
               {
                  if (row.Cells[NameColumn].Value?.ToString() == FindValue)
                  {
                     result = row;
                     break;
                  }
               }
            }
            else
            {
               foreach (DataGridViewRow row in table.Rows)
               {
                  foreach (DataGridViewCell cell in row.Cells)
                  {
                     if (cell.Value?.ToString() == FindValue)
                     {
                        result = row;
                        break;
                     }
                  }
               }
            }
         }
         return result;
      }

      /// <summary>
      /// Поиск строки в таблице по значению в указанном столбце
      /// </summary>
      /// <param name="table"></param>
      /// <param name="NameColumn"></param>
      /// <param name="FindValue"></param>
      /// <returns></returns>
      public static DataRow Find(DataTable table, string NameColumn, string FindValue)
      {
         DataRow result = null;
         if (table != null)
         {
            string strNamesColumn = string.Join("|", GetColumsNames(table));
            if (strNamesColumn.Contains(NameColumn))
            {
               foreach (DataRow row in table.Rows)
               {
                  if (row[NameColumn]?.ToString() == FindValue)
                  {
                     result = row;
                     break;
                  }
               }
            }
            else
            {
               foreach (DataRow row in table.Rows)
               {
                  for(int cell=0; cell< table.Columns.Count; cell++)
                  {
                     if (row[cell]?.ToString() == FindValue)
                     {
                        result = row;
                        break;
                     }
                  }
               }
            }
         }
         return result;
      }

      /// <summary>
      /// Наполнение данными из словаря, объекта ComboBox
      /// </summary>
      /// <param name="comboBox"> Объект ComboBox для наполнения </param>
      /// <param name="dic"> Словарь с данными для наполнения </param>
      /// <param name="index"> Индекс для начального отображения в ComboBox </param>
      public static void Fill(ref ComboBox comboBox, Dictionary<string, string> dic, bool clear = false, int index = 0)
      {
         if (clear) comboBox.Items.Clear();
         foreach (string data in dic.Keys)
         {
            comboBox.Items.Add(data);
         }
         if (comboBox.Items.Count > 0)
            comboBox.SelectedIndex = index;
      }
      public static void Fill(ref ComboBox comboBox, List<string> lst, bool clear = false, int index = 0)
      {
         if (clear) comboBox.Items.Clear();
         foreach (string data in lst)
         {
            comboBox.Items.Add(data);
         }
         if (comboBox.Items.Count > 0)
            comboBox.SelectedIndex = index;
      }
      public static void Fill(ref ListBox listoBox, List<string> lst, bool clear = false, int index = 0)
      {
         if (clear) listoBox.Items.Clear();
         foreach (string data in lst)
         {
            listoBox.Items.Add(data);
         }
         if (listoBox.Items.Count > 0)
            listoBox.SelectedIndex = index;
      }
      public static void Fill(ref TreeView treeView, List<string> lst, TreeViewCancelEventArgs root = null, bool clear = false, int index = 0)
      {
         if (clear || root != null)
         {
            if (root == null)
               treeView.Nodes.Clear();
            else
            {
               root.Node.Nodes.Clear();
            }
         }
         foreach (string data in lst)
         {
            TreeNode node = new TreeNode(data);
            node.Tag = "";
            if (root == null)
            {
               treeView.Nodes.Add(node);
            }
            else
            {
               root.Node.Nodes.Add(node);
            }

         }
      }
      public static void Fill(ref TreeView treeView, Dictionary<string, string> dicData, TreeViewCancelEventArgs root = null, bool clear = false, int index = 0)
      {
         var listData = dicData.ToList();
         listData.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

         if (clear || root != null)
         {
            if (root == null)
               treeView.Nodes.Clear();
            else
            {
               root.Node.Nodes.Clear();
            }
         }
         for (int i = 0; i < listData.Count; i++)
         {
            TreeNode node = new TreeNode(listData[i].Key);
            node.Tag = listData[i].Value;
            if (listData[i].Value.IndexOf("[") > 0)
            {

               int count_arr = int.Parse(listData[i].Value.Split(new char[] { '[' })[1].Replace("]", ""));
               for (int n = 1; n <= count_arr; n++)
               {
                  TreeNode node_next = new TreeNode("[" + n + "]");
                  node_next.Tag = listData[i].Value.Split(new char[] { '[' })[0];
                  node.Nodes.Add(node_next);
               }
            }
            if (root == null)
            {
               treeView.Nodes.Add(node);
            }
            else
            {
               root.Node.Nodes.Add(node);
            }

         }
      }

      ///// <summary>
      ///// Начальное наполнение объекта TreeView
      ///// </summary>
      ///// <param name="treeView"></param>
      ///// <param name="query"></param>
      ///// <param name="name_table_gen"></param>
      //public static void Fill_BDTreeView_Init(ref TreeView treeView, Query query, string name_table_gen = "TypesHead")
      //{
      //   if (query == null) { MessageBox.Show("При вызове Ctrl.Fill_TreeView_Init, параметр query==null!"); return; }
      //   //1.1 Очистка TreeView
      //   treeView.Nodes.Clear();

      //   //1.2 Чтение всех баз данных c проверкой на наличие генераторной таблицы 'TypesObjects' в базе;
      //   List<string> lst_name_db = new List<string>();
      //   if (!query.GetNamesDB(ref lst_name_db, only_gen: true, name_table_gen)) return;

      //   //1.3 Построение узлов-систем из имён БД где есть генераторная таблица;
      //   foreach (string nameDB in lst_name_db)
      //   {
      //      // Создание узла БД системы
      //      TreeNode node_root = new TreeNode();
      //      node_root = new TreeNode(nameDB);
      //      // Коментарий из расширенных свойств база данных
      //      node_root.ToolTipText = query.GetExtPropDB(nameDB);
      //      // Наполнение пустым узлом (для отображения плюсика)
      //      node_root.Nodes.Add("");
      //      // Добавление корневого узла дерева 
      //      treeView.Nodes.Add(node_root);
      //   }
      //}

      ///// <summary>
      ///// Наполнение выделенного корневого узла (системы) контрола TreeView узлами
      ///// </summary>
      ///// <param name="TreeView">Контрол</param>
      ///// <param name="query">Запрос-соединение</param>
      //public static void Fill_ObjTreeView_BD(ref TreeView TreeView, ref Query query, string NameBase, out DataTable dataTable, string NameTableGen = "TypesHead")
      //{
      //   TreeNode node_root = new TreeNode();
         
      //   // Очистка дерева
      //   foreach (TreeNode treeNode in TreeView.Nodes)
      //   {
      //      if (treeNode.Text == NameBase)
      //      {
      //         query.BaseName = NameBase;
      //         //treeView.Nodes.Remove(treeNode);
      //         treeNode.Nodes.Clear();
      //         node_root = treeNode;
      //         break;
      //      }
      //   }
      //   // Чтение таблицы TypesObjects из выделенной системы с выборкой по одному столбцу и сортировкой
      //   dataTable = new DataTable();
      //   dataTable.Namespace = NameTableGen;
      //   if (!query.GetTable(NameTableGen, ref dataTable, NameBase, order: "NAME")) return;
      //   Str.ObjectsNames.Clear();
      //   foreach (DataRow row in dataTable.Rows)
      //   {
      //      TreeNode addNode = new TreeNode(row["NAME"].ToString());
      //      addNode.Nodes.Add("");
      //      addNode.ToolTipText = row["DESCRIPTION"].ToString();
      //      node_root.Nodes.Add(addNode);

      //      Str.ObjectsNames.Add(new string[] { addNode.Text, row["OBJECT_ID"].ToString() });
      //   }
      //}

      ///// <summary>
      ///// Формирование данных по выделенному узлу
      ///// </summary>
      ///// <param name="treeView"></param>
      ///// <param name="query"></param>
      ///// <param name="node_select"></param>
      ///// <param name="data_table"></param>
      //public static void Fill_ObjTreeView_Node(ref TreeView treeView, ref Query query, TreeViewEventArgs node_select, out DataTable data_table)
      //{
      //   // Очистка узла
      //   node_select.Node.Nodes.Clear();
      //   TreeNode treeNode;

      //   // Инициализация
      //   data_table = null;

      //   // Получение информации о выделенно узле
      //   string[] result = node_select.Node.FullPath.Split(new char[] { '\\' });
      //   string obj_db = result[0];                                    // База данных (Система)
      //   string obj_parText = node_select.Node.Parent.Text;            // Имя родителя
      //   string obj_parType = node_select.Node.Parent.Tag?.ToString(); // Тип родителя
      //   string obj_name = node_select.Node.Text;
      //   string obj_type = node_select.Node.Tag?.ToString();

      //   // Переключение на выбронную базу данных
      //   query.BaseName = obj_db;
      //   // Таблица с данными
      //   data_table = new DataTable();
      //   data_table.Namespace = "TypesBody";
      //   // Проверить Выделенный Узел, входит ли в Список Объектов (входит в таблицу TypesObjects):
      //   if (Str.IsObject(obj_name) > 0)
      //   {
      //      query.GetTable("TypesBody", ref data_table, /*select: "SELECT NAME, TYPE, ARRAY_SIZE, DESCRIPTION",*/ where: "WHERE NAME_TYPE = '" + obj_name + "'", order: "NAME");
      //   }
      //   else // Базовый тип
      //   {
      //      query.GetTable("TypesBody", ref data_table, /*select: "SELECT NAME, TYPE, ARRAY_SIZE, DESCRIPTION",*/ where: "WHERE NAME_TYPE = '" + obj_parText + "' AND NAME = '" + obj_name + "'");
      //   }
      //   foreach (DataRow row in data_table.Rows)
      //   {
      //      string type_name = row["TYPE"].ToString();
      //      treeNode = new TreeNode(row["NAME"].ToString());
      //      treeNode.Tag = type_name;
      //      treeNode.ToolTipText = row["DESCRIPTION"].ToString();
      //      // Если тип входит в СО добавить вложенный пустой узел;
      //      if ( Str.IsObject(type_name) > 0 )
      //      {
      //         treeNode.Nodes.Add("");
      //      }
      //      if ( data_table.Rows.Count > 1 & treeNode.Text != "")
      //      {
      //         node_select.Node.Nodes.Add(treeNode);
      //      }
      //   }
      //   return;
      //}

     

      /// <summary>
      /// Получение списка ComboBox из коллекции controls
      /// </summary>
      /// <param name="controls"></param>
      /// <returns></returns>
      public static List<ComboBox> GetComboBoxs(Control.ControlCollection controls)
      {
         List<ComboBox> lstCbx = new List<ComboBox>();
         foreach (Control control in controls)
         {
            if (control is ComboBox)
               lstCbx.Add((ComboBox)control);
         }
         return lstCbx;
      }

      /// <summary>
      /// Получение списка TextoBox из коллекции controls
      /// </summary>
      /// <param name="controls"></param>
      /// <returns></returns>
      public static List<TextBox> GetTextBoxs(Control.ControlCollection controls)
      {
         List<TextBox> lstTbx = new List<TextBox>();
         foreach (Control control in controls)
         {
            if (control is TextBox)
               lstTbx.Add((TextBox)control);
         }
         return lstTbx;
      }

      /// <summary>
      /// Очищение элементов в коллекции элементов
      /// </summary>
      /// <param name="controls"></param>
      /// <returns></returns>
      public static bool Clear(Control.ControlCollection controls)
      {
         bool result = false;
         foreach (Control control in controls)
         {
            if (control is ComboBox)
            {
               ((ComboBox)control).Items.Clear();
               result = true;
            }
            if (control is TextBox)
            {
               ((TextBox)control).Text = "";
               result = true;
            }

         }
         return result;
      }

      /// <summary>
      /// Удаляет данные в выделенных элементах контрола
      /// </summary>
      /// <param name="sender"></param>
      public static bool Clear(object sender)
      {
         try
         {
            DataGridView dgView = sender as DataGridView;
            foreach (DataGridViewCell cell in dgView.SelectedCells)
               cell.Value = "";
            return true;
         }
         catch { }
         return false;
      }

      /// <summary>
      /// Настройка видимости строк по пустым значениям в ячейки
      /// </summary>
      /// <param name="DataTable"></param>
      /// <param name="NameColEmtyCellNotVis"></param>
      /// <returns></returns>
      public static bool TuneRowVisDataGridView(ref DataGridView DataTable, List<string> NameColEmtyCellNotVis=null)
      {
         bool result = false;
         bool AllVis = false;
         if (NameColEmtyCellNotVis == null || NameColEmtyCellNotVis.Count == 0) AllVis = true;
         foreach ( DataGridViewRow row in DataTable.Rows )
         {
            if ( AllVis )
            {
               row.Visible = true;
            }
            else
            {
               foreach (string NameColumn in NameColEmtyCellNotVis)
               {
                  if ( row.Cells[DataTable.Columns[NameColumn].Index].Value == null || row.Cells[NameColumn].Value.ToString() == "" )
                  {
                     try
                     {
                        row.Visible = false;
                     }
                     catch(Exception)
                     {
                     }
                  }
                  
               }
            }
         }
         return result;
      }

      /// <summary>
      /// Настройка столбцов таблицы
      /// </summary>
      /// <param name="dtView">Таблица</param>
      /// <param name="TuneType">Вид настройки: 0-стандарт</param>
      /// <param name="NameColumns">Список имён заголовка</param>
      /// <returns></returns>
      public static bool TuneColumnDataGridView(ref DataGridView dtView, 
                                                List<string> NameColumns = null, 
                                                int TuneType = 0, 
                                                bool ReadOnly = true,
                                                bool AllowAddDellRow = false,
                                                DataTable dt=null)
      {
         // ======================================================
         // Таблица (DataGrid) может быть заполненная данными (Т1) или пустая (Т0)
         // Списко колонок (NameColumns)  есть: (С1) или нет: (С0)
         // ======================================================
         // Нет данных - выход
         bool result = false; 
         if ( dtView.Columns.Count == 0 & NameColumns.Count == 0 ) 
            return result;
         // Создание новой таблице
         bool NewColums = false;
         if ( dtView.Columns.Count == 0 ) 
            NewColums = true;
         // Определение списка столбцов
         List<string> local_NameColumns = new List<string>();
         if (NameColumns.Count == 0)
            local_NameColumns = GetListNameColums(dtView);
         else
            local_NameColumns = NameColumns;

         switch (TuneType)
         {
            case 0:
               {
                  if (NewColums)
                  {
                     foreach (string NameColumn in local_NameColumns)
                     {
                        var column        = new DataGridViewColumn();
                        column.Name       = NameColumn; // Имя
                        column.HeaderText = NameColumn; // Название
                        column.ReadOnly   = ReadOnly;   // Только чтение
                        //column.Frozen   = true;       // На месте
                        column.CellTemplate = new DataGridViewTextBoxCell();           // Тип
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // Авто-ширина
                        column.ToolTipText = dt.Columns[column.DataPropertyName].Namespace;
                        dtView.Columns.Add(column);
                     }
                  }
                  else
                  {
                     TuneOrderColumn(ref dtView, local_NameColumns, VisNotOrderColumns: false);
                     foreach (DataGridViewColumn column in dtView.Columns)
                     {
                        column.ReadOnly     = ReadOnly; // Только чтение
                        //column.CellTemplate = new DataGridViewTextBoxCell();           // Тип
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // Авто-ширина
                        int column_width    = column.Width;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        column.Width        = column_width;
                        column.ToolTipText = dt.Columns[column.DataPropertyName].Namespace;
                     }
                  }
                  dtView.AllowUserToAddRows    = AllowAddDellRow; // Запрет на добавление
                  dtView.AllowUserToDeleteRows = AllowAddDellRow; // Запрет на удаление
                  result = true;
                  break;
               }
         }
         return result;
      }



      /// <summary>
      /// Заполняет данными DataGridView с учётом связанных таблиц  и порядка колонок
      /// </summary>
      /// <param name="DataGrid"></param>
      /// <param name="query"></param>
      /// <returns></returns>
      public static bool Fill(ref DataGridView DataGrid, Query query, ref DataTable table,
                                    string where=null,
                                    string order=null,
                                    List<string> lstOrderColumns=null)
      {
         #region Проверка входных данных
         if (DataGrid==null || DataGrid.Name == ""){
            MessageBox.Show(text: "DataGridView == null || DataGridView.Name == '' !", 
                         caption: "Предупреждение", 
                            icon: MessageBoxIcon.Warning, 
                         buttons: MessageBoxButtons.OK);
            return false;
         }
         #endregion
         #region Инициализация
         DataGrid.Columns.Clear();
         Query queryTemp = new Query(query.GetStrConnect());
         #endregion
         #region Колонки в определённом порядке

         if (lstOrderColumns == null && table.Columns.Count == 0)
         {
            queryTemp.GetNamesColumn(out lstOrderColumns, DataGrid.Name); 
         }
         else if (lstOrderColumns == null && table.Columns.Count > 0)
         {
            lstOrderColumns = new List<string>();
            lstOrderColumns = Ctrl.GetColumsNames(table, true);
         }
         if ( table.Columns.Count == 0 )
         {
            table = new DataTable();
            if (!query.GetTable(DataGrid.Name, out table,
               select: string.Join(",", lstOrderColumns.ToArray()), where: where, order: order))
            {
               return false;
            }
         }


         DataTable tabColLink;
         List<string[]> colLinks = queryTemp.GetForeignKey(table: out tabColLink,
                                                      NameTable: table.Namespace,
                                                      type: Query.GetLinkTableType.CURR);
         DataTable[] dtLinks = new DataTable[colLinks.Count];
         BindingSource[] bsLinks = new BindingSource[colLinks.Count];
         int i = 0;

         foreach (string NameColumn in lstOrderColumns){
            if (Str.Find(colLinks, NameColumn, out string[] colLink,1)){
               queryTemp.GetTable(colLink[2], out dtLinks[i], select: colLink[3], order: colLink[3]);
               bsLinks[i] = new BindingSource();

               if (dtLinks[i].Columns[0].DataType.Name == "Int32")
                  bsLinks[i].DataSource = GetListRowsIntInOneColumnTable(dtLinks[i], 0);
               else
                  bsLinks[i].DataSource = GetListRowsStrInOneColumnTable(dtLinks[i], 0);

               DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
               column.Name = colLink[1];
               column.HeaderText = colLink[1];
               column.DataPropertyName = colLink[1];
               column.DataSource = bsLinks[i];
               column.ToolTipText = DataGrid.Columns.Count.ToString();
               DataGrid.Columns.Add(column);
               i++;
            }
            else if(table.Columns[NameColumn].DataType.Name == "Boolean")
            {
               DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
               column.Name = NameColumn;
               column.HeaderText = NameColumn;
               column.DataPropertyName = NameColumn;
               column.ToolTipText = DataGrid.Columns.Count.ToString();
               DataGrid.Columns.Add(column);
            }
            else
            {
               DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
               column.Name = NameColumn;
               column.HeaderText = NameColumn;
               column.DataPropertyName = NameColumn;
               column.ToolTipText = DataGrid.Columns.Count.ToString();
               DataGrid.Columns.Add(column);
            }
         }
         #endregion
         BindingSource binding = new BindingSource();
         binding.DataSource = table;
         DataGrid.DataSource = binding;
         foreach (DataGridViewColumn col in DataGrid.Columns) col.Visible = false;
         foreach (string col in lstOrderColumns) DataGrid.Columns[col].Visible = true;
         return true;
      }
      public static string[] GetColumsNames(DataTable table)
      {
         List<string> lst_ColumnsNames = new List<string>();
         foreach (DataColumn column in table.Columns)
            lst_ColumnsNames.Add(column.ColumnName);
         return lst_ColumnsNames.ToArray();
      }
      public static List<string> GetColumsNames(DataTable table, bool ListOut=true)
      {
         List<string> lst_ColumnsNames = new List<string>();
         foreach (DataColumn column in table.Columns)
            lst_ColumnsNames.Add(column.ColumnName);
         return lst_ColumnsNames;
      }
      public static string[] GetColumsNames(DataGridView table)
      {
         List<string> lst_ColumnsNames = new List<string>();
         foreach (DataGridViewColumn column in table.Columns)
            lst_ColumnsNames.Add(column.Name);
         return lst_ColumnsNames.ToArray();
      }

      public static int GetColumsWidth(DataGridView table, int MinWidth = 50, int MaxWidth = 100, int AddWidth = 20)
      {
         int result = 0;
         foreach (DataGridViewColumn column in table.Columns)
            if(column.Visible) result = result + column.Width;
         result = result + AddWidth + table.RowHeadersWidth;
         if (result > MaxWidth && MaxWidth > 0) result = MaxWidth;
         if (result < MinWidth && MinWidth > 0) result = MinWidth;
         return result;
      }
      public static int GetRowsHeight(DataGridView table, int MinHeight = 50, int MaxHeight = 100, int AddHeight = 50)
      {
         int result = 0;
         foreach (DataGridViewRow row in table.Rows)
            result = result + row.Height;
         result = result + AddHeight + table.ColumnHeadersHeight;
         if (result > MaxHeight && MaxHeight > 0) result = MaxHeight;
         if (result < MinHeight && MinHeight > 0) result = MinHeight;
         return result;
      }

      public static string[] GetArrayNameColums(DataGridView table)
      {
         List<string> lst_ColumnsNames = new List<string>();
         foreach (DataGridViewColumn column in table.Columns)
            lst_ColumnsNames.Add(column.HeaderText);
         return lst_ColumnsNames.ToArray();
      }

      public static List<string> GetListNameColums(DataGridView table, bool sort = false)
      {
         List<string> lst_ColumnsNames = new List<string>();
         foreach (DataGridViewColumn column in table.Columns)
            lst_ColumnsNames.Add(column.HeaderText);
         if (sort) lst_ColumnsNames.Sort();
         return lst_ColumnsNames;
      }

      public static DataGridView GetDataGridViewRows(DataGridView table, string FindVal, int ColIndex = -1, bool FirstRow = true)
      {
         DataGridView result = new DataGridView();
         List<int> RowsNum = new List<int>();
         int ColStart = ColIndex < 0 ? 0 : ColIndex;
         int ColEnd = ColIndex < 0 ? table.Columns.Count - 1 : ColIndex;

         foreach (DataGridViewRow row in table.Rows)
         {
            for(int i = ColStart; i<= ColEnd; i++)
            {
               if ( row.Cells[i].Value.ToString() == FindVal )
               {
                  RowsNum.Add(row.Index);
                  if (FirstRow)
                  {
                     result = CopyDataGridView(table, RowsNum.ToArray());
                     return result;
                  }
               }
            }
         }
         return CopyDataGridView(table, RowsNum.ToArray());
      }


      

      /// <summary>
      /// Получить данные в TreeLayer.Node, указанные в TreeLayer.Node[0], заданной структуры (NameType), из таблицы (NameTable='TypeBody')
      /// </summary>
      /// <param name="table"></param>
      /// <param name="NameType"></param>
      /// <param name="TreeLayer"></param>
      /// <param name="NameTable"></param>
      /// <param name="NameBase"></param>
      /// <returns></returns>
      public static bool GetDataType(string NameType, out DataTable table, ref Struct.TreeLayer TreeLayer, Query query,
                              string NameBase = "", bool RTN = false, string NameTable = "TypeBody")
      {
         bool result = false;
         table = new DataTable();

         if (TreeLayer.Nodes.Count == 0) return result;

         Dictionary<string, string> CopyNode = new Dictionary<string, string>(TreeLayer.Nodes[0]);
         string[] arrColumnsName = Str.GetArrStrFromDic(CopyNode);
         TreeLayer.Nodes.Clear();

         NameType.Replace("[", "").Replace("]", "");
         Query q = new Query(query.GetStrConnect());

         if (!q.GetTable(NameTable: NameTable,
                       NameBase: NameBase,
                       table: out table,
                       where: "NAME_TYPE = '" + NameType + "'" + (RTN ? " AND PLC_RTN = '1'" : ""),
                       order: "'NAME'")) return false;



         List<string[]> lstRowsData = GetListRowsFromTable(table: table,
                                                   AddColNum: null,
                                                   AddColName: arrColumnsName);
         foreach (string[] RowData in lstRowsData)
         {
            Dictionary<string, string> Node = new Dictionary<string, string>();
            for (int i = 0; i < arrColumnsName.Length; i++)
            {
               Node.Add(key: arrColumnsName[i], value: RowData[i]);
            }
            TreeLayer.Nodes.Add(Str.CopyDict2Str(Node));
         }
         if (TreeLayer.Nodes.Count > 0)
            result = true;
         else
            TreeLayer.Nodes.Add(CopyNode);
         TreeLayer.NodeSelector = 0;

         return result;
      }

      public static DataTable GetRowsDataTable(DataGridView table, 
                                           string FindVal, 
                                           int ColIndex = -1, 
                                           bool FirstRow = true,
                                           int beside = -1)
      {
         DataTable result = new DataTable();
         foreach(DataGridViewColumn column in table.Columns)
         {
            result.Columns.Add(column.Name);
         }
         List<int> RowsNum = new List<int>();
         int ColStart = ColIndex < 0 ? 0 : ColIndex;
         int ColEnd = ColIndex < 0 ? table.Columns.Count - 1 : ColIndex;

         foreach (DataGridViewRow row in table.Rows)
         {
            for (int i = ColStart; i <= ColEnd; i++)
            {
               if (row.Cells[i].Value.ToString() == FindVal)
               {
                  RowsNum.Add(row.Index);
                  if (FirstRow)
                  {
                     result = CopyDataGridViewInDataTable(table, RowsNum.ToArray());
                     return result;
                  }
               }
            }
            // Найдена первая строка и нужно искать только рядом, в диапазоне beside
            if (RowsNum.Count > 0 && beside > 0) {
               if (beside == 0)
                  break;
               beside--; }
         }
         return CopyDataGridViewInDataTable(table, RowsNum.ToArray());
      }

      public static DataGridView CopyDataGridView(DataGridView dgv_org, int[] RowsNum)
      {
         DataGridView dgv_copy = new DataGridView();
         try
         {
            if (dgv_copy.Columns.Count == 0)
            {
               foreach (DataGridViewColumn dgvc in dgv_org.Columns)
               {
                  dgv_copy.Columns.Add(dgvc.Clone() as DataGridViewColumn);
               }
            }

            DataGridViewRow row = new DataGridViewRow();

            for (int i = 0; i < RowsNum.Length; i++)
            {
               row = (DataGridViewRow)dgv_org.Rows[RowsNum[i]].Clone();
               int intColIndex = 0;
               foreach (DataGridViewCell cell in dgv_org.Rows[RowsNum[i]].Cells)
               {
                  row.Cells[intColIndex].Value = cell.Value;
                  intColIndex++;
               }
               dgv_copy.Rows.Add(row);
            }
            dgv_copy.AllowUserToAddRows = false;
            dgv_copy.Refresh();

         }
         catch (Exception ex)
         {
            MessageBox.Show("Copy DataGridViw", ex.Message);
         }
         return dgv_copy;
      }

      public static DataTable CopyDataGridViewInDataTable(DataGridView dgv_org, int[] RowsNum)
      {
         DataTable dt_copy = new DataTable();
         try
         {
            if (dt_copy.Columns.Count == 0)
            {
               foreach (DataGridViewColumn dgvc in dgv_org.Columns)
               {
                  dt_copy.Columns.Add(dgvc.Name);
               }
            }

            for (int i = 0; i < RowsNum.Length; i++)
            {
               DataRow row = dt_copy.NewRow();
               for (int m=0;m< dgv_org.Columns.Count;m++)
               {
                  row[m] = dgv_org.Rows[RowsNum[i]].Cells[m].Value;
               }
               dt_copy.Rows.Add(row);
            }
      }
         catch (Exception ex)
         {
            MessageBox.Show("Copy DataGridViw in DataTable", ex.Message);
         }
         return dt_copy;
      }

      /// <summary>
      /// Настройка порядка отображения столбцов в таблице по списку.
      /// Столбцы таблице, которых нет в сортировочном списке,
      /// остаются без изменений и отображаются с учётом VisNotOrderColumns
      /// </summary>
      /// <param name="dgvData">Таблица данных</param>
      /// <param name="OrderColumns">Последовательность столбцов</param>
      /// <param name="VisNotOrderColumns">Отображение столюцов невключённых в OrderColumns</param>
      /// <returns></returns>
      public static bool TuneOrderColumn(ref DataGridView dgvData, List<string> OrderColumns, bool VisNotOrderColumns = true)
      {
         bool result = false;
         if (OrderColumns.Count == 0 || dgvData.Columns.Count == 0) return result;
         // Настройка отображения столбцов
         foreach ( DataGridViewColumn column in dgvData.Columns )
         {
            column.Visible = VisNotOrderColumns;
         }
         // Список имён столбцов таблицы
         List<string> table_colum_names = Ctrl.GetListNameColums(dgvData);

         for (int i = 0; i < OrderColumns.Count; i++)
         {
            // Проверка на наличие в таблице сортировочного имени
            if (!table_colum_names.Contains(OrderColumns[i])) continue;
            // Именя заменяемой колонки 
            string replase_column_name = dgvData.Columns[OrderColumns[i]].DataPropertyName;
            // Индекс откуда перемещается имя колонки
            int old_column_index = table_colum_names.IndexOf(OrderColumns[i]);

            table_colum_names[i]                = OrderColumns[i];
            dgvData.Columns[i].DataPropertyName = OrderColumns[i];
            dgvData.Columns[i].HeaderText       = OrderColumns[i];
            dgvData.Columns[i].Visible          = true;

            table_colum_names[old_column_index]                = replase_column_name;
            dgvData.Columns[old_column_index].DataPropertyName = replase_column_name;
            dgvData.Columns[old_column_index].HeaderText       = replase_column_name;

            if (i == OrderColumns.Count - 1) result = true;
         }

         return result;
      }
      public static bool SetNumRowsDataGrid(ref DataGridView dataGrid, int NumStart = 0)
      {
         foreach(DataGridViewRow row in dataGrid.Rows)
         {
            row.HeaderCell.Value = (row.Index + NumStart).ToString();
         }
         dataGrid.TopLeftHeaderCell.Value = "Num";
         return true;
      }

      /// <summary>
      /// Устанавливает видимость строк в таблице по найденному значению,
      /// если находит значение в ячейке, равное указанным в val[]
      /// Поиск производится в указанной колонке NumCol, если меньше 0, то ищет везде
      /// </summary>
      /// <param name="table"></param>
      /// <param name="val"></param>
      /// <param name="NumCol"></param>
      /// <returns></returns>
      public static bool SetVisRowDgv(ref DataGridView table, string[] val, int NumCol=0, string NameCol ="", bool MsgYes=true)
      {
         try
         {
            if (NameCol != "") NumCol = table.Columns[NameCol].Index;
            table.CurrentCell = null;
            string data = "|" + string.Join("|", val) + "|";
            foreach (DataGridViewRow row in table.Rows){
               row.Visible = false;
               if (NumCol >= 0 && NumCol <= table.Columns.Count)
               {
                  if (data.Contains("|"+ row.Cells[NumCol].Value.ToString() + "|"))
                     row.Visible = true;
               }
               else
               {
                  foreach (DataGridViewCell cell in row.Cells)
                  {
                     if (data.Contains("|" + cell.Value.ToString() + "|"))
                     {
                        row.Visible = true;
                        break;
                     }
                  }
               }
            }
            return true;
         }
         catch(Exception ex){ if (MsgYes) MessErr(ex); return false; }
      }

      private static DispatcherTimer timer;

      /// <summary>
      /// Инициализация списка объектов и запуск таймера
      /// </summary>
      /// <param name="interval"></param>
      public static void InitBlinkControl(TimeSpan interval)
      {
         timer = new DispatcherTimer() { Interval = interval };
         timer.Tick += Timer_Tick;
         timer.Start();
      }

      
      private static List<Controls> _controls;
      private static void Timer_Tick(object sender, object e)
      {
         foreach(Controls control in _controls)
         {
            if(control.controls.GetType().Name == "Button")
            {
               MessageBox.Show(((Button)control.controls).Text);
            }
         }
      }
      /// <summary>
      /// Сообщение об ошибки
      /// </summary>
      /// <param name="ex"></param>
      /// <param name="caption"></param>
      /// <param name="text"></param>
      /// <param name="buttons"></param>
      /// <param name="icon"></param>
      /// <returns></returns>
      public static DialogResult MessErr(
         Exception ex=null, string caption = "Ошибка", string text="", 
         MessageBoxButtons buttons = MessageBoxButtons.OK, 
         MessageBoxIcon icon = MessageBoxIcon.Error)
      {
         text = text != "" ? "Error: " + text + "\n": text;
         return MessageBox.Show(
            text: text + ex!=null?
               "Message: " + ex.Message + "\n" +
               "Source: " + ex.Source + "\n" +
               "StackTrace: " + ex.StackTrace + "\n" +
               "TargetSite: " + ex.TargetSite + "\n" +
               "HelpLink: " + ex.HelpLink :"",
            caption: caption,
            buttons: buttons,
            icon: icon);
      }
      /// <summary>
      /// Вставляет текстовый данные в таблицу из буфера обмена
      /// </summary>
      /// <param name="table"></param>
      public bool PasteInDataGridViewFromClipboard(ref DataGridView table, bool AllowAddRows = false)
      {
         bool result = false;
         string RawData = Clipboard.GetText().Replace("\r", "");
         if ( RawData[RawData.Length - 1] == '\n' )
            RawData = RawData.Remove(RawData.Length - 1, 1);
         string[] LinesData = RawData.Split(new char[] { '\n' });
         List<List<string>> ResultData = new List<List<string>>();
         // Максимальная длина строки в таблице
         int MaxRows = 0; 
         foreach (string LineData in LinesData)
         {
            string[] arrValues = LineData.Split(new char[] { '\t' });
            List<string> lstValues = new List<string>(arrValues);
            if (MaxRows < lstValues.Count) MaxRows = lstValues.Count;
            ResultData.Add(lstValues);
         }
         int MaxColumns = ResultData.Count;
         // Проверка границ
         if ( table.CurrentCellAddress.X + MaxRows > table.Columns.Count )
         {
            MessageBox.Show(text: "Вставляемый диапазон больше таблицы,\n одна из СТРОК данных длинней!",
                            caption: "Ошибка",
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Error);
            return result;
         }
         if (!AllowAddRows && table.CurrentCellAddress.Y + MaxColumns > table.Rows.Count)
         {
            MessageBox.Show(text: "Вставляемый диапазон больше таблицы,\n один из СТОЛБЦОВ данных длинней!",
                            caption: "Ошибка",
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Error);
            return result;
         }
         
         int StartRow = table.CurrentCellAddress.Y;
         int StartCol = table.CurrentCellAddress.X;

         if (StartRow == -1 || StartCol == -1) return result;

         if (AllowAddRows)
         {
            for (; ; )
            {
               if (ResultData.Count > table.Rows.Count)
                  table.Rows.Add();
               else
                  break;
            }
         }

         // Вставка в таблицу
         for (int OffSetRow = 0; OffSetRow < ResultData.Count; OffSetRow++)
         {
            for (int OffSetCol = 0; OffSetCol < ResultData[OffSetRow].Count; OffSetCol++)
            {
               int NumRow = StartRow + OffSetRow;
               int NumCol = StartCol + OffSetCol;

               table.Rows[NumRow].Cells[NumCol].Value = ResultData[OffSetRow][OffSetCol];
               result = true;
            }
         }
         return result;
      }
      public enum PermitData
      {
         Strings,
         Numbers
      }
      public enum ColumnTrim{ Left, Right}
      public bool PasteInDataTableFromClipboard(ref DataTable table, 
                                                int CurRow=0, 
                                                int CurCol = 0, 
                                                bool AllowAddRows = false,
                                                PermitData permitData = PermitData.Strings)
      {
         ColumnTrim ColTrim = ColumnTrim.Right;
         bool result = false;
         string RawData = Clipboard.GetText().Replace("\r", "");
         if (RawData[RawData.Length - 1] == '\n')
            RawData = RawData.Remove(RawData.Length - 1, 1);
         string[] LinesData = RawData.Split(new char[] { '\n' });
         List<List<string>> ResultData = new List<List<string>>();
         // Максимальная длина строки в таблице
         int MaxColumns = 0;
         foreach (string LineData in LinesData)
         {
            string[] arrValues = LineData.Split(new char[] { '\t' });
            List<string> lstValues = new List<string>(arrValues);
            if (MaxColumns < lstValues.Count) MaxColumns = lstValues.Count;
            ResultData.Add(lstValues);
         }
         int MaxRows = ResultData.Count;
         // Смещение внутри вставляемых данных
         int ResultDataTrim = 0;
         // Проверка границ
         for (; ; )
         {
            if (CurCol + MaxColumns - ResultDataTrim > table.Columns.Count)
            {
               ResultDataTrim++;
            }
            else break;
         }
         if (ResultDataTrim > 0)
         {
            DialogResult DRres =
            MessageBox.Show(text: "Вставляемый диапазон данных больше таблицы, \n"+
                                  "одна из СТРОК длинней! \n" +
                                  "  Yes - обрезка слева\n"+
                                  "  No - обрезка справа\n" +
                                  "  Cancel - отмена вставки\n",
                         caption: "Информация",
                         buttons: MessageBoxButtons.YesNoCancel,
                            icon: MessageBoxIcon.Question);
            if (DRres == DialogResult.Yes) ColTrim = ColumnTrim.Left;
            if (DRres == DialogResult.No) ColTrim = ColumnTrim.Right;
            if (DRres == DialogResult.Cancel) return false;
         }
         if (!AllowAddRows && CurRow + MaxRows > table.Rows.Count)
         {
            MessageBox.Show(text: "Вставляемый диапазон больше таблицы,\n один из СТОЛБЦОВ данных длинней!",
                            caption: "Ошибка",
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Error);
            return result;
         }

         // Вставка в таблицу
         for (int OffSetRow = 0; OffSetRow < ResultData.Count; OffSetRow++)
         {
            for (int OffSetCol = 0; OffSetCol < ResultData[OffSetRow].Count; OffSetCol++)
            {
               int NumRow = CurRow + OffSetRow;
               int NumCol = CurCol + OffSetCol;

               if (AllowAddRows && NumRow >= table.Rows.Count)
                  table.Rows.Add();

               if (table.Rows[NumRow].RowState == DataRowState.Deleted || NumCol > table.Rows[NumRow].ItemArray.Length - 1) continue;

               // Обрезка вставляемых данных если не достаточно столбцов
               // в области куда производиться вставка
               if (ResultDataTrim != 0)
               {
                  if (ColTrim == ColumnTrim.Left)
                  {
                     OffSetCol = OffSetCol + ResultDataTrim;
                  }
                  else
                  {
                     if (OffSetCol + ResultDataTrim > ResultData[OffSetRow].Count - 1) break;
                  }
                  if (OffSetCol > ResultData[OffSetRow].Count - 1) break;
               }

               if (permitData == PermitData.Strings)
                  try
                  {
                     table.Rows[NumRow][NumCol] = ResultData[OffSetRow][OffSetCol];
                     if (ResultDataTrim != 0){
                        if (ColTrim == ColumnTrim.Left) OffSetCol = OffSetCol - ResultDataTrim;
                     }
                  }
                  catch(Exception ex)
                  { 
                     MessageBox.Show(text: ex.Message +
                                           "\n Row: " + NumRow + 
                                           "\n Col: " + NumCol,
                                  caption: "Ошибка",
                                     icon: MessageBoxIcon.Error,
                                  buttons: MessageBoxButtons.OK); 
                    return false;
                  }
               else
               {
                  try
                  {
                     float.Parse(ResultData[OffSetRow][OffSetCol]);
                     table.Rows[NumRow][NumCol] = ResultData[OffSetRow][OffSetCol];
                  }
                  catch
                  {
                     table.Rows[NumRow][NumCol] = 0;
                  }
               }
               result = true;
            }
         }
         return result;
      }
      public enum CopyDataTableType{ byDest, bySource};
      /// <summary>
      /// Копирование данных между таблицами
      /// </summary>
      /// <param name="tabSource"></param>
      /// <param name="tabDest"></param>
      /// <param name="NumColSourceMerge"></param>
      /// <param name="NumColDestMerge"></param>
      /// <param name="type"></param>
      /// <returns></returns>
      public static bool CopyDataTable(DataTable tabSource, ref DataTable tabDest, 
                                       int NumColSourceMerge = -1, int NumColDestMerge = -1, 
                                       CopyDataTableType type = CopyDataTableType.byDest)
      {
         try{
            // Слияние таблиц без учёта данных в таблице назначения
            if (NumColSourceMerge == -1)
            {
               // Копирование в размере таблицы назначения
               if (type == CopyDataTableType.byDest) 
               {
                  for (int r = 0; r < tabDest.Rows.Count; r++)
                  {
                     for (int c = 0; c < tabDest.Columns.Count; c++)
                     {
                        try
                        {
                           tabDest.Rows[r][c] = tabSource.Rows[r][c];
                        }
                        catch{ break; }
                     }
                  }
                  return true;
               }
               // Копирование в размере таблицы источника
               else
               {
                  if (tabSource.Columns.Count > tabDest.Columns.Count)
                  {
                     MessageBox.Show(text: "Количество столбцов в таблице источнике\n БОЛЬШЕ чем в таблице назначения!",
                                  caption: "Информация",
                                  icon: MessageBoxIcon.Exclamation,
                                  buttons: MessageBoxButtons.OK);
                     //return false;
                  }
                  // Добавление недостающих строк
                  for (; ; )
                  {
                     if (tabSource.Rows.Count > tabDest.Rows.Count)
                     {
                        DataRow row = tabDest.NewRow(); tabDest.Rows.Add(row);
                     }
                     else break;
                  }
                  // Копирование данных
                  for (int r = 0; r < tabSource.Rows.Count; r++)
                  {
                     for (int c = 0; c < tabSource.Columns.Count; c++)
                     {
                        try
                        {
                           tabDest.Rows[r][c] = tabSource.Rows[r][c];
                        }
                        catch{ break; }
                     }
                  }
                  return true;
               }
            }
            // Слияние таблиц по данным в определённом столбце таблицы назначения
            else
            {
               if (NumColDestMerge == -1) NumColDestMerge = NumColSourceMerge;
               // Копирование в размере таблицы назначения
               if (type == CopyDataTableType.byDest)
               {
                  for(int rD=0;rD<tabDest.Rows.Count;rD++)
                  {
                     for(int rS=0;rS<tabSource.Rows.Count;rS++)
                     {
                        if(tabDest.Rows[rD][NumColDestMerge].ToString() == tabSource.Rows[rS][NumColSourceMerge].ToString())
                        {
                           for(int cD=0; cD<tabDest.Columns.Count;cD++)
                           {
                              try
                              {
                                 tabDest.Rows[rD][cD] = tabSource.Rows[rS][cD];
                              }
                              catch{ break; }
                           }
                           break;
                        }
                     }
                  }
                  return true;
               }
               // Копирование в размере таблице источника
               else if (type == CopyDataTableType.bySource)
               {
                  if (tabSource.Columns.Count > tabDest.Columns.Count)
                  {
                     MessageBox.Show(text: "Количество столбцов в таблице источнике\n БОЛЬШЕ чем в таблице назначения!",
                                  caption: "Информация",
                                  icon: MessageBoxIcon.Exclamation,
                                  buttons: MessageBoxButtons.OK);
                     //return false;
                  }
                  // Добавление недостающих строк
                  for (; ; )
                  {
                     if (tabSource.Rows.Count > tabDest.Rows.Count)
                     {
                        DataRow row = tabDest.NewRow(); tabDest.Rows.Add(row);
                     }
                     else break;
                  }
                  // Копирование данных
                  for (int rS = 0; rS < tabSource.Rows.Count; rS++)
                  {
                     for (int rD = 0; rD < tabDest.Rows.Count; rD++)
                     {
                        if (tabSource.Rows[rS][NumColDestMerge].ToString() == tabDest.Rows[rD][NumColSourceMerge].ToString())
                        {
                           for (int cS = 0; cS < tabSource.Columns.Count; cS++)
                           {
                              try
                              {
                                 tabDest.Rows[rD][cS] = tabSource.Rows[rS][cS];
                              }
                              catch { break; }
                           }
                           break;
                        }
                     }
                  }
                  return true;
               }
            }
         }
         catch (Exception ex) { MessErr(ex); }

         return false;
      }
      public static DataTable CopyDataTable(DataTable table)
      {
         DataTable result = table.Clone();
         foreach (DataRow row in table.Rows) result.Rows.Add(row.ItemArray);
         return result;
      }
      public static DataRow CopyDataRow(DataRow row)
      {
         DataRow result = row.Table.NewRow();
         result.ItemArray = (object[])row.ItemArray.Clone();
         return result;
      }

      /// <summary>
      /// Возвращает true, если находит хотя бы одну удалённую строку в переданной таблице
      /// </summary>
      /// <param name="table"></param>
      /// <returns></returns>
      public static bool YesDelRowDataTAble(DataTable table)
      {
         foreach(DataRow row in table.Rows) if (row.RowState == DataRowState.Deleted) return true;return false;
      }

      /// <summary>
      /// Добавление/удаление символа '*' в тексте кнопки
      /// </summary>
      /// <param name="button"></param>
      /// <param name="Add"></param>
      public static void ButtonStar(Button button, bool Add = true)
      {
         if (Add)
            button.Text = button.Text + (button.Text.Contains("*") ? "" : "*");
         else
            button.Text = button.Text.Replace("*", "");
      }

      /// <summary>
      /// Проверяет наличие данных в указанной колонке CheckCol 
      /// по найденному значению FindVal в поисковой колонке FindCol
      /// </summary>
      /// <param name="table"></param>
      /// <param name="FindVal"></param>
      /// <param name="FindCol"></param>
      /// <param name="CheckCol"></param>
      /// <param name="CheckYes"></param>
      /// <param name="CheckNo"></param>
      /// <returns></returns>
      public static bool CheckDataTable(ref DataGridView table, string FindVal, string FindCol, string CheckCol, 
                                       System.Drawing.Color CheckYes,
                                       System.Drawing.Color CheckNo)
      {
         bool result = false;
         try
         {
            foreach (DataGridViewRow row in table.Rows)
            {
               if(row.Cells[FindCol].Value.ToString() == FindVal || FindVal == null)
               {
                  if (row.Cells[CheckCol].Value == null || row.Cells[CheckCol].Value.ToString() == "" || row.Cells[CheckCol].Value.ToString().ToLower() == "false")
                  {
                     if(CheckNo!= CheckYes)
                        row.Cells[CheckCol].Style.BackColor = CheckNo;
                     result = false;
                     //if (FindVal != null) break;
                  }
                  else{
                     if(CheckNo != CheckYes)
                        row.Cells[CheckCol].Style.BackColor = CheckYes;
                     result = true;
                     //if (FindVal != null) break;
                  }
               }
            }
         }
         catch (Exception ex){ MessErr(ex); result = false; }
         return result;
      }

      /// <summary>
      /// Проверяет текс переданной кнопки на содержание определённого символа,
      /// при его наличии выдаётся заданное сообщение и возвращается
      /// результат выбора диалогового окна
      /// </summary>
      /// <param name="button"></param>
      /// <param name="symbol"></param>
      /// <param name="text"></param>
      /// <returns></returns>
      public static bool CheckNeedSave(Button button, string symbol = "*", string text = "Имеются несохранённые данные, \n сохранить?")
      {
         if (button.Text.Contains(symbol)){
            DialogResult result = 
               MessageBox.Show(text: text,
                            caption: "Предупреждение",
                               icon: MessageBoxIcon.Question,
                            buttons: MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) return true; }
         return false;
      }


      /// <summary>
      /// Наполение строки данными из строкового массива
      /// </summary>
      /// <param name="row"> Формат строки </param>
      /// <param name="sourse"> Данные для вставки </param>
      /// <param name="fillEmpty"> Включение заполнения пустых </param>
      /// <param name="fillText"> Текст для заполнения пустых </param>
      /// <returns></returns>
      public static DataRow SetDataRow(DataRow row, string[] sourse, bool fillEmpty = false, string fillText = "")
      {
         try
         {
            int count = row.Table.Columns.Count - 1; // Вычитаем столбец ID
            if (count < 0) return row;
            string[] result = new string[row.Table.Columns.Count];
            Str.CopyArrStr(sourse, result, 0, true);
            for (int i = 1; i < row.Table.Columns.Count; i++)
            {
               row[row.Table.Columns[i]] = result[i - 1];
            }
         }
         catch (Exception ex)
         {
            MessErr(ex);
         }
         return row;
      }

      public static DataRow SetDataRow(DataRow row, List<string> sourse, bool fillEmpty = false, string fillText = "")
      {
         try
         {
            int count = row.Table.Columns.Count - 1; // Вычитаем столбец ID
            if (count < 0) return row;
            List<string> result = new List<string>(row.Table.Columns.Count);
            Str.CopyLstStr(sourse, result, 0, true);
            for (int i = 1; i < row.Table.Columns.Count; i++)
            {
               row[i] = result[i - 1];
            }
         }
         catch (Exception ex)
         {
            MessErr(ex);
         }
         return row;
      }

      /// <summary>
      /// Получение списка значений строк из одной колонки таблицы
      /// </summary>
      /// <param name="table"> Таблица с данными </param>
      /// <param name="ColumnNum"> Номер колонки </param>
      /// <returns></returns>
      public static List<string> GetListRowsStrInOneColumnTable(DataTable table,
                                                       int ColumnNum = 0,
                                                       bool unique = false,
                                                       string ColumnName = "",
                                                       bool NotEmpty = true)
      {
         List<string> lstResult = new List<string>();
         Dictionary<string, string> dicResult = new Dictionary<string, string>();
         try
         {
            foreach (DataRow Row in table.Rows)
            {
               if (unique)
               {
                  if (ColumnName == "")
                  {
                     if (!dicResult.ContainsKey(Row[ColumnNum].ToString()))
                     {
                        if (NotEmpty)
                        {
                           if (Row[ColumnNum].ToString() != "")
                           {
                              dicResult.Add(Row[ColumnNum].ToString(), "value");
                              lstResult.Add(Row[ColumnNum].ToString());
                           }
                        }
                        else
                        {
                           dicResult.Add(Row[ColumnNum].ToString(), "value");
                           lstResult.Add(Row[ColumnNum].ToString());
                        }

                     }
                  }
                  else
                  {
                     if (!dicResult.ContainsKey(Row[ColumnName].ToString()))
                     {
                        if (NotEmpty)
                        {
                           if (Row[ColumnName].ToString() != "")
                           {
                              dicResult.Add(Row[ColumnName].ToString(), "value");
                              lstResult.Add(Row[ColumnName].ToString());
                           }
                        }
                        else
                        {
                           dicResult.Add(Row[ColumnName].ToString(), "value");
                           lstResult.Add(Row[ColumnName].ToString());
                        }
                     }
                  }
               }
               else
               {
                  if (ColumnName == "")
                  {
                     if (NotEmpty)
                     {
                        if (Row[ColumnNum].ToString() != "")
                        {
                           lstResult.Add(Row[ColumnNum].ToString());
                        }
                     }
                     else
                     {
                        lstResult.Add(Row[ColumnNum].ToString());
                     }
                  }
                  else
                  {
                     if (NotEmpty)
                     {
                        if (Row[ColumnName].ToString() != "")
                        {
                           lstResult.Add(Row[ColumnName].ToString());
                        }
                     }
                     else
                     {
                        lstResult.Add(Row[ColumnName].ToString());
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            MessErr(ex);
         }
         return lstResult;
      }

      public static List<int> GetListRowsIntInOneColumnTable(DataTable table,
                                                       int ColumnNum = 0,
                                                       bool unique = false,
                                                       string ColumnName = "",
                                                       bool NotEmpty = true)
      {
         List<int> lstResult = new List<int>();
         Dictionary<string, string> dicResult = new Dictionary<string, string>();
         try
         {
            foreach (DataRow Row in table.Rows)
            {
               if (unique)
               {
                  if (ColumnName == "")
                  {
                     if (!dicResult.ContainsKey(Row[ColumnNum].ToString()))
                     {
                        if (NotEmpty)
                        {
                           if (Row[ColumnNum].ToString() != "")
                           {
                              dicResult.Add(Row[ColumnNum].ToString(), "value");
                              lstResult.Add(int.Parse(Row[ColumnNum].ToString()));
                           }
                        }
                        else
                        {
                           dicResult.Add(Row[ColumnNum].ToString(), "value");
                           lstResult.Add(int.Parse(Row[ColumnNum].ToString()));
                        }

                     }
                  }
                  else
                  {
                     if (!dicResult.ContainsKey(Row[ColumnName].ToString()))
                     {
                        if (NotEmpty)
                        {
                           if (Row[ColumnName].ToString() != "")
                           {
                              dicResult.Add(Row[ColumnName].ToString(), "value");
                              lstResult.Add(int.Parse(Row[ColumnName].ToString()));
                           }
                        }
                        else
                        {
                           dicResult.Add(Row[ColumnName].ToString(), "value");
                           lstResult.Add(int.Parse(Row[ColumnName].ToString()));
                        }
                     }
                  }
               }
               else
               {
                  if (ColumnName == "")
                  {
                     if (NotEmpty)
                     {
                        if (Row[ColumnNum].ToString() != "")
                        {
                           lstResult.Add(int.Parse(Row[ColumnNum].ToString()));
                        }
                     }
                     else
                     {
                        lstResult.Add(int.Parse(Row[ColumnNum].ToString()));
                     }
                  }
                  else
                  {
                     if (NotEmpty)
                     {
                        if (Row[ColumnName].ToString() != "")
                        {
                           lstResult.Add(int.Parse(Row[ColumnName].ToString()));
                        }
                     }
                     else
                     {
                        lstResult.Add(int.Parse(Row[ColumnName].ToString()));
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            MessErr(ex);
         }
         return lstResult;
      }

      public static List<string[]> GetListRowsFromTable(DataTable table,
                                                       int[] AddColNum = null,
                                                       string[] AddColName = null)
      {
         List<string[]> lstResult = new List<string[]>();
         if (AddColNum == null && AddColName == null) return lstResult;
         try
         {
            bool ByName = AddColName?.Length > 0 ? true : false;
            string[] tempReselats = new string[ByName ? AddColName.Length : AddColNum.Length];

            foreach (DataRow Row in table.Rows)
            {
               for (int i = 0; i < tempReselats.Length; i++)
               {
                  tempReselats[i] = ByName ? Row[AddColName[i]].ToString() : Row[AddColNum[i]].ToString();
               }
               lstResult.Add((string[])tempReselats.Clone());
            }
         }
         catch (Exception ex)
         {
            MessErr(ex);
         }
         return lstResult;
      }
      public static List<string[]> GetListRowsFromTable(DataGridView table,
                                                       int[] AddColNum = null,
                                                       string[] AddColName = null,
                                                       string[] ExcColName = null,
                                                       int DataYes = 0,
                                                       bool NotNull = true)
      {
         List<string[]> lstResult = new List<string[]>();
         try
         {
            bool ByName = false;
            bool AllCol = false;
            string[] strNotNull;
            string[] tempReselats;
            if (AddColNum == null & AddColName == null)
               AllCol = true; // Все ячейки строки для копирования
            else
               ByName = AddColName?.Length > 0 ? true : false;
            int take = ExcColName == null ? 0 : ExcColName.Length;
            // Определение индексов столбиков для исключения
            int[] ExcColIdx = new int[take];
            int n = 0;
            if (ExcColIdx.Length > 0)
            {
               foreach (string ColName in ExcColName)
               { ExcColIdx[n] = table.Columns[ColName].Index; n++; }
            }
            // Определение индексов столбиков для чтения
            int[] ReadColIdx = new int[table.Columns.Count - take];
            n = 0;
            string strExcColIdx = Misc.ArrIntToString(ExcColIdx);
            foreach (DataGridViewColumn column in table.Columns)
            {
               if (!strExcColIdx.Contains(column.Index.ToString() + ","))
               { ReadColIdx[n] = column.Index; n++; }
            }

            tempReselats = new string[ByName ? AddColName.Length : (AllCol ? table.Columns.Count - take : AddColNum.Length)];

            foreach (DataGridViewRow Row in table.Rows)
            {
               // Копирование из строки заданных ячеек в темповый массив
               for (int i = 0; i < tempReselats.Length; i++)
               {
                  tempReselats[i] = ByName ? Row.Cells[AddColName[i]].Value?.ToString() : (AllCol ? Row.Cells[ReadColIdx[i]].Value?.ToString() : Row.Cells[AddColNum[i]].Value?.ToString());
               }
               // Все ячейки в строке должны иметь значения для копирования
               if (DataYes == 0)
               {
                  bool Add = true;
                  foreach (string data in tempReselats) if (data == null || data == "") Add = false;
                  if (Add)
                  {
                     if (NotNull)
                     {
                        strNotNull = Str.CopyArrStr(source: tempReselats, NotNull: true);
                        lstResult.Add((string[])strNotNull.Clone());
                     }
                     else
                        lstResult.Add((string[])tempReselats.Clone());
                  }
               }
               // Определённое количество ячейк в строке должны иметь значения для копирования
               else if (DataYes > 0)
               {
                  int Add = DataYes;
                  foreach (string data in tempReselats) if (data != null & data != "") Add--;
                  if (Add <= 0)
                  {
                     if (NotNull)
                     {
                        strNotNull = Str.CopyArrStr(source: tempReselats, NotNull: true);
                        lstResult.Add((string[])strNotNull.Clone());
                     }
                     else
                        lstResult.Add((string[])tempReselats.Clone());
                  }
               }
               // Все строки копируются не зависимо от содержания ячеек
               else
               {
                  if (NotNull)
                  {
                     strNotNull = Str.CopyArrStr(source: tempReselats, NotNull: true);
                     lstResult.Add((string[])strNotNull.Clone());
                  }
                  else
                     lstResult.Add((string[])tempReselats.Clone());
               }
            }
         }
         catch (Exception ex)
         {
            MessErr(ex);
         }
         return lstResult;
      }

      /// <summary>
      /// Получение списка значений строк из трёх колонок таблицы
      /// </summary>
      /// <param name="table"> Таблица с данными </param>
      /// <param name="ColumnNum1"> Номер колонки </param>
      /// <returns></returns>
      public static List<string[]> GetListRowsInThreeColumnTable(DataTable table,
                                                       int ColumnNum1 = 0,
                                                       int ColumnNum2 = 1,
                                                       int ColumnNum3 = 2,
                                                       string ColumnName1 = "",
                                                       string ColumnName2 = "",
                                                       string ColumnName3 = "",
                                                       bool unique = false,
                                                       bool NotEmpty = true)
      {
         List<string[]> lstResult = new List<string[]>();
         Dictionary<string, string> dicResult = new Dictionary<string, string>();
         try
         {
            foreach (DataRow Row in table.Rows)
            {
               if (unique)
               {
                  if (ColumnName1 == "")
                  {
                     if (!dicResult.ContainsKey(Row[ColumnNum1].ToString()))
                     {
                        if (NotEmpty)
                        {
                           if (Row[ColumnNum1].ToString() != "")
                           {
                              dicResult.Add(Row[ColumnNum1].ToString(), "value");
                              lstResult.Add(new string[] { Row[ColumnNum1].ToString(),
                                                           Row[ColumnNum2].ToString(),
                                                           Row[ColumnNum3].ToString() });
                           }
                        }
                        else
                        {
                           dicResult.Add(Row[ColumnNum1].ToString(), "value");
                           lstResult.Add(new string[] { Row[ColumnNum1].ToString(),
                                                        Row[ColumnNum2].ToString(),
                                                        Row[ColumnNum3].ToString() });
                        }

                     }
                  }
                  else
                  {
                     if (!dicResult.ContainsKey(Row[ColumnName1].ToString()))
                     {
                        if (NotEmpty)
                        {
                           if (Row[ColumnName1].ToString() != "")
                           {
                              dicResult.Add(Row[ColumnName1].ToString(), "value");
                              lstResult.Add(new string[] { Row[ColumnName1].ToString(),
                                                           Row[ColumnName2].ToString(),
                                                           Row[ColumnName3].ToString() });
                           }
                        }
                        else
                        {
                           dicResult.Add(Row[ColumnName1].ToString(), "value");
                           lstResult.Add(new string[] { Row[ColumnName1].ToString(),
                                                        Row[ColumnName2].ToString(),
                                                        Row[ColumnName3].ToString() });
                        }
                     }
                  }
               }
               else
               {
                  if (ColumnName1 == "")
                  {
                     if (NotEmpty)
                     {
                        if (Row[ColumnNum1].ToString() != "")
                        {
                           lstResult.Add(new string[] { Row[ColumnNum1].ToString(),
                                                        Row[ColumnNum2].ToString(),
                                                        Row[ColumnNum3].ToString() });
                        }
                     }
                     else
                     {
                        lstResult.Add(new string[] { Row[ColumnNum1].ToString(),
                                                     Row[ColumnNum2].ToString(),
                                                     Row[ColumnNum3].ToString() });
                     }
                  }
                  else
                  {
                     if (NotEmpty)
                     {
                        if (Row[ColumnName1].ToString() != "")
                        {
                           lstResult.Add(new string[] { Row[ColumnName1].ToString(),
                                                        Row[ColumnName2].ToString(),
                                                        Row[ColumnName3].ToString() });
                        }
                     }
                     else
                     {
                        lstResult.Add(new string[] { Row[ColumnName1].ToString(),
                                                     Row[ColumnName2].ToString(),
                                                     Row[ColumnName3].ToString() });
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            MessErr(ex);
         }
         return lstResult;
      }

      /// <summary>
      /// Получение списка значений строк из двух колонок таблицы
      /// </summary>
      /// <param name="table"> Таблица с данными </param>
      /// <param name="ColumnNum1"> Номер колонки </param>
      /// <returns></returns>
      public static List<string[]> GetListRowsInTwoColumnTable(DataTable table,
                                                       int ColumnNum1 = 0,
                                                       int ColumnNum2 = 1,
                                                       bool unique = false,
                                                       string ColumnName1 = "",
                                                       string ColumnName2 = "",
                                                       bool NotEmpty = true)
      {
         List<string[]> lstResult = new List<string[]>();
         Dictionary<string, string> dicResult = new Dictionary<string, string>();
         try
         {
            foreach (DataRow Row in table.Rows)
            {
               if (unique)
               {
                  if (ColumnName1 == "")
                  {
                     if (!dicResult.ContainsKey(Row[ColumnNum1].ToString()))
                     {
                        if (NotEmpty)
                        {
                           if (Row[ColumnNum1].ToString() != "")
                           {
                              dicResult.Add(Row[ColumnNum1].ToString(), "value");
                              lstResult.Add(new string[] { Row[ColumnNum1].ToString(),
                                                           Row[ColumnNum2].ToString() });
                           }
                        }
                        else
                        {
                           dicResult.Add(Row[ColumnNum1].ToString(), "value");
                           lstResult.Add(new string[] { Row[ColumnNum1].ToString(),
                                                        Row[ColumnNum2].ToString() });
                        }

                     }
                  }
                  else
                  {
                     if (!dicResult.ContainsKey(Row[ColumnName1].ToString()))
                     {
                        if (NotEmpty)
                        {
                           if (Row[ColumnName1].ToString() != "")
                           {
                              dicResult.Add(Row[ColumnName1].ToString(), "value");
                              lstResult.Add(new string[] { Row[ColumnName1].ToString(),
                                                           Row[ColumnName2].ToString() });
                           }
                        }
                        else
                        {
                           dicResult.Add(Row[ColumnName1].ToString(), "value");
                           lstResult.Add(new string[] { Row[ColumnName1].ToString(),
                                                        Row[ColumnName2].ToString() });
                        }
                     }
                  }
               }
               else
               {
                  if (ColumnName1 == "")
                  {
                     if (NotEmpty)
                     {
                        if (Row[ColumnNum1].ToString() != "")
                        {
                           lstResult.Add(new string[] { Row[ColumnNum1].ToString(),
                                                        Row[ColumnNum2].ToString() });
                        }
                     }
                     else
                     {
                        lstResult.Add(new string[] { Row[ColumnNum1].ToString(),
                                                     Row[ColumnNum2].ToString() });
                     }
                  }
                  else
                  {
                     if (NotEmpty)
                     {
                        if (Row[ColumnName1].ToString() != "")
                        {
                           lstResult.Add(new string[] { Row[ColumnName1].ToString(),
                                                        Row[ColumnName2].ToString() });
                        }
                     }
                     else
                     {
                        lstResult.Add(new string[] { Row[ColumnName1].ToString(),
                                                     Row[ColumnName2].ToString() });
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            MessErr(ex);
         }
         return lstResult;
      }

      public static List<string> GetListRowsInOneColumnTable(DataTable table, string name, bool unique = false)
      {
         List<string> lstResult = new List<string>();
         Dictionary<string, string> dicResult = new Dictionary<string, string>();
         try
         {
            foreach (DataRow Row in table.Rows)
            {
               if (unique)
               {
                  if (!dicResult.ContainsKey(Row[name].ToString()))
                  {
                     dicResult.Add(Row[name].ToString(), "value");
                     lstResult.Add(Row[name].ToString());
                  }
               }
               else
               {
                  lstResult.Add(Row[name].ToString());
               }
            }
         }
         catch (Exception ex)
         {
            MessErr(ex);
         }
         return lstResult;
      }

      /// <summary>
      /// Наполнение словаря данными из таблицы
      /// </summary>
      /// <param name="table">Таблица</param>
      /// <param name="num">Номер столбца для ключа</param>
      /// <param name="num2">Номер столбца для значения</param>
      /// <param name="num3">Номер столбца для доп данных для замены символа</param>
      /// <param name="find_text">Символы для замены</param>
      /// <returns></returns>
      public static Dictionary<string, string> GetListRowsInOneColumnTable(DataTable table, int num, int num2, int num3 = 5, string find_text = "[]")
      {
         Dictionary<string, string> dicResult = new Dictionary<string, string>();
         try
         {
            foreach (DataRow Row in table.Rows)
            {
               if (!dicResult.ContainsKey(Row[num].ToString()))
                  dicResult.Add(Row[num].ToString(), Row[num2].ToString().IndexOf(find_text) > 0 ? Row[num2].ToString().Replace("[]", "[" + Row[num3].ToString() + "]") : Row[num2].ToString());
            }
         }
         catch (Exception ex)
         {
            MessErr(ex);
         }
         return dicResult;
      }

      /// <summary>
      /// Получает список значений из указанной строки (numRow) таблицы
      /// или из строки по найденному значению (findVal) в указанном столбце (findCol) или будет искать везде
      /// </summary>
      /// <param name="table"></param>
      /// <param name="findVal"></param>
      /// <param name="findCol"></param>
      /// <param name="numRow"></param>
      /// <returns></returns>
      public static List<string> GetListDataTableRow(DataTable table, string findVal=null, string findCol=null, int? numRow = null)
      {
         if (numRow == null && findVal == null)
         {
            MessErr(text: "Не задан номер строки (numRow==null) \nи не задано значения для поиска (findVal=null)");
            return null;
         }
         if (findVal!=null){
            if (findCol == null)
            {
               for (int r = 0; r < table.Rows.Count; r++)
               {
                  foreach (object cell in table.Rows[r].ItemArray)
                  {
                     if (cell.ToString() == findVal)
                     {
                        numRow = r;
                     }
                  }
               }
            }
            else{
               for (int r=0; r<table.Rows.Count; r++)
               {
                  try
                  {
                     if (table.Rows[r][findCol].ToString() == findVal)
                     {
                        numRow = r;
                     }
                  }
                  catch
                  {
                     MessErr(text: "Нет столбца с таким наименованием findCol = " + findCol);
                  }
               }
            }
         }

         if (numRow == null || numRow < 0 || numRow > table.Columns.Count)
         {
            MessErr(text: "Номер строки, для получения данных, не корретен: " + numRow.ToString());
            return null;
         }
         try
         {
            List<string> result = new List<string>();
            foreach (object cell in table.Rows[(int)numRow].ItemArray)
            {
               result.Add(cell.ToString());
            }
            return result;
         }
         catch (Exception ex)
         {
            MessErr(ex);
            return null;
         }
      }

      /// <summary>
      /// Получает список значений из указанной строки (numRow) таблицы
      /// или из строки по найденному значению (findVal) в указанном столбце (findCol) или будет искать везде
      /// </summary>
      /// <param name="table"></param>
      /// <param name="findVal"></param>
      /// <param name="findCol"></param>
      /// <param name="numRow"></param>
      /// <returns></returns>
      public static List<string> GetListDataTableRow(DataGridView table, string findVal = null, string findCol = null, int? numRow = null)
      {
         if (numRow == null && findVal == null)
         {
            MessErr(text: "Не задан номер строки (numRow==null) \nи не задано значения для поиска (findVal=null)");
            return null;
         }
         if (findVal != null)
         {
            if (findCol == null)
            {
               for (int r = 0; r < table.Rows.Count; r++)
               {
                  foreach (DataGridViewCell cell in table.Rows[r].Cells)
                  {
                     if (cell.Value.ToString() == findVal)
                     {
                        numRow = r;
                     }
                  }
               }
            }
            else
            {
               for (int r = 0; r < table.Rows.Count; r++)
               {
                  try
                  {
                     if (table.Rows[r].Cells[findCol].Value.ToString() == findVal)
                     {
                        numRow = r;
                     }
                  }
                  catch
                  {
                     MessErr(text: "Нет столбца с таким наименованием findCol = " + findCol);
                  }
               }
            }
         }

         if (numRow == null || numRow < 0 || numRow > table.Columns.Count)
         {
            MessErr(text: "Номер строки, для получения данных, не корретен: " + numRow.ToString());
            return null;
         }
         try
         {
            List<string> result = new List<string>();
            foreach (DataGridViewCell cell in table.Rows[(int)numRow].Cells)
            {
               result.Add(cell.Value.ToString());
            }
            return result;
         }
         catch (Exception ex)
         {
            MessErr(ex);
            return null;
         }
      }

      /// <summary>
      /// Получает массив значений из row если любая cell == FindVal. 
      /// NumColGetNull: 
      /// "больше || = 0" - index column null-value,
      /// "=-1" - all null-value,
      /// "меньше-1" - not null-value.
      /// Если null-value ограничен(меньше -1 или больше или = 0) результирующий массив сжимается
      /// </summary>
      /// <param name="table"></param>
      /// <param name="FindVal"></param>
      /// <param name="GetNumColNull "></param>
      /// <returns></returns>
      public static string[] GetListColumnsInOneRowTable(DataGridView table, string FindVal, int GetNumColNull = -1, int GetStartCol = 0)
      {
         List<string> lstResult = new List<string>();
         try
         {
            foreach (DataGridViewRow row in table.Rows)
            {
               foreach (DataGridViewCell cell in row.Cells)
               {
                  if (cell.Value?.ToString() == FindVal)
                  {
                     foreach (DataGridViewCell cellResult in row.Cells)
                     {
                        if (cellResult.ColumnIndex < GetStartCol) continue;
                        if (GetNumColNull != -1 || cellResult.Value != null)
                        {
                           if (cellResult.ColumnIndex == GetNumColNull || cellResult.Value != null)
                              lstResult.Add(cellResult.Value == null ? null : cellResult.Value.ToString());
                        }
                        else if ((GetNumColNull == -1))
                           lstResult.Add(cellResult.Value == null ? null : cellResult.Value.ToString());
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            MessErr(ex);
         }
         return lstResult.ToArray();
      }

      public static List<string> GetListColumnsInOneRowTable(List<List<string>> table, int num, bool unique = false)
      {
         List<string> lstResult = new List<string>();
         Dictionary<string, string> dicResult = new Dictionary<string, string>();
         try
         {
            for (int i = 0; i < table.Count; i++)
            {
               if (unique)
               {
                  if (!dicResult.ContainsKey(table[i][num].ToString()))
                  {
                     dicResult.Add(table[i][num].ToString(), "value");
                     lstResult.Add(table[i][num].ToString());
                  }
               }
               else
               {
                  lstResult.Add(table[i][num].ToString());
               }
            }
         }
         catch (Exception ex)
         {
            MessErr(ex);
         }
         return lstResult;
      }

      /// <summary>
      /// Получение списка имён столбцов по найденной строке, 
      /// где в ячейках есть данные отличающиеся от заданного (NotVal = "None")
      /// </summary>
      /// <param name="table"></param>
      /// <param name="FinVal">Искомое значение ячейки</param>
      /// <param name="FinNumCol">Номер столбца для поиска значения</param>
      /// <param name="FinNameCol">Имя столбца для поиска значения</param>
      /// <param name="NotVal">Значение, добавить столбец, если значение ячейки отличается от этого</param>
      /// <returns></returns>
      public static List<string> GetListColDataTable(DataTable table, string FinVal,
                                                  int FinNumCol = 0,
                                                  string FinNameCol = "",
                                                  string NotVal = "None")
      {
         List<string> result = new List<string>();
         try
         {
            if (FinNameCol != "") FinNumCol = table.Columns[FinNameCol].Ordinal;
         }
         catch { }
         bool FindInCol = (FinNumCol >= 0 && FinNumCol <= table.Columns.Count);
         foreach (DataRow row in table.Rows)
         {
            if (FindInCol) // Поиск в определённой колонке
            {
               // Найдено значение
               if (row[FinNumCol].ToString() == FinVal){
                  // Проход по всем ячейкам строки
                  for(int c=0;c<row.ItemArray.Length;c++){
                     // Кроме колонки поиска
                     if(c!= FinNumCol){
                        // Значение отличное от заданного
                        if(row[c].ToString()!= NotVal){
                           // Добаляем имя колонки
                           result.Add(table.Columns[c].ColumnName);
                        }
                     }
                  }
               }
            }
            else
            {
               for (int cf=0;cf< row.ItemArray.Length;cf++)
               {
                  // Найдено значение
                  if (row[cf].ToString() == FinVal)
                  {
                     // Проход по всем ячейкам строки
                     for (int c = 0; c < row.ItemArray.Length; c++)
                     {
                        // Кроме колонки поиска
                        if (cf != c)
                        {
                           // Значение отличное от заданного
                           if (row[c].ToString() != NotVal)
                           {
                              // Добаляем имя колонки
                              result.Add(table.Columns[c].ColumnName);
                           }
                        }
                     }
                     break;
                  }
               }
            }
         }
         return result;
      }

      /// <summary>
      /// Получает список строковых данных из указанного столбца (RetNumColt)
      /// при условии, если найдено значение (FinVal) в указанном столбце (FinNumCol)
      /// если FinNotVal == true, то будет искать значение не равное FinVal
      /// </summary>
      /// <param name="table"></param>
      /// <param name="FinVal"></param>
      /// <param name="FinNumCol"></param>
      /// <param name="RetNumCol"></param>
      /// <param name="FinNotVal"></param>
      /// <returns></returns>
      public static List<string> GetListDataTable(DataTable table, string FinVal, 
                                                  int FinNumCol = 0, int RetNumCol=1,
                                                  string FinNameCol = "", string RetNameCol="",
                                                  bool FinNotVal = false)
      {
         List<string> result = new List<string>();
         try
         {
            if (FinNameCol != "") FinNumCol = table.Columns[FinNameCol].Ordinal;
            if (RetNameCol != "") RetNumCol = table.Columns[RetNameCol].Ordinal;
         }
         catch{ }
         foreach (DataRow row in table.Rows){
            if (FinNumCol >=0 && FinNumCol <= table.Columns.Count){
               if ((row[FinNumCol].ToString() == FinVal && !FinNotVal) ||
                   (row[FinNumCol].ToString() != FinVal &&  FinNotVal)) result.Add(row[RetNumCol].ToString());
            }
            else{
               foreach(object cell in row.ItemArray){
                  if ((cell.ToString() == FinVal && !FinNotVal) ||
                      (cell.ToString() != FinVal &&  FinNotVal))
                  {
                     result.Add(row[RetNumCol].ToString());
                     break;
                  }
               }
            }
         }
         return result;
      }

      
      /// <summary>
      /// Получает список строковых данных из указанного столбца (RetNumColt)
      /// при условии, если найдено значение (FinVal) в указанном столбце (FinNumCol)
      /// </summary>
      /// <param name="table"></param>
      /// <param name="FinVal"></param>
      /// <param name="FinNumCol"></param>
      /// <param name="RetNumCol"></param>
      /// <param name="FinNameCol"></param>
      /// <param name="RetNameCol"></param>
      /// <param name="VisOnly"></param>
      /// <returns></returns>
      public static List<string> GetListDataTable(DataGridView table, string FinVal,
                                                  int FinNumCol = 0, int RetNumCol = 1,
                                                  string FinNameCol = "", string RetNameCol = "",
                                                  bool VisOnly=false)
      {
         List<string> result = new List<string>();
         try
         {
            if (FinNameCol != "") FinNumCol = table.Columns[FinNameCol].Index;
            if (RetNameCol != "") RetNumCol = table.Columns[RetNameCol].Index;
         }
         catch { }
         foreach (DataGridViewRow row in table.Rows)
         {
            if (VisOnly)
               if (!row.Visible) continue;
            if (FinNumCol >= 0 && FinNumCol <= table.Columns.Count)
            {
               if (row.Cells[FinNumCol].Value.ToString() == FinVal) result.Add(row.Cells[RetNumCol].Value.ToString());
            }
            else
            {
               foreach (DataGridViewCell cell in row.Cells)
               {
                  if (cell.Value.ToString() == FinVal)
                  {
                     result.Add(row.Cells[RetNumCol].Value.ToString());
                     break;
                  }
               }
            }
         }
         return result;
      }

      /// <summary>
      /// Получение списка типов и таблицы типов системы
      /// </summary>
      /// <param name="table"></param>
      /// <param name="lstNameType"></param>
      /// <param name="NameBase"></param>
      /// <returns></returns>
      public static bool GetNamesType(out DataTable table, out List<string[]> lstNameType,
                              Query query, string NameBase = "")
      {
         table = new DataTable();
         lstNameType = new List<string[]>();
         Query q = new Query(query.GetStrConnect());
         if (!q.GetTable(NameTable: "TypeHead",
                       NameBase: NameBase,
                       table: out table,
                       order: "NAME")) return false;

         lstNameType = GetListRowsInTwoColumnTable(table: table,
                                                   ColumnName1: "NAME",
                                                   ColumnName2: "OBJECT_ID",
                                                   unique: true,
                                                   NotEmpty: true);
         return lstNameType.Count > 0 ? true : false;
      }

   }

   /// <summary>
   /// Структура объекта и команды управления им
   /// </summary>
   public class Controls
   {
      public Control controls;
      public Boolean cOn;
   }

   

   
}
