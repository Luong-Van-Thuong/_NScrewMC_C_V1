using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{

    public partial class FormTeachScrew : Form
    {
        Bitmap _ImgGrap;
        Line line1;
        Line line2;

        int _indexRe = 0;
        bool IsJogZUp = false;
        TimerDelay TimCheck = new TimerDelay();
        #region  Posteach
        public enum PosTeach
        {
            //Screw Position
            ePOS_MASTER_SCREW,
            ePOS_MASTER_VISION,
            ePOS_READY,
            ePOS_TRASH,
            ePOS_PICKUP,
            ePOS_Z_SAFETY,
            ePOS_SCREW,
            ePOS_BARCODE,
            ePOS_VISION_TEST,
            ePOS_JIG_COVER,
            ePOS_MAX
        }
        public PosTeach P_Teach = new PosTeach();
        public enum AxisTEACHING
        {
            eAXIS_X = 0,
            eAXIS_Y = 1,
            eAXIS_Z = 2,
            eAXIS_MAX
        }
        #endregion

        #region //Button Tech
        //Position
        AxBTNENHLib4.AxBtnEnh[] m_btCurrentPos = new AxBTNENHLib4.AxBtnEnh[(int)AxisTEACHING.eAXIS_MAX];
        AxBTNENHLib4.AxBtnEnh[] m_btTargetPos = new AxBTNENHLib4.AxBtnEnh[(int)AxisTEACHING.eAXIS_MAX];

        // private const int MaxPOS = 15;
        public AxBTNENHLib4.AxBtnEnh[] m_btPoselect = new AxBTNENHLib4.AxBtnEnh[(int)PosTeach.ePOS_MAX];
        #endregion

        ImageList ImageListPoint = new ImageList();
        System.Windows.Forms.Timer TimUpdateData = new System.Windows.Forms.Timer();
        public readonly object LockUpDate = new object();
        bool m_iPress = false;

        #region //Variable
        public int m_idx = 0;

        int _indexLog = 0;
        //string[] strNameCols = { "ID", "X", "Y", "Z", "Skip", "Retry", "Channel", "Light" };
        //int[] _withNameCols = { 68, 83, 83, 83, 48, 52, 74, 80 };
        string[] strNameCols;// = { "ID", "X", "Y", "Z", "Skip", "Vision", "Retry", "Channel", "FasNumbers" };
        int[] _withNameCols;// = { 70, 90, 90, 90, 60, 60,60,90,105 };
        
        bool JogRemember = false;
        public int _numRowsCheck = 12;

        private bool IsDoorOpenStatus = false;
        private int PositionListPage = 0;
        #endregion

        #region //Form Init
        public FormTeachScrew(int index)
        {
            m_idx = index;
            MSystem.m_bIsTeachDlg = true;
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            #region ListView Initial 
            for (int idx = 0; idx < MSystem.m_ScrewTestResult.Count(); idx++)
                MSystem.m_ScrewTestResult[idx] = 0;
            if (InforManager.Instance.InspectionType == "VST")
            {
                strNameCols = new string[] { "ID", "X", "Y", "Z", "Use" ,"Test", "Preset"};
                _withNameCols = new int[] { 70, 90, 90, 90, 60 ,60, 60 };
            }
            else
            {
                strNameCols = new string[] { "ID", "X", "Y", "Z", "Use","Test", "Preset", "Vision","Retry", "RetryCount" };
                _withNameCols = new int[] { 70, 90, 90, 90, 60, 60, 60, 60, 60,105 };
            }
            LV_LIST_SCREW.Items.Clear();

            MSystem.m_pListScrew.InitListview(m_idx, LV_LIST_SCREW, strNameCols, _withNameCols, LV_LIST_SCREW.Top, LV_LIST_SCREW.Left,
                                            LV_LIST_SCREW.Size, 11F, FontStyle.Bold, 50);

            this.ImageListPoint.ImageSize = new System.Drawing.Size(16, 16);
            ImageListPoint.Images.Add("ImageOn", Properties.Resources.On);
            ImageListPoint.Images.Add("ImageOff", Properties.Resources.Off);
            ImageListPoint.Images.Add("ImageCheck", Properties.Resources.check);
            ImageListPoint.Images.Add("ImageSel", Properties.Resources.Select);
            ImageListPoint.Images.Add("ImageUnSel", Properties.Resources.UnSelect);
            ImageListPoint.Images.Add("Dis", Properties.Resources.Dis);
            LV_LIST_SCREW.BackColor = Color.White;

            int i = 0;
            LV_LIST_SCREW.SmallImageList = ImageListPoint;
            for (i = 0; i < 20; i++)
            {
                var item1 = new ListViewItem(new string[] { $"P{i}", "100.00", "100.00", "100.00", "", ""});//, $"{i % 2}", "" });
                item1.ImageKey = "ImageCheck";
                LV_LIST_SCREW.Items.Add(item1);

            }
            if (LV_LIST_SCREW.Items.Count > 0)
            {
                LV_LIST_SCREW.SelectedItems.Clear();
                LV_LIST_SCREW.Items[0].Selected = true;

                LV_LIST_SCREW.EnsureVisible(0);
            }
            LV_LIST_SCREW.Refresh();
            #endregion

            #region Button Select Position
            i = 0;
            m_btPoselect[i++] = BT_LOAD;
            m_btPoselect[i++] = BT_UNLOAD;// new CBbutton(BT_UNLOAD, false);
            m_btPoselect[i++] = BT_READY;//new CBbutton(BT_READY, false);
            m_btPoselect[i++] = BT_TRASH;//new CBbutton(BT_TRASH, false);
            m_btPoselect[i++] = BT_PICKUP;// new CBbutton(BT_PICKUP, false);
            m_btPoselect[i++] = BT_Z_SAFETY;
            m_btPoselect[i++] = BT_SCREW;
            m_btPoselect[i++] = BT_BARCODE;
            m_btPoselect[i++] = BT_VISION_TEST;
            m_btPoselect[i++] = BT_JIG_COVER;

            #endregion

            #region BT Current Position and Target position
            i = 0;
            m_btCurrentPos[i++] = TB_CURRENT0;
            m_btCurrentPos[i++] = TB_CURRENT1;
            m_btCurrentPos[i++] = TB_CURRENT2;
            i = 0;
            m_btTargetPos[i++] = TB_TARGET0;
            m_btTargetPos[i++] = TB_TARGET1;
            m_btTargetPos[i++] = TB_TARGET2;

            #endregion

            #region Other Initial
            MSystem._mRun = new Thread(UpDateData);
            MSystem._mRun.IsBackground = true;
            MSystem._mRun.Start();
            //this.TimUpdateData.Tick += new System.EventHandler(this.UpDateData);
            //TimUpdateData.Interval = 100;
            //TimUpdateData.Start();
            //Init Speed Jog
            MSystem.m_pTrsJog._JogMode = 0;
            LoadJogModeDisplay();

            if (index == Constants.left)
            {
                BT_SELECT_SCREW.Caption = "Left";
                m_idx = 0;
                MSystem.m_pTrsJog._JogMode = 0;
                //LoadJogModeDisplay();


                LV_LIST_SCREW.Items.Clear();
                MSystem.m_pListScrew.LoadDataScrew(m_idx);
                MSystem.m_pListScrew.SelectIndex();
                SelectButton(m_btPoselect[(int)FormTeachScrew.PosTeach.ePOS_SCREW]);
            }
            else
            {
                BT_SELECT_SCREW.Caption = "Right";
                m_idx = 1;
                MSystem.m_pTrsJog._JogMode = 0;
                //LoadJogModeDisplay();


                LV_LIST_SCREW.Items.Clear();
                MSystem.m_pListScrew.LoadDataScrew(m_idx);
                MSystem.m_pListScrew.SelectIndex();
                SelectButton(m_btPoselect[(int)FormTeachScrew.PosTeach.ePOS_SCREW]);
            }

            //if (LV_LIST_SCREW.Items.Count < _numRowsCheck)
            //    LV_LIST_SCREW.Columns[6].Width = 88;
            //else
            //    LV_LIST_SCREW.Columns[6].Width = 74;
            #endregion


            timer1.Start();

            timer2.Interval = 500;
            timer2.Start();

            // Screw TokeValue Read
            ushort[] temp = MSystem.m_pHantas[m_idx].MbReadHoldingRegister(0x01, 0x02, 7);
            if (temp != null && temp.Length == 7)
            {
                int torque = Convert.ToInt32(temp[0]);
                textTorque.Text = (torque / 100.0).ToString("f02");
                textTorqueMinMax.Text = (Convert.ToInt32(temp[6])).ToString();
            }
            this.textTorque.TextChanged += new System.EventHandler(this.TorqueInput);

            // Tablet
            if (InforManager.Instance.InspectionType == "Tablet")
            {
                buttonJigUpDown.Text = "Cover Up/Down";
                buttonJigFwdBwd.Text = "Fix / UnFix"; //buttonJigUpDown.Text = "Fix / UnFix";
            }
            else
            {
                buttonJigUpDown.Text = "Up / Down";
                buttonJigFwdBwd.Text = "FWD / BWD"; //buttonJigUpDown.Text = "FWD / BWD";
            }
            if (InforManager.Instance.InspectionType == "VST"
                || InforManager.Instance.InspectionType == "Tablet Normal"
                || InforManager.Instance.AutoVisionAlignMode == 0)
            {
                BT_VISION_TEST.Visible = false;
                BT_LOAD.Visible = false;
                BT_UNLOAD.Visible = false;
                axBtnEnh2.Visible = false;
                //BT_SCREW.Visible = false;
                BT_ALIGN.Visible = false;
                BT_SET_FIRSTPOS.Enabled = true;
                InforManager.Instance.IsVisionPoint = false;
            }
            else
            {
                
                BT_VISION_TEST.Visible = true;
                BT_LOAD.Visible = true;
                BT_UNLOAD.Visible = true;
                //BT_SCREW.Visible = true;
                axBtnEnh2.Visible = true;
                BT_ALIGN.Visible = true;
                //BT_SET_FIRSTPOS.Enabled = false;
            }
            if (InforManager.Instance.IsBarcodeUse == true)
                BT_BARCODE.Visible = true;
            else
                BT_BARCODE.Visible = false;
            if (InforManager.Instance.IsSmartKitUse == true)
                BT_JIG_COVER.Visible = true;
            else
                BT_JIG_COVER.Visible = false;

            // Hide Copy Offset and Get Offset button
            BT_GET_OFFSET.Visible = false;
            BT_COPY_POINT.Visible = false;
            BT_SCREW.Caption = InforManager.Instance.IsVisionPoint == true ? "Vision Point" : "Screw Point";
        }
        #endregion
        #region Update Data Form
        private void UpDateData(/*object sender, EventArgs e*/)
        {
            Thread.Sleep(1000);
            while (true)
            {
                /*Disable Jog*/
                try
                {
                    if (Visible == true)
                    {
                        if (!m_iPress && JogRemember)
                        {
                            JogRemember = false;
                            DisableJog();
                        }
                        UpdatePosition();
                        Thread.Sleep(1);
                        JogTP();
                        Thread.Sleep(1);
                        SensorUpdate();
                        Thread.Sleep(1);
                    }
                }
                catch (Exception)
                {
                    MSystem.MyMessagerBottom("Logic form teach screw fail");
                }
                Thread.Sleep(10);
            }
        }

        public void SensorUpdate()
        {
            MSystem.IsSensorOn(IDC_SCREW_JIG_DETECT_COVER_SENS, MSystem.m_pTrsJig[m_idx].IsDetectCover());
            MSystem.IsSensorOn(IDC_VACUUM_ON, MSystem.m_pTrsScrew[m_idx].IsVacuumSensorOn());
            MSystem.IsSensorOn(IDC_BLOW_ON, MSystem.m_pTrsScrew[m_idx].IsBlowSolOn());

            MSystem.IsSensorOn(IDC_FEEDER_READY, MSystem.m_pTrsScrew[m_idx].IsFeederReady());
            MSystem.IsSensorOn(IDC_DRIVER_READY, MSystem.m_pTrsScrew[m_idx].IsDriverReady());
            MSystem.IsSensorOnOut(IDC_DRIVER_RUN, MSystem.m_pTrsScrew[m_idx].IsDriverRun(), MSystem.m_pTrsScrew[m_idx].IsDriverAlarm(), "Driver Run", "Driver Stop", "Driver Alarm");
            MSystem.IsSensorOn(IDC_DRIVER_RESET, MSystem.m_pTrsScrew[m_idx].IsDriverResetAlarm());
            MSystem.IsSensorOn(IDC_DRIVER_FASTEN_LOOSEN, MSystem.m_pTrsScrew[m_idx].IsDriverFastenLoosen());
            MSystem.IsSensorOn(IDC_DRIVER_RESET, MSystem.m_pTrsScrew[m_idx].IsDriverResetAlarm());
            if (MSystem.m_pTrsScrew[m_idx].IsVacuumSolOn() == true)
            {
                if (IDC_VACUUM_ON.BackColorInterior != Color.Lime)
                    IDC_VACUUM_ON.BackColorInterior = Color.Lime;
            }
            else
            {
                if (IDC_VACUUM_ON.BackColorInterior != Color.White)
                    IDC_VACUUM_ON.BackColorInterior = Color.White;
            }
        }

        private void PreLoadEvent(object sender, EventArgs e)
        {
            switch ((sender as AxBTNENHLib4.AxBtnEnh).Name)
            {
                case "BT_SAVE_Z":
                    SaveAllZ();
                    break;

                case "BT_SAVE_XY":
                    SaveDataXY();
                    break;

                case "BT_SAVE_XYZ":
                    SaveDataXYZ();
                    break;

                case "BT_MOVE_XY":
                    if (MSystem.MyMsgMemo($"Are you want Move XY To Position {((PosTeach)GetButtonSelected()).ToString()}?", "Question", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
                        return;
                    MoveXY();
                    break;

                case "BT_MOVE_XYZ":
                    if (MSystem.MyMsgMemo($"Are you want Move XYZ To Position {((PosTeach)GetButtonSelected()).ToString()}?", "Question", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
                        return;
                    MoveXYZ();
                    break;

                case "BT_MOVE_UP":
                    if (MSystem.MyMsgMemo($"Are you want Move Z-UP {((PosTeach)GetButtonSelected()).ToString()}?", "Question", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
                        return;
                    MoveUP();
                    break;

                case "BT_MOVE_DOWN":
                    if (MSystem.MyMsgMemo($"Are you want Move Z-DOWN {((PosTeach)GetButtonSelected()).ToString()}?", "Question", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
                        return;
                    MoveDown();
                    break;

                case "BT_SPEED":
                case "BT_SPEEDZ":
                    if (MSystem.m_pTrsJog._JogMode == 0)
                        MSystem.m_pTrsJog._JogMode = 1;
                    else if (MSystem.m_pTrsJog._JogMode == 1)
                        MSystem.m_pTrsJog._JogMode = 2;
                    else MSystem.m_pTrsJog._JogMode = 0;
                    LoadJogModeDisplay();
                    break;

                case "BT_EXIT":
                    this.Close();
                    break;

                default:
                    break;
            }
        }

        void PreLoadEventPos(object sender, EventArgs e)
        {
            if (LV_LIST_SCREW.Items.Count > 0)
                LV_LIST_SCREW.SelectedItems.Clear();
            SelectButton(sender as AxBTNENHLib4.AxBtnEnh);
            if (sender as AxBTNENHLib4.AxBtnEnh == BT_SCREW)
            {
                if (LV_LIST_SCREW.Items.Count > 0)
                {
                    LV_LIST_SCREW.SelectedItems.Clear();
                    LV_LIST_SCREW.Items[0].Selected = true;
                    LV_LIST_SCREW.EnsureVisible(0);
                    LV_LIST_SCREW.Focus();
                }
            }
        }
        void PreLoadEventDbMove(object sender, EventArgs e)
        {
            SelectButton(sender as AxBTNENHLib4.AxBtnEnh);
            if (LV_LIST_SCREW.Items.Count > 0 && (sender as AxBTNENHLib4.AxBtnEnh) != BT_SCREW)
            {
                SelectButton(sender as AxBTNENHLib4.AxBtnEnh);
                LV_LIST_SCREW.SelectedItems.Clear();
            }
            MoveXY();
        }
        void UpdatePosition()
        {
            m_btCurrentPos[0].Caption = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx).ToString("f3");
            m_btCurrentPos[1].Caption = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx).ToString("f3");
            m_btCurrentPos[2].Caption = MSystem.GetCurrentPos(Axis.AXIS_Z1 + m_idx).ToString("f3");
            // CHECK LIMIT Z+
            if (MSystem.GetCurrentPos(Axis.AXIS_Z1) >= InforManager.Instance.m_dLimitZPlus
                || MSystem.GetCurrentPos(Axis.AXIS_Z2) >= InforManager.Instance.m_dLimitZPlus)  
            {
                if (IsJogZUp) return;
                else
                {
                    m_iPress = false;
                    JogRemember = false;
                    DisableJog();
                    //MSystem.SetAllStop();
                    return;
                }
            }
        }

        public void UnSelectButton()
        {

            for (int i = 0; i < (int)PosTeach.ePOS_MAX; i++)
            {
                if (i == (int)PosTeach.ePOS_SCREW) continue;
                m_btPoselect[i].Value = 0;
            }
        }

        public void SelectButton(AxBTNENHLib4.AxBtnEnh _bt)
        {
            UnSelectButton();
            m_btPoselect[GetIndexButton(_bt)].Value = 1;
            SelectTargetPost((PosTeach)GetIndexButton(_bt));
        }
        void SelectTargetPost(PosTeach _PosSelected)
        {
            try
            {
                ResetColorPosition();
                switch (_PosSelected)
                {
                    case PosTeach.ePOS_MASTER_SCREW:
                        m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X.ToString("f03")}";
                        m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y.ToString("f03")}";
                        m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Z.ToString("f03")}";
                        SetColorPosition(0);
                        SetColorPosition(1);
                        SetColorPosition(2);
                        break;

                    case PosTeach.ePOS_MASTER_VISION:
                        m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X.ToString("f03")}";
                        m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y.ToString("f03")}";
                        m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Z.ToString("f03")}";
                        SetColorPosition(0);
                        SetColorPosition(1);
                        SetColorPosition(2);
                        break;

                    case PosTeach.ePOS_READY:
                        m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].X.ToString("f03")}";
                        m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Y.ToString("f03")}";
                        m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Z.ToString("f03")}";
                        SetColorPosition(0);
                        SetColorPosition(1);
                        SetColorPosition(2);
                        break;

                    case PosTeach.ePOS_TRASH:
                        m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].X.ToString("f03")}";
                        m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].Y.ToString("f03")}";
                        m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].Z.ToString("f03")}";
                        SetColorPosition(0);
                        SetColorPosition(1);
                        SetColorPosition(2);
                        break;

                    case PosTeach.ePOS_PICKUP:
                        m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].X.ToString("f03")}";
                        m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].Y.ToString("f03")}";
                        m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].Z.ToString("f03")}";
                        SetColorPosition(0);
                        SetColorPosition(1);
                        SetColorPosition(2);
                        break;

                    case PosTeach.ePOS_Z_SAFETY:
                        m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Z.ToString("f03")}";
                        SetColorPosition(2);
                        break;

                    case PosTeach.ePOS_SCREW:
                        int _index = -1;
                        //int _LVCount = -1;
                        if (LV_LIST_SCREW.SelectedIndices.Count > 0)
                        {
                            _index = LV_LIST_SCREW.SelectedIndices[0];

                            double posX = InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.X;
                            double posY = InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.Y;
                            double posZ = InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.Z;
                            if (InforManager.Instance.IsVisionPoint == true)
                            {
                                posX += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                                posY += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                                //posZ += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_VISION].Z;
                                posX -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                                posY -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                                //posZ -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_SCREW].Z;

                                posZ = InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.VisionZ;
                            }

                            m_btTargetPos[0].Caption = $"{posX.ToString("f03")}";
                            m_btTargetPos[1].Caption = $"{posY.ToString("f03")}";
                            m_btTargetPos[2].Caption = $"{posZ.ToString("f03")}";
                            SetColorPosition(0);
                            SetColorPosition(1);
                            SetColorPosition(2);
                        }
                        break;

                    case PosTeach.ePOS_BARCODE:
                        SetColorPosition(0);
                        SetColorPosition(1);
                        SetColorPosition(2);
                        m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].X.ToString("f02")}";
                        m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].Y.ToString("f02")}";
                        m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].Z.ToString("f02")}";
                        
                        break;

                    case PosTeach.ePOS_JIG_COVER:
                        m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].X.ToString("f02")}";
                        m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].Y.ToString("f02")}";
                        m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].Z.ToString("f02")}";
                        SetColorPosition(0);
                        SetColorPosition(1);
                        SetColorPosition(2);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
        }
        void SelectButton(int _index)
        {
            UnSelectButton();
            if (LV_LIST_SCREW.Items.Count > 0)
                LV_LIST_SCREW.SelectedItems.Clear();
            m_btPoselect[_index].Value = 1;
        }

        int GetIndexButton(AxBTNENHLib4.AxBtnEnh _bt)
        {
            for (int i = 0; i < (int)PosTeach.ePOS_MAX; i++)
            {
                if (m_btPoselect[i] == _bt)
                    return i;
            }
            return -1;
        }
        int GetButtonSelected()
        {
            for (int i = 0; i < (int)PosTeach.ePOS_MAX; i++)
            {
                if (m_btPoselect[i].Value == 1) return i;
            }
            return -1;
        }
        void ResetColorPosition()
        {
            for (int i = 0; i < (int)AxisTEACHING.eAXIS_MAX; i++)
            {
                // m_btCurrentPos[i].CtlForeColor = Color.Black;
                m_btTargetPos[i].CtlForeColor = Color.Black;
            }
        }
        void SetColorPosition(int _index)
        {
            m_btCurrentPos[_index].CtlForeColor = Color.Red;
            m_btTargetPos[_index].CtlForeColor = Color.Red;
        }

        void SetTargetPort(object sender, EventArgs e)
        {
            int _index = GetButtonSelected();
            double tmpData = 0;
            AxBTNENHLib4.AxBtnEnh _tmpBt = sender as AxBTNENHLib4.AxBtnEnh;
            if (_tmpBt.CtlForeColor != Color.Red) return;
            string old_value = _tmpBt.Caption;
            if (!MSystem.Getdouble2(sender, ref tmpData))
            {
                _tmpBt.Caption = old_value;
                return;
            }
            else
            {
                SavePosByHand(_index);
            }

        }
        void SavePosByHand(int _Posindex)
        {
            double dPosX = 0;
            double dPosY = 0;
            double dPosZ = 0;
            switch (_Posindex)
            {
                case (int)PosTeach.ePOS_MASTER_SCREW:
                    dPosX = double.Parse(m_btTargetPos[0].Caption);
                    dPosY = double.Parse(m_btTargetPos[1].Caption);
                    dPosZ = double.Parse(m_btTargetPos[2].Caption);
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y = dPosY;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Z = dPosZ;

                    InforTeaching.Instance.SaveSettings();
                    break;

                case (int)PosTeach.ePOS_MASTER_VISION:
                    dPosX = double.Parse(m_btTargetPos[0].Caption);
                    dPosY = double.Parse(m_btTargetPos[1].Caption);
                    dPosZ = double.Parse(m_btTargetPos[2].Caption);
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y = dPosY;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Z = dPosZ;

                    InforTeaching.Instance.SaveSettings();
                    break;

                case (int)PosTeach.ePOS_READY:
                    dPosX = double.Parse(m_btTargetPos[0].Caption);
                    dPosY = double.Parse(m_btTargetPos[1].Caption);
                    dPosZ = double.Parse(m_btTargetPos[2].Caption);
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Y = dPosY;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Z = dPosZ;

                    InforTeaching.Instance.SaveSettings();
                    break;

                case (int)PosTeach.ePOS_TRASH:
                    dPosX = double.Parse(m_btTargetPos[0].Caption);
                    dPosY = double.Parse(m_btTargetPos[1].Caption);
                    dPosZ = double.Parse(m_btTargetPos[2].Caption);
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].Y = dPosY;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].Z = dPosZ;

                    InforTeaching.Instance.SaveSettings();
                    break;

                case (int)PosTeach.ePOS_PICKUP:
                    dPosX = double.Parse(m_btTargetPos[0].Caption);
                    dPosY = double.Parse(m_btTargetPos[1].Caption);
                    dPosZ = double.Parse(m_btTargetPos[2].Caption);
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].Y = dPosY;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].Z = dPosZ;

                    InforTeaching.Instance.SaveSettings();
                    break;

                case (int)PosTeach.ePOS_Z_SAFETY:
                    dPosZ = double.Parse(m_btTargetPos[2].Caption);
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Z = dPosZ;
                    InforTeaching.Instance.SaveSettings();
                    break;

                case (int)PosTeach.ePOS_SCREW:
                    {
                        int _index = -1;
                        //int _LVCount = -1;

                        dPosX = double.Parse(m_btTargetPos[0].Caption);
                        dPosY = double.Parse(m_btTargetPos[1].Caption);
                        dPosZ = double.Parse(m_btTargetPos[2].Caption);
                        
                        if (LV_LIST_SCREW.SelectedIndices.Count > 0)
                        {
                            _index = LV_LIST_SCREW.SelectedIndices[0];

                            LV_LIST_SCREW.Items[_index].SubItems[1].Text = $"{dPosX.ToString("f03")}";
                            LV_LIST_SCREW.Items[_index].SubItems[2].Text = $"{dPosY.ToString("f03")}";
                            LV_LIST_SCREW.Items[_index].SubItems[3].Text = $"{dPosZ.ToString("f03")}";

                            if (InforManager.Instance.IsVisionPoint == true)
                            {
                                dPosX += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                                dPosY += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                                //dPosZ += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_SCREW].Z;
                                dPosX -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                                dPosY -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                                //dPosZ -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_VISION].Z;
                                InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.VisionZ = dPosZ;
                            }
                            else
                            {
                                InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.Z = dPosZ;
                            }
                            InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.X = dPosX;
                            InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.Y = dPosY;

                            InforTeaching.Instance.SaveSettings();
                        }
                    }
                    break;

                case (int)PosTeach.ePOS_BARCODE:
                    dPosX = double.Parse(m_btTargetPos[0].Caption);
                    dPosY = double.Parse(m_btTargetPos[1].Caption);
                    dPosZ = double.Parse(m_btTargetPos[2].Caption);
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].Y = dPosY;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].Z = dPosZ;

                    InforTeaching.Instance.SaveSettings();
                    break;

                case (int)PosTeach.ePOS_JIG_COVER:
                    dPosX = double.Parse(m_btTargetPos[0].Caption);
                    dPosY = double.Parse(m_btTargetPos[1].Caption);
                    dPosZ = double.Parse(m_btTargetPos[2].Caption);
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].Y = dPosY;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].Z = dPosZ;

                    InforTeaching.Instance.SaveSettings();
                    break;

                default:
                    break;
            }
        }

        void SaveDataXYZ()
        {
            if (!MSystem.IsOriginAll() && !MSystem.SIMULATION)
            {
                MSystem.MyMsgMemo("NO ORIGIN!!", "Error", msgButton.OK, msgIcon.Error);
                return;
            }

            if (MSystem.MyMsgMemo($"Do you want Save Position {((PosTeach)GetButtonSelected()).ToString()}?", "Question", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            // Save data
            var Result = Task.Run(async () =>
            {
                await Task.Delay(500);
                SavePosXYZ((PosTeach)GetButtonSelected());
                MoveUP();
                await Task.Delay(100);
                MSystem.MyMsgMemo("Saving Complete...!", "Thongbao", msgButton.OK, msgIcon.Infor);
                MSystem.KillMsgDisplay();

                //if (GetButtonSelected() == (int)PosTeach.ePOS_SCREW)
                //    MoveNextScrewPoint();
            });
            MSystem.MyMsgDisplay("Start Save Data.....\r\nWait a moment....");
        }
        void SavePosXYZ(PosTeach _PosSelected)
        {
            double dPosX = 0;
            double dPosY = 0;
            double dPosZ = 0;

            switch (_PosSelected)
            {
                case PosTeach.ePOS_MASTER_SCREW:
                    dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                    dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);
                    dPosZ = MSystem.GetCurrentPos(Axis.AXIS_Z1 + m_idx);

                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y = dPosY;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Z = dPosZ;

                    m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X.ToString("f03")}";
                    m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y.ToString("f03")}";
                    m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Z.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;
                case PosTeach.ePOS_MASTER_VISION:
                    dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                    dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);
                    dPosZ = MSystem.GetCurrentPos(Axis.AXIS_Z1 + m_idx);

                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y = dPosY;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Z = dPosZ;

                    m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X.ToString("f03")}";
                    m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y.ToString("f03")}";
                    m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].VisionZ.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;
                case PosTeach.ePOS_READY:
                    dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                    dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);
                    dPosZ = MSystem.GetCurrentPos(Axis.AXIS_Z1 + m_idx);

                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Y = dPosY;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Z = dPosZ;

                    m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].X.ToString("f03")}";
                    m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Y.ToString("f03")}";
                    m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Z.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;
                case PosTeach.ePOS_TRASH:
                    dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                    dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);
                    dPosZ = MSystem.GetCurrentPos(Axis.AXIS_Z1 + m_idx);

                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].Y = dPosY;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].Z = dPosZ;

                    m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].X.ToString("f03")}";
                    m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].Y.ToString("f03")}";
                    m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].Z.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;
                case PosTeach.ePOS_PICKUP:
                    dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                    dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);
                    dPosZ = MSystem.GetCurrentPos(Axis.AXIS_Z1 + m_idx);

                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].Y = dPosY;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].Z = dPosZ;

                    m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].X.ToString("f03")}";
                    m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].Y.ToString("f03")}";
                    m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].Z.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;
                case PosTeach.ePOS_BARCODE:
                    dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                    dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);
                    dPosZ = MSystem.GetCurrentPos(Axis.AXIS_Z1 + m_idx);

                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].Y = dPosY;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].Z = dPosZ;

                    m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].X.ToString("f03")}";
                    m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].Y.ToString("f03")}";
                    m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].Z.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;

                case PosTeach.ePOS_JIG_COVER:
                    dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                    dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);
                    dPosZ = MSystem.GetCurrentPos(Axis.AXIS_Z1 + m_idx);

                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].Y = dPosY;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].Z = dPosZ;

                    m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].X.ToString("f03")}";
                    m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].Y.ToString("f03")}";
                    m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].Z.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;


                case PosTeach.ePOS_Z_SAFETY:
                    dPosZ = MSystem.GetCurrentPos(Axis.AXIS_Z1 + m_idx);
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Z = dPosZ;
                    m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Z.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;
                case PosTeach.ePOS_SCREW:
                    {
                        int _index = -1;
                        //int _LVCount = -1;

                        dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                        dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);
                        dPosZ = MSystem.GetCurrentPos(Axis.AXIS_Z1 + m_idx);

                        if (LV_LIST_SCREW.SelectedIndices.Count > 0)
                        {
                            _index = LV_LIST_SCREW.SelectedIndices[0];

                            LV_LIST_SCREW.Items[_index].SubItems[1].Text = $"{dPosX.ToString("f03")}";
                            LV_LIST_SCREW.Items[_index].SubItems[2].Text = $"{dPosY.ToString("f03")}";
                            LV_LIST_SCREW.Items[_index].SubItems[3].Text = $"{dPosZ.ToString("f03")}";

                            if (InforManager.Instance.IsVisionPoint == true)
                            {
                                dPosX += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                                dPosY += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                                //dPosZ += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_SCREW].Z;
                                dPosX -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                                dPosY -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                                //dPosZ -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_VISION].Z;
                                InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.VisionZ = dPosZ;
                            }
                            else
                            {
                                InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.Z = dPosZ;

                            }
                            InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.X = dPosX;
                            InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.Y = dPosY;

                            InforTeaching.Instance.SaveSettings();
                        }
                    }
                    break;
                default: break;
            }
        }

        void SaveDataXY()
        {
            if (!MSystem.IsOriginAll() && !MSystem.SIMULATION)
            {
                MSystem.MyMsgMemo("NO ORIGIN!!", "Error", msgButton.OK, msgIcon.Error);
                return;
            }
            if (MSystem.MyMsgMemo($"Do you want Save Position {((PosTeach)GetButtonSelected()).ToString()}?", "Question", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            if (MSystem.SIMULATION)
            {
                return;
            }
            // Save data
            var Result = Task.Run(async () =>
            {
                await Task.Delay(500);
                SavePosXY((PosTeach)GetButtonSelected());
                MoveUP();
                await Task.Delay(100);
                MSystem.MyMsgMemo("Saving Complete...!", "Thongbao", msgButton.OK, msgIcon.Infor);
                MSystem.KillMsgDisplay();

                //if (GetButtonSelected() == (int)PosTeach.ePOS_SCREW)
                //    MoveNextScrewPoint();
            });
            MSystem.MyMsgDisplay("Start Save Data.....\r\nWait a moment....");
        }

        private void MoveNextScrewPoint()
        {
            int _index = -1;
            int _LVCount = -1;
            _LVCount = LV_LIST_SCREW.Items.Count;
            if (LV_LIST_SCREW.SelectedIndices.Count > 0)
            {
                _index = LV_LIST_SCREW.SelectedIndices[0];
                _index = _index + 1;

                if (_index >= _LVCount)
                    _index = _LVCount - 1;

                LV_LIST_SCREW.SelectedItems.Clear();
                LV_LIST_SCREW.Items[_index].Selected = true;
                LV_LIST_SCREW.EnsureVisible(_index);
                SelectButton(m_btPoselect[(int)PosTeach.ePOS_SCREW]);
                if (LV_LIST_SCREW.Items.Count < _numRowsCheck)
                {
                    //LV_LIST_SCREW.Columns[6].Width = 88;
                }
                else
                {
                    //LV_LIST_SCREW.Columns[6].Width = 74;
                }
                MoveXY();
            }
            else
            {
                if (LV_LIST_SCREW.Items.Count > 0)
                {
                    MSystem.m_pListScrew.SelectIndex(LV_LIST_SCREW.Items.Count - 1);
                    if (LV_LIST_SCREW.Items.Count < _numRowsCheck)
                    {
                        //LV_LIST_SCREW.Columns[6].Width = 88;
                    }
                    else
                    {
                        //LV_LIST_SCREW.Columns[6].Width = 74;
                    }
                }

                SelectButton(m_btPoselect[(int)PosTeach.ePOS_SCREW]);
                MoveXY();
            }
        }

        void SaveAllZ()
        {

            if (MSystem.MyMsgMemo($"Do you want Save Current Z for all Point ? ", "Question", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            double _dPosZ = 0.0;

            // Save data
            var Result = Task.Run(async () =>
            {
                await Task.Delay(500);

                PosTeach _PosSelected = (PosTeach)GetButtonSelected();
                if (_PosSelected == PosTeach.ePOS_MASTER_SCREW || _PosSelected == PosTeach.ePOS_MASTER_VISION ||
                   _PosSelected == PosTeach.ePOS_READY || _PosSelected == PosTeach.ePOS_Z_SAFETY || _PosSelected == PosTeach.ePOS_PICKUP ||
                   _PosSelected == PosTeach.ePOS_TRASH)
                {
                    MSystem.MyMsgMemo("Select Point Screw before Save", "Error", msgButton.OK, msgIcon.Error);
                    MSystem.KillMsgDisplay();
                    return;
                }
                if (LV_LIST_SCREW.SelectedIndices.Count > 0)
                {
                    if (InforManager.Instance.IsVisionPoint == true)
                        _dPosZ = InforTeaching.Instance.P_Screw[m_idx][LV_LIST_SCREW.SelectedIndices[0]].PScrew.VisionZ;
                    else
                        _dPosZ = InforTeaching.Instance.P_Screw[m_idx][LV_LIST_SCREW.SelectedIndices[0]].PScrew.Z;
                }
                for (int _i = 0; _i < InforTeaching.Instance.P_Screw[m_idx].Count; _i++)
                {
                    if (InforManager.Instance.IsVisionPoint == true)
                        InforTeaching.Instance.P_Screw[m_idx][_i].PScrew.VisionZ = _dPosZ;
                    else
                        InforTeaching.Instance.P_Screw[m_idx][_i].PScrew.Z = _dPosZ;
                }
                InforTeaching.Instance.SaveSettings();
                MSystem.m_pListScrew.LoadDataScrew(m_idx);
                await Task.Delay(100);
                MSystem.KillMsgDisplay();
            });
            MSystem.MyMsgDisplay("Start Save Data.....\r\nWait a moment....");
        }
        void SavePosXY(PosTeach _PosSelected)
        {
            double dPosX = 0;
            double dPosY = 0;
            double dPosZ = 0;

            switch (_PosSelected)
            {
                case PosTeach.ePOS_MASTER_SCREW:
                    dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                    dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);

                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y = dPosY;

                    m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X.ToString("f03")}";
                    m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;

                case PosTeach.ePOS_MASTER_VISION:
                    dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                    dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);

                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y = dPosY;

                    m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X.ToString("f03")}";
                    m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;

                case PosTeach.ePOS_READY:
                    dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                    dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);

                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Y = dPosY;

                    m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].X.ToString("f03")}";
                    m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Y.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;

                case PosTeach.ePOS_TRASH:
                    dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                    dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);

                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].Y = dPosY;

                    m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].X.ToString("f03")}";
                    m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_TRASH].Y.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;

                case PosTeach.ePOS_PICKUP:
                    dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                    dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);

                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].Y = dPosY;

                    m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].X.ToString("f03")}";
                    m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_PICKUP].Y.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;

                case PosTeach.ePOS_Z_SAFETY:
                    dPosZ = MSystem.GetCurrentPos(Axis.AXIS_Z1 + m_idx);
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Z = dPosZ;
                    m_btTargetPos[2].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Z.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;

                case PosTeach.ePOS_SCREW:
                    {
                        int _index = -1;
                        //int _LVCount = -1;

                        dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                        dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);

                        if (LV_LIST_SCREW.SelectedIndices.Count > 0)
                        {
                            _index = LV_LIST_SCREW.SelectedIndices[0];

                            LV_LIST_SCREW.Items[_index].SubItems[1].Text = $"{dPosX.ToString("f03")}";
                            LV_LIST_SCREW.Items[_index].SubItems[2].Text = $"{dPosY.ToString("f03")}";

                            if (InforManager.Instance.IsVisionPoint == true)
                            {
                                dPosX += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                                dPosY += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                                dPosX -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                                dPosY -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                            }

                            InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.X = dPosX;
                            InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.Y = dPosY;

                            InforTeaching.Instance.SaveSettings();
                        }
                    }
                    break;

                case PosTeach.ePOS_BARCODE:
                    dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                    dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);

                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].Y = dPosY;

                    m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].X.ToString("f03")}";
                    m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].Y.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;

                case PosTeach.ePOS_JIG_COVER:
                    dPosX = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
                    dPosY = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);

                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].X = dPosX;
                    InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].Y = dPosY;

                    m_btTargetPos[0].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].X.ToString("f03")}";
                    m_btTargetPos[1].Caption = $"{InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].Y.ToString("f03")}";

                    InforTeaching.Instance.SaveSettings();
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region List View Tool
        private void LV_LIST_SCREW_MouseClick(object sender, MouseEventArgs e)
        {
            int _IValue = 0;
            //double _DValue = 0;
            ListView listView = sender as ListView;
            ListViewHitTestInfo hit = listView.HitTest(e.X, e.Y);

            if (hit.Item != null)
            {
                int rowIndex = hit.Item.Index;
                int columnIndex = hit.Item.SubItems.IndexOf(hit.SubItem);
                SelectButton(m_btPoselect[(int)PosTeach.ePOS_SCREW]);
                if(columnIndex == 9) // FasNumber
                {

                    if (MSystem.GetInLV(rowIndex, columnIndex, ref _IValue))
                    {
                        //MSystem.m_pListScrew.UpdateListViewCell(rowIndex, columnIndex, $"{_IValue}");
                        InforTeaching.Instance.P_Screw[m_idx][rowIndex].FasNumbers = _IValue;
                        //TimUpdateData.Start();
                    }
                    MSystem.m_pListScrew.LoadDataScrew(m_idx);
                    MSystem.m_pListScrew.SelectIndex(rowIndex);

                    //InforTeaching.Instance.P_Screw[m_idx][rowIndex].Light = 0;
                    //MSystem.m_pListScrew.LoadDataScrew(m_idx);
                    //MSystem.m_pListScrew.SelectIndex(rowIndex);
                }
                else if (columnIndex == 6) // Chanel
                {

                    if (MSystem.GetInLV(rowIndex, columnIndex, ref _IValue))
                    {
                        //MSystem.m_pListScrew.UpdateListViewCell(rowIndex, columnIndex, $"{_IValue}");
                        if (_IValue > 3)
                            _IValue = 3;
                        if (_IValue == 0)
                            _IValue = 1;
                        InforTeaching.Instance.P_Screw[m_idx][rowIndex].Channel = _IValue;
                        //TimUpdateData.Start();
                    }
                    MSystem.m_pListScrew.LoadDataScrew(m_idx);
                    MSystem.m_pListScrew.SelectIndex(rowIndex);
                }
                else if (columnIndex == 1)
                {
                    //if (MSystem.GetDoubleLV(rowIndex, columnIndex, ref _DValue))
                    //{
                    //    InforTeaching.Instance.P_Screw[m_idx][rowIndex].PScrew.X = _DValue;
                    //}
                    //MSystem.m_pListScrew.LoadDataScrew(m_idx);
                    //MSystem.m_pListScrew.SelectIndex(rowIndex);
                }
                else if (columnIndex == 2)
                {
                    //if (MSystem.GetDoubleLV(rowIndex, columnIndex, ref _DValue))
                    //{
                    //    InforTeaching.Instance.P_Screw[m_idx][rowIndex].PScrew.Y = _DValue;
                    //}
                    //MSystem.m_pListScrew.LoadDataScrew(m_idx);
                    //MSystem.m_pListScrew.SelectIndex(rowIndex);
                }
                else if (columnIndex == 3)
                {
                    //if (MSystem.GetDoubleLV(rowIndex, columnIndex, ref _DValue))
                    //{
                    //    InforTeaching.Instance.P_Screw[m_idx][rowIndex].PScrew.Z = _DValue;
                    //}
                    //MSystem.m_pListScrew.LoadDataScrew(m_idx);
                    //MSystem.m_pListScrew.SelectIndex(rowIndex);
                }
            }
        }

        private void BT_First_Point_ClickEvent(object sender, EventArgs e)
        {
            if (LV_LIST_SCREW.Items.Count > 0)
            {
                if (PositionListPage == LV_LIST_SCREW.Items.Count - 1)
                {
                    PositionListPage -= 11;
                }
                else
                    PositionListPage -= 6;
                if (PositionListPage < 0)
                {
                    PositionListPage = 0;
                }
                LV_LIST_SCREW.SelectedItems.Clear();
                LV_LIST_SCREW.Items[PositionListPage].Selected = true;
                LV_LIST_SCREW.EnsureVisible(PositionListPage);
                SelectButton(m_btPoselect[(int)PosTeach.ePOS_SCREW]);
            }
        }
        private void BT_End_Point_ClickEvent(object sender, EventArgs e)
        {
            if (LV_LIST_SCREW.Items.Count > 0)
            {
                PositionListPage += 11;
                if (PositionListPage > LV_LIST_SCREW.Items.Count)
                {
                    PositionListPage = LV_LIST_SCREW.Items.Count - 1;
                }
                LV_LIST_SCREW.SelectedItems.Clear();
                LV_LIST_SCREW.Items[PositionListPage].Selected = true;//LV_LIST_SCREW.Items[LV_LIST_SCREW.Items.Count - 1].Selected = true;
                LV_LIST_SCREW.EnsureVisible(PositionListPage);//(LV_LIST_SCREW.Items.Count - 1);
                SelectButton(m_btPoselect[(int)PosTeach.ePOS_SCREW]);
            }
        }
        private void BT_Preview_Point_ClickEvent(object sender, EventArgs e)
        {
            int _index = -1;
            int _LVCount = -1;
            _LVCount = LV_LIST_SCREW.Items.Count;
            if (LV_LIST_SCREW.SelectedIndices.Count > 0)
            {
                _index = LV_LIST_SCREW.SelectedIndices[0];
                _index = _index - 1;
                if (_index <= 0)
                    _index = 0;
                LV_LIST_SCREW.SelectedItems.Clear();
                LV_LIST_SCREW.Items[_index].Selected = true;
                LV_LIST_SCREW.EnsureVisible(_index);
                SelectButton(m_btPoselect[(int)PosTeach.ePOS_SCREW]);
                if (LV_LIST_SCREW.Items.Count < _numRowsCheck)
                {
                    LV_LIST_SCREW.Columns[6].Width = 88;
                }
                else
                {
                    LV_LIST_SCREW.Columns[6].Width = 74;
                }
                MoveXY();
            }
            else
            {
                if (LV_LIST_SCREW.Items.Count > 0)
                {
                    if (LV_LIST_SCREW.Items.Count < _numRowsCheck)
                    {
                        LV_LIST_SCREW.Columns[6].Width = 88;
                    }
                    else
                    {
                        LV_LIST_SCREW.Columns[6].Width = 74;
                    }
                    MSystem.m_pListScrew.SelectIndex(0);
                }

                SelectButton(m_btPoselect[(int)PosTeach.ePOS_SCREW]);
                MoveXY();
            }
        }

        private void BT_Next_Point_ClickEvent(object sender, EventArgs e)
        {
            MoveNextScrewPoint();
        }

        private void LV_LIST_SCREW_MouseDown(object sender, MouseEventArgs e)
        {
            
            ListView listView = sender as ListView;
            ListViewHitTestInfo hit = listView.HitTest(e.X, e.Y);
            //listView.BeginUpdate();
            if (hit.Item != null)
            {
                int rowIndex = hit.Item.Index;
                int columnIndex = hit.Item.SubItems.IndexOf(hit.SubItem);
                if (columnIndex == 4) // Skip
                {
                    if (InforTeaching.Instance.P_Screw[m_idx][rowIndex].Skip)
                        InforTeaching.Instance.P_Screw[m_idx][rowIndex].Skip = false;
                    else
                        InforTeaching.Instance.P_Screw[m_idx][rowIndex].Skip = true;
                }
                else if (columnIndex == 7) // Vision
                {
                    if (InforTeaching.Instance.P_Screw[m_idx][rowIndex].Vision)
                        InforTeaching.Instance.P_Screw[m_idx][rowIndex].Vision = false;
                    else
                        InforTeaching.Instance.P_Screw[m_idx][rowIndex].Vision = true;
                }
                else if (columnIndex == 8) // Retry
                {
                    if (InforTeaching.Instance.P_Screw[m_idx][rowIndex].Retry)
                        InforTeaching.Instance.P_Screw[m_idx][rowIndex].Retry = false;
                    else
                        InforTeaching.Instance.P_Screw[m_idx][rowIndex].Retry = true;
                }
                //
                else if (columnIndex == 1 || columnIndex == 2 || columnIndex == 3|| columnIndex == 8 || columnIndex == 9)
                {

                }
                InforTeaching.Instance.SaveSettings();
            }
            else
            {

                if (LV_LIST_SCREW.SelectedIndices.Count > 0)
                {
                    _indexRe = LV_LIST_SCREW.SelectedIndices[0];
                    MSystem.m_pListScrew.SelectIndex(_indexRe);
                }
            }
            listView.Refresh();
        }

        private void LV_LIST_SCREW_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LV_LIST_SCREW.SelectedIndices.Count > 0)
            {
                try
                {
                    int selectedIndex = LV_LIST_SCREW.SelectedIndices[0];
                    MSystem.m_pListScrew.m_iCuIndex = selectedIndex;
                    if (LV_LIST_SCREW.SelectedIndices.Count > 0)
                    {
                        if (this.Visible)
                        {
                            SelectButton(m_btPoselect[(int)PosTeach.ePOS_SCREW]);
                        }
                    }
                    else
                    {
                        if (LV_LIST_SCREW.Items.Count > 0)
                        {
                            MSystem.m_pListScrew.SelectIndex(_indexRe);
                        }
                    }
                    //MessageBox.Show("Selected index: " + selectedIndex.ToString());
                }
                catch (ArgumentOutOfRangeException)
                {
                    //MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                //MessageBox.Show("No item is selected.");
            }
        }

        private void LV_LIST_SCREW_MouseMove(object sender, MouseEventArgs e)
        {
            if (LV_LIST_SCREW.Items.Count < _numRowsCheck)
            {
                //LV_LIST_SCREW.Columns[6].Width = 88;
            }
            else
            {
                int i = LV_LIST_SCREW.Columns.Count;
                //LV_LIST_SCREW.Columns[6].Width = 74;
            }
        }
        private void LV_LIST_SCREW_MouseUp(object sender, MouseEventArgs e)
        {
            if (LV_LIST_SCREW.Items.Count > _indexRe && LV_LIST_SCREW.SelectedIndices.Count <= 0)
            {
                MSystem.m_pListScrew.SelectIndex(_indexRe);
            }
        }
        #endregion

        #region JOG
        public void LoadJogModeDisplay()
        {
            if (MSystem.m_pTrsJog._JogMode == 0 && (BT_SPEED.Caption != "LOW" || BT_SPEEDZ.Caption != "LOW"))
            {
                BT_SPEED.Caption = "LOW";
                BT_SPEEDZ.Caption = "LOW";
            }
            else if (MSystem.m_pTrsJog._JogMode == 1 && (BT_SPEED.Caption != "MID" || BT_SPEEDZ.Caption != "MID"))
            {
                BT_SPEED.Caption = "MID";
                BT_SPEEDZ.Caption = "MID";
            }
            else if (MSystem.m_pTrsJog._JogMode == 2 && (BT_SPEED.Caption != "HIGHT" || BT_SPEEDZ.Caption != "HIGHT"))
            {
                BT_SPEED.Caption = "HIGHT";
                BT_SPEEDZ.Caption = "HIGHT";
            }
        }
        private void JogActive(object sender, AxBTNENHLib4._DBtnEnhEvents_MouseDownEvent e)
        {
            m_iPress = true;
            JogRemember = true;
            switch ((sender as AxBTNENHLib4.AxBtnEnh).Name.ToString())
            {
                case "BT_Y_JOG_UP":
                    MSystem.m_pTrsJog.FlagY1 = true;
                    if (InforManager.Instance.IsYAxisReverce == false)
                        JogYMinus();
                    else
                        JogYPlus();
                    break;

                case "BT_Y_JOG_DOWN":
                    MSystem.m_pTrsJog.FlagX1 = true;
                    if (InforManager.Instance.IsYAxisReverce == false)
                        JogYPlus();
                    else
                        JogYMinus();

                    break;

                case "BT_X_JOG_LEFT":
                    MSystem.m_pTrsJog.FlagY1 = true;
                    JogXPlus();
                    break;

                case "BT_X_JOG_RIGHT":
                    MSystem.m_pTrsJog.FlagY1 = true;
                    JogXMinus();
                    break;

                case "BT_Z_JOG_UP":
                    JogZMinus();
                    IsJogZUp = true;
                    MSystem.m_pTrsJog.FlagZ1 = true;
                    break;

                case "BT_Z_JOG_DOWN":
                    JogZPlus();
                    IsJogZUp = false;
                    MSystem.m_pTrsJog.FlagZ1 = true;
                    break;

                default: break;
            }
        }
        private void JogDisable(object sender, AxBTNENHLib4._DBtnEnhEvents_MouseUpEvent e)
        {
            m_iPress = false;
            //if (JogRemember)
            //{
            //    JogRemember = false;
            //    DisableJog();
            //}
        }
        private void MouseOut(object sender, EventArgs e)
        {
            m_iPress = false;
            //if (JogRemember)
            //{
            //    JogRemember = false;
            //    //DisableJog();
            //}
        }
        void DisableJog()
        {
            try
            {
                MSystem.SetAllStop();
            }
            catch (Exception)
            {
                MSystem.MyMsgMemo("Reconnect to servo Motor!", "Servo Error", msgButton.eBtMax, msgIcon.eMessgMax);
            }
            finally
            {
                MSystem.m_pTrsJog.FlagX1 = MSystem.m_pTrsJog.FlagY1 = MSystem.m_pTrsJog.FlagZ1 = MSystem.m_pTrsJog.FlagJ1 = false;
                MSystem.m_pTrsJog.FlagX2 = MSystem.m_pTrsJog.FlagY2 = MSystem.m_pTrsJog.FlagZ2 = MSystem.m_pTrsJog.FlagJ2 = false;
                MSystem.m_pTrsJog.FlagX11 = MSystem.m_pTrsJog.FlagY11 = MSystem.m_pTrsJog.FlagZ11 = MSystem.m_pTrsJog.FlagJ11 = false;
                MSystem.m_pTrsJog.FlagX22 = MSystem.m_pTrsJog.FlagY22 = MSystem.m_pTrsJog.FlagZ22 = MSystem.m_pTrsJog.FlagJ22 = false;
                MSystem.m_pTrsJog.FlagStop = true;
                for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
                {
                    MSystem.m_pTrsJog._CMDRun[i] = false;
                    MSystem.m_pTrsJog._indexDir[i] = false;
                }

            }
        }

        void JogTP()
        {
            if (JogRemember == false)
            {
                if (m_idx == Constants.left)
                {
                    if (MSystem.m_pTrsJog.FlagX1)
                    {
                        MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_X1] = true;
                        MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_X1] = true;
                    }
                    else if (MSystem.m_pTrsJog.FlagX11)
                    {
                        MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_X1] = true;
                        MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_X1] = false;
                    }

                    if (MSystem.m_pTrsJog.FlagY1)
                    {
                        MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Y1] = true;
                        if (InforManager.Instance.IsYAxisReverce == false)
                            MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Y1] = true;
                        else
                            MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Y1] = false;
                        //MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Y1] = true;
                    }
                    else if (MSystem.m_pTrsJog.FlagY11)
                    {
                        MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Y1] = true;
                        if (InforManager.Instance.IsYAxisReverce == false)
                            MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Y1] = false;

                        else
                            MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Y1] = true;
                        //MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Y1] = false;
                    }

                    if (MSystem.m_pTrsJog.FlagZ1)
                    {
                        IsJogZUp = true;
                        MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Z1] = true;
                        MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Z1] = false;
                    }
                    else if (MSystem.m_pTrsJog.FlagZ11)
                    {
                        IsJogZUp = false;
                        if (MSystem.GetCurrentPos(Axis.AXIS_Z1) >= InforManager.Instance.m_dLimitZPlus)
                        // CHECK LIMIT Z+
                        {
                            JogRemember = false;
                            DisableJog();
                            MSystem.SetAllStop();
                            MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Z1] = false;
                            MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Z1] = true;
                            MSystem.MyMsgMemo("Check Limit Z+ Fail", "Alarm", msgButton.OK, msgIcon.Error);
                            MSystem.KillMsgDisplay();
                            return;
                        }
                        else
                        {
                            MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Z1] = true;
                            MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Z1] = true;
                        }
                    }
                }

                if (m_idx == Constants.right)
                {
                    if (MSystem.m_pTrsJog.FlagX2)
                    {
                        MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_X2] = true;
                        MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_X2] = false;
                    }
                    else if (MSystem.m_pTrsJog.FlagX22)
                    {
                        MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_X2] = true;
                        MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_X2] = true;
                    }

                    if (MSystem.m_pTrsJog.FlagY2)
                    {
                        MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Y2] = true;
                        if (InforManager.Instance.IsYAxisReverce == false)
                            MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Y2] = true;
                        else
                            MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Y2] = false;
                    }
                    else if (MSystem.m_pTrsJog.FlagY22)
                    {
                        MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Y2] = true;
                        if (InforManager.Instance.IsYAxisReverce == false)
                            MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Y2] = false;
                        else
                            MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Y2] = true;
                    }

                    if (MSystem.m_pTrsJog.FlagZ2)
                    {
                        IsJogZUp = true;
                        MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Z2] = true;
                        MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Z2] = false;
                    }
                    else if (MSystem.m_pTrsJog.FlagZ22)
                    {
                        IsJogZUp = false;
                        if (MSystem.GetCurrentPos(Axis.AXIS_Z2) >= InforManager.Instance.m_dLimitZPlus)
                        // CHECK LIMIT Z+
                        {
                            JogRemember = false;
                            DisableJog();
                            MSystem.SetAllStop();
                            MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Z2] = false;
                            MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Z2] = true;
                            MSystem.MyMsgMemo("Check Limit Z+ Fail", "Alarm", msgButton.OK, msgIcon.Error);
                            MSystem.KillMsgDisplay();
                            return;
                        }
                        else
                        {
                            MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Z2] = true;
                            MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Z2] = true;
                        }
                    }
                }
            }
        }

        void JogXPlus()
        {
            if (m_idx == Constants.left)
            {
                MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_X1] = true;
                MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_X1] = false;
            }
            else
            {
                MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_X2] = true;
                MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_X2] = true;
            }
        }

        void JogXMinus()
        {          
            if (m_idx == Constants.left)
            {
                MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_X1] = true;
                MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_X1] = true;
            }
            else
            {
                MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_X2] = true;
                MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_X2] = false;
            }
        }

        void JogYPlus()
        {
            //if (MSystem.m_pTrsJig[m_idx].IsFixDownSol() == true) return;        

            if (m_idx == Constants.left)
            {
                MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Y1] = true;
                MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Y1] = true;
            }
            else
            {
                MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Y2] = true;
                MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Y2] = true;
            }
        }

        void JogYMinus()
        {
            //if (MSystem.m_pTrsJig[m_idx].IsFixDownSol() == true) return;

            if (m_idx == Constants.left)
            {
                MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Y1] = true;
                MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Y1] = false;
            }
            else
            {
                MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Y2] = true;
                MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Y2] = false;
            }
        }

        void JogZPlus()
        {
            if(m_idx == Constants.left)
            {
                if (MSystem.GetCurrentPos(Axis.AXIS_Z1) >= InforManager.Instance.m_dLimitZPlus)
                // CHECK LIMIT Z+
                {
                    JogRemember = false;
                    DisableJog();
                    MSystem.SetAllStop();
                    MSystem.MyMsgMemo("Check Limit Z+ Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    MSystem.KillMsgDisplay();
                    return;
                }
            }
            else if (m_idx == Constants.right)
            {
                if (MSystem.GetCurrentPos(Axis.AXIS_Z2) >= InforManager.Instance.m_dLimitZPlus)
                // CHECK LIMIT Z+
                {
                    JogRemember = false;
                    DisableJog();
                    MSystem.SetAllStop();
                    MSystem.MyMsgMemo("Check Limit Z+ Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    MSystem.KillMsgDisplay();
                    return;
                }
            }
            if (m_idx == Constants.left)
            {
                MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Z1] = true;
                MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Z1] = true;
            }
            else
            {
                MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Z2] = true;
                MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Z2] = true;
            }
        }
        void JogZMinus()
        {
            if (m_idx == Constants.left)
            {
                MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Z1] = true;
                MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Z1] = false;
            }
            else
            {
                MSystem.m_pTrsJog._CMDRun[(int)Axis.AXIS_Z2] = true;
                MSystem.m_pTrsJog._indexDir[(int)Axis.AXIS_Z2] = false;
            }
        }
        #endregion

        #region Servo Move
        void MoveXY()
        {
            IsJogZUp = true;
            if (MSystem.IsOriginAll() == false)
            {
                MSystem.MyMsgMemo("NO ORIGIN!!", "Error", msgButton.OK, msgIcon.Error);
                return;
            }

            if (MSystem.IsDetectDoorOpen(out string m) == true)
            {
                //MSystem.MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                MSystem.MySafetyAlarm();
                return;
            }

            if (MSystem.IsDetectEmergency() == true)
            {
                MSystem.MyMsgMemo("EMG Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                return;
            }
            if (MSystem.IsLightCurtainDetected() == true)
            {
                MSystem.MyMsgMemo("Light Curtain Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                return;
            }
            if (MSystem.m_pTrsJig[m_idx].IsCoverDownFront()&& MSystem.m_pTrsJig[m_idx].IsDetectCover()&& MSystem.m_pTrsJig[m_idx].IsFixUPSol())
            {
                MSystem.MyMsgMemo("Please Check Cover Status!", "Alarm", msgButton.OK, msgIcon.Error);
                return;
            }


            /******************************************************************************************************************/
            double _dX = 0.0;
            double _dY = 0.0;
            double _dZ = 0.0;

            PosTeach _PosMove = (PosTeach)GetButtonSelected();

            if (_PosMove == PosTeach.ePOS_MASTER_SCREW)
            {
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_MASTER_VISION)
            {
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_READY)
            {
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Y;
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].X;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_TRASH)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_TRASH].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_TRASH].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_PICKUP)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_PICKUP].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_PICKUP].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_Z_SAFETY)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_Z_SAFETY].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_SCREW)
            {
                int _index = 0;
                if (LV_LIST_SCREW.SelectedIndices.Count > 0)
                {
                    _index = LV_LIST_SCREW.SelectedIndices[0];
                    _dX = InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.X;
                    _dY = InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.Y;
                    _dZ = InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Z;

                    if (InforManager.Instance.IsVisionPoint == true)
                    {
                        _dX += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                        _dY += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                        _dX -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                        _dY -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                    }
                }
                else
                {
                    MSystem.MyMsgMemo("Position Screw Select Not Correct", "Error", msgButton.OK, msgIcon.Error);
                    return;
                }
            }
            else if (_PosMove == PosTeach.ePOS_BARCODE) //Insert khjo
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_BARCODE].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_BARCODE].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            //else if (_PosMove == PosTeach.ePOS_VISION_TEST)
            //{
            //    if (MSystem.MyMsgMemo($"Are you want Move XYZ To Position {((PosTeach)GetButtonSelected()).ToString()}?", "Question", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
            //        return;
            //    MSystem.m_pTrsScrew[m_idx].VisionAlign(); //await MSystem.m_pTrsScrew[m_idx].VisionAlignStart();
            //    return;
            //}
            else if (_PosMove == PosTeach.ePOS_JIG_COVER)
            {
                //수동조작 인터락 추가...khjo
                if (MSystem.m_pTrsJig[m_idx].IsSleeveDetect() == true && MSystem.m_pTrsJig[m_idx].IsCoverDownFront() == true) return;
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_COVER].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_COVER].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else
            {
                MSystem.MyMsgMemo("Position Select Not Correct", "Error", msgButton.OK, msgIcon.Error);
                return;
            }
            //Start Move
            var Result = Task.Run(() =>
            {
                if (m_idx == (int)_NSC.eLEFT)
                    MSystem.SetMsgDisplay("Start Move Screw Left ....");
                else
                    MSystem.SetMsgDisplay("Start Move Screw Right ....");

                Thread.Sleep(100);

                TimCheck.StartTimer();

                //Z-UP
                int _result2 = 0;
                int _result = MSystem.m_pTrsScrew[(int)_NSC.eLEFT + m_idx].MovePosition(Axis.AXIS_Z1 + m_idx, _dZ);
                if (_result != MSystem.MMC_OK)
                {
                    if (m_idx == (int)_NSC.eLEFT)
                        MSystem.MyMsgMemo("Screw Left Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    else
                        MSystem.MyMsgMemo("Screw Right Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);

                    MSystem.KillMsgDisplay();
                    return;
                }

                TimCheck.StartTimer();
                while (true)
                {
                    if (MSystem.m_pTrsScrew[(int)_NSC.eLEFT + m_idx].IsMoveComplete(Axis.AXIS_Z1 + m_idx, _dZ))
                    {
                        break;
                    }
                    if (TimCheck.MoreThan(6.0) || MSystem.IsDetectDoorOpen(out _) || MSystem.IsDetectEmergency() || MSystem.IsLightCurtainDetected())
                    {
                        MSystem.MoveStop(Axis.AXIS_Z1 + m_idx);
                        if (m_idx == (int)_NSC.eLEFT)
                            MSystem.MyMsgMemo("Screw Left Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        else
                            MSystem.MyMsgMemo("Screw Right Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        MSystem.KillMsgDisplay();
                        return;
                    }
                }
                if (MSystem.m_pTrsJig[m_idx].IsUp() == false)
                {
                    MSystem.m_pTrsJig[m_idx].Up();
                }
                /*******************************************************************************************/
                //Start Move
                _result = MSystem.SetMove((int)Axis.AXIS_X1 + m_idx, _dX);
                if (_PosMove == PosTeach.ePOS_PICKUP || _PosMove == PosTeach.ePOS_TRASH)
                {
                    _result2 = MSystem.MMC_OK;
                }
                else
                {
                    if (InforManager.Instance.InspectionType == "Mobile")
                    {
                        if (MSystem.m_pTrsJig[m_idx].IsUp() == false)
                        {
                            MSystem.m_pTrsJig[m_idx].Up();
                        }
                    }
                    
                    _result2 = MSystem.SetMove((int)Axis.AXIS_Y1 + m_idx, _dY);
                }
                if (_result != MSystem.MMC_OK || _result2 != MSystem.MMC_OK)
                {
                    if (m_idx == (int)_NSC.eLEFT)
                        MSystem.MyMsgMemo("Screw Left Move XY Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    else
                        MSystem.MyMsgMemo("Screw Right Move XY Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    MSystem.KillMsgDisplay();
                    return;
                }

                TimCheck.StartTimer();
                while (true)
                {
                    if (MSystem.m_pTrsJig[(int)_NSC.eLEFT + m_idx].IsMoveComplete(Axis.AXIS_X1 + m_idx, _dX) &&
                        (MSystem.m_pTrsJig[(int)_NSC.eLEFT + m_idx].IsMoveComplete(Axis.AXIS_Y1 + m_idx, _dY) || _PosMove == PosTeach.ePOS_PICKUP || _PosMove == PosTeach.ePOS_TRASH))
                    {
                        if (_PosMove != PosTeach.ePOS_PICKUP && _PosMove != PosTeach.ePOS_TRASH)
                        {
                            //if (InforManager.Instance.InspectionType == "H8")
                            //{
                            //    if (MSystem.m_pTrsJig[m_idx].IsUnCentering() == true)
                            //    {
                            //        MSystem.m_pTrsJig[m_idx].Centering();
                            //    }
                            //}
                            if (InforManager.Instance.InspectionType == "Mobile")
                            {
                                if (_PosMove == PosTeach.ePOS_READY)
                                {
                                    if (MSystem.m_pTrsJig[m_idx].IsFwdSol())
                                        MSystem.m_pTrsJig[m_idx].Bwd();
                                    break;
                                }
                                else
                                {
                                    if (MSystem.m_pTrsJig[m_idx].IsFwdSol() == false)
                                    {
                                        MSystem.m_pTrsJig[m_idx].Fwd();
                                    }
                                    if (MSystem.m_pTrsJig[m_idx].IsFwd() == true)
                                    {
                                        MSystem.m_pTrsJig[m_idx].Down();
                                        break;
                                    }
                                }
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                    if (TimCheck.MoreThan(6.0) || MSystem.IsDetectDoorOpen(out _) || MSystem.IsDetectEmergency() || MSystem.IsLightCurtainDetected())
                    {
                        MSystem.MoveStop(Axis.AXIS_X1 + m_idx);
                        MSystem.MoveStop(Axis.AXIS_Y1 + m_idx);
                        string index = m_idx == 0 ? "Left" : "Right";
                        string door = string.Empty;
                        if (MSystem.IsDetectDoorOpen(out door))
                            MSystem.MyMsgMemo($"Screw {index} Move XY Fail ( {door} )", "Alarm", msgButton.OK, msgIcon.Error);
                        else if (MSystem.IsDetectEmergency())
                            MSystem.MyMsgMemo($"Screw {index} Move XY Fail ( EMO Detect )", "Alarm", msgButton.OK, msgIcon.Error);
                        else if (MSystem.IsLightCurtainDetected())
                            MSystem.MyMsgMemo($"Screw {index} Move XY Fail ( Light Curtain Detect )", "Alarm", msgButton.OK, msgIcon.Error);
                        else
                            MSystem.MyMsgMemo($"Screw {index} Move XY Fail", "Alarm", msgButton.OK, msgIcon.Error);

                        MSystem.KillMsgDisplay();
                        return;
                    }
                    Thread.Sleep(10);
                }
                /*******************************************************************************************/
                MSystem.KillMsgDisplay();
            });
            MSystem.MyMsgDisplay("Start Move to Position Select .....\r\nWait a moment....");
        }
        void MoveXYZ()
        {
            IsJogZUp = true;
            if (MSystem.SIMULATION) return;
            if (!MSystem.IsOriginAll() && !MSystem.SIMULATION)
            {
                MSystem.MyMsgMemo("NO ORIGIN!!", "Error", msgButton.OK, msgIcon.Error);
                return;
            }
            if (MSystem.IsDetectDoorOpen(out string m) == true)
            {
               // MSystem.MyMsgMemo(m, "Origin Error", msgButton.OK, msgIcon.Error);
                return;
            }
            if (MSystem.IsDetectEmergency())
            {
                MSystem.MyMsgMemo("EMG Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                return;
            }
            if (MSystem.IsLightCurtainDetected() == true)
            {
                MSystem.MyMsgMemo("Light Curtain Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                return;
            }
            if (MSystem.m_pTrsJig[m_idx].IsCoverDownFront() && MSystem.m_pTrsJig[m_idx].IsDetectCover() && MSystem.m_pTrsJig[m_idx].IsFixUPSol())
            {
                MSystem.MyMsgMemo("Please Check Cover Status!", "Alarm", msgButton.OK, msgIcon.Error);
                return;
            }



            /******************************************************************************************************************/
            double _dX = 0.0;
            double _dY = 0.0;
            double _dZ = 0.0;
            double _dZReady = 0.0;
            PosTeach _PosMove = (PosTeach)GetButtonSelected();

            if (_PosMove == PosTeach.ePOS_MASTER_SCREW)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Z;
            }
            else if (_PosMove == PosTeach.ePOS_MASTER_VISION)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Z;
            }
            else if (_PosMove == PosTeach.ePOS_READY)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
                _dZReady = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_TRASH)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_TRASH].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_TRASH].Z;
                _dZReady = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_PICKUP)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_PICKUP].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_PICKUP].Z;
                _dZReady = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_Z_SAFETY)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_Z_SAFETY].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Z;
                _dZReady = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_SCREW)
            {
                int _index = 0;
                if (LV_LIST_SCREW.SelectedIndices.Count > 0)
                {
                    _index = LV_LIST_SCREW.SelectedIndices[0];
                    _dX = InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.X;
                    _dY = InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.Y;
                    _dZ = InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.Z;
                    _dZReady = InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Z;

                    if (InforManager.Instance.IsVisionPoint == true)
                    {
                        _dX += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                        _dY += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                        //_dZ += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_VISION].Z;
                        _dX -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                        _dY -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                        //_dZ -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_SCREW].Z;
                        _dZ = InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.VisionZ;
                    }
                }
                else
                {
                    MSystem.MyMsgMemo("Position Screw Select Not Correct", "Error", msgButton.OK, msgIcon.Error);
                    return;
                }
            }
            else if (_PosMove == PosTeach.ePOS_BARCODE)
            {
                _dX = InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].X;
                _dY = InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_BARCODE].Z;
                _dZReady = InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_JIG_COVER)
            {
                _dX = InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].X;
                _dY = InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_COVER].Z;
                _dZReady = InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else
            {
                MSystem.MyMsgMemo("Position Select Not Correct", "Error", msgButton.OK, msgIcon.Error);
                return;
            }
            //Start Move
            var Result = Task.Run(async () =>
            {
                if (m_idx == (int)_NSC.eLEFT)
                    MSystem.SetMsgDisplay("Start Move Screw Left ....");
                else
                    MSystem.SetMsgDisplay("Start Move Screw Right ....");

                await Task.Delay(100);

                TimCheck.StartTimer();
                //Z-UP
                if (MSystem.m_pTrsScrew[(int)_NSC.eLEFT + m_idx].MovePosition(Axis.AXIS_Z1 + m_idx, _dZReady) != MSystem.MMC_OK)
                {

                    if (m_idx == (int)_NSC.eLEFT)
                        MSystem.MyMsgMemo("Screw Left Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    else
                        MSystem.MyMsgMemo("Screw Right Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);

                    MSystem.KillMsgDisplay();
                    return;
                }

                TimCheck.StartTimer();
                while (true)
                {
                    if (MSystem.m_pTrsScrew[(int)_NSC.eLEFT + m_idx].IsMoveComplete(Axis.AXIS_Z1 + m_idx, _dZReady))
                    {
                        break;
                    }
                    if (TimCheck.MoreThan(6.0) || MSystem.IsDetectDoorOpen(out _) || MSystem.IsDetectEmergency() || MSystem.IsLightCurtainDetected())
                    {
                        MSystem.MoveStop(Axis.AXIS_Z1 + m_idx);
                        if (m_idx == (int)_NSC.eLEFT)
                            MSystem.MyMsgMemo("Screw Left Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        else
                            MSystem.MyMsgMemo("Screw Right Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        MSystem.KillMsgDisplay();
                        return;
                    }
                }
                
                /*******************************************************************************************/
                //Start Move
                int _result = 0;
                int _result2 = 0;
                _result = MSystem.SetMove((int)Axis.AXIS_X1 + m_idx, _dX);
                if (_PosMove == PosTeach.ePOS_PICKUP || _PosMove == PosTeach.ePOS_TRASH)
                    _result2 = MSystem.MMC_OK;
                else
                {
                    if (InforManager.Instance.InspectionType == "Tablet" && InforManager.Instance.IsSmartKitUse == true)
                    {
                        //if (_PosMove != PosTeach.ePOS_JIG_COVER && _PosMove != PosTeach.ePOS_BARCODE && _PosMove != PosTeach.ePOS_Z_SAFETY && _PosMove != PosTeach.ePOS_READY)
                        //{
                        //    if (_PosMove == PosTeach.ePOS_READY)
                        //    {

                        //    }
                        //    else
                        //    {
                        //        MSystem.m_pTrsJig[m_idx].MoveCoverPosY();
                        //        TimCheck.StartTimer();
                        //        while (true)
                        //        {
                        //            if (MSystem.m_pTrsJig[m_idx].IsMoveDoneCoverPosY())
                        //            {
                        //                break;
                        //            }
                        //            else if (TimCheck.MoreThan(2.0))
                        //            {
                        //                MSystem.MyMsgMemo("Jig Cover Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        //                MSystem.KillMsgDisplay();
                        //                return;
                        //            }
                        //            Thread.Sleep(10);
                        //        }
                        //        MSystem.m_pTrsJig[m_idx].CoverDown();
                        //        TimCheck.StartTimer();
                        //        while (true)
                        //        {
                        //            if (MSystem.m_pTrsJig[m_idx].IsCoverDownFront())
                        //            {
                        //                break;
                        //            }
                        //            else if (TimCheck.MoreThan(2.0))
                        //            {
                        //                MSystem.MyMsgMemo("Jig Cover Down Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        //                MSystem.KillMsgDisplay();
                        //                return;
                        //            }
                        //            Thread.Sleep(10);
                        //        }
                        //        MSystem.m_pTrsJig[m_idx].FixDown();
                        //        TimCheck.StartTimer();
                        //        while (true)
                        //        {
                        //            if (MSystem.m_pTrsJig[m_idx].IsCoverDownFront())
                        //            {
                        //                break;
                        //            }
                        //            else if (TimCheck.MoreThan(2.0))
                        //            {
                        //                MSystem.MyMsgMemo("Jig Cover Down Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        //                MSystem.KillMsgDisplay();
                        //                return;
                        //            }
                        //            Thread.Sleep(10);
                        //        }
                        //    }
                        //}
                    }
                    else
                    {
                        if (MSystem.m_pTrsJig[m_idx].IsUp() == false)
                        {
                            MSystem.m_pTrsJig[m_idx].Up();
                        }
                    }
                    
                    _result2 = MSystem.SetMove((int)Axis.AXIS_Y1 + m_idx, _dY);
                }
                if (_result != MSystem.MMC_OK || _result2 != MSystem.MMC_OK)
                {

                    if (m_idx == (int)_NSC.eLEFT)
                        MSystem.MyMsgMemo("Screw Left Move XY Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    else
                        MSystem.MyMsgMemo("Screw Right Move XY Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    MSystem.KillMsgDisplay();
                    return;
                }

                TimCheck.StartTimer();
                while (true)
                {
                    if (MSystem.m_pTrsJig[m_idx].IsMoveComplete(Axis.AXIS_X1 + m_idx, _dX) &&
                        (MSystem.m_pTrsJig[m_idx].IsMoveComplete(Axis.AXIS_Y1 + m_idx, _dY) || _PosMove == PosTeach.ePOS_PICKUP || _PosMove == PosTeach.ePOS_TRASH))
                    {

                        if (_PosMove != PosTeach.ePOS_PICKUP && _PosMove != PosTeach.ePOS_TRASH)
                        {
                            if (InforManager.Instance.InspectionType == "Mobile")
                            {
                                if (_PosMove == PosTeach.ePOS_READY)
                                {
                                    if (MSystem.m_pTrsJig[m_idx].IsFwdSol())
                                        MSystem.m_pTrsJig[m_idx].Bwd();
                                    break;
                                }
                                else
                                {
                                    if (MSystem.m_pTrsJig[m_idx].IsFwdSol() == false)
                                    {
                                        MSystem.m_pTrsJig[m_idx].Fwd();
                                    }
                                    if (MSystem.m_pTrsJig[m_idx].IsFwd() == true)
                                    {
                                        MSystem.m_pTrsJig[m_idx].Down();
                                        break;
                                    }
                                }
                            }
                            else
                                break;
                        }
                        else
                            break;
                    }
                    if (TimCheck.MoreThan(6.0) || MSystem.IsDetectDoorOpen(out _) || MSystem.IsDetectEmergency() || MSystem.IsLightCurtainDetected())
                    {
                        MSystem.MoveStop(Axis.AXIS_X1 + m_idx);
                        MSystem.MoveStop(Axis.AXIS_Y1 + m_idx);
                        if (m_idx == (int)_NSC.eLEFT)
                            MSystem.MyMsgMemo("Screw Left Move XY Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        else
                            MSystem.MyMsgMemo("Screw Right Move XY Fail", "Alarm", msgButton.OK, msgIcon.Error);

                        MSystem.KillMsgDisplay();
                        return;
                    }
                }
                // CHECK LIMIT Z+
                if (m_idx == Constants.left)
                {
                    if (_dZ >= InforManager.Instance.m_dLimitZPlus)
                    {
                        MSystem.MyMsgMemo("Check Limit Z+ Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        MSystem.KillMsgDisplay();
                        return;
                    }
                }
                else if (m_idx == Constants.right)
                {
                    if (_dZ >= InforManager.Instance.m_dLimitZPlus)
                    {
                        MSystem.MyMsgMemo("Check Limit Z+ Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        MSystem.KillMsgDisplay();
                        return;
                    }
                }
                /*******************************************************************************************/
                //Z-Down'
                _dZ -= 5; 
                if (MSystem.m_pTrsScrew[m_idx].MovePosition(Axis.AXIS_Z1 + m_idx, _dZ) != MSystem.MMC_OK)
                {

                    if (m_idx == (int)_NSC.eLEFT)
                        MSystem.MyMsgMemo("Screw Left Z Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    else
                        MSystem.MyMsgMemo("Screw Right Z Fail", "Alarm", msgButton.OK, msgIcon.Error);

                    MSystem.KillMsgDisplay();
                    return;
                }

                TimCheck.StartTimer();
                while (true)
                {
                    if (MSystem.m_pTrsScrew[m_idx].IsMoveComplete(Axis.AXIS_Z1 + m_idx, _dZ))
                    {
                        break;
                    }
                    if (TimCheck.MoreThan(6.0) || MSystem.IsDetectDoorOpen(out _) || MSystem.IsDetectEmergency() || MSystem.IsLightCurtainDetected())
                    {
                        MSystem.MoveStop(Axis.AXIS_Z1 + m_idx);
                        if (m_idx == (int)_NSC.eLEFT)
                            MSystem.MyMsgMemo("Screw Left Z Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        else
                            MSystem.MyMsgMemo("Screw Right Z Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        MSystem.KillMsgDisplay();
                        return;
                    }
                }
                MSystem.KillMsgDisplay();
            });
            MSystem.MyMsgDisplay("Start Move to Position Select .....\r\nWait a moment....");
        }
        void MoveUP()
        {
            IsJogZUp = true;
            if (MSystem.SIMULATION) return;
            if (!MSystem.IsOriginAll() && !MSystem.SIMULATION)
            {
                MSystem.MyMsgMemo("NO ORIGIN!!", "Error", msgButton.OK, msgIcon.Error);
                return;
            }
            if (MSystem.IsDetectDoorOpen(out string m) == true)
            {
                //MSystem.MyMsgMemo(m, "Origin Error", msgButton.OK, msgIcon.Error);
                return;
            }
            if (MSystem.IsDetectEmergency())
            {
                MSystem.MyMsgMemo("EMG Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                return;
            }
            /******************************************************************************************************************/
            double _dZReady = 0.0;
            _dZReady = 0.0;// InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Z;

            var Result = Task.Run(async () =>
            {
                if (m_idx == (int)_NSC.eLEFT)
                    MSystem.SetMsgDisplay("Start Move Screw Left Z-UP....");
                else
                    MSystem.SetMsgDisplay("Start Move Screw Right Z-UP....");

                await Task.Delay(100);

                /*******************************************************************************************/
                //Z-UP
                int _result = MSystem.m_pTrsScrew[m_idx].MovePosition(Axis.AXIS_Z1 + m_idx, _dZReady);
                if (_result != MSystem.MMC_OK)
                {

                    if (m_idx == Constants.left)
                        MSystem.MyMsgMemo("Screw Left Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    else
                        MSystem.MyMsgMemo("Screw Right Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);

                    MSystem.KillMsgDisplay();
                    return;
                }

                TimCheck.StartTimer();
                while (true)
                {
                    if (MSystem.m_pTrsScrew[m_idx].IsMoveComplete(Axis.AXIS_Z1 + m_idx, _dZReady))
                    {
                        break;
                    }
                    if (TimCheck.MoreThan(6.0) || MSystem.IsDetectDoorOpen(out _) || MSystem.IsDetectEmergency())
                    {
                        if (m_idx == Constants.left)
                            MSystem.MyMsgMemo("Screw Left Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        else
                            MSystem.MyMsgMemo("Screw Right Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        MSystem.KillMsgDisplay();
                        return;
                    }
                }
                MSystem.KillMsgDisplay();
            });
            MSystem.MyMsgDisplay("Start Move to Position Select .....\r\nWait a moment....");

        }
        void MoveDown()
        {
            if (MSystem.IsOriginAll() == false)
            {
                MSystem.MyMsgMemo("NO ORIGIN!!", "Error", msgButton.OK, msgIcon.Error);
                return;
            }

            if (MSystem.IsDetectDoorOpen(out string m) == true)
            {
                //MSystem.MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                return;
            }

            if (MSystem.IsDetectEmergency())
            {
                MSystem.MyMsgMemo("EMG Detect!", "Error", msgButton.OK, msgIcon.Error);
                return;
            }
            /******************************************************************************************************************/
            double _dX = 0.0;
            double _dY = 0.0;
            double _dZ = 0.0;
            double _dZReady = 0.0;
            PosTeach _PosMove = (PosTeach)GetButtonSelected();

            if (_PosMove == PosTeach.ePOS_MASTER_SCREW)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Z;
                _dZReady = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_MASTER_VISION)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Z;
                _dZReady = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_READY)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Y;
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
                _dZReady = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_TRASH)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_TRASH].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_TRASH].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_TRASH].Z;
                _dZReady = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_PICKUP)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_PICKUP].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_PICKUP].Y;
                _dZ = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_PICKUP].Z;
                _dZReady = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_Z_SAFETY)
            {
                _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_Z_SAFETY].X;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Y;
                _dY = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Z;
                _dZReady = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT + m_idx, (int)PScrewMain.ePOS_READY].Z;
            }
            else if (_PosMove == PosTeach.ePOS_SCREW)
            {
                int _index = 0;
                if (LV_LIST_SCREW.SelectedIndices.Count > 0)
                {
                    _index = LV_LIST_SCREW.SelectedIndices[0];
                    _dX = InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.X;
                    _dY = InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.Y;
                    _dZ = InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.Z;
                    _dZReady = InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_READY].Z;

                    if (InforManager.Instance.IsVisionPoint == true)
                    {
                        _dX += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                        _dY += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                        //_dZ += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_VISION].Z;
                        _dX -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                        _dY -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                        //_dZ -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_SCREW].Z;
                        _dZ = InforTeaching.Instance.P_Screw[m_idx][_index].PScrew.VisionZ;
                    }
                }
                else
                {
                    MSystem.MyMsgMemo("Position Screw Select Not Correct", "Error", msgButton.OK, msgIcon.Error);
                    return;
                }
            }
            else
            {
                MSystem.MyMsgMemo("Position Select Not Correct", "Error", msgButton.OK, msgIcon.Error);
                return;
            }
            //Start Move
            var Result = Task.Run(async () =>
            {
                if (m_idx == (int)_NSC.eLEFT)
                    MSystem.SetMsgDisplay("Start Move Screw Left ....");
                else
                    MSystem.SetMsgDisplay("Start Move Screw Right ....");

                await Task.Delay(100);

                TimCheck.StartTimer();
                /*******************************************************************************************/
                //Z-UP
                int _Result = MSystem.m_pTrsScrew[m_idx].MovePosition(Axis.AXIS_Z1 + m_idx, _dZReady);
                if (_Result != MSystem.MMC_OK)
                {

                    if (m_idx == Constants.left)
                        MSystem.MyMsgMemo("Screw Left Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    else
                        MSystem.MyMsgMemo("Screw Right Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);

                    MSystem.KillMsgDisplay();
                    return;
                }

                TimCheck.StartTimer();
                while (true)
                {
                    if (MSystem.m_pTrsScrew[m_idx].IsMoveComplete(Axis.AXIS_Z1 + m_idx, _dZReady))
                    {
                        break;
                    }
                    if (TimCheck.MoreThan(6.0) || MSystem.IsDetectDoorOpen(out _) || MSystem.IsDetectEmergency())
                    {
                        if (m_idx == Constants.left)
                            MSystem.MyMsgMemo("Screw Left Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        else
                            MSystem.MyMsgMemo("Screw Right Z UP Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        MSystem.KillMsgDisplay();
                        return;
                    }
                }
                /*******************************************************************************************/
                //Start Move
                if (MSystem.SetMoveLinear((int)Axis.AXIS_X1 + m_idx, _dX, (int)Axis.AXIS_Y1 + m_idx, _dY) != MSystem.MMC_OK)
                {

                    if (m_idx == Constants.left)
                        MSystem.MyMsgMemo("Screw Left Move XY Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    else
                        MSystem.MyMsgMemo("Screw Right Move XY Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    MSystem.KillMsgDisplay();
                    return;
                }

                TimCheck.StartTimer();
                while (true)
                {
                    if (MSystem.m_pTrsJig[m_idx].IsMoveComplete(Axis.AXIS_X1 + m_idx, _dX) &&
                        MSystem.m_pTrsJig[m_idx].IsMoveComplete(Axis.AXIS_Y1 + m_idx, _dY))
                    {
                        break;
                    }

                    if (TimCheck.MoreThan(6.0) || MSystem.IsDetectDoorOpen(out _) || MSystem.IsDetectEmergency())
                    {
                        if (m_idx == (int)_NSC.eLEFT)
                            MSystem.MyMsgMemo("Screw Left Move XY Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        else
                            MSystem.MyMsgMemo("Screw Right Move XY Fail", "Alarm", msgButton.OK, msgIcon.Error);

                        MSystem.KillMsgDisplay();
                        return;
                    }
                }
                // CHECK LIMIT Z+
                if (m_idx == Constants.left)
                {
                    if (_dZ >= InforManager.Instance.m_dLimitZPlus)
                    {
                        MSystem.MyMsgMemo("Check Limit Z+ Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        MSystem.KillMsgDisplay();
                        return;
                    }
                }
                else if (m_idx == Constants.right)
                {
                    if (_dZ >= InforManager.Instance.m_dLimitZPlus)
                    {
                        MSystem.MyMsgMemo("Check Limit Z+ Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        MSystem.KillMsgDisplay();
                        return;
                    }
                }
                /*******************************************************************************************/
                //Z-Down
                Thread.Sleep(200);
                _Result = MSystem.m_pTrsScrew[m_idx].MovePosition(Axis.AXIS_Z1 + m_idx, _dZ);
                if (_Result != MSystem.MMC_OK)
                {

                    if (m_idx == (int)_NSC.eLEFT)
                        MSystem.MyMsgMemo("Screw Left Z Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    else
                        MSystem.MyMsgMemo("Screw Right Z Fail", "Alarm", msgButton.OK, msgIcon.Error);

                    MSystem.KillMsgDisplay();
                    return;
                }

                TimCheck.StartTimer();
                while (true)
                {
                    if (MSystem.m_pTrsScrew[m_idx].IsMoveComplete(Axis.AXIS_Z1 + m_idx, _dZ))
                    {
                        break;
                    }
                    if (TimCheck.MoreThan(6.0) || MSystem.IsDetectDoorOpen(out _) || MSystem.IsDetectEmergency())
                    {
                        if (m_idx == Constants.left)
                            MSystem.MyMsgMemo("Screw Left Z Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        else
                            MSystem.MyMsgMemo("Screw Right Z Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        MSystem.KillMsgDisplay();
                        return;
                    }
                }
                MSystem.KillMsgDisplay();
            });
            MSystem.MyMsgDisplay("Start Move to Position Select .....\r\nWait a moment....");
        }
        #endregion

        #region Manual IO control
        private void IDC_VACUUM_ON_ClickEvent(object sender, EventArgs e)
        {
            if (MSystem.m_pTrsScrew[m_idx].IsVacuumSolOn() == true)
            {
                MSystem.m_pTrsScrew[m_idx].VacuumOff();
                if (IDC_VACUUM_ON.BackColorInterior != Color.White)
                    IDC_VACUUM_ON.BackColorInterior = Color.White;
            }
            else
            {
                MSystem.m_pTrsScrew[m_idx].VacuumOn();
                if (IDC_VACUUM_ON.BackColorInterior != Color.Lime)
                    IDC_VACUUM_ON.BackColorInterior = Color.Lime;
            }
        }

        private void IDC_BOLLOW_ON_ClickEvent(object sender, EventArgs e)
        {
            if (MSystem.m_pTrsScrew[m_idx].IsBlowSolOn())
                MSystem.m_pTrsScrew[m_idx].BlowOff();
            else
                MSystem.m_pTrsScrew[m_idx].BlowOn();
        }
        private void IDC_BLOW_ON_ClickEvent(object sender, EventArgs e)
        {
            if (MSystem.m_pTrsScrew[m_idx].IsBlowSolOn())
                MSystem.m_pTrsScrew[m_idx].BlowOff();
            else
                MSystem.m_pTrsScrew[m_idx].BlowOn();
        }
        private void IDC_COVER_FRONT_UP_ClickEvent(object sender, EventArgs e)
        {

        }

        private void IDC_COVER_REAR_UP_ClickEvent(object sender, EventArgs e)
        {

        }

        private void IDC_COVER_REAR_DOWN_ClickEvent(object sender, EventArgs e)
        {
            MSystem.m_pTrsJig[m_idx].CoverDown();
        }

        private void IDC_FIX_LEFT_UP_ClickEvent(object sender, EventArgs e)
        {
            MSystem.m_pTrsJig[m_idx].FixUP();
        }
        private void IDC_FIX_LEFT_DOWN_ClickEvent(object sender, EventArgs e)
        {
            MSystem.m_pTrsJig[m_idx].FixDown();
        }

        private void IDC_FIX_RIGHT_UP_ClickEvent(object sender, EventArgs e)
        {
            MSystem.m_pTrsJig[m_idx].FixUP();
        }

        private void IDC_FIX_RIGHT_DOWN_ClickEvent(object sender, EventArgs e)
        {
            MSystem.m_pTrsJig[m_idx].FixDown();
        }

        private void IDC_DRIVER_RUN_ClickEvent(object sender, EventArgs e)
        {
            int preset = 1;
            if (radioButtonPreset1.Checked)
                preset = 1;
            else if (radioButtonPreset2.Checked)
                preset = 2;
            else if (radioButtonPreset3.Checked)
                preset = 3;
            if (MSystem.m_pTrsScrew[m_idx].IsDriverRunOut())
                MSystem.m_pTrsScrew[m_idx].DriverStop();
            else
                MSystem.m_pTrsScrew[m_idx].DriverRun(preset);
        }

        private void IDC_DRIVER_RESET_ClickEvent(object sender, EventArgs e)
        {
            MSystem.m_pTrsScrew[m_idx].DriverStop();
            Thread.Sleep(200);
            if (MSystem.m_pTrsScrew[m_idx].IsDriverResetAlarm())
                MSystem.m_pTrsScrew[m_idx].DriverResetAlarm(MSystem.OFF);
            else
                MSystem.m_pTrsScrew[m_idx].DriverResetAlarm(MSystem.ON);
        }

        private void IDC_DRIVER_FASTEN_LOOSEN_ClickEvent(object sender, EventArgs e)
        {
            if (MSystem.m_pTrsScrew[m_idx].IsDriverFastenLoosen())
                MSystem.m_pTrsScrew[m_idx].DriverFastenLoosen(MSystem.OFF);
            else
                MSystem.m_pTrsScrew[m_idx].DriverFastenLoosen(MSystem.ON);
        }
        #endregion

        private void axBtnEnh9_ClickEvent(object sender, EventArgs e)
        {
            double _dx = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
            double _dy = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);
            double _dz = MSystem.GetCurrentPos(Axis.AXIS_Z1 + m_idx);
            PointScrew _tmpPoint = new PointScrew();
            int _count = LV_LIST_SCREW.Items.Count;
            _tmpPoint.P_Name = $"P{_count}";
            _tmpPoint.Skip = false;
            _tmpPoint.Vision = false;
            _tmpPoint.Channel = 1;
            _tmpPoint.Retry = false;
            _tmpPoint.PScrew.X = _dx;
            _tmpPoint.PScrew.Y = _dy;
            _tmpPoint.PScrew.Z = _dz;
            var itemsL = new ListViewItem(new string[] { $"{_tmpPoint.P_Name}",
                                                        $"{_tmpPoint.PScrew.X.ToString("f03")}",
                                                        $"{_tmpPoint.PScrew.Y.ToString("f03")}",
                                                        $"{_tmpPoint.PScrew.Z.ToString("f03")}",
                                                        $" ",$" ",});

            InforTeaching.Instance.P_Screw[m_idx].Add(_tmpPoint);
            //LV_LIST_SCREW.Items.Add(itemsL);
            MSystem.m_pListScrew.LoadDataScrew(m_idx);
            MSystem.m_pListScrew.SelectIndex(_count);
            InforTeaching.Instance.SaveSettings();
        }

        private void axBtnEnh14_ClickEvent(object sender, EventArgs e)
        {
            int _CountTmp = LV_LIST_SCREW.Items.Count;
            if (MSystem.MyMsgMemo("Do You Want To Delete Point ?", "Delete", msgButton.YESNO, msgIcon.Question) == DialogResult.Yes)
            {
                if (_CountTmp > 1)
                {
                    LV_LIST_SCREW.Items.RemoveAt(_CountTmp - 1);
                    InforTeaching.Instance.P_Screw[m_idx].RemoveAt(_CountTmp - 1);

                    int _CountTmp2 = LV_LIST_SCREW.Items.Count;
                    if (_CountTmp2 > 0)
                        MSystem.m_pListScrew.SelectIndex(_CountTmp2 - 1);
                    InforTeaching.Instance.SaveSettings();
                }
            }
        }

        private void BT_COPY_POINT_ClickEvent(object sender, EventArgs e)
        {
            return;
            if (m_idx == 0) return;
            if (MSystem.MyMsgMemo("Do You Want Copy Data Point From Jig 1?", "Question", msgButton.YESNO, msgIcon.Question) == DialogResult.Yes)
            {
                InforTeaching.Instance.P_Screw[m_idx].Clear();
                for (int i = 0; i < InforTeaching.Instance.P_Screw[0].Count; i++)
                {
                    PointScrew _tmpPoint = new PointScrew();
                    _tmpPoint.P_Name = $"P{i + 1}";
                    _tmpPoint.PScrew.PointName = _tmpPoint.P_Name;
                    _tmpPoint.PScrew.X = 0.00;
                    _tmpPoint.PScrew.Y = 0.00;
                    _tmpPoint.PScrew.Z = 0.00;
                    _tmpPoint.Skip = false;
                    _tmpPoint.Channel = 1;
                    _tmpPoint.Retry = false;
                    InforTeaching.Instance.P_Screw[m_idx].Add(_tmpPoint);
                }

                for (int i = 0; i < InforTeaching.Instance.P_Screw[0].Count; i++)
                {
                    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X = InforTeaching.Instance.P_Screw[0][i].PScrew.X;
                    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y = InforTeaching.Instance.P_Screw[0][i].PScrew.Y;
                    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Z = InforTeaching.Instance.P_Screw[0][i].PScrew.Z;
                }
                MSystem.m_pListScrew.LoadDataScrew(m_idx);
            }
        }

        private void BT_GET_OFFSET_ClickEvent(object sender, EventArgs e)
        {
            if (m_idx == 0) return;
            if (MSystem.MyMsgMemo("Do You Want Get Offset with Jig 1", "Question", msgButton.YESNO, msgIcon.Question) == DialogResult.Yes)
            {
                if (LV_LIST_SCREW.SelectedIndices.Count > 0)
                {
                    int _idxPoint = LV_LIST_SCREW.SelectedIndices[0];
                    if (_idxPoint != 0)
                    {
                        MSystem.MyMsgMemo("Please Move To Point Screw To point 1", "Memo");
                        return;
                    }
                    if (!MSystem.m_pTrsScrew[m_idx].IsMoveCompleteLineXY(InforTeaching.Instance.P_Screw[m_idx][0]))
                    {
                        MSystem.MyMsgMemo("Please Move To Point Screw To point 1", "Memo");
                        return;
                    }
                    else
                    {
                        double[,] _DeltaData = new double[2, InforTeaching.Instance.P_Screw[0].Count];
                        for (int i = 0; i < InforTeaching.Instance.P_Screw[0].Count; i++)
                        {
                            _DeltaData[0, i] = InforTeaching.Instance.P_Screw[0][i].PScrew.X - InforTeaching.Instance.P_Screw[0][0].PScrew.X;
                            _DeltaData[1, i] = InforTeaching.Instance.P_Screw[0][i].PScrew.Y - InforTeaching.Instance.P_Screw[0][0].PScrew.Y;
                        }
                        for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
                        {
                            InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X = InforTeaching.Instance.P_Screw[m_idx][0].PScrew.X - _DeltaData[0, i];
                            InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y = InforTeaching.Instance.P_Screw[m_idx][0].PScrew.Y + _DeltaData[1, i];
                        }
                        MSystem.m_pListScrew.LoadDataScrew(m_idx);
                    }
                }
                else
                {
                    MSystem.MyMsgMemo("Please Select One Screw Point", "Memo");
                    return;
                }
            }
        }

        private void axBtnEnh2_ClickEvent(object sender, EventArgs e)
        {
            InforManager.Instance.IsVisionPoint = !InforManager.Instance.IsVisionPoint;
            BT_SCREW.Caption = InforManager.Instance.IsVisionPoint == true ? "Vision Point" : "Screw Point";
            LV_LIST_SCREW.Items.Clear();
            MSystem.m_pListScrew.LoadDataScrew(m_idx);
            MSystem.m_pListScrew.SelectIndex();
        }

        private void axBtnEnh5_ClickEvent(object sender, EventArgs e)//Load Job File Button
        {
            try
            {
                var dlg = new OpenFileDialog();
                dlg.InitialDirectory = "C:\\";
                dlg.Multiselect = false;
                dlg.Multiselect = false;
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                string[] lines = File.ReadAllLines(dlg.FileName);
                if (lines.Length <= 1) return;

                InforTeaching.Instance.P_Screw[m_idx].Clear();

                int index = 0;
                int p=0, no=0, x=0, y = 0;
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] value = lines[i].Split(',');
                    if (value.Length < 4)
                        return;

                    for (int j = 0; j < value.Length; j++)
                    {
                        if (value[j].Trim() == "P1")
                        {
                            p = j;
                            no = j + 1;
                            x = j + 2;
                            y = j + 3;
                        }
                    }
                    bool result = false;
                    double _dx = 0;
                    double _dy = 0;
                    result = int.TryParse(value[no], out int number);

                    if (result == false)
                        continue;

                    if (index + 1 != number)
                    {
                        MSystem.MyMsgMemo("Job File Error", "Error", msgButton.OK, msgIcon.Error);
                        return;
                    }

                    double.TryParse(value[x], out _dx);
                    double.TryParse(value[y], out _dy);
                    /*
                    if (MSystem.m_IsSite == false)
                    {
                        result = int.TryParse(value[1], out int number);

                        if (result == false)
                            continue;

                        if (index + 1 != number)
                        {
                            MSystem.MyMsgMemo("Job File Error", "Error", msgButton.OK, msgIcon.Error);
                            return;
                        }

                        double.TryParse(value[2], out _dx);
                        double.TryParse(value[3], out _dy);
                    }
                    else
                    {
                        result = int.TryParse(value[8], out int number);

                        if (result == false)
                            continue;

                        if (index + 1 != number)
                        {
                            MSystem.MyMsgMemo("Job File Error", "Error", msgButton.OK, msgIcon.Error);
                            return;
                        }

                        double.TryParse(value[9], out _dx);
                        double.TryParse(value[10], out _dy);
                    }
                    */
                    double _dz = 0;
                    InforTeaching.Instance.P_ScrewMaster[index].X = _dx;
                    InforTeaching.Instance.P_ScrewMaster[index].Y = _dy;
                    if (m_idx == Constants.right)
                    {
                        _dx *= -1.0;
                    }

                    PointScrew _tmpPoint = new PointScrew();
                    int _count = LV_LIST_SCREW.Items.Count;
                    _tmpPoint.P_Name = $"P{number}";
                    _tmpPoint.Skip = false;
                    _tmpPoint.Vision = false;
                    _tmpPoint.Channel = 1;
                    _tmpPoint.FasNumbers = 1;
                    _tmpPoint.Retry = false;
                    _tmpPoint.PScrew.X = _dx;
                    _tmpPoint.PScrew.Y = _dy;
                    _tmpPoint.PScrew.Z = _dz;
                    var itemsL = new ListViewItem(new string[] 
                    {
                        $"{_tmpPoint.P_Name}",
                        $"{_tmpPoint.PScrew.X.ToString("f03")}",
                        $"{_tmpPoint.PScrew.Y.ToString("f03")}",
                        $"{_tmpPoint.PScrew.Z.ToString("f03")}",
                        $" ", // Use
                        $" ", // Test
                        $"{_tmpPoint.Channel}",
                        $" ", // Vision
                        $" ", // Retry
                        $"{_tmpPoint.FasNumbers}"
                    });

                    InforTeaching.Instance.P_Screw[m_idx].Add(_tmpPoint);
                    index += 1;
                }

                MSystem.m_pListScrew.LoadDataScrew(m_idx);
                MSystem.m_pListScrew.SelectIndex();
                InforTeaching.Instance.SaveSettings();
            }
            catch
            {
                MSystem.MyMsgMemo("Get data from file fail\nRecheck file and try again", "Error", msgButton.OK, msgIcon.Error);
            }
        }

        private PointAxis p1Old;
        private PointAxis p2Old;
        private PointAxis p1New;
        private PointAxis p2New;

        private void axBtnEnh3_ClickEvent(object sender, EventArgs e)// set First Pos
        {
            BT_SET_LASTPOS.Enabled = true;
            BT_SET_FIRSTPOS.BackColorInterior = Color.Orange;
            int index = LV_LIST_SCREW.SelectedIndices[0];
            p1Old.X = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.X;
            p1Old.Y = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Y;
            //p1Old.Z = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Z;

            p1New.X = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
            p1New.Y = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);
            //p1New.Z = MSystem.GetCurrentPos(Axis.AXIS_Z1 + m_idx);

            if (InforManager.Instance.IsVisionPoint == true)
            {
                p1New.X += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                p1New.Y += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                //p1New.Z += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_SCREW].Z;
                p1New.X -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                p1New.Y -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                //p1New.Z -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_VISION].Z;
            }

        }

        private void axBtnEnh4_ClickEvent(object sender, EventArgs e)// Set Last Pos Button
        {
            BT_SET_FIRSTPOS.BackColorInterior = Color.Orange;
            BT_SET_LASTPOS.Enabled = true;
           
            int index = LV_LIST_SCREW.SelectedIndices[0];
            p2Old.X = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.X;
            p2Old.Y = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Y;
            //p2Old.Z = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Z;

            p2New.X = MSystem.GetCurrentPos(Axis.AXIS_X1 + m_idx);
            p2New.Y = MSystem.GetCurrentPos(Axis.AXIS_Y1 + m_idx);
            //p2New.Z = MSystem.GetCurrentPos(Axis.AXIS_Z1 + m_idx);

            if (InforManager.Instance.IsVisionPoint == true)
            {
                p2New.X += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                p2New.Y += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                //p2New.Z += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_SCREW].Z;
                p2New.X -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                p2New.Y -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                //p2New.Z -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_VISION].Z;
            }

            try
            {
                double p2NewXDist = p2New.X - p1New.X;
                double p2NewYDist = p2New.Y - p1New.Y;
                double p2OldXDist = p2Old.X - p1Old.X;
                double p2OldYDist = p2Old.Y - p1Old.Y;

                double p2NewXDistSquare = Math.Pow(p2NewXDist, 2);
                double p2NewYDistSquare = Math.Pow(p2NewYDist, 2);
                double p2OldXDistSquare = Math.Pow(p2OldXDist, 2);
                double p2OldYDistSquare = Math.Pow(p2OldYDist, 2);

                double newDist = Math.Sqrt(p2NewXDistSquare + p2NewYDistSquare);
                double oldDist = Math.Sqrt(p2OldXDistSquare + p2OldYDistSquare);

                if (Math.Abs(newDist - oldDist) > 0.3)
                {
                    MSystem.MyMsgMemo("Old and New Position Gap(0.3)Over", "Error", msgButton.OK, msgIcon.Error);
                    return;
                }

                double degreeNew = Math.Atan2(p2NewYDist, p2NewXDist);
                double degreeOld = Math.Atan2(p2OldYDist, p2OldXDist);
                double degreeDist = degreeNew - degreeOld;
                double sin = Math.Sin(degreeDist);
                double cos = Math.Cos(degreeDist);

                double newCenterX = (p1New.X + p2New.X) / 2;
                double newCenterY = (p1New.Y + p2New.Y) / 2;
                //double newCenterZ = (p1New.Z + p2New.Z) / 2;
                double oldCenterX = (p1Old.X + p2Old.X) / 2;
                double oldCenterY = (p1Old.Y + p2Old.Y) / 2;

                double convertX;
                double convertY;
                double x;
                double y;
                for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
                {
                    x = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X - oldCenterX;
                    y = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y - oldCenterY;

                    convertX = cos * x - sin * y;
                    convertY = sin * x + cos * y;

                    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X = convertX + newCenterX;
                    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y = convertY + newCenterY;
                    //InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Z = newCenterZ;
                }

                MSystem.m_pListScrew.LoadDataScrew(m_idx);
                MSystem.m_pListScrew.SelectIndex();
                InforTeaching.Instance.SaveSettings();

            }
            catch { }
        }

        private void buttonJigUpDown_Click(object sender, EventArgs e)
        {
            if (MSystem.MyMsgMemo($"Do yot want to move the jig cylinder?", "Question", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
                return;
            // Tablet
            if (InforManager.Instance.InspectionType == "Tablet")
            {
                if (MSystem.m_pTrsJig[m_idx].IsCoverDownFront() == true)
                {
                    MSystem.m_pTrsJig[m_idx].CoverUp();
                }
                else
                {
                    MSystem.m_pTrsJig[m_idx].CoverDown();
                }
            }
            else
            {
                if (MSystem.m_pTrsJig[m_idx].IsDownSol() == true)
                {
                    MSystem.m_pTrsJig[m_idx].Up();
                }
                else
                {
                    MSystem.m_pTrsJig[m_idx].Down();
                }
            }
        }

        private void buttonJigFwdBwd_Click(object sender, EventArgs e)
        {
            if (MSystem.MyMsgMemo($"Do yot want to move the fix cylinder?", "Question", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
                return;
            if (InforManager.Instance.InspectionType == "Tablet")
            {
                if (MSystem.m_pTrsJig[m_idx].IsFixDownSol() == true)
                {
                    MSystem.m_pTrsJig[m_idx].FixUP();
                }
                else
                {
                    MSystem.m_pTrsJig[m_idx].FixDown();
                }
            }
            else
            {
                if (MSystem.m_pTrsJig[m_idx].IsUp() == false)
                {
                    MSystem.MyMsgMemo("NO UP!!", "Info", msgButton.OK, msgIcon.Infor);
                    return;
                }

                if (MSystem.m_pTrsJig[m_idx].IsFwdSol() == true)
                {
                    MSystem.m_pTrsJig[m_idx].Bwd();
                }
                else
                {
                    MSystem.m_pTrsJig[m_idx].Fwd();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (InforManager.Instance.InspectionType == "Tablet")
            {
                labelJigUp.BackColor = MSystem.m_pTrsJig[m_idx].IsCoverUPFront() == true ? Color.Lime : Color.LightGray;
                labelJigDown.BackColor = MSystem.m_pTrsJig[m_idx].IsCoverDownFront() == true ? Color.Lime : Color.LightGray;
                labelJigFwd.BackColor = MSystem.m_pTrsJig[m_idx].IsFixUPSol() == true ? Color.Lime : Color.LightGray;
                labelJigBwd.BackColor = MSystem.m_pTrsJig[m_idx].IsFixDownSol() == true ? Color.Lime : Color.LightGray;

                buttonJigUpDown.BackColor = MSystem.m_pTrsJig[m_idx].IsCoverUPFront() == true ? Color.Lime : Color.LightGray;
                buttonJigFwdBwd.BackColor = MSystem.m_pTrsJig[m_idx].IsFixDownSol() == true ? Color.Lime : Color.LightGray;
            }
            else
            {
                labelJigUp.BackColor = MSystem.m_pTrsJig[m_idx].IsUp() == true ? Color.Lime : Color.LightGray;
                labelJigDown.BackColor = MSystem.m_pTrsJig[m_idx].IsDown() == true ? Color.Lime : Color.LightGray;
                labelJigFwd.BackColor = MSystem.m_pTrsJig[m_idx].IsFwd() == true ? Color.Lime : Color.LightGray;
                labelJigBwd.BackColor = MSystem.m_pTrsJig[m_idx].IsBwd() == true ? Color.Lime : Color.LightGray;

                buttonJigUpDown.BackColor = MSystem.m_pTrsJig[m_idx].IsDownSol() == true ? Color.Lime : Color.LightGray;
                buttonJigFwdBwd.BackColor = MSystem.m_pTrsJig[m_idx].IsFwdSol() == true ? Color.Lime : Color.LightGray;
            }
        }

        private void buttonHantasSetting_Click(object sender, EventArgs e)
        {
            string Pass = MSystem.GetPass();
            if (Pass == null) return;
            if (Pass != "1111")
            {
                MSystem.MyMsgMemo("Password is incorrect", "Error", msgButton.OK, msgIcon.Error);
                return;
            }

            buttonHantasSetting.Enabled = false;
            try
            {
                InforTeaching.Instance.Handtas_Touque[m_idx] = Convert.ToDouble(textTorque.Text);
                InforTeaching.Instance.SaveSettings();
                ushort[] data = new ushort[1];
                data[0] = (ushort)Convert.ToUInt16(Convert.ToDouble(textTorque.Text) * 100);
                int _data = (int)Convert.ToInt32(Convert.ToDouble(textTorque.Text) * 100);
                int preset = 0;
                if (radioButtonPreset1.Checked)
                    preset = 1;
                else if (radioButtonPreset2.Checked)
                    preset = 2;
                else if (radioButtonPreset3.Checked)
                    preset = 3;
                if (MSystem.m_pHantas[m_idx].setTorpueData(preset, _data) == false)
                //if (MSystem.m_pHantas[m_idx].MbWriteRegister(0x01, 0x02, data) == false)
                {
                    MSystem.MyMsgMemo("Screw Torque Setting Fail", "Error", msgButton.OK, msgIcon.Error);
                }
                else
                {
                    MSystem.m_pTrsScrew[m_idx].DriverRun(preset);
                    Thread.Sleep(500);
                    MSystem.m_pTrsScrew[m_idx].DriverStop();

                    int address = 0;
                    if (preset == 1)
                        address = 2;
                    else if (preset == 2)
                        address = 17;
                    else if (preset == 3)
                        address = 32;

                    ushort[] temp = MSystem.m_pHantas[m_idx].MbReadHoldingRegister(0x01, (ushort)address, 7);
                    if (temp != null && temp.Length == 7)
                    {
                        textTorque.Text = Convert.ToString(temp[0] / 100.0);
                        textTorqueMinMax.Text = Convert.ToString(temp[6]);
                    }

                    buttonHantasSetting.BackColor = Color.LightGray;
                }
            }
            catch (Exception)
            {
                MSystem.MyMsgMemo("Screw Torque Value Write Fail", "Error", msgButton.OK, msgIcon.Error);
                buttonHantasSetting.Enabled = true;
            }
            textTorque.ForeColor = Color.Black;
            buttonHantasSetting.Enabled = true;
        }

        private void buttonHantasRead_Click(object sender, EventArgs e)
        {
            try
            {
                textTorque.Text = "0.00";
                textTorqueMinMax.Text = "00";
                int address = 0;
                if (radioButtonPreset1.Checked)
                    address = 2;
                else if (radioButtonPreset2.Checked)
                    address = 17;
                else if (radioButtonPreset3.Checked)
                    address = 32;
                
                ushort[] temp = MSystem.m_pHantas[m_idx].MbReadHoldingRegister(0x01, (ushort)address, 7);
                if (temp != null && temp.Length == 7)
                {
                    textTorque.Text = Convert.ToString(temp[0] / 100.0);
                    textTorqueMinMax.Text = Convert.ToString(temp[6]);
                }
                else
                {
                    MSystem.MyMsgMemo("Screw Torque Value Read Fail", "Error", msgButton.OK, msgIcon.Error);
                }
            }
            catch (Exception)
            {
                MSystem.MyMsgMemo("Screw Torque Value Read Fail", "Error", msgButton.OK, msgIcon.Error);
                buttonHantasSetting.Enabled = true;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (MSystem.IsDetectDoorOpen(out string m) == true)
            {
                if (IsDoorOpenStatus == true)
                {
                    buttonDoorStatus.BackColor = Color.Red;
                    IsDoorOpenStatus = false;
                }
                else
                {
                    buttonDoorStatus.BackColor = Color.White;
                    IsDoorOpenStatus = true;
                }
            }
            else
            {
                buttonDoorStatus.BackColor = Color.White;
                IsDoorOpenStatus = true;
            }
        }

        private void FormTeachScrew_FormClosed(object sender, FormClosedEventArgs e)
        {
            //MTrsJig.m_pTrsJig[m_idx].FixUP();
            //MTrsJig.m_pTrsJig[m_idx].CoverUp();

            MSystem.m_bIsTeachDlg = false;
        }

        private void BT_GET_MASTER_ClickEvent(object sender, EventArgs e)
        {
            if (m_idx == 0)
            {
                for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
                {

                    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X = InforTeaching.Instance.P_Screw[m_idx][0].PScrew.X + InforTeaching.Instance.P_ScrewMaster[i].X;

                    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y = InforTeaching.Instance.P_Screw[m_idx][0].PScrew.Y - InforTeaching.Instance.P_ScrewMaster[i].Y;

                }
            }
            else
            {
                for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
                {

                    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X = InforTeaching.Instance.P_Screw[m_idx][0].PScrew.X - InforTeaching.Instance.P_ScrewMaster[i].X;

                    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y = InforTeaching.Instance.P_Screw[m_idx][0].PScrew.Y - InforTeaching.Instance.P_ScrewMaster[i].Y;

                }
            }


            MSystem.m_pListScrew.LoadDataScrew(m_idx);
            MSystem.m_pListScrew.SelectIndex();
            InforTeaching.Instance.SaveSettings();


        }
        private void BT_ALIGN_ClickEvent(object sender, EventArgs e)
        {
            MSystem.m_bManual_Align = true;
            if (InforManager.Instance.AutoVisionAlignMode == 2)
            {
                DemoSet demoSet = new DemoSet();
                if (demoSet.ShowDialog() != DialogResult.OK)
                {
                    MSystem.m_bManual_Align = false;
                    return;
                }
            }
            if (MSystem.m_pTrsScrew[m_idx].VisionAlign(false) == false)
                MessageBox.Show("Vision Align Position Find Fail.");
            else
            {
                if (InforManager.Instance.AutoVisionAlignMode == 2)
                {
                    if (MSystem.MyMsgMemo("Are you want saves vision Data?", "Save Data", msgButton.YESNO, msgIcon.Question) == DialogResult.Yes)
                    {
                        IniFile ini;
                        string path = Config.SystemSavePath + "DemoSetPosition.ini";
                        ini = new IniFile(path);
                        string section = MSystem.m_bVisionTestDemo == true ? "DEMO" : "SET";
                        for (int i=0; i< InforTeaching.Instance.P_Screw[m_idx].Count; i++)
                        {
                            InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X += MSystem.m_pTrsScrew[m_idx].m_dVisionOffsetX[i];
                            InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y += MSystem.m_pTrsJig[m_idx].m_dVisionOffsetY[i];

                            string key = $"{i + 1}";
                            string value = $"{InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X},{InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y}";
                            ini.Write(section, key, value);
                        }
                    }
                }
            }
            MSystem.m_bManual_Align = false;

            MSystem.m_pListScrew.LoadDataScrew(m_idx);
            MSystem.m_pListScrew.SelectIndex();
            InforTeaching.Instance.SaveSettings();
        }

        private void TorqueInput(object sender, EventArgs e)
        {
            if (buttonHantasSetting.BackColor != Color.Red)
            {
                buttonHantasSetting.BackColor = Color.Red;
            }
            if (!string.IsNullOrEmpty(textTorque.Text))
            {
                // 글씨 색상을 파란색으로 변경
                textTorque.ForeColor = Color.Red;
            }
        }

        private void BT_PICKUP_TEST_Click(object sender, EventArgs e)
        {
            int stepIndex = 0;
            bool StepRun = true;
            int retrycount = 0;
            BT_PICKUP_TEST.Enabled = false;
            Stopwatch sw = new Stopwatch();
            Task.Run(() =>
            {
                while (StepRun)
                {
                    switch(stepIndex)
                    {
                        case 0:
                            MSystem.m_pTrsScrew[m_idx].MoveReadyPosZ();
                            stepIndex = 1;
                            sw.Restart();
                            break;
                        case 1:
                            if (MSystem.m_pTrsScrew[m_idx].IsReadyPosZ())
                            {
                                MSystem.m_pTrsScrew[m_idx].MoveTrashPosX();
                                stepIndex = 2;
                                sw.Restart();
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                MSystem.MyMsgMemo("Move Z Ready Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                StepRun = false;
                            }
                            break;
                        case 2:
                            if (MSystem.m_pTrsScrew[m_idx].IsTrashPosX())
                            {
                                MSystem.m_pTrsScrew[m_idx].MoveTrashPosZ();
                                sw.Restart();
                                stepIndex = 3;
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                MSystem.MyMsgMemo("Move X Trash Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                StepRun = false;
                            }
                            break;
                        case 3:
                            if (MSystem.m_pTrsScrew[m_idx].IsTrashPosZ())
                            {
                                MSystem.m_pTrsScrew[m_idx].BlowOn(); 
                                Thread.Sleep(500);
                                MSystem.m_pTrsScrew[m_idx].BlowOff();

                                MSystem.m_pTrsScrew[m_idx].MoveReadyPosZ();
                                sw.Restart();
                                stepIndex = 4;
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                MSystem.MyMsgMemo("Move Z Trash Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                StepRun = false;
                            }
                            break;
                        case 4:
                            if (MSystem.m_pTrsScrew[m_idx].IsReadyPosZ())
                            {
                                MSystem.m_pTrsScrew[m_idx].MovePickUpPosX();
                                sw.Restart();
                                stepIndex = 5;
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                MSystem.MyMsgMemo("Move Z Ready Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                StepRun = false;
                            }
                            break;
                        case 5:
                            if (MSystem.m_pTrsScrew[m_idx].IsPickUpPosX()
                            && MSystem.m_pTrsScrew[m_idx].IsFeederReady())
                            {
                                sw.Restart();
                                stepIndex = 6;
                                MSystem.m_pTrsScrew[m_idx].MovePickUpPosZ();
                                MSystem.m_pTrsScrew[m_idx].VacuumOn();
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                MSystem.MyMsgMemo("Move X Pickup & Feeder Ready Fail", "Error", msgButton.OK, msgIcon.Error);
                                StepRun = false;
                            }
                            break;
                        case 6:
                            if (MSystem.m_pTrsScrew[m_idx].IsPickUpPosZ()
                            && MSystem.m_pTrsScrew[m_idx].IsVacuumSensorOn())
                            {
                                sw.Restart();
                                stepIndex = 7;
                                
                                MSystem.m_pTrsScrew[m_idx].MoveReadyPosZ();
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                MSystem.MyMsgMemo("Move Z Pickup & Vaccum Check Fail", "Error", msgButton.OK, msgIcon.Error);
                                StepRun = false;
                                MSystem.m_pTrsScrew[m_idx].MoveReadyPosZ();
                            }
                            break;
                        case 7:
                            if (sw.ElapsedMilliseconds > (InforManager.Instance.PickupDelayTime*1000))
                            {
                                sw.Restart();
                                stepIndex = 8;

                                MSystem.m_pTrsScrew[m_idx].MoveReadyPosZ();
                            }
                            break;
                        case 8:
                            if (MSystem.m_pTrsScrew[m_idx].IsReadyPosZ())
                            {
                                MSystem.m_pTrsScrew[m_idx].MoveTrashPosX();
                                stepIndex = 9;
                                sw.Restart();
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                MSystem.MyMsgMemo("Move Z Ready Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                StepRun = false;
                            }
                            break;
                        case 9:
                            if (MSystem.m_pTrsScrew[m_idx].IsTrashPosX())
                            {
                                MSystem.m_pTrsScrew[m_idx].MoveTrashPosZ();
                                sw.Restart();
                                stepIndex = 10;
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                MSystem.MyMsgMemo("Move X Trash Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                StepRun = false;
                            }
                            break;
                        case 10:
                            if (MSystem.m_pTrsScrew[m_idx].IsTrashPosZ())
                            {
                                MSystem.m_pTrsScrew[m_idx].BlowOn();
                                Thread.Sleep(500);
                                MSystem.m_pTrsScrew[m_idx].BlowOff();

                                MSystem.m_pTrsScrew[m_idx].MoveReadyPosZ();
                                sw.Restart();
                                retrycount++;
                                if (retrycount > 0)
                                    StepRun = false;
                                stepIndex = 0;
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                MSystem.MyMsgMemo("Move Z Trash Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                StepRun = false;
                            }
                            break;
                    }
                }
                BT_PICKUP_TEST.Enabled = true;

            });
        }

        private async void BT_SCREW_TEST_ONE_ClickEvent(object sender, EventArgs e)
        {
            try
            {
                BT_SCREW_TEST_ONE.Enabled = false;
                BT_SCREW_TEST_All.Enabled = false;
                if (LV_LIST_SCREW.SelectedIndices.Count > 0)
                {
                    int _index = LV_LIST_SCREW.SelectedIndices[0];
                    bool result = await Task.Run(() =>
                    {
                        return ScrewTestThread(_index);
                    });

                    if (result == false)
                    {
                        MSystem.m_ScrewTestResult[_index] = 2;
                    }
                    else
                    {
                        MSystem.m_ScrewTestResult[_index] = 1;
                    }
                    LV_LIST_SCREW.Refresh();
                }
            }
            catch { }
            BT_SCREW_TEST_ONE.Enabled = true;
            BT_SCREW_TEST_All.Enabled = true;

        }
        private async void BT_SCREW_TEST_All_ClickEvent(object sender, EventArgs e)
        {
            try
            {
                BT_SCREW_TEST_ONE.Enabled = false;
                BT_SCREW_TEST_All.Enabled = false;
                for (int j = 0; j < LV_LIST_SCREW.Items.Count; j++)
                {
                    MSystem.m_ScrewTestResult[j] = 0;
                    LV_LIST_SCREW.Items[j].BackColor = Color.Red;
                }
                for (int i = 0; i < LV_LIST_SCREW.Items.Count; i++)
                {
                    bool result = await Task.Run(() =>
                    {
                        return ScrewTestThread(i);
                    });
                    if (result == false)
                    {
                        MSystem.m_ScrewTestResult[i] = 2;
                        LV_LIST_SCREW.Refresh();

                        break;
                    }
                    else
                    {
                        MSystem.m_ScrewTestResult[i] = 1;
                        LV_LIST_SCREW.Refresh();
                    }
                }
            }
            catch { }

            BT_SCREW_TEST_ONE.Enabled = true;
            BT_SCREW_TEST_All.Enabled = true;
        }
        private bool ScrewTestThread(int index)
        {
            //bool result = Task.Run(() =>
            {
                Stopwatch sw = new Stopwatch();
                int ScrewTestStep = 0;
                bool StepRun = true;
                MSystem.m_ScrewTestResult[index] = 0;
                LV_LIST_SCREW.Refresh();
                #region // Trash Move
                MSystem.m_pTrsScrew[m_idx].MoveReadyPosZ();
                sw.Restart();
                while (true)
                {
                    if (MSystem.m_pTrsScrew[m_idx].IsReadyPosZ())
                    {
                        MSystem.m_pTrsScrew[m_idx].MoveTrashPosX();

                        break;
                    }
                    else if (sw.ElapsedMilliseconds > 5000)
                    {
                        MSystem.MyMsgMemo("Screw Move Z Ready Position Fail", "Error", msgButton.OK, msgIcon.Error);
                        return false;
                    }
                    if (MSystem.m_pTrsJig[m_idx].IsPressStop() == true)
                    {
                        return false;
                    }
                    Thread.Sleep(10);
                }
                sw.Restart();
                while (true)
                {
                    if (MSystem.m_pTrsScrew[m_idx].IsTrashPosX())
                    {
                        MSystem.m_pTrsScrew[m_idx].MoveTrashPosZ();
                        break;
                    }
                    else if (sw.ElapsedMilliseconds > 5000)
                    {
                        MSystem.MyMsgMemo("Screw Move X Trash Position Fail", "Error", msgButton.OK, msgIcon.Error);
                        return false;
                    }
                    if (MSystem.m_pTrsJig[m_idx].IsPressStop() == true)
                    {
                        return false;
                    }
                    if (MSystem.IsDetectDoorOpen(out string m) == true)
                    {
                        MSystem.MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                        return false;
                    }

                    if (MSystem.IsDetectEmergency() == true)
                    {
                        MSystem.MyMsgMemo("EMG Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                        return false;
                    }
                    if (MSystem.IsLightCurtainDetected() == true)
                    {
                        MSystem.MyMsgMemo("Light Curtain Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                        return false;
                    }
                    Thread.Sleep(10);
                }
                sw.Restart();
                while (true)
                {
                    if (MSystem.m_pTrsScrew[m_idx].IsTrashPosZ())
                    {
                        MSystem.m_pTrsScrew[m_idx].BlowOn();
                        Thread.Sleep(500);
                        MSystem.m_pTrsScrew[m_idx].BlowOff();
                        MSystem.m_pTrsScrew[m_idx].MoveReadyPosZ();
                        break;
                    }
                    else if (sw.ElapsedMilliseconds > 5000)
                    {
                        MSystem.MyMsgMemo("Screw Move Z Trash Position Fail", "Error", msgButton.OK, msgIcon.Error);
                        return false;
                    }
                    if (MSystem.m_pTrsJig[m_idx].IsPressStop() == true)
                    {
                        return false;
                    }
                    if (MSystem.IsDetectDoorOpen(out string m) == true)
                    {
                        MSystem.MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                        return false;
                    }

                    if (MSystem.IsDetectEmergency() == true)
                    {
                        MSystem.MyMsgMemo("EMG Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                        return false;
                    }
                    if (MSystem.IsLightCurtainDetected() == true)
                    {
                        MSystem.MyMsgMemo("Light Curtain Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                        return false;
                    }
                    Thread.Sleep(10);
                }
                #endregion
                while (StepRun)
                {
                    if (MSystem.m_pTrsJig[m_idx].IsPressStop() == true)
                    {
                        return false;
                    }
                    if (MSystem.IsDetectDoorOpen(out string m) == true)
                    {
                        MSystem.MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                        return false;
                    }

                    if (MSystem.IsDetectEmergency() == true)
                    {
                        MSystem.MyMsgMemo("EMG Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                        return false;
                    }
                    if (MSystem.IsLightCurtainDetected() == true)
                    {
                        MSystem.MyMsgMemo("Light Curtain Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                        return false;
                    }
                    switch (ScrewTestStep)
                    {
                        case 0:
                            if (MSystem.m_pTrsScrew[m_idx].IsReadyPosZ())
                            {
                                ScrewTestStep = 1;
                                sw.Restart();
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                StepRun = false;
                                ScrewTestStep = 0;
                                MSystem.MyMsgMemo("Screw Move Z Ready Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                return false;
                            }
                            break;
                        case 1:
                            MSystem.m_pTrsJig[m_idx].MoveScrewPosY(index);
                            ScrewTestStep = 2;
                            sw.Restart();
                            break;
                        case 2:
                            if (MSystem.m_pTrsJig[m_idx].IsScrewPosY(index))
                            {
                                MSystem.m_pTrsScrew[m_idx].MovePickUpPosX();
                                ScrewTestStep = 3;
                                sw.Restart();
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                StepRun = false;
                                ScrewTestStep = 0;
                                MSystem.MyMsgMemo("Screw Move Z Ready Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                return false;
                            }
                            break;
                        case 3:
                            if (MSystem.m_pTrsScrew[m_idx].IsPickUpPosX()
                                && MSystem.m_pTrsScrew[m_idx].IsFeederReady())
                            {
                                MSystem.m_pTrsScrew[m_idx].MovePickUpPosZ();
                                MSystem.m_pTrsScrew[m_idx].VacuumOn();
                                ScrewTestStep = 4;
                                sw.Restart();
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                StepRun = false;
                                ScrewTestStep = 0;
                                MSystem.MyMsgMemo("Screw Move X Pickup & Feeder Ready Fail", "Error", msgButton.OK, msgIcon.Error);

                                return false;
                            }
                            break;
                        case 4:
                            if (MSystem.m_pTrsScrew[m_idx].IsPickUpPosZ()
                                && MSystem.m_pTrsScrew[m_idx].IsVacuumSensorOn())
                            {
                                MSystem.m_pTrsScrew[m_idx].MoveReadyPosZ();
                                ScrewTestStep = 5;
                                sw.Restart();
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                StepRun = false;
                                ScrewTestStep = 0;
                                MSystem.MyMsgMemo("Screw Move Z Pickup & Vaccum Check Fail", "Error", msgButton.OK, msgIcon.Error);
                                MSystem.m_pTrsScrew[m_idx].MoveReadyPosZ();
                                return false;
                            }
                            break;
                        case 5:
                            if (MSystem.m_pTrsScrew[m_idx].IsReadyPosZ()
                            && MSystem.m_pTrsScrew[m_idx].IsVacuumSensorOn())
                            {
                                MSystem.m_pTrsScrew[m_idx].MoveScrewPosX(index);

                                ScrewTestStep = 6;
                                sw.Restart();
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                StepRun = false;
                                ScrewTestStep = 0;
                                MSystem.MyMsgMemo("Screw Move Z Ready Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                return false;
                            }
                            break;
                        case 6:
                            if (MSystem.m_pTrsScrew[m_idx].IsScrewPosX(index))
                            {
                                MSystem.m_pTrsScrew[m_idx].MoveScrewReadyPosZ(index);
                                ScrewTestStep = 7;
                                sw.Restart();
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                StepRun = false;
                                ScrewTestStep = 0;
                                MSystem.MyMsgMemo("Screw Move X Screw Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                return false;
                            }
                            break;
                        case 7:
                            if (MSystem.m_pTrsScrew[m_idx].IsMoveDoneScrewReadyPosZ(index))
                            {
                                MSystem.m_pTrsScrew[m_idx].VacuumOn();
                                MSystem.m_pTrsScrew[m_idx].DriverRun(InforTeaching.Instance.P_Screw[m_idx][index].Channel);
                                double dPosZ = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Z;

                                MSystem.m_pTrsScrew[m_idx].MovePosition(Axis.AXIS_Z1 + m_idx, dPosZ, InforManager.Instance.m_dSpeedScrew);
                                ScrewTestStep = 8;
                                sw.Restart();
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                StepRun = false;
                                ScrewTestStep = 0;
                                MSystem.MyMsgMemo("Screw Move Z Screw Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                return false;
                            }
                            break;
                        case 8:
                            if (MSystem.m_pTrsScrew[m_idx].IsDriverFastenOK())
                            {
                                MSystem.m_pTrsScrew[m_idx].DriverStop();
                                MSystem.m_pTrsScrew[m_idx].MoveReadyPosZ();
                                StepRun = false;
                                ScrewTestStep = 0;
                            }
                            else if (sw.ElapsedMilliseconds > 5000)
                            {
                                StepRun = false;
                                ScrewTestStep = 0;
                                MSystem.MyMsgMemo("Screw Move Z Screw Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                MSystem.m_pTrsScrew[m_idx].DriverStop();
                                MSystem.m_pTrsScrew[m_idx].MoveReadyPosZ();
                                return false;
                            }
                            break;
                    }
                    Thread.Sleep(10);
                }
                return true;
                //});

            }

        }

        private void BT_LIST_BACKUP_Click(object sender, EventArgs e)
        {

            string unit = m_idx == 0 ? "Left" : "Right";
            string sourceFileName = Config.ModelSavePath + InforManager.Instance.ModelName + ".json";
            string destFileName = Config.ModelSavePath + InforManager.Instance.ModelName + $"_{unit}_" + "_ScrewDataBackup.json";

            try
            {
                // Ensure the destination directory exists
                string destinationDirectory = Path.GetDirectoryName(destFileName);
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                // Copy the file
                File.Copy(sourceFileName, destFileName);
                //MSystem.MyMsgMemo("Model File backup copied successfully.","BACKUP");
            }
            catch
            {
                
            }
            //string path = Config.ModelSavePath + "InforManager.Instance.ModelName" + "_ScrewDataBackup.ini";
            //IniFile ini;
            //ini = new IniFile(path);
            //string section = m_idx == 0 ? "LEFT" : "RIGHT";
            //string value = "";
            //ini.Write(section, null, null);
            //for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
            //{
            //    string key = (i+1).ToString();
            //    value = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X.ToString() + "," +
            //                   InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y.ToString() + "," +
            //                   InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Z.ToString();
            //    ini.Write(section, key, value);
            //}
            //value = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT, (int)PScrewMain.ePOS_PICKUP].X.ToString() + ","+
            //InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT, (int)PScrewMain.ePOS_PICKUP].Y.ToString() + ","+
            //InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT, (int)PScrewMain.ePOS_PICKUP].Z.ToString();
            //ini.Write(section, "PICKUP", value);

            //value = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT, (int)PScrewMain.ePOS_TRASH].X.ToString() + "," +
            //InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT, (int)PScrewMain.ePOS_TRASH].Y.ToString() + "," +
            //InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT, (int)PScrewMain.ePOS_TRASH].Z.ToString();
            //ini.Write(section, "TRASH", value);

            //value = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT, (int)PScrewMain.ePOS_TRASH].X.ToString() + "," +
            //InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT, (int)PScrewMain.ePOS_TRASH].Y.ToString() + "," +
            //InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT, (int)PScrewMain.ePOS_TRASH].Z.ToString();
            //ini.Write(section, "READY", value);
            MSystem.MyMsgMemo("Position List Backup Sucess", "Save",msgButton.OK, msgIcon.Infor);
        }

        private void BT_LIST_RESTORE_Click(object sender, EventArgs e)
        {
            if (MSystem.MyMsgMemo("Would you like to restore the screw position list?", "Restore", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
                return;
            //IniFile ini;
            //string path = Config.ModelSavePath + "InforManager.Instance.ModelName" + "_ScrewDataBackup.ini";
            //ini = new IniFile(path);
            //string section = m_idx == 0 ? "LEFT" : "RIGHT";
            //for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
            //{
            //    string key = (i + 1).ToString();
            //    string value = string.Empty;
            //    value = ini.Read(section, key, "");
            //    string[] split = value.Split(',');
            //    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X = Convert.ToDouble(split[0]);
            //    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y = Convert.ToDouble(split[1]);
            //    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Z = Convert.ToDouble(split[2]);
            //}
            string unit = m_idx == 0 ? "Left" : "Right";
            string FileName = Config.ModelSavePath + InforManager.Instance.ModelName + $"_{unit}_" + "_ScrewDataBackup.json";
            if (File.Exists(FileName))
            {
                InforTeaching.Instance.LoadSettingPath(FileName);
            }
            MSystem.m_pListScrew.LoadDataScrew(m_idx);
            InforTeaching.Instance.SaveSettings();
        }

        private void radioButtonPreset1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                textTorque.Text = "0.00";
                textTorqueMinMax.Text = "00";
                int address = 2;

                ushort[] temp = MSystem.m_pHantas[m_idx].MbReadHoldingRegister(0x01, (ushort)address, 7);
                if (temp != null && temp.Length == 7)
                {
                    textTorque.Text = Convert.ToString(temp[0] / 100.0);
                    textTorqueMinMax.Text = Convert.ToString(temp[6]);
                }
                else
                {
                    MSystem.MyMsgMemo("Screw Torque Value Read Fail", "Error", msgButton.OK, msgIcon.Error);
                }
            }
            catch (Exception)
            {
                MSystem.MyMsgMemo("Screw Torque Value Read Fail", "Error", msgButton.OK, msgIcon.Error);
                buttonHantasSetting.Enabled = true;
            }
        }

        private void radioButtonPreset2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                textTorque.Text = "0.00";
                textTorqueMinMax.Text = "00";
                int address = 17;

                ushort[] temp = MSystem.m_pHantas[m_idx].MbReadHoldingRegister(0x01, (ushort)address, 7);
                if (temp != null && temp.Length == 7)
                {
                    textTorque.Text = Convert.ToString(temp[0] / 100.0);
                    textTorqueMinMax.Text = Convert.ToString(temp[6]);
                }
                else
                {
                    MSystem.MyMsgMemo("Screw Torque Value Read Fail", "Error", msgButton.OK, msgIcon.Error);
                }
            }
            catch (Exception)
            {
                MSystem.MyMsgMemo("Screw Torque Value Read Fail", "Error", msgButton.OK, msgIcon.Error);
                buttonHantasSetting.Enabled = true;
            }
        }

        private void radioButtonPreset3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                textTorque.Text = "0.00";
                textTorqueMinMax.Text = "00";
                int address = 32;

                ushort[] temp = MSystem.m_pHantas[m_idx].MbReadHoldingRegister(0x01, (ushort)address, 7);
                if (temp != null && temp.Length == 7)
                {
                    textTorque.Text = Convert.ToString(temp[0] / 100.0);
                    textTorqueMinMax.Text = Convert.ToString(temp[6]);
                }
                else
                {
                    MSystem.MyMsgMemo("Screw Torque Value Read Fail", "Error", msgButton.OK, msgIcon.Error);
                }
            }
            catch (Exception)
            {
                MSystem.MyMsgMemo("Screw Torque Value Read Fail", "Error", msgButton.OK, msgIcon.Error);
                buttonHantasSetting.Enabled = true;
            }
        }


        /* private void BT_ALIGN_ClickEvent(object sender, EventArgs e)
{
//return;
double _dx = 0.00;
double _dy = 0.00;
double _dz = 0.00;
bool LastPos = false;
for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
{
if (InforTeaching.Instance.P_Screw[m_idx][i].Vision == true)
{

if (LastPos == false)
{
if (m_idx == 0)
p1Old.X = InforTeaching.Instance.P_ScrewMaster[i].X;
else
p1Old.X = (-1)*InforTeaching.Instance.P_ScrewMaster[i].X;


p1Old.Y = (-1) * InforTeaching.Instance.P_ScrewMaster[i].Y;
// p1Old.Z = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Z;

p1New.X = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X; 
p1New.Y = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y;
//  p1New.Z = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Z;
LastPos = true;
}
else
{
if (m_idx == 0)
p2Old.X = InforTeaching.Instance.P_ScrewMaster[i].X;
else
p2Old.X = (-1) * InforTeaching.Instance.P_ScrewMaster[i].X;

p2Old.Y = (-1) * InforTeaching.Instance.P_ScrewMaster[i].Y;
// p1Old.Z = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Z;

p2New.X = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X;
p2New.Y = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y;
//  p1New.Z = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Z;
//  return true;

try
{
/////////////////////////////////Rotation///////////////////////////////
double p2NewXDist = p2New.X - p1New.X;
double p2NewYDist = p2New.Y - p1New.Y;
double p2OldXDist = p2Old.X - p1Old.X;
double p2OldYDist = p2Old.Y - p1Old.Y;

double p2NewXDistSquare = Math.Pow(p2NewXDist, 2);
double p2NewYDistSquare = Math.Pow(p2NewYDist, 2);
double p2OldXDistSquare = Math.Pow(p2OldXDist, 2);
double p2OldYDistSquare = Math.Pow(p2OldYDist, 2);

double newDist = Math.Sqrt(p2NewXDistSquare + p2NewYDistSquare);
double oldDist = Math.Sqrt(p2OldXDistSquare + p2OldYDistSquare);

double degreeNew = Math.Atan2(p2NewYDist, p2NewXDist);
double degreeOld = Math.Atan2(p2OldYDist, p2OldXDist);
double degreeDist = degreeNew - degreeOld;
double sin = Math.Sin(degreeDist);
double cos = Math.Cos(degreeDist);

double newCenterX = (p1New.X + p2New.X) / 2;
double newCenterY = (p1New.Y + p2New.Y) / 2;
double newCenterZ = (p1New.Z + p2New.Z) / 2;
double oldCenterX = (p1Old.X + p2Old.X) / 2;
double oldCenterY = (p1Old.Y + p2Old.Y) / 2;

double convertX;
double convertY;
double x;
double y;
for (int index = 0; index < InforTeaching.Instance.P_Screw[m_idx].Count; index++)
{
if (m_idx == 0)
{
x = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.X - oldCenterX;
y = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Y - oldCenterY;
}
else
{
x = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.X - oldCenterX;
y = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Y - oldCenterY;
}


convertX = cos * x - sin * y;
convertY = sin * x + cos * y;

InforTeaching.Instance.P_Screw[m_idx][index].PScrew.X = convertX + newCenterX;
InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Y = convertY + newCenterY;
//  InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Z = newCenterZ;
}



///////////////////////Nội Suy///////////////////////////////////////
*/

        /*

MSystem.m_pListScrew.LoadDataScrew(m_idx);
MSystem.m_pListScrew.SelectIndex();
InforTeaching.Instance.SaveSettings();
}
catch { }




}
}
}
}*/
    }
}
