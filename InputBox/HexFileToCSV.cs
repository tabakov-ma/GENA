using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkFile;
using WorkControl;

namespace WorkBox
{
   public partial class HexFileToCSV : Form
   {
      Dictionary<string, int> TypeCountByte = new Dictionary<string, int>();
      bool ProgChang = false;
      //DataTable dataTable;
      public HexFileToCSV()
      {
         InitializeComponent();
         TypeCountByte.Add("Byte", 1);
         TypeCountByte.Add("Float", 4);
         TypeCountByte.Add("Int16", 2);
         TypeCountByte.Add("Int32", 2);
         TypeCountByte.Add("Real", 4);
         dgvColumns.Rows[0].Cells["NUM"].Value = 1;
      }

      private void txbPath_Enter(object sender, EventArgs e)
      {
      }

      private void dgvColumns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
         DataGridView table = (DataGridView)sender;
         if (ProgChang || table.CurrentCell == null || table.CurrentCell.Value == null || table.CurrentCell.ColumnIndex != 6) return;
         table.CurrentRow.Cells["BYTES"].Value = TypeCountByte[table.CurrentCell.Value.ToString()].ToString();
         int OFFSET = 0;
         if (table.CurrentRow.Index != 0)
            OFFSET = (int.Parse(table.Rows[table.CurrentRow.Index - 1].Cells["BYTES"].Value.ToString())) + (int.Parse(table.Rows[table.CurrentRow.Index - 1].Cells["OFFSET"].Value.ToString()));
         table.CurrentRow.Cells["OFFSET"].Value = OFFSET;
      }

      //private void txbPath_MouseDownEvent(object sender, AxMicrosoft.Vbe.Interop.Forms.MdcTextEvents_MouseDownEvent e)
      //{
      //   OpenFileDialog OPF = new OpenFileDialog();
      //   if (OPF.ShowDialog() == DialogResult.OK)
      //   {
      //      // Получение RAW-Data
      //      txbPath.Text = OPF.FileName;
      //      Files files = new Files();
      //      byte[] data = files.GetBytesFromHexFile(txbPath.Text);

      //      int ColCount = 0;
      //      Dictionary<int,string[]> columns = new Dictionary<int, string[]>();
      //      bool SUBHEAD = false;
      //      List<string[]> Data = new List<string[]>();

      //      // Словарь заголовка и подзаголовка
      //      for (int i = 0; i < dgvColumns.Rows.Count; i++)
      //      {
      //         if (dgvColumns.Rows[i].Cells["NUM"].Value == null ||
      //             dgvColumns.Rows[i].Cells["NUM"].Value.ToString() == "") continue;
      //         ColCount++;
      //         if (dgvColumns.Rows[i].Cells["COLUMN"].Value == null ||
      //             dgvColumns.Rows[i].Cells["COLUMN"].Value.ToString() == "") continue;

      //         try{ columns.Add(key: int.Parse(dgvColumns.Rows[i].Cells["COLUMN"].Value.ToString()),
      //                   value: new string[]{ dgvColumns.Rows[i].Cells["HEAD"].Value.ToString(), 
      //                            dgvColumns.Rows[i].Cells["SUBHEAD"].Value.ToString()});}catch{ }

      //         if (dgvColumns.Rows[i].Cells["SUBHEAD"].Value.ToString() != "") SUBHEAD = true;
      //      }
      //      string[] row = new string[columns.Count];
      //      // Заголовок
      //      for(int i=1; i<=columns.Count;i++)
      //      {
      //         row[i-1] = columns[i][0];
      //      }
      //      Data.Add((string[])row.Clone());
      //      row = new string[columns.Count];
      //      // Подзаголовок
      //      if (SUBHEAD)
      //      {
      //         for (int i = 1; i <= columns.Count; i++)
      //         {
      //            row[i-1] = columns[i][1];
      //         }
      //         Data.Add((string[])row.Clone());
      //         row = new string[columns.Count];
      //      }
      //      // Определение размера структуры
      //      int RowLenBytes = 0;
      //      for (int i = 0; i < ColCount; i++)
      //      {
      //         RowLenBytes = RowLenBytes + int.Parse(dgvColumns.Rows[i].Cells["BYTES"].Value.ToString());
      //      }
      //      DataTable DTRows;
      //      int COLUMN = 0;
      //      int ORDER = 0;
      //      int BYTES = 0;
      //      int OFFSET = 0;
      //      string ADD = "";
      //      string[] CellData;
      //      // 
      //      for (int i = 0; i < data.Length; i = i + RowLenBytes)
      //      {
      //         foreach (var column in columns)
      //         {
      //            DTRows = Ctrl.GetRowsDataTable(table: dgvColumns, FindVal: column.Key.ToString(), ColIndex: 1, FirstRow: false, beside: 1);
      //            CellData = new string[DTRows.Rows.Count];
      //            foreach (DataRow DTRow in DTRows.Rows)
      //            {
      //               COLUMN = int.Parse(DTRow["COLUMN"].ToString());
      //               ORDER  = int.Parse(DTRow["ORDER"].ToString());
      //               BYTES  = int.Parse(DTRow["BYTES"].ToString());
      //               OFFSET = int.Parse(DTRow["OFFSET"].ToString()) + i;
      //               ADD = DTRow["ADD"].ToString();
      //               try
      //               {
      //                  switch (DTRow["TYPE"].ToString())
      //                  {
      //                     case "Byte":
      //                        CellData[ORDER] = data[OFFSET].ToString() + ADD;
      //                        break;
      //                     case "Int16":
      //                        CellData[ORDER] = BitConverter.ToUInt16(data, OFFSET).ToString("00") + ADD;
      //                        break;
      //                     case "Float":
      //                        CellData[ORDER] = BitConverter.ToSingle(data, OFFSET).ToString() + ADD;
      //                        break;
      //                     case "Real":
      //                        CellData[ORDER] = BitConverter.ToSingle(data, OFFSET).ToString() + ADD;
      //                        break;
      //                  }
      //               }
      //               catch{ }
      //            }
      //            row[COLUMN-1] = String.Concat(CellData);
      //         }
      //         Data.Add((string[])row.Clone());
      //         row = new string[columns.Count];
      //      }
      //      files.SetDataCsv(txbPath.Text + ".csv", Data);
      //   }
      //}

      private void dgvColumns_KeyPress(object sender, KeyPressEventArgs e)
      {
         if (e.KeyChar == 22 )
         {
            ProgChang = true;
            BindingList<string[]> vs = new BindingList<string[]>();
            
            Clip(dgvColumns);
            dgvColumns.Refresh();
            ProgChang = false;
         }
      }

      private void Clip(object sender)
      {
         Ctrl ctrl = new Ctrl();
         ctrl.PasteInDataGridViewFromClipboard(ref dgvColumns, AllowAddRows: true);
      }

      private void dgvColumns_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
      {
         DataGridView table = (DataGridView)sender;
         if (ProgChang) return;
         if (table.CurrentRow.Index != 0)
            table.CurrentRow.Cells["NUM"].Value = int.Parse(table.Rows[table.CurrentRow.Index - 1].Cells["NUM"].Value.ToString()) + 1;

      }

      private void button1_Click(object sender, EventArgs e)
      {
         dgvColumns.Visible = !dgvColumns.Visible;
         pbxHelp.Visible = !dgvColumns.Visible;
      }
   }
}
