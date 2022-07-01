using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;




namespace AdvancedTextEditorCSharp
{

    public partial class AdvancedTextEditor : Form
    {
        private int TabCount = 0;
        public int PLCTYPE = 2;
        private int LastPointNum = 0; //координаты последней введеной точки
        private int LastPointLine = 0; //номер линии последней введеной точки
        public const int BEREMIZ = 1;
        public const int MK200C = 2;
        public const int ISAGRAPH = 3;
        public string[] bufline;
        public string tmpstring;
        public bool isControlChar = true;
        public int curLine;
        public int curLineLength;
        public int firstCharLine;      
        Point rtboffset = new Point(12,42);
        List<string> tmplist = new List<string>();
        public AdvancedTextEditor()
        {
            InitializeComponent();
            VarPrompt.Visible = false;
            //toolStripComboBox1.SelectedItem = "Consolas";
            //Font NewFont = new Font(toolStripComboBox1.SelectedItem.ToString(), GetCurrentDocument.SelectionFont.Size, GetCurrentDocument.SelectionFont.Style);
            //GetCurrentDocument.SelectionFont = NewFont;

        }

        #region Input

        void AdvancedTextEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            isControlChar = false;
            int[] lineinfo = { -1, -1 };
            lineinfo = getLineInformation();
            toolStripStatusLabel2.Text = "линия номер" + lineinfo[1].ToString();
            toolStripStatusLabel3.Text = "номер в строке" + GetCurrentDocument.GetFirstCharIndexOfCurrentLine().ToString();
        }

        /// <summary>
        /// IMS 16_02_22 Обработчик нажатия клавишы в RichTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AdvancedTextEditor_KeyUp(object sender, KeyEventArgs e)
        {
            int[] lineinfo = { -1, -1 };
            lineinfo = getLineInformation();
            curLine = lineinfo[1];


            KeysConverter kc = new KeysConverter();
            string keyChar = kc.ConvertToString(e.KeyCode);
            //. 46
            if (e.KeyCode == Keys.OemPeriod)//46
            {
                lineinfo = getLineInformation();
                //Запоминаем положение последней точки в тексте
                if (!VarPrompt.Visible)
                {
                    LastPointNum = lineinfo[0];
                    LastPointLine = lineinfo[1];
                }
                //Отображение формы подсказки
                Point tmppoint = new Point(GetCurrentDocument.GetPositionFromCharIndex(lineinfo[0]).X + rtboffset.X, GetCurrentDocument.GetPositionFromCharIndex(lineinfo[0]).Y + rtboffset.Y);
                VarPrompt.Visible = true;
                VarPrompt.Location = tmppoint;
                //MessageBox.Show($"Form.pointer:1'{GetCurrentDocument.GetPositionFromCharIndex(lineinfo[0])}' 2 '{this.Location}' 3 '{tmppoint.ToString()}' pressed. char number '{lineinfo[0].ToString()}' in line '{lineinfo[1].ToString()}'");
            }
            //" "32
            if (e.KeyCode == Keys.Space)
            {
                lineinfo = getLineInformation();
                //скрытие подсказки
                VarPrompt.Visible = false;
                //tmpstring = getStringToSpace(lineinfo[0], lineinfo[1]);
                //MessageBox.Show($"Form.KeyPress: '{tmpstring}' pressed.");
            }
            //enter 13
            if (e.KeyCode == Keys.Enter)
            {
                lineinfo = getLineInformation();                
                //скрытие подсказки
                VarPrompt.Visible = false;
                //MessageBox.Show($"Form.KeyPress: '{e.KeyChar}' pressed.");
                //Регулирование предыдущей строки
                curLine = curLine - 1;
                regexp(2);
                
            }
            //Escape
            if (e.KeyCode == Keys.Escape)
            {
                //скрытие подсказки
                VarPrompt.Visible = true;
                //!!!Временно
                //Регулирование всего кода (!!! необходмио добавить опцию регулирования только одну строку текущую или предыдущую) 
                //if (!isControlChar)
                //{
                    regexp(3);
                //}
            }

            //Регулирование текущий строки кода
            if (!isControlChar & e.KeyCode != Keys.Enter)
            {
                firstCharLine = GetCurrentDocument.GetFirstCharIndexOfCurrentLine();
                if ((lineinfo[0] != 0) & (lineinfo[1] != 0)) { curLineLength = GetCurrentDocument.Lines[curLine].Length; }
                regexp(1);
            }
            //Если подсказка активна подсвечиваем строку
            if (VarPrompt.Visible & !isControlChar)//& keyChar.Length ==1)//!char.IsControl(Convert.ToChar(keyChar)))
            {
                lineinfo = getLineInformation();
                
                string tmpchar= "";
                //tmpstring = getStringToSpace(lineinfo[0], lineinfo[1]);
                //if ((e) { tmpchar = e.KeyChar.ToString(); };
                    //Анализ строки на переменную для подсказки
                    tmplist = regexpvar(lineinfo[0], lineinfo[1], tmpchar);
                for (int j = 0; j < tmplist.Count; j++)
                {
                    if (j == 0) { testlabel.Text = tmplist[j]; }
                    if (j == 1) { testlabel2.Text = tmplist[j]; }
                    if (j == 2) { testlabel3.Text = tmplist[j]; }
                }
                testlabel4.Text = LastPointNum.ToString();
                //testlabel5.Text = lineinfo[0].ToString();
                //testlabel5.Text = testint.ToString();
                
                //testlabel7.Text = keyChar;
            }
            else
            {
                //Обнуление
                LastPointNum = 0;
                LastPointLine = 0;
            }
            //if (!isControlChar)
            //{
            //    testlabel6.Text = "0";
            //    isControlChar = true;
            //}
            //else
            //{ testlabel6.Text = "1"; }
        }
        #endregion

