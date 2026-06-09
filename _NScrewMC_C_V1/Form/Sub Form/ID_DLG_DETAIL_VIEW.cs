using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class DetailView : Form
    {
        public int _FirstIndex = 0;
        public int _LeftRight;
        public int _pos;
        public DetailView(int LeftRight, int pos)
        {
            _LeftRight = LeftRight;
            _pos = pos;
            InitializeComponent();
            #region Test code
            //MSystem.m_pTrsScrew[LeftRight]._LastTorque = 1.0;
            //InforProduct.Instance.DetailDataAdd(LeftRight, pos); Thread.Sleep(100);
            //MSystem.m_pTrsScrew[LeftRight]._LastTorque = 2.0;
            //InforProduct.Instance.DetailDataAdd(LeftRight, pos); Thread.Sleep(100);
            //MSystem.m_pTrsScrew[LeftRight]._LastTorque = 3.0;
            //InforProduct.Instance.DetailDataAdd(LeftRight, pos); Thread.Sleep(100);
            //MSystem.m_pTrsScrew[LeftRight]._LastTorque = 4.0;
            //InforProduct.Instance.DetailDataAdd(LeftRight, pos); Thread.Sleep(100);
            //MSystem.m_pTrsScrew[LeftRight]._LastTorque = 5.0;
            //InforProduct.Instance.DetailDataAdd(LeftRight, pos); Thread.Sleep(100);
            //MSystem.m_pTrsScrew[LeftRight]._LastTorque = 6.0;
            //InforProduct.Instance.DetailDataAdd(LeftRight, pos); Thread.Sleep(100);
            //MSystem.m_pTrsScrew[LeftRight]._LastTorque = 7.0;
            //InforProduct.Instance.DetailDataAdd(LeftRight, pos); Thread.Sleep(100);
            //InforProduct.Instance.SaveSettings();
            //InforProduct.Instance.LoadSetting();
            //InforProduct.Instance.ProductDetail[LeftRight][pos].DetailStack.Clear();
            //for (int i = 0; i < 995; i++)
            //{
            //    MSystem.m_pTrsScrew[LeftRight]._LastTorque += 0.1;
            //    InforProduct.Instance.DetailDataAdd(LeftRight, pos);
            //}
            #endregion
            // GridView settings
            dataGridView1.Columns.Add("Time", "Time");
            dataGridView1.Columns.Add("Torque", "Torque");
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            DataGridView(_LeftRight, _pos);
            labelPageCount.Text = $@"{_FirstIndex / 10 + 1} / {InforProduct.Instance.ProductDetail[_LeftRight][_pos].DetailStack.Count / 10 + 1}";
        }
        private void DetailView_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            //this.WindowState = FormWindowState.Maximized;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void DetailView_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; // Prevent the form from closing
            FormproductInfor parentForm = (FormproductInfor)this.Owner;
            if (parentForm != null)
            {
                parentForm.ClearDetailViewInstance();
            }
        }
        private void DataGridView(int LeftRight, int pos)
        {
            dataGridView1.Rows.Clear();
            string title;
            if (LeftRight == MSystem.eLEFT)
            {
                title = $@"LEFT - P{pos+1}";
            }
            else
            {
                title = $@"RIGHT - P{pos + 1}";
            }
            textBoxDetailPos.Text = title;
            string[] allItems = InforProduct.Instance.ProductDetail[LeftRight][pos].DetailStack.ToArray();
            for (int i = _FirstIndex; i < _FirstIndex + 10; i++)
            {
                if (i >= allItems.Length) break;
                string[] item = allItems[i].Split(',');
                dataGridView1.Rows.Add(item[0], item[1]);
            }
        }

        private void buttonPageUp_Click(object sender, EventArgs e)
        {
            _FirstIndex -= 10;
            if (_FirstIndex < 0)
            {
                _FirstIndex = 0;
                return;
            }
            DataGridView(_LeftRight, _pos);
            labelPageCount.Text = $@"{_FirstIndex / 10 + 1} / {InforProduct.Instance.ProductDetail[_LeftRight][_pos].DetailStack.Count / 10 + 1}";
        }

        private void buttonPageDown_Click(object sender, EventArgs e)
        {
            _FirstIndex += 10;
            if (_FirstIndex >= InforProduct.Instance.ProductDetail[_LeftRight][_pos].DetailStack.Count)
            {
                _FirstIndex -= 10;
                return;
            }
            DataGridView(_LeftRight, _pos);
            labelPageCount.Text = $@"{_FirstIndex / 10 + 1} / {InforProduct.Instance.ProductDetail[_LeftRight][_pos].DetailStack.Count / 10 + 1}";
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            labelPageCount.Text = "0 / 0";
            InforProduct.Instance.ProductScrew_Fail[_LeftRight] -= InforProduct.Instance.ProductDetail[_LeftRight][_pos].m_dCountFail;
            InforProduct.Instance.ProductScrew_Total[_LeftRight] -= InforProduct.Instance.ProductDetail[_LeftRight][_pos].m_dCountFail;
            InforProduct.Instance.ProductDetail[_LeftRight][_pos].DetailStack.Clear();
            InforProduct.Instance.ProductDetail[_LeftRight][_pos].m_dCountFail = 0;
            InforProduct.Instance.SaveSettings();
            InforProduct.Instance.LoadSetting();
        }
    }
}
