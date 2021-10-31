
namespace Box
{
  partial class InputBox
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
      this.btnOk = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.tbxInput = new System.Windows.Forms.TextBox();
      this.lblText = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // btnOk
      // 
      this.btnOk.Location = new System.Drawing.Point(301, 12);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new System.Drawing.Size(75, 28);
      this.btnOk.TabIndex = 0;
      this.btnOk.Text = "Да";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(301, 46);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 28);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // tbxInput
      // 
      this.tbxInput.Location = new System.Drawing.Point(12, 79);
      this.tbxInput.Name = "tbxInput";
      this.tbxInput.Size = new System.Drawing.Size(363, 20);
      this.tbxInput.TabIndex = 2;
      this.tbxInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxInput_KeyPress);
      // 
      // lblText
      // 
      this.lblText.Location = new System.Drawing.Point(12, 12);
      this.lblText.Name = "lblText";
      this.lblText.Size = new System.Drawing.Size(283, 64);
      this.lblText.TabIndex = 3;
      this.lblText.Text = "Введите данные:";
      this.lblText.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
      // 
      // InputBox
      // 
      this.AcceptButton = this.btnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(388, 108);
      this.Controls.Add(this.lblText);
      this.Controls.Add(this.tbxInput);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOk);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.MaximumSize = new System.Drawing.Size(404, 146);
      this.MinimumSize = new System.Drawing.Size(404, 146);
      this.Name = "InputBox";
      this.Text = "Ввод данных";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.TextBox tbxInput;
    private System.Windows.Forms.Label lblText;
  }
}