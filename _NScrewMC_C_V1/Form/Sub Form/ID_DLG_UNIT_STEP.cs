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
    public partial class FormUnitStep : Form
    {
        Timer _updateStep = new Timer();
        public FormUnitStep()
        {
            InitializeComponent();
            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + (FormMain.m_Width - this.Width) / 2, FormMain.GetLocation().Y + (FormMain.m_Height - this.Height) / 2);
            this.FormClosed += new FormClosedEventHandler(FormClose);
            _updateStep.Tick += new EventHandler(UpdateData);
            _updateStep.Interval = 100;
            _updateStep.Start();
        }
        private void FormClose(object sender, FormClosedEventArgs e) {
            _updateStep.Stop();
            _updateStep = null;
        }

        private void UpdateData(object sender, EventArgs e)
        {
            IDC_SCREW_LEFT.Text = $"{MSystem.m_pTrsScrew[(int)_NSC.eLEFT].m_iCurrentStep} - {MSystem.m_pTrsJig[(int)_NSC.eLEFT].m_iCurrentStep}";
            IDC_SCREW_RIGHT.Text = $"{MSystem.m_pTrsScrew[(int)_NSC.eRIGHT].m_iCurrentStep} - {MSystem.m_pTrsJig[(int)_NSC.eRIGHT].m_iCurrentStep}";
           
        }
        private void BT_EXIT_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
