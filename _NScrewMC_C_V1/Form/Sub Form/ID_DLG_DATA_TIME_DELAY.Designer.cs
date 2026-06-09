
namespace _NScrewMC_C_V1
{
    partial class FormTimeAndDelay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTimeAndDelay));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.colorButton8 = new SUserControls.ColorButton();
            this.TB_TRANSFER_GRIP_ON = new SUserControls.ColorButton();
            this.BtSave = new SUserControls.ColorButton();
            this.BtExit = new SUserControls.ColorButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.colorButton16 = new SUserControls.ColorButton();
            this.TB_ATT_PUSH_DELAY = new SUserControls.ColorButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.colorButton1 = new SUserControls.ColorButton();
            this.TB_TIME_FEEDER_RD = new SUserControls.ColorButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.colorButton2 = new SUserControls.ColorButton();
            this.TB_TIME_BUZZER = new SUserControls.ColorButton();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.colorButton8);
            this.groupBox2.Controls.Add(this.TB_TRANSFER_GRIP_ON);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(382, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(331, 87);
            this.groupBox2.TabIndex = 263;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Unloader ";
            // 
            // colorButton8
            // 
            this.colorButton8.BackColor = System.Drawing.Color.Transparent;
            this.colorButton8.BorderLineColor = System.Drawing.Color.DimGray;
            this.colorButton8.Checked = false;
            this.colorButton8.CheckedButtonColor = System.Drawing.Color.Blue;
            this.colorButton8.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorButton8.ForeColor = System.Drawing.Color.Black;
            this.colorButton8.GradientBottom = System.Drawing.Color.Gainsboro;
            this.colorButton8.GradientTop = System.Drawing.Color.Gainsboro;
            this.colorButton8.Location = new System.Drawing.Point(6, 25);
            this.colorButton8.Name = "colorButton8";
            this.colorButton8.Size = new System.Drawing.Size(198, 54);
            this.colorButton8.TabIndex = 244;
            this.colorButton8.Text = "Grip On Time";
            this.colorButton8.UseVisualStyleBackColor = false;
            // 
            // TB_TRANSFER_GRIP_ON
            // 
            this.TB_TRANSFER_GRIP_ON.BackColor = System.Drawing.Color.Transparent;
            this.TB_TRANSFER_GRIP_ON.BorderLineColor = System.Drawing.Color.DimGray;
            this.TB_TRANSFER_GRIP_ON.Checked = false;
            this.TB_TRANSFER_GRIP_ON.CheckedButtonColor = System.Drawing.Color.Blue;
            this.TB_TRANSFER_GRIP_ON.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_TRANSFER_GRIP_ON.ForeColor = System.Drawing.Color.Black;
            this.TB_TRANSFER_GRIP_ON.GradientBottom = System.Drawing.Color.White;
            this.TB_TRANSFER_GRIP_ON.GradientTop = System.Drawing.Color.White;
            this.TB_TRANSFER_GRIP_ON.Location = new System.Drawing.Point(207, 25);
            this.TB_TRANSFER_GRIP_ON.Name = "TB_TRANSFER_GRIP_ON";
            this.TB_TRANSFER_GRIP_ON.Size = new System.Drawing.Size(118, 54);
            this.TB_TRANSFER_GRIP_ON.TabIndex = 245;
            this.TB_TRANSFER_GRIP_ON.Text = "0.0 Sec";
            this.TB_TRANSFER_GRIP_ON.UseVisualStyleBackColor = false;
            this.TB_TRANSFER_GRIP_ON.Click += new System.EventHandler(this.PreLoadEvent);
            // 
            // BtSave
            // 
            this.BtSave.BackColor = System.Drawing.Color.Transparent;
            this.BtSave.BorderLineColor = System.Drawing.Color.Green;
            this.BtSave.Checked = false;
            this.BtSave.CheckedButtonColor = System.Drawing.Color.Black;
            this.BtSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtSave.ForeColor = System.Drawing.Color.Black;
            this.BtSave.GradientBottom = System.Drawing.Color.Gainsboro;
            this.BtSave.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.BtSave.Image = global::_NScrewMC_C_V1.Properties.Resources.저장;
            this.BtSave.Location = new System.Drawing.Point(410, 257);
            this.BtSave.Name = "BtSave";
            this.BtSave.RectCornerRadius = 3;
            this.BtSave.Size = new System.Drawing.Size(140, 60);
            this.BtSave.TabIndex = 265;
            this.BtSave.Text = "Save";
            this.BtSave.UseVisualStyleBackColor = false;
            this.BtSave.Click += new System.EventHandler(this.PreLoadEvent);
            // 
            // BtExit
            // 
            this.BtExit.BackColor = System.Drawing.Color.Transparent;
            this.BtExit.BorderLineColor = System.Drawing.Color.Red;
            this.BtExit.Checked = false;
            this.BtExit.CheckedButtonColor = System.Drawing.Color.Black;
            this.BtExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtExit.ForeColor = System.Drawing.Color.Black;
            this.BtExit.GradientBottom = System.Drawing.Color.Gainsboro;
            this.BtExit.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.BtExit.Image = global::_NScrewMC_C_V1.Properties.Resources.Exit;
            this.BtExit.Location = new System.Drawing.Point(556, 257);
            this.BtExit.Name = "BtExit";
            this.BtExit.RectCornerRadius = 3;
            this.BtExit.Size = new System.Drawing.Size(140, 60);
            this.BtExit.TabIndex = 264;
            this.BtExit.Text = "EXIT";
            this.BtExit.UseVisualStyleBackColor = false;
            this.BtExit.Click += new System.EventHandler(this.PreLoadEvent);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.colorButton16);
            this.groupBox4.Controls.Add(this.TB_ATT_PUSH_DELAY);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(382, 141);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(331, 92);
            this.groupBox4.TabIndex = 265;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "GMES";
            // 
            // colorButton16
            // 
            this.colorButton16.BackColor = System.Drawing.Color.Transparent;
            this.colorButton16.BorderLineColor = System.Drawing.Color.DimGray;
            this.colorButton16.Checked = false;
            this.colorButton16.CheckedButtonColor = System.Drawing.Color.Blue;
            this.colorButton16.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorButton16.ForeColor = System.Drawing.Color.Black;
            this.colorButton16.GradientBottom = System.Drawing.Color.Gainsboro;
            this.colorButton16.GradientTop = System.Drawing.Color.Gainsboro;
            this.colorButton16.Location = new System.Drawing.Point(6, 25);
            this.colorButton16.Name = "colorButton16";
            this.colorButton16.Size = new System.Drawing.Size(198, 54);
            this.colorButton16.TabIndex = 244;
            this.colorButton16.Text = "Time Wait Result";
            this.colorButton16.UseVisualStyleBackColor = false;
            // 
            // TB_ATT_PUSH_DELAY
            // 
            this.TB_ATT_PUSH_DELAY.BackColor = System.Drawing.Color.Transparent;
            this.TB_ATT_PUSH_DELAY.BorderLineColor = System.Drawing.Color.DimGray;
            this.TB_ATT_PUSH_DELAY.Checked = false;
            this.TB_ATT_PUSH_DELAY.CheckedButtonColor = System.Drawing.Color.Blue;
            this.TB_ATT_PUSH_DELAY.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_ATT_PUSH_DELAY.ForeColor = System.Drawing.Color.Black;
            this.TB_ATT_PUSH_DELAY.GradientBottom = System.Drawing.Color.White;
            this.TB_ATT_PUSH_DELAY.GradientTop = System.Drawing.Color.White;
            this.TB_ATT_PUSH_DELAY.Location = new System.Drawing.Point(207, 25);
            this.TB_ATT_PUSH_DELAY.Name = "TB_ATT_PUSH_DELAY";
            this.TB_ATT_PUSH_DELAY.Size = new System.Drawing.Size(118, 54);
            this.TB_ATT_PUSH_DELAY.TabIndex = 245;
            this.TB_ATT_PUSH_DELAY.Text = "0.0 Sec";
            this.TB_ATT_PUSH_DELAY.UseVisualStyleBackColor = false;
            this.TB_ATT_PUSH_DELAY.Click += new System.EventHandler(this.PreLoadEvent);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.colorButton1);
            this.groupBox1.Controls.Add(this.TB_TIME_FEEDER_RD);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(331, 102);
            this.groupBox1.TabIndex = 266;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Common";
            // 
            // colorButton1
            // 
            this.colorButton1.BackColor = System.Drawing.Color.Transparent;
            this.colorButton1.BorderLineColor = System.Drawing.Color.DimGray;
            this.colorButton1.Checked = false;
            this.colorButton1.CheckedButtonColor = System.Drawing.Color.Blue;
            this.colorButton1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorButton1.ForeColor = System.Drawing.Color.Black;
            this.colorButton1.GradientBottom = System.Drawing.Color.Gainsboro;
            this.colorButton1.GradientTop = System.Drawing.Color.Gainsboro;
            this.colorButton1.Location = new System.Drawing.Point(6, 25);
            this.colorButton1.Name = "colorButton1";
            this.colorButton1.Size = new System.Drawing.Size(198, 54);
            this.colorButton1.TabIndex = 244;
            this.colorButton1.Text = "Time Wait Feeder\r\n      Ready";
            this.colorButton1.UseVisualStyleBackColor = false;
            // 
            // TB_TIME_FEEDER_RD
            // 
            this.TB_TIME_FEEDER_RD.BackColor = System.Drawing.Color.Transparent;
            this.TB_TIME_FEEDER_RD.BorderLineColor = System.Drawing.Color.DimGray;
            this.TB_TIME_FEEDER_RD.Checked = false;
            this.TB_TIME_FEEDER_RD.CheckedButtonColor = System.Drawing.Color.Blue;
            this.TB_TIME_FEEDER_RD.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_TIME_FEEDER_RD.ForeColor = System.Drawing.Color.Black;
            this.TB_TIME_FEEDER_RD.GradientBottom = System.Drawing.Color.White;
            this.TB_TIME_FEEDER_RD.GradientTop = System.Drawing.Color.White;
            this.TB_TIME_FEEDER_RD.Location = new System.Drawing.Point(207, 25);
            this.TB_TIME_FEEDER_RD.Name = "TB_TIME_FEEDER_RD";
            this.TB_TIME_FEEDER_RD.Size = new System.Drawing.Size(118, 54);
            this.TB_TIME_FEEDER_RD.TabIndex = 245;
            this.TB_TIME_FEEDER_RD.Text = "0.0 Sec";
            this.TB_TIME_FEEDER_RD.UseVisualStyleBackColor = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.colorButton2);
            this.groupBox3.Controls.Add(this.TB_TIME_BUZZER);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(18, 131);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(331, 102);
            this.groupBox3.TabIndex = 267;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Buzzer";
            // 
            // colorButton2
            // 
            this.colorButton2.BackColor = System.Drawing.Color.Transparent;
            this.colorButton2.BorderLineColor = System.Drawing.Color.DimGray;
            this.colorButton2.Checked = false;
            this.colorButton2.CheckedButtonColor = System.Drawing.Color.Blue;
            this.colorButton2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorButton2.ForeColor = System.Drawing.Color.Black;
            this.colorButton2.GradientBottom = System.Drawing.Color.Gainsboro;
            this.colorButton2.GradientTop = System.Drawing.Color.Gainsboro;
            this.colorButton2.Location = new System.Drawing.Point(6, 25);
            this.colorButton2.Name = "colorButton2";
            this.colorButton2.Size = new System.Drawing.Size(198, 54);
            this.colorButton2.TabIndex = 244;
            this.colorButton2.Text = "Time Buzzer";
            this.colorButton2.UseVisualStyleBackColor = false;
            // 
            // TB_TIME_BUZZER
            // 
            this.TB_TIME_BUZZER.BackColor = System.Drawing.Color.Transparent;
            this.TB_TIME_BUZZER.BorderLineColor = System.Drawing.Color.DimGray;
            this.TB_TIME_BUZZER.Checked = false;
            this.TB_TIME_BUZZER.CheckedButtonColor = System.Drawing.Color.Blue;
            this.TB_TIME_BUZZER.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_TIME_BUZZER.ForeColor = System.Drawing.Color.Black;
            this.TB_TIME_BUZZER.GradientBottom = System.Drawing.Color.White;
            this.TB_TIME_BUZZER.GradientTop = System.Drawing.Color.White;
            this.TB_TIME_BUZZER.Location = new System.Drawing.Point(207, 25);
            this.TB_TIME_BUZZER.Name = "TB_TIME_BUZZER";
            this.TB_TIME_BUZZER.Size = new System.Drawing.Size(118, 54);
            this.TB_TIME_BUZZER.TabIndex = 245;
            this.TB_TIME_BUZZER.Text = "0.0 Sec";
            this.TB_TIME_BUZZER.UseVisualStyleBackColor = false;
            // 
            // FormTimeAndDelay
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(730, 328);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.BtSave);
            this.Controls.Add(this.BtExit);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTimeAndDelay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Time and Delay";
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        public SUserControls.ColorButton colorButton8;
        public SUserControls.ColorButton TB_TRANSFER_GRIP_ON;
        public SUserControls.ColorButton BtSave;
        public SUserControls.ColorButton BtExit;
        private System.Windows.Forms.GroupBox groupBox4;
        public SUserControls.ColorButton colorButton16;
        public SUserControls.ColorButton TB_ATT_PUSH_DELAY;
        private System.Windows.Forms.GroupBox groupBox1;
        public SUserControls.ColorButton colorButton1;
        public SUserControls.ColorButton TB_TIME_FEEDER_RD;
        private System.Windows.Forms.GroupBox groupBox3;
        public SUserControls.ColorButton colorButton2;
        public SUserControls.ColorButton TB_TIME_BUZZER;
    }
}