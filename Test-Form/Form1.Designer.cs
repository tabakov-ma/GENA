
namespace Test_Form
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
         this.btnConnect = new System.Windows.Forms.Button();
         this.tbx1 = new System.Windows.Forms.TextBox();
         this.btnDisconnect = new System.Windows.Forms.Button();
         this.btnClear = new System.Windows.Forms.Button();
         this.btnCreate = new System.Windows.Forms.Button();
         this.tbxSourse = new System.Windows.Forms.TextBox();
         this.tbxAdd = new System.Windows.Forms.TextBox();
         this.tbxDest = new System.Windows.Forms.TextBox();
         this.btnAddStr = new System.Windows.Forms.Button();
         this.dgvTest = new System.Windows.Forms.DataGridView();
         this.btnSaveDgv = new System.Windows.Forms.Button();
         ((System.ComponentModel.ISupportInitialize)(this.dgvTest)).BeginInit();
         this.SuspendLayout();
         // 
         // btnConnect
         // 
         this.btnConnect.Location = new System.Drawing.Point(48, 12);
         this.btnConnect.Name = "btnConnect";
         this.btnConnect.Size = new System.Drawing.Size(75, 23);
         this.btnConnect.TabIndex = 0;
         this.btnConnect.Text = "Connect";
         this.btnConnect.UseVisualStyleBackColor = true;
         this.btnConnect.Click += new System.EventHandler(this.button1_Click);
         // 
         // tbx1
         // 
         this.tbx1.Location = new System.Drawing.Point(48, 77);
         this.tbx1.Multiline = true;
         this.tbx1.Name = "tbx1";
         this.tbx1.Size = new System.Drawing.Size(523, 82);
         this.tbx1.TabIndex = 1;
         // 
         // btnDisconnect
         // 
         this.btnDisconnect.Location = new System.Drawing.Point(48, 48);
         this.btnDisconnect.Name = "btnDisconnect";
         this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
         this.btnDisconnect.TabIndex = 2;
         this.btnDisconnect.Text = "Disconnect";
         this.btnDisconnect.UseVisualStyleBackColor = true;
         this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
         // 
         // btnClear
         // 
         this.btnClear.Location = new System.Drawing.Point(496, 408);
         this.btnClear.Name = "btnClear";
         this.btnClear.Size = new System.Drawing.Size(75, 23);
         this.btnClear.TabIndex = 3;
         this.btnClear.Text = "Clear text";
         this.btnClear.UseVisualStyleBackColor = true;
         this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
         // 
         // btnCreate
         // 
         this.btnCreate.Location = new System.Drawing.Point(129, 12);
         this.btnCreate.Name = "btnCreate";
         this.btnCreate.Size = new System.Drawing.Size(75, 23);
         this.btnCreate.TabIndex = 4;
         this.btnCreate.Text = "Create";
         this.btnCreate.UseVisualStyleBackColor = true;
         this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
         // 
         // tbxSourse
         // 
         this.tbxSourse.Location = new System.Drawing.Point(48, 411);
         this.tbxSourse.Name = "tbxSourse";
         this.tbxSourse.Size = new System.Drawing.Size(100, 20);
         this.tbxSourse.TabIndex = 5;
         // 
         // tbxAdd
         // 
         this.tbxAdd.Location = new System.Drawing.Point(154, 411);
         this.tbxAdd.Name = "tbxAdd";
         this.tbxAdd.Size = new System.Drawing.Size(100, 20);
         this.tbxAdd.TabIndex = 6;
         // 
         // tbxDest
         // 
         this.tbxDest.Location = new System.Drawing.Point(260, 411);
         this.tbxDest.Name = "tbxDest";
         this.tbxDest.Size = new System.Drawing.Size(100, 20);
         this.tbxDest.TabIndex = 7;
         // 
         // btnAddStr
         // 
         this.btnAddStr.Location = new System.Drawing.Point(366, 408);
         this.btnAddStr.Name = "btnAddStr";
         this.btnAddStr.Size = new System.Drawing.Size(75, 23);
         this.btnAddStr.TabIndex = 8;
         this.btnAddStr.Text = "Соеденить";
         this.btnAddStr.UseVisualStyleBackColor = true;
         this.btnAddStr.Click += new System.EventHandler(this.btnAddStr_Click);
         // 
         // dgvTest
         // 
         this.dgvTest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dgvTest.Location = new System.Drawing.Point(54, 167);
         this.dgvTest.Name = "dgvTest";
         this.dgvTest.Size = new System.Drawing.Size(516, 205);
         this.dgvTest.TabIndex = 9;
         // 
         // btnSaveDgv
         // 
         this.btnSaveDgv.Location = new System.Drawing.Point(495, 378);
         this.btnSaveDgv.Name = "btnSaveDgv";
         this.btnSaveDgv.Size = new System.Drawing.Size(75, 23);
         this.btnSaveDgv.TabIndex = 10;
         this.btnSaveDgv.Text = "Сохранить";
         this.btnSaveDgv.UseVisualStyleBackColor = true;
         this.btnSaveDgv.Click += new System.EventHandler(this.btnSaveDgv_Click);
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(606, 441);
         this.Controls.Add(this.btnSaveDgv);
         this.Controls.Add(this.dgvTest);
         this.Controls.Add(this.btnAddStr);
         this.Controls.Add(this.tbxDest);
         this.Controls.Add(this.tbxAdd);
         this.Controls.Add(this.tbxSourse);
         this.Controls.Add(this.btnCreate);
         this.Controls.Add(this.btnClear);
         this.Controls.Add(this.btnDisconnect);
         this.Controls.Add(this.tbx1);
         this.Controls.Add(this.btnConnect);
         this.Name = "Form1";
         this.Text = "Form1";
         ((System.ComponentModel.ISupportInitialize)(this.dgvTest)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox tbx1;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnClear;
  private System.Windows.Forms.Button btnCreate;
    private System.Windows.Forms.TextBox tbxSourse;
    private System.Windows.Forms.TextBox tbxAdd;
    private System.Windows.Forms.TextBox tbxDest;
    private System.Windows.Forms.Button btnAddStr;
      private System.Windows.Forms.DataGridView dgvTest;
      private System.Windows.Forms.Button btnSaveDgv;
   }
}

