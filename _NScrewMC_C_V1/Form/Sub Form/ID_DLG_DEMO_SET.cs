using Modbus.Device;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace _NScrewMC_C_V1
{
    public partial class DemoSet : Form
    {
        public DemoSet()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void BT_DEMO_SELECT_Click(object sender, EventArgs e)
        {
            MSystem.m_bVisionTestDemo = true;
            this.DialogResult = DialogResult.OK;   // true 의미
            this.Close();
        }

        private void BT_SET_SELECT_Click(object sender, EventArgs e)
        {
            MSystem.m_bVisionTestDemo = false;
            this.DialogResult = DialogResult.OK;   // true 의미
            this.Close();
        }

        private void BT_CLOSE_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;   // true 의미
            this.Close();
        }
    }
}
