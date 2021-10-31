
namespace SQLTuneConnect
{
  partial class SQLTuneCon
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
            this.gbxTuneServer = new System.Windows.Forms.GroupBox();
            this.lblConnctYes = new System.Windows.Forms.Label();
            this.pbrRun = new System.Windows.Forms.ProgressBar();
            this.cbxUserPass = new System.Windows.Forms.ComboBox();
            this.cbxUserName = new System.Windows.Forms.ComboBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cbxTimeConnect = new System.Windows.Forms.ComboBox();
            this.cbxNameDB = new System.Windows.Forms.ComboBox();
            this.cbxNameServer = new System.Windows.Forms.ComboBox();
            this.chbWinUserAccess = new System.Windows.Forms.CheckBox();
            this.lblTimeConnect = new System.Windows.Forms.Label();
            this.lblUserPass = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblWinUserAccess = new System.Windows.Forms.Label();
            this.lblNameDB = new System.Windows.Forms.Label();
            this.lblNameServer = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnCreateDB = new System.Windows.Forms.Button();
            this.lblPatchNewDB = new System.Windows.Forms.Label();
            this.btnFindPatchDB = new System.Windows.Forms.Button();
            this.tbxPatchNewDB = new System.Windows.Forms.TextBox();
            this.gbxCreateNewDB = new System.Windows.Forms.GroupBox();
            this.btnDeleteDB = new System.Windows.Forms.Button();
            this.tbxSizeDB = new System.Windows.Forms.TextBox();
            this.lblSizeDB = new System.Windows.Forms.Label();
            this.tbxNameNewDB = new System.Windows.Forms.TextBox();
            this.lblNameNewDB = new System.Windows.Forms.Label();
            this.btnDelTbl = new System.Windows.Forms.Button();
            this.btnRenameTbl = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxCurNameTbl = new System.Windows.Forms.TextBox();
            this.btnCreateTbl = new System.Windows.Forms.Button();
            this.btnListTbl = new System.Windows.Forms.Button();
            this.gbxTuneServer.SuspendLayout();
            this.gbxCreateNewDB.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxTuneServer
            // 
            this.gbxTuneServer.Controls.Add(this.lblConnctYes);
            this.gbxTuneServer.Controls.Add(this.pbrRun);
            this.gbxTuneServer.Controls.Add(this.cbxUserPass);
            this.gbxTuneServer.Controls.Add(this.cbxUserName);
            this.gbxTuneServer.Controls.Add(this.btnTest);
            this.gbxTuneServer.Controls.Add(this.btnSave);
            this.gbxTuneServer.Controls.Add(this.cbxTimeConnect);
            this.gbxTuneServer.Controls.Add(this.cbxNameDB);
            this.gbxTuneServer.Controls.Add(this.cbxNameServer);
            this.gbxTuneServer.Controls.Add(this.chbWinUserAccess);
            this.gbxTuneServer.Controls.Add(this.lblTimeConnect);
            this.gbxTuneServer.Controls.Add(this.lblUserPass);
            this.gbxTuneServer.Controls.Add(this.lblUserName);
            this.gbxTuneServer.Controls.Add(this.lblWinUserAccess);
            this.gbxTuneServer.Controls.Add(this.lblNameDB);
            this.gbxTuneServer.Controls.Add(this.lblNameServer);
            this.gbxTuneServer.Location = new System.Drawing.Point(14, 11);
            this.gbxTuneServer.Name = "gbxTuneServer";
            this.gbxTuneServer.Size = new System.Drawing.Size(463, 236);
            this.gbxTuneServer.TabIndex = 0;
            this.gbxTuneServer.TabStop = false;
            this.gbxTuneServer.Text = "Настройка подключения к БД";
            // 
            // lblConnctYes
            // 
            this.lblConnctYes.AutoSize = true;
            this.lblConnctYes.BackColor = System.Drawing.Color.Green;
            this.lblConnctYes.ForeColor = System.Drawing.Color.White;
            this.lblConnctYes.Location = new System.Drawing.Point(31, 158);
            this.lblConnctYes.MaximumSize = new System.Drawing.Size(150, 20);
            this.lblConnctYes.MinimumSize = new System.Drawing.Size(150, 20);
            this.lblConnctYes.Name = "lblConnctYes";
            this.lblConnctYes.Size = new System.Drawing.Size(150, 20);
            this.lblConnctYes.TabIndex = 17;
            this.lblConnctYes.Text = "СВЯЗЬ УСТАНОВЛЕНА";
            this.lblConnctYes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblConnctYes.Visible = false;
            // 
            // pbrRun
            // 
            this.pbrRun.Location = new System.Drawing.Point(6, 214);
            this.pbrRun.Name = "pbrRun";
            this.pbrRun.Size = new System.Drawing.Size(451, 13);
            this.pbrRun.TabIndex = 3;
            // 
            // cbxUserPass
            // 
            this.cbxUserPass.FormattingEnabled = true;
            this.cbxUserPass.Location = new System.Drawing.Point(280, 131);
            this.cbxUserPass.Name = "cbxUserPass";
            this.cbxUserPass.Size = new System.Drawing.Size(172, 21);
            this.cbxUserPass.TabIndex = 16;
            // 
            // cbxUserName
            // 
            this.cbxUserName.FormattingEnabled = true;
            this.cbxUserName.Location = new System.Drawing.Point(280, 108);
            this.cbxUserName.Name = "cbxUserName";
            this.cbxUserName.Size = new System.Drawing.Size(172, 21);
            this.cbxUserName.TabIndex = 15;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(266, 176);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(91, 31);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "Проверка";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(361, 176);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(91, 31);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbxTimeConnect
            // 
            this.cbxTimeConnect.FormattingEnabled = true;
            this.cbxTimeConnect.Location = new System.Drawing.Point(280, 60);
            this.cbxTimeConnect.Name = "cbxTimeConnect";
            this.cbxTimeConnect.Size = new System.Drawing.Size(172, 21);
            this.cbxTimeConnect.TabIndex = 14;
            this.cbxTimeConnect.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbxTimeConnect_KeyPress);
            // 
            // cbxNameDB
            // 
            this.cbxNameDB.FormattingEnabled = true;
            this.cbxNameDB.Location = new System.Drawing.Point(280, 36);
            this.cbxNameDB.Name = "cbxNameDB";
            this.cbxNameDB.Size = new System.Drawing.Size(172, 21);
            this.cbxNameDB.TabIndex = 13;
            this.cbxNameDB.SelectedIndexChanged += new System.EventHandler(this.cbxNameDB_SelectedIndexChanged);
            // 
            // cbxNameServer
            // 
            this.cbxNameServer.FormattingEnabled = true;
            this.cbxNameServer.Location = new System.Drawing.Point(280, 12);
            this.cbxNameServer.Name = "cbxNameServer";
            this.cbxNameServer.Size = new System.Drawing.Size(172, 21);
            this.cbxNameServer.TabIndex = 12;
            // 
            // chbWinUserAccess
            // 
            this.chbWinUserAccess.AutoSize = true;
            this.chbWinUserAccess.Location = new System.Drawing.Point(281, 88);
            this.chbWinUserAccess.Name = "chbWinUserAccess";
            this.chbWinUserAccess.Size = new System.Drawing.Size(15, 14);
            this.chbWinUserAccess.TabIndex = 9;
            this.chbWinUserAccess.UseVisualStyleBackColor = true;
            this.chbWinUserAccess.CheckedChanged += new System.EventHandler(this.chbWinUserAccess_CheckedChanged);
            // 
            // lblTimeConnect
            // 
            this.lblTimeConnect.AutoSize = true;
            this.lblTimeConnect.Location = new System.Drawing.Point(31, 64);
            this.lblTimeConnect.Name = "lblTimeConnect";
            this.lblTimeConnect.Size = new System.Drawing.Size(161, 13);
            this.lblTimeConnect.TabIndex = 5;
            this.lblTimeConnect.Text = "Время для подключения, сек.:";
            // 
            // lblUserPass
            // 
            this.lblUserPass.AutoSize = true;
            this.lblUserPass.Location = new System.Drawing.Point(31, 136);
            this.lblUserPass.Name = "lblUserPass";
            this.lblUserPass.Size = new System.Drawing.Size(122, 13);
            this.lblUserPass.TabIndex = 4;
            this.lblUserPass.Text = "Пароль пользователя:";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(31, 112);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(106, 13);
            this.lblUserName.TabIndex = 3;
            this.lblUserName.Text = "Имя пользователя:";
            // 
            // lblWinUserAccess
            // 
            this.lblWinUserAccess.AutoSize = true;
            this.lblWinUserAccess.Location = new System.Drawing.Point(31, 88);
            this.lblWinUserAccess.Name = "lblWinUserAccess";
            this.lblWinUserAccess.Size = new System.Drawing.Size(248, 13);
            this.lblWinUserAccess.TabIndex = 2;
            this.lblWinUserAccess.Text = "Аутентификация через пользователя системы:";
            // 
            // lblNameDB
            // 
            this.lblNameDB.AutoSize = true;
            this.lblNameDB.Location = new System.Drawing.Point(31, 40);
            this.lblNameDB.Name = "lblNameDB";
            this.lblNameDB.Size = new System.Drawing.Size(101, 13);
            this.lblNameDB.TabIndex = 1;
            this.lblNameDB.Text = "Имя базы данных:";
            // 
            // lblNameServer
            // 
            this.lblNameServer.AutoSize = true;
            this.lblNameServer.Location = new System.Drawing.Point(31, 16);
            this.lblNameServer.Name = "lblNameServer";
            this.lblNameServer.Size = new System.Drawing.Size(118, 13);
            this.lblNameServer.TabIndex = 0;
            this.lblNameServer.Text = "Название SQL Server:";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnCreateDB
            // 
            this.btnCreateDB.Location = new System.Drawing.Point(361, 121);
            this.btnCreateDB.Name = "btnCreateDB";
            this.btnCreateDB.Size = new System.Drawing.Size(91, 31);
            this.btnCreateDB.TabIndex = 4;
            this.btnCreateDB.Text = "Создать БД";
            this.btnCreateDB.UseVisualStyleBackColor = true;
            this.btnCreateDB.Click += new System.EventHandler(this.btnCreateDB_Click);
            // 
            // lblPatchNewDB
            // 
            this.lblPatchNewDB.AutoSize = true;
            this.lblPatchNewDB.Location = new System.Drawing.Point(31, 74);
            this.lblPatchNewDB.Name = "lblPatchNewDB";
            this.lblPatchNewDB.Size = new System.Drawing.Size(178, 13);
            this.lblPatchNewDB.TabIndex = 17;
            this.lblPatchNewDB.Text = "Путь, при создании базы данных:";
            // 
            // btnFindPatchDB
            // 
            this.btnFindPatchDB.Location = new System.Drawing.Point(426, 70);
            this.btnFindPatchDB.Name = "btnFindPatchDB";
            this.btnFindPatchDB.Size = new System.Drawing.Size(26, 20);
            this.btnFindPatchDB.TabIndex = 5;
            this.btnFindPatchDB.Text = "...";
            this.btnFindPatchDB.UseVisualStyleBackColor = true;
            this.btnFindPatchDB.Click += new System.EventHandler(this.btnFindPatch_Click);
            // 
            // tbxPatchNewDB
            // 
            this.tbxPatchNewDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbxPatchNewDB.Location = new System.Drawing.Point(34, 96);
            this.tbxPatchNewDB.Name = "tbxPatchNewDB";
            this.tbxPatchNewDB.Size = new System.Drawing.Size(418, 20);
            this.tbxPatchNewDB.TabIndex = 18;
            this.tbxPatchNewDB.TextChanged += new System.EventHandler(this.tbxPatchNewDB_TextChanged);
            // 
            // gbxCreateNewDB
            // 
            this.gbxCreateNewDB.Controls.Add(this.btnDeleteDB);
            this.gbxCreateNewDB.Controls.Add(this.tbxSizeDB);
            this.gbxCreateNewDB.Controls.Add(this.lblSizeDB);
            this.gbxCreateNewDB.Controls.Add(this.tbxNameNewDB);
            this.gbxCreateNewDB.Controls.Add(this.lblNameNewDB);
            this.gbxCreateNewDB.Controls.Add(this.lblPatchNewDB);
            this.gbxCreateNewDB.Controls.Add(this.btnFindPatchDB);
            this.gbxCreateNewDB.Controls.Add(this.tbxPatchNewDB);
            this.gbxCreateNewDB.Controls.Add(this.btnCreateDB);
            this.gbxCreateNewDB.Location = new System.Drawing.Point(14, 264);
            this.gbxCreateNewDB.Name = "gbxCreateNewDB";
            this.gbxCreateNewDB.Size = new System.Drawing.Size(461, 159);
            this.gbxCreateNewDB.TabIndex = 5;
            this.gbxCreateNewDB.TabStop = false;
            this.gbxCreateNewDB.Text = "Управление БД";
            // 
            // btnDeleteDB
            // 
            this.btnDeleteDB.Location = new System.Drawing.Point(264, 121);
            this.btnDeleteDB.Name = "btnDeleteDB";
            this.btnDeleteDB.Size = new System.Drawing.Size(91, 31);
            this.btnDeleteDB.TabIndex = 23;
            this.btnDeleteDB.Text = "Удалить БД";
            this.btnDeleteDB.UseVisualStyleBackColor = true;
            this.btnDeleteDB.Click += new System.EventHandler(this.btnDeleteDB_Click);
            // 
            // tbxSizeDB
            // 
            this.tbxSizeDB.Location = new System.Drawing.Point(280, 45);
            this.tbxSizeDB.Name = "tbxSizeDB";
            this.tbxSizeDB.Size = new System.Drawing.Size(172, 20);
            this.tbxSizeDB.TabIndex = 22;
            this.tbxSizeDB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxSizeDB_KeyPress);
            // 
            // lblSizeDB
            // 
            this.lblSizeDB.AutoSize = true;
            this.lblSizeDB.Location = new System.Drawing.Point(31, 45);
            this.lblSizeDB.Name = "lblSizeDB";
            this.lblSizeDB.Size = new System.Drawing.Size(214, 13);
            this.lblSizeDB.TabIndex = 21;
            this.lblSizeDB.Text = "Максимальный размер БД, MB (>10MB):";
            // 
            // tbxNameNewDB
            // 
            this.tbxNameNewDB.Location = new System.Drawing.Point(280, 19);
            this.tbxNameNewDB.Name = "tbxNameNewDB";
            this.tbxNameNewDB.Size = new System.Drawing.Size(172, 20);
            this.tbxNameNewDB.TabIndex = 20;
            // 
            // lblNameNewDB
            // 
            this.lblNameNewDB.AutoSize = true;
            this.lblNameNewDB.Location = new System.Drawing.Point(31, 19);
            this.lblNameNewDB.Name = "lblNameNewDB";
            this.lblNameNewDB.Size = new System.Drawing.Size(101, 13);
            this.lblNameNewDB.TabIndex = 19;
            this.lblNameNewDB.Text = "Имя базы данных:";
            // 
            // btnDelTbl
            // 
            this.btnDelTbl.Location = new System.Drawing.Point(103, 19);
            this.btnDelTbl.Name = "btnDelTbl";
            this.btnDelTbl.Size = new System.Drawing.Size(91, 31);
            this.btnDelTbl.TabIndex = 24;
            this.btnDelTbl.Text = "Удалить";
            this.btnDelTbl.UseVisualStyleBackColor = true;
            this.btnDelTbl.Click += new System.EventHandler(this.btnDelTbl_Click);
            // 
            // btnRenameTbl
            // 
            this.btnRenameTbl.Location = new System.Drawing.Point(6, 56);
            this.btnRenameTbl.Name = "btnRenameTbl";
            this.btnRenameTbl.Size = new System.Drawing.Size(91, 31);
            this.btnRenameTbl.TabIndex = 25;
            this.btnRenameTbl.Text = "Переименов.";
            this.btnRenameTbl.UseVisualStyleBackColor = true;
            this.btnRenameTbl.Click += new System.EventHandler(this.btnRenameTbl_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnListTbl);
            this.groupBox1.Controls.Add(this.tbxCurNameTbl);
            this.groupBox1.Controls.Add(this.btnCreateTbl);
            this.groupBox1.Controls.Add(this.btnRenameTbl);
            this.groupBox1.Controls.Add(this.btnDelTbl);
            this.groupBox1.Location = new System.Drawing.Point(14, 441);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(463, 100);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Управление таблицами в БД";
            // 
            // tbxCurNameTbl
            // 
            this.tbxCurNameTbl.Location = new System.Drawing.Point(280, 19);
            this.tbxCurNameTbl.Name = "tbxCurNameTbl";
            this.tbxCurNameTbl.Size = new System.Drawing.Size(172, 20);
            this.tbxCurNameTbl.TabIndex = 24;
            // 
            // btnCreateTbl
            // 
            this.btnCreateTbl.Location = new System.Drawing.Point(6, 19);
            this.btnCreateTbl.Name = "btnCreateTbl";
            this.btnCreateTbl.Size = new System.Drawing.Size(91, 31);
            this.btnCreateTbl.TabIndex = 26;
            this.btnCreateTbl.Text = "Создать";
            this.btnCreateTbl.UseVisualStyleBackColor = true;
            this.btnCreateTbl.Click += new System.EventHandler(this.btnCreateTbl_Click);
            // 
            // btnListTbl
            // 
            this.btnListTbl.Location = new System.Drawing.Point(103, 56);
            this.btnListTbl.Name = "btnListTbl";
            this.btnListTbl.Size = new System.Drawing.Size(91, 31);
            this.btnListTbl.TabIndex = 27;
            this.btnListTbl.Text = "Список";
            this.btnListTbl.UseVisualStyleBackColor = true;
            this.btnListTbl.Click += new System.EventHandler(this.btnListTbl_Click);
            // 
            // SQLTuneCon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 545);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbxCreateNewDB);
            this.Controls.Add(this.gbxTuneServer);
            this.MaximizeBox = false;
            this.Name = "SQLTuneCon";
            this.Text = "БД";
            this.gbxTuneServer.ResumeLayout(false);
            this.gbxTuneServer.PerformLayout();
            this.gbxCreateNewDB.ResumeLayout(false);
            this.gbxCreateNewDB.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox gbxTuneServer;
    private System.Windows.Forms.CheckBox chbWinUserAccess;
    private System.Windows.Forms.Label lblTimeConnect;
    private System.Windows.Forms.Label lblUserPass;
    private System.Windows.Forms.Label lblUserName;
    private System.Windows.Forms.Label lblWinUserAccess;
    private System.Windows.Forms.Label lblNameDB;
    private System.Windows.Forms.Label lblNameServer;
    private System.Windows.Forms.Button btnTest;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.ProgressBar pbrRun;
    private System.Windows.Forms.ComboBox cbxNameDB;
    private System.Windows.Forms.ComboBox cbxNameServer;
    private System.Windows.Forms.ComboBox cbxUserPass;
    private System.Windows.Forms.ComboBox cbxUserName;
    private System.Windows.Forms.ComboBox cbxTimeConnect;
    private System.Windows.Forms.Timer timer1;
  private System.Windows.Forms.TextBox tbxPatchNewDB;
  private System.Windows.Forms.Button btnFindPatchDB;
  private System.Windows.Forms.Label lblPatchNewDB;
  private System.Windows.Forms.Button btnCreateDB;
  private System.Windows.Forms.GroupBox gbxCreateNewDB;
  private System.Windows.Forms.Label lblNameNewDB;
  private System.Windows.Forms.TextBox tbxNameNewDB;
  private System.Windows.Forms.TextBox tbxSizeDB;
  private System.Windows.Forms.Label lblSizeDB;
    private System.Windows.Forms.Label lblConnctYes;
    private System.Windows.Forms.Button btnDeleteDB;
    private System.Windows.Forms.Button btnDelTbl;
    private System.Windows.Forms.Button btnRenameTbl;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button btnCreateTbl;
    private System.Windows.Forms.TextBox tbxCurNameTbl;
        private System.Windows.Forms.Button btnListTbl;
    }
}