        #region Methods

        #region Tabs

        private void AddTab()
        {

            RichTextBox Body = new RichTextBox();

            Body.Name = "Body";
            Body.Dock = DockStyle.Fill;
            Body.ContextMenuStrip = contextMenuStrip1;
            Body.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AdvancedTextEditor_KeyPress);
            Body.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AdvancedTextEditor_KeyUp);

            TabPage NewPage = new TabPage();
            TabCount += 1;

            string DocumentText = "Document " + TabCount;
            NewPage.Name = DocumentText;
            NewPage.Text = DocumentText;
            NewPage.Controls.Add(Body);

            tabControl1.TabPages.Add(NewPage);

        }

        private void RemoveTab()
        {
            if (tabControl1.TabPages.Count != 1)
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
            else
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                AddTab();
            }
        }

        private void RemoveAllTabs()
        {
            foreach (TabPage Page in tabControl1.TabPages)
            {
                tabControl1.TabPages.Remove(Page);
            }

            AddTab();
        }

        private void RemoveAllTabsButThis()
        {
            foreach (TabPage Page in tabControl1.TabPages)
            {
                if (Page.Name != tabControl1.SelectedTab.Name)
                {
                    tabControl1.TabPages.Remove(Page);
                }
            }
        }

        #endregion

        #region SaveAndOpen

        private void Save()
        {
            saveFileDialog1.FileName = tabControl1.SelectedTab.Name;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Filter = "RTF|.rtf";
            saveFileDialog1.Title = "Save";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Length > 0)
                {
                    GetCurrentDocument.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                }
            }
        }

        private void SaveAs()
        {
            saveFileDialog1.FileName = tabControl1.SelectedTab.Name;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Filter = "Text Files|*.txt|VB Files|*.vb|C# Files|*.cs|All Files|*.*";
            saveFileDialog1.Title = "Save As";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Length > 0)
                {
                    GetCurrentDocument.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                }
            }
        }

        private void Open()
        {
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog1.Filter = "RTF|*.rtf|Text Files|*.txt|VB Files|*.vb|C# Files|*.cs|All Files|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (openFileDialog1.FileName.Length > 9)
                {
                    GetCurrentDocument.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.RichText);
                }
            }

        }

        private void Generate()
        {
            //Считывание из RichtextBox
            string[] lines = GetCurrentDocument.Lines;
            Boolean DelPrevLine;
            Boolean DelNextLine = false;
            List<string> tmplines = new List<string>();
            List<string> textlines = new List<string>();
            //Запись в файл
            StreamWriter f = new StreamWriter("D:\\tmp\\" + tabControl1.SelectedTab.Name + ".txt");
            for (int i = 0; i < lines.Length; i++)
            {
                //Удаление предыдущей строки 
                if (!DelNextLine)
                {

                    //Проверка есть ли предыдущая строка и парсинг
                    if (i > 0) { tmplines = ParsingText(lines[i], lines[i - 1], out DelPrevLine, out DelNextLine); }
                    else { tmplines = ParsingText(lines[i], "", out DelPrevLine, out DelNextLine); };
                    //Удаление предыдущей строки 
                    if (DelPrevLine) { textlines.RemoveRange(textlines.Count - 1, 1); DelPrevLine = false; }
                    //Подготовка текста
                    for (int j = 0; j < tmplines.Count; j++)
                    {
                        textlines.Add(tmplines[j]);
                    }
                }
                else
                { DelNextLine = false; }
            }
            for (int j = 0; j < textlines.Count; j++)
            {
                f.WriteLine(textlines[j]);
            }
            f.Close();

        }
       

        #endregion

        #region RedLibFunctions
        private int[] getLineInformation()
        {
            //Считывание из RichtextBox
            int[] lineinfo = { -1, -1 };
            lineinfo[0] = GetCurrentDocument.SelectionStart;
            lineinfo[1] = GetCurrentDocument.GetLineFromCharIndex(lineinfo[0]);
            return lineinfo;
        }

        private string getStringToSpace(int charindex, int linenum)
        {
            int firstlinxcharindex = GetCurrentDocument.GetFirstCharIndexFromLine(linenum);
            MessageBox.Show($"Form.firstlineindex: '{firstlinxcharindex.ToString()}' .");
            string strcontain="";
            for (int j = charindex - 1; j < firstlinxcharindex; j--)
            {
                MessageBox.Show($"Form.GetCurrentDocument.Text[: '{j.ToString()}'] = '{GetCurrentDocument.Text}'.");
                if (GetCurrentDocument.Text[j] != ' ')
                {
                    
                    strcontain = GetCurrentDocument.Text[j].ToString() + strcontain;                    
                }
                else { break; }
            }    
            
            return strcontain;
        }

        /// <summary>
        /// Тестовая функция по регулярным выражениям
        /// </summary>
        private void regexp(int lineoption)
        {
            string pattern = @"(\b(while|next|if|then|end_if|end_while|break|exit|true|false)\b)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            int curpos = GetCurrentDocument.SelectionStart;
            int cursellen = GetCurrentDocument.SelectionLength;
            bool returncurpos = false;

            //GetCurrentDocument.SelectionStart = 0;
            //GetCurrentDocument. = Color.Black;
            //GetCurrentDocument.Select(0, GetCurrentDocument.Text.Length);
            //GetCurrentDocument.SelectionColor = Color.Black;
            //GetCurrentDocument.SelectionStart = curpos;
            ////GetCurrentDocument.ForeColor = Color.Red;           
            int numline = 0;
            int lengthline = 0;
            //Если не сменяется строка, то редактируем только эту строку
            if (lineoption == 1) { numline = GetCurrentDocument.GetLineFromCharIndex(curpos); lengthline = 1; }
            //Если переход на новую строку через Enter, то редактируем предыдущую строку
            if (lineoption == 2) { numline = GetCurrentDocument.GetLineFromCharIndex(curpos)-1; lengthline = 1; }
            //Если нужно отредактировать весь текст (например: вставка текста)
            if (lineoption == 3) { numline = 0; lengthline = GetCurrentDocument.Lines.Length; }

            //защита от обработки пустого документа
            if (GetCurrentDocument.TextLength != 0)
            {
                for (int i = numline; i <= numline + lengthline; i++)
                {

                    foreach (Match m in regex.Matches(GetCurrentDocument.Lines[numline]))
                    {
                        //на первом скане форматирование в черное (!!!надо повторять будет везде во всех заменах цвета)
                        if (!returncurpos)
                        {
                            GetCurrentDocument.Select(firstCharLine, curLineLength);
                            GetCurrentDocument.SelectionColor = Color.Black;
                            returncurpos = true;
                        }
                        GetCurrentDocument.Select(firstCharLine + m.Index, m.Length);
                        GetCurrentDocument.SelectionColor = Color.Blue;
                        GetCurrentDocument.SelectedText = GetCurrentDocument.SelectedText.ToUpper();

                        //GetCurrentDocument.SetSelectionUnderlineStyle(UnderLineStyle.Wave);
                        //GetCurrentDocument.SetSelectionUnderlineColor(Color.Red);
                        //GetCurrentDocument.selection
                    }
                }
            }
            //GetCurrentDocument.SelectionFont = new Font(GetCurrentDocument.SelectionFont, FontStyle.Underline); 

            //GetCurrentDocument.ForeColor = Color.Black;

            //var start = richTextBox1.Document.ContentStart;
            //while (start != null && start.CompareTo(richTextBox1.Document.ContentEnd) < 0)
            //{
            //    if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
            //    {
            //        var match = regex.Match(start.GetTextInRun(LogicalDirection.Forward));

            //        var textrange = new TextRange(start.GetPositionAtOffset(match.Index, LogicalDirection.Forward), start.GetPositionAtOffset(match.Index + match.Length, LogicalDirection.Backward));
            //        textrange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));
            //        textrange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            //        start = textrange.End;
            //    }
            //    start = start.GetNextContextPosition(LogicalDirection.Forward);
            //}
            //возврат места ввода
            if (returncurpos) 
            { 
                GetCurrentDocument.SelectionStart = curpos; returncurpos = false;
                if (cursellen > 0)
                {
                    GetCurrentDocument.Select(GetCurrentDocument.SelectionStart, cursellen);
                }
            }
            //возврат шрифта и размера
            float NewSize;
            float.TryParse(toolStripComboBox2.SelectedItem.ToString(), out NewSize);
            Font NewFont = new Font(toolStripComboBox1.SelectedItem.ToString(), NewSize, GetCurrentDocument.SelectionFont.Style);
            GetCurrentDocument.Font = NewFont;
        }

        /// <summary>
        /// функция разбития переменной на составляющие
        /// </summary>
        private List<string> regexpvar(int charnum, int linenum, string tmpchar)
        {
            List<string> partlines = new List<string>();
            //!!!Исключения
            // 1) проверка наличия строки 
            if (linenum < 0) { VarPrompt.Visible = false; return partlines; }
            // 2) проверка что номер символа не стал меньше последней нажатой точки, а также сохранилась строка (!!! исчезает пока только после попытки ввода) 
            if (charnum < LastPointNum | linenum != LastPointLine) { VarPrompt.Visible = false; return partlines; }

            //получаем строку для анализа плюс добавляем последний введеный символ
            string line = GetCurrentDocument.Lines[linenum];           
            string tmpstr ="";
            bool firstline;            
            //нахождение последнего ввода (возвращает все кроме точки)
            string[] splitline = line.Split(new char[] {' '});
            foreach (string m in splitline)
            {
                tmpstr = m ;
            }
            int curpos = GetCurrentDocument.SelectionStart;
            //проверка наличия точки 
            if (!tmpstr.Contains('.')) { VarPrompt.Visible = false; return partlines; }
            //разбиение на слова  (возвращает все кроме точки)
            splitline = tmpstr.Split(new char[] {'.'});

            firstline = false;
            foreach (string m in splitline)
            {
                if (!firstline) { firstline = true;}
                partlines.Add(m);
            }

            GetCurrentDocument.SelectionStart = curpos;
            return partlines;            
        }

        private string replaceWhile(string line, string prevline, out Boolean DelPrevLine)
        {
            string spaceval = "";
            string indexline = "";
            string startline = "";
            string endline = "";
            string endlineC = "";
            int startind, endind;
            DelPrevLine = false;
            string tmplines ="";

            //Проверка что ничего менять не надо
            if (this.PLCTYPE != ISAGRAPH)
            {
                //!!! Временно убрали так как пока счиатем что все циклы с прибавление +1
                ////Считывание из RichtextBox всего текста
                //string[] tmptext = GetCurrentDocument.Lines;
                ////Поиск строки с прибавлением
                //for (int i = 0; i < tmptext.Length; i++)
                //{

                //}
                for (int j = 0; j < line.Length; j++)
                {
                    if (line.IndexOf(' ') != -1 & (line[0] == ' '))
                    {
                        startind = line.IndexOf(' ') + 1;
                        line = line.Substring(startind, line.Length - 1);
                        spaceval = spaceval + " ";
                    }
                    else
                    { break; };
                }
                DelPrevLine = true;
                //index
                endind = prevline.IndexOf(':');
                startind = endind-1;
                indexline = prevline.Substring(startind, endind - startind);
                //startvalue
                startind = prevline.IndexOf('=');
                endind = prevline.IndexOf(';');
                startline = prevline.Substring(startind+1, endind - startind-1);
                //startline = Convert.ToString(int.Parse(startline));
                //endvalue
                startind = line.IndexOf(indexline);
                endind = line.IndexOf('D');
                endlineC = line.Substring(startind, endind - startind-1);
                startind = line.IndexOf("=");
                endind = line.IndexOf('D');
                endline = line.Substring(startind+1, endind - startind - 1);
                //endline = Convert.ToString(int.Parse(startline));
                //В зависимости от типа ПЛК
                switch (this.PLCTYPE)
                {
                    case BEREMIZ:
                        tmplines = spaceval + "FOR " + indexline + " := " + startline + " TO " + endline + " DO";

                        break;
                    case MK200C:
                        tmplines = spaceval + "for (" + indexline + " =" + startline + ";" + endlineC + "; " + indexline +"++ ){";
                        break;
                    case ISAGRAPH:
                        ;
                        break;
                }

            }
            else
            { tmplines = line; };
            return tmplines;

        }

        private string replaceEndWhile(string line, out Boolean DelPrevLine)
        {
            string spaceval = "";
            DelPrevLine = false;
            string tmplines ="";
            int startind;
            //Проверка что ничего менять не надо
            if (this.PLCTYPE != ISAGRAPH)
            {
                //!!! Временно убрали так как пока счиатем что все циклы с прибавление +1
                ////Считывание из RichtextBox всего текста
                //string[] tmptext = GetCurrentDocument.Lines;
                ////Поиск строки с прибавлением
                //for (int i = 0; i < tmptext.Length; i++)
                //{

                //}
                DelPrevLine = true;
                for (int j = 0; j < line.Length; j++)
                {
                    if (line.IndexOf(' ') != -1 & (line[0] == ' '))
                    {
                        startind = line.IndexOf(' ') + 1;
                        line = line.Substring(startind, line.Length - 1);
                        spaceval = spaceval + " ";
                    }
                    else
                    { break; };
                }
                //В зависимости от типа ПЛК
                switch (this.PLCTYPE)
                {
                    case BEREMIZ:
                        tmplines= spaceval + "END_FOR;";
                        break;
                    case MK200C:
                        tmplines = spaceval + "}";
                        break;
                }

            }
            else
                { tmplines = line; };
            return tmplines;

        }

        private string replaceIf(string line)
        {
            string replaceline = "";
            string tmplines = "";
            //Проверка что ничего менять не надо
            if (this.PLCTYPE != ISAGRAPH)
            {
                switch (this.PLCTYPE)
                {
                    case BEREMIZ:
                        replaceline = "IF ";
                        break;
                    case MK200C:
                        replaceline = "if (";
                        break;
                    case ISAGRAPH:
                        ;
                        break;
                }
                tmplines = line.Replace("IF ", replaceline);
            }
            else
            { tmplines = line; };
            return tmplines;
        }
        
        private List<string> replaceRtrig(string line, out Boolean DelNextLine)
        {
            string spaceval = "";
            string inline = "";
            string rtline = "";
            int startind, endind;
            List<string> tmplines = new List<string>();
            DelNextLine = false;
            if (this.PLCTYPE != ISAGRAPH)
            {
                DelNextLine = true;
                for (int j = 0; j < line.Length; j++)
                {
                    if (line.IndexOf(' ') != -1 & (line[0] == ' '))
                    {
                        startind = line.IndexOf(' ') + 1;
                        line = line.Substring(startind, line.Length - 1);
                        spaceval = spaceval + " ";
                    }
                    else
                    { break; };
                }
                //in
                startind = line.IndexOf('(') + 1;
                endind = line.IndexOf(',');
                inline = line.Substring(startind, endind - startind);
                startind = line.IndexOf(',') + 2;
                endind = line.IndexOf(')');
                rtline = line.Substring(startind, endind - startind);

                //Название функции
                switch (this.PLCTYPE)
                {
                    case BEREMIZ:
                        tmplines.Add(spaceval + "RTFunc:= " + rtline + ";");
                        tmplines.Add(spaceval + "RetFunc := Rtrig(" + inline + ", RTFunc);");
                        tmplines.Add(spaceval + rtline + " := RTFunc;");
                        break;
                    case MK200C:
                        tmplines.Add(spaceval + "RetFunc := Rtrig(" + inline + "," + rtline + ");");
                        break;
                    case ISAGRAPH:
                        ;
                        break;
                }
            }
            else
            { tmplines.Add(line); };
            return tmplines;
        }

 

        private List<string> ParsingText(string line, string prevline, out Boolean DelPrevLine, out Boolean DelNextLine)
        {
            List<string> tmplines = new List<string>();
            DelPrevLine = false;
            DelNextLine = false;
            string tmpstringline = line;
            tmplines.Add(tmpstringline);
            if (line.Contains("IF"))
            {
                tmpstringline = replaceIf(line);
                tmplines[0] = tmpstringline;
            }
            if (line.Contains("Rtrig"))
            {
                tmplines = replaceRtrig(tmpstringline, out DelNextLine);
            }
            if (line.Contains("WHILE") & !line.Contains("_WHILE"))
            {
                tmpstringline = replaceWhile(tmpstringline, prevline, out DelPrevLine);
                tmplines[0] = tmpstringline;
            }
            if (line.Contains("END_WHILE") )
            {
                tmpstringline = replaceEndWhile(tmpstringline, out DelPrevLine);
                tmplines[0] = tmpstringline;
            }
            return tmplines;
        }


        #endregion
        #region TextFunctions

        private void Undo()
        {
            GetCurrentDocument.Undo();
        }

        private void Redo()
        {
            GetCurrentDocument.Redo();
        }

        private void Cut()
        {
            GetCurrentDocument.Cut();
        }

        private void Copy()
        {
            GetCurrentDocument.Copy();
        }

        private void Paste()
        {
            GetCurrentDocument.Paste();
        }

        private void SelectAll()
        {
            GetCurrentDocument.SelectAll();
        }

        #endregion

      #region General

        private void GetFontCollection()
        {
            InstalledFontCollection InsFonts = new InstalledFontCollection();

            foreach (FontFamily item in InsFonts.Families)
            {
                if (item.Name == "Consolas" | item.Name == "SimSun-ExtB")
                toolStripComboBox1.Items.Add(item.Name);
            }
            toolStripComboBox1.SelectedIndex= 0;
        }

        private void PopulateFontSizes()
        {
            for (int i = 1; i <= 75; i++)
            {
                toolStripComboBox2.Items.Add(i);
            }

            toolStripComboBox2.SelectedIndex = 11;
        }
        #endregion


  #endregion

