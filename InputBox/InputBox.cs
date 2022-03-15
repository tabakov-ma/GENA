using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkBox
{
  public partial class InputBox : Form
  {
    private bool OnlyNum = false;
    public InputBox()
    {
      InitializeComponent();
    }
    public InputBox(string title = "Ввод данных",
                  string lblText = "Введите данные:", 
                  string tbxText = "", 
                  bool onlyNum = false )
    {
      InitializeComponent();  
      this.lblText.Text = lblText;
      this.tbxInput.Text = tbxText;
      this.Text = title;
      InitLocation();
      OnlyNum = onlyNum;
      this.AutoSizeMode = AutoSizeMode.GrowAndShrink; // нельзя изменять размер формы
    }
    public string Result = "";
    private void btnOk_Click(object sender, EventArgs e)
    {
      Result = tbxInput.Text;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }
    private void InitLocation()
    {
      Size resolution = Screen.PrimaryScreen.Bounds.Size;
      int X = Cursor.Position.X, Y = Cursor.Position.Y;
      if (Cursor.Position.X <= resolution.Width && Cursor.Position.Y <= resolution.Height)
      {
        if (Cursor.Position.X + this.Width > resolution.Width) X = resolution.Width - this.Width;
        if (Cursor.Position.Y + this.Height > resolution.Height-35) Y = resolution.Height - this.Height-35;
      }
      
      this.StartPosition = FormStartPosition.Manual;
      this.Location = new Point(X, Y);
    }

    private void tbxInput_KeyPress(object sender, KeyPressEventArgs e)
    {
      char number = e.KeyChar;

      if (!Char.IsDigit(number) & !(number == '\b') & OnlyNum)
      {
        e.Handled = true;
      }
    }
    public void ShowDialog(string title = "Ввод данных",
                     string lblText = "Введите данные:",
                     string tbxText = "",
                     bool onlyNum = false)
    {
      this.lblText.Text = lblText;
      this.tbxInput.Text = tbxText;
      this.Text = title;
      OnlyNum = onlyNum;
      InitLocation();
      base.ShowDialog();
    }
  }
}
