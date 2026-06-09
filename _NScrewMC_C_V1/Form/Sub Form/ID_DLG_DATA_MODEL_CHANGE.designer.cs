namespace _NScrewMC_C_V1
{
    partial class FormModel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormModel));
            this.colorButton3 = new SUserControls.ColorButton();
            this.LbModelList = new System.Windows.Forms.ListBox();
            this.colorButton1 = new SUserControls.ColorButton();
            this.colorButton2 = new SUserControls.ColorButton();
            this.BtCreate = new SUserControls.ColorButton();
            this.BtDelete = new SUserControls.ColorButton();
            this.BtChange = new SUserControls.ColorButton();
            this.BtExit = new SUserControls.ColorButton();
            this.colorButton4 = new SUserControls.ColorButton();
            this.TxModel = new SUserControls.ColorButton();
            this.TB_NEW_MODEL = new SUserControls.ColorButton();
            this.colorButton6 = new SUserControls.ColorButton();
            this.colorButton5 = new SUserControls.ColorButton();
            this.SuspendLayout();
            // 
            // colorButton3
            // 
            this.colorButton3.BackColor = System.Drawing.Color.Transparent;
            this.colorButton3.BorderLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.colorButton3.Checked = false;
            this.colorButton3.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.colorButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.colorButton3.ForeColor = System.Drawing.Color.Black;
            this.colorButton3.GradientBottom = System.Drawing.Color.White;
            this.colorButton3.GradientTop = System.Drawing.Color.Transparent;
            this.colorButton3.Location = new System.Drawing.Point(7, 5);
            this.colorButton3.Name = "colorButton3";
            this.colorButton3.Size = new System.Drawing.Size(252, 414);
            this.colorButton3.TabIndex = 186;
            this.colorButton3.UseVisualStyleBackColor = false;
            // 
            // LbModelList
            // 
            this.LbModelList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.LbModelList.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbModelList.FormattingEnabled = true;
            this.LbModelList.ItemHeight = 23;
            this.LbModelList.Location = new System.Drawing.Point(9, 43);
            this.LbModelList.Name = "LbModelList";
            this.LbModelList.Size = new System.Drawing.Size(248, 372);
            this.LbModelList.TabIndex = 213;
            this.LbModelList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxModelList_DrawItem);
            this.LbModelList.SelectedIndexChanged += new System.EventHandler(this.LbModelList_SelectedIndexChanged);
            // 
            // colorButton1
            // 
            this.colorButton1.BackColor = System.Drawing.Color.Transparent;
            this.colorButton1.BorderLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.colorButton1.Checked = false;
            this.colorButton1.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.colorButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.colorButton1.ForeColor = System.Drawing.Color.Black;
            this.colorButton1.GradientBottom = System.Drawing.Color.WhiteSmoke;
            this.colorButton1.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.colorButton1.Location = new System.Drawing.Point(265, 5);
            this.colorButton1.Name = "colorButton1";
            this.colorButton1.Size = new System.Drawing.Size(367, 206);
            this.colorButton1.TabIndex = 214;
            this.colorButton1.UseVisualStyleBackColor = false;
            // 
            // colorButton2
            // 
            this.colorButton2.BackColor = System.Drawing.Color.Transparent;
            this.colorButton2.BorderLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.colorButton2.Checked = false;
            this.colorButton2.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.colorButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorButton2.ForeColor = System.Drawing.Color.Black;
            this.colorButton2.GradientBottom = System.Drawing.Color.Gainsboro;
            this.colorButton2.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.colorButton2.Location = new System.Drawing.Point(269, 10);
            this.colorButton2.Name = "colorButton2";
            this.colorButton2.Size = new System.Drawing.Size(102, 41);
            this.colorButton2.TabIndex = 216;
            this.colorButton2.Text = "CURRENT \r\nMODEL";
            this.colorButton2.UseVisualStyleBackColor = false;
            // 
            // BtCreate
            // 
            this.BtCreate.BackColor = System.Drawing.Color.Transparent;
            this.BtCreate.BorderLineColor = System.Drawing.Color.Blue;
            this.BtCreate.Checked = false;
            this.BtCreate.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.BtCreate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtCreate.ForeColor = System.Drawing.Color.Black;
            this.BtCreate.GradientBottom = System.Drawing.Color.Gainsboro;
            this.BtCreate.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.BtCreate.Location = new System.Drawing.Point(269, 148);
            this.BtCreate.Name = "BtCreate";
            this.BtCreate.Size = new System.Drawing.Size(102, 59);
            this.BtCreate.TabIndex = 229;
            this.BtCreate.Text = "CREAT";
            this.BtCreate.UseVisualStyleBackColor = false;
            this.BtCreate.Click += new System.EventHandler(this.BtCreate_Click);
            // 
            // BtDelete
            // 
            this.BtDelete.BackColor = System.Drawing.Color.Transparent;
            this.BtDelete.BorderLineColor = System.Drawing.Color.Red;
            this.BtDelete.Checked = false;
            this.BtDelete.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.BtDelete.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtDelete.ForeColor = System.Drawing.Color.Black;
            this.BtDelete.GradientBottom = System.Drawing.Color.Gainsboro;
            this.BtDelete.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.BtDelete.Location = new System.Drawing.Point(396, 148);
            this.BtDelete.Name = "BtDelete";
            this.BtDelete.Size = new System.Drawing.Size(102, 59);
            this.BtDelete.TabIndex = 230;
            this.BtDelete.Text = "DELETE";
            this.BtDelete.UseVisualStyleBackColor = false;
            this.BtDelete.Click += new System.EventHandler(this.BtDelete_Click);
            // 
            // BtChange
            // 
            this.BtChange.BackColor = System.Drawing.Color.Transparent;
            this.BtChange.BorderLineColor = System.Drawing.Color.Blue;
            this.BtChange.Checked = false;
            this.BtChange.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.BtChange.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtChange.ForeColor = System.Drawing.Color.Black;
            this.BtChange.GradientBottom = System.Drawing.Color.Gainsboro;
            this.BtChange.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.BtChange.Location = new System.Drawing.Point(523, 148);
            this.BtChange.Name = "BtChange";
            this.BtChange.Size = new System.Drawing.Size(102, 59);
            this.BtChange.TabIndex = 231;
            this.BtChange.Text = "CHANGE";
            this.BtChange.UseVisualStyleBackColor = false;
            this.BtChange.Click += new System.EventHandler(this.BtChange_Click);
            // 
            // BtExit
            // 
            this.BtExit.BackColor = System.Drawing.Color.Transparent;
            this.BtExit.BorderLineColor = System.Drawing.Color.Red;
            this.BtExit.Checked = false;
            this.BtExit.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.BtExit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtExit.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtExit.ForeColor = System.Drawing.Color.Black;
            this.BtExit.GradientBottom = System.Drawing.Color.Gainsboro;
            this.BtExit.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.BtExit.Image = global::_NScrewMC_C_V1.Properties.Resources.Next;
            this.BtExit.Location = new System.Drawing.Point(474, 365);
            this.BtExit.Name = "BtExit";
            this.BtExit.RectCornerRadius = 2;
            this.BtExit.Size = new System.Drawing.Size(158, 54);
            this.BtExit.TabIndex = 232;
            this.BtExit.Text = "Exit";
            this.BtExit.UseVisualStyleBackColor = false;
            this.BtExit.Click += new System.EventHandler(this.BtExit_Click);
            // 
            // colorButton4
            // 
            this.colorButton4.BackColor = System.Drawing.Color.Transparent;
            this.colorButton4.BorderLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.colorButton4.Checked = false;
            this.colorButton4.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.colorButton4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorButton4.ForeColor = System.Drawing.Color.Red;
            this.colorButton4.GradientBottom = System.Drawing.Color.LightGray;
            this.colorButton4.GradientTop = System.Drawing.Color.LightGray;
            this.colorButton4.Location = new System.Drawing.Point(269, 107);
            this.colorButton4.Name = "colorButton4";
            this.colorButton4.Size = new System.Drawing.Size(356, 32);
            this.colorButton4.TabIndex = 233;
            this.colorButton4.Text = "Enter model name !";
            this.colorButton4.UseVisualStyleBackColor = false;
            // 
            // TxModel
            // 
            this.TxModel.BackColor = System.Drawing.Color.Transparent;
            this.TxModel.BorderLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.TxModel.Checked = false;
            this.TxModel.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.TxModel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxModel.ForeColor = System.Drawing.Color.Black;
            this.TxModel.GradientBottom = System.Drawing.Color.White;
            this.TxModel.GradientTop = System.Drawing.Color.White;
            this.TxModel.Location = new System.Drawing.Point(377, 10);
            this.TxModel.Name = "TxModel";
            this.TxModel.Size = new System.Drawing.Size(248, 41);
            this.TxModel.TabIndex = 234;
            this.TxModel.UseVisualStyleBackColor = false;
            this.TxModel.Click += new System.EventHandler(this.TxModel_Click);
            // 
            // TB_NEW_MODEL
            // 
            this.TB_NEW_MODEL.BackColor = System.Drawing.Color.Transparent;
            this.TB_NEW_MODEL.BorderLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.TB_NEW_MODEL.Checked = false;
            this.TB_NEW_MODEL.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.TB_NEW_MODEL.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_NEW_MODEL.ForeColor = System.Drawing.Color.Black;
            this.TB_NEW_MODEL.GradientBottom = System.Drawing.Color.White;
            this.TB_NEW_MODEL.GradientTop = System.Drawing.Color.White;
            this.TB_NEW_MODEL.Location = new System.Drawing.Point(377, 60);
            this.TB_NEW_MODEL.Name = "TB_NEW_MODEL";
            this.TB_NEW_MODEL.Size = new System.Drawing.Size(248, 41);
            this.TB_NEW_MODEL.TabIndex = 236;
            this.TB_NEW_MODEL.UseVisualStyleBackColor = false;
            this.TB_NEW_MODEL.Click += new System.EventHandler(this.TB_NEW_MODEL_Click);
            // 
            // colorButton6
            // 
            this.colorButton6.BackColor = System.Drawing.Color.Transparent;
            this.colorButton6.BorderLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.colorButton6.Checked = false;
            this.colorButton6.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.colorButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorButton6.ForeColor = System.Drawing.Color.Black;
            this.colorButton6.GradientBottom = System.Drawing.Color.Gainsboro;
            this.colorButton6.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.colorButton6.Location = new System.Drawing.Point(269, 60);
            this.colorButton6.Name = "colorButton6";
            this.colorButton6.Size = new System.Drawing.Size(102, 41);
            this.colorButton6.TabIndex = 235;
            this.colorButton6.Text = "NEW\r\nMODEL";
            this.colorButton6.UseVisualStyleBackColor = false;
            // 
            // colorButton5
            // 
            this.colorButton5.BackColor = System.Drawing.Color.Transparent;
            this.colorButton5.BorderLineColor = System.Drawing.Color.Blue;
            this.colorButton5.Checked = false;
            this.colorButton5.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.colorButton5.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorButton5.ForeColor = System.Drawing.Color.Black;
            this.colorButton5.GradientBottom = System.Drawing.Color.Gainsboro;
            this.colorButton5.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.colorButton5.Location = new System.Drawing.Point(9, 8);
            this.colorButton5.Name = "colorButton5";
            this.colorButton5.Size = new System.Drawing.Size(248, 34);
            this.colorButton5.TabIndex = 237;
            this.colorButton5.Text = "Model List";
            this.colorButton5.UseVisualStyleBackColor = false;
            // 
            // FormModel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(639, 425);
            this.ControlBox = false;
            this.Controls.Add(this.colorButton5);
            this.Controls.Add(this.TB_NEW_MODEL);
            this.Controls.Add(this.colorButton6);
            this.Controls.Add(this.TxModel);
            this.Controls.Add(this.colorButton4);
            this.Controls.Add(this.BtExit);
            this.Controls.Add(this.BtChange);
            this.Controls.Add(this.BtDelete);
            this.Controls.Add(this.BtCreate);
            this.Controls.Add(this.colorButton2);
            this.Controls.Add(this.colorButton1);
            this.Controls.Add(this.LbModelList);
            this.Controls.Add(this.colorButton3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormModel";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Model Setting";
            this.Load += new System.EventHandler(this.FormModel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public SUserControls.ColorButton colorButton3;
        private System.Windows.Forms.ListBox LbModelList;
        public SUserControls.ColorButton colorButton1;
        public SUserControls.ColorButton colorButton2;
        public SUserControls.ColorButton BtCreate;
        public SUserControls.ColorButton BtDelete;
        public SUserControls.ColorButton BtChange;
        public SUserControls.ColorButton BtExit;
        public SUserControls.ColorButton colorButton4;
        public SUserControls.ColorButton TxModel;
        public SUserControls.ColorButton TB_NEW_MODEL;
        public SUserControls.ColorButton colorButton6;
        public SUserControls.ColorButton colorButton5;
    }
}