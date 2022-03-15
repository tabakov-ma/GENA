
namespace WorkBox
{
  partial class OutBox
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
         this.lblText = new System.Windows.Forms.Label();
         this.btnCancel = new System.Windows.Forms.Button();
         this.btnOk = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // lblText
         // 
         this.lblText.AutoSize = true;
         this.lblText.Dock = System.Windows.Forms.DockStyle.Top;
         this.lblText.Location = new System.Drawing.Point(0, 0);
         this.lblText.Name = "lblText";
         this.lblText.Size = new System.Drawing.Size(73, 13);
         this.lblText.TabIndex = 6;
         this.lblText.Text = "Информация";
         this.lblText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.lblText.TextChanged += new System.EventHandler(this.lblText_TextChanged);
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(224, 76);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(75, 28);
         this.btnCancel.TabIndex = 5;
         this.btnCancel.Text = "Cancel";
         this.btnCancel.UseVisualStyleBackColor = true;
         this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(92, 76);
         this.btnOk.Name = "btnOk";
         this.btnOk.Size = new System.Drawing.Size(75, 28);
         this.btnOk.TabIndex = 4;
         this.btnOk.Text = "Ok";
         this.btnOk.UseVisualStyleBackColor = true;
         this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
         // 
         // OutBox
         // 
         this.AcceptButton = this.btnCancel;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSize = true;
         this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.ClientSize = new System.Drawing.Size(388, 112);
         this.Controls.Add(this.lblText);
         this.Controls.Add(this.btnCancel);
         this.Controls.Add(this.btnOk);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
         this.MaximumSize = new System.Drawing.Size(404, 146);
         this.MinimumSize = new System.Drawing.Size(404, 146);
         this.Name = "OutBox";
         this.Text = "OutBox";
         this.Activated += new System.EventHandler(this.OutBox_Activated);
         this.ResumeLayout(false);
         this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblText;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOk;
   }
}