namespace _NScrewMC_C_V1
{
    partial class FormTitle
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
            this.BtGetStepJob = new SUserControls.ColorButton();
            this.Txt_DateTime = new System.Windows.Forms.Label();
            this.LB_MODEL = new System.Windows.Forms.Label();
            this.Text_Version = new System.Windows.Forms.Label();
            this.LABEL_DOOR_OPEN_TIMER = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtGetStepJob
            // 
            this.BtGetStepJob.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtGetStepJob.BorderLineColor = System.Drawing.Color.Firebrick;
            this.BtGetStepJob.Checked = true;
            this.BtGetStepJob.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.BtGetStepJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtGetStepJob.Font = new System.Drawing.Font("HY견고딕", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtGetStepJob.ForeColor = System.Drawing.Color.White;
            this.BtGetStepJob.GradientBottom = System.Drawing.Color.DodgerBlue;
            this.BtGetStepJob.GradientTop = System.Drawing.Color.DodgerBlue;
            this.BtGetStepJob.Location = new System.Drawing.Point(0, 0);
            this.BtGetStepJob.MaximumSize = new System.Drawing.Size(1024, 85);
            this.BtGetStepJob.MinimumSize = new System.Drawing.Size(1024, 85);
            this.BtGetStepJob.Name = "BtGetStepJob";
            this.BtGetStepJob.Size = new System.Drawing.Size(1024, 85);
            this.BtGetStepJob.TabIndex = 185;
            this.BtGetStepJob.UseVisualStyleBackColor = false;
            this.BtGetStepJob.Click += new System.EventHandler(this.IDC_THREAD_STEP_Click);
            // 
            // Txt_DateTime
            // 
            this.Txt_DateTime.BackColor = System.Drawing.Color.DodgerBlue;
            this.Txt_DateTime.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.Txt_DateTime.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Txt_DateTime.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Txt_DateTime.Location = new System.Drawing.Point(794, 54);
            this.Txt_DateTime.Name = "Txt_DateTime";
            this.Txt_DateTime.Size = new System.Drawing.Size(226, 23);
            this.Txt_DateTime.TabIndex = 190;
            this.Txt_DateTime.Text = "Date/Time";
            this.Txt_DateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LB_MODEL
            // 
            this.LB_MODEL.BackColor = System.Drawing.Color.DodgerBlue;
            this.LB_MODEL.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.LB_MODEL.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LB_MODEL.Location = new System.Drawing.Point(794, 8);
            this.LB_MODEL.Name = "LB_MODEL";
            this.LB_MODEL.Size = new System.Drawing.Size(226, 23);
            this.LB_MODEL.TabIndex = 188;
            this.LB_MODEL.Text = "Model Name";
            this.LB_MODEL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Text_Version
            // 
            this.Text_Version.BackColor = System.Drawing.Color.DodgerBlue;
            this.Text_Version.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.Text_Version.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Text_Version.Location = new System.Drawing.Point(794, 31);
            this.Text_Version.Name = "Text_Version";
            this.Text_Version.Size = new System.Drawing.Size(226, 23);
            this.Text_Version.TabIndex = 191;
            this.Text_Version.Text = "Version";
            this.Text_Version.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LABEL_DOOR_OPEN_TIMER
            // 
            this.LABEL_DOOR_OPEN_TIMER.BackColor = System.Drawing.Color.DodgerBlue;
            this.LABEL_DOOR_OPEN_TIMER.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LABEL_DOOR_OPEN_TIMER.ForeColor = System.Drawing.Color.Red;
            this.LABEL_DOOR_OPEN_TIMER.Location = new System.Drawing.Point(30, 47);
            this.LABEL_DOOR_OPEN_TIMER.Name = "LABEL_DOOR_OPEN_TIMER";
            this.LABEL_DOOR_OPEN_TIMER.Size = new System.Drawing.Size(100, 23);
            this.LABEL_DOOR_OPEN_TIMER.TabIndex = 192;
            this.LABEL_DOOR_OPEN_TIMER.Text = "00:00";
            this.LABEL_DOOR_OPEN_TIMER.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormTitle
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1024, 85);
            this.Controls.Add(this.LABEL_DOOR_OPEN_TIMER);
            this.Controls.Add(this.Text_Version);
            this.Controls.Add(this.Txt_DateTime);
            this.Controls.Add(this.LB_MODEL);
            this.Controls.Add(this.BtGetStepJob);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTitle";
            this.Text = "FormTitle";
            this.ResumeLayout(false);

        }

        #endregion

        public SUserControls.ColorButton BtGetStepJob;
        private System.Windows.Forms.Label Txt_DateTime;
        private System.Windows.Forms.Label LB_MODEL;
        private System.Windows.Forms.Label Text_Version;
        private System.Windows.Forms.Label LABEL_DOOR_OPEN_TIMER;
    }
}