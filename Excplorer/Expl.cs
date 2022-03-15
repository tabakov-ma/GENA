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


namespace Explorer
{
   public partial class Expl : Form
   {
      Entity entity;
      //Query qTypes;
      Query qObjects;
      //DataTable dtTypes;
      //DataTable dtObjects;
      OutBox OutBox = new OutBox();
      const int NOT_SELECT = -1;
      string StrConnect = "";
      //bool Collapse = false;
      TuneViewTable tuneViewTable;

      /// <summary>
      /// Конструктор класса формы
      /// </summary>
      public Expl()
      {
         InitializeComponent();

         //treeView1.BeforeSelect += treeView1_BeforeSelect;
         trvObjects.BeforeExpand += treeView1_BeforeExpand;
         // заполняем дерево дисками
         //FillDriveNodes();


         // Проверка всех мониторов и возможности открыть окно по сохранённым координатам
         Screen[] scs;
         scs = Screen.AllScreens;
         bool testX = false, testY = false;
         int X = AccessProp.GetPropInt("X");
         int Y = AccessProp.GetPropInt("Y");
         int Width = AccessProp.GetPropInt("Width");
         int Height = AccessProp.GetPropInt("Height");
         foreach (Screen sc in scs)
         {
            if (X + 10 >= sc.Bounds.X && X < sc.Bounds.X + sc.Bounds.Width) testX = true;
            if (Y + 10 >= sc.Bounds.Y && Y < sc.Bounds.Y + sc.Bounds.Height) testY = true;
         }
         if (!testX || !testY)
         {
            foreach (Screen sc in scs)
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
         entity = new Entity();
         entity.Change += Entity_Change;
         Init();
         // Инициализация объекта переодики
         //Ctrl.InitBlinkControl(new TimeSpan(0, 0, 1));
         //List<Controls> controls = new List<Controls>();
         //Controls control = new Controls();
         //control.cOn = true;
         //control.controls = btnSafeChange;
         //controls.Add(control);
         //Ctrl.SetControl(controls);
         // 
      } // Конструктор класса формы

      private void Init()
      {

         StrConnect = AccessProp.GetPropStr("StrConnect");
         if (StrConnect == "")
         {
            OutBox.ShowDialog("Нет сохранённой информации о подключении к БД", "Предупреждение", true);
            return;
         }
         // Сущность строки подключения
         entity.Fill(StrConnect);
         qObjects = new Query(StrConnect);
         Ctrl.Fill_TreeView_Init(ref trvObjects, qObjects);

         // Настройка отображения таблиц
         //tuneViewTable = new TuneViewTable(query: qObjects);

         // Данные по именам типов объектов
         //qTypes = new Query(StrConnect);
         //if (!qTypes.GetTable("TypesObjects", ref dtTypes)) return;
         //List<string> data = new List<string>(qTypes.GetListRowsInOneColumnTable(dtTypes, 1));
         //data.Sort();
         //lbxType.DataSource = data;

         // Данные типов объектов
         //dgvTypes.DataSource = dtTypes;
         //// Данные объектов
         //qObjects = new Query(StrConnect);
         //if (!qObjects.GetTable("Objects", ref dtObjects)) return;
         //dgvObjects.DataSource = dtObjects;
         //// Убрать выделение у ListBox типов
         //lbxType.SelectedIndex = NOT_SELECT;

      }

      private void Entity_Change()
      {
         lblStrCon.Text = entity.ToStringHidePass();
      }




      // ======================================================================
      // Контейнеры. Изменение разделителей
      // ======================================================================
      private void spc_H_SplitMov(object sender, SplitterEventArgs e)
      {
         AccessProp.SetProp("SplitterDistanceTop", ((SplitContainer)sender).SplitterDistance);
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
      // ======================================================================



      // ======================================================================
      // Форма. События
      // ======================================================================
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
      }
      // ======================================================================



      // Формирование запроса на конкретный тип объекта или на все
      private void lbxType_MouseUp(object sender, MouseEventArgs e)
      {
         //// Выбор всех элементов
         //if (((ListBox)sender).SelectedItem == null || e.Button == MouseButtons.Right)
         //{
         //   qTypes.GetTable("TypesObjects", ref dtTypes);
         //   dgvTypes.DataSource = dtTypes;

         //   qObjects.GetTable("Objects", ref dtObjects);
         //   dgvObjects.DataSource = dtObjects;

         //   lbxType.SelectedIndex = NOT_SELECT;
         //   return;
         //}

         //qTypes.GetTable("TypesObjects", ref dtTypes, "NAME = '" + ((ListBox)sender).SelectedItem.ToString() + "'");
         //dgvTypes.DataSource = dtTypes;

         //qObjects.GetTable("Objects", ref dtObjects, "NAME_TYPE = '" + ((ListBox)sender).SelectedItem.ToString() + "'");
         //dgvObjects.DataSource = dtObjects;
      }

     
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
         qObjects.SetDataTable();
      }
      /// <summary>
      /// Меню. Файл. Настройки
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tsmTune_MouseUp(object sender, MouseEventArgs e)
      {
         // Вкл./выкл. автосохранения изменений в БД, настройка периода сохранения
         // Настройка шрифтов в контролах
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
      /// Меню. БД
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
            entity.Fill(con.ResultStrCon);
            Init();
         }
      }
      // ======================================================================

