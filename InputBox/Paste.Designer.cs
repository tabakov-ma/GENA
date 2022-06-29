
namespace WorkBox
{
   partial class Paste
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
         this.btnPaste = new System.Windows.Forms.Button();
         this.btnCancel = new System.Windows.Forms.Button();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // btnPaste
         // 
         this.btnPaste.Dock = System.Windows.Forms.DockStyle.Fill;
         this.btnPaste.Location = new System.Drawing.Point(3, 3);
         this.btnPaste.Name = "btnPaste";
         this.btnPaste.Size = new System.Drawing.Size(108, 26);
         this.btnPaste.TabIndex = 0;
         this.btnPaste.Text = "Вставить";
         this.btnPaste.UseVisualStyleBackColor = true;
         this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
         // 
         // btnCancel
         // 
         this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.btnCancel.Location = new System.Drawing.Point(117, 3);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(109, 26);
         this.btnCancel.TabIndex = 1;
         this.btnCancel.Text = "Отмена";
         this.btnCancel.UseVisualStyleBackColor = true;
         this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Controls.Add(this.btnPaste, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.btnCancel, 1, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 1;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(229, 32);
         this.tableLayoutPanel1.TabIndex = 2;
         // 
         // Paste
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(229, 32);
         this.ControlBox = false;
         this.Controls.Add(this.tableLayoutPanel1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "Paste";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.TopMost = true;
         this.Load += new System.EventHandler(this.Paste_Load);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button btnPaste;
      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
   }
}