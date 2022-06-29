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
using System.IO;
using WorkMisc;

namespace WorkBox
{
   public partial class HexFileRW : Form
   {
      DataTable dataAll;
      DataTable dataChange;
      string BufferData;
      bool ProgChange = false;
      bool Remove = false;
      bool Paste = false;
      public HexFileRW()
      {
      
         InitializeComponent();

         toolTip1.SetToolTip(cmbOrder,  "Порядок следования Byte");
         toolTip1.SetToolTip(ckbFI,     "Режим редактирования Value float или int");
         toolTip1.SetToolTip(ckbHexViw, "Представление Byte и Address HEX");

         dgvBits.Columns.Clear();

         DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
         dataGridViewCellStyle.Font = new Font("Tahoma", (float)8.0);
         //dataGridViewCellStyle.BackColor = Color.FromArgb(255, 240, 240, 240);
         dgvBits.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
         dgvBits.EnableHeadersVisualStyles = false;

         //dgvBits.ColumnHeadersVisible = false;
         dgvBits.RowHeadersVisible = false;

         dgvBits.AllowUserToAddRows = false;
         dgvBits.AllowUserToResizeRows = false;
         dgvBits.AllowUserToResizeColumns = false;
         //dgvBits.ScrollBars = ScrollBars.Both;

         

         for (int i=0;i<32;i++)
         {
            DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
            column.Width = 32;
            column.HeaderText = "b" + (31-i);
            column.Name = "b" + (31-i);
            column.HeaderCell.Style.BackColor = ((i / 8) % 2) == 1 ? Color.FromArgb(180,230,160)
                                                                   : Color.White;
            dgvBits.Columns.Add(column);
         }
         dgvBits.Rows.Add(false, false, false, false, false, false, false, false,
                          false, false, false, false, false, false, false, false,
                          false, false, false, false, false, false, false, false,
                          false, false, false, false, false, false, false, false);
         foreach(DataGridViewCell cell in dgvBits.Rows[0].Cells)
         {
            cell.Style.BackColor = Color.LightGray;
            cell.Selected = false;
         }
         dgvBits.Enabled = false;
         dgvBits.Refresh();
      }

