using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormTeach : Form
    {
        public FormTeach()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;
            this.Location = new Point(0, 0);
        }
        private void BT_SCREW_LEFT_Click(object sender, EventArgs e)
        {
            FormTeachScrew TeachScrew = new FormTeachScrew(Constants.left);
            TeachScrew.ShowDialog();

            //MSystem.NumDisplay = (int)NumViewMain.eviewTeadScrew;

            //MSystem.TeachScrew.m_idx = 0;
            //MSystem.m_pTrsJog._JogMode = 0;
            //MSystem.TeachScrew.LoadJogModeDisplay();

            //MSystem.TeachScrew.BT_SELECT_SCREW.Caption = "SCREW LEFT";
            //MSystem.TeachScrew.LV_LIST_SCREW.Items.Clear();
            //MSystem.m_pListScrew.LoadDataScrew(MSystem.TeachScrew.m_idx);
            //MSystem.m_pListScrew.SelectIndex();

            //MSystem.TeachScrew.SelectButton(MSystem.TeachScrew.m_btPoselect[(int)FormTeachScrew.PosTeach.ePOS_SCREW]);

            //if (MSystem.TeachScrew.LV_LIST_SCREW.Items.Count < MSystem.TeachScrew._numRowsCheck)
            //    MSystem.TeachScrew.LV_LIST_SCREW.Columns[6].Width = 88;
            //else
            //    MSystem.TeachScrew.LV_LIST_SCREW.Columns[6].Width = 74;
        }
        private void BT_SCREW_MIDDLE_ClickEvent(object sender, EventArgs e)
        {
            FormTeachScrew TeachScrew = new FormTeachScrew(Constants.right);
            TeachScrew.ShowDialog();
            //MSystem.NumDisplay = (int)NumViewMain.eviewTeadScrew;

            //MSystem.TeachScrew.m_idx = 1;
            //MSystem.m_pTrsJog._JogMode = 0;
            //MSystem.TeachScrew.LoadJogModeDisplay();

            //MSystem.TeachScrew.BT_SELECT_SCREW.Caption = "SCREW RIGHT";
            //MSystem.TeachScrew.LV_LIST_SCREW.Items.Clear();
            //MSystem.m_pListScrew.LoadDataScrew(MSystem.TeachScrew.m_idx);
            //MSystem.m_pListScrew.SelectIndex();
            //MSystem.TeachScrew.SelectButton(MSystem.TeachScrew.m_btPoselect[(int)FormTeachScrew.PosTeach.ePOS_SCREW]);

            //if (MSystem.TeachScrew.LV_LIST_SCREW.Items.Count < MSystem.TeachScrew._numRowsCheck)
            //    MSystem.TeachScrew.LV_LIST_SCREW.Columns[6].Width = 88;
            //else
            //    MSystem.TeachScrew.LV_LIST_SCREW.Columns[6].Width = 74;
        }

        private void BT_TEST_Click(object sender, EventArgs e)
        {
            ScrewTest st = new ScrewTest();
            st.ShowDialog();
        }
    }
}