      // событие перед раскрытием узла
      void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
      {
         ((TreeView)sender).SelectedNode = e.Node;
      }

      // Переключение на вкладку
      private void tabExplorer_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (((TabControl)sender).SelectedIndex == 2)
         {
            // Начальная загрузка [Fill_TreeView_Init]
            //Ctrl.Fill_TreeView_Init(ref trvObjects, qObjects);
         }
      }

      private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
      {
         //Collapse = true;
      }

      private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
      {
         ////if (e.Node.IsExpanded) { return; }
         //DataTable dataTable;
         //if (e.Node.Parent == null)
         //{
            
         //   Ctrl.Fill_TreeView_BD(ref trvObjects, qObjects, e.Node.Text, out dataTable);
         //   this.dgvObjectsDetails.Columns.Clear();
         //   this.dgvObjectsDetails.DataSource = dataTable;
         //   Ctrl.TuneDataGridView(ref dgvObjectsDetails, read_only: false);
         //   lblNamaTableDetail.Text = e.Node.Text;


         //}
         //else
         //{
         //   Ctrl.Fill_TreeView_Node(ref trvObjects, qObjects, e, out dataTable);
         //   this.dgvObjectsDetails.Columns.Clear();
         //   this.dgvObjectsDetails.DataSource = dataTable;
         //   Ctrl.TuneDataGridView(ref dgvObjectsDetails, read_only: false);
         //   lblNamaTableDetail.Text = e.Node.Text;
         //}
      }

      private void btnSafeChange_Click(object sender, EventArgs e)
      {
         
      }

      private void dgvObjectsDetails_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
        
      }

      private void lblNamaTableDetail_Click(object sender, EventArgs e)
      {

      }

      private void trvObjects_AfterSelect(object sender, TreeViewEventArgs e)
      {
         //if (e.Node.IsExpanded) { return; }
         if(this.lblNamaTableDetail.Text.IndexOf("*")>=0)
         {
            DialogResult result =
               MessageBox.Show(text: "Данные были изменены, сохранить данные?",
                               caption: "Предупреждение",
                               buttons: MessageBoxButtons.OKCancel,
                               icon: MessageBoxIcon.Warning,
                               defaultButton: MessageBoxDefaultButton.Button1);
            if (result == DialogResult.OK)
            {
               qObjects.SetDataTable();
            }
         }
         DataTable dataTable;
         if (e.Node.Parent == null)
         {
            Ctrl.Fill_TreeView_BD(ref trvObjects, ref qObjects, e.Node.Text, out dataTable);
            tuneViewTable = new TuneViewTable(query: qObjects);
         }
         else
         {
            Ctrl.Fill_TreeView_Node(ref trvObjects, ref qObjects, e, out dataTable);
            tuneViewTable = new TuneViewTable(query: qObjects);
         }

         this.dgvObjectsDetails.Columns.Clear();
         this.dgvObjectsDetails.DataSource = dataTable;
         tuneViewTable.NameBase = e.Node.Text;
         if(dataTable?.Namespace != null)
            tuneViewTable.NameTable = dataTable.Namespace;
         //Ctrl.TuneRowDataGridView(ref dgvObjectsDetails, NameColEmtyCellNotVis: tuneViewTable.NamesColEmptyCellNotVis);
         Ctrl.TuneColumnDataGridView(ref dgvObjectsDetails, ReadOnly: false, NameColumns: tuneViewTable.NamesColumns);

         lblNamaTableDetail.Text = e.Node.Text + ":";
      }

      private void btnSafeChange_Click_1(object sender, EventArgs e)
      {
         qObjects.SetDataTable();
      }

      /// <summary>
      /// Изменение данных в таблице
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void dgvObjectsDetails_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
      {
         if(lblNamaTableDetail.Text.IndexOf('*')==-1)
            lblNamaTableDetail.Text = lblNamaTableDetail.Text + "*";
      }

      /// <summary>
      /// Tool Strip Menu Item
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tsmTuneViewTableColumn_MouseUp(object sender, MouseEventArgs e)
      {
         try
         {
            //// Получить настройки отображения колонок (Порядок и видимость) из БД
            //string tune_column = qObjects.GetExtPropDB(name_prop: "TuneColumn");
            WorkBox.FormTuneTableColumn tuneTableColumn = new FormTuneTableColumn( qObjects );
            tuneTableColumn.ShowDialog();
            //// Сохранить настройки отображения колонок (Порядок и видимость) из БД
            //qObjects.SetExtPropDB(name_prop: "TuneColumn", val: tuneTableColumn.StrTuneTableColumn);
            //AccessProp.SetProp("strTuneTableColumn", tuneTableColumn.StrTuneTableColumn);
            // Обновить настройки
            tuneViewTable.strTuneTableColumn = tuneTableColumn.StrTuneTableColumn;
            tuneViewTable.FillSettingsFromStr();
         }
         catch(Exception ex)
         {
            MessageBox.Show(text: ex.Message + "\n" + ex.StackTrace,
                            caption: "Ошибка",
                            buttons: MessageBoxButtons.OK,
                            icon: MessageBoxIcon.Error,
                            defaultButton: MessageBoxDefaultButton.Button1);
         }
         
      }
   }
}
