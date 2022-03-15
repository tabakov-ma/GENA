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
   public partial class OutBox : Form
   {
      /// <summary>
      /// Результат действия пользователя
      /// </summary>
      public DialogResult Result;
      public OutBox()
      {
         InitializeComponent();
      }
      public OutBox(Font font, Color foreColor, string textOk = "Ok", string textCancel = "Cancel" )
      {
         InitializeComponent();
         this.lblText.Font = font;
         this.lblText.ForeColor= foreColor;
         this.btnOk.Text = textOk;
         this.btnCancel.Text = textCancel;
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
      public DialogResult ShowDialog(string text = "Информация", 
                                     string title = "Вывод", 
                                     bool onlyOk = false, 
                                     string textOk = "Ok", 
                                     string textCancel = "Cancel")
      {
         this.lblText.Text = text;
         this.Text = title;
         this.btnCancel.Visible = !onlyOk;
         this.btnOk.Left = onlyOk ? this.Width / 2 - this.btnOk.Width / 2 : 92;
         this.btnOk.Text = textOk;
         this.btnCancel.Text = textCancel;
         InitLocation();
         Result = DialogResult.Cancel;
         base.ShowDialog();
         return Result;
      }

      public DialogResult ShowDialog(string text, 
                                     string title,
                                     Font font, 
                                     Color color, 
                                     bool onlyOk = false,
                                     string textOk = "Ok",
                                     string textCancel = "Cancel")
      {
         this.lblText.Text = text;
         this.lblText.Font = font;
         this.lblText.ForeColor = color;
         this.Text = title;
         this.btnCancel.Visible = !onlyOk;
         this.btnOk.Left = onlyOk ? this.Width / 2 - this.btnOk.Width / 2 : 92;
         this.btnOk.Text = textOk;
         this.btnCancel.Text = textCancel;
         InitLocation();
         Result = DialogResult.Cancel;
         base.ShowDialog();
         return Result;
      }

      private void btnOk_Click(object sender, EventArgs e)
      {
         Result = DialogResult.OK;
         base.Close();
      }

      private void btnCancel_Click(object sender, EventArgs e)
      {
         Result = DialogResult.Cancel;
         base.Close();
      }

      private void lblText_TextChanged(object sender, EventArgs e)
      {
         //this.Height = ((Label)sender).Height + 20;
      }

      private void OutBox_Activated(object sender, EventArgs e)
      {
         MaximumSize = new Size( this.MaximumSize.Width, lblText.Height + 60);
         this.Height = lblText.Height + 60;
      }
   }


}
