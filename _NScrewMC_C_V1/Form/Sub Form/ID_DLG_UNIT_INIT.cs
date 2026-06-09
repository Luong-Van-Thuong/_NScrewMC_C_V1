using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormUnitInit : Form
    {
        public const int SUCCESS = 0;
        CBbutton[] m_btUnit = new CBbutton[(int)Unit.eUNIT_MAX];
        #region// INIT BUTTON
        public FormUnitInit()
        {
            InitializeComponent();
            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + (FormMain.m_Width - this.Width) / 2, FormMain.GetLocation().Y + (FormMain.m_Height - this.Height) / 2);
            InitButton();
            this.FormClosed += new FormClosedEventHandler(CloseForm);
            MSystem.m_bIsTeachDlg = true;

            if (MSystem.IsAutoInitillize == true)
            {
                timer1.Interval = 200;
                timer1.Start();
            }
            if (InforManager.Instance.IsLeftUnitUse == false)
                BT_UNIT_1.Enabled = false;
            if (InforManager.Instance.IsRightUnitUse == false)
                BT_UNIT_2.Enabled = false;
        }
        ~FormUnitInit()
        {

        }
        private void CloseForm(object sender, EventArgs e)
        {
            MSystem.m_bIsTeachDlg = false;
        }
        void InitButton()
        {
            m_btUnit[(int)Unit.eUNIT_SCREW_1] = new CBbutton(BT_UNIT_1, false);
            m_btUnit[(int)Unit.eUNIT_SCREW_2] = new CBbutton(BT_UNIT_2, false);
        }
        private void PreLoadEvent(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)Unit.eUNIT_MAX; i++)
            {
                if ((sender as Button) == m_btUnit[i].Button)
                {
                    if (m_btUnit[i].m_iSel)
                    {
                        m_btUnit[i].SetValue(CBbutton.NormalColor);
                    }
                    else
                    {
                        m_btUnit[i].SetValue(CBbutton.SelectColor);
                    }
                }
            }
        }
#endregion

        private void BtExit_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void colorButton2_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < (int)Unit.eUNIT_MAX; index++)
            {
                if (index == 0)
                {
                    if (InforManager.Instance.IsLeftUnitUse == false)
                        continue;
                }
                if (index == 1)
                {
                    if (InforManager.Instance.IsRightUnitUse == false)
                        continue;
                }

                m_btUnit[index].SetValue(CBbutton.SelectColor);
            }
        }

        private void colorButton3_Click(object sender, EventArgs e)
        {
            for (int index = 0; index < (int)Unit.eUNIT_MAX; index++)
            {
                m_btUnit[index].SetValue(CBbutton.NormalColor);
            }
        }

        private void BT_UNIT_Click(object sender, EventArgs e)
        {
            try
            {
                UnitIntilaze();
            }
            catch (Exception)
            {
                MSystem.MyMessagerBottom("Unit Init Fail");
            }
        }
        public void UnitIntilaze()
        {
            // INIT
            bool[] iUnitSelect = new bool[(int)Unit.eUNIT_MAX];
            int Result = 100;
            int _NumUnit = (int)Unit.eUNIT_MAX;
            bool m_iSelect = false;

            for (int i = 0; i < _NumUnit; i++)
            {
                if (m_btUnit[i].m_iSel) iUnitSelect[i] = true;
                else iUnitSelect[i] = false;
            }

            /*Check Get Select*/
            for (int i = 0; i < _NumUnit; i++)
            {
                if (m_btUnit[i].m_iSel)
                {
                    m_iSelect = true;
                }
            }

            /*Check Select*/
            if (!m_iSelect)
            {
                MSystem.MyMsgMemo("No Select Unit!", "Unit Init", msgButton.OK, msgIcon.Question);
                return;
            }

            /*Question for Unit*/
            if (MSystem.IsAutoInitillize != true)
            {
                if (MSystem.MyMsgMemo("Are you want initial unit Select ?", "Unit Init", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }
            MSystem.IsInitillizeStatus = false;

            var TaskRun = Task.Run(async () =>
            {
                await Task.Delay(200);

                //Screw Left
                MSystem.SetMsgDisplay($"Init Unit {Unit.eUNIT_SCREW_1.ToString()} ......");
                if (iUnitSelect[(int)Unit.eUNIT_SCREW_1] && Result == 100)
                {
                    if (MSystem.m_pTrsScrew[Constants.left].SetUnitInitialize() != SUCCESS)
                        Result = 0;

                    if (Result == 100 && MSystem.m_pTrsJig[Constants.left].SetUnitInitialize() != SUCCESS)
                        Result = 0;

                    if (Result == 100)
                        m_btUnit[Constants.left].SetValue(CBbutton.NormalColor);
                }

                //Screw Right
                MSystem.SetMsgDisplay($"Init Unit {Unit.eUNIT_SCREW_2.ToString()} ......");
                if (iUnitSelect[Constants.right] && Result == 100)
                {
                    if (MSystem.m_pTrsScrew[Constants.right].SetUnitInitialize() != SUCCESS)
                        Result = 1;

                    if (Result == 100 && MSystem.m_pTrsJig[Constants.right].SetUnitInitialize() != SUCCESS)
                        Result = 1;

                    if (Result == 100)
                        m_btUnit[Constants.right].SetValue(CBbutton.NormalColor);
                }

                if (Result != 100)
                {
                    MSystem.KillMsgDisplay();
                    MSystem.MyMsgMemo($"Init {(Unit)Result} Fail! \r\nPLEASE  DOOR RESET.", "Unit Init", msgButton.OK, msgIcon.Infor);
                }
                else
                {
                    //MSystem.KillMsgDisplay();
                    MSystem.IsInitillizeStatus = true;
                    MSystem.MyMsgMemo("Init all Ok! Ready for run", "Unit Init", msgButton.OK, msgIcon.Success);
                }

                MSystem.KillMsgDisplay();
            });

            MSystem.MyMsgDisplay("Start Init Unit ........\r\nWait a moment ...");
        }

        private void FormUnitInit_Load(object sender, EventArgs e)
        {

        }

        private void BT_UNIT_1_Click(object sender, EventArgs e)
        {
            if (m_btUnit[Constants.left].m_iSel)
            {
                m_btUnit[Constants.left].SetValue(CBbutton.NormalColor);
            }
            else
            {
                m_btUnit[Constants.left].SetValue(CBbutton.SelectColor);
            }
        }

        private void BT_UNIT_2_Click(object sender, EventArgs e)
        {
            if (m_btUnit[Constants.right].m_iSel)
            {
                m_btUnit[Constants.right].SetValue(CBbutton.NormalColor);
            }
            else
            {
                m_btUnit[Constants.right].SetValue(CBbutton.SelectColor);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            m_btUnit[(int)Unit.eUNIT_SCREW_1].m_iSel = true;
            m_btUnit[(int)Unit.eUNIT_SCREW_2].m_iSel = true;
            
            try
            {
                UnitIntilaze();
            }
            catch (Exception)
            {
                MSystem.MyMessagerBottom("Unit Init Fail");
            }
            Thread.Sleep(500);
            this.Close();
        }
    }
}
