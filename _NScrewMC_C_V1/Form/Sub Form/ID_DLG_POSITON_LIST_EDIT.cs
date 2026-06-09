using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
//using System.Windows.Controls;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class PositionListEdit : Form
    {
        #region ini file Read Write
        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileString(string section,
                                                           string key,
                                                           string defaultValue, //키값이 없을 때의 기본 값
                                                           StringBuilder returnedString,
                                                           uint size,
                                                           string filePath);

        [DllImport("kernel32.dll")]
        private static extern bool WritePrivateProfileString(string section,
                                                             string key,
                                                             string value,
                                                             string filePath);
        #endregion
        private HashSet<int> previouslySelectedRows = new HashSet<int>();
        PointScrew[] _PointNew;
        
        private double[,] _X;
        private double[,] _Y;
        private double[,] _Z;
        private double[,] _VisionZ;
        private bool[,] _Retry;
        private bool[,] _Skip;
        private int[,] _FasNumbers;
        private int[,] _Light;
        private int[,] _Channel;
        private int[,] _Status;
        private bool[,] _Vision;

        private int AddselectedIndex;
        private bool[] overlapAddFlag; // 동일좌표 중복 방지 플래그
        private int[] newNormber;
        private bool enableLeft = true;
        private bool enableRight = true;
        private int LR = 0; // Left or Right (0: Left, 1: Right)
        public PositionListEdit()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            LV_POSITION_NEW.Clear();
            LV_POSITION_NOW.Clear();

            //LV_POSITION_NOW.DrawColumnHeader += LV_POSITION_DrawColumnHeader;
            //LV_POSITION_NOW.DrawItem += LV_POSITION_DrawItem;
            //LV_POSITION_NOW.DrawSubItem += LV_POSITION_DrawSubItem;
            LV_POSITION_NOW.MouseClick += LV_POSITION_NOW_MouseClick;

            //LV_POSITION_NEW.DrawColumnHeader += LV_POSITION_DrawColumnHeader;
            //LV_POSITION_NEW.DrawItem += LV_POSITION_DrawItem;
            //LV_POSITION_NEW.DrawSubItem += LV_POSITION_DrawSubItem;

            _PointNew = new PointScrew[InforTeaching.Instance.P_Screw[LR].Count];
            _X = new double[2,InforTeaching.Instance.P_Screw[LR].Count];
            _Y = new double[2, InforTeaching.Instance.P_Screw[LR].Count];
            _Z = new double[2, InforTeaching.Instance.P_Screw[LR].Count];
            _VisionZ = new double[2, InforTeaching.Instance.P_Screw[LR].Count];
            _Retry = new bool[2, InforTeaching.Instance.P_Screw[LR].Count];
            _Skip = new bool[2, InforTeaching.Instance.P_Screw[LR].Count];
            _Light = new int[2, InforTeaching.Instance.P_Screw[LR].Count];
            _FasNumbers = new int[2, InforTeaching.Instance.P_Screw[LR].Count];
            _Channel = new int[2, InforTeaching.Instance.P_Screw[LR].Count];
            _Status = new int[2, InforTeaching.Instance.P_Screw[LR].Count];
            InitPositionList(LR);
        }
        private void InitPositionList(int UnitIndex)
        {
            LV_POSITION_NOW.View = View.Details;
            LV_POSITION_NEW.View = View.Details;

            LV_POSITION_NOW.FullRowSelect = true;
            LV_POSITION_NEW.FullRowSelect = true;
            LV_POSITION_NOW.OwnerDraw = false;
            LV_POSITION_NEW.OwnerDraw = false;

            LV_POSITION_NEW.Columns.Add("Old", 50, HorizontalAlignment.Center);
            LV_POSITION_NEW.Columns.Add("New", 60, HorizontalAlignment.Center);
            //LV_POSITION_NEW.Columns.Add("X", 100, HorizontalAlignment.Center);
            //LV_POSITION_NEW.Columns.Add("Y", 100, HorizontalAlignment.Center);
            //LV_POSITION_NEW.Columns.Add("Z", 100, HorizontalAlignment.Center);
            LV_POSITION_NOW.Columns.Add("No", 50, HorizontalAlignment.Center);
            //LV_POSITION_NOW.Columns.Add("X", 100, HorizontalAlignment.Center);
            //LV_POSITION_NOW.Columns.Add("Y", 100, HorizontalAlignment.Center);
            //LV_POSITION_NOW.Columns.Add("Z", 100, HorizontalAlignment.Center);
            if (InforTeaching.Instance.P_Screw == null || InforTeaching.Instance.P_Screw[UnitIndex].Count == 0)
            {
                MessageBox.Show("P_Screw 데이터가 비어 있습니다.");
                return;

            }

            ListViewItem LvItem;
            
            for (int i=0; i < InforTeaching.Instance.P_Screw[UnitIndex].Count; i++)
            {
                string item = $"P{i+1}";
                LvItem = new ListViewItem(item);
                LvItem.SubItems.Add(InforTeaching.Instance.P_Screw[UnitIndex][i].PScrew.X.ToString());
                LvItem.SubItems.Add(InforTeaching.Instance.P_Screw[UnitIndex][i].PScrew.Y.ToString());
                LvItem.SubItems.Add(InforTeaching.Instance.P_Screw[UnitIndex][i].PScrew.Z.ToString());
                LV_POSITION_NOW.Items.Add(LvItem);
            }
            LV_POSITION_NOW.Refresh();

            overlapAddFlag = new bool[InforTeaching.Instance.P_Screw[LR].Count];

            #region // 기능확인 용
            //string filePath = Config.ModelSavePath + InforManager.Instance.ModelName + ".ini";
            //string wtdata;
            //for (int j = 0; j < InforTeaching.Instance.P_Screw[LR].Count; j++)
            //{
            //    wtdata = $"X:{InforTeaching.Instance.P_Screw[LR][j].PScrew.X}, " +
            //        $"Y:{InforTeaching.Instance.P_Screw[LR][j].PScrew.Y}, " +
            //        $"Z:{InforTeaching.Instance.P_Screw[LR][j].PScrew.Z}, " +
            //        $"T:{InforTeaching.Instance.P_Screw[LR][j].PScrew.T}, " +
            //        $"Retry:{InforTeaching.Instance.P_Screw[LR][j].Retry}" +
            //        $"Skip:{InforTeaching.Instance.P_Screw[LR][j].Skip}" +
            //        $"Light:{InforTeaching.Instance.P_Screw[LR][j].Light}" +
            //        $"Ch:{InforTeaching.Instance.P_Screw[LR][j].Channel}" +
            //        $"Status:{InforTeaching.Instance.P_Screw[LR][j].status}";
            //    WritePrivateProfileString("NOW POSITION", $"P{j + 1}", wtdata, filePath);
            //}
            #endregion
        }
        private void buttonNewListAdd_Click(object sender, EventArgs e)
        {
            if (overlapAddFlag[AddselectedIndex] == true)
                return;
            #region // new List로 추가한 항목을 컬러변경
            // 선택된 행 확인
            if (LV_POSITION_NOW.SelectedItems.Count > 0)
            {
                foreach (ListViewItem selectedItem in LV_POSITION_NOW.SelectedItems)
                {
                    // UseItemStyleForSubItems를 false로 설정하여 개별 스타일 적용
                    selectedItem.UseItemStyleForSubItems = false;

                    // 행 배경색 변경
                    selectedItem.BackColor = Color.LightCoral;

                    // 텍스트 색상 변경
                    foreach (ListViewItem.ListViewSubItem subItem in selectedItem.SubItems)
                    {
                        subItem.ForeColor = Color.White;
                    }
                }
            }
            else
            {
                MessageBox.Show("행을 선택하세요!", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            #endregion

            ListViewItem LvItem;
            int count = LV_POSITION_NEW.Items.Count;
            string item = InforTeaching.Instance.P_Screw[LR][AddselectedIndex].P_Name;
            LvItem = new ListViewItem(item);
            LvItem.SubItems.Add($"P{count + 1}");
            LvItem.SubItems.Add(InforTeaching.Instance.P_Screw[LR][AddselectedIndex].PScrew.X.ToString());
            LvItem.SubItems.Add(InforTeaching.Instance.P_Screw[LR][AddselectedIndex].PScrew.Y.ToString());
            LvItem.SubItems.Add(InforTeaching.Instance.P_Screw[LR][AddselectedIndex].PScrew.Z.ToString());
            LV_POSITION_NEW.Items.Add(LvItem);

            LV_POSITION_NEW.Refresh();

            overlapAddFlag[AddselectedIndex] = true;

            if (enableLeft == true)
            {
                _X[MSystem.eLEFT,LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eLEFT][AddselectedIndex].PScrew.X;
                _Y[MSystem.eLEFT,LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eLEFT][AddselectedIndex].PScrew.Y;
                _Z[MSystem.eLEFT,LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eLEFT][AddselectedIndex].PScrew.Z;
                _VisionZ[MSystem.eLEFT,LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eLEFT][AddselectedIndex].PScrew.VisionZ;
                _Channel[MSystem.eLEFT, LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eLEFT][AddselectedIndex].Channel;
                _Retry[MSystem.eLEFT,LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eLEFT][AddselectedIndex].Retry;
                _Light[MSystem.eLEFT,LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eLEFT][AddselectedIndex].Light;
                _FasNumbers[MSystem.eLEFT,LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eLEFT][AddselectedIndex].FasNumbers;
                _Skip[MSystem.eLEFT,LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eLEFT][AddselectedIndex].Skip;
                _Status[MSystem.eLEFT,LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eLEFT][AddselectedIndex].status;
            }
            if (enableRight == true)
            {
                _X[MSystem.eRIGHT, LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eRIGHT][AddselectedIndex].PScrew.X;
                _Y[MSystem.eRIGHT, LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eRIGHT][AddselectedIndex].PScrew.Y;
                _Z[MSystem.eRIGHT, LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eRIGHT][AddselectedIndex].PScrew.Z;
                _VisionZ[MSystem.eRIGHT, LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eRIGHT][AddselectedIndex].PScrew.VisionZ;
                _Channel[MSystem.eRIGHT, LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eRIGHT][AddselectedIndex].Channel;
                _Retry[MSystem.eRIGHT, LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eRIGHT][AddselectedIndex].Retry;
                _Light[MSystem.eRIGHT, LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eRIGHT][AddselectedIndex].Light;
                _FasNumbers[MSystem.eRIGHT, LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eRIGHT][AddselectedIndex].FasNumbers;
                _Skip[MSystem.eRIGHT, LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eRIGHT][AddselectedIndex].Skip;
                _Status[MSystem.eRIGHT, LV_POSITION_NEW.Items.Count - 1] = InforTeaching.Instance.P_Screw[MSystem.eRIGHT][AddselectedIndex].status;
            }
            
        }
        #region
        //private void LV_POSITION_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        //{
        //    // 배경색 설정
        //    using (Brush headerBackgroundBrush = new SolidBrush(Color.DarkGray))
        //    {
        //        e.Graphics.FillRectangle(headerBackgroundBrush, e.Bounds);
        //    }

        //    // 텍스트 스타일 설정
        //    using (Font headerFont = new Font("Arial", 10, FontStyle.Bold))
        //    using (Brush textBrush = new SolidBrush(Color.White))
        //    {
        //        StringFormat stringFormat = new StringFormat
        //        {
        //            Alignment = StringAlignment.Center, // 텍스트 중앙 정렬
        //            LineAlignment = StringAlignment.Center
        //        };
        //        e.Graphics.DrawString(e.Header.Text, headerFont, textBrush, e.Bounds, stringFormat);
        //    }

        //    // 테두리 그리기
        //    ControlPaint.DrawBorder(e.Graphics, e.Bounds, Color.Black, ButtonBorderStyle.Solid);
        //}

        //private void LV_POSITION_DrawItem(object sender, DrawListViewItemEventArgs e)
        //{
        //    // 이전에 선택된 행의 배경색 변경
        //    //if (previouslySelectedRows.Contains(e.ItemIndex))
        //    //{
        //    //    e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);
        //    //}
        //    //else if (e.Item.Selected) // 현재 선택된 행
        //    //{
        //    //    e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
        //    //}
        //    //else // 기본 배경색
        //    //{
        //    //    e.Graphics.FillRectangle(Brushes.White, e.Bounds);
        //    //}

        //    // 텍스트 그리기
        //   e.DrawText();
        //   // e.DrawDefault = true;
        //}

        //private void LV_POSITION_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        //{
        //    // 선택된 SubItem 강조
        //    if (e.Item.Selected && e.ColumnIndex == 1) // 예: 두 번째 열 강조
        //    {
        //        e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
        //        using (Font subItemFont = new Font("Arial", 9, FontStyle.Bold))
        //        {
        //            e.Graphics.DrawString(e.SubItem.Text, subItemFont, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
        //        }
        //    }
        //    else
        //    {
        //        e.Graphics.FillRectangle(Brushes.White, e.Bounds);
        //        using (Font subItemFont = new Font("Arial", 9, FontStyle.Regular))
        //        {
        //            e.Graphics.DrawString(e.SubItem.Text, subItemFont, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
        //        }
        //    }
        //}
        #endregion
        private void LV_POSITION_NOW_MouseClick(object sender, MouseEventArgs e)
        {
            if (LV_POSITION_NOW.SelectedItems.Count > 0)
            {
                AddselectedIndex = LV_POSITION_NOW.SelectedItems[0].Index;

                // 선택된 행을 추적
                if (!previouslySelectedRows.Contains(AddselectedIndex))
                {
                    previouslySelectedRows.Add(AddselectedIndex);
                }

                // ListView 다시 그리기
                LV_POSITION_NOW.Invalidate();
            }
        }
        private void InitializeComponent()
        {
            this.LV_POSITION_NOW = new System.Windows.Forms.ListView();
            this.LV_POSITION_NEW = new System.Windows.Forms.ListView();
            this.buttonNewListAdd = new System.Windows.Forms.Button();
            this.buttonListReset = new System.Windows.Forms.Button();
            this.buttonListSave = new System.Windows.Forms.Button();
            this.buttonRight = new System.Windows.Forms.Button();
            this.buttonLeft = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textboxTitle = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LV_POSITION_NOW
            // 
            this.LV_POSITION_NOW.BackColor = System.Drawing.SystemColors.Window;
            this.LV_POSITION_NOW.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LV_POSITION_NOW.GridLines = true;
            this.LV_POSITION_NOW.HideSelection = false;
            this.LV_POSITION_NOW.Location = new System.Drawing.Point(9, 78);
            this.LV_POSITION_NOW.Name = "LV_POSITION_NOW";
            this.LV_POSITION_NOW.Size = new System.Drawing.Size(78, 448);
            this.LV_POSITION_NOW.TabIndex = 0;
            this.LV_POSITION_NOW.UseCompatibleStateImageBehavior = false;
            this.LV_POSITION_NOW.View = System.Windows.Forms.View.Details;
            // 
            // LV_POSITION_NEW
            // 
            this.LV_POSITION_NEW.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LV_POSITION_NEW.GridLines = true;
            this.LV_POSITION_NEW.HideSelection = false;
            this.LV_POSITION_NEW.Location = new System.Drawing.Point(168, 78);
            this.LV_POSITION_NEW.Name = "LV_POSITION_NEW";
            this.LV_POSITION_NEW.OwnerDraw = true;
            this.LV_POSITION_NEW.Size = new System.Drawing.Size(112, 448);
            this.LV_POSITION_NEW.TabIndex = 1;
            this.LV_POSITION_NEW.UseCompatibleStateImageBehavior = false;
            this.LV_POSITION_NEW.View = System.Windows.Forms.View.Details;
            // 
            // buttonNewListAdd
            // 
            this.buttonNewListAdd.Image = global::_NScrewMC_C_V1.Properties.Resources.jog_2_down;
            this.buttonNewListAdd.Location = new System.Drawing.Point(93, 258);
            this.buttonNewListAdd.Name = "buttonNewListAdd";
            this.buttonNewListAdd.Size = new System.Drawing.Size(69, 63);
            this.buttonNewListAdd.TabIndex = 2;
            this.buttonNewListAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonNewListAdd.UseVisualStyleBackColor = true;
            this.buttonNewListAdd.Click += new System.EventHandler(this.buttonNewListAdd_Click);
            // 
            // buttonListReset
            // 
            this.buttonListReset.BackColor = System.Drawing.Color.Moccasin;
            this.buttonListReset.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonListReset.Location = new System.Drawing.Point(299, 346);
            this.buttonListReset.Name = "buttonListReset";
            this.buttonListReset.Size = new System.Drawing.Size(205, 87);
            this.buttonListReset.TabIndex = 3;
            this.buttonListReset.Text = "New List Clear";
            this.buttonListReset.UseVisualStyleBackColor = false;
            this.buttonListReset.Click += new System.EventHandler(this.buttonListReset_Click);
            // 
            // buttonListSave
            // 
            this.buttonListSave.BackColor = System.Drawing.Color.LightGreen;
            this.buttonListSave.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonListSave.Location = new System.Drawing.Point(299, 439);
            this.buttonListSave.Name = "buttonListSave";
            this.buttonListSave.Size = new System.Drawing.Size(205, 87);
            this.buttonListSave.TabIndex = 4;
            this.buttonListSave.Text = "Save";
            this.buttonListSave.UseVisualStyleBackColor = false;
            this.buttonListSave.Click += new System.EventHandler(this.buttonListSave_Click);
            // 
            // buttonRight
            // 
            this.buttonRight.BackColor = System.Drawing.Color.Lime;
            this.buttonRight.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonRight.Location = new System.Drawing.Point(16, 85);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(184, 64);
            this.buttonRight.TabIndex = 6;
            this.buttonRight.Text = "Right";
            this.buttonRight.UseVisualStyleBackColor = false;
            this.buttonRight.Click += new System.EventHandler(this.buttonRight_Click);
            // 
            // buttonLeft
            // 
            this.buttonLeft.BackColor = System.Drawing.Color.Lime;
            this.buttonLeft.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonLeft.Location = new System.Drawing.Point(16, 15);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(184, 64);
            this.buttonLeft.TabIndex = 5;
            this.buttonLeft.Text = "Left";
            this.buttonLeft.UseVisualStyleBackColor = false;
            this.buttonLeft.Click += new System.EventHandler(this.buttonLeft_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.groupBox1.Controls.Add(this.buttonLeft);
            this.groupBox1.Controls.Add(this.buttonRight);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(295, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 170);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Unit Select";
            // 
            // textboxTitle
            // 
            this.textboxTitle.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.textboxTitle.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textboxTitle.Location = new System.Drawing.Point(9, 2);
            this.textboxTitle.Name = "textboxTitle";
            this.textboxTitle.Size = new System.Drawing.Size(493, 35);
            this.textboxTitle.TabIndex = 10;
            this.textboxTitle.Text = "LEFT / RIGHT ALL CHANGE";
            this.textboxTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox1.ForeColor = System.Drawing.Color.Black;
            this.textBox1.Location = new System.Drawing.Point(7, 53);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(82, 25);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "NOW LIST";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Info;
            this.textBox2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox2.ForeColor = System.Drawing.Color.Black;
            this.textBox2.Location = new System.Drawing.Point(167, 53);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(113, 25);
            this.textBox2.TabIndex = 12;
            this.textBox2.Text = "NEW LIST";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PositionListEdit
            // 
            this.ClientSize = new System.Drawing.Size(516, 538);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textboxTitle);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonListSave);
            this.Controls.Add(this.buttonListReset);
            this.Controls.Add(this.buttonNewListAdd);
            this.Controls.Add(this.LV_POSITION_NEW);
            this.Controls.Add(this.LV_POSITION_NOW);
            this.Name = "PositionListEdit";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ResetRowColors()
        {
            foreach (ListViewItem item in LV_POSITION_NOW.Items)
            {
                item.UseItemStyleForSubItems = true; // 기본 스타일로 복원
                item.BackColor = Color.White; // 기본 배경색
                item.ForeColor = Color.Black; // 기본 텍스트 색상
            }
        }

        private void buttonListReset_Click(object sender, EventArgs e)
        {
            if (MSystem.MyMsgMemo("Do You Want Reset New List?", "Reset New List", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
                return;
            buttonListReset.BackColor = Color.DarkGray;
            ResetRowColors();
            LV_POSITION_NEW.Clear();
            LV_POSITION_NOW.Clear();
            InitPositionList(MSystem.eLEFT);

            for (int i = 0; i < InforTeaching.Instance.P_Screw[MSystem.eLEFT].Count; i++)
            {
                overlapAddFlag[i] = false;
            }
            buttonListReset.BackColor = Color.Moccasin;
        }
        private void buttonListSave_Click(object sender, EventArgs e)
        {
            if (MSystem.MyMsgMemo("Do You Want Save New Position?", "Save New Position", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
                return;
            if (LV_POSITION_NEW.Items.Count == 0)
            {
                MessageBox.Show("There is no data to store.");
                return;
            }
            if (enableLeft == false && enableRight == false)
            {
                MessageBox.Show("Pleass Select left or right.");
                return;
            }
            buttonListSave.BackColor = Color.DarkGray;
            string[,] buffer = new string[LV_POSITION_NEW.Items.Count,9];
            for (int i = 0; i < LV_POSITION_NEW.Items.Count; i++)
            {
                for (int unitIndex = 0; unitIndex < 2; unitIndex++)
                {
                    if ((enableLeft == true && unitIndex == 0) || (enableRight == true && unitIndex == 1))
                    {
                        InforTeaching.Instance.P_Screw[unitIndex][i].P_Name = $"P{i + 1}";
                        InforTeaching.Instance.P_Screw[unitIndex][i].PScrew.X = _X[unitIndex, i];
                        InforTeaching.Instance.P_Screw[unitIndex][i].PScrew.Y = _Y[unitIndex, i];
                        InforTeaching.Instance.P_Screw[unitIndex][i].PScrew.Z = _Z[unitIndex, i];
                        InforTeaching.Instance.P_Screw[unitIndex][i].PScrew.VisionZ = _VisionZ[unitIndex, i];
                        InforTeaching.Instance.P_Screw[unitIndex][i].Skip = _Skip[unitIndex, i];
                        InforTeaching.Instance.P_Screw[unitIndex][i].Channel = _Channel[unitIndex, i];
                        InforTeaching.Instance.P_Screw[unitIndex][i].FasNumbers = _FasNumbers[unitIndex, i];
                        InforTeaching.Instance.P_Screw[unitIndex][i].Light = _Light[unitIndex, i];
                        InforTeaching.Instance.P_Screw[unitIndex][i].Retry = _Retry[unitIndex, i];
                        //InforTeaching.Instance.P_Screw[unitIndex][i].Vision = _Vision[unitIndex, i];
                    }
                }
            }
            InforTeaching.Instance.SaveSettings();
            InforTeaching.Instance.LoadSetting();
            buttonListSave.BackColor = Color.Lime;
            #region
            //string filePath = Config.ModelSavePath + InforManager.Instance.ModelName + ".ini";
            //string wtdata;
            //for (int j = 0; j < InforTeaching.Instance.P_Screw[LR].Count; j++)
            //{
            //    for (int unitIndex = 0; unitIndex < 2; unitIndex++)
            //    {
            //        if ((enableLeft == true && unitIndex == 0) || (enableRight == true && unitIndex == 1))
            //        {
            //            wtdata = $"X:{InforTeaching.Instance.P_Screw[unitIndex][j].PScrew.X}, " +
            //            $"Y:{InforTeaching.Instance.P_Screw[unitIndex][j].PScrew.Y}, " +
            //            $"Z:{InforTeaching.Instance.P_Screw[unitIndex][j].PScrew.Z}, " +
            //            $"T:{InforTeaching.Instance.P_Screw[unitIndex][j].PScrew.T}, " +
            //            $"Retry:{InforTeaching.Instance.P_Screw[unitIndex][j].Retry}" +
            //            $"Skip:{InforTeaching.Instance.P_Screw[unitIndex][j].Skip}" +
            //            $"Light:{InforTeaching.Instance.P_Screw[unitIndex][j].Light}" +
            //            $"Ch:{InforTeaching.Instance.P_Screw[unitIndex][j].Channel}" +
            //            $"Status:{InforTeaching.Instance.P_Screw[unitIndex][j].status}";
            //            WritePrivateProfileString($"NEW_POSITION_{unitIndex}", $"P{j + 1}", wtdata, filePath);
            //        }
            //    }
            //}
            #endregion
        }
        private void buttonLeft_Click(object sender, EventArgs e)
        {
            if (enableRight == false && enableLeft == true)
                return;
            if (enableLeft == false)
            {
                buttonLeft.BackColor = Color.Lime;
                enableLeft = true;
            }
            else
            {
                buttonLeft.BackColor = Color.Gray;
                enableLeft = false;
            }
            setTitleText();
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            if (enableRight == true && enableLeft == false)
                return;
            if (enableRight == false)
            {
                buttonRight.BackColor = Color.Lime;
                enableRight = true;
            }
            else
            {
                buttonRight.BackColor = Color.Gray;
                enableRight = false;
            }
            setTitleText();
        }
        private void setTitleText()
        {
            if (enableLeft == true && enableRight == true)
            {
                textboxTitle.Text = "LEFT / RIGHT ALL CHANGE";
            }
            else if (enableLeft == true && enableRight == false)
            {
                textboxTitle.Text = "LEFT ONLY CHANGE";
            }
            else if (enableLeft == false && enableRight == true)
            {
                textboxTitle.Text = "RIGHT ONLY CHANGE";
            }
            else
            {
                textboxTitle.Text = "NO UNIT SELECTED";
            }
        }
    }   
}
