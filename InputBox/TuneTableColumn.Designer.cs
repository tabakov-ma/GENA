
namespace WorkBox
{
   partial class FormTuneTableColumn
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
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
         this.tlp_H = new System.Windows.Forms.TableLayoutPanel();
         this.dgv_TuneColumn = new System.Windows.Forms.DataGridView();
         this.tlp_H3_V = new System.Windows.Forms.TableLayoutPanel();
         this.btn_Save = new System.Windows.Forms.Button();
         this.tlp_H1_V = new System.Windows.Forms.TableLayoutPanel();
         this.cbx_TableName = new System.Windows.Forms.ComboBox();
         this.lbl_Base = new System.Windows.Forms.Label();
         this.lbl_Table = new System.Windows.Forms.Label();
         this.lblBaseName = new System.Windows.Forms.Label();
         this.tlp_H.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dgv_TuneColumn)).BeginInit();
         this.tlp_H3_V.SuspendLayout();
         this.tlp_H1_V.SuspendLayout();
         this.SuspendLayout();
         // 
         // tlp_H
         // 
         this.tlp_H.ColumnCount = 1;
         this.tlp_H.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlp_H.Controls.Add(this.dgv_TuneColumn, 0, 1);
         this.tlp_H.Controls.Add(this.tlp_H3_V, 0, 2);
         this.tlp_H.Controls.Add(this.tlp_H1_V, 0, 0);
         this.tlp_H.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tlp_H.Location = new System.Drawing.Point(0, 0);
         this.tlp_H.Name = "tlp_H";
         this.tlp_H.RowCount = 3;
         this.tlp_H.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
         this.tlp_H.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlp_H.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
         this.tlp_H.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tlp_H.Size = new System.Drawing.Size(1039, 203);
         this.tlp_H.TabIndex = 0;
         // 
         // dgv_TuneColumn
         // 
         this.dgv_TuneColumn.AllowUserToOrderColumns = true;
         this.dgv_TuneColumn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
         dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
         dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
         dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
         dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
         dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
         this.dgv_TuneColumn.DefaultCellStyle = dataGridViewCellStyle1;
         this.dgv_TuneColumn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.dgv_TuneColumn.Location = new System.Drawing.Point(3, 48);
         this.dgv_TuneColumn.Name = "dgv_TuneColumn";
         this.dgv_TuneColumn.RowHeadersWidth = 51;
         this.dgv_TuneColumn.RowTemplate.Height = 24;
         this.dgv_TuneColumn.Size = new System.Drawing.Size(1033, 107);
         this.dgv_TuneColumn.TabIndex = 1;
         this.dgv_TuneColumn.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_TuneColumn_CellValueChanged);
         this.dgv_TuneColumn.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgv_TuneColumn_ColumnDisplayIndexChanged);
         this.dgv_TuneColumn.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_TuneColumn_RowHeaderMouseDoubleClick);
         // 
         // tlp_H3_V
         // 
         this.tlp_H3_V.ColumnCount = 1;
         this.tlp_H3_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1033F));
         this.tlp_H3_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tlp_H3_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tlp_H3_V.Controls.Add(this.btn_Save, 0, 0);
         this.tlp_H3_V.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tlp_H3_V.Location = new System.Drawing.Point(3, 161);
         this.tlp_H3_V.Name = "tlp_H3_V";
         this.tlp_H3_V.RowCount = 1;
         this.tlp_H3_V.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlp_H3_V.Size = new System.Drawing.Size(1033, 39);
         this.tlp_H3_V.TabIndex = 2;
         // 
         // btn_Save
         // 
         this.btn_Save.Dock = System.Windows.Forms.DockStyle.Right;
         this.btn_Save.Enabled = false;
         this.btn_Save.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.btn_Save.Location = new System.Drawing.Point(868, 3);
         this.btn_Save.Name = "btn_Save";
         this.btn_Save.Size = new System.Drawing.Size(162, 33);
         this.btn_Save.TabIndex = 0;
         this.btn_Save.Text = "Сохранить";
         this.btn_Save.UseVisualStyleBackColor = true;
         this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
         // 
         // tlp_H1_V
         // 
         this.tlp_H1_V.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.tlp_H1_V.ColumnCount = 6;
         this.tlp_H1_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
         this.tlp_H1_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
         this.tlp_H1_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
         this.tlp_H1_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
         this.tlp_H1_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
         this.tlp_H1_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
         this.tlp_H1_V.Controls.Add(this.cbx_TableName, 5, 0);
         this.tlp_H1_V.Controls.Add(this.lbl_Base, 2, 0);
         this.tlp_H1_V.Controls.Add(this.lbl_Table, 4, 0);
         this.tlp_H1_V.Controls.Add(this.lblBaseName, 3, 0);
         this.tlp_H1_V.Location = new System.Drawing.Point(603, 3);
         this.tlp_H1_V.Name = "tlp_H1_V";
         this.tlp_H1_V.RowCount = 1;
         this.tlp_H1_V.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlp_H1_V.Size = new System.Drawing.Size(433, 39);
         this.tlp_H1_V.TabIndex = 3;
         // 
         // cbx_TableName
         // 
         this.cbx_TableName.Dock = System.Windows.Forms.DockStyle.Right;
         this.cbx_TableName.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.cbx_TableName.FormattingEnabled = true;
         this.cbx_TableName.Location = new System.Drawing.Point(250, 3);
         this.cbx_TableName.Name = "cbx_TableName";
         this.cbx_TableName.Size = new System.Drawing.Size(180, 29);
         this.cbx_TableName.TabIndex = 8;
         this.cbx_TableName.SelectedIndexChanged += new System.EventHandler(this.cbx_TableName_SelectedIndexChanged);
         // 
         // lbl_Base
         // 
         this.lbl_Base.AutoSize = true;
         this.lbl_Base.Dock = System.Windows.Forms.DockStyle.Right;
         this.lbl_Base.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.lbl_Base.Location = new System.Drawing.Point(3, 0);
         this.lbl_Base.Name = "lbl_Base";
         this.lbl_Base.Size = new System.Drawing.Size(52, 39);
         this.lbl_Base.TabIndex = 3;
         this.lbl_Base.Text = "База:";
         this.lbl_Base.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // lbl_Table
         // 
         this.lbl_Table.AutoSize = true;
         this.lbl_Table.Dock = System.Windows.Forms.DockStyle.Right;
         this.lbl_Table.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.lbl_Table.Location = new System.Drawing.Point(161, 0);
         this.lbl_Table.Name = "lbl_Table";
         this.lbl_Table.Size = new System.Drawing.Size(82, 39);
         this.lbl_Table.TabIndex = 1;
         this.lbl_Table.Text = "Таблица:";
         this.lbl_Table.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // lblBaseName
         // 
         this.lblBaseName.AutoSize = true;
         this.lblBaseName.Dock = System.Windows.Forms.DockStyle.Left;
         this.lblBaseName.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.lblBaseName.Location = new System.Drawing.Point(61, 0);
         this.lblBaseName.Name = "lblBaseName";
         this.lblBaseName.Size = new System.Drawing.Size(94, 39);
         this.lblBaseName.TabIndex = 9;
         this.lblBaseName.Text = "Base Name";
         this.lblBaseName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // FormTuneTableColumn
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1039, 203);
         this.Controls.Add(this.tlp_H);
         this.MaximumSize = new System.Drawing.Size(2500, 250);
         this.MinimumSize = new System.Drawing.Size(1057, 250);
         this.Name = "FormTuneTableColumn";
         this.Text = "Настройка таблиц";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTuneTableColumn_FormClosed);
         this.Shown += new System.EventHandler(this.TuneTableColumn_Shown);
         this.tlp_H.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.dgv_TuneColumn)).EndInit();
         this.tlp_H3_V.ResumeLayout(false);
         this.tlp_H1_V.ResumeLayout(false);
         this.tlp_H1_V.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tlp_H;
      private System.Windows.Forms.DataGridView dgv_TuneColumn;
      private System.Windows.Forms.TableLayoutPanel tlp_H3_V;
      private System.Windows.Forms.TableLayoutPanel tlp_H1_V;
      private System.Windows.Forms.Label lbl_Table;
      private System.Windows.Forms.Label lbl_Base;
      private System.Windows.Forms.Button btn_Save;
      private System.Windows.Forms.Label lblBaseName;
      private System.Windows.Forms.ComboBox cbx_TableName;
   }
}