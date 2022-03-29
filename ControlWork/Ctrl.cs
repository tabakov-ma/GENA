using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Threading;
using WorkSQL;
using WorkString;
using System.Text.RegularExpressions;


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

      /// <summary>
      /// Начальное наполнение объекта TreeView
      /// </summary>
      /// <param name="treeView"></param>
      /// <param name="query"></param>
      /// <param name="name_table_gen"></param>
      public static void Fill_TreeView_Init(ref TreeView treeView, Query query, string name_table_gen = "TypesObjects")
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
            node_root.ToolTipText = query.GetExtPropDB(nameDB);
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
      public static void Fill_TreeView_BD(ref TreeView TreeView, ref Query query, string NameBase, out DataTable dataTable, string NameTableGen = "TypesObjects")
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
         dataTable.Namespace = NameTableGen;
         if (!query.GetTable(NameTableGen, ref dataTable, NameBase, order_by: "NAME")) return;
         Str.ObjectsNames.Clear();
         foreach (DataRow row in dataTable.Rows)
         {
            TreeNode addNode = new TreeNode(row["NAME"].ToString());
            addNode.Nodes.Add("");
            addNode.ToolTipText = row["DESCRIPTION"].ToString();
            node_root.Nodes.Add(addNode);

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
      public static void Fill_TreeView_Node(ref TreeView treeView, ref Query query, TreeViewEventArgs node_select, out DataTable data_table)
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
         data_table.Namespace = "Objects";
         // Проверить Выделенный Узел, входит ли в Список Объектов (входит в таблицу TypesObjects):
         if (Str.IsObject(obj_name) > 0)
         {
            query.GetTable("Objects", ref data_table, /*select: "SELECT NAME, TYPE, ARRAY_SIZE, DESCRIPTION",*/ where: "WHERE NAME_TYPE = '" + obj_name + "'", order_by: "NAME");
         }
         else // Базовый тип
         {
            query.GetTable("Objects", ref data_table, /*select: "SELECT NAME, TYPE, ARRAY_SIZE, DESCRIPTION",*/ where: "WHERE NAME_TYPE = '" + obj_parText + "' AND NAME = '" + obj_name + "'");
         }
         foreach (DataRow row in data_table.Rows)
         {
            string type_name = row["TYPE"].ToString();
            treeNode = new TreeNode(row["NAME"].ToString());
            treeNode.Tag = type_name;
            treeNode.ToolTipText = row["DESCRIPTION"].ToString();
            // Если тип входит в СО добавить вложенный пустой узел;
            if ( Str.IsObject(type_name) > 0 )
            {
               treeNode.Nodes.Add("");
            }
            if ( data_table.Rows.Count > 1 & treeNode.Text != "")
            {
               node_select.Node.Nodes.Add(treeNode);
            }
         }
         return;
      }

         // Проверка на ключевые слова в имени узла
         //if (obj_name == "Н/Д")
         //{
         //   return;
         //}

         // Проверка выделенного узла на массив
         // Является массивом
         //if (Str.IsArray(obj_name) == 1)
         //{
         //   int start; int end = Str.GetArraySizeFromStr(obj_name, out start);
         //   if (start > end) { MessageBox.Show("У узла с именем массива начало > конца массива!"); return; }
         //   for (int num = start; num <= end; num++)
         //   {
         //      treeNode = new TreeNode(Str.GetNameArray(obj_name) + "[" + num + "]");
         //      // Проверяем имя родителя
         //      if (Str.IsObject(obj_type) > 0)
         //      {
         //         treeNode.Tag = null; // Объект
         //      }
         //      else
         //      {
         //         treeNode.Tag = Str.GetNameArray(obj_type);
         //         DataTable dataTable = new DataTable();
         //         dataTable.Namespace = "TypesObjects";
         //         query.GetTable("TypesObjects", ref dataTable, /*select: "SELECT DESCRIPTION",*/ where: "WHERE NAME = '" + Str.GetNameArray(obj_type) + "'", order_by: "NAME");
         //         data_table = dataTable;
         //         if (dataTable.Rows.Count == 0)
         //         {
         //            treeNode.ToolTipText = "Базовый тип";
         //         }
         //         else
         //         {
         //            treeNode.ToolTipText = dataTable.Rows[0]["DESCRIPTION"].ToString();
         //         }
         //      }
         //      treeNode.Nodes.Add("");
         //      node_select.Node.Nodes.Add(treeNode);
         //   }
         //   return;
         //}

         //// Является элементом массива
         //else if (Str.IsArray(obj_name) == 2)
         //{
         //   // Если объект
         //   if (Str.IsObject(obj_type) > 0)
         //   {
         //      DataTable dataTable = new DataTable();
         //      dataTable.Namespace = "Objects";
         //      query.GetTable("Objects", ref dataTable, /*select: "SELECT NAME, TYPE, DESCRIPTION",*/ where: "WHERE NAME_TYPE = '" + obj_type + "'", order_by: "NAME");
         //      data_table = dataTable;
         //      if (dataTable.Rows.Count == 0)
         //      {
         //         treeNode = new TreeNode("Н/Д");
         //         treeNode.ToolTipText = "В таблице 'Objects' нет данных с типом " + obj_type;
         //         node_select.Node.Nodes.Add(treeNode);
         //         return;
         //      }
         //      else
         //      {
         //         foreach (DataRow row in dataTable.Rows)
         //         {
         //            treeNode = new TreeNode(row["NAME"].ToString());
         //            treeNode.Tag = row["TYPE"];
         //            treeNode.Nodes.Add("");
         //            treeNode.ToolTipText = row["DESCRIPTION"].ToString();
         //            node_select.Node.Nodes.Add(treeNode);
         //         }
         //         return;
         //      }
         //   }
         //   // Если базовый тип
         //   else
         //   {
         //      treeNode = new TreeNode(obj_type);
         //      treeNode.ToolTipText = "Базовый тип";
         //      node_select.Node.Nodes.Add(treeNode);
         //      return;
         //   }
         //}
         //// Является не массивом
         //else if (Str.IsArray(obj_name) == 0)
         //{
         //   int num;
         //   if (obj_type == "@END@")
         //   {
         //      return;
         //   }
         //   else if (int.TryParse(obj_type, out num) || obj_type == null || obj_type == "" || Str.IsObject(obj_type) > 0) // Объект
         //   {
         //DataTable dataTable = new DataTable();
         //dataTable.Namespace = "Objects";
         //query.GetTable("Objects", ref dataTable, /*select: "SELECT NAME, TYPE, ARRAY_SIZE, DESCRIPTION",*/ where: "WHERE NAME_TYPE = '" + obj_name + "'", order_by: "NAME");
         //data_table = dataTable;
         //      if (dataTable.Rows.Count == 0)
         //      {
         //         treeNode = new TreeNode("Н/Д");
         //         treeNode.ToolTipText = "В таблице 'Objects' нет данных с типом " + obj_type;
         //         node_select.Node.Nodes.Add(treeNode);
         //         return;
         //      }
         //      else
         //      {
         //         foreach (DataRow row in dataTable.Rows)
         //         {
         //            if (row["NAME"].ToString() == "" || row["NAME"] == null)
         //            {
         //               continue;
         //            }
         //            if (Str.IsArray(row["TYPE"].ToString()) == 3)
         //            {
         //               int start = 0;
         //               int end = int.Parse(row["ARRAY_SIZE"].ToString());
         //               if (Str.IsObject(Str.GetNameArray((row["TYPE"].ToString()))) == 1)
         //               {
         //                  start = 1;
         //               }
         //               treeNode = new TreeNode(row["NAME"].ToString() + "[" + start + ".." + end + "]");
         //               treeNode.Tag = row["TYPE"];
         //               treeNode.Nodes.Add("");
         //               treeNode.ToolTipText = row["DESCRIPTION"].ToString();
         //               node_select.Node.Nodes.Add(treeNode);
         //            }
         //            else
         //            {
         //               treeNode = new TreeNode(row["NAME"].ToString());
         //               treeNode.Tag = row["TYPE"];
         //               treeNode.Nodes.Add("");
         //               treeNode.ToolTipText = row["DESCRIPTION"].ToString();
         //               node_select.Node.Nodes.Add(treeNode);
         //            }
         //         }
         //         return;
         //      }
         //   }
         //   else // Базовый тип
         //   {
         //      // Добавляем данные из тега (тип)
         //      treeNode = new TreeNode(obj_type);
         //      treeNode.Tag = "@END@";
         //      treeNode.ToolTipText = "Базовый тип";
         //      node_select.Node.Nodes.Add(treeNode);
         //   }
         //}
      

      //public static void FillTreeViewNode(ref TreeView treeView, Query query, TreeViewCancelEventArgs node_select)
      //{
      //   // Очистка выделенного узла (от вложенных узлов)
      //   node_select.Node.Nodes.Clear();

      //   // Получение от TreeView полного пути расположения выделенного узла (Система/Родительский узел/Выделенный узел)
      //   string[] result = node_select.Node.FullPath.Split(new char[] { '\\' });
      //   string obj_db = result[0];               // Система
      //   string obj_parent = result[0];
      //   if (result.Length > 2)
      //      obj_parent = result[result.Length - 2]; // Родительский узел
      //   string obj_name = node_select.Node.Text; // Выделенный узел
      //   string obj_type = node_select.Node.Tag?.ToString();
      //   DataTable data;   // Таблица с полученными данными

      //   int ARRAY_SIZE = 0; int ARRAY_START = 0;
      //   //Проверка выделенного узла на массив([..])
      //   if (Str.IsArray(obj_name) != 0)
      //   {
      //      result = obj_name.Split(new char[] { '[' });
      //      // Если узел является массивом
      //      if (Str.IsArray(obj_name) == 1)
      //      {
      //         // - инициализация переменной массива размером
      //         ARRAY_SIZE = Str.GetArraySizeFromStr(obj_name, out ARRAY_START);
      //      }
      //      // Если узел является элементом массива
      //      else if (Str.IsArray(obj_name) == 2)
      //      {
      //         // - То нужно определить тип родителя
      //         TreeNode node = new TreeNode(Str.GetStrWithoutSepar(obj_parent));
      //         data = new DataTable();
      //         query.GetTable("TypesObjects", ref data, where: "WHERE NAME_TYPE = '" + node.Text + "'");
      //         // - добавляем пустой узел если есть данные по новому узлу
      //         if (data.Rows.Count > 0)
      //            node.Nodes.Add("");
      //         node_select.Node.Nodes.Add(node);
      //         return;
      //      }
      //      // Чтение таблицы Objects запросом по выделенному узлу 
      //      data = new DataTable();
      //      query.GetTable("Objects", ref data, where: "NAME_TYPE = '" + obj_name + "'", order_by: "NAME");
      //      // Данных нет
      //      if (data.Rows.Count == 0)
      //      {
      //         // Если не массив
      //         if (ARRAY_SIZE == 0)
      //         {
      //            return;
      //         }
      //         //Если массмв
      //         else
      //         {
      //            // Добавляем узлы с 1-го по количество элементов [..] c вложенным пустым узлом
      //            for (int i = 1; i <= ARRAY_SIZE; i++)
      //            {
      //               TreeNode node = new TreeNode("[" + i.ToString() + "]");
      //               node.Nodes.Add("");
      //               node_select.Node.Nodes.Add(node);
      //            }
      //            return;

      //         }
      //      }
      //      // Данные есть
      //      else
      //      {
      //         // Циклом проходим по всем строкам
      //         foreach (DataRow row in data.Rows)
      //         {
      //            // Проверяем тип 'TYPE'
      //            // Если массив
      //            string type = row["TYPE"]?.ToString();
      //            if (Str.IsArray(type) == 3)
      //            {
      //               ARRAY_SIZE = 0;
      //               int.TryParse(row["ARRAY_SIZE"].ToString(), out ARRAY_SIZE);
      //               TreeNode node = new TreeNode(Str.GetStrWithoutSepar(type) + "[" + ARRAY_SIZE.ToString() + "]");
      //               for (int i = 1; i <= ARRAY_SIZE; i++)
      //               {
      //                  node.Nodes.Add("[" + i.ToString() + "]");
      //               }
      //            }
      //            //  Если не массив
      //            else
      //            {
      //               // Чтение таблицы Objects запросом по текущему типу (WHERE NAME_TYPE = 'текущий тип');
      //               DataTable data2 = new DataTable();
      //               query.GetTable("Objects", ref data2, where: "NAME_TYPE = '" + type + "'", order_by: "NAME");
      //               TreeNode node;
      //               // Данных нет
      //               if (data2.Rows.Count == 0)
      //               {
      //                  // Добавляем узел с именим текущего типа без вложенных узлов
      //                  node = new TreeNode(type);
      //               }
      //               // Данные есть
      //               else
      //               {
      //                  // Добавляем узел с именем текущего типа с одним пустым вложенным узлом
      //                  node = new TreeNode(type);
      //                  node.Nodes.Add("");
      //               }
      //               node_select.Node.Nodes.Add(node);
      //               return;
      //            }
      //         }
      //      }

      //   }

      //   // Запрашиваем данные
      //   // Если нет типа, то объект 1-го уровня системы
      //   if (obj_type == null)
      //   {
      //      DataTable dtObjects = new DataTable();
      //      if (!query.GetTable(nameTable: "Objects", table: ref dtObjects, nameBase: obj_db, where: "WHERE NAME_TYPE = '" + obj_name + "'", order_by: "NAME")) return;
      //      TreeNode treeNode = node_select.Node;
      //      if (dtObjects.Rows.Count > 0)
      //         FillNode(dtObjects, ref treeNode);
      //      else
      //         treeNode.Nodes.Add(obj_type);


      //   }
      //}
      //private static void FillNode(DataTable dtObjects, ref TreeNode node_root)
      //{
      //   //DataTable dtObjects = new DataTable();
      //   //if (!query.GetTable(nameTable: obj_table, table: ref dtObjects, name_base: obj_db, expression: "NAME_TYPE = '" + obj_root + "'", name_column_sort: "NAME")) return node_root;
      //   foreach (DataRow row in dtObjects.Rows)
      //   {
      //      // Проверяем тип
      //      // Если массив, то добавляем к имени узла [размер]
      //      TreeNode node = new TreeNode(row["NAME"].ToString());
      //      if (node.Text == "") continue;
      //      int array_size = 0;
      //      if (row["TYPE"].ToString().IndexOf('[') > 0)
      //      {
      //         node.Tag = row["TYPE"].ToString().Replace("[]", "");
      //         array_size = int.Parse(row["ARRAY_SIZE"].ToString());
      //         //node_root = new TreeNode(row["NAME"].ToString() + "[" + array_size.ToString() + "]");
      //         for (int i = 0; i <= array_size; i++)
      //         {
      //            TreeNode node_next = new TreeNode(row["NAME"].ToString() + "[" + (i + 1).ToString() + "]");
      //            node_next.Tag = row["TYPE"].ToString().Replace("[]", "");
      //            if (node_next.Text == "") continue;
      //            node.Nodes.Add(node_next);
      //         }
      //      }
      //      else
      //      {
      //         TreeNode node_next = new TreeNode(row["TYPE"].ToString());
      //         node_root.Tag = row["TYPE"].ToString();
      //         if (node_next.Text == "") continue;
      //         node.Nodes.Add(node_next);
      //      }
      //      node_root.Nodes.Add(node);
      //   }

      //}
      //private void FillTreeNode(TreeNode node, List<string> data)
      //{
      //   try
      //   {
      //      foreach (string el_data in data)
      //      {
      //         TreeNode add_node = new TreeNode(el_data);
      //         node.Nodes.Add(add_node);
      //      }
      //   }
      //   catch (Exception ex) { MessageBox.Show(ex.Message); }
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
      /// <param name="DataTable">Таблица</param>
      /// <param name="TuneType">Вид настройки: 0-стандарт</param>
      /// <param name="NameColumns">Список имён заголовка</param>
      /// <returns></returns>
      public static bool TuneColumnDataGridView(ref DataGridView DataTable, List<string> NameColumns = null, int TuneType = 0, bool ReadOnly = true)
      {
         // ======================================================
         // Таблица (DataGrid) может быть заполненная данными (Т1) или пустая (Т0)
         // Списко колонок (NameColumns)  есть: (С1) или нет: (С0)
         // ======================================================
         // Нет данных - выход
         bool result = false; 
         if ( DataTable.Columns.Count == 0 & NameColumns.Count == 0 ) 
            return result;
         // Создание новой таблице
         bool NewColums = false;
         if ( DataTable.Columns.Count == 0 ) 
            NewColums = true;
         // Определение списка столбцов
         List<string> local_NameColumns = new List<string>();
         if (NameColumns.Count == 0)
            local_NameColumns = GetColumsNamesToList(DataTable);
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
                        DataTable.Columns.Add(column);
                     }
                  }
                  else
                  {
                     TuneOrderColumn(ref DataTable, local_NameColumns, VisNotOrderColumns: false);
                     foreach (DataGridViewColumn column in DataTable.Columns)
                     {
                        column.ReadOnly     = ReadOnly; // Только чтение
                        //column.Frozen     = true;     // На месте
                        column.CellTemplate = new DataGridViewTextBoxCell();           // Тип
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // Авто-ширина
                        int column_width    = column.Width;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        column.Width        = column_width;
                     }
                  }
                  DataTable.AllowUserToAddRows    = false; // Запрет на добавление
                  DataTable.AllowUserToDeleteRows = false; // Запрет на удаление
                  result = true;
                  break;
               }
         }
         return result;
      }


      /// <summary>
      /// Заполнение данными DataGridView из DataTable (с очисткой/добавление в конец) и если есть столбец с номерами строк, то добавление номеров 
      /// </summary>
      /// <param name="DataGrid">Графическая таблица</param>
      /// <param name="table">Логическая таблица</param>
      /// <param name="clear">Предворительная очистка графической таблицы</param>
      /// <param name="order">Порядок данных строки из логической таблицы</param>
      /// <returns></returns>
      public static bool FillDataGridViewFromDataTable(ref DataGridView DataGrid, DataTable table, bool clear = true, params int[] order)
      {
         bool result = false;

         if (DataGrid == null) { MessageBox.Show("DataGridView не инициализарована!"); return result; }
         if (table == null) { MessageBox.Show("DataTable не инициализарована!"); return result; }

         if (order == null)
         {
            if (DataGrid.Columns.Count != table.Columns.Count) { MessageBox.Show("Количество столбцов в DataGridView не равны количеству в  DataTable!"); return result; }
            DataGrid.DataSource = table;
            result = true;
         }
         else
         {
            if (order.Length != table.Columns.Count) { MessageBox.Show("Количество столбцов в DataTable не равны количеству в order!"); return result; }
            foreach (int num in order)
            {
               if (num > table.Columns.Count - 1 || num < 0) { MessageBox.Show("В order не корректное значение, нет в DataTable такого столбца!"); return result; }
            }
            int NumberRow = 0;
            if (clear)
            {
               DataGrid.Rows.Clear();
            }
            else
            {
               NumberRow = DataGrid.Rows.Count;
            }
            // Проверка нумерного столбца для последующей нумерации строк
            string[] NameColumnNum = { "№", "Номер", "номер" };
            int offset = 0;
            if (NameColumnNum.Any(DataGrid.Columns[0].HeaderText.Contains))
            {
               offset = 1;
            }

            foreach (DataRow dataRow in table.Rows)
            {
               DataGrid.Rows.Add();
               if (offset > 0) DataGrid[offset - 1, DataGrid.Rows.Count - 1].Value = NumberRow;
               foreach (int num in order)
               {
                  DataGrid[num + offset, DataGrid.Rows.Count - 1].Value = dataRow[num];
                  NumberRow++;
               }
            }
            result = true;
         }

         return result;
      }
      public static string[] GetColumsNames(DataTable table)
      {
         List<string> lst_ColumnsNames = new List<string>();
         foreach (DataColumn column in table.Columns)
            lst_ColumnsNames.Add(column.ColumnName);
         return lst_ColumnsNames.ToArray();
      }

      public static string[] GetColumsNames(DataGridView table)
      {
         List<string> lst_ColumnsNames = new List<string>();
         foreach (DataGridViewColumn column in table.Columns)
            lst_ColumnsNames.Add(column.HeaderText);
         return lst_ColumnsNames.ToArray();
      }

      public static List<string> GetColumsNamesToList(DataGridView table)
      {
         List<string> lst_ColumnsNames = new List<string>();
         foreach (DataGridViewColumn column in table.Columns)
            lst_ColumnsNames.Add(column.HeaderText);
         return lst_ColumnsNames;
      }

      /// <summary>
      /// Настройка порядка отображения столбцов в таблице по списку.
      /// Столбцы таблице, которых нет в сортировочном списке,
      /// остаются без изменений и отображаются с учётом VisNotOrderColumns
      /// </summary>
      /// <param name="DataTable">Таблица данных</param>
      /// <param name="OrderColumns">Последовательность столбцов</param>
      /// <param name="VisNotOrderColumns">Отображение столюцов невключённых в OrderColumns</param>
      /// <returns></returns>
      public static bool TuneOrderColumn(ref DataGridView DataTable, List<string> OrderColumns, bool VisNotOrderColumns = true)
      {
         bool result = false;
         if (OrderColumns.Count == 0 || DataTable.Columns.Count == 0) return result;
         // Настройка отображения столбцов
         foreach ( DataGridViewColumn column in DataTable.Columns )
         {
            column.Visible = VisNotOrderColumns;
         }
         // Список имён столбцов таблицы
         List<string> table_colum_names = Ctrl.GetColumsNamesToList(DataTable);

         for (int i = 0; i < OrderColumns.Count; i++)
         {
            // Проверка на наличие в таблице сортировочного имени
            if (!table_colum_names.Contains(OrderColumns[i])) continue;
            // Именя заменяемой колонки 
            string replase_column_name = DataTable.Columns[i].DataPropertyName;
            // Индекс откуда перемещается имя колонки
            int old_column_index = table_colum_names.IndexOf(OrderColumns[i]);

            table_colum_names[i] = OrderColumns[i];
            DataTable.Columns[i].DataPropertyName = OrderColumns[i];
            DataTable.Columns[i].HeaderText = OrderColumns[i];
            DataTable.Columns[i].Visible = true;

            table_colum_names[old_column_index] = replase_column_name;
            DataTable.Columns[old_column_index].DataPropertyName = replase_column_name;
            DataTable.Columns[old_column_index].HeaderText = replase_column_name;

            if (i == OrderColumns.Count - 1) result = true;
         }

         return result;
      }

      private static DispatcherTimer timer;
      public static void SetControl(List<Controls> controls)
      {
         _controls = controls;
      }
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

   }

   /// <summary>
   /// Структура объекта и команды управления им
   /// </summary>
   public class Controls
   {
      public Control controls;
      public Boolean cOn;
   }

   /// <summary>
   /// Структура настройки столбца
   /// </summary>
   public class SettingViewColumn : ICloneable
   {
      public string ColNam;
      public bool ColVis;
      public bool RowVis;
      public SettingViewColumn(SettingViewColumn settingColumn)
      {
         ColNam = settingColumn.ColNam;
         ColVis = settingColumn.ColVis;
         RowVis = settingColumn.RowVis;
      }
      public object Clone()
      {
         return MemberwiseClone();
      }
      public SettingViewColumn()
      {
      }
   }

   /// <summary>
   /// Структура настройки таблицы
   /// </summary>
   public class SettingViewTable : ICloneable
   {
      public string NameTable;
      public List<SettingViewColumn> SettingColumns = new List<SettingViewColumn>();
      public object Clone()
      {
         return MemberwiseClone();
      }
   }

   /// <summary>
   /// Структура настройки БД
   /// </summary>
   public class SettingViewBase : ICloneable
   {
      public string Namebase;
      public List<SettingViewTable> SettingTables = new List<SettingViewTable>();
      public object Clone()
      {
         return MemberwiseClone();
      }
   }

   ///// <summary>
   ///// Настройка отображения таблиц (Порядок, видимость столбцов и видимость строк с пустыми значениями)
   ///// из ранее сохранённых настроек либо из данных текущего подключения к БД 
   ///// </summary>
   //public class TuneViewTable
   //{
   //   public string strTuneTableColumn;
   //   public List<SettingViewBase> Settings = new List<SettingViewBase>();
   //   public List<string> NamesBases = new List<string>();
   //   public List<string> NamesTables = new List<string>();
   //   public List<string> NamesColumns = new List<string>();
   //   public List<string> NamesColEmptyCellNotVis = new List<string>();
   //   /// <summary>
   //   /// Фдаг программного заполнения данными таблицы
   //   /// </summary>
   //   public bool FillDataTable { get { return fillDataTable; } }
   //   private bool fillDataTable = false;
   //   private DataGridView dataGridView;
   //   private List<ComboBox> comboBoxes;
   //   private string nameBase = "";
   //   private string nameTable = "";
   //   /// <summary>
   //   /// Чтение/задание текущей БД
   //   /// </summary>
   //   public string NameBase
   //   {
   //      get
   //      {
   //         return nameBase;
   //      }
   //      set
   //      {
   //         if (nameBase != value)
   //         {
   //            NamesTables.Clear();
   //            foreach (SettingViewBase settingBase in Settings)
   //            {
   //               if (settingBase.Namebase == value)
   //               {
   //                  foreach (SettingViewTable settingTable in settingBase.SettingTables)
   //                  {
   //                     NamesTables.Add(settingTable.NameTable);
   //                  }
   //                  nameBase = value;
   //                  NameTable = NamesTables[0];
   //                  if (comboBoxes != null)
   //                  {
   //                     //prog_change = true;
   //                     ComboBox comboBox = comboBoxes[0];
   //                     SetDataComboBox(ref comboBox);
   //                     comboBox = comboBoxes[1];
   //                     SetDataComboBox(ref comboBox);
   //                  }
   //               }
   //            }
   //         }
   //      }
   //   }
   //   /// <summary>
   //   /// Чтение/задание текущей таблицы БД
   //   /// </summary>
   //   public string NameTable
   //   {
   //      get
   //      {
   //         return nameTable;
   //      }
   //      set
   //      {
   //         if (value != nameTable)
   //         {
   //            nameTable = value;
   //            if(dataGridView != null)
   //               SetDataTable(ref dataGridView);
   //            FillNamesColumns();
   //            //FillNamesColEmptyCell();
   //         }
   //      }
   //   }

   //   //private void FillNamesColEmptyCell()
   //   //{
   //   //   NamesColEmptyCell.Clear();
   //   //   foreach(SettingViewBase setting in Settings)
   //   //   {
   //   //      if (setting.Namebase == nameBase)
   //   //      {
   //   //         foreach(SettingViewTable table in setting.SettingTables)
   //   //         {
   //   //            if 
   //   //         }
   //   //      }
   //   //   }
   //   //}

   //   /// <summary>
   //   /// Сохранение настроек в строковую переменную
   //   /// </summary>
   //   public void SafeSettingToStr()
   //   {
   //      if (Settings == null || Settings.Count == 0) return;

   //      List<string> _lstTuneTableColumn = new List<string>();

   //      foreach (SettingViewBase settingBase in Settings)
   //      {
   //         string _strTuneTableColumn = "";
   //         _strTuneTableColumn = "{BasNam=" + settingBase.Namebase;
   //         foreach (SettingViewTable settingTable in settingBase.SettingTables)
   //         {
   //            _strTuneTableColumn = _strTuneTableColumn + "(TabNam=" + settingTable.NameTable;
   //            foreach (SettingViewColumn settingColumn in settingTable.SettingColumns)
   //            {
   //               _strTuneTableColumn = _strTuneTableColumn + ",";
   //               _strTuneTableColumn = _strTuneTableColumn + "ColNam=" + settingColumn.ColNam + ",";
   //               _strTuneTableColumn = _strTuneTableColumn + "ColVis=" + settingColumn.ColVis.ToString().ToLower() + ",";
   //               _strTuneTableColumn = _strTuneTableColumn + "RowVis=" + settingColumn.RowVis.ToString().ToLower();
   //            }
   //            _strTuneTableColumn = _strTuneTableColumn + ")";
   //         }
   //         _strTuneTableColumn = _strTuneTableColumn + "}";
   //         _lstTuneTableColumn.Add(_strTuneTableColumn);
   //      }

   //      // Проверка наличия в ранее сохранённых настройках
   //      if (strTuneTableColumn != "")
   //      {
   //         int i = 0;
   //         foreach (SettingViewBase settingViewBase in Settings)
   //         {
   //            Regex regexBases = new Regex(@"\{BasNam=" + settingViewBase.Namebase + @"[\w,=\(\)_]*\}");
   //            MatchCollection matchesBases = regexBases.Matches(strTuneTableColumn);

   //            if (matchesBases.Count > 0)
   //            {
   //               strTuneTableColumn = regexBases.Replace(strTuneTableColumn, _lstTuneTableColumn[i]);
   //            }
   //            else
   //            {
   //               foreach (string TTC in _lstTuneTableColumn)
   //               {
   //                  strTuneTableColumn = strTuneTableColumn + TTC;
   //               }
   //            }
   //            i++;
   //         }
   //      }
   //      else
   //      {
   //         foreach ( string TTC in _lstTuneTableColumn)
   //         {
   //            strTuneTableColumn = strTuneTableColumn + TTC;
   //         }
   //      }

   //   }
   //   /// <summary>
   //   /// Наполнение данными из сохранённых данных
   //   /// </summary>
   //   /// <param name="val"></param>
   //   public void FillSettingsFromFile(string val=null)
   //   {
   //      Settings.Clear();
   //      if (val == null)
   //         val = strTuneTableColumn;
   //      else
   //         strTuneTableColumn = val;

   //      Regex regexBases = new Regex(@"\{[\w,=\(\)_]*\}"); // Шаблон данных по базам
   //      MatchCollection matchesBases = regexBases.Matches(val);

   //      if (matchesBases.Count > 0)
   //      {
   //         SettingViewBase settingBase = new SettingViewBase(); // Настройки одной базы данных

   //         foreach (Match matchBase in matchesBases)
   //         {
   //            settingBase.SettingTables = new List<SettingViewTable>();

   //            Regex regexNameBase = new Regex(@"(?<=BasNam=)\w+"); // Шаблон по имени базы
   //            MatchCollection matchesNameBase = regexNameBase.Matches(matchBase.Value);
   //            if (matchesNameBase.Count > 0)
   //            {
   //               foreach (Match matchNameBase in matchesNameBase)
   //               {
   //                  settingBase.Namebase = matchNameBase.Value; // Имя базы данных
   //               }
   //            }

   //            Regex regexTables = new Regex(@"TabNam=[\w\,=_]*\)"); // Шаблон данных по таблицам
   //            MatchCollection matchesTables = regexTables.Matches(matchBase.Value);
   //            if (matchesTables.Count > 0)
   //            {
   //               foreach (Match matchTable in matchesTables) // 
   //               {
   //                  SettingViewTable settingTable = new SettingViewTable(); // Настройка одной таблицы

   //                  Regex regexNameTable = new Regex(@"(?<=TabNam=)\w+"); // Шаблон по имени таблице
   //                  MatchCollection matchesNameTable = regexNameTable.Matches(matchTable.Value);
   //                  if (matchesNameTable.Count > 0)
   //                  {
   //                     foreach (Match matchNameTable in matchesNameTable)
   //                     {
   //                        settingTable.NameTable = matchNameTable.Value; // Имя таблицы данных
   //                     }
   //                  }

   //                  Regex regexColName = new Regex(@"(?<=ColNam=)\w+");
   //                  MatchCollection matchColNam = regexColName.Matches(matchTable.Value);
   //                  Regex regexColVis = new Regex(@"(?<=ColVis=)\w+");
   //                  MatchCollection matchColVis = regexColVis.Matches(matchTable.Value);
   //                  Regex regexRowVis = new Regex(@"(?<=RowVis=)\w+");
   //                  MatchCollection matchRowVis = regexRowVis.Matches(matchTable.Value);

   //                  for (int i = 0; i < matchColNam.Count; i++)
   //                  {
   //                     SettingViewColumn settingColumn = new SettingViewColumn();

   //                     settingColumn.ColNam = matchColNam[i].Value;
   //                     settingColumn.ColVis = matchColVis[i].Value == "true";
   //                     settingColumn.RowVis = matchRowVis[i].Value == "true";

   //                     settingTable.SettingColumns.Add((SettingViewColumn)settingColumn.Clone());
   //                  }
   //                  settingBase.SettingTables.Add((SettingViewTable)settingTable.Clone());
   //               }
   //            }
   //            Settings.Add((SettingViewBase)settingBase.Clone());
   //         }
   //         NameBase = Settings[0].Namebase;
   //         FillNamesColumns();
   //      }
   //   }

   //   /// <summary>
   //   /// Наполнение данными из текущего подключения к базе данных
   //   /// </summary>
   //   /// <param name="query"></param>
   //   public void FillSettingFromDataSet(Query query)
   //   {
   //      Settings.Clear();
   //      DataSet dataSet = new DataSet();
   //      query.GetTablesBase(ref dataSet);
   //      SettingViewBase settingBase = new SettingViewBase();
   //      settingBase.Namebase = query.CurrBaseName;
   //      foreach (DataTable dataTable in dataSet.Tables)
   //      {
   //         SettingViewTable settingTable = new SettingViewTable();
   //         settingTable.NameTable = dataTable.Namespace;
   //         foreach (DataColumn dataColumn in dataTable.Columns)
   //         {
   //            SettingViewColumn settingColumn = new SettingViewColumn();
   //            settingColumn.ColNam = dataColumn.ColumnName;
   //            settingColumn.ColVis = true;
   //            settingColumn.RowVis = true;
   //            settingTable.SettingColumns.Add((SettingViewColumn)settingColumn.Clone());
   //         }
   //         settingBase.SettingTables.Add((SettingViewTable)settingTable.Clone());
   //      }
   //      Settings.Add(settingBase);
   //   }
   //   /// <summary>
   //   /// Конструктор
   //   /// </summary>
   //   /// <param name="val"></param>
   //   public TuneViewTable(DataGridView dataGridView=null, List<ComboBox> comboBox=null, string val = null, Query query = null)
   //   {
   //      this.dataGridView = dataGridView;
   //      this.comboBoxes = comboBox;
   //      bool set_cbx = false;
   //      if (val != null && val != "")
   //      {
   //         FillSettingsFromFile(val);
   //         set_cbx = true;
   //      }
   //      else if (query != null)
   //      {
   //         FillSettingFromDataSet(query);
   //         set_cbx = true;
   //      }
   //      else
   //      {
   //         MessageBox.Show(text: "Нет сохранённых данных для настройки и нет подключения к БД!",
   //                         caption: "Ошибка",
   //                         buttons: MessageBoxButtons.OK,
   //                         icon: MessageBoxIcon.Error,
   //                         defaultButton: MessageBoxDefaultButton.Button1);
   //      }
   //      if (this.comboBoxes != null && set_cbx)
   //      {
   //         ComboBox box = comboBoxes[0];
   //         SetDataComboBox(ref box);
   //         box = comboBoxes[1];
   //         SetDataComboBox(ref box);
   //      }

   //   }

   //   /// <summary>
   //   /// Задать данные в таблицу
   //   /// </summary>
   //   /// <param name="table"></param>
   //   public void SetDataTable(ref DataGridView table)
   //   {
   //      // Выход если нет данных
   //      if (table == null || Settings == null || Settings.Count == 0 || NameBase == null || NameTable == null) return;
   //      // Программное редактирование
   //      fillDataTable = true;
   //      // Очистка данных
   //      table.Rows.Clear();
   //      table.Columns.Clear();
   //      // Проход по всем базам
   //      foreach (SettingViewBase settingBase in Settings)
   //      {
   //         // Нахождение выбранной базы
   //         if (settingBase.Namebase == NameBase)
   //         {
   //            // Проход по всем таблицам выбранной базы
   //            foreach (SettingViewTable settingTable in settingBase.SettingTables)
   //            {
   //               // Нахождени выбранной таблицы
   //               if (settingTable.NameTable == NameTable)
   //               {
   //                  // Проход по всем столбцам выбранной таблицы
   //                  foreach (SettingViewColumn settingColumn in settingTable.SettingColumns)
   //                  {
   //                     // Добавляем столбцы
   //                     var column = new DataGridViewCheckBoxColumn();
   //                     column.Name = settingColumn.ColNam; // Имя
   //                     column.HeaderText = settingColumn.ColNam; // Имя
   //                     column.ValueType = typeof(bool);
   //                     column.CellTemplate = new DataGridViewCheckBoxCell();// Тип
   //                     table.Columns.Add(column);
   //                  }
   //                  // Добавляем строки
   //                  var row = new DataGridViewRow();
   //                  row.HeaderCell.Value = "Видимость столбца";
   //                  table.Rows.Add(row);
   //                  row = new DataGridViewRow();
   //                  row.HeaderCell.Value = "Видимость строки при пустом значении ячейки";
   //                  table.Rows.Add(row);

   //                  // Добавление данных (инициализация)
   //                  for (int i = 0; i < settingTable.SettingColumns.Count; i++)
   //                  {
   //                     table.Rows[0].Cells[i].Value = settingTable.SettingColumns[i].ColVis;
   //                     table.Rows[1].Cells[i].Value = settingTable.SettingColumns[i].RowVis;
   //                  }
   //               }
   //            }
   //         }
   //      }

   //      // Настройка таблицы
   //      table.RowHeadersWidth = 300;
   //      table.AllowUserToAddRows = false;

   //      // Программное редактирование
   //      fillDataTable = false;
   //   }
   //   /// <summary>
   //   /// Получить данные из таблицы
   //   /// </summary>
   //   public void GetDataTable()
   //   {
   //      SettingViewTable settingTableNew = new SettingViewTable();
   //      settingTableNew.NameTable = NameTable;
   //      for (int i = 0; i < dataGridView.Columns.Count; i++)
   //      {
   //         SettingViewColumn settingColumn = new SettingViewColumn();
   //         for(int m = 0; m < dataGridView.Columns.Count; m++)
   //         {
   //            if (i == dataGridView.Columns[m].DisplayIndex)
   //            {
   //               settingColumn.ColNam = dataGridView.Columns[m].HeaderText;
   //               settingColumn.ColVis = (bool)dataGridView.Rows[0].Cells[m].Value;
   //               settingColumn.RowVis = (bool)dataGridView.Rows[1].Cells[m].Value;
   //               settingTableNew.SettingColumns.Add(settingColumn);
   //            }
   //         }
   //      }
   //      foreach (SettingViewBase settingBase in Settings)
   //      {
   //         if (settingBase.Namebase == NameBase)
   //         {
   //            foreach (SettingViewTable settingTable in settingBase.SettingTables)
   //            {
   //               if (settingTable.NameTable == NameTable)
   //               {
   //                  settingTable.SettingColumns = settingTableNew.SettingColumns;
   //               }
   //            }
   //         }
   //      }
   //      SafeSettingToStr();
   //   }
   //   /// <summary>
   //   /// Задать данные в ComboBox
   //   /// </summary>
   //   /// <param name="comboBox"></param>
   //   private void SetDataComboBox(ref ComboBox comboBox)
   //   {
   //      if (comboBox == null) return;
   //      if (comboBox.Name == "cbx_BaseName")
   //      {
   //         List<string> namesBase = new List<string>();
   //         foreach (SettingViewBase settingBase in Settings)
   //         {
   //            namesBase.Add(settingBase.Namebase);
   //         }
   //         Ctrl.Fill(ref comboBox, namesBase, clear: true);
   //         string namebase = comboBox.SelectedItem.ToString();
   //         try
   //         {
   //            NameBase = namebase;
   //         }
   //         catch (Exception ex)
   //         {
   //            MessageBox.Show(text: ex.Message,
   //                               caption: "Ошибка",
   //                               buttons: MessageBoxButtons.OKCancel,
   //                               icon: MessageBoxIcon.Error,
   //                               defaultButton: MessageBoxDefaultButton.Button1);
   //         }
   //      }
   //      else if (comboBox.Name == "cbx_TableName")
   //      {
   //         List<string> namesTable = new List<string>();
   //         foreach (SettingViewBase settingBase in Settings)
   //         {
   //            if (settingBase.Namebase == NameBase)
   //            {
   //               foreach (SettingViewTable settingTable in settingBase.SettingTables)
   //               {
   //                  namesTable.Add(settingTable.NameTable);
   //               }
   //            }
   //         }
   //         Ctrl.Fill(ref comboBox, namesTable, clear: true);
   //         NameTable = comboBox.SelectedItem.ToString();
   //      }
   //   }

   //   /// <summary>
   //   /// Наполнение списка имён колонок
   //   /// </summary>
   //   private void FillNamesColumns()
   //   {
   //      if (Settings == null) return;
   //      foreach(SettingViewBase bas in Settings)
   //      {
   //         if(bas.Namebase == nameBase)
   //         {
   //            foreach(SettingViewTable tab in bas.SettingTables)
   //            {
   //               if(tab.NameTable == nameTable)
   //               {
   //                  NamesColumns.Clear();
   //                  NamesColEmptyCellNotVis.Clear();
   //                  foreach (SettingViewColumn col in tab.SettingColumns)
   //                  {
   //                     if ( col.ColVis )
   //                        NamesColumns.Add(col.ColNam);
   //                     if ( ! col.RowVis )
   //                        NamesColEmptyCellNotVis.Add(col.ColNam);
   //                  }
   //               }
   //            }
   //         }
   //      }
   //   }
   //}
}
