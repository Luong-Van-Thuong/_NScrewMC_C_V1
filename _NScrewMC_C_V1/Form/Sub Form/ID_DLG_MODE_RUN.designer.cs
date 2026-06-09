namespace _NScrewMC_C_V1
{
    partial class FormMode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMode));
            this.BtExit = new SUserControls.ColorButton();
            this.ChangeMode = new SUserControls.ColorButton();
            this.btauto = new SUserControls.ColorButton();
            this.btdry = new SUserControls.ColorButton();
            this.SuspendLayout();
            // 
            // BtExit
            // 
            this.BtExit.BackColor = System.Drawing.Color.Transparent;
            this.BtExit.BorderLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtExit.Checked = false;
            this.BtExit.CheckedButtonColor = System.Drawing.Color.Blue;
            this.BtExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtExit.ForeColor = System.Drawing.Color.White;
            this.BtExit.GradientBottom = System.Drawing.Color.Black;
            this.BtExit.GradientTop = System.Drawing.Color.DimGray;
            this.BtExit.Image = ((System.Drawing.Image)(resources.GetObject("BtExit.Image")));
            this.BtExit.Location = new System.Drawing.Point(222, 123);
            this.BtExit.Name = "BtExit";
            this.BtExit.RectCornerRadius = 2;
            this.BtExit.Size = new System.Drawing.Size(139, 50);
            this.BtExit.TabIndex = 221;
            this.BtExit.Text = "Exit";
            this.BtExit.UseVisualStyleBackColor = false;
            this.BtExit.Click += new System.EventHandler(this.BtExit_Click);
            // 
            // ChangeMode
            // 
            this.ChangeMode.BackColor = System.Drawing.Color.Transparent;
            this.ChangeMode.BorderLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ChangeMode.Checked = false;
            this.ChangeMode.CheckedButtonColor = System.Drawing.Color.Blue;
            this.ChangeMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChangeMode.ForeColor = System.Drawing.Color.Navy;
            this.ChangeMode.GradientBottom = System.Drawing.Color.Gainsboro;
            this.ChangeMode.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.ChangeMode.Image = global::_NScrewMC_C_V1.Properties.Resources.원점복귀실행;
            this.ChangeMode.Location = new System.Drawing.Point(50, 123);
            this.ChangeMode.Name = "ChangeMode";
            this.ChangeMode.RectCornerRadius = 2;
            this.ChangeMode.Size = new System.Drawing.Size(139, 50);
            this.ChangeMode.TabIndex = 220;
            this.ChangeMode.Text = "Change";
            this.ChangeMode.UseVisualStyleBackColor = false;
            this.ChangeMode.Click += new System.EventHandler(this.ChangeMode_Click);
            // 
            // btauto
            // 
            this.btauto.BackColor = System.Drawing.Color.Transparent;
            this.btauto.BorderLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btauto.Checked = false;
            this.btauto.CheckedButtonColor = System.Drawing.Color.Black;
            this.btauto.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btauto.ForeColor = System.Drawing.Color.Black;
            this.btauto.GradientBottom = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.btauto.GradientTop = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.btauto.Location = new System.Drawing.Point(50, 25);
            this.btauto.Name = "btauto";
            this.btauto.RectCornerRadius = 5;
            this.btauto.Size = new System.Drawing.Size(139, 79);
            this.btauto.TabIndex = 507;
            this.btauto.Text = "AUTO";
            this.btauto.UseVisualStyleBackColor = false;
            // 
            // btdry
            // 
            this.btdry.BackColor = System.Drawing.Color.Transparent;
            this.btdry.BorderLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btdry.Checked = false;
            this.btdry.CheckedButtonColor = System.Drawing.Color.Black;
            this.btdry.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btdry.ForeColor = System.Drawing.Color.Black;
            this.btdry.GradientBottom = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.btdry.GradientTop = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.btdry.Location = new System.Drawing.Point(222, 25);
            this.btdry.Name = "btdry";
            this.btdry.RectCornerRadius = 5;
            this.btdry.Size = new System.Drawing.Size(139, 79);
            this.btdry.TabIndex = 508;
            this.btdry.Text = "DRY";
            this.btdry.UseVisualStyleBackColor = false;
            // 
            // FormMode
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(430, 185);
            this.ControlBox = false;
            this.Controls.Add(this.btdry);
            this.Controls.Add(this.btauto);
            this.Controls.Add(this.BtExit);
            this.Controls.Add(this.ChangeMode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(446, 224);
            this.MinimumSize = new System.Drawing.Size(446, 224);
            this.Name = "FormMode";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Mode Run";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormOrigin_FormClosed);
            this.Load += new System.EventHandler(this.FormMode_Load);
            this.ResumeLayout(false);

        }

        #endregion
        public SUserControls.ColorButton ChangeMode;
        public SUserControls.ColorButton BtExit;
        public SUserControls.ColorButton btauto;
        public SUserControls.ColorButton btdry;
    }
}