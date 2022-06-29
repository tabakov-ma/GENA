using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkBox;
using WorkSQL;
using WorkControl;
using WorkMisc;
using WorkString;
using WorkStruct;


namespace WorkBox
{
   public partial class TuneInstance : Form
   {
      private Query query;
      string ListBoxName = "";
      public TuneInstance(Query query)
      {
         InitializeComponent();
         this.query = query;
         this.Text = query.BaseName + ". Управление экземплярами";
         CheckNeedData();
         Init();
         ltbTypes.DataSource = ListInTypes;
         ltbInst.DataSource = ListOutInsts;
      }

      /// <summary>
      /// Проверка наличия необходимых для работы программы данных
      /// </summary>
      /// <param name="query"></param>
      private int CheckNeedData()
      {
         int result = 0;
         // Проверка подключения
         if (this.query == null || !this.query.TestCon())
         {
            MessageBox.Show(text: "Нет подключения к БД.",
                            caption: "Предупреждение",
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Warning,
                            defaultButton: MessageBoxDefaultButton.Button1);
            this.Close();
            return result;
         }
         // Проверка наличия необходимых таблиц для работы программы:
         // TypeHead
         if (!this.query.AvailabTable("TypeHead"))
         {
            MessageBox.Show(text: "В БД нет таблицы: 'TypeHead'.",
                            caption: "Ошибка",
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Error,
                            defaultButton: MessageBoxDefaultButton.Button1);
            this.Close();
            return result;
         }
         // TypesBody
         if (!this.query.AvailabTable("TypeBody"))
         {
            MessageBox.Show(text: "В БД нет таблицы: 'TypesBody'.",
                            caption: "Ошибка",
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Error,
                            defaultButton: MessageBoxDefaultButton.Button1);
            this.Close();
            return result;
         }
         // InstanceHead
         if (!this.query.AvailabTable("InstHead"))
         {
            this.query.CopyTable("TypeHead", "InstHead");
         }
         return result;
      }
      /// <summary>
      /// Список всех типов сисетмы
      /// </summary>
      private List<string> ListAllTypes = new List<string>();
      /// <summary>
      /// Список входных типов системы (Types)
      /// </summary>
      private BindingList<string> ListInTypes = new BindingList<string>();
      /// <summary>
      /// Список выходных экземпляров системы (Instance)
      /// </summary>
      private BindingList<string> ListOutInsts = new BindingList<string>();

      /// <summary>
      /// Загрузка списка всех типов системы
      /// </summary>
      private void Load_ListAllTypes()
      {
         DataTable table = new DataTable();
         this.query.GetTable( NameTable: "TypeHead",
                         table: out table,
                         where: "OBJECT_ID !=''",
                         order: "NAME" );
         ListAllTypes = Ctrl.GetListRowsStrInOneColumnTable( table: table,
                                                           ColumnName: "NAME" );
      }

      /// <summary>
      /// Загрузка выходного списка экземпляров системы
      /// </summary>
      private bool Load_ListOutInsts()
      {
         bool result = false;
         ListOutInsts.Clear();
         List<string> ListAllTableName = new List<string>();
         this.query.GetNamesTable( ref ListAllTableName );
         foreach( string TableName in ListAllTableName )
         {
            if (TableName.IndexOf("InstBody") >= 0 )
            {
               ListOutInsts.Add(TableName.Replace("InstBody", ""));
            }
         }
         if (ListOutInsts.Count > 0)
         {
            //ListOutInsts = new BindingList<string>(ListOutInsts.OrderBy(x => x).ToList());
            result = true;
         }
         return result;
      }

      /// <summary>
      /// Загрузка входного списка типов системы
      /// </summary>
      /// <param name="query"></param>
      /// <returns></returns>
      private bool Load_ListInTypes()
      {
         bool result = false;
         ListInTypes.Clear();
         foreach (string nameType in ListAllTypes)
         {
            if ( !ListOutInsts.Contains(nameType) )
            {
               ListInTypes.Add(nameType);
            }
         }
         if (ListInTypes.Count > 0) 
         {
            //ListInTypes = new BindingList<string>(ListInTypes.OrderBy(x => x).ToList());
            result = true;
         }
         
         return result;
      }

      private void Init()
      {
         Load_ListAllTypes();
         Load_ListOutInsts();
         Load_ListInTypes();
      }

      private void ltbTypes_MouseDown(object sender, MouseEventArgs e)
      {
         ListBoxName = ((ListBox)sender).Name;
         if (ltbTypes.SelectedItem != null )
         {
            ltbInst.DoDragDrop(ltbTypes.SelectedItem.ToString(), DragDropEffects.Copy);
         }
      }

      private void ltbInst_DragEnter(object sender, DragEventArgs e)
      {
         e.Effect = DragDropEffects.Copy;
      }

      private void ltbInst_DragDrop(object sender, DragEventArgs e)
      {
         if (ListBoxName == ((ListBox)sender).Name) return;
         CheckNeedData();
         string nameTable = e.Data.GetData(DataFormats.Text).ToString();
         if (AddTable(nameTable))
         {
            Init();
         }
      }
      
      /// <summary>
      /// Добавление таблицы
      /// </summary>
      /// <param name="nameTable"></param>
      /// <returns></returns>
      private bool AddTable( string nameTable )
      {
         // =============================================
         // Инициализация текущего узала
         Dictionary<string, string> Node = new Dictionary<string, string>()
         {
            { "NAME", nameTable },
            { "TYPE", nameTable },
            { "ARRAY_SIZE", "" }
         };
         // Инициализация уровня
         Struct.TreeLayer TreeLayer = new Struct.TreeLayer()
         {
            Nodes = new List<Dictionary<string, string>>(),
            NodeSelector = 0
         };
         // Инициализация словаря данных узла
         TreeLayer.Nodes.Add(Node);

         Struct.DynamicStructTreeData TreeRead = new Struct.DynamicStructTreeData()
         { 
            Layers = new List<Struct.TreeLayer>(), 
            LayerSelector = 0 
         };
         TreeRead.Layers.Add((Struct.TreeLayer)TreeLayer.Clone());
         TreeRead.NodeCopy();
         // =============================================

         int NumLayer = 0;
         int NumNode = 0;
         bool PermitRight = false;
         for (; ; ) // Цикл чтения
         {
            // Не первый цикл (не СТАРТ) Селектор горизонтальных узлов 'ВПРАВО'
            // Проверка достижения конечного горизонтального узала
            if ( PermitRight && !TreeRead.NodeSelectRight() )
            {
               // TreeRead.Layers.RemoveAt(NumLayer); // Удаления верхнего уровня после чтения
               // Селектор вертикальных уровней 'ВНИЗ' и Проверка достижение ЗЕМЛИ
               if ( !TreeRead.LayerSelectDown() )
               {
                  break; // ЗЕМЛЯ Выход из цикла чтения
               }
               continue; // К нижнему уровню
            }
            PermitRight = true;

            // ВВЕРХ
            if (Ctrl.GetDataType(NameType: TreeRead.NodeCurr["TYPE"], 
                                          table: out DataTable table,
                                          query: query,
                                      TreeLayer: ref TreeLayer,
                                            RTN: true))
            {
               TreeRead.LayerSelectUp((Struct.TreeLayer)TreeLayer.Clone());

               for (int i=0; i< TreeLayer.Nodes.Count; i++) // ПО ГОРИЗОНТУ (к соседу)
               {
                  NumLayer = TreeRead.LayerSelector;
                  NumNode = TreeRead.Layers[NumLayer].NodeSelector;
                  // Проверка на объектность
                  if ( Str.IsObject(TreeLayer.Nodes[NumNode]["TYPE"]) > 0 )
                  {
                     PermitRight = false;
                     break;
                  }
                  else
                  {
                     TreeRead.NodeCopy(InResult: true);
                  }
                  TreeRead.NodeSelectRight();
               }
            }
            else
            {
               TreeRead.NodeCopy(InResult: true);
            }
         }

         // Добавление обязательных столбцов
         TreeRead.ResultNodes.Insert(0, new Dictionary<string, string>() 
                                       { { "NAME", "Num"}, { "TYPE", "Int" }, { "ARRAY_SIZE", "" } });
         TreeRead.ResultNodes.Insert(0, new Dictionary<string, string>()
                                       { { "NAME", "Descript"}, { "TYPE", "" }, { "ARRAY_SIZE", "" } });
         TreeRead.ResultNodes.Insert(0, new Dictionary<string, string>()
                                       { { "NAME", "Pos"}, { "TYPE", "NVARCHAR(255)" }, { "ARRAY_SIZE", "" } });

         // Пробуем создать новую талблицу
         if (!this.query.CreateTable(NameTable: "InstBody" + nameTable,
                                       NameAndTypeColumns: TreeRead.ResultNodes,
                                       ErrMess: true,
                                       UseType: true,
                                       PrimKey: "Pos")) return false;
         // Копируем данные из таблицы
         if (!this.query.CopyDataTable(NameTableSource: "TypeHead",
                                       NameTableDest: "InstHead",
                                       where: "NAME = '" + nameTable + "'")) return false;
         foreach (var col in TreeRead.ResultNodes)
         {
            this.query.SetExtProp(name_bas: null,
                                  name_tab: "InstBody" + nameTable,
                                  name_col: col["NAME"],
                                  name_prop: "Type",
                                  val: col["TYPE"] );
         }
         return true;
      }

      private bool DelTable( string nameTable )
      {
         if (nameTable == "InstHead"){
            if (!this.query.DelTable(nameTable: nameTable,
                           ErrMess: true)) return false;}
         else{
            if (!this.query.DelTable(nameTable: "InstBody" + nameTable,
                                    ErrMess: true)) return false;
            if (!this.query.DelDataTable(NameTable: "InstHead",
                                         where: "NAME = '" + nameTable + "'")) return false;}
         return true;
      }

      private void ltbInst_MouseDown(object sender, MouseEventArgs e)
      {
         ListBoxName = ((ListBox)sender).Name;
         if ( ltbInst.SelectedItem != null )
         {
            ltbTypes.DoDragDrop(ltbInst.SelectedItem.ToString(), DragDropEffects.Copy);
         }
      }

      private void ltbTypes_DragEnter(object sender, DragEventArgs e)
      {
         e.Effect = DragDropEffects.Copy;
      }

      private void ltbTypes_DragDrop(object sender, DragEventArgs e)
      {
         if (ListBoxName == ((ListBox)sender).Name) return;
         string nameTable = e.Data.GetData(DataFormats.Text).ToString();
         if (DelTable(nameTable))
         {
            Init();
         }

      }

      private void btn_Add_Click(object sender, EventArgs e)
      {
         CheckNeedData();
         for ( int i = 0; i < ltbTypes.Items.Count; i++ )
         {
            if (ltbTypes.GetSelected(i))
            {
               AddTable( ltbTypes.GetItemText(ltbTypes.Items[i]) );
            }
         }
         Init();
      }

      private void btn_Del_Click(object sender, EventArgs e)
      {
         for (int i = 0; i < ltbInst.Items.Count; i++)
         {
            if (ltbInst.GetSelected(i))
            {
               DelTable(ltbInst.GetItemText(ltbInst.Items[i]));
            }
         }
         Init();
         if (ltbInst.Items.Count == 0 && this.query.AvailabTable("InstHead"))
         {
            DelTable("InstHead");
         }
      }
   }
}