      private void CalcDgvDataFloatOrInt()
      {
         int EndRows;
         EndRows = ckbNotZero.Checked ? dgvData.Rows.Count : dgvData.Rows.Count - 1;
         if (ckbFI.Checked)
         {
            for (int i = 0; i < EndRows; i++)
            {
               dataAll.Rows[i]["Value"] = BitConverter.ToSingle(BitConverter.GetBytes(int.Parse(dgvData.Rows[i].Cells["Value"].Value.ToString())), 0);
            }
         }
         else
         {
            for (int i = 0; i < EndRows; i++)
            {
               dataAll.Rows[i]["Value"] = BitConverter.ToInt32(BitConverter.GetBytes(float.Parse(dgvData.Rows[i].Cells["Value"].Value.ToString())), 0);
            }
         }
         dgvData.DataSource = dataAll;
         dgvData.Refresh();
      }
      private bool CheckInputData()
      {
         bool result = false;
         if (cmbOrder.Text.Length != 4) {}
         else { foreach (char ch in cmbOrder.Text) { result = "0123".Contains(ch); } }
         if (result) cmbOrder.BackColor = Color.White;
         else        cmbOrder.BackColor = Color.Red;
         return result;
      }
      private void ReadData(DataTable table)
      {
         if (!CheckInputData()) return;
         ProgChange = true;

         Files files = new Files();
         List<byte[]> RawData = new List<byte[]>();
         Dictionary<int, float> FloatData = null;
         string[] TempData = new string[5];

         if (table == null)
         {
            FloatData = files.GetFloatFromHexFile(txbPath.Text, out RawData, 0, 0, cmbOrder.Text, ckbNotZero.Checked);
            
            table = new DataTable();
            table.Columns.Add(new DataColumn(columnName: "ADDRESS"));
            table.Columns.Add(new DataColumn(columnName: "byte0"));
            table.Columns.Add(new DataColumn(columnName: "byte1"));
            table.Columns.Add(new DataColumn(columnName: "byte2"));
            table.Columns.Add(new DataColumn(columnName: "byte3"));
            table.Columns.Add(new DataColumn(columnName: "NUM"));
            table.Columns.Add(new DataColumn(columnName: "VALUE"));
            table.Columns.Add(new DataColumn(columnName: "HEX"));
            table.Columns.Add(new DataColumn(columnName: "BITS"));

            if ( FloatData == null )
            {
               // Создаём новый пустой файл
               files.SetFloatInHexFile(txbPath.Text, new float[] { 0, 0 }, 0, cmbOrder.Text);
               FloatData = files.GetFloatFromHexFile(txbPath.Text, out RawData, 0, 0, cmbOrder.Text, ckbNotZero.Checked);
            }

            int i = 0;
            foreach (var fData in FloatData)
            {
               if (ckbHexViw.Checked)
               {
                  TempData[0] = fData.Key.ToString("X2");
                  TempData[1] = RawData[i][0].ToString("X2");
                  TempData[2] = RawData[i][1].ToString("X2");
                  TempData[3] = RawData[i][2].ToString("X2");
                  TempData[4] = RawData[i][3].ToString("X2");
               }
               else
               {
                  TempData[0] = fData.Key.ToString();
                  TempData[1] = RawData[i][0].ToString();
                  TempData[2] = RawData[i][1].ToString();
                  TempData[3] = RawData[i][2].ToString();
                  TempData[4] = RawData[i][3].ToString();
               }
               if (ckbFI.Checked)
               {
                  table.Rows.Add(TempData[0],
                                 TempData[1],
                                 TempData[2],
                                 TempData[3],
                                 TempData[4], i, 
                                 fData.Value, BitConverter.ToString(BitConverter.GetBytes(fData.Value),0),
                                 WorkMisc.Misc.GetBitsState(fData.Value));
               }
               else
               {
                  byte[] valB = BitConverter.GetBytes(fData.Value);
                  int valI = BitConverter.ToInt32(valB,0);
                  table.Rows.Add(TempData[0],
                                    TempData[1],
                                    TempData[2],
                                    TempData[3],
                                    TempData[4], i, valI, 
                                    BitConverter.ToString(BitConverter.GetBytes(valI), 0),
                                    WorkMisc.Misc.GetBitsState(valI));
               }
               i++;
            }

            dgvData.DataSource = dataAll = table;
            dgvData.Columns["address"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns["address"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns["NUM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns["VALUE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 240, 240, 240);
            dgvData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            
            foreach (DataGridViewColumn column in dgvData.Columns)
            { column.SortMode = DataGridViewColumnSortMode.NotSortable; }
            for(int nb=0;nb<4;nb++)
            {
               dgvData.Columns["byte" + nb.ToString()].DefaultCellStyle.ForeColor = Color.DarkGreen;
               dgvData.Columns["byte" + nb.ToString()].HeaderCell.Style.ForeColor = Color.DarkGreen;
            }
         }
         else
         {
            try
            {
               foreach (DataRow row in table.Rows)
               {
                  int Address = (int)WorkMisc.Misc.HexConverterStrToInt(row["Address"].ToString(), ckbHexViw.Checked);
                  FloatData = files.GetFloatFromHexFile(txbPath.Text, out RawData, Address / 4, 1, cmbOrder.Text, ckbNotZero.Checked);
                  DataGridViewRow FindRow = Ctrl.Find(dgvData, "Address", row["Address"].ToString());
                  if (RawData.Count == 0)
                  {
                     if (FindRow != null)
                     {
                        dgvData.Rows.Remove(FindRow);
                     }
                  }
                  else
                  {
                     if (ckbHexViw.Checked)
                     {
                        TempData[0] = RawData[0][0].ToString("X2");
                        TempData[1] = RawData[0][1].ToString("X2");
                        TempData[2] = RawData[0][2].ToString("X2");
                        TempData[3] = RawData[0][3].ToString("X2");
                     }
                     else
                     {
                        TempData[0] = RawData[0][0].ToString();
                        TempData[1] = RawData[0][1].ToString();
                        TempData[2] = RawData[0][2].ToString();
                        TempData[3] = RawData[0][3].ToString();
                     }
                     FindRow.Cells["Byte0"].Value = TempData[0];
                     FindRow.Cells["Byte1"].Value = TempData[1];
                     FindRow.Cells["Byte2"].Value = TempData[2];
                     FindRow.Cells["Byte3"].Value = TempData[3];
                  }
               }
            }
            catch{ }
         }
         TuneDefDgvData();
         CalcDgvBits(dgvData);
         ProgChange = false;
      }
      private void CalcDgvDataAddresNum()
      {
         for (int i = 0; i < dataAll.Rows.Count; i++)
         {
            dataAll.Rows[i]["Address"] = WorkMisc.Misc.HexConverterStrToStr((i * 4).ToString(), ckbHexViw.Checked);
            dataAll.Rows[i]["Num"] = i.ToString();
         }
      }
      private void TuneDefDgvData()
      {
         for (int c = 0; c < 9; c++)
         {
            dgvData.Columns[c].ReadOnly = true;
            dgvData.Columns[c].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvData.Columns[c].DefaultCellStyle.BackColor = Color.FromArgb(180,230,160);
         }
         
         for (int c = 1; c < 5; c++)
         {
            dgvData.Columns[c].ReadOnly = false;
            dgvData.Columns[c].DefaultCellStyle.BackColor = Color.White;
         }

         dgvData.Columns[6].ReadOnly = false;
         dgvData.Columns[6].DefaultCellStyle.BackColor = Color.White;
         //dgvData.Refresh();
      }

      private void btnRead_Click(object sender, EventArgs e)
      {
         ReadData(null);
         GoEdit();
      }

      private bool SetCurRowFromBytes(byte[] bytes, ref DataGridView table, int OffSet=0, bool GoLeft=false, bool HexView=false)
      {
         try
         {
            Calc calc;
            if (GoLeft) calc = Decrease; else calc = Increase;
            for (int b = 0; b < bytes.Length; b++)
            {
               table.CurrentRow.Cells[OffSet].Value = WorkMisc.Misc.HexConverterByteToStr(bytes[b],ckbHexViw.Checked);
               OffSet = calc(OffSet);
            }
            return true;
         }
         catch{ return false; }
      }

      private byte[] GetBytesFromRow(DataGridViewRow row)
      {
         byte[] result = new byte[4] { byte.Parse(WorkMisc.Misc.HexConverterStrToInt(row.Cells[1].Value.ToString(), ckbHexViw.Checked, false).ToString()),
                                       byte.Parse(WorkMisc.Misc.HexConverterStrToInt(row.Cells[2].Value.ToString(), ckbHexViw.Checked, false).ToString()),
                                       byte.Parse(WorkMisc.Misc.HexConverterStrToInt(row.Cells[3].Value.ToString(), ckbHexViw.Checked, false).ToString()),
                                       byte.Parse(WorkMisc.Misc.HexConverterStrToInt(row.Cells[4].Value.ToString(), ckbHexViw.Checked, false).ToString()) };
         return result;
      }
      private byte[] GetBytesFromRow(DataRow row)
      {
         
      byte[] result = new byte[4] { byte.Parse(WorkMisc.Misc.HexConverterStrToInt(row[int.Parse(cmbOrder.Text[0].ToString()) + 1].ToString(), ckbHexViw.Checked, false).ToString()),
                                       byte.Parse(WorkMisc.Misc.HexConverterStrToInt(row[int.Parse(cmbOrder.Text[1].ToString()) + 1].ToString(), ckbHexViw.Checked, false).ToString()),
                                       byte.Parse(WorkMisc.Misc.HexConverterStrToInt(row[int.Parse(cmbOrder.Text[2].ToString()) + 1].ToString(), ckbHexViw.Checked, false).ToString()),
                                       byte.Parse(WorkMisc.Misc.HexConverterStrToInt(row[int.Parse(cmbOrder.Text[3].ToString()) + 1].ToString(), ckbHexViw.Checked, false).ToString()) };
         return result;
      }

      private void dgvData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
      {
         DataGridView table = (DataGridView)sender;
         string BufferData_ = "";
         try
         {
            float valF;
            int valI;
            byte valB;
            if (table.CurrentCell.ColumnIndex == 6)
            {
               if (ckbFI.Checked)
               {
                  valF = float.Parse(table.CurrentCell.Value?.ToString().Replace(".", ","));
                  BufferData_ = valF.ToString();
               }
               else
               {
                  valI = int.Parse(table.CurrentCell.Value?.ToString().Replace(".", ","));
                  BufferData_ = valI.ToString();
               }
            }
            else
            {
               valB = (byte)WorkMisc.Misc.HexConverterStrToByte(table.CurrentCell.Value.ToString(), ckbHexViw.Checked);
               BufferData_ = valB.ToString();
            }
         }
         catch (Exception ex) { table.CurrentCell.Value = BufferData; }

         // Добавляем в таблицу записи если значение изменилось
         if (table.CurrentCell.Value.ToString() != BufferData)
         {
            BufferData = WorkMisc.Misc.HexConverterStrToStr(BufferData_, ckbHexViw.Checked);
            ProgChange = true;
            int SelectedCellCount = table.SelectedCells.Count;
            foreach (DataGridViewCell cell in table.SelectedCells)
            {
               if (SelectedCellCount != 1)
               {
                  cell.Value = BufferData;
                  table.CurrentCell = cell;
               }
               CalcDgvDataBytesValue();
               CalcDgvBits(sender);
               GoWrite();
            }
            ProgChange = false;
         }
      }

      private void CalcDgvDataBytesValue(bool chBit=false)
      
      {
         DataGridView table = dgvData;
         if (dataChange == null) dataChange = dataAll.Clone();
         DataRow row = dataChange.NewRow();
         for (int i = 0; 
         i < table.CurrentRow.Cells.Count; i++)
         {
            row[i] = table.CurrentRow.Cells[i].Value;
         }
         foreach (DataRow dataRow in dataChange.Rows)
         {
            if (dataRow["Address"] == row["Address"] ||
               (dataRow["Address"].ToString() == "" & dataRow["Value"] == row["Value"]))
            {
               dataChange.Rows.Remove(dataRow);
               break;
            }
         }
         // Редактирования Byte или Bits
         if (table.CurrentCell.ColumnIndex < 6 || chBit)
         {
            byte[] bytes = GetBytesFromRow(row);
            if (ckbFI.Checked)
            {
               float valF = BitConverter.ToSingle(bytes, 0);
               row["Value"] = valF;
               row["HEX"] = BitConverter.ToString(BitConverter.GetBytes(valF), 0);
               row["Bits"] = WorkMisc.Misc.GetBitsState(valF);
            }
            else
            {
               int valI = BitConverter.ToInt32(bytes, 0);
               row["Value"] = valI;
               row["HEX"] = BitConverter.ToString(BitConverter.GetBytes(valI), 0);
               row["Bits"] = WorkMisc.Misc.GetBitsState(valI);
            }
            table.CurrentRow.Cells["Value"].Value = row["Value"].ToString();
            table.CurrentRow.Cells[7].Value = row["HEX"];
            table.CurrentRow.Cells[8].Value = row["Bits"];
         }
         else
         {
            float valF;
            int valI;
            string valS;
            byte[] TempButes;
            valS = table.CurrentRow.Cells["Value"].Value.ToString();
            if (ckbFI.Checked)
            {
               valF = float.Parse(valS == "" ? "0,0" : valS.Replace(".",","));
               TempButes = WorkMisc.Misc.ByteSwap(BitConverter.GetBytes(valF), cmbOrder.Text);
               row["HEX"] = BitConverter.ToString(BitConverter.GetBytes(valF), 0);
               row["Bits"] = WorkMisc.Misc.GetBitsState(valF);
            }
            else
            {
               valI = int.Parse(valS == ""?"0": valS);
               TempButes = WorkMisc.Misc.ByteSwap(BitConverter.GetBytes(valI), cmbOrder.Text);
               row["HEX"] = BitConverter.ToString(BitConverter.GetBytes(valI), 0);
               row["Bits"] = WorkMisc.Misc.GetBitsState(valI);
            }
            for (int i = 1; i <= TempButes.Length; i++) 
            { 
               table.CurrentRow.Cells[i].Value = row[i] = WorkMisc.Misc.HexConverterByteToStr(TempButes[i - 1], ckbHexViw.Checked); 
            }
            table.CurrentRow.Cells[7].Value = row["HEX"];
            table.CurrentRow.Cells[8].Value = row["BITS"];
         }
         dataChange.Rows.Add(row);
      }

      private bool WriteData(DataTable table)
      {
         bool result = false;
         btnWrite.Enabled = false;
         Files files = new Files();
         if (Remove)
         {
            File.Copy(txbPath.Text, txbPath.Text + ".bak", true);
            File.Delete(txbPath.Text);
            FileStream fs = File.Create(txbPath.Text);
            fs.Close();
         }
         if ( table?.Rows.Count > 0 )
         {
            foreach (DataRow row in table.Rows)
            {
               int Address = (int)WorkMisc.Misc.HexConverterStrToInt(row["Address"].ToString(), ckbHexViw.Checked, errNull: false);
               byte[] Data = GetBytesFromRow(row);
               files.SetBytesInHexFile(Path: txbPath.Text,
                                          Data: Data,
                                          Addr: Address / 4);
            }
         }
         return result;
      }
      
      private void btnWrite_Click(object sender, EventArgs e)
      {
         WriteData(Remove || Paste ? dataAll : dataChange);
         ReadData(Remove ? dataAll : dataChange);
         dataChange = null;
         GoEdit();
         dgvData.Select();
         Remove = false;
         Paste = false;
      }

      private void ckbNotZero_CheckedChanged(object sender, EventArgs e)
      {
         ReadData(null);
         GoEdit();
         if (ckbNotZero.CheckState == CheckState.Checked)
         {
            ProgChange = true;
            dgvData.AllowUserToAddRows = false;
            dgvData.AllowUserToDeleteRows = false;
            dgvData.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(255,240,240,240);
            dgvData.Refresh();
            ProgChange = false;
         }
         else
         {
            if ( dgvData.Rows.Count != 0 )
               dgvData.CurrentCell = dgvData.Rows[dgvData.Rows.Count - 1].Cells["Value"];
            dgvData.AllowUserToAddRows = true;
            dgvData.AllowUserToDeleteRows = true;
            dgvData.RowHeadersDefaultCellStyle.BackColor = Color.White;
            dgvData.Refresh();
         }
      }

      private void ckbHexViw_CheckedChanged(object sender, EventArgs e)
      {
         ReadData(null);
         GoEdit();
      }
      private void SetDgvBits(byte[] Bytes)
      {
         for(int numByte=0; numByte< Bytes.Length; numByte++)
         {
            for (int numBitByte = 0; numBitByte < 8; numBitByte++)
            {
               bool val = WorkMisc.Misc.GetBit(Bytes[numByte], numBitByte);
               int numBitInt = numByte * 8 + numBitByte;
               int numBitDgv = 31 - numBitInt;
               dgvBits.Rows[0].Cells[numBitDgv].Value = val;
               dgvBits.Rows[0].Cells[numBitDgv].Selected = false;
               dgvBits.Columns[numBitDgv].ReadOnly = false;//! Write;
               dgvBits.Rows[0].Cells[numBitDgv].Style.BackColor = Color.White; 
            }
         }
      }

      private void CalcDgvBits(object sender)
      {
         DataGridView table = (DataGridView)sender;
         DataGridViewRow row = table.CurrentRow;
         DataGridViewCell cell = table.CurrentCell;
         if (cell == null) return;

         //BufferData = cell?.Value.ToString();
         try
         {
            byte[] dataB = GetBytesFromRow(row);
            SetDgvBits(dataB);
         }
         catch { }
      }



      private void dgvData_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
      {
         if (! ProgChange )
         {
            DataGridViewRowCollection rows = ((DataGridView)sender).Rows;
            int Address;
            int Num;
            if (dgvData.CurrentRow != null)
            {
               if (rows.Count > 1)
               {
                  if (dgvData.CurrentRow.Index == 0)
                  {
                     Address = 0;
                     Num = 0;
                  }
                  else
                  {
                     Address = (int)WorkMisc.Misc.HexConverterStrToInt(rows[dgvData.CurrentRow.Index - 1].Cells["Address"].Value.ToString(), ckbHexViw.Checked) + 4;
                     Num = int.Parse(rows[dgvData.CurrentRow.Index - 1].Cells["NUM"].Value.ToString()) + 1;
                  }
                  dgvData.CurrentRow.Cells["address"].Value = WorkMisc.Misc.HexConverterStrToStr(Address.ToString(), ckbHexViw.Checked);
                  dgvData.CurrentRow.Cells["NUM"].Value = Num;
               }
            }
         }
      }

      private void dgvData_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
      {
         if ( !ProgChange && ((DataGridView)sender).SelectedRows.Count == 0 )
         {
            Remove = true;
            GoWrite();
            InitNumDataTable(dataAll);
         }
      }
      private void InitNumDataGridView(DataGridView table)
      {
         for(int i=0; i<table.Rows.Count-1;i++)
         {
            table.Rows[i].Cells["address"].Value = WorkMisc.Misc.HexConverterStrToStr((i * 4).ToString(), ckbHexViw.Checked);
            table.Rows[i].Cells["NUM"].Value = i.ToString();
         }
      }
      private void InitNumDataTable(DataTable table)
      {
         long addr = 0;
         for (int i = 0; i < table.Rows.Count; i++)
         {
            if ( table.Rows[i].RowState == DataRowState.Added )
            {
               table.Rows[i]["address"] = WorkMisc.Misc.HexConverterStrToStr((addr * 4).ToString(), ckbHexViw.Checked);
               table.Rows[i]["NUM"] = i.ToString();
               addr++;
            }
         }
         dgvData.DataSource = table;
         dgvData.Refresh();
      }

      private void dgvData_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
      {
         CalcDgvBits(sender);
         if ( !ckbNotZero.Checked && 
              e.Button == MouseButtons.Right && 
            ((DataGridView)sender).CurrentCell.ColumnIndex == 6 )
         {
            if (Clipboard.ContainsText() == true)
            {
               Paste paste = new Paste(Cursor.Position.X, Cursor.Position.Y);
               if (paste.ShowDialog() == DialogResult.Yes)
               {
                  Clip(sender);
               }
            }
         }
      }
      private void Clip(object sender)
      {
         Ctrl ctrl = new Ctrl();
         if (ctrl.PasteInDataTableFromClipboard(
                        table: ref dataAll,
                     CurRow: ((DataGridView)sender).CurrentCell.RowIndex,
                     CurCol: ((DataGridView)sender).CurrentCell.ColumnIndex,
               AllowAddRows: true,
                 permitData: Ctrl.PermitData.Numbers))
         {
            Paste = true;
            CalcDgvDataAddresNum();
            for(int i=0;i< dgvData.Rows.Count-1;i++)
            {
               dgvData.CurrentCell = dgvData.Rows[i].Cells["VALUE"];
               CalcDgvDataBytesValue();
            }
            GoWrite();
         }
      }

      private void GoWrite()
      {
         btnWrite.Enabled   = true;
         btnRead.Enabled    = true;
         cmbOrder.Enabled   = false;
         ckbHexViw.Enabled  = false;
         ckbNotZero.Enabled = false;
         //ckbFI.Enabled      = false;
         btnRead.Text       = "Отмена";
      }

      private void GoEdit()
      {
         dgvBits.Enabled = true;
         btnWrite.Enabled   = false;
         btnRead.Enabled    = true;
         cmbOrder.Enabled   = true;
         ckbHexViw.Enabled  = true;
         ckbNotZero.Enabled = true;
         ckbFI.Enabled      = true;
         btnRead.Text       = "Чтение";
      }

      private void txbOrder_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Enter)
         {
            ReadData(null);
            GoEdit();
         }
      }

