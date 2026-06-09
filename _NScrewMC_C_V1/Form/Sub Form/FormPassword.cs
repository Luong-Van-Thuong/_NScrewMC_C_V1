using System;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormPassword : System.Windows.Forms.Form
    {
        private string _password = string.Empty;
        public bool IsOK { get; set; } = false;
        private bool _isShift = true;

        public FormPassword(string password)
        {
            _password = password;
            InitializeComponent();
            //BtOK.DialogResult = DialogResult.OK;
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

            //Character
            BTQ.Click += new EventHandler(PreEvent);
            BTW.Click += new EventHandler(PreEvent);
            BTE.Click += new EventHandler(PreEvent);
            BTR.Click += new EventHandler(PreEvent);
            BTT.Click += new EventHandler(PreEvent);
            BTY.Click += new EventHandler(PreEvent);
            BTU.Click += new EventHandler(PreEvent);
            BTI.Click += new EventHandler(PreEvent);
            BTO.Click += new EventHandler(PreEvent);
            BTP.Click += new EventHandler(PreEvent);
            BTA.Click += new EventHandler(PreEvent);
            BTS.Click += new EventHandler(PreEvent);
            BTD.Click += new EventHandler(PreEvent);
            BTF.Click += new EventHandler(PreEvent);
            BTG.Click += new EventHandler(PreEvent);
            BTH.Click += new EventHandler(PreEvent);
            BTJ.Click += new EventHandler(PreEvent);
            BTK.Click += new EventHandler(PreEvent);
            BTL.Click += new EventHandler(PreEvent);
            BTZ.Click += new EventHandler(PreEvent);
            BTX.Click += new EventHandler(PreEvent);
            BTC.Click += new EventHandler(PreEvent);
            BTV.Click += new EventHandler(PreEvent);
            BTB.Click += new EventHandler(PreEvent);
            BTN.Click += new EventHandler(PreEvent);
            BTM.Click += new EventHandler(PreEvent);
            BT_.Click += new EventHandler(PreEvent);
        }
        private void Initial(object sender, EventArgs e)
        {
            TxValue.Text = "";
        }
        private void BtExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PreEvent(object sender, EventArgs e)
        {
            CheckDataInput((sender as Button).Text);
        }

        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    // MessageBox.Show(keyData.ToString());
        //    //int value = (int)keyData;
        //    //txtvaluetest.Text = value.ToString();
        //    switch (keyData)
        //    {
        //        case (Keys)0x100BD:
        //            CheckDataInput("_");
        //            BT_.Focus();
        //            break;
        //        case Keys.A:
        //            CheckDataInput("A");
        //            BTA.Focus();
        //            break;
        //        case Keys.B:
        //            CheckDataInput("B");
        //            BTB.Focus();
        //            break;
        //        case Keys.C:
        //            CheckDataInput("C");
        //            BTC.Focus();
        //            break;
        //        case Keys.D:
        //            CheckDataInput("D");
        //            BTD.Focus();
        //            break;
        //        case Keys.E:
        //            CheckDataInput("E");
        //            BTE.Focus();
        //            break;
        //        case Keys.F:
        //            CheckDataInput("F");
        //            BTF.Focus();
        //            break;
        //        case Keys.G:
        //            CheckDataInput("G");
        //            BTG.Focus();
        //            break;
        //        case Keys.H:
        //            CheckDataInput("H");
        //            BTH.Focus();
        //            break;
        //        case Keys.I:
        //            CheckDataInput("I");
        //            BTI.Focus();
        //            break;
        //        case Keys.J:
        //            CheckDataInput("J");
        //            BTJ.Focus();
        //            break;
        //        case Keys.K:
        //            CheckDataInput("K");
        //            BTK.Focus();
        //            break;
        //        case Keys.L:
        //            CheckDataInput("L");
        //            BTL.Focus();
        //            break;
        //        case Keys.M:
        //            CheckDataInput("M");
        //            BTM.Focus();
        //            break;
        //        case Keys.N:
        //            CheckDataInput("N");
        //            BTN.Focus();
        //            break;
        //        case Keys.O:
        //            CheckDataInput("O");
        //            BTO.Focus();
        //            break;
        //        case Keys.P:
        //            CheckDataInput("P");
        //            BTP.Focus();
        //            break;
        //        case Keys.Q:
        //            CheckDataInput("Q");
        //            BTQ.Focus();
        //            break;
        //        case Keys.R:
        //            CheckDataInput("R");
        //            BTR.Focus();
        //            break;
        //        case Keys.S:
        //            CheckDataInput("S");
        //            BTS.Focus();
        //            break;
        //        case Keys.T:
        //            CheckDataInput("T");
        //            BTT.Focus();
        //            break;
        //        case Keys.U:
        //            CheckDataInput("U");
        //            BTU.Focus();
        //            break;
        //        case Keys.V:
        //            CheckDataInput("V");
        //            BTV.Focus();
        //            break;
        //        case Keys.W:
        //            CheckDataInput("W");
        //            BTW.Focus();
        //            break;
        //        case Keys.X:
        //            CheckDataInput("X");
        //            BTX.Focus();
        //            break;
        //        case Keys.Y:
        //            CheckDataInput("Y");
        //            BTY.Focus();
        //            break;
        //        case Keys.Z:
        //            CheckDataInput("Z");
        //            BTZ.Focus();
        //            break;

        //        case Keys.Escape:
        //            CheckDataInput("Exit");
        //            break;
        //        case Keys.Return:
        //            BtOK.Focus();
        //            break;
        //        case Keys.Back:
        //            CheckDataInput("Back");
        //            break;
        //        case Keys.D0:
        //            CheckDataInput("0");
        //            BT0.Focus();
        //            break;
        //        case Keys.D1:
        //            CheckDataInput("1");
        //            BT1.Focus();
        //            break;
        //        case Keys.D2:
        //            CheckDataInput("2");
        //            BT2.Focus();
        //            break;
        //        case Keys.D3:
        //            CheckDataInput("3");
        //            BT3.Focus();
        //            break;
        //        case Keys.D4:
        //            CheckDataInput("4");
        //            BT4.Focus();
        //            break;
        //        case Keys.D5:
        //            CheckDataInput("5");
        //            BT5.Focus();
        //            break;
        //        case Keys.D6:
        //            CheckDataInput("6");
        //            BT6.Focus();
        //            break;
        //        case Keys.D7:
        //            CheckDataInput("7");
        //            BT7.Focus();
        //            break;
        //        case Keys.D8:
        //            CheckDataInput("8");
        //            BT8.Focus();
        //            break;
        //        case Keys.D9:
        //            CheckDataInput("9");
        //            BT9.Focus();
        //            break;
        //        case Keys.NumPad0:
        //            CheckDataInput("0");
        //            BT0.Focus();
        //            break;
        //        case Keys.NumPad1:
        //            CheckDataInput("1");
        //            BT1.Focus();
        //            break;
        //        case Keys.NumPad2:
        //            CheckDataInput("2");
        //            BT2.Focus();
        //            break;
        //        case Keys.NumPad3:
        //            CheckDataInput("3");
        //            BT3.Focus();
        //            break;
        //        case Keys.NumPad4:
        //            CheckDataInput("4");
        //            BT4.Focus();
        //            break;
        //        case Keys.NumPad5:
        //            CheckDataInput("5");
        //            BT5.Focus();
        //            break;
        //        case Keys.NumPad6:
        //            CheckDataInput("6");
        //            BT6.Focus();
        //            break;
        //        case Keys.NumPad7:
        //            CheckDataInput("7");
        //            BT7.Focus();
        //            break;
        //        case Keys.NumPad8:
        //            CheckDataInput("8");
        //            BT8.Focus();
        //            break;
        //        case Keys.NumPad9:
        //            CheckDataInput("9");
        //            BT9.Focus();
        //            break;
        //        default: break;
        //    }
            
        //    return base.ProcessCmdKey(ref msg, keyData);
        //}
        #region //Tool Fuciton
        private void CheckDataInput(string keydata)
        {
            if (keydata == "Cancel")
            {
                this.Close();
                return;
            }
            else if (keydata == "O K")
            {
                if (TxValue.Text == _password)
                {
                    IsOK = true;
                    this.Close();
                }
                else { MSystem.MyMsgMemo("Password incorrect!", "Error", msgButton.OK, msgIcon.Error); }
                TxValue.Text = string.Empty;
                return;
            }
            else if (keydata == "CLEAR")
            {
                TxValue.Text = "";
                return;
            }
            else if (keydata == "Backspace")
            {
                try
                {
                    if (TxValue.Text.Length > 1)
                    {
                        TxValue.Text = TxValue.Text.Substring(0, TxValue.Text.Length - 1);
                    }
                    else
                    {
                        TxValue.Text = string.Empty;
                    }
                    BtBack.Focus();
                    return;
                }
                catch
                {

                }
            }
            else
            {
                TxValue.Text += keydata;
            }
        }
        #endregion
    }
}
