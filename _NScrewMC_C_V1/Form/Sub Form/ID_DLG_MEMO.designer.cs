
namespace _NScrewMC_C_V1
{
    partial class FMsgMemo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMsgMemo));
            this.BT_IMAGE = new SUserControls.ColorButton();
            this.LB_MSG = new System.Windows.Forms.Label();
            this.BT_YES = new SUserControls.ColorButton();
            this.BT_NO = new SUserControls.ColorButton();
            this.BT_OK = new SUserControls.ColorButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // BT_IMAGE
            // 
            this.BT_IMAGE.BackColor = System.Drawing.Color.Transparent;
            this.BT_IMAGE.BorderLineColor = System.Drawing.Color.Transparent;
            this.BT_IMAGE.Checked = false;
            this.BT_IMAGE.CheckedButtonColor = System.Drawing.Color.Blue;
            this.BT_IMAGE.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_IMAGE.ForeColor = System.Drawing.Color.Black;
            this.BT_IMAGE.GradientBottom = System.Drawing.Color.Transparent;
            this.BT_IMAGE.GradientTop = System.Drawing.Color.Transparent;
            this.BT_IMAGE.Location = new System.Drawing.Point(26, 60);
            this.BT_IMAGE.Name = "BT_IMAGE";
            this.BT_IMAGE.RectCornerRadius = 5;
            this.BT_IMAGE.Size = new System.Drawing.Size(62, 58);
            this.BT_IMAGE.TabIndex = 231;
            this.BT_IMAGE.UseVisualStyleBackColor = false;
            // 
            // LB_MSG
            // 
            this.LB_MSG.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_MSG.Location = new System.Drawing.Point(97, 14);
            this.LB_MSG.Name = "LB_MSG";
            this.LB_MSG.Size = new System.Drawing.Size(581, 148);
            this.LB_MSG.TabIndex = 232;
            this.LB_MSG.Text = "This My Messenger Box";
            this.LB_MSG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BT_YES
            // 
            this.BT_YES.BackColor = System.Drawing.Color.Transparent;
            this.BT_YES.BorderLineColor = System.Drawing.Color.Red;
            this.BT_YES.Checked = false;
            this.BT_YES.CheckedButtonColor = System.Drawing.Color.Blue;
            this.BT_YES.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_YES.ForeColor = System.Drawing.Color.Black;
            this.BT_YES.GradientBottom = System.Drawing.Color.LightGray;
            this.BT_YES.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.BT_YES.Location = new System.Drawing.Point(53, 178);
            this.BT_YES.Name = "BT_YES";
            this.BT_YES.RectCornerRadius = 2;
            this.BT_YES.Size = new System.Drawing.Size(240, 49);
            this.BT_YES.TabIndex = 233;
            this.BT_YES.Text = "Yes";
            this.BT_YES.UseVisualStyleBackColor = false;
            this.BT_YES.Visible = false;
            // 
            // BT_NO
            // 
            this.BT_NO.BackColor = System.Drawing.Color.Transparent;
            this.BT_NO.BorderLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.BT_NO.Checked = false;
            this.BT_NO.CheckedButtonColor = System.Drawing.Color.Blue;
            this.BT_NO.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_NO.ForeColor = System.Drawing.Color.Black;
            this.BT_NO.GradientBottom = System.Drawing.Color.LightGray;
            this.BT_NO.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.BT_NO.Location = new System.Drawing.Point(408, 178);
            this.BT_NO.Name = "BT_NO";
            this.BT_NO.RectCornerRadius = 2;
            this.BT_NO.Size = new System.Drawing.Size(240, 49);
            this.BT_NO.TabIndex = 234;
            this.BT_NO.Text = "No";
            this.BT_NO.UseVisualStyleBackColor = false;
            this.BT_NO.Visible = false;
            // 
            // BT_OK
            // 
            this.BT_OK.BackColor = System.Drawing.Color.Transparent;
            this.BT_OK.BorderLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.BT_OK.Checked = false;
            this.BT_OK.CheckedButtonColor = System.Drawing.Color.Blue;
            this.BT_OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_OK.ForeColor = System.Drawing.Color.Black;
            this.BT_OK.GradientBottom = System.Drawing.Color.LightGray;
            this.BT_OK.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.BT_OK.Location = new System.Drawing.Point(408, 178);
            this.BT_OK.Name = "BT_OK";
            this.BT_OK.RectCornerRadius = 2;
            this.BT_OK.Size = new System.Drawing.Size(240, 49);
            this.BT_OK.TabIndex = 235;
            this.BT_OK.Text = "Ok";
            this.BT_OK.UseVisualStyleBackColor = false;
            this.BT_OK.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FMsgMemo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(704, 244);
            this.ControlBox = false;
            this.Controls.Add(this.BT_OK);
            this.Controls.Add(this.BT_NO);
            this.Controls.Add(this.BT_YES);
            this.Controls.Add(this.LB_MSG);
            this.Controls.Add(this.BT_IMAGE);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FMsgMemo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Title";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FMsgMemo_FormClosed);
            this.Load += new System.EventHandler(this.MyMessenger_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public SUserControls.ColorButton BT_IMAGE;
        private System.Windows.Forms.Label LB_MSG;
        public SUserControls.ColorButton BT_YES;
        public SUserControls.ColorButton BT_NO;
        public SUserControls.ColorButton BT_OK;
        private System.Windows.Forms.Timer timer1;
    }
}