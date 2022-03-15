
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
         this.components = new System.ComponentModel.Container();
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
         this.button1 = new System.Windows.Forms.Button();
         this.btnImport = new System.Windows.Forms.Button();
         this.timer1 = new System.Windows.Forms.Timer(this.components);
         this.lblCountCell = new System.Windows.Forms.Label();
         this.btnImportStop = new System.Windows.Forms.Button();
         this.richTextBox1 = new System.Windows.Forms.RichTextBox();
         this.btnWrProp = new System.Windows.Forms.Button();
         this.gbxEctPropDb = new System.Windows.Forms.GroupBox();
         this.btnReadProp = new System.Windows.Forms.Button();
         this.tbxPropName = new System.Windows.Forms.TextBox();
         this.tbxPropVal = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.btnDelProp = new System.Windows.Forms.Button();
         ((System.ComponentModel.ISupportInitialize)(this.dgvTest)).BeginInit();
         this.gbxEctPropDb.SuspendLayout();
         this.SuspendLayout();
         // 
         // btnConnect
         // 
         this.btnConnect.Location = new System.Drawing.Point(64, 14);
         this.btnConnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.btnConnect.Name = "btnConnect";
         this.btnConnect.Size = new System.Drawing.Size(100, 28);
         this.btnConnect.TabIndex = 0;
         this.btnConnect.Text = "Connect";
         this.btnConnect.UseVisualStyleBackColor = true;
         this.btnConnect.Click += new System.EventHandler(this.button1_Click);
         // 
         // tbx1
         // 
         this.tbx1.Location = new System.Drawing.Point(64, 94);
         this.tbx1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.tbx1.Multiline = true;
         this.tbx1.Name = "tbx1";
         this.tbx1.Size = new System.Drawing.Size(696, 100);
         this.tbx1.TabIndex = 1;
         // 
         // btnDisconnect
         // 
         this.btnDisconnect.Location = new System.Drawing.Point(64, 59);
         this.btnDisconnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.btnDisconnect.Name = "btnDisconnect";
         this.btnDisconnect.Size = new System.Drawing.Size(100, 28);
         this.btnDisconnect.TabIndex = 2;
         this.btnDisconnect.Text = "Disconnect";
         this.btnDisconnect.UseVisualStyleBackColor = true;
         this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
         // 
         // btnClear
         // 
         this.btnClear.Location = new System.Drawing.Point(661, 502);
         this.btnClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.btnClear.Name = "btnClear";
         this.btnClear.Size = new System.Drawing.Size(100, 28);
         this.btnClear.TabIndex = 3;
         this.btnClear.Text = "Clear text";
         this.btnClear.UseVisualStyleBackColor = true;
         this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
         // 
         // btnCreate
         // 
         this.btnCreate.Location = new System.Drawing.Point(172, 14);
         this.btnCreate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.btnCreate.Name = "btnCreate";
         this.btnCreate.Size = new System.Drawing.Size(100, 28);
         this.btnCreate.TabIndex = 4;
         this.btnCreate.Text = "Create";
         this.btnCreate.UseVisualStyleBackColor = true;
         this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
         // 
         // tbxSourse
         // 
         this.tbxSourse.Location = new System.Drawing.Point(64, 506);
         this.tbxSourse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.tbxSourse.Name = "tbxSourse";
         this.tbxSourse.Size = new System.Drawing.Size(132, 22);
         this.tbxSourse.TabIndex = 5;
         // 
         // tbxAdd
         // 
         this.tbxAdd.Location = new System.Drawing.Point(205, 506);
         this.tbxAdd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.tbxAdd.Name = "tbxAdd";
         this.tbxAdd.Size = new System.Drawing.Size(132, 22);
         this.tbxAdd.TabIndex = 6;
         // 
         // tbxDest
         // 
         this.tbxDest.Location = new System.Drawing.Point(347, 506);
         this.tbxDest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.tbxDest.Name = "tbxDest";
         this.tbxDest.Size = new System.Drawing.Size(132, 22);
         this.tbxDest.TabIndex = 7;
         // 
         // btnAddStr
         // 
         this.btnAddStr.Location = new System.Drawing.Point(488, 502);
         this.btnAddStr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.btnAddStr.Name = "btnAddStr";
         this.btnAddStr.Size = new System.Drawing.Size(100, 28);
         this.btnAddStr.TabIndex = 8;
         this.btnAddStr.Text = "Соеденить";
         this.btnAddStr.UseVisualStyleBackColor = true;
         this.btnAddStr.Click += new System.EventHandler(this.btnAddStr_Click);
         // 
         // dgvTest
         // 
         this.dgvTest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dgvTest.Location = new System.Drawing.Point(72, 206);
         this.dgvTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.dgvTest.Name = "dgvTest";
         this.dgvTest.RowHeadersWidth = 62;
         this.dgvTest.Size = new System.Drawing.Size(688, 252);
         this.dgvTest.TabIndex = 9;
         // 
         // btnSaveDgv
         // 
         this.btnSaveDgv.Location = new System.Drawing.Point(660, 466);
         this.btnSaveDgv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.btnSaveDgv.Name = "btnSaveDgv";
         this.btnSaveDgv.Size = new System.Drawing.Size(100, 28);
         this.btnSaveDgv.TabIndex = 10;
         this.btnSaveDgv.Text = "Сохранить";
         this.btnSaveDgv.UseVisualStyleBackColor = true;
         this.btnSaveDgv.Click += new System.EventHandler(this.btnSaveDgv_Click);
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(280, 14);
         this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(168, 28);
         this.button1.TabIndex = 11;
         this.button1.Text = "Открыть настройку";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click_1);
         // 
         // btnImport
         // 
         this.btnImport.Location = new System.Drawing.Point(172, 59);
         this.btnImport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.btnImport.Name = "btnImport";
         this.btnImport.Size = new System.Drawing.Size(100, 28);
         this.btnImport.TabIndex = 12;
         this.btnImport.Text = "Import";
         this.btnImport.UseVisualStyleBackColor = true;
         this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
         // 
         // timer1
         // 
         this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
         // 
         // lblCountCell
         // 
         this.lblCountCell.AutoSize = true;
         this.lblCountCell.Location = new System.Drawing.Point(430, 66);
         this.lblCountCell.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
         this.lblCountCell.Name = "lblCountCell";
         this.lblCountCell.Size = new System.Drawing.Size(16, 17);
         this.lblCountCell.TabIndex = 13;
         this.lblCountCell.Text = "0";
         // 
         // btnImportStop
         // 
         this.btnImportStop.Location = new System.Drawing.Point(280, 59);
         this.btnImportStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.btnImportStop.Name = "btnImportStop";
         this.btnImportStop.Size = new System.Drawing.Size(100, 28);
         this.btnImportStop.TabIndex = 14;
         this.btnImportStop.Text = "Import Stop";
         this.btnImportStop.UseVisualStyleBackColor = true;
         this.btnImportStop.Click += new System.EventHandler(this.btnImportStop_Click);
         // 
         // richTextBox1
         // 
         this.richTextBox1.Location = new System.Drawing.Point(455, 14);
         this.richTextBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.richTextBox1.Name = "richTextBox1";
         this.richTextBox1.Size = new System.Drawing.Size(168, 50);
         this.richTextBox1.TabIndex = 15;
         this.richTextBox1.Text = "";
         this.richTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.richTextBox1_KeyPress);
         // 
         // btnWrProp
         // 
         this.btnWrProp.Location = new System.Drawing.Point(6, 38);
         this.btnWrProp.Name = "btnWrProp";
         this.btnWrProp.Size = new System.Drawing.Size(75, 23);
         this.btnWrProp.TabIndex = 16;
         this.btnWrProp.Text = "Записать";
         this.btnWrProp.UseVisualStyleBackColor = true;
         this.btnWrProp.Click += new System.EventHandler(this.btnWrProp_Click);
         // 
         // gbxEctPropDb
         // 
         this.gbxEctPropDb.Controls.Add(this.btnDelProp);
         this.gbxEctPropDb.Controls.Add(this.label2);
         this.gbxEctPropDb.Controls.Add(this.label1);
         this.gbxEctPropDb.Controls.Add(this.tbxPropVal);
         this.gbxEctPropDb.Controls.Add(this.tbxPropName);
         this.gbxEctPropDb.Controls.Add(this.btnReadProp);
         this.gbxEctPropDb.Controls.Add(this.btnWrProp);
         this.gbxEctPropDb.Location = new System.Drawing.Point(795, 118);
         this.gbxEctPropDb.Name = "gbxEctPropDb";
         this.gbxEctPropDb.Size = new System.Drawing.Size(242, 184);
         this.gbxEctPropDb.TabIndex = 17;
         this.gbxEctPropDb.TabStop = false;
         this.gbxEctPropDb.Text = "Расширенные свойства БД";
         this.gbxEctPropDb.Enter += new System.EventHandler(this.groupBox1_Enter);
         // 
         // btnReadProp
         // 
         this.btnReadProp.Location = new System.Drawing.Point(6, 67);
         this.btnReadProp.Name = "btnReadProp";
         this.btnReadProp.Size = new System.Drawing.Size(75, 23);
         this.btnReadProp.TabIndex = 17;
         this.btnReadProp.Text = "Прочитать";
         this.btnReadProp.UseVisualStyleBackColor = true;
         this.btnReadProp.Click += new System.EventHandler(this.btnReadProp_Click);
         // 
         // tbxPropName
         // 
         this.tbxPropName.Location = new System.Drawing.Point(136, 38);
         this.tbxPropName.Name = "tbxPropName";
         this.tbxPropName.Size = new System.Drawing.Size(100, 22);
         this.tbxPropName.TabIndex = 18;
         // 
         // tbxPropVal
         // 
         this.tbxPropVal.Location = new System.Drawing.Point(136, 68);
         this.tbxPropVal.Name = "tbxPropVal";
         this.tbxPropVal.Size = new System.Drawing.Size(100, 22);
         this.tbxPropVal.TabIndex = 19;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(85, 38);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(45, 17);
         this.label1.TabIndex = 20;
         this.label1.Text = "Name";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(87, 73);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(44, 17);
         this.label2.TabIndex = 21;
         this.label2.Text = "Value";
         // 
         // btnDelProp
         // 
         this.btnDelProp.Location = new System.Drawing.Point(6, 96);
         this.btnDelProp.Name = "btnDelProp";
         this.btnDelProp.Size = new System.Drawing.Size(75, 23);
         this.btnDelProp.TabIndex = 22;
         this.btnDelProp.Text = "Удалить";
         this.btnDelProp.UseVisualStyleBackColor = true;
         this.btnDelProp.Click += new System.EventHandler(this.btnDelProp_Click);
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1049, 553);
         this.Controls.Add(this.gbxEctPropDb);
         this.Controls.Add(this.richTextBox1);
         this.Controls.Add(this.btnImportStop);
         this.Controls.Add(this.lblCountCell);
         this.Controls.Add(this.btnImport);
         this.Controls.Add(this.button1);
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
         this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.Name = "Form1";
         this.Text = "Form1";
         ((System.ComponentModel.ISupportInitialize)(this.dgvTest)).EndInit();
         this.gbxEctPropDb.ResumeLayout(false);
         this.gbxEctPropDb.PerformLayout();
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
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Button btnImport;
      private System.Windows.Forms.Timer timer1;
      private System.Windows.Forms.Label lblCountCell;
      private System.Windows.Forms.Button btnImportStop;
      private System.Windows.Forms.RichTextBox richTextBox1;
      private System.Windows.Forms.Button btnWrProp;
      private System.Windows.Forms.GroupBox gbxEctPropDb;
      private System.Windows.Forms.TextBox tbxPropVal;
      private System.Windows.Forms.TextBox tbxPropName;
      private System.Windows.Forms.Button btnReadProp;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Button btnDelProp;
   }
}