      private void dgvData_KeyPress(object sender, KeyPressEventArgs e)
      {
         if (e.KeyChar == 22 &&
            ((DataGridView)sender).CurrentCell.ColumnIndex == 6)
         {
            Clip(sender);
         }
      }

      private delegate int Calc(int val);
      int Increase(int val) => val+1;
      int Decrease(int val) => val-1;
      private byte[] GetBytesFromRowBits(DataGridViewRow row, int CountByte, int OffSet=0, bool GoLeft=false)
      {
         try
         
         {
            Calc calc;
            if (GoLeft) calc = Decrease; else calc = Increase;
            byte[] result = new byte[CountByte];

            for (int b = 0; b < CountByte; b++)
            {
               for (int i = 0; i < 8; i++)
               {
                  result[b] = WorkMisc.Misc.SetBit(result[b], i, (bool)row.Cells[OffSet].Value);
                  OffSet = calc(OffSet);
               
                  }
            }
            
            
            return result;
         }
         catch{ return null; }
      }

      private void dgvBits_CellClick(object sender, DataGridViewCellEventArgs e)
      {

         DataGridView table = ((DataGridView)sender);
         DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)table.CurrentCell;
         DataGridViewRow row = table.CurrentRow;

         cell.Value = !(bool)cell.Value;
               
