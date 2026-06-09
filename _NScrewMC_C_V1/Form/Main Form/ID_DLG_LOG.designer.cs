namespace _NScrewMC_C_V1
{
    partial class FormLog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.IDC_CLEAR = new SUserControls.ColorButton();
            this.BT_LOG_ERROR = new SUserControls.ColorButton();
            this.IDC_DATA_PICK_TIME_START = new System.Windows.Forms.DateTimePicker();
            this.IDC_DATA_PICK_TIME_END = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.colorButton1 = new SUserControls.ColorButton();
            this.LV_LOG_COUNT = new System.Windows.Forms.ListView();
            this.LV_LOG = new System.Windows.Forms.ListView();
            this.colorButton2 = new SUserControls.ColorButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.IDC_CLEAR);
            this.panel1.Controls.Add(this.BT_LOG_ERROR);
            this.panel1.Controls.Add(this.IDC_DATA_PICK_TIME_START);
            this.panel1.Controls.Add(this.IDC_DATA_PICK_TIME_END);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(669, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(343, 175);
            this.panel1.TabIndex = 228;
            // 
            // IDC_CLEAR
            // 
            this.IDC_CLEAR.BackColor = System.Drawing.Color.Transparent;
            this.IDC_CLEAR.BorderLineColor = System.Drawing.Color.Blue;
            this.IDC_CLEAR.Checked = false;
            this.IDC_CLEAR.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.IDC_CLEAR.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IDC_CLEAR.ForeColor = System.Drawing.Color.Black;
            this.IDC_CLEAR.GradientBottom = System.Drawing.Color.Gainsboro;
            this.IDC_CLEAR.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.IDC_CLEAR.Location = new System.Drawing.Point(3, 114);
            this.IDC_CLEAR.Name = "IDC_CLEAR";
            this.IDC_CLEAR.Size = new System.Drawing.Size(158, 41);
            this.IDC_CLEAR.TabIndex = 223;
            this.IDC_CLEAR.Text = "Clear Error";
            this.IDC_CLEAR.UseVisualStyleBackColor = false;
            this.IDC_CLEAR.Click += new System.EventHandler(this.IDC_CLEAR_Click);
            // 
            // BT_LOG_ERROR
            // 
            this.BT_LOG_ERROR.BackColor = System.Drawing.Color.Transparent;
            this.BT_LOG_ERROR.BorderLineColor = System.Drawing.Color.Blue;
            this.BT_LOG_ERROR.Checked = false;
            this.BT_LOG_ERROR.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.BT_LOG_ERROR.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_LOG_ERROR.ForeColor = System.Drawing.Color.Black;
            this.BT_LOG_ERROR.GradientBottom = System.Drawing.Color.Gainsboro;
            this.BT_LOG_ERROR.GradientTop = System.Drawing.Color.WhiteSmoke;
            this.BT_LOG_ERROR.Location = new System.Drawing.Point(180, 114);
            this.BT_LOG_ERROR.Name = "BT_LOG_ERROR";
            this.BT_LOG_ERROR.Size = new System.Drawing.Size(158, 41);
            this.BT_LOG_ERROR.TabIndex = 212;
            this.BT_LOG_ERROR.Text = "Read Error";
            this.BT_LOG_ERROR.UseVisualStyleBackColor = false;
            this.BT_LOG_ERROR.Click += new System.EventHandler(this.BT_LOG_ERROR_Click);
            // 
            // IDC_DATA_PICK_TIME_START
            // 
            this.IDC_DATA_PICK_TIME_START.CalendarFont = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IDC_DATA_PICK_TIME_START.CustomFormat = "ddd,dd-MM-yyyy";
            this.IDC_DATA_PICK_TIME_START.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IDC_DATA_PICK_TIME_START.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.IDC_DATA_PICK_TIME_START.Location = new System.Drawing.Point(125, 11);
            this.IDC_DATA_PICK_TIME_START.MinDate = new System.DateTime(2022, 12, 14, 0, 0, 0, 0);
            this.IDC_DATA_PICK_TIME_START.Name = "IDC_DATA_PICK_TIME_START";
            this.IDC_DATA_PICK_TIME_START.Size = new System.Drawing.Size(211, 32);
            this.IDC_DATA_PICK_TIME_START.TabIndex = 220;
            this.IDC_DATA_PICK_TIME_START.Value = new System.DateTime(2022, 12, 15, 0, 0, 0, 0);
            // 
            // IDC_DATA_PICK_TIME_END
            // 
            this.IDC_DATA_PICK_TIME_END.CalendarFont = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IDC_DATA_PICK_TIME_END.CustomFormat = " ddd,dd-MM-yyyy";
            this.IDC_DATA_PICK_TIME_END.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IDC_DATA_PICK_TIME_END.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.IDC_DATA_PICK_TIME_END.Location = new System.Drawing.Point(125, 54);
            this.IDC_DATA_PICK_TIME_END.MinDate = new System.DateTime(2022, 12, 15, 0, 0, 0, 0);
            this.IDC_DATA_PICK_TIME_END.Name = "IDC_DATA_PICK_TIME_END";
            this.IDC_DATA_PICK_TIME_END.Size = new System.Drawing.Size(211, 32);
            this.IDC_DATA_PICK_TIME_END.TabIndex = 219;
            this.IDC_DATA_PICK_TIME_END.Value = new System.DateTime(2022, 12, 15, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 23);
            this.label2.TabIndex = 222;
            this.label2.Text = "End Date  :";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 23);
            this.label1.TabIndex = 221;
            this.label1.Text = "Start Date :";
            // 
            // colorButton1
            // 
            this.colorButton1.BackColor = System.Drawing.Color.Transparent;
            this.colorButton1.BorderLineColor = System.Drawing.Color.Black;
            this.colorButton1.Checked = false;
            this.colorButton1.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.colorButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorButton1.ForeColor = System.Drawing.Color.Black;
            this.colorButton1.GradientBottom = System.Drawing.Color.Silver;
            this.colorButton1.GradientTop = System.Drawing.Color.Silver;
            this.colorButton1.Location = new System.Drawing.Point(1, 184);
            this.colorButton1.Name = "colorButton1";
            this.colorButton1.Size = new System.Drawing.Size(500, 33);
            this.colorButton1.TabIndex = 226;
            this.colorButton1.Text = "Error Time List";
            this.colorButton1.UseVisualStyleBackColor = false;
            // 
            // LV_LOG_COUNT
            // 
            this.LV_LOG_COUNT.BackColor = System.Drawing.Color.White;
            this.LV_LOG_COUNT.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LV_LOG_COUNT.HideSelection = false;
            this.LV_LOG_COUNT.Location = new System.Drawing.Point(512, 216);
            this.LV_LOG_COUNT.Name = "LV_LOG_COUNT";
            this.LV_LOG_COUNT.Size = new System.Drawing.Size(500, 214);
            this.LV_LOG_COUNT.TabIndex = 225;
            this.LV_LOG_COUNT.UseCompatibleStateImageBehavior = false;
            // 
            // LV_LOG
            // 
            this.LV_LOG.BackColor = System.Drawing.Color.White;
            this.LV_LOG.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LV_LOG.HideSelection = false;
            this.LV_LOG.Location = new System.Drawing.Point(1, 216);
            this.LV_LOG.Name = "LV_LOG";
            this.LV_LOG.Size = new System.Drawing.Size(500, 214);
            this.LV_LOG.TabIndex = 224;
            this.LV_LOG.UseCompatibleStateImageBehavior = false;
            // 
            // colorButton2
            // 
            this.colorButton2.BackColor = System.Drawing.Color.Transparent;
            this.colorButton2.BorderLineColor = System.Drawing.Color.Black;
            this.colorButton2.Checked = false;
            this.colorButton2.CheckedButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(238)))), ((int)(((byte)(160)))));
            this.colorButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorButton2.ForeColor = System.Drawing.Color.Black;
            this.colorButton2.GradientBottom = System.Drawing.Color.Silver;
            this.colorButton2.GradientTop = System.Drawing.Color.Silver;
            this.colorButton2.Location = new System.Drawing.Point(512, 184);
            this.colorButton2.Name = "colorButton2";
            this.colorButton2.Size = new System.Drawing.Size(500, 33);
            this.colorButton2.TabIndex = 227;
            this.colorButton2.Text = "Error Count";
            this.colorButton2.UseVisualStyleBackColor = false;
            // 
            // FormLog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1024, 433);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.colorButton2);
            this.Controls.Add(this.colorButton1);
            this.Controls.Add(this.LV_LOG_COUNT);
            this.Controls.Add(this.LV_LOG);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLog";
            this.Text = "FormLog";
            this.Load += new System.EventHandler(this.FormLog_Load);
            this.VisibleChanged += new System.EventHandler(this.FormLog_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public SUserControls.ColorButton IDC_CLEAR;
        public SUserControls.ColorButton BT_LOG_ERROR;
        private System.Windows.Forms.DateTimePicker IDC_DATA_PICK_TIME_START;
        private System.Windows.Forms.DateTimePicker IDC_DATA_PICK_TIME_END;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public SUserControls.ColorButton colorButton1;
        private System.Windows.Forms.ListView LV_LOG_COUNT;
        private System.Windows.Forms.ListView LV_LOG;
        public SUserControls.ColorButton colorButton2;
    }
}