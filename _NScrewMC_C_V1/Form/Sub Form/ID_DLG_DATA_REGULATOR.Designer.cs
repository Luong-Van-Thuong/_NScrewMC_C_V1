
namespace _NScrewMC_C_V1
{
    partial class ID_DLG_DATA_REGULATOR
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Btn_Scan = new System.Windows.Forms.Button();
            this.TXT_PRESSURE = new SUserControls.ColorButton();
            this.colorButton1 = new SUserControls.ColorButton();
            this.BT_EXIT = new SUserControls.ColorButton();
            this.SuspendLayout();
            // 
            // Btn_Scan
            // 
            this.Btn_Scan.BackColor = System.Drawing.Color.LightGray;
            this.Btn_Scan.Font = new System.Drawing.Font("Malgun Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Btn_Scan.Location = new System.Drawing.Point(24, 11);
            this.Btn_Scan.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.Btn_Scan.Name = "Btn_Scan";
            this.Btn_Scan.Size = new System.Drawing.Size(164, 71);
            this.Btn_Scan.TabIndex = 900;
            this.Btn_Scan.Text = "Press Value";
            this.Btn_Scan.UseVisualStyleBackColor = false;
            // 
            // TXT_PRESSURE
            // 
            this.TXT_PRESSURE.BackColor = System.Drawing.Color.Transparent;
            this.TXT_PRESSURE.BorderLineColor = System.Drawing.Color.Black;
            this.TXT_PRESSURE.Checked = false;
            this.TXT_PRESSURE.CheckedButtonColor = System.Drawing.Color.LimeGreen;
            this.TXT_PRESSURE.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXT_PRESSURE.ForeColor = System.Drawing.Color.Black;
            this.TXT_PRESSURE.GradientBottom = System.Drawing.Color.White;
            this.TXT_PRESSURE.GradientTop = System.Drawing.Color.White;
            this.TXT_PRESSURE.Location = new System.Drawing.Point(200, 11);
            this.TXT_PRESSURE.Name = "TXT_PRESSURE";
            this.TXT_PRESSURE.Size = new System.Drawing.Size(202, 71);
            this.TXT_PRESSURE.TabIndex = 903;
            this.TXT_PRESSURE.Text = "0.00";
            this.TXT_PRESSURE.UseVisualStyleBackColor = false;
            this.TXT_PRESSURE.Click += new System.EventHandler(this.TXT_PRESSURE_Click);
            // 
            // colorButton1
            // 
            this.colorButton1.BackColor = System.Drawing.Color.Transparent;
            this.colorButton1.BorderLineColor = System.Drawing.Color.Black;
            this.colorButton1.Checked = false;
            this.colorButton1.CheckedButtonColor = System.Drawing.Color.LimeGreen;
            this.colorButton1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorButton1.ForeColor = System.Drawing.Color.Black;
            this.colorButton1.GradientBottom = System.Drawing.Color.White;
            this.colorButton1.GradientTop = System.Drawing.Color.White;
            this.colorButton1.Location = new System.Drawing.Point(24, 113);
            this.colorButton1.Name = "colorButton1";
            this.colorButton1.Size = new System.Drawing.Size(298, 252);
            this.colorButton1.TabIndex = 904;
            this.colorButton1.Text = "   Value\r\n  1Bar = 0.1MPa\r\n  2Bar = 0.2MPa\r\n  3Bar = 0.3MPa\r\n  4Bar = 0.4MPa\r\n  5" +
    "Bar = 0.5MPa";
            this.colorButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.colorButton1.UseVisualStyleBackColor = false;
            // 
            // BT_EXIT
            // 
            this.BT_EXIT.BackColor = System.Drawing.Color.Transparent;
            this.BT_EXIT.BorderLineColor = System.Drawing.Color.Red;
            this.BT_EXIT.Checked = false;
            this.BT_EXIT.CheckedButtonColor = System.Drawing.Color.Blue;
            this.BT_EXIT.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_EXIT.ForeColor = System.Drawing.Color.Black;
            this.BT_EXIT.GradientBottom = System.Drawing.Color.Gainsboro;
            this.BT_EXIT.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.BT_EXIT.Image = global::_NScrewMC_C_V1.Properties.Resources.Exit;
            this.BT_EXIT.Location = new System.Drawing.Point(380, 298);
            this.BT_EXIT.Name = "BT_EXIT";
            this.BT_EXIT.RectCornerRadius = 2;
            this.BT_EXIT.Size = new System.Drawing.Size(192, 67);
            this.BT_EXIT.TabIndex = 907;
            this.BT_EXIT.Text = "Exit";
            this.BT_EXIT.UseVisualStyleBackColor = false;
            this.BT_EXIT.Click += new System.EventHandler(this.PreLoadEvent);
            // 
            // ID_DLG_DATA_REGULATOR
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(584, 384);
            this.Controls.Add(this.BT_EXIT);
            this.Controls.Add(this.colorButton1);
            this.Controls.Add(this.TXT_PRESSURE);
            this.Controls.Add(this.Btn_Scan);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 423);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 423);
            this.Name = "ID_DLG_DATA_REGULATOR";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Regulator Setting";
            this.Load += new System.EventHandler(this.ID_DLG_DATA_REGULATOR_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_Scan;
        public SUserControls.ColorButton TXT_PRESSURE;
        public SUserControls.ColorButton colorButton1;
        public SUserControls.ColorButton BT_EXIT;
    }
}