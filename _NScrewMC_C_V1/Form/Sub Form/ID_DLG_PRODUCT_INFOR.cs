using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormproductInfor : Form
    {
        Timer _updateStatus = new Timer();
        // Variable
        public int m_iIdxPage = 0;
        SUserControls.ColorButton[] m_btPostSCREW = new SUserControls.ColorButton[5];
        SUserControls.ColorButton[] m_btLeftCount = new SUserControls.ColorButton[5];
        SUserControls.ColorButton[] m_btRightCount = new SUserControls.ColorButton[5];
        SUserControls.ColorButton[] m_btLeftCountVision = new SUserControls.ColorButton[5];
        SUserControls.ColorButton[] m_btRightCountVision = new SUserControls.ColorButton[5];

        DetailView detailView;
        public FormproductInfor()
        {
            InitializeComponent();
            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + (FormMain.m_Width - this.Width) / 2, FormMain.GetLocation().Y + (FormMain.m_Height - this.Height) / 2);
            //
            int i = 0;
            m_btPostSCREW[i++] = P00;
            m_btPostSCREW[i++] = P01;
            m_btPostSCREW[i++] = P02;
            m_btPostSCREW[i++] = P03;
            m_btPostSCREW[i++] = P04;

            i = 0;
            m_btLeftCount[i++] = L_CNT_P00;
            m_btLeftCount[i++] = L_CNT_P01;
            m_btLeftCount[i++] = L_CNT_P02;
            m_btLeftCount[i++] = L_CNT_P03;
            m_btLeftCount[i++] = L_CNT_P04;
            i = 0;
            m_btLeftCountVision[i++] = L_CNT_P00_VISION;
            m_btLeftCountVision[i++] = L_CNT_P01_VISION;
            m_btLeftCountVision[i++] = L_CNT_P02_VISION;
            m_btLeftCountVision[i++] = L_CNT_P03_VISION;
            m_btLeftCountVision[i++] = L_CNT_P04_VISION;

            i = 0;
            m_btRightCount[i++] = M_CNT_P00;
            m_btRightCount[i++] = M_CNT_P01;
            m_btRightCount[i++] = M_CNT_P02;
            m_btRightCount[i++] = M_CNT_P03;
            m_btRightCount[i++] = M_CNT_P04;
            i = 0;
            m_btRightCountVision[i++] = M_CNT_P00_VISION;
            m_btRightCountVision[i++] = M_CNT_P01_VISION;
            m_btRightCountVision[i++] = M_CNT_P02_VISION;
            m_btRightCountVision[i++] = M_CNT_P03_VISION;
            m_btRightCountVision[i++] = M_CNT_P04_VISION;

            //for (int j = 0; j < 14; j++)
            //{
            //    InforProduct.Instance.ProductDetail[MSystem.eRIGHT][j].m_dCountFail = j * 2;
            //    InforProduct.Instance.ProductDetail[MSystem.eLEFT][j].m_dCountFail = j * 2;

            //    InforProduct.Instance.ProductDetail[MSystem.eLEFT][j].DetailQueue.Enqueue($"Left P{j + 1} Error Detect");
            //    InforProduct.Instance.ProductDetail[MSystem.eRIGHT][j].DetailQueue.Enqueue($"Right P{j + 1} Error Detect");
            //}
            //InforProduct.Instance.SaveSettings();
            this.L_CNT_P00.Click += (s, e) => DetailValueClick(MSystem.eLEFT, 0);
            this.L_CNT_P01.Click += (s, e) => DetailValueClick(MSystem.eLEFT, 1);
            this.L_CNT_P02.Click += (s, e) => DetailValueClick(MSystem.eLEFT, 2);
            this.L_CNT_P03.Click += (s, e) => DetailValueClick(MSystem.eLEFT, 3);
            this.L_CNT_P04.Click += (s, e) => DetailValueClick(MSystem.eLEFT, 4);
            this.M_CNT_P00.Click += (s, e) => DetailValueClick(MSystem.eRIGHT, 0);
            this.M_CNT_P01.Click += (s, e) => DetailValueClick(MSystem.eRIGHT, 1);
            this.M_CNT_P02.Click += (s, e) => DetailValueClick(MSystem.eRIGHT, 2);
            this.M_CNT_P03.Click += (s, e) => DetailValueClick(MSystem.eRIGHT, 3);
            this.M_CNT_P04.Click += (s, e) => DetailValueClick(MSystem.eRIGHT, 4);


            m_iIdxPage = 0;
            _updateStatus.Tick += new EventHandler(UpdateData);
            this.Closed += new EventHandler(CloseAllData);
            _updateStatus.Interval = 50;
            _updateStatus.Start();

            if (InforManager.Instance.InspectionType == "VST"
                || InforManager.Instance.AutoVisionAlignMode == 0)
            {
                groupBoxVisionOnly.Visible = false;
                buttonPress1.Visible = false;
                buttonPress2.Visible = false;
                L_CNT_P00_VISION.Visible = false;
                L_CNT_P01_VISION.Visible = false;
                L_CNT_P02_VISION.Visible = false;
                L_CNT_P03_VISION.Visible = false;
                L_CNT_P04_VISION.Visible = false;
                M_CNT_P00_VISION.Visible = false;
                M_CNT_P01_VISION.Visible = false;
                M_CNT_P02_VISION.Visible = false;
                M_CNT_P03_VISION.Visible = false;
                M_CNT_P04_VISION.Visible = false;
            }
            else
            {
                groupBoxVisionOnly.Visible = true;
                buttonPress1.Visible = true;
                buttonPress2.Visible = true;
                L_CNT_P00_VISION.Visible = true;
                L_CNT_P01_VISION.Visible = true;
                L_CNT_P02_VISION.Visible = true;
                L_CNT_P03_VISION.Visible = true;
                L_CNT_P04_VISION.Visible = true;
                M_CNT_P00_VISION.Visible = true;
                M_CNT_P01_VISION.Visible = true;
                M_CNT_P02_VISION.Visible = true;
                M_CNT_P03_VISION.Visible = true;
                M_CNT_P04_VISION.Visible = true;
            }
            if (InforManager.Instance.IsBarcodeUse == false)
            {
                LBL_TOTAL_BCR_NG.Enabled = false;
                IDC_LEFT_BCR_NG.Enabled = false;
                IDC_RIGHT_BCR_NG.Enabled = false;
                LBL_TOTAL_BCR_NG.GradientBottom = Color.Gray;
                LBL_TOTAL_BCR_NG.GradientTop = Color.Gray;
                IDC_LEFT_BCR_NG.GradientBottom = Color.Gray;
                IDC_LEFT_BCR_NG.GradientTop = Color.Gray;
                IDC_RIGHT_BCR_NG.GradientBottom = Color.Gray;
                IDC_RIGHT_BCR_NG.GradientTop = Color.Gray;
            }
        }
        private void UpdateData(object sender, EventArgs e)
        {
            /*Show Hide Button*/
            MSystem.IsSensorOn(IDC_PRODUCT_INFOR_SHOW, InforManager.Instance.ProductInfor, 0);
            MSystem.IsSensorOn(IDC_PRODUCT_INFOR_HIDE, !InforManager.Instance.ProductInfor, 0);
            UpdatePointName();
            /*Update Infor data Product*/

            //REAR

            // RIGHT
            long total_mid = InforProduct.Instance.ProductScrew_Pass[1] + InforProduct.Instance.ProductScrew_Fail[1];
            MSystem.DisPlayString(IDC_MID_INPUT, $"{total_mid}");
            MSystem.DisPlayString(IDC_MID_OK, $"{InforProduct.Instance.ProductScrew_Pass[1]}");
            MSystem.DisPlayString(IDC_MID_NG, $"{InforProduct.Instance.ProductScrew_Fail[1]}");
            MSystem.DisPlayString(IDC_RIGHT_BCR_NG, $"{InforProduct.Instance.ProductBCR_Fail_R}");

            if (InforProduct.Instance.ProductScrew_Fail[1] > 0)
                MSystem.DisPlayString(IDC_MID_RATE, $"{(((float)InforProduct.Instance.ProductScrew_Fail[1] / total_mid) * 100).ToString("f02")} %");
            else
                MSystem.DisPlayString(IDC_MID_RATE, "0.00%");

            // LEFT
            long total_front = InforProduct.Instance.ProductScrew_Pass[0] + InforProduct.Instance.ProductScrew_Fail[0];
            MSystem.DisPlayString(IDC_FRONT_INPUT, $"{total_front}");
            MSystem.DisPlayString(IDC_FRONT_OK, $"{InforProduct.Instance.ProductScrew_Pass[0]}");
            MSystem.DisPlayString(IDC_FRONT_NG, $"{InforProduct.Instance.ProductScrew_Fail[0]}");
            MSystem.DisPlayString(IDC_LEFT_BCR_NG, $"{InforProduct.Instance.ProductBCR_Fail_L}");

            if (InforProduct.Instance.ProductScrew_Fail[0] > 0)
                MSystem.DisPlayString(IDC_FRONT_RATE, $"{(((float)InforProduct.Instance.ProductScrew_Fail[0] / total_front) * 100).ToString("f02")} %");
            else
                MSystem.DisPlayString(IDC_FRONT_RATE, "0.00%");

            // Update Total
            long sum_input =  total_mid + total_front;
            long sum_pass = InforProduct.Instance.ProductScrew_Pass[0] + InforProduct.Instance.ProductScrew_Pass[1];// + InforProduct.Instance.ProductScrew_Pass[2];
            long sum_fail = InforProduct.Instance.ProductScrew_Fail[0] + InforProduct.Instance.ProductScrew_Fail[1];// + InforProduct.Instance.ProductScrew_Fail[2];
            long sum_bcr_fail = InforProduct.Instance.ProductBCR_Fail_L + InforProduct.Instance.ProductBCR_Fail_R;
            MSystem.DisPlayString(LB_INPUT_TOTAL, $"{sum_input}");
            MSystem.DisPlayString(LB_OUTPUT_TOTAL, $"{sum_pass}");
            MSystem.DisPlayString(TB_NG_TOTAL, $"{sum_fail}");
            MSystem.DisPlayString(LBL_TOTAL_BCR_NG, $"{sum_bcr_fail}");
            if (sum_fail > 0)
                MSystem.DisPlayString(LB_NG_RATE, $"{(((float)sum_fail / sum_input) * 100).ToString("f02")} %");
            else
                MSystem.DisPlayString(LB_NG_RATE, "0.00%");

            // VISION

            //MIDD
            long total_mid_vision = InforProduct.Instance.ProductScrew_Pass_Vision[1] + InforProduct.Instance.ProductScrew_Fail_Vision[1];
            MSystem.DisPlayString(IDC_MID_INPUT_VISION, $"{total_mid_vision}");
            MSystem.DisPlayString(IDC_MID_OK_VISION, $"{InforProduct.Instance.ProductScrew_Pass_Vision[1]}");
            MSystem.DisPlayString(IDC_MID_NG_VISION, $"{InforProduct.Instance.ProductScrew_Fail_Vision[1]}");
            if (InforProduct.Instance.ProductScrew_Fail_Vision[1] > 0)
                MSystem.DisPlayString(IDC_MID_RATE_VISION, $"{(((float)InforProduct.Instance.ProductScrew_Fail_Vision[1] / total_mid_vision) * 100).ToString("f02")} %");
            else
                MSystem.DisPlayString(IDC_MID_RATE_VISION, "0.00%");

            //FRONT
            long total_front_vision = InforProduct.Instance.ProductScrew_Pass_Vision[0] + InforProduct.Instance.ProductScrew_Fail_Vision[0];
            MSystem.DisPlayString(IDC_FRONT_INPUT_VISION, $"{total_front_vision}");
            MSystem.DisPlayString(IDC_FRONT_OK_VISION, $"{InforProduct.Instance.ProductScrew_Pass_Vision[0]}");
            MSystem.DisPlayString(IDC_FRONT_NG_VISION, $"{InforProduct.Instance.ProductScrew_Fail_Vision[0]}");

            if (InforProduct.Instance.ProductScrew_Fail_Vision[0] > 0)
                MSystem.DisPlayString(IDC_FRONT_RATE_VISION, $"{(((float)InforProduct.Instance.ProductScrew_Fail_Vision[0] / total_front_vision) * 100).ToString("f02")} %");
            else
                MSystem.DisPlayString(IDC_FRONT_RATE_VISION, "0.00%");

            // Update Total Vision
            long sum_input_vision = total_mid_vision + total_front_vision;
            long sum_pass_vision = InforProduct.Instance.ProductScrew_Pass_Vision[0] + InforProduct.Instance.ProductScrew_Pass_Vision[1];// + InforProduct.Instance.ProductScrew_Pass[2];
            long sum_fail_vision = InforProduct.Instance.ProductScrew_Fail_Vision[0] + InforProduct.Instance.ProductScrew_Fail_Vision[1];// + InforProduct.Instance.ProductScrew_Fail[2];
            MSystem.DisPlayString(LB_INPUT_TOTAL_VISION, $"{sum_input_vision}");
            MSystem.DisPlayString(LB_OUTPUT_TOTAL_VISION, $"{sum_pass_vision}");
            MSystem.DisPlayString(TB_NG_TOTAL_VISION, $"{sum_fail_vision}");
            if (sum_fail_vision > 0)
                MSystem.DisPlayString(LB_NG_RATE_VISION, $"{(((float)sum_fail_vision / sum_input_vision) * 100).ToString("f02")} %");
            else
                MSystem.DisPlayString(LB_NG_RATE_VISION, "0.00%");

        }

        void UpdatePointName()
        {
            for (int i = 0; i < 5; i++)
            {
                m_btPostSCREW[i].Text = $"P{i + 1 + 5 * m_iIdxPage}";

                if (i + 5 * m_iIdxPage < InforTeaching.Instance.P_Screw[MSystem.eLEFT].Count)
                    m_btLeftCount[i].Text = InforProduct.Instance.ProductDetail[MSystem.eLEFT][i + 5 * m_iIdxPage].m_dCountFail.ToString();
                else
                    m_btLeftCount[i].Text = "0";

                if (i + 5 * m_iIdxPage < InforTeaching.Instance.P_Screw[MSystem.eRIGHT].Count)
                    m_btRightCount[i].Text = InforProduct.Instance.ProductDetail[MSystem.eRIGHT][i + 5 * m_iIdxPage].m_dCountFail.ToString();
                else
                    m_btRightCount[i].Text = "0";

                if (i + 5 * m_iIdxPage < InforTeaching.Instance.P_Screw[MSystem.eLEFT].Count)
                    m_btLeftCountVision[i].Text = InforProduct.Instance.ProductDetail[MSystem.eLEFT][i + 5 * m_iIdxPage].m_dCountFailVision.ToString();
                else
                    m_btLeftCountVision[i].Text = "0";

                if (i + 5 * m_iIdxPage < InforTeaching.Instance.P_Screw[MSystem.eRIGHT].Count)
                    m_btRightCountVision[i].Text = InforProduct.Instance.ProductDetail[MSystem.eRIGHT][i + 5 * m_iIdxPage].m_dCountFailVision.ToString();
                else
                    m_btRightCountVision[i].Text = "0";
            }
        }

        private void CloseAllData(object sender, EventArgs e)
        {
            _updateStatus.Stop();
            _updateStatus = null;
        }

        private void BT_EXIT_Click(object sender, EventArgs e)
        {
            // DetailView가 이미 실행 중인지 확인
            if (detailView != null && !detailView.IsDisposed)
            {
                detailView.Close(); // 기존 다이얼로그 종료
                detailView.Dispose();
                detailView = null;
            }
            this.Close();
        }

        private void IDC_PRODUCT_INFOR_SHOW_Click(object sender, EventArgs e)
        {
            InforManager.Instance.ProductInfor = true;
            InforManager.Instance.SaveData();
        }

        private void IDC_PRODUCT_INFOR_HIDE_Click(object sender, EventArgs e)
        {
            InforManager.Instance.ProductInfor = false;
            InforManager.Instance.SaveData();
        }
        //************************************************************
        public int CalculatePageScrewLeft()
        {
            int _calpage = 0;
            if ((InforTeaching.Instance.P_Screw[MSystem.eLEFT].Count % 5) == 0)
            {
                _calpage = (int)(InforTeaching.Instance.P_Screw[MSystem.eLEFT].Count / 5);
            }
            else
            {
                _calpage = (int)(InforTeaching.Instance.P_Screw[MSystem.eLEFT].Count / 5) + 1;
            }
            return _calpage;
        }

        public int CalculatePageScrewRight()
        {
            int _calpage = 0;

            if ((InforTeaching.Instance.P_Screw[MSystem.eRIGHT].Count % 5) == 0)
            {
                _calpage = (int)(InforTeaching.Instance.P_Screw[MSystem.eRIGHT].Count / 5);
            }
            else
            {
                _calpage = (int)(InforTeaching.Instance.P_Screw[MSystem.eRIGHT].Count / 5) + 1;
            }
            return _calpage;
        }
        private void BT_RST_CNT_FAIL_Click(object sender, EventArgs e)
        {
            // Left Screw Count RST
            for (int i = 0; i < InforTeaching.Instance.P_Screw[MSystem.eLEFT].Count; i++)
            {
                InforProduct.Instance.ProductDetail[MSystem.eLEFT][i].m_dCountFail = 0;
            }
            // Right Screw Count RST
            for (int i = 0; i < InforTeaching.Instance.P_Screw[MSystem.eRIGHT].Count; i++)
            {
                InforProduct.Instance.ProductDetail[MSystem.eRIGHT][i].m_dCountFail = 0;
            }
            for (int i = 0; i < 5; i++)
            {
                m_btLeftCount[i].Text = "";
                m_btRightCount[i].Text = "";
            }
            
            InforProduct.Instance.SaveSettings();
        }
        private int CaculateMax()
        {
            if (InforTeaching.Instance.P_Screw[MSystem.eLEFT].Count >= InforTeaching.Instance.P_Screw[MSystem.eRIGHT].Count)
                return MSystem.eLEFT;
            else
                return MSystem.eRIGHT;
        }
        private void BT_NEXT_Click(object sender, EventArgs e)
        {
            if (CaculateMax() == MSystem.eLEFT)
            {
                m_iIdxPage++;
                if (m_iIdxPage >= CalculatePageScrewLeft())
                    m_iIdxPage = CalculatePageScrewLeft() - 1;
            }
            else
            {
                m_iIdxPage++;
                if (m_iIdxPage >= CalculatePageScrewRight())
                    m_iIdxPage = CalculatePageScrewRight() - 1;
            }
        }

        private void BT_PREVIOUS_Click(object sender, EventArgs e)
        {
            m_iIdxPage--;
            if (m_iIdxPage < 1)
                m_iIdxPage = 0;
        }

        private void BT_TOTAL_RESET_Click(object sender, EventArgs e)
        {
            if (MSystem.MyMsgMemo("Are You Want Reset All Data Product ?", "Question", msgButton.YESNO, msgIcon.Question) == DialogResult.Yes)
            {
                MSystem.ResetAllProductInfor();
            }
        }

        private void BT_REAR_RESET_Click(object sender, EventArgs e)
        {
            if (MSystem.MyMsgMemo("Are You Want Reset Data ?", "Question", msgButton.YESNO, msgIcon.Question) == DialogResult.Yes)
            {
                MSystem.ResetInforJig(Constants.left);
            }
        }

        private void BT_MID_RESET_Click(object sender, EventArgs e)
        {
            if (MSystem.MyMsgMemo("Are You Want Reset Data ?", "Question", msgButton.YESNO, msgIcon.Question) == DialogResult.Yes)
            {
                MSystem.ResetInforJig(Constants.right);
            }
        }

        private void FormproductInfor_Load(object sender, EventArgs e)
        {
            InforTeaching.Instance.LoadSetting();
            InforProduct.Instance.LoadSetting();
        }

        private void DetailValueClick(int UnitIndex, int pos)
        {
            // DetailView가 이미 실행 중인지 확인
            if (detailView != null && !detailView.IsDisposed)
            {
                detailView.Close(); // 기존 다이얼로그 종료
                detailView.Dispose();
                detailView = null;
            }
            if (InforTeaching.Instance.P_Screw[UnitIndex].Count <= (m_iIdxPage * 5 + pos))
                return;
            if (InforProduct.Instance.ProductDetail[UnitIndex][m_iIdxPage * 5 + pos].m_dCountFail == 0)
            {
                return;
            }

            detailView = new DetailView(UnitIndex, m_iIdxPage * 5 + pos);
            detailView.StartPosition = FormStartPosition.Manual;
            detailView.Location = new Point(10, 10);//(this.Location.X + (this.Width - detailView.Width) / 2, this.Location.Y + (this.Height - detailView.Height) / 2);
            detailView.Show();
        }
        public void ClearDetailViewInstance()
        {
            detailView = null;
        }
    }
}
