
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
         this.components = new System.ComponentModel.Container();
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
         this.tlp_H = new System.Windows.Forms.TableLayoutPanel();
         this.dgv_TuneColumn = new System.Windows.Forms.DataGridView();
         this.tlp_H3_V = new System.Windows.Forms.TableLayoutPanel();
         this.btn_SaveAll = new System.Windows.Forms.Button();
         this.btn_Save = new System.Windows.Forms.Button();
         this.btn_Reset = new System.Windows.Forms.Button();
         this.tlp_H1_V = new System.Windows.Forms.TableLayoutPanel();
         this.cbx_TableName = new System.Windows.Forms.ComboBox();
         this.lbl_Table = new System.Windows.Forms.Label();
         this.menuStrip1 = new System.Windows.Forms.MenuStrip();
         this.tsmTune = new System.Windows.Forms.ToolStripMenuItem();
         this.tsmTune_SelCol = new System.Windows.Forms.ToolStripMenuItem();
         this.tsmTune_SelRow = new System.Windows.Forms.ToolStripMenuItem();
         this.tsmTune_Sort = new System.Windows.Forms.ToolStripMenuItem();
         this.toolTip = new System.Windows.Forms.ToolTip(this.components);
         this.tlp_H.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dgv_TuneColumn)).BeginInit();
         this.tlp_H3_V.SuspendLayout();
         this.tlp_H1_V.SuspendLayout();
         this.menuStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // tlp_H
         // 
         this.tlp_H.ColumnCount = 1;
         this.tlp_H.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlp_H.Controls.Add(this.dgv_TuneColumn, 0, 2);
         this.tlp_H.Controls.Add(this.tlp_H3_V, 0, 3);
         this.tlp_H.Controls.Add(this.tlp_H1_V, 0, 1);
         this.tlp_H.Controls.Add(this.menuStrip1, 0, 0);
         this.tlp_H.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tlp_H.Location = new System.Drawing.Point(0, 0);
         this.tlp_H.Name = "tlp_H";
         this.tlp_H.RowCount = 4;
         this.tlp_H.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tlp_H.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
         this.tlp_H.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlp_H.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
         this.tlp_H.Size = new System.Drawing.Size(1039, 238);
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
         this.dgv_TuneColumn.Location = new System.Drawing.Point(3, 73);
         this.dgv_TuneColumn.Name = "dgv_TuneColumn";
         this.dgv_TuneColumn.RowHeadersWidth = 51;
         this.dgv_TuneColumn.RowTemplate.Height = 24;
         this.dgv_TuneColumn.Size = new System.Drawing.Size(1033, 117);
         this.dgv_TuneColumn.TabIndex = 1;
         this.dgv_TuneColumn.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_TuneColumn_CellClick);
         this.dgv_TuneColumn.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_TuneColumn_CellValueChanged);
         this.dgv_TuneColumn.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgv_TuneColumn_ColumnDisplayIndexChanged);
         this.dgv_TuneColumn.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgv_TuneColumn_ColumnWidthChanged);
         this.dgv_TuneColumn.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_TuneColumn_RowHeaderMouseDoubleClick);
         // 
         // tlp_H3_V
         // 
         this.tlp_H3_V.ColumnCount = 3;
         this.tlp_H3_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 464F));
         this.tlp_H3_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 169F));
         this.tlp_H3_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
         this.tlp_H3_V.Controls.Add(this.btn_SaveAll, 0, 0);
         this.tlp_H3_V.Controls.Add(this.btn_Save, 2, 0);
         this.tlp_H3_V.Controls.Add(this.btn_Reset, 0, 0);
         this.tlp_H3_V.Dock = System.Windows.Forms.DockStyle.Right;
         this.tlp_H3_V.Location = new System.Drawing.Point(233, 196);
         this.tlp_H3_V.Name = "tlp_H3_V";
         this.tlp_H3_V.RowCount = 1;
         this.tlp_H3_V.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlp_H3_V.Size = new System.Drawing.Size(803, 39);
         this.tlp_H3_V.TabIndex = 2;
         // 
         // btn_SaveAll
         // 
         this.btn_SaveAll.Dock = System.Windows.Forms.DockStyle.Right;
         this.btn_SaveAll.Enabled = false;
         this.btn_SaveAll.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.btn_SaveAll.Location = new System.Drawing.Point(468, 3);
         this.btn_SaveAll.Name = "btn_SaveAll";
         this.btn_SaveAll.Size = new System.Drawing.Size(162, 33);
         this.btn_SaveAll.TabIndex = 3;
         this.btn_SaveAll.Text = "Сохранить всё";
         this.btn_SaveAll.UseVisualStyleBackColor = true;
         this.btn_SaveAll.Click += new System.EventHandler(this.btn_SaveAll_Click);
         // 
         // btn_Save
         // 
         this.btn_Save.Dock = System.Windows.Forms.DockStyle.Right;
         this.btn_Save.Enabled = false;
         this.btn_Save.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.btn_Save.Location = new System.Drawing.Point(638, 3);
         this.btn_Save.Name = "btn_Save";
         this.btn_Save.Size = new System.Drawing.Size(162, 33);
         this.btn_Save.TabIndex = 2;
         this.btn_Save.Text = "Сохранить";
         this.btn_Save.UseVisualStyleBackColor = true;
         this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
         // 
         // btn_Reset
         // 
         this.btn_Reset.Dock = System.Windows.Forms.DockStyle.Right;
         this.btn_Reset.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.btn_Reset.Location = new System.Drawing.Point(299, 3);
         this.btn_Reset.Name = "btn_Reset";
         this.btn_Reset.Size = new System.Drawing.Size(162, 33);
         this.btn_Reset.TabIndex = 1;
         this.btn_Reset.Text = "Сброс";
         this.btn_Reset.UseVisualStyleBackColor = true;
         this.btn_Reset.Click += new System.EventHandler(this.btn_Reset_Click);
         // 
         // tlp_H1_V
         // 
         this.tlp_H1_V.ColumnCount = 3;
         this.tlp_H1_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
         this.tlp_H1_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
         this.tlp_H1_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 214F));
         this.tlp_H1_V.Controls.Add(this.cbx_TableName, 1, 0);
         this.tlp_H1_V.Controls.Add(this.lbl_Table, 0, 0);
         this.tlp_H1_V.Dock = System.Windows.Forms.DockStyle.Right;
         this.tlp_H1_V.Location = new System.Drawing.Point(724, 33);
         this.tlp_H1_V.Name = "tlp_H1_V";
         this.tlp_H1_V.RowCount = 1;
         this.tlp_H1_V.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlp_H1_V.Size = new System.Drawing.Size(312, 34);
         this.tlp_H1_V.TabIndex = 3;
         // 
         // cbx_TableName
         // 
         this.cbx_TableName.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.cbx_TableName.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.cbx_TableName.FormattingEnabled = true;
         this.cbx_TableName.Location = new System.Drawing.Point(91, 3);
         this.cbx_TableName.Name = "cbx_TableName";
         this.cbx_TableName.Size = new System.Drawing.Size(206, 29);
         this.cbx_TableName.TabIndex = 8;
         this.cbx_TableName.SelectedIndexChanged += new System.EventHandler(this.cbx_TableName_SelectedIndexChanged);
         // 
         // lbl_Table
         // 
         this.lbl_Table.AutoSize = true;
         this.lbl_Table.Dock = System.Windows.Forms.DockStyle.Right;
         this.lbl_Table.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.lbl_Table.Location = new System.Drawing.Point(3, 0);
         this.lbl_Table.Name = "lbl_Table";
         this.lbl_Table.Size = new System.Drawing.Size(82, 34);
         this.lbl_Table.TabIndex = 1;
         this.lbl_Table.Text = "Таблица:";
         this.lbl_Table.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // menuStrip1
         // 
         this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
         this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmTune});
         this.menuStrip1.Location = new System.Drawing.Point(0, 0);
         this.menuStrip1.Name = "menuStrip1";
         this.menuStrip1.Size = new System.Drawing.Size(1039, 28);
         this.menuStrip1.TabIndex = 4;
         this.menuStrip1.Text = "menuStrip1";
         // 
         // tsmTune
         // 
         this.tsmTune.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmTune_SelCol,
            this.tsmTune_SelRow,
            this.tsmTune_Sort});
         this.tsmTune.Name = "tsmTune";
         this.tsmTune.Size = new System.Drawing.Size(98, 24);
         this.tsmTune.Text = "Настройки";
         // 
         // tsmTune_SelCol
         // 
         this.tsmTune_SelCol.Name = "tsmTune_SelCol";
         this.tsmTune_SelCol.Size = new System.Drawing.Size(591, 26);
         this.tsmTune_SelCol.Text = "Разрешение для выделении столбцов (Групповое изменение ширины)";
         this.tsmTune_SelCol.Click += new System.EventHandler(this.tsmTune_SelCol_Click);
         // 
         // tsmTune_SelRow
         // 
         this.tsmTune_SelRow.Checked = true;
         this.tsmTune_SelRow.CheckState = System.Windows.Forms.CheckState.Checked;
         this.tsmTune_SelRow.Name = "tsmTune_SelRow";
         this.tsmTune_SelRow.Size = new System.Drawing.Size(591, 26);
         this.tsmTune_SelRow.Text = "Разрешение для выделении строк (Перенос столбцов)";
         this.tsmTune_SelRow.Click += new System.EventHandler(this.tsmTune_SelRow_Click);
         // 
         // tsmTune_Sort
         // 
         this.tsmTune_Sort.Checked = true;
         this.tsmTune_Sort.CheckState = System.Windows.Forms.CheckState.Checked;
         this.tsmTune_Sort.Name = "tsmTune_Sort";
         this.tsmTune_Sort.Size = new System.Drawing.Size(591, 26);
         this.tsmTune_Sort.Text = "Алфавитная сортировка столбцов при начальной загрузки";
         this.tsmTune_Sort.Click += new System.EventHandler(this.tsmTune_Sort_Click);
         // 
         // toolTip
         // 
         this.toolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip_Popup);
         // 
         // FormTuneTableColumn
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1039, 238);
         this.Controls.Add(this.tlp_H);
         this.MainMenuStrip = this.menuStrip1;
         this.MaximumSize = new System.Drawing.Size(2500, 285);
         this.MinimumSize = new System.Drawing.Size(1057, 285);
         this.Name = "FormTuneTableColumn";
         this.Text = "Настройка таблиц";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTuneTableColumn_FormClosed);
         this.Shown += new System.EventHandler(this.TuneTableColumn_Shown);
         this.tlp_H.ResumeLayout(false);
         this.tlp_H.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dgv_TuneColumn)).EndInit();
         this.tlp_H3_V.ResumeLayout(false);
         this.tlp_H1_V.ResumeLayout(false);
         this.tlp_H1_V.PerformLayout();
         this.menuStrip1.ResumeLayout(false);
         this.menuStrip1.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tlp_H;
      private System.Windows.Forms.DataGridView dgv_TuneColumn;
      private System.Windows.Forms.TableLayoutPanel tlp_H3_V;
      private System.Windows.Forms.TableLayoutPanel tlp_H1_V;
      private System.Windows.Forms.Label lbl_Table;
      private System.Windows.Forms.Button btn_Save;
      private System.Windows.Forms.Button btn_Reset;
      private System.Windows.Forms.Button btn_SaveAll;
      private System.Windows.Forms.ToolTip toolTip;
      private System.Windows.Forms.MenuStrip menuStrip1;
      private System.Windows.Forms.ToolStripMenuItem tsmTune;
      private System.Windows.Forms.ToolStripMenuItem tsmTune_SelCol;
      private System.Windows.Forms.ToolStripMenuItem tsmTune_SelRow;
      private System.Windows.Forms.ToolStripMenuItem tsmTune_Sort;
      private System.Windows.Forms.ComboBox cbx_TableName;
   }
}