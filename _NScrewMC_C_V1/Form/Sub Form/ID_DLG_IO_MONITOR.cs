using System;
using System.Drawing;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormIOMonitor : Form
    {
        #region Variable
        int IndexInput;
        int IndexOutPut;
        //Image IO_ON = Image.FromFile("Image/res/On.ico");
        //Image IO_OFF = Image.FromFile("Image/res/Off.ico");
        Timer UpdateStatus = new Timer();
        static Button[] InPort = new Button[16];
        static Button[] OutPort = new Button[16];

        public static SUserControls.ColorButton[] LbXY = new SUserControls.ColorButton[4];
        int index = 0;
        #endregion

        #region//Form Init
        public FormIOMonitor()
        {
            InitializeComponent();
            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + ((FormMain.m_Width - this.Width) / 2)
                                    , FormMain.GetLocation().Y + ((FormMain.m_Height - this.Height) / 2));
            this.Load += new EventHandler(LoadData);
            this.Closed += new EventHandler(CloseAllData);
            ///////////////////////////////////////////////////////
            BtExit.Click += new EventHandler(PreLoadEventInput);
            BtFirstIn.Click += new EventHandler(PreLoadEventInput);
            BtPreviewIn.Click += new EventHandler(PreLoadEventInput);
            BtNextIn.Click += new EventHandler(PreLoadEventInput);
            BtLastIn.Click += new EventHandler(PreLoadEventInput);
            //////////////////////////////////////////////////////
            BtFirstOut.Click += new EventHandler(PreLoadEventOut);
            BtPreviewOut.Click += new EventHandler(PreLoadEventOut);
            BtNextOut.Click += new EventHandler(PreLoadEventOut);
            BtLastOut.Click += new EventHandler(PreLoadEventOut);
            /////////////////////////////////////////////////////
            UpdateStatus.Tick += new EventHandler(ScanIOStatus);
            /////////////////////////////////////////////////////
            index = 0;
            LbXY[0] = BtX1; index++;
            LbXY[1] = BtX2; index++;
            LbXY[2] = BtY1; index++;
            LbXY[3] = BtY2; index++;
            /////////////////////////////////////////////////////
            index = 0;
            InPort[index] = in0; index++;
            InPort[index] = in1; index++;
            InPort[index] = in2; index++;
            InPort[index] = in3; index++;
            InPort[index] = in4; index++;
            InPort[index] = in5; index++;
            InPort[index] = in6; index++;
            InPort[index] = in7; index++;
            InPort[index] = in8; index++;
            InPort[index] = in9; index++;
            InPort[index] = inA; index++;
            InPort[index] = inB; index++;
            InPort[index] = inC; index++;
            InPort[index] = inD; index++;
            InPort[index] = inE; index++;
            InPort[index] = inF; index++;
            /////////////////////////////////////////////////////
            index = 0;
            OutPort[index] = out0; index++;
            OutPort[index] = out1; index++;
            OutPort[index] = out2; index++;
            OutPort[index] = out3; index++;
            OutPort[index] = out4; index++;
            OutPort[index] = out5; index++;
            OutPort[index] = out6; index++;
            OutPort[index] = out7; index++;
            OutPort[index] = out8; index++;
            OutPort[index] = out9; index++;
            OutPort[index] = outA; index++;
            OutPort[index] = outB; index++;
            OutPort[index] = outC; index++;
            OutPort[index] = outD; index++;
            OutPort[index] = outE; index++;
            OutPort[index] = outF; index++;
            /////////////////////////////////////////////////////
            for (index = 0; index < OutPort.Length; index++)
            {
                OutPort[index].Click += new EventHandler(PreLoadSetOutput);
            }
        }
        private void LoadData(object sender, EventArgs e)
        {
            UpdateStatus.Interval = 15;
            UpdateStatus.Enabled = true;
            IndexInput = IndexOutPut = 0;
            LoadTextButton();
        }
        private void CloseAllData(object sender, EventArgs e)
        {
            UpdateStatus.Dispose();
            this.Dispose();
            UpdateStatus = null;
        }

        #endregion

        #region//Event Form
        private void PreLoadEventInput(object sender, EventArgs e)
        {
            switch ((sender as Button).Text)
            {
                case "Exit":
                    this.Close();
                    break;
                case "First":
                    IndexInput = 0;
                    break;
                case "Last":
                    IndexInput = ((IOMap.IN["IN_MAX"] - SystemIO.Input_Origin) / 16) - 1;
                    break;
                case "Preview":
                    if (IndexInput > 0) IndexInput--;
                    break;
                case "Next":
                    if (IndexInput < ((IOMap.IN["IN_MAX"] - SystemIO.Input_Origin) / 16) - 1) IndexInput++;
                    break;
                default: break;
            }
            LoadTextButton();
        }
        private void PreLoadEventOut(object sender, EventArgs e)
        {
            switch ((sender as Button).Text)
            {
                case "First":
                    IndexOutPut = 0;
                    break;
                case "Last":
                    IndexOutPut = ((int)(IOMap.OUT["OUT_MAX"] - SystemIO.Output_Origin) / 16) - 1;
                    break;
                case "Preview":
                    if (IndexOutPut > 0) IndexOutPut--;
                    break;
                case "Next":
                    if (IndexOutPut < ((int)(IOMap.OUT["OUT_MAX"] - SystemIO.Output_Origin) / 16) - 1) IndexOutPut++;
                    break;
                default: break;
            }
            LoadTextButton();
        }
        private void PreLoadSetOutput(object sender, EventArgs e)
        {
            if (MSystem.SysStatus != StatusRun.RUN)
            {
                string[] strIO = (sender as Button).Text.Split(':');
                strIO[1] = strIO[1].Trim();
                //IO getData = (IO)Enum.Parse(typeof(IO), strIO[1]);
                if (MSystem.m_pDIO.IsOn(IOMap.OUT[strIO[1]]))
                {
                    MSystem.m_pDIO.OutPutOff(IOMap.OUT[strIO[1]]);
                }
                else
                {
                    MSystem.m_pDIO.OutPutOn(IOMap.OUT[strIO[1]]);
                }
                LoadTextButton();
            }
        }
        private void ScanIOStatus(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    LoadDataIN();
                    LoadTextButton();
                }));
            }
            else
            {
                LoadDataIN();
                LoadTextButton();
            }
        }
        #endregion

        #region //Tool Fuction
        private void LoadDataIN()
        {
            //Input
            int temp = (int)SystemIO.Input_Origin + IndexInput * 16;
            for (int i = temp; i < temp + 16; i++)
            {
                //IOName = (IO)i;
                if (MSystem.m_pDIO.IsOn(i))
                {
                    if (InPort[i - temp].BackColor != Color.LawnGreen)
                    {
                        SetcolorON(InPort[i - temp]);
                        //InPort[i - temp].Image = IO_ON;
                    }
                }
                else
                {
                    if (InPort[i - temp].BackColor != Color.White)
                    {
                        SetcolorOFF(InPort[i - temp]);
                        //InPort[i - temp].Image = IO_OFF;
                    }
                }
            }
        }
        public void LoadTextButton()
        {
            //OutPut
            int temp = (int)SystemIO.Output_Origin + IndexOutPut * 16;
            for (int i = temp; i < temp + 16; i++)
            {
                //IOName = (IO)i;
                if (MSystem.m_pDIO.IsOn(i))
                {
                    if (OutPort[i - temp].BackColor != Color.LawnGreen)
                        SetcolorON(OutPort[i - temp]);
                }
                else
                {
                    if (OutPort[i - temp].BackColor != Color.White)
                        SetcolorOFF(OutPort[i - temp]);
                }
                OutPort[i - temp].Text = "Y" + (i - (int)SystemIO.Output_Origin).ToString("X3") + ":\r\n" + IOMap.OUT_Name[i];
            }
            LbXY[2].Text = "Y" + (temp - (int)SystemIO.Output_Origin).ToString("X3") + " - " + "Y" + (temp + 7 - (int)SystemIO.Output_Origin).ToString("X3");
            LbXY[3].Text = "Y" + (temp + 8 - (int)SystemIO.Output_Origin).ToString("X3") + " - " + "Y" + (temp + 15 - (int)SystemIO.Output_Origin).ToString("X3");

            //Input
            temp = (int)SystemIO.Input_Origin + IndexInput * 16;
            for (int i = temp; i < temp + 16; i++)
            {
                //IOName = (IO)i;
                InPort[i - temp].Text = "X" + (i - (int)SystemIO.Input_Origin).ToString("X3") + ":\r\n" + IOMap.IN_Name[i];
            }
            LbXY[0].Text = "X" + (temp - (int)SystemIO.Input_Origin).ToString("X3") + " - " + "X" + (temp + 7 - (int)SystemIO.Input_Origin).ToString("X3");
            LbXY[1].Text = "X" + (temp + 8 - (int)SystemIO.Input_Origin).ToString("X3") + " - " + "X" + (temp + 15 - (int)SystemIO.Input_Origin).ToString("X3");
        }
        void SetcolorON(SUserControls.ColorButton _bt)
        {
            _bt.GradientBottom = Color.LawnGreen;
            _bt.GradientTop = Color.Lime;
        }
        void SetcolorOFF(SUserControls.ColorButton _bt)
        {
            _bt.GradientBottom = Color.Khaki;
            _bt.GradientTop = Color.FromArgb(255, 255, 192);
        }
        void SetcolorON(Button _bt)
        {
            _bt.BackColor = Color.LawnGreen;
        }
        void SetcolorOFF(Button _bt)
        {
            _bt.BackColor = Color.White;
        }

        private void BtExit_Click(object sender, EventArgs e)
        {
            MSystem.m_bIsTeachDlg = false;
        }

        private void FormIOMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            MSystem.m_bIsTeachDlg = false;
        }

        private void FormIOMonitor_FormClosed(object sender, FormClosedEventArgs e)
        {
            MSystem.m_bIsTeachDlg = false;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        //end code
    }
}
