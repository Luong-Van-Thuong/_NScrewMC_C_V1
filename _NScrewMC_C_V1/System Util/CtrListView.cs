using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public class CtrListView
    {
        public  DataTable TB_Error = new DataTable();
        public  DataTable TB_ErrorCount = new DataTable();
        public void InitListview(ListView _listview, string[] _Name, int[] _with, int _Top, int _left, Size _size, float _fontSize)
        {
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
        public void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(SystemBrushes.Menu, e.Bounds);
            e.Graphics.DrawRectangle(SystemPens.GradientInactiveCaption,
                new Rectangle(e.Bounds.X, 0, e.Bounds.Width, e.Bounds.Height));

            string text = (sender as ListView).Columns[e.ColumnIndex].Text;
            TextFormatFlags cFlag = TextFormatFlags.HorizontalCenter;
            //  | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(e.Graphics, text, (sender as ListView).Font, e.Bounds, Color.Black, cFlag);
        }

        public void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        public void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex == 0) // Or whatever the column in question is.
            {
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, new SolidBrush(e.SubItem.ForeColor), e.Bounds, format);
                e.DrawDefault = false;
            }
            else
            {
                e.DrawDefault = true;
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