#region Properties

        private RichTextBox GetCurrentDocument
        {
            get { return (System.Windows.Forms.RichTextBox)tabControl1.SelectedTab.Controls["Body"]; }
        }

        #endregion

#region EventBindings



        private void AdvancedTextEditor_Load(object sender, EventArgs e)
        {
            AddTab();
            GetFontCollection();
            PopulateFontSizes();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (GetCurrentDocument.Text.Length > 0)
            {
                toolStripStatusLabel1.Text = GetCurrentDocument.Text.Length.ToString();
            }

        }

        

        #region Menu

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTab();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void generateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Generate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAll();
        }



        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveTab();
        }

        #endregion

        #region Toolbar


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Font BoldFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Bold);
            Font RegularFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (GetCurrentDocument.SelectionFont.Bold)
            {
                GetCurrentDocument.SelectionFont = RegularFont;
            }
            else
            {
                GetCurrentDocument.SelectionFont = BoldFont;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Font ItalicFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Italic);
            Font RegularFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (GetCurrentDocument.SelectionFont.Italic)
            {
                GetCurrentDocument.SelectionFont = RegularFont;
            }
            else
            {
                GetCurrentDocument.SelectionFont = ItalicFont;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Font UnderlineFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Underline);
            Font RegularFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (GetCurrentDocument.SelectionFont.Underline)
            {
                GetCurrentDocument.SelectionFont = RegularFont;
            }
            else
            {
                GetCurrentDocument.SelectionFont = UnderlineFont;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Font Strikeout = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Strikeout);
            Font RegularFont = new Font(GetCurrentDocument.SelectionFont.FontFamily, GetCurrentDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (GetCurrentDocument.SelectionFont.Strikeout)
            {
                GetCurrentDocument.SelectionFont = RegularFont;
            }
            else
            {
                GetCurrentDocument.SelectionFont = Strikeout;
            }
        }


        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectedText = GetCurrentDocument.SelectedText.ToUpper();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectedText = GetCurrentDocument.SelectedText.ToLower();
        }





        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetCurrentDocument.SelectionColor = colorDialog1.Color;
            }
        }

        private void HighlighGreen_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectionBackColor = Color.LightGreen;
        }

        private void HighlighOrange_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectionBackColor = Color.Orange;
        }

        private void HighlighYellow_Click(object sender, EventArgs e)
        {
            GetCurrentDocument.SelectionBackColor = Color.Yellow;
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            Font NewFont = new Font(toolStripComboBox1.SelectedItem.ToString(), GetCurrentDocument.SelectionFont.Size, GetCurrentDocument.SelectionFont.Style);


            GetCurrentDocument.Font = NewFont;
            //GetCurrentDocument.SelectionFont = NewFont;
            //MessageBox.Show($"Font: '{NewFont.ToString()}' pressed.");
        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            float NewSize;

            float.TryParse(toolStripComboBox2.SelectedItem.ToString(), out NewSize);

            Font NewFont = new Font(GetCurrentDocument.SelectionFont.Name, NewSize, GetCurrentDocument.SelectionFont.Style);

            GetCurrentDocument.Font = NewFont;
        }

        #endregion

        #region LeftToolStrip

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            AddTab();
        }

        private void RemoveTabToolStripButton_Click(object sender, EventArgs e)
        {
            RemoveTab();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            Paste();
        }

        #endregion

        #region ContextMenu

        private void undoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void redoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveAllTabs();
        }

        private void closeAllButThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveAllTabsButThis();
        }






        #endregion

        #endregion

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }
    }
}