         SetCurRowFromBytes(GetBytesFromRowBits(row,4,31,true),ref dgvData,1,false,ckbHexViw.Checked);
         CalcDgvDataBytesValue(true);
         SendKeys.Send("{ENTER}");
         GoWrite();
      }

      private void ckbFI_CheckedChanged(object sender, EventArgs e)
      {
         CalcDgvDataFloatOrInt();
      }

      private void cmbOrder_KeyPress(object sender, KeyPressEventArgs e)
      {
         if (e.KeyChar == '\r')
            ReadData(null);
      }

      private void timer1_Tick(object sender, EventArgs e)
      {
         if (txbPath.ForeColor != Color.DarkGreen)
            txbPath.ForeColor = Color.DarkGreen;
         else
            txbPath.ForeColor = Color.Black;

         if (txbPath.Text != "Нажми для выбора HEX-файла")
         {
            txbPath.ForeColor = Color.DarkGreen;
         }
         if ( btnWrite.Enabled )
         {
            if ( btnWrite.ForeColor != Color.DarkGreen)
               btnWrite.ForeColor = Color.DarkGreen;
            else
               btnWrite.ForeColor = Color.Black;
         }
         else
            btnWrite.ForeColor = Color.Black;
         txbPath.Invalidate();
      }

      private void txbPath_MouseClick_1(object sender, AxMicrosoft.Vbe.Interop.Forms.MdcTextEvents_MouseDownEvent e)
      {
         OpenFileDialog OPF = new OpenFileDialog();
         if (OPF.ShowDialog() == DialogResult.OK)
         {
            txbPath.Text = OPF.FileName;
            ReadData(null);
            GoEdit();
         }
      }

      private void dgvData_SelectionChanged(object sender, EventArgs e)
      {
         if(!ProgChange)
            BufferData = ((DataGridView)sender).CurrentCell.Value.ToString();
      }

      private void txbPath_Enter(object sender, EventArgs e)
      {

      }
   }
}
