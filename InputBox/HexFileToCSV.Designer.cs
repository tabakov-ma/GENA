
namespace WorkBox
{
   partial class HexFileToCSV
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HexFileToCSV));
         this.tlp_H = new System.Windows.Forms.TableLayoutPanel();
         this.dgvColumns = new System.Windows.Forms.DataGridView();
         this.NUM = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.COLUMN = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.ORDER = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.HEAD = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.SUBHEAD = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.ADD = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.TYPE = new System.Windows.Forms.DataGridViewComboBoxColumn();
         this.BYTES = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.OFFSET = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.tlp_H1_V = new System.Windows.Forms.TableLayoutPanel();
         this.button1 = new System.Windows.Forms.Button();
         this.pbxHelp = new System.Windows.Forms.PictureBox();
         this.txbPath = new System.Windows.Forms.TextBox();
         this.tlp_H.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dgvColumns)).BeginInit();
         this.tlp_H1_V.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pbxHelp)).BeginInit();
         this.SuspendLayout();
         // 
         // tlp_H
         // 
         this.tlp_H.ColumnCount = 1;
         this.tlp_H.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlp_H.Controls.Add(this.dgvColumns, 0, 0);
         this.tlp_H.Controls.Add(this.tlp_H1_V, 0, 1);
         this.tlp_H.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tlp_H.Location = new System.Drawing.Point(0, 0);
         this.tlp_H.Name = "tlp_H";
         this.tlp_H.RowCount = 2;
         this.tlp_H.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlp_H.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
         this.tlp_H.Size = new System.Drawing.Size(912, 554);
         this.tlp_H.TabIndex = 0;
         // 
         // dgvColumns
         // 
         this.dgvColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dgvColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NUM,
            this.COLUMN,
            this.ORDER,
            this.HEAD,
            this.SUBHEAD,
            this.ADD,
            this.TYPE,
            this.BYTES,
            this.OFFSET});
         this.dgvColumns.Dock = System.Windows.Forms.DockStyle.Fill;
         this.dgvColumns.Location = new System.Drawing.Point(3, 3);
         this.dgvColumns.Name = "dgvColumns";
         this.dgvColumns.RowHeadersWidth = 51;
         this.dgvColumns.RowTemplate.Height = 24;
         this.dgvColumns.Size = new System.Drawing.Size(906, 508);
         this.dgvColumns.TabIndex = 0;
         this.dgvColumns.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvColumns_CellValueChanged);
         this.dgvColumns.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvColumns_RowsAdded);
         this.dgvColumns.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvColumns_KeyPress);
         // 
         // NUM
         // 
         this.NUM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
         this.NUM.HeaderText = "NUM";
         this.NUM.MinimumWidth = 6;
         this.NUM.Name = "NUM";
         this.NUM.Width = 68;
         // 
         // COLUMN
         // 
         this.COLUMN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
         this.COLUMN.HeaderText = "COLUMN";
         this.COLUMN.MinimumWidth = 6;
         this.COLUMN.Name = "COLUMN";
         this.COLUMN.Width = 96;
         // 
         // ORDER
         // 
         this.ORDER.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
         this.ORDER.HeaderText = "ORDER";
         this.ORDER.MinimumWidth = 6;
         this.ORDER.Name = "ORDER";
         this.ORDER.Width = 87;
         // 
         // HEAD
         // 
         this.HEAD.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
         this.HEAD.HeaderText = "HEAD";
         this.HEAD.MinimumWidth = 6;
         this.HEAD.Name = "HEAD";
         this.HEAD.Width = 75;
         // 
         // SUBHEAD
         // 
         this.SUBHEAD.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
         this.SUBHEAD.HeaderText = "SUBHEAD";
         this.SUBHEAD.MinimumWidth = 6;
         this.SUBHEAD.Name = "SUBHEAD";
         this.SUBHEAD.Width = 103;
         // 
         // ADD
         // 
         this.ADD.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
         this.ADD.HeaderText = "ADD";
         this.ADD.MinimumWidth = 6;
         this.ADD.Name = "ADD";
         this.ADD.Width = 66;
         // 
         // TYPE
         // 
         this.TYPE.HeaderText = "TYPE";
         this.TYPE.Items.AddRange(new object[] {
            "Byte",
            "Float",
            "Int16",
            "Int32",
            "Real"});
         this.TYPE.MinimumWidth = 6;
         this.TYPE.Name = "TYPE";
         this.TYPE.Width = 125;
         // 
         // BYTES
         // 
         this.BYTES.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
         this.BYTES.HeaderText = "BYTES";
         this.BYTES.MinimumWidth = 6;
         this.BYTES.Name = "BYTES";
         this.BYTES.Width = 82;
         // 
         // OFFSET
         // 
         this.OFFSET.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
         this.OFFSET.HeaderText = "OFFSET";
         this.OFFSET.MinimumWidth = 6;
         this.OFFSET.Name = "OFFSET";
         this.OFFSET.Width = 91;
         // 
         // tlp_H1_V
         // 
         this.tlp_H1_V.ColumnCount = 2;
         this.tlp_H1_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlp_H1_V.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
         this.tlp_H1_V.Controls.Add(this.txbPath, 0, 0);
         this.tlp_H1_V.Controls.Add(this.button1, 1, 0);
         this.tlp_H1_V.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tlp_H1_V.Location = new System.Drawing.Point(3, 517);
         this.tlp_H1_V.Name = "tlp_H1_V";
         this.tlp_H1_V.RowCount = 1;
         this.tlp_H1_V.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlp_H1_V.Size = new System.Drawing.Size(906, 34);
         this.tlp_H1_V.TabIndex = 4;
         // 
         // button1
         // 
         this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.button1.Location = new System.Drawing.Point(839, 3);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(64, 28);
         this.button1.TabIndex = 4;
         this.button1.Text = "Help";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // pbxHelp
         // 
         this.pbxHelp.Image = ((System.Drawing.Image)(resources.GetObject("pbxHelp.Image")));
         this.pbxHelp.Location = new System.Drawing.Point(0, 0);
         this.pbxHelp.Name = "pbxHelp";
         this.pbxHelp.Size = new System.Drawing.Size(912, 515);
         this.pbxHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
         this.pbxHelp.TabIndex = 1;
         this.pbxHelp.TabStop = false;
         this.pbxHelp.Visible = false;
         // 
         // txbPath
         // 
         this.txbPath.Dock = System.Windows.Forms.DockStyle.Fill;
         this.txbPath.Location = new System.Drawing.Point(3, 3);
         this.txbPath.Name = "txbPath";
         this.txbPath.Size = new System.Drawing.Size(830, 22);
         this.txbPath.TabIndex = 5;
         this.txbPath.Text = "Нажми для выбора HEX-файла";
         // 
         // HexFileToCSV
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(912, 554);
         this.Controls.Add(this.pbxHelp);
         this.Controls.Add(this.tlp_H);
         this.Name = "HexFileToCSV";
         this.Text = "HEX to CSV";
         this.tlp_H.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.dgvColumns)).EndInit();
         this.tlp_H1_V.ResumeLayout(false);
         this.tlp_H1_V.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pbxHelp)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tlp_H;
      private System.Windows.Forms.DataGridView dgvColumns;
      //private AxMicrosoft.Vbe.Interop.Forms.AxTextBox txbPath;
      private System.Windows.Forms.DataGridViewTextBoxColumn NUM;
      private System.Windows.Forms.DataGridViewTextBoxColumn COLUMN;
      private System.Windows.Forms.DataGridViewTextBoxColumn ORDER;
      private System.Windows.Forms.DataGridViewTextBoxColumn HEAD;
      private System.Windows.Forms.DataGridViewTextBoxColumn SUBHEAD;
      private System.Windows.Forms.DataGridViewTextBoxColumn ADD;
      private System.Windows.Forms.DataGridViewComboBoxColumn TYPE;
      private System.Windows.Forms.DataGridViewTextBoxColumn BYTES;
      private System.Windows.Forms.DataGridViewTextBoxColumn OFFSET;
      private System.Windows.Forms.TableLayoutPanel tlp_H1_V;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.PictureBox pbxHelp;
      private System.Windows.Forms.TextBox txbPath;
   }
}