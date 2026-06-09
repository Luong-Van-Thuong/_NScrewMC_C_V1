using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormAuto : Form
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern bool IsIconic(IntPtr hWnd);

        public bool _IsTorqueGraph = false;
        System.Windows.Forms.Timer GeimUpdate = new System.Windows.Forms.Timer();
        public FormAuto()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.Location = new Point(0, 0);
            this.Load += FormAuto_Load;

            GeimUpdate.Tick += new System.EventHandler(GeimUpdateData);
            GeimUpdate.Interval = 60000;
            GeimUpdate.Start();

            if (!MSystem.SIMULATION)
            {
                MSystem._mRun = new Thread(HantashUpdate);
                MSystem._mRun.IsBackground = true;
                MSystem._mRun.Start();
            }
       
            this.Dock = DockStyle.Fill;

            BT_START.SetPress = false;
            BT_STOP.SetPress = true;

            Thread.Sleep(100);
            ButtonEnable();
            if (InforManager.Instance.InspectionType != "VST"
                && InforManager.Instance.InspectionType != "Tablet Normal")
            {
                if (!MSystem.SIMULATION)
                {
                    MSystem.CheckVisionProgramRun();
                }
                ClearAlignImage(0);
                ClearAlignImage(1);
            }
            timer2.Interval = 1000; // Initialize,Origin status check
            timer2.Start();
        }

        private void FormAuto_Load(object sender, EventArgs e)
        {
            AutoItemUpdate();
        }
        public void AutoItemUpdate()
        {
            if ((InforManager.Instance.IsBarcodeUse))
            {
                if (LBL_RESULT_BCR_L.Visible == false)
                {
                    LBL_RESULT_BCR_L.ForeColor = Color.Black;
                    LBL_RESULT_BCR_L.Text = "";
                    LBL_RESULT_BCR_L.Visible = true;
                }
                if (LBL_RESULT_BCR_R.Visible == false)
                {
                    LBL_RESULT_BCR_R.ForeColor = Color.Black;
                    LBL_RESULT_BCR_R.Text = "";
                    LBL_RESULT_BCR_R.Visible = true;
                }
            }
            else
            {
                if (LBL_RESULT_BCR_L.Visible == true)
                {
                    LBL_RESULT_BCR_L.ForeColor = Color.Black;
                    LBL_RESULT_BCR_L.Text = "BCR NOT USE";
                    LBL_RESULT_BCR_L.Visible = false;
                }
                if (LBL_RESULT_BCR_R.Visible == true)
                {
                    LBL_RESULT_BCR_R.ForeColor = Color.Black;
                    LBL_RESULT_BCR_R.Text = "BCR NOT USE";
                    LBL_RESULT_BCR_R.Visible = false;
                }
            }
            if (InforManager.Instance.AutoVisionAlignMode == 0)
            {
                if (PB_L_0.Visible == true)
                    PB_L_0.Visible = false;
                if (PB_R_0.Visible == true)
                    PB_R_0.Visible = false;
                if (LB_LEFT_VISION_TITLE.Visible == true)
                    LB_LEFT_VISION_TITLE.Visible = false;
                if (LB_RIGHT_VISION_TITLE.Visible == true)
                    LB_RIGHT_VISION_TITLE.Visible = false;
                if (label_LeftVisionOffsetX.Visible == true)
                    label_LeftVisionOffsetX.Visible = false;
                if (label_RightVisionOffsetX.Visible == true)
                    label_RightVisionOffsetX.Visible = false;
                if (label_LeftVisionOffsetY.Visible == true)
                    label_LeftVisionOffsetY.Visible = false;
                if (label_RightVisionOffsetY.Visible == true)
                    label_RightVisionOffsetY.Visible = false;

                LEFT_VISION_STATUS.Visible = false;
                RIGHT_VISION_STATUS.Visible = false;
            }
            else
            {
                if (PB_L_0.Visible == false)
                    PB_L_0.Visible = true;
                if (PB_R_0.Visible == false)
                    PB_R_0.Visible = true;
                if (LB_LEFT_VISION_TITLE.Visible == false)
                    LB_LEFT_VISION_TITLE.Visible = true;
                if (LB_RIGHT_VISION_TITLE.Visible == false)
                    LB_RIGHT_VISION_TITLE.Visible = true;
                if (label_LeftVisionOffsetX.Visible == false)
                    label_LeftVisionOffsetX.Visible = true;
                if (label_RightVisionOffsetX.Visible == false)
                    label_RightVisionOffsetX.Visible = true;
                if (label_LeftVisionOffsetY.Visible == false)
                    label_LeftVisionOffsetY.Visible = true;
                if (label_RightVisionOffsetY.Visible == false)
                    label_RightVisionOffsetY.Visible = true;

                LEFT_VISION_STATUS.Visible = true;
                RIGHT_VISION_STATUS.Visible = true;
            }
        }
        private void GeimUpdateData(object sender, EventArgs e)
        {
            if (InforManager.Instance.GeimUse)
            {
                MSystem.m_pLogSave.AddTail(LogIndex.eLogGEIM_D1, "");
            }
        }
        private void FormAuto_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true)
            {
                timer1.Start();

                //UpdateAlignImage(0); // test
                //UpdateAlignImage(1); // test
            }
            else
                timer1.Stop();
        }

        public Bitmap bitmapfromfile(string filename)
        {
            var file = File.ReadAllBytes(filename);
            MemoryStream stream = new MemoryStream(file);
            Bitmap bitmap = new Bitmap(stream);
            return bitmap;
        }

        public void UpdateAlignImage(int Unit, int index)
        {
            try
            {
                if (Unit == 0)
                {
                    string str = string.Format(@"D:\RESULT_IMG\CAM_L{0}.jpg", index + 1);
                    PB_L_0.Image = bitmapfromfile(str);
                    label_LeftVisionOffsetX.Text = "X : " + MSystem.vinterface.VR[Unit].X.ToString() + "mm";
                    label_LeftVisionOffsetY.Text = "Y : " + MSystem.vinterface.VR[Unit].Y.ToString() + "mm";
                }
                if (Unit == 1)
                {
                    string str = string.Format(@"D:\RESULT_IMG\CAM_R{0}.jpg", index + 1);
                    PB_R_0.Image = bitmapfromfile(str);
                    label_RightVisionOffsetX.Text = "X : " + MSystem.vinterface.VR[Unit].X.ToString() + "mm";
                    label_RightVisionOffsetY.Text = "Y : " + MSystem.vinterface.VR[Unit].Y.ToString() + "mm";
                }
            }
            catch (Exception) 
            {
                if (Unit == 0)
                {
                    PB_L_0.Image = null; // add 250701
                }
                if (Unit == 1)
                {
                    PB_R_0.Image = null;
                }
            }
        }
        public void ClearAlignImage(int idx)
        {
            try
            {
                if (idx == 0)
                {
                    PB_L_0.Image = null; // add 250701
                }
                if (idx == 1)
                {
                    PB_R_0.Image = null;
                }
            }
            catch (Exception) { }
        }

        private void HantashUpdate()
        {
            ushort[] data = new ushort[1];
            

            int[] countError = { 0, 0 };
            
            while (true)
            {
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        ushort[] temp = MSystem.m_pHantas[i].MbReadInputRegister(1, 3200, 14);
                        if (temp != null && temp.Length > 4)
                        {
                            countError[i] = 0;
                            MSystem.m_pTrsScrew[i].TargetTorque = temp[3] / 100.0;
                            MSystem.m_pTrsScrew[i]._LastTorque = temp[4] / 100.0;
                        }
                        else
                        {
                            countError[i]++;
                            if (countError[i] > 3)
                            {
                                countError[i] = 0;
                                MSystem.m_pHantas[i].CreateMbRTU();
                                Thread.Sleep(500);
                            }
                        }
                    }
                    catch (Exception ex) 
                    {
                        string err = ex.ToString();
                        MSystem.MyMessagerBottom("Screw Torque Update Fail.");
                        Thread.Sleep(1000);
                    }
                }
                Thread.Sleep(50);
            }
        }

        private void MAIN_BT_IO_MONITOR_Click(object sender, EventArgs e)
        {
            FormIOMonitor dlg = new FormIOMonitor();
            dlg.ShowDialog();
        }

        private void MAIN_BT_SLECT_MODE_Click(object sender, EventArgs e)
        {
            if (MSystem.SysStatus == StatusRun.RUN) return;
            FormMode dlg = new FormMode();
            dlg.ShowDialog();
        }

        private void MAIN_BT_PRODUCT_Click(object sender, EventArgs e)
        {
            //if (MSystem.SysStatus == StatusRun.RUN) return;
            InforTeaching.Instance.SaveSettings();
            FormproductInfor dlg = new FormproductInfor();
            dlg.ShowDialog();
        }

        private void MAIN_BT_ORIGIN_Click(object sender, EventArgs e)
        {
            if (MSystem.SysStatus == StatusRun.RUN)
            {
                if (MSystem.IsOriginStatus == true)
                    MAIN_BT_ORIGIN.SetPress = true;
                return;
            }
            FormOrigin dlg = new FormOrigin();
            dlg.ShowDialog();
            MSystem.IsOriginInitFlag = true;
            timer2.Start();
        }

        private void MAIN_BT_UNIT_INIT_Click(object sender, EventArgs e)
        {
            if (MSystem.SysStatus == StatusRun.RUN)
            {
                if (MSystem.IsInitillizeStatus == true)
                    MAIN_BT_UNIT_INIT.SetPress = true;
                return;
            }
            FormUnitInit dlg = new FormUnitInit();
            dlg.ShowDialog();
            MSystem.IsOriginInitFlag = true;
            timer2.Start();
        }

        private void BT_START_Click(object sender, EventArgs e)
        {
            MSystem.m_pTrsOP.OnStartButton();
        }

        private void BT_STOP_Click(object sender, EventArgs e)
        {
            MSystem.m_pTrsOP.OnStopButton();
        }

        // Unit Init
        public void JigScrewManual(int idx)
        {
            if (MSystem.isStart[idx] == true)
                return;

            ID_JIG_SCREW_MANUAL dlg = new ID_JIG_SCREW_MANUAL(idx);
            dlg.ShowDialog();
        }

        private void labelLeftStatus_Click(object sender, EventArgs e)
        {
            JigScrewManual(Constants.left);
        }

        private void labelRightStatus_Click(object sender, EventArgs e)
        {
            JigScrewManual(Constants.right);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawScrewPoint(Constants.left);
            DrawScrewPoint(Constants.right);

            DisplayMode();
            SensorUpdate();
            UpdateScrewDisplay();
            UpdateLampMain();

            if (InforManager.Instance.m_bVisionType == false)
            {
                if (MSystem.vinterface.Server[0].IsClient_Connected == true)
                {
                    if (LEFT_VISION_STATUS.Image != Properties.Resources.On)
                        LEFT_VISION_STATUS.Image = Properties.Resources.On;
                    if (RIGHT_VISION_STATUS.Image != Properties.Resources.On)
                        RIGHT_VISION_STATUS.Image = Properties.Resources.On;
                }
                else
                {
                    if (LEFT_VISION_STATUS.Image != Properties.Resources.Dis)
                        LEFT_VISION_STATUS.Image = Properties.Resources.Dis;
                    if (RIGHT_VISION_STATUS.Image != Properties.Resources.Dis)
                        RIGHT_VISION_STATUS.Image = Properties.Resources.Dis;
                }
            }
            else
            {
                if (MSystem.vinterface.Server[0].IsClient_Connected == true)
                {
                    if (LEFT_VISION_STATUS.Image != Properties.Resources.On)
                        LEFT_VISION_STATUS.Image = Properties.Resources.On;
                }
                else
                {
                    if (LEFT_VISION_STATUS.Image != Properties.Resources.Dis)
                        LEFT_VISION_STATUS.Image = Properties.Resources.Dis;
                }
                if (MSystem.vinterface.Server[1].IsClient_Connected == true)
                {
                    if (RIGHT_VISION_STATUS.Image != Properties.Resources.On)
                        RIGHT_VISION_STATUS.Image = Properties.Resources.On;
                }
                else
                {
                    if (RIGHT_VISION_STATUS.Image != Properties.Resources.Dis)
                        RIGHT_VISION_STATUS.Image = Properties.Resources.Dis;
                }
            }
            //UpdateAlignImage(0); // test
            //UpdateAlignImage(1); // test
        }

        public void DrawScrewPoint(int index)
        {
            Graphics g;
            Size s;
            if (index == Constants.left)
            {
                g = pictureBox1.CreateGraphics();
                s = pictureBox1.Size;
            }
            else
            {
                g = pictureBox2.CreateGraphics();
                s = pictureBox2.Size;
            }

            BufferedGraphicsContext context;
            BufferedGraphics grafx;
            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = s;

            grafx = context.Allocate(g, new Rectangle(0, 0, s.Width, s.Height));

            int pictureX = s.Width - 60;
            int pictureY = s.Height - 40;

            Point pictureCenter = new Point(s.Width / 2, s.Height / 2);

            try
            {
                PointAxis min = InforTeaching.Instance.P_Screw[index][0].PScrew;
                PointAxis max = InforTeaching.Instance.P_Screw[index][0].PScrew;

                for (int i = 0; i < InforTeaching.Instance.P_Screw[index].Count; i++)
                {
                    if (InforTeaching.Instance.P_Screw[index][i].PScrew.X > max.X)
                        max.X = InforTeaching.Instance.P_Screw[index][i].PScrew.X;

                    if (InforTeaching.Instance.P_Screw[index][i].PScrew.Y > max.Y)
                        max.Y = InforTeaching.Instance.P_Screw[index][i].PScrew.Y;

                    if (InforTeaching.Instance.P_Screw[index][i].PScrew.X < min.X)
                        min.X = InforTeaching.Instance.P_Screw[index][i].PScrew.X;

                    if (InforTeaching.Instance.P_Screw[index][i].PScrew.Y < min.Y)
                        min.Y = InforTeaching.Instance.P_Screw[index][i].PScrew.Y;
                }

                double distX = max.X - min.X;
                double distY = max.Y - min.Y;

                PointAxis center = new PointAxis();
                center.X = (max.X + min.X) / 2;
                center.Y = (max.Y + min.Y) / 2;

                grafx.Graphics.Clear(Color.WhiteSmoke);

                double x;
                double y;
                for (int i = 0; i < InforTeaching.Instance.P_Screw[index].Count; i++)
                {
                    x = InforTeaching.Instance.P_Screw[index][i].PScrew.X - center.X;
                    y = InforTeaching.Instance.P_Screw[index][i].PScrew.Y - center.Y;

                    if (index == Constants.right)
                        x *= -1.0;
                    if (InforManager.Instance.IsYAxisReverce == false)
                        y *= -1.0;

                    if (Math.Abs(distX) > 1.0)
                        x = x / distX * pictureX;

                    if (Math.Abs(distY) > 1.0)
                        y = y / distY * pictureY;

                    x += pictureCenter.X;
                    y += pictureCenter.Y;

                    if (InforTeaching.Instance.P_Screw[index][i].Skip == true)
                        grafx.Graphics.FillEllipse(new SolidBrush(Color.Gray), (int)x, (int)y - 10, 20, 20);
                    else if (InforTeaching.Instance.P_Screw[index][i].status == 0)
                        grafx.Graphics.FillEllipse(new SolidBrush(Color.Yellow), (int)x, (int)y - 10, 20, 20);
                    else if (InforTeaching.Instance.P_Screw[index][i].status == 1)
                        grafx.Graphics.FillEllipse(new SolidBrush(Color.Lime), (int)x, (int)y - 10, 20, 20);
                    else if (InforTeaching.Instance.P_Screw[index][i].status == 2)
                        grafx.Graphics.FillEllipse(new SolidBrush(Color.Red), (int)x, (int)y - 10, 20, 20);
                    else if (InforTeaching.Instance.P_Screw[index][i].status == 4)
                        grafx.Graphics.FillEllipse(new SolidBrush(Color.Orange), (int)x, (int)y - 10, 20, 20);

                    if (i < 9)
                        grafx.Graphics.DrawString($"{1 + i}", new Font("맑은 고딕", 15, FontStyle.Bold), Brushes.Black, new PointF((float)x - 20, (float)y - 15));
                    else
                        grafx.Graphics.DrawString($"{1 + i}", new Font("맑은 고딕", 15, FontStyle.Bold), Brushes.Black, new PointF((float)x - 30, (float)y - 15));
                }
            }
            catch (Exception) { }

            grafx.Render(g);
            grafx.Dispose();
            g.Dispose();
        }

        void DisplayMode()
        {
            if (MSystem.SysMode == ModeRun.Auto)
            {
                if (MAIN_BT_SLECT_MODE.Text != "Auto Run")
                    MAIN_BT_SLECT_MODE.Text = "Auto Run";
            }
            else if (MSystem.SysMode == ModeRun.DryRun)
            {
                if (MAIN_BT_SLECT_MODE.Text != "Dry Run")
                    MAIN_BT_SLECT_MODE.Text = "Dry Run";
            }
        }

        public void SensorUpdate()
        {
            label6.BackColor = MSystem.m_pTrsScrew[Constants.left].IsDriverFastenOK() == true ? Color.Lime : Color.Transparent;
            label10.BackColor = MSystem.m_pTrsScrew[Constants.right].IsDriverFastenOK() == true ? Color.Lime : Color.Transparent;
        }

        public void UpdateScrewDisplay()
        {
            if (InforManager.Instance.IsLeftUnitUse == false)
                MSystem.m_pTrsJig[Constants.left].JigStatus = 2;
            #region <SCREW #1 - LEFT>
            switch (MSystem.m_pTrsJig[Constants.left].JigStatus)
            {
                case 0:
                    if (labelLeftJigStatus.BackColor != Color.White)
                    {
                        labelLeftJigStatus.Text = "Left";
                        labelLeftJigStatus.BackColor = Color.White;
                    }
                    break;

                case 1:
                    if (labelLeftJigStatus.BackColor != Color.Yellow)
                    {
                        labelLeftJigStatus.Text = "Left";
                        labelLeftJigStatus.BackColor = Color.Yellow;
                    }
                    break;

                case 2:
                    if (labelLeftJigStatus.BackColor != Color.Gray)
                    {
                        labelLeftJigStatus.BackColor = Color.Gray;
                    }
                    break;

                case 3:
                    if (labelLeftJigStatus.BackColor != Color.Lime)
                    {
                        labelLeftJigStatus.Text = "Left";
                        labelLeftJigStatus.BackColor = Color.Lime;
                    }
                    break;

                case 4:
                    if (labelLeftJigStatus.BackColor != Color.Red)
                    {
                        labelLeftJigStatus.Text = "Left";
                        labelLeftJigStatus.BackColor = Color.Red;
                    }
                    break;


                case 8:
                    if (labelLeftJigStatus.BackColor != Color.Red)
                    {
                        labelLeftJigStatus.Text = "Align Fail...!";
                        labelLeftJigStatus.BackColor = Color.Red;
                    } 
                    break;
                case 9:
                    if (labelLeftJigStatus.BackColor != Color.Red)
                    {
                        labelLeftJigStatus.Text = "Barcode Fail...!";
                        labelLeftJigStatus.BackColor = Color.Red;
                    }
                    break;

                default:
                    if (labelLeftJigStatus.BackColor != Color.White)
                        labelLeftJigStatus.BackColor = Color.White;
                    break;
            }

            // Update Cycle Time && Point
            MSystem.DisPlayString(IDC_SCREW_L_POINT_COUNT, $"{MSystem.m_pTrsJig[Constants.left].m_iIndexPoint}/{InforTeaching.Instance.P_Screw[Constants.left].Count}");

            // jig status == Test
            if (MSystem.m_pTrsJig[Constants.left].JigStatus == 1)
            {
                labelLeftTaktTime.Text = $"{MSystem.m_pTrsScrew[Constants.left].swScriewFlowTime.ElapsedMilliseconds / 1000.0:F1}" + " / " + $"{MSystem.m_pTrsJig[Constants.left].ScrewTime.ElapsedMilliseconds / 1000.0:F1}";
            }

            // Update Fail Rate
            long total = InforProduct.Instance.ProductScrew_Pass[Constants.left] + InforProduct.Instance.ProductScrew_Fail[Constants.left];
            if (InforProduct.Instance.ProductScrew_Fail[Constants.left] > 0)
                MSystem.DisPlayString(IDC_SCREW_L_FAIL_RATE, $"{(((float)InforProduct.Instance.ProductScrew_Fail[Constants.left] / total) * 100).ToString("f02")} %");
            else
                MSystem.DisPlayString(IDC_SCREW_L_FAIL_RATE, "0.00%");

            // Update Last Torque
            IDC_SCREW_L_DRIVER_TORQUE.Text = $"{MSystem.m_pTrsScrew[Constants.left]._LastTorque:F2}/{MSystem.m_pTrsScrew[Constants.left].TargetTorque:F2}";
            #endregion

            if (InforManager.Instance.IsRightUnitUse == false)
                MSystem.m_pTrsJig[Constants.right].JigStatus = 2;
            #region <SCREW #2 - RIGHT>
            switch (MSystem.m_pTrsJig[Constants.right].JigStatus)
            {
                case 0:
                    if (labelRightJigStatus.BackColor != Color.White)
                    {
                        labelRightJigStatus.Text = "Right";
                        labelRightJigStatus.BackColor = Color.White;
                    }
                    break;

                case 1:
                    if (labelRightJigStatus.BackColor != Color.Yellow)
                    {
                        labelRightJigStatus.Text = "Right";
                        labelRightJigStatus.BackColor = Color.Yellow;
                    }
                    break;

                case 2:
                    if (labelRightJigStatus.BackColor != Color.Gray)
                    {
                        labelRightJigStatus.Text = "Right";
                        labelRightJigStatus.BackColor = Color.Gray;
                    }
                    break;

                case 3:
                    if (labelRightJigStatus.BackColor != Color.Lime)
                    {
                        labelRightJigStatus.Text = "Right";
                        labelRightJigStatus.BackColor = Color.Lime;
                    }
                    break;

                case 4:
                    if (labelRightJigStatus.BackColor != Color.Red)
                    {
                        labelRightJigStatus.Text = "Right";
                        labelRightJigStatus.BackColor = Color.Red;
                    }
                    break;

                case 8:
                    if (labelRightJigStatus.BackColor != Color.Red)
                    {
                        labelRightJigStatus.Text = "Vision Align Fail...!";
                        labelRightJigStatus.BackColor = Color.Red;
                    }
                    break;
                case 9:
                    if (labelLeftJigStatus.BackColor != Color.Red)
                    {
                        labelLeftJigStatus.Text = "Barcode Fail...!";
                        labelLeftJigStatus.BackColor = Color.Red;
                    }
                    break;

                default:
                    if (labelRightJigStatus.BackColor != Color.White)
                        labelRightJigStatus.BackColor = Color.White;
                    break;
            }

            // Update Cycle Time && Point
            MSystem.DisPlayString(IDC_SCREW_R_POINT_COUNT, $"{MSystem.m_pTrsJig[Constants.right].m_iIndexPoint}/{InforTeaching.Instance.P_Screw[Constants.right].Count}");

            if (MSystem.m_pTrsJig[Constants.right].JigStatus == 1)
            {
                labelRightTaktTime.Text = $"{MSystem.m_pTrsScrew[Constants.right].swScriewFlowTime.ElapsedMilliseconds / 1000.0:F1}" + " / " + $"{MSystem.m_pTrsJig[Constants.right].ScrewTime.ElapsedMilliseconds / 1000.0:F1}";
            }

            // Update Fail Rate
            total = InforProduct.Instance.ProductScrew_Pass[Constants.right] + InforProduct.Instance.ProductScrew_Fail[Constants.right];
            if (InforProduct.Instance.ProductScrew_Fail[Constants.right] > 0)
                MSystem.DisPlayString(IDC_SCREW_R_FAIL_RATE, $"{(((float)InforProduct.Instance.ProductScrew_Fail[Constants.right] / total) * 100).ToString("f02")} %");
            else
                MSystem.DisPlayString(IDC_SCREW_R_FAIL_RATE, "0.00%");

            // Update Last Torque
            IDC_SCREW_R_DRIVER_TORQUE.Text = $"{MSystem.m_pTrsScrew[Constants.right]._LastTorque:F2}/{MSystem.m_pTrsScrew[Constants.right].TargetTorque:F2}";
            #endregion

        }

        void UpdateLampMain()
        {
            if (MSystem.SysStatus == StatusRun.RUN)
            {
                BT_START.SetPress = true;
                BT_STOP.SetPress = false;
            }
            else
            {
                BT_START.SetPress = false;
                BT_STOP.SetPress = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            Point p = new Point(mouseEventArgs.X, mouseEventArgs.Y);

            int screwIndex = GetScrewIndex(Constants.left, p);
            if (screwIndex == -1)
                return;
            if (_IsTorqueGraph == true)
            {
                TorqueGraphView dlg = new TorqueGraphView(Constants.left, screwIndex);
                dlg.Show();
                return;
            }

            if (!InforManager.Instance.IsUseRework)//Lock Rework Mode -Add Tung 250807
            {
                Task.Run(async () =>
                {
                    await Task.Delay(10);
                    string msg = " Rework Mode Not Use!";
                    MSystem.MyMsgMemo(msg, "Error", msgButton.OK, msgIcon.Error);

                });
                return;
            }
            InforTeaching.Instance.P_Screw[Constants.left][screwIndex].Skip = !InforTeaching.Instance.P_Screw[Constants.left][screwIndex].Skip;
            InforTeaching.Instance.SaveSettings(); //Test Delete Save 
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            Point p = new Point(mouseEventArgs.X, mouseEventArgs.Y);

            int screwIndex = GetScrewIndex(Constants.right, p);
            if (screwIndex == -1)
                return;
            if (_IsTorqueGraph == true)
            {
                TorqueGraphView dlg = new TorqueGraphView(Constants.right, screwIndex);
                dlg.Show();
                return;
            }

            if (!InforManager.Instance.IsUseRework) //Lock Rework Mode -Add Tung 250807
            {
                Task.Run(async () =>
                {
                    await Task.Delay(10);
                    string msg = " Rework Mode Not Use!";
                    MSystem.MyMsgMemo(msg, "Error", msgButton.OK, msgIcon.Error);

                });
                return;
            }
            InforTeaching.Instance.P_Screw[Constants.right][screwIndex].Skip = !InforTeaching.Instance.P_Screw[Constants.right][screwIndex].Skip;
             InforTeaching.Instance.SaveSettings();//Test Delete Save 
        }

        private int GetScrewIndex(int jigIndex, Point mouse)
        {
            Size s;
            if (jigIndex == Constants.left)
            {
                s = pictureBox1.Size;
            }
            else
            {
                s = pictureBox2.Size;
            }

            int pictureX = s.Width - 60;
            int pictureY = s.Height - 40;

            Point pictureCenter = new Point(s.Width / 2, s.Height / 2);

            PointAxis min = InforTeaching.Instance.P_Screw[jigIndex][0].PScrew;
            PointAxis max = InforTeaching.Instance.P_Screw[jigIndex][0].PScrew;

            for (int i = 0; i < InforTeaching.Instance.P_Screw[jigIndex].Count; i++)
            {
                if (InforTeaching.Instance.P_Screw[jigIndex][i].PScrew.X > max.X)
                    max.X = InforTeaching.Instance.P_Screw[jigIndex][i].PScrew.X;

                if (InforTeaching.Instance.P_Screw[jigIndex][i].PScrew.Y > max.Y)
                    max.Y = InforTeaching.Instance.P_Screw[jigIndex][i].PScrew.Y;

                if (InforTeaching.Instance.P_Screw[jigIndex][i].PScrew.X < min.X)
                    min.X = InforTeaching.Instance.P_Screw[jigIndex][i].PScrew.X;

                if (InforTeaching.Instance.P_Screw[jigIndex][i].PScrew.Y < min.Y)
                    min.Y = InforTeaching.Instance.P_Screw[jigIndex][i].PScrew.Y;
            }

            double distX = max.X - min.X;
            double distY = max.Y - min.Y;

            PointAxis center = new PointAxis();
            center.X = (max.X + min.X) / 2;
            center.Y = (max.Y + min.Y) / 2;

            double x;
            double y;
            for (int i = 0; i < InforTeaching.Instance.P_Screw[jigIndex].Count; i++)
            {
                x = InforTeaching.Instance.P_Screw[jigIndex][i].PScrew.X - center.X;
                y = InforTeaching.Instance.P_Screw[jigIndex][i].PScrew.Y - center.Y;

                if (jigIndex == Constants.right)
                    x *= -1.0;

                // 베트남에서는 주석하여 사용 중 cnz
                if (InforManager.Instance.IsYAxisReverce == false)
                    y *= -1.0;

                if (Math.Abs(distX) > 1.0)
                    x = x / distX * pictureX;

                if (Math.Abs(distY) > 1.0)
                    y = y / distY * pictureY;

                x += pictureCenter.X;
                y += pictureCenter.Y;

                if (mouse.X > x && mouse.X < x + 20 && mouse.Y > y - 10 && mouse.Y < y + 10)
                    return i;
            }

            return -1;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //if (MSystem.IsOriginInitFlag == true)
            {
                //timer2.Stop();
                //MSystem.IsOriginInitFlag = false;
                
                if (MSystem.IsOriginAll() == false)
                {
                    if (MSystem.IsOriginStatus == true)
                    {
                        MSystem.IsOriginStatus = false;
                        MAIN_BT_ORIGIN.GradientBottom = Color.LightBlue;
                        MAIN_BT_ORIGIN.GradientTop = Color.LightBlue;
                        MAIN_BT_UNIT_INIT.GradientBottom = Color.LightBlue;
                        MAIN_BT_UNIT_INIT.GradientTop = Color.LightBlue;
                    }
                }
                else
                {
                    if (MSystem.IsOriginStatus == false)
                    {
                        MSystem.IsOriginStatus = true;
                        MAIN_BT_ORIGIN.GradientBottom = Color.Orange;
                        MAIN_BT_ORIGIN.GradientTop = Color.Orange;
                        MAIN_BT_UNIT_INIT.GradientBottom = Color.Orange;
                        MAIN_BT_UNIT_INIT.GradientTop = Color.Orange;
                    }
                }

                if (MSystem.IsInitillizeStatus == true)
                {
                    if (MAIN_BT_UNIT_INIT.GradientBottom != Color.Orange)
                    {
                        MAIN_BT_UNIT_INIT.GradientBottom = Color.Orange;
                        MAIN_BT_UNIT_INIT.GradientTop = Color.Orange;
                    }
                }
                else
                {
                    if (MAIN_BT_UNIT_INIT.GradientBottom != Color.LightBlue)
                    {
                        MAIN_BT_UNIT_INIT.GradientBottom = Color.LightBlue;
                        MAIN_BT_UNIT_INIT.GradientTop = Color.LightBlue;
                    }
                }
            }
        }

        private void buttonGraphOrSkip_Click(object sender, EventArgs e)
        {
            if (_IsTorqueGraph == false)
            {
                _IsTorqueGraph = true;
                buttonGraphOrSkip.Text = "Graph View";
                buttonGraphOrSkip.BackColor = Color.Gold;
            }
            else
            {
                _IsTorqueGraph = false;
                buttonGraphOrSkip.Text = "Point Skip";
                buttonGraphOrSkip.BackColor = Color.DarkGray;
            }
        }
        private void ButtonEnable()
        {
            LBL_RESULT_BCR_L.Visible = false;
            LBL_RESULT_BCR_R.Visible = false;
            MAIN_BT_LIGHT.Visible = true;
            MAIN_BT_FEEDER_READY_L_MOVE.Visible = true;
            MAIN_BT_FEEDER_READY_L_MOVE.Text = "L Screw\r\nInput";
            MAIN_BT_FEEDER_READY_R_MOVE.Visible = true;
            MAIN_BT_FEEDER_READY_R_MOVE.Text = "R Screw\r\nInput";
        }

        private void MAIN_BT_LREADY_MOVE_Click(object sender, EventArgs e)
        {
            //MSystem.INC_JigInOutStatus9020(0, false);
            //MSystem.INC_ProductPass();
            //return;
            //위치만 이동
            if (MSystem.m_pDIO.IsOn(IOMap.OUT["OUT_LEFT_JIG_STOP_SW_L"])/*MSystem.SysStatus == StatusRun.STOP*/)
            {
                if (MSystem.IsDetectDoorOpen(out string m) == true)
                {
                    //MSystem.m_pTrsBuzzer.SetBuzzerPattern(0);
                    //MSystem.MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                    MSystem.MySafetyAlarm();
                    return;
                }
                if (MSystem.SetMove((int)Axis.AXIS_Z1, 0.0) != MSystem.MMC_OK)
                {
                    MSystem.MyMsgMemo($"Left Z-UP Home Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                }
                Stopwatch sw = new Stopwatch();
                sw.Restart();
                while (true)
                {
                    if (MSystem.m_pTrsScrew[Constants.left].IsReadyZ())
                    {
                        //MSystem.m_pTrsJig[Constants.left].MoveReadyPosY();
                        MSystem.m_pTrsScrew[Constants.left].MoveReadyPosX();
                        break;
                    }
                    else if (sw.ElapsedMilliseconds > 3000)
                    {
                        MSystem.MyMsgMemo($"Left Z-UP Home Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        break;
                    }
                    Thread.Sleep(10);
                }
            }
        }

        private void MAIN_BT_RREADY_MOVE_Click(object sender, EventArgs e)
        {
            //MSystem.INC_JigInOutStatus9020(0, false, false);
            //MSystem.INC_ProductNG();
            //MSystem.SysStatus = StatusRun.ERROR;
            //return;
            //위치만 이동
            if (MSystem.m_pDIO.IsOn(IOMap.OUT["OUT_RIGHT_JIG_STOP_SW_R"])/*MSystem.SysStatus == StatusRun.STOP*/)
            {
                if (MSystem.IsDetectDoorOpen(out string m) == true)
                {
                    //MSystem.m_pTrsBuzzer.SetBuzzerPattern(0);
                    //MSystem.MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                    MSystem.MySafetyAlarm();
                    return;
                }
                if (MSystem.SetMove((int)Axis.AXIS_Z2, 0.0) != MSystem.MMC_OK)
                {
                    MSystem.MyMsgMemo($"Right Z-UP Home Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                }
                Stopwatch sw = new Stopwatch();
                sw.Restart();
                while (true)
                {
                    if (MSystem.m_pTrsScrew[Constants.right].IsReadyZ())
                    {
                        //MSystem.m_pTrsJig[Constants.right].MoveReadyPosY();
                        MSystem.m_pTrsScrew[Constants.right].MoveReadyPosX();
                        break;
                    }
                    else if (sw.ElapsedMilliseconds > 3000)
                    {
                        MSystem.MyMsgMemo($"Right Z-UP Home Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        break;
                    }
                    Thread.Sleep(10);
                }
            }
        }

        private void MAIN_BT_LIGHT_Click(object sender, EventArgs e)
        {
            //MSystem.INC_ProductInput();
            //MSystem.INC_JigInOutStatus9020(0, true);
            //return;
            if (MSystem.m_pDIO.IsOn(IOMap.OUT["OUT_LIGHT_ON"]))
            {
                MSystem.m_pDIO.OutPutOff(IOMap.OUT["OUT_LIGHT_ON"]);
            }
            else
            {
                MSystem.m_pDIO.OutPutOn(IOMap.OUT["OUT_LIGHT_ON"]);
            }
        }

        private void MAIN_BT_FEEDER_READY_L_MOVE_Click(object sender, EventArgs e)
        {
            if (MSystem.m_pDIO.IsOn(IOMap.OUT["OUT_LEFT_JIG_STOP_SW_L"]))
            {
                if (MSystem.IsDetectDoorOpen(out string m) == true)
                {
                    //MSystem.m_pTrsBuzzer.SetBuzzerPattern(0);
                    //MSystem.MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                    MSystem.MySafetyAlarm();
                    return;
                }
                MSystem.m_pTrsScrew[Constants.left].MoveReadyPosZ();
                Stopwatch sw = new Stopwatch();
                sw.Restart();
                while (true)
                {
                    if (MSystem.m_pTrsScrew[Constants.left].IsReadyZ())
                    {
                        double _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT, (int)PScrewMain.ePOS_PICKUP].X;
                        _dX += 150;
                        if (MSystem.SetMove((int)Axis.AXIS_X1, _dX) != MSystem.MMC_OK)
                        {
                            MSystem.MyMsgMemo($"Right X Feeder Ready Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        }
                        break;
                    }
                    else if (sw.ElapsedMilliseconds > 3000)
                    {
                        MSystem.MyMsgMemo($"Right Z-UP Home Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        break;
                    }
                    Thread.Sleep(10);
                }
                MSystem.MyMsgMemo($"Start inspection after initializing the equipment.", "Alarm", msgButton.OK, msgIcon.Error);
            }
        }

        private void MAIN_BT_FEEDER_READY_R_MOVE_Click(object sender, EventArgs e)
        {
            if (MSystem.m_pDIO.IsOn(IOMap.OUT["OUT_RIGHT_JIG_STOP_SW_R"]))
            {
                if (MSystem.IsDetectDoorOpen(out string m) == true)
                {
                    //MSystem.m_pTrsBuzzer.SetBuzzerPattern(0);
                    //MSystem.MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                    MSystem.MySafetyAlarm();
                    return;
                }
                MSystem.m_pTrsScrew[Constants.right].MoveReadyPosZ();
                Stopwatch sw = new Stopwatch();
                sw.Restart();
                while (true)
                {
                    if (MSystem.m_pTrsScrew[Constants.right].IsReadyZ())
                    {
                        double _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eRIGHT, (int)PScrewMain.ePOS_PICKUP].X;
                        _dX += 150;
                        if (MSystem.SetMove((int)Axis.AXIS_X2, _dX) != MSystem.MMC_OK)
                        {
                            MSystem.MyMsgMemo($"Right X Feeder Ready Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        }
                        break;
                    }
                    else if (sw.ElapsedMilliseconds > 3000)
                    {
                        MSystem.MyMsgMemo($"Left Z-UP Home Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        break;
                    }
                    Thread.Sleep(10);
                }
                MSystem.MyMsgMemo($"Start inspection after initializing the equipment.", "Alarm", msgButton.OK, msgIcon.Error);
            }
        }

        private void PB_L_0_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("ScrewAlign_VPRO");

            if (processes.Length > 0)
            {
                IntPtr hWnd = processes[0].MainWindowHandle;

                if (hWnd != IntPtr.Zero)
                {
                    // 최소화 상태면 복원
                    if (IsIconic(hWnd))
                    {
                        ShowWindow(hWnd, 9);
                    }

                    // 최상위로 가져오기
                    SetForegroundWindow(hWnd);
                }
            }
            else
            {
                MessageBox.Show("프로세스를 찾을 수 없습니다.");
            }
        }

        private void PB_R_0_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("ScrewAlign_VPRO");

            if (processes.Length > 0)
            {
                IntPtr hWnd = processes[0].MainWindowHandle;

                if (hWnd != IntPtr.Zero)
                {
                    // 최소화 상태면 복원
                    if (IsIconic(hWnd))
                    {
                        ShowWindow(hWnd, 9);
                    }

                    // 최상위로 가져오기
                    SetForegroundWindow(hWnd);
                }
            }
            else
            {
                MessageBox.Show("프로세스를 찾을 수 없습니다.");
            }
        }
    }
}
