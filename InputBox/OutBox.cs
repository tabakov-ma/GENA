using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Box
{
  public partial class OutBox : Form
  {
    private bool result = false;
    public OutBox()
    {
      InitializeComponent();
    }
    public OutBox(Font font, Color foreColor)
    {
      InitializeComponent();
      this.lblText.Font = font;
      this.lblText.ForeColor= foreColor;
    }
    private void InitLocation()
    {
      Size resolution = Screen.PrimaryScreen.Bounds.Size;
      int X = Cursor.Position.X, Y = Cursor.Position.Y;
      if (Cursor.Position.X <= resolution.Width && Cursor.Position.Y <= resolution.Height)
      {
        if (Cursor.Position.X + this.Width > resolution.Width) X = resolution.Width - this.Width;
        if (Cursor.Position.Y + this.Height > resolution.Height - 35) Y = resolution.Height - this.Height - 35;
      }

      this.StartPosition = FormStartPosition.Manual;
      this.Location = new Point(X, Y);
    }
    public bool ShowDialog(string title = "Вывод",
                     string lblText = "Информация")
    {
      this.lblText.Text = lblText;
      this.Text = title;
      InitLocation();
      result = false;
      base.ShowDialog();
      return result;
    }
    public bool ShowDialog(Font font,
                      Color color,
                      string title = "Вывод",
                      string lblText = "Информация")
    {
      this.lblText.Text = lblText;
      this.lblText.Font = font;
      this.lblText.ForeColor = color;
      this.Text = title;
      InitLocation();
      result = false;
      base.ShowDialog();
      return result;
    }
    public new bool ShowDialog()
    {
      InitLocation();
      result = false;
      base.ShowDialog();
      return result;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      result = true;
      base.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      result = false;
      base.Close();
    }
  }


}
