
namespace WorkBox
{
   partial class CreatorProject
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
         System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
         this.flp_H1 = new System.Windows.Forms.FlowLayoutPanel();
         this.tlpHMI = new System.Windows.Forms.TableLayoutPanel();
         this.dgvHMI = new System.Windows.Forms.DataGridView();
         this.lblHMI = new System.Windows.Forms.Label();
         this.tlpPLC = new System.Windows.Forms.TableLayoutPanel();
         this.dgvPLC = new System.Windows.Forms.DataGridView();
         this.lblPLC = new System.Windows.Forms.Label();
         this.HMI_PROJECT_EN = new System.Windows.Forms.DataGridViewCheckBoxColumn();
         this.HMI_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.PLC_PROJECT_EN = new System.Windows.Forms.DataGridViewCheckBoxColumn();
         this.PLC_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
         this.flp_H1.SuspendLayout();
         this.tlpHMI.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dgvHMI)).BeginInit();
         this.tlpPLC.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dgvPLC)).BeginInit();
         this.SuspendLayout();
         // 
         // flp_H1
         // 
         this.flp_H1.AutoScroll = true;
         this.flp_H1.Controls.Add(this.tlpHMI);
         this.flp_H1.Controls.Add(this.tlpPLC);
         this.flp_H1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.flp_H1.Location = new System.Drawing.Point(0, 0);
         this.flp_H1.Name = "flp_H1";
         this.flp_H1.Size = new System.Drawing.Size(800, 450);
         this.flp_H1.TabIndex = 2;
         // 
         // tlpHMI
         // 
         this.tlpHMI.ColumnCount = 1;
         this.tlpHMI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlpHMI.Controls.Add(this.dgvHMI, 0, 1);
         this.tlpHMI.Controls.Add(this.lblHMI, 0, 0);
         this.tlpHMI.Location = new System.Drawing.Point(3, 3);
         this.tlpHMI.Name = "tlpHMI";
         this.tlpHMI.RowCount = 2;
         this.tlpHMI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tlpHMI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlpHMI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tlpHMI.Size = new System.Drawing.Size(391, 384);
         this.tlpHMI.TabIndex = 1;
         // 
         // dgvHMI
         // 
         this.dgvHMI.AllowUserToAddRows = false;
         this.dgvHMI.AllowUserToDeleteRows = false;
         this.dgvHMI.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dgvHMI.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.HMI_PROJECT_EN,
            this.HMI_NAME});
         this.dgvHMI.Dock = System.Windows.Forms.DockStyle.Fill;
         this.dgvHMI.Location = new System.Drawing.Point(3, 33);
         this.dgvHMI.Name = "dgvHMI";
         this.dgvHMI.RowHeadersWidth = 51;
         this.dgvHMI.RowTemplate.Height = 24;
         this.dgvHMI.Size = new System.Drawing.Size(385, 348);
         this.dgvHMI.TabIndex = 0;
         this.dgvHMI.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHMI_CellContentClick);
         // 
         // lblHMI
         // 
         this.lblHMI.AutoSize = true;
         this.lblHMI.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblHMI.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.lblHMI.Location = new System.Drawing.Point(3, 0);
         this.lblHMI.Name = "lblHMI";
         this.lblHMI.Size = new System.Drawing.Size(385, 30);
         this.lblHMI.TabIndex = 1;
         this.lblHMI.Text = "HMI";
         this.lblHMI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // tlpPLC
         // 
         this.tlpPLC.ColumnCount = 1;
         this.tlpPLC.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlpPLC.Controls.Add(this.dgvPLC, 0, 1);
         this.tlpPLC.Controls.Add(this.lblPLC, 0, 0);
         this.tlpPLC.Location = new System.Drawing.Point(400, 3);
         this.tlpPLC.Name = "tlpPLC";
         this.tlpPLC.RowCount = 2;
         this.tlpPLC.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tlpPLC.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tlpPLC.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tlpPLC.Size = new System.Drawing.Size(391, 384);
         this.tlpPLC.TabIndex = 2;
         // 
         // dgvPLC
         // 
         this.dgvPLC.AllowUserToAddRows = false;
         this.dgvPLC.AllowUserToDeleteRows = false;
         this.dgvPLC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dgvPLC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PLC_PROJECT_EN,
            this.PLC_NAME});
         this.dgvPLC.Dock = System.Windows.Forms.DockStyle.Fill;
         this.dgvPLC.Location = new System.Drawing.Point(3, 33);
         this.dgvPLC.Name = "dgvPLC";
         this.dgvPLC.RowHeadersWidth = 51;
         this.dgvPLC.RowTemplate.Height = 24;
         this.dgvPLC.Size = new System.Drawing.Size(385, 348);
         this.dgvPLC.TabIndex = 0;
         // 
         // lblPLC
         // 
         this.lblPLC.AutoSize = true;
         this.lblPLC.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblPLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.lblPLC.Location = new System.Drawing.Point(3, 0);
         this.lblPLC.Name = "lblPLC";
         this.lblPLC.Size = new System.Drawing.Size(385, 30);
         this.lblPLC.TabIndex = 1;
         this.lblPLC.Text = "PLC";
         this.lblPLC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // HMI_PROJECT_EN
         // 
         this.HMI_PROJECT_EN.DataPropertyName = "PROJECT_EN";
         this.HMI_PROJECT_EN.HeaderText = "EN";
         this.HMI_PROJECT_EN.MinimumWidth = 6;
         this.HMI_PROJECT_EN.Name = "HMI_PROJECT_EN";
         this.HMI_PROJECT_EN.ToolTipText = "Включить";
         this.HMI_PROJECT_EN.Width = 40;
         // 
         // HMI_NAME
         // 
         this.HMI_NAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
         this.HMI_NAME.DataPropertyName = "NAME";
         dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.HMI_NAME.DefaultCellStyle = dataGridViewCellStyle1;
         this.HMI_NAME.HeaderText = "NAME";
         this.HMI_NAME.MinimumWidth = 6;
         this.HMI_NAME.Name = "HMI_NAME";
         this.HMI_NAME.ReadOnly = true;
         this.HMI_NAME.Resizable = System.Windows.Forms.DataGridViewTriState.True;
         this.HMI_NAME.Width = 76;
         // 
         // PLC_PROJECT_EN
         // 
         this.PLC_PROJECT_EN.DataPropertyName = "PROJECT_EN";
         this.PLC_PROJECT_EN.HeaderText = "EN";
         this.PLC_PROJECT_EN.MinimumWidth = 6;
         this.PLC_PROJECT_EN.Name = "PLC_PROJECT_EN";
         this.PLC_PROJECT_EN.ToolTipText = "Включить";
         this.PLC_PROJECT_EN.Width = 40;
         // 
         // PLC_NAME
         // 
         this.PLC_NAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
         this.PLC_NAME.DataPropertyName = "NAME";
         dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.PLC_NAME.DefaultCellStyle = dataGridViewCellStyle2;
         this.PLC_NAME.HeaderText = "NAME";
         this.PLC_NAME.MinimumWidth = 6;
         this.PLC_NAME.Name = "PLC_NAME";
         this.PLC_NAME.ReadOnly = true;
         this.PLC_NAME.Width = 76;
         // 
         // CreatorProject
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(800, 450);
         this.Controls.Add(this.flp_H1);
         this.Name = "CreatorProject";
         this.Text = "CreatorProject";
         this.flp_H1.ResumeLayout(false);
         this.tlpHMI.ResumeLayout(false);
         this.tlpHMI.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dgvHMI)).EndInit();
         this.tlpPLC.ResumeLayout(false);
         this.tlpPLC.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.dgvPLC)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.FlowLayoutPanel flp_H1;
      private System.Windows.Forms.TableLayoutPanel tlpHMI;
      private System.Windows.Forms.DataGridView dgvHMI;
      private System.Windows.Forms.Label lblHMI;
      private System.Windows.Forms.TableLayoutPanel tlpPLC;
      private System.Windows.Forms.DataGridView dgvPLC;
      private System.Windows.Forms.Label lblPLC;
      private System.Windows.Forms.DataGridViewCheckBoxColumn HMI_PROJECT_EN;
      private System.Windows.Forms.DataGridViewTextBoxColumn HMI_NAME;
      private System.Windows.Forms.DataGridViewCheckBoxColumn PLC_PROJECT_EN;
      private System.Windows.Forms.DataGridViewTextBoxColumn PLC_NAME;
   }
}