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
   public partial class Paste : Form
   {
      public Paste(int x = 0, int y = 0)
      {
         InitializeComponent();
         this.Left = x;
         this.Top = y;
      }

      private void btnCancel_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      private void btnPaste_Click(object sender, EventArgs e)
      {
         this.DialogResult = DialogResult.Yes;
      }

      private void Paste_Load(object sender, EventArgs e)
      {

      }
   }
}
