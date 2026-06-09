using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public class CtrListViewScrew
    {
        Image img;
        public  DataTable TB_Error = new DataTable();
        public  DataTable TB_ErrorCount = new DataTable();
        public ListView ListViewInstall  = null;
        public int _RowsHeight = 10;
        public bool[] _ScrewRun = new bool[20];
        public bool[] _ScrewRetry = new bool[20];
        public int m_iCuIndex = 0;
        private int _index;
        public void InitListview(int index, ListView _listview, string[] _Name, int[] _with, int _Top, int _left, Size _size, 
                                float _fontSize, FontStyle _Style, int _Height)
        {
            _index = index;
            ListViewInstall = _listview;
            _listview.Columns.Clear();
            _listview.Top = _Top;
            _listview.Left = _left;
            _listview.Size = _size;
            

            _listview.Font = new Font("Microsoft Sans Serif", _fontSize, _Style, GraphicsUnit.Point);
            _RowsHeight = _Height;

            _listview.OwnerDraw = true;
            _listview.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(listView1_DrawColumnHeader);
            _listview.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(listView1_DrawItem);
            _listview.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(listView1_DrawSubItem);

            _listview.View = View.Details;
            _listview.GridLines = true;

            _listview.FullRowSelect = true;
            _listview.Dock = DockStyle.Fill;

            _listview.BorderStyle = BorderStyle.Fixed3D;
            for (int i = 0; i < _Name.Length; i++)
            {
                _listview.Columns.Add(_Name[i], _with[i], HorizontalAlignment.Center);
            }

        }
        public void InitListview(ListView _listview, string[] _Name, int[] _with, int _Top, int _left, Size _size, float _fontSize)
        {
            ListViewInstall = _listview;
            _listview.Columns.Clear();
            _listview.Top = _Top;
            _listview.Left = _left;
            _listview.Size = _size;
            

            _listview.Font = new System.Drawing.Font("Aria", _fontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

            _listview.OwnerDraw = true;
            _listview.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(listView1_DrawColumnHeader);
            _listview.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(listView1_DrawItem);
            _listview.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(listView1_DrawSubItem);

            _listview.View = View.Details;
            _listview.GridLines = true;
            _listview.FullRowSelect = true;
            _listview.BorderStyle = BorderStyle.Fixed3D;
           

            for (int i = 0; i < _Name.Length; i++)
            {
                _listview.Columns.Add(_Name[i], _with[i], HorizontalAlignment.Left);
            }
            _listview.BackColor = Color.White;
        }
        #region //Load Data to ListView
        public void CoutErrorLog(ListView _listview, DataTable _Table)
        {
            //Count Data Fix . Don't change any thing here
            lock (MSystem.Lock_DisplayLog) {
                var query = _Table.AsEnumerable()
                            .GroupBy(r => new { Name = r.Field<string>("CONTENS"), Action = r.Field<string>("NUMERROR") })
                            .Select(grp => new
                            {
                                Count = grp.Count(),
                                Action = grp.Key.Action,
                                Name = grp.Key.Name
                            });
                
                _listview.Items.Clear();

                _listview.BeginUpdate();
                foreach (var item in query)
                {
                    string[] _tmpData = { $"{item.Count}", $"{item.Action}", $"{item.Name}" };
                    _listview.Items.Insert(0, new ListViewItem(_tmpData));
                }
                _listview.EndUpdate();
            }
        }
        public void LoadListData(ListView _listview, DataTable _Table)
        {
            _listview.Items.Clear();
            lock (MSystem.Lock_DisplayLog) {
                if (_Table.Rows.Count > 0)
                {
                    _listview.BeginUpdate();
                    foreach (DataRow dr in _Table.Rows)
                    {
                        string[] _tmpData = string.Join(",", dr.ItemArray).Split(',').ToArray();
                        _listview.Items.Insert(0, new ListViewItem(_tmpData));
                    }
                    _listview.EndUpdate();
                }
            }
                
        }
        public void InsertRowTolist(ListView _listview, string[] _dataRowList)
        {
            _dataRowList[0] = _listview.Items.Count.ToString();
            ListViewItem _lvItem = new ListViewItem(_dataRowList);
            _listview.Items.Add(_lvItem);
        }
        public void InsertRowScrew(ListView _listview, string[] _dataRowList)
        {
            ListViewItem _lvItem = new ListViewItem(_dataRowList);
            _listview.Items.Add(_lvItem);
        }
        public void InsertRow(ListView _listview, string[] _dataRowList)
        {
            _listview.Items.Insert(0, new ListViewItem(_dataRowList));
        }
        public void InsertRowPass(ListView _listview, string[] _dataRowList)
        {
            string[] _tmp = new string[_dataRowList.Length + 1];
            _tmp[0] = (_listview.Items.Count + 1).ToString();
            Array.Copy(_dataRowList, 0, _tmp, 1, _dataRowList.Length);
            ListViewItem _lvItem = new ListViewItem(_tmp);
            _listview.Items.Add(_lvItem);
        }
        #endregion

        #region // Re Draw listview
        public void SelectIndex(int _idex = 0) 
        {
            if (ListViewInstall.Items.Count > 0 && _idex < ListViewInstall.Items.Count)
            {
                ListViewInstall.SelectedItems.Clear();
                ListViewInstall.Items[_idex].Selected = true;
                ListViewInstall.EnsureVisible(_idex);
                ListViewInstall.Focus();
            }
        }

        public void LoadDataScrew(int m_idx) 
        {
            ListViewInstall.Items.Clear();
            if (InforTeaching.Instance.P_Screw[m_idx] != null && InforTeaching.Instance.P_Screw[m_idx].Count > 0)
            {
                double posX;
                double posY;
                double posZ;
                for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
                {
                    posX = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X;
                    posY = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y;
                    posZ = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Z;
                    if (InforManager.Instance.IsVisionPoint == true)
                    {
                        posX += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                        posY += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                        //posZ += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_VISION].Z;
                        posX -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                        posY -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                        //posZ -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_SCREW].Z;
                        posZ = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.VisionZ;
                    }

                    InforTeaching.Instance.P_Screw[m_idx][i].P_Name = $"P{i+1}";
                    var itemsL = new ListViewItem(new string[] {
                                      $"{InforTeaching.Instance.P_Screw[m_idx][i].P_Name}",
                                      $"{posX.ToString("f03")}",
                                      $"{posY.ToString("f03")}",
                                      $"{posZ.ToString("f03")}",
                                      $" ",        // Cột 5: Skip - sẽ vẽ checkbox
                                      $" ",        // Cột 6: Test Result - sẽ vẽ checkbox
                                      $"{InforTeaching.Instance.P_Screw[m_idx][i].Channel}", //Cot 7
                                      $" ",        // Cột 8: Vision - sẽ vẽ checkbox
                                      $" ",        // Cột 9: Retry - sẽ vẽ checkbox
                                      $"{InforTeaching.Instance.P_Screw[m_idx][i].FasNumbers}" // Cột 10
});
                    ListViewInstall.Items.Add(itemsL);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    PointScrew _tmpPoint = new PointScrew();
                    _tmpPoint.P_Name = $"P{i+1}";
                    _tmpPoint.PScrew.PointName = _tmpPoint.P_Name;
                    _tmpPoint.PScrew.X = 0.00;
                    _tmpPoint.PScrew.Y = 0.00;
                    _tmpPoint.PScrew.Z = 0.00;
                    _tmpPoint.Skip = false;
                    _tmpPoint.Vision = false;
                    _tmpPoint.Retry = false;
                    //_tmpPoint.Channel = 1;
                    _tmpPoint.FasNumbers = 1;
                    InforTeaching.Instance.P_Screw[m_idx].Add(_tmpPoint);
                }
                for (int i = 0; i < 5; i++)
                {
                    var itemsL = new ListViewItem(new string[] { 
                                                                 $"{InforTeaching.Instance.P_Screw[m_idx][i].P_Name}",
                                                                 $"{InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X.ToString("f03")}",
                                                                 $"{InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y.ToString("f03")}",
                                                                 $"{InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Z.ToString("f03")}",
                                                                 $" ",        // Cột 5: Skip - sẽ vẽ checkbox
                                                                 $" ",        // Cột 6: Test Result - sẽ vẽ checkbox
                                                                 $"{InforTeaching.Instance.P_Screw[m_idx][i].Channel}",// cot 7
                                                                 $" ",        // Cột 8: Vision - sẽ vẽ checkbox
                                                                 $" ",        // Cột 9: Retry - sẽ vẽ checkbox
                                                                 $"{InforTeaching.Instance.P_Screw[m_idx][i].FasNumbers}"    // Cột 10
                                                                });
                    ListViewInstall.Items.Add(itemsL);
                }
                InforTeaching.Instance.SaveSettings();
            }
        }
        public void UpdateListViewCell(int rowIndex, int columnIndex, string newValue)
        {
            if (rowIndex >= 0 && rowIndex < ListViewInstall.Items.Count)
            {
                ListViewItem item = ListViewInstall.Items[rowIndex];
                if (columnIndex >= 0 && columnIndex < item.SubItems.Count)
                {
                    item.SubItems[columnIndex].Text = newValue;
                }
                else
                {
                    //MessageBox.Show("Column index out of range.");
                }
            }
            else
            {
                //MessageBox.Show("Row index out of range.");
            }
        }
        public string GetOldvalue(int rowIndex, int columnIndex)
        {
            if (rowIndex >= 0 && rowIndex < ListViewInstall.Items.Count)
            {
                ListViewItem item = ListViewInstall.Items[rowIndex];
                if (columnIndex >= 0 && columnIndex < item.SubItems.Count)
                {
                    return item.SubItems[columnIndex].Text;
                }
                else
                {
                    //MessageBox.Show("Column index out of range.");
                }
            }
            else
            {
                //MessageBox.Show("Row index out of range.");
            }
            return null;
        }
        public void DisplayImageInCell(int itemIndex, int subItemIndex, string imageKey)
        {
            if (itemIndex >= 0 && itemIndex < ListViewInstall.Items.Count)
            {
                var item = ListViewInstall.Items[itemIndex];
                if (subItemIndex >= 0 && subItemIndex < item.SubItems.Count)
                {
                    item.SubItems[subItemIndex].Tag = imageKey;
                    ListViewInstall.Invalidate(item.SubItems[subItemIndex].Bounds);
                }
            }
        }
        public void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
           // e.DrawDefault = true;
            e.Graphics.FillRectangle(SystemBrushes.Menu, e.Bounds);
            e.Graphics.DrawRectangle(SystemPens.ActiveBorder,
                new Rectangle(e.Bounds.X, 0, e.Bounds.Width, e.Bounds.Height));

            string text = (sender as ListView).Columns[e.ColumnIndex].Text;
            TextFormatFlags cFlag = TextFormatFlags.HorizontalCenter;
            TextRenderer.DrawText(e.Graphics, text, (sender as ListView).Font, e.Bounds, Color.Black, cFlag);

            //using (Pen gridLinePen = new Pen(Color.Black)) // Change to your desired color
            //{
            //    // Draw vertical grid line for column headers
            //    e.Graphics.DrawLine(gridLinePen, e.Bounds.Right - 1, e.Bounds.Top, e.Bounds.Right - 1, e.Bounds.Bottom);
            //}
        }

        public void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            
            e.Item.UseItemStyleForSubItems = true;
            int imageOffset = 0;
            Rectangle rect = e.Item.Bounds;
            bool drawImage = !(e.Item.ImageList is null);
            Color itemColor = Color.FromName(e.Item.Text.Substring(e.Item.Text.LastIndexOf(" ") + 1));

            using (var format = new StringFormat(StringFormatFlags.FitBlackBox))
            {
                format.LineAlignment = StringAlignment.Center;

                if (e.Item.Selected)
                {
                    using (var bkgrBrush = new SolidBrush(Color.Gainsboro))
                    //using (var foreBrush = new SolidBrush(Color.Blue))
                    {
                        e.Graphics.FillRectangle(bkgrBrush, rect);
                        //e.Graphics.DrawString(e.Item.Text, e.Item.Font, foreBrush, rect, format);
                    }
                    //e.DrawFocusRectangle();
                    //e.Item.BackColor = Color.Red;
                    if (drawImage)
                    {
                        imageOffset = e.Item.ImageList.ImageSize.Width + 1;
                        var img = e.Item.ImageList.Images["ImageOn"];
                        int x = e.Bounds.X;
                        int y = e.Bounds.Top + (e.Bounds.Height - img.Height) / 2 - 1;
                        e.Graphics.DrawImage(img, x, y);
                    }
                }
                else
                {
                    using (var foreBrush = new SolidBrush(itemColor))
                    {
                        e.Graphics.DrawString(e.Item.Text, e.Item.Font, foreBrush, rect, format);
                    }
                }
            }
        }

        public void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.Item.Index > ListViewInstall.Items.Count - 1) return;
            int imageOffset = 0;
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            Font _newFont = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point);
            Color itemColor = Color.FromName(e.Item.Text.Substring(e.Item.Text.LastIndexOf(" ") + 1));
            Rectangle bounds = e.Bounds;
            bounds.Height = _RowsHeight;
            e.Graphics.DrawString(e.SubItem.Text, _newFont, new SolidBrush(Color.Black), e.Bounds, format);

            if (e.ColumnIndex == 4)
            {
                if (InforTeaching.Instance.P_Screw[_index][e.Item.Index].Skip)
                    img = e.Item.ImageList.Images["Dis"];
                else
                    img = e.Item.ImageList.Images["ImageOn"];

                imageOffset = e.Item.ImageList.ImageSize.Width + 1;
                int x = e.Bounds.Left + (e.Bounds.Width - img.Width) / 2;
                int y = e.Bounds.Top + (e.Bounds.Height - img.Height) / 2 - 1;
                e.Graphics.DrawImage(img, x, y);
            }
            else if (e.ColumnIndex == 5) 
            {
                if (MSystem.m_ScrewTestResult[e.Item.Index] == 0)
                    img = e.Item.ImageList.Images["Dis"];
                else if (MSystem.m_ScrewTestResult[e.Item.Index] == 1)
                    img = e.Item.ImageList.Images["ImageOn"];
                else if (MSystem.m_ScrewTestResult[e.Item.Index] == 2)
                    img = e.Item.ImageList.Images["ImageOff"];

                imageOffset = e.Item.ImageList.ImageSize.Width + 1;
                int x = e.Bounds.Left + (e.Bounds.Width - img.Width) / 2;
                int y = e.Bounds.Top + (e.Bounds.Height - img.Height) / 2 - 1;
                e.Graphics.DrawImage(img, x, y);
            }
            else if (e.ColumnIndex == 7)
            {
                if (InforManager.Instance.AutoVisionAlignMode != 0)
                {
                    if (InforTeaching.Instance.P_Screw[_index][e.Item.Index].Vision==false)
                        img = e.Item.ImageList.Images["Dis"];
                    else
                        img = e.Item.ImageList.Images["ImageOn"];
                    InforTeaching.Instance.SaveSettings();
                }
                else
                {
                    img = e.Item.ImageList.Images["Dis"];
                }

                imageOffset = e.Item.ImageList.ImageSize.Width + 1;
                int x = e.Bounds.Left + (e.Bounds.Width - img.Width) / 2;
                int y = e.Bounds.Top + (e.Bounds.Height - img.Height) / 2 - 1;
                e.Graphics.DrawImage(img, x, y);
            }
            else if (e.ColumnIndex == 8) 
            {
                if (InforTeaching.Instance.P_Screw[_index][e.Item.Index].Retry==false)
                    img = e.Item.ImageList.Images["Dis"];
                else
                    img = e.Item.ImageList.Images["ImageOn"];


                InforTeaching.Instance.SaveSettings();

                imageOffset = e.Item.ImageList.ImageSize.Width + 1;
                int x = e.Bounds.Left + (e.Bounds.Width - img.Width) / 2;
                int y = e.Bounds.Top + (e.Bounds.Height - img.Height) / 2 - 1;
                e.Graphics.DrawImage(img, x, y);
            }
        }
        #endregion

        #region // Re draw groupbox
        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                // Clear text and border
                g.Clear(box.BackColor);

                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Drawing Border
                //Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                //Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }
        public void Gr_Block_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Black, Color.Blue);
        }
        #endregion

        #region //redraw combobx
        public void cbxDesign_DrawItem(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0)
                {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    // Draw the string
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }
        #endregion
    }
}