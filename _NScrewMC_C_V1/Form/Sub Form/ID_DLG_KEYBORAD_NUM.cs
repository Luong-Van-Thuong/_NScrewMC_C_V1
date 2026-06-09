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
    public partial class KeyboardNum : Form
    {
        #region//Form Init
        public KeyboardNum()
        {
            InitializeComponent();
            BtOK.DialogResult = DialogResult.OK;

            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + ((FormMain.m_Width - this.Width) / 2)
                                   , FormMain.GetLocation().Y + ((FormMain.m_Height - this.Height) / 2));
            this.Load += new EventHandler(Initial);

            BT0.Click += new EventHandler(PreEvent);
            BT1.Click += new EventHandler(PreEvent);
            BT2.Click += new EventHandler(PreEvent);
            BT3.Click += new EventHandler(PreEvent);
            BT4.Click += new EventHandler(PreEvent);
            BT5.Click += new EventHandler(PreEvent);
            BT6.Click += new EventHandler(PreEvent);
            BT7.Click += new EventHandler(PreEvent);
            BT8.Click += new EventHandler(PreEvent);
            BT9.Click += new EventHandler(PreEvent);
            //
            BtBack.Click += new EventHandler(PreEvent);
            BtClear.Click += new EventHandler(PreEvent);
            //
            BtOK.Click += new EventHandler(PreEvent);
            BtExit.Click += new EventHandler(PreEvent);
            //
            BTdot.Click += new EventHandler(PreEvent);
            BtSub.Click += new EventHandler(PreEvent);
        }
        private void Initial(object sender, EventArgs e) {
            TxValue.Text = ".";
        }
        #endregion

        #region //Event Form
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
           // MessageBox.Show(keyData.ToString());
            switch (keyData) {
                case Keys.Escape:
                    CheckDataInput("Exit");
                    break;
                case Keys.Return:
                    BtOK.Focus();
                    break;
                case Keys.Back:
                    CheckDataInput("Back");
                    break;
                case Keys.D0:
                    CheckDataInput("0");
                    BT0.Focus();
                    break;
                case Keys.D1:
                    CheckDataInput("1");
                    BT1.Focus();
                    break;
                case Keys.D2:
                    CheckDataInput("2");
                    BT2.Focus();
                    break;
                case Keys.D3:
                    CheckDataInput("3");
                    BT3.Focus();
                    break;
                case Keys.D4:
                    CheckDataInput("4");
                    BT4.Focus();
                    break;
                case Keys.D5:
                    CheckDataInput("5");
                    BT5.Focus();
                    break;
                case Keys.D6:
                    CheckDataInput("6");
                    BT6.Focus();
                    break;
                case Keys.D7:
                    CheckDataInput("7");
                    BT7.Focus();
                    break;
                case Keys.D8:
                    CheckDataInput("8");
                    BT8.Focus();
                    break;
                case Keys.D9:
                    CheckDataInput("9");
                    BT9.Focus();
                    break;
                case Keys.NumPad0:
                    CheckDataInput("0");
                    BT0.Focus();
                    break;
                case Keys.NumPad1:
                    CheckDataInput("1");
                    BT1.Focus();
                    break;
                case Keys.NumPad2:
                    CheckDataInput("2");
                    BT2.Focus();
                    break;
                case Keys.NumPad3:
                    CheckDataInput("3");
                    BT3.Focus();
                    break;
                case Keys.NumPad4:
                    CheckDataInput("4");
                    BT4.Focus();
                    break;
                case Keys.NumPad5:
                    CheckDataInput("5");
                    BT5.Focus();
                    break;
                case Keys.NumPad6:
                    CheckDataInput("6");
                    BT6.Focus();
                    break;
                case Keys.NumPad7:
                    CheckDataInput("7");
                    BT7.Focus();
                    break;
                case Keys.NumPad8:
                    CheckDataInput("8");
                    BT8.Focus();
                    break;
                case Keys.NumPad9:
                    CheckDataInput("9");
                    BT9.Focus();
                    break;
                case Keys.OemPeriod:
                    CheckDataInput(".");
                    BTdot.Focus();
                    break;
                case Keys.Decimal:
                    BTdot.Focus();
                    CheckDataInput(".");
                    break;
                default:break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void PreEvent(object sender, EventArgs e) {
            CheckDataInput((sender as Button).Text);
        }
        #endregion

        #region //Tool Fuciton
        private void CheckDataInput(string keydata) {
            if (keydata == "Exit" || keydata == "OK") {
                this.Close();
                return;
            }
            else if (keydata == "Clear"){
                TxValue.Text = ".";
                return;
            }
            else if (keydata == "Back") {
                try {
                    if (TxValue.Text.Length > 1) {
                        TxValue.Text = TxValue.Text.Substring(0, TxValue.Text.Length - 1);
                    }
                    else {
                        TxValue.Text = ".";
                    }
                    BtBack.Focus();
                    return;
                }
                catch{

                }
            }
            else{
                if (TxValue.Text == ".") TxValue.Text = "";
                if (TxValue.Text == "0" || TxValue.Text == "-0") {
                    if (keydata == "0") return;
                }
                try{
                    float flTemp = 0;
                    if (keydata == "+/-"){
                        if (TxValue.Text == "") TxValue.Text = "-0";
                        else if (TxValue.Text == "-") TxValue.Text = "";
                        else if (float.TryParse(TxValue.Text, out flTemp)){
                            if (flTemp != 0){
                                flTemp = 0 - flTemp;
                                TxValue.Text = flTemp.ToString();
                            }
                            else{
                                foreach (char c in TxValue.Text){
                                    if (c != '-') TxValue.Text = "-" + TxValue.Text;
                                    else TxValue.Text = TxValue.Text.Substring(1);
                                    break;
                                }
                            }
                        }
                    }
                    string temp = TxValue.Text + keydata;
                    float.Parse(temp);
                    TxValue.Text += keydata;
                }
                catch{

                }

            }
        }
        #endregion
    }
}
