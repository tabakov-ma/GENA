
namespace WorkBox
{
   partial class TuneInstance
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
         this.btn_Add = new System.Windows.Forms.Button();
         this.btn_Del = new System.Windows.Forms.Button();
         this.ltbTypes = new System.Windows.Forms.ListBox();
         this.ltbInst = new System.Windows.Forms.ListBox();
         this.tableLayoutPanel1.SuspendLayout();
         this.tableLayoutPanel2.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 3;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
         this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.ltbTypes, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.ltbInst, 2, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 1;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(601, 250);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // tableLayoutPanel2
         // 
         this.tableLayoutPanel2.ColumnCount = 1;
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel2.Controls.Add(this.btn_Add, 0, 0);
         this.tableLayoutPanel2.Controls.Add(this.btn_Del, 0, 1);
         this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel2.Location = new System.Drawing.Point(243, 3);
         this.tableLayoutPanel2.Name = "tableLayoutPanel2";
         this.tableLayoutPanel2.RowCount = 2;
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.Size = new System.Drawing.Size(114, 244);
         this.tableLayoutPanel2.TabIndex = 0;
         // 
         // btn_Add
         // 
         this.btn_Add.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.btn_Add.Location = new System.Drawing.Point(3, 86);
         this.btn_Add.Name = "btn_Add";
         this.btn_Add.Size = new System.Drawing.Size(108, 33);
         this.btn_Add.TabIndex = 0;
         this.btn_Add.Text = ">>";
         this.btn_Add.UseVisualStyleBackColor = true;
         this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
         // 
         // btn_Del
         // 
         this.btn_Del.Dock = System.Windows.Forms.DockStyle.Top;
         this.btn_Del.Location = new System.Drawing.Point(3, 125);
         this.btn_Del.Name = "btn_Del";
         this.btn_Del.Size = new System.Drawing.Size(108, 33);
         this.btn_Del.TabIndex = 1;
         this.btn_Del.Text = "<<";
         this.btn_Del.UseVisualStyleBackColor = true;
         this.btn_Del.Click += new System.EventHandler(this.btn_Del_Click);
         // 
         // ltbTypes
         // 
         this.ltbTypes.AllowDrop = true;
         this.ltbTypes.Dock = System.Windows.Forms.DockStyle.Fill;
         this.ltbTypes.FormattingEnabled = true;
         this.ltbTypes.ItemHeight = 16;
         this.ltbTypes.Location = new System.Drawing.Point(3, 3);
         this.ltbTypes.Name = "ltbTypes";
         this.ltbTypes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.ltbTypes.Size = new System.Drawing.Size(234, 244);
         this.ltbTypes.TabIndex = 1;
         this.ltbTypes.DragDrop += new System.Windows.Forms.DragEventHandler(this.ltbTypes_DragDrop);
         this.ltbTypes.DragEnter += new System.Windows.Forms.DragEventHandler(this.ltbTypes_DragEnter);
         this.ltbTypes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ltbTypes_MouseDown);
         // 
         // ltbInst
         // 
         this.ltbInst.AllowDrop = true;
         this.ltbInst.Dock = System.Windows.Forms.DockStyle.Fill;
         this.ltbInst.FormattingEnabled = true;
         this.ltbInst.ItemHeight = 16;
         this.ltbInst.Location = new System.Drawing.Point(363, 3);
         this.ltbInst.Name = "ltbInst";
         this.ltbInst.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.ltbInst.Size = new System.Drawing.Size(235, 244);
         this.ltbInst.TabIndex = 2;
         this.ltbInst.DragDrop += new System.Windows.Forms.DragEventHandler(this.ltbInst_DragDrop);
         this.ltbInst.DragEnter += new System.Windows.Forms.DragEventHandler(this.ltbInst_DragEnter);
         this.ltbInst.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ltbInst_MouseDown);
         // 
         // TuneInstance
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(601, 250);
         this.Controls.Add(this.tableLayoutPanel1);
         this.MinimumSize = new System.Drawing.Size(619, 297);
         this.Name = "TuneInstance";
         this.Text = "Управление экземплярами";
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel2.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
      private System.Windows.Forms.Button btn_Add;
      private System.Windows.Forms.Button btn_Del;
      private System.Windows.Forms.ListBox ltbTypes;
      private System.Windows.Forms.ListBox ltbInst;
   }
}