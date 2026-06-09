namespace _NScrewMC_C_V1
{
    partial class DemoSet
    {
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BT_DEMO_SELECT = new System.Windows.Forms.Button();
            this.BT_SET_SELECT = new System.Windows.Forms.Button();
            this.BT_CLOSE = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BT_DEMO_SELECT
            // 
            this.BT_DEMO_SELECT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BT_DEMO_SELECT.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BT_DEMO_SELECT.Location = new System.Drawing.Point(16, 12);
            this.BT_DEMO_SELECT.Name = "BT_DEMO_SELECT";
            this.BT_DEMO_SELECT.Size = new System.Drawing.Size(200, 121);
            this.BT_DEMO_SELECT.TabIndex = 0;
            this.BT_DEMO_SELECT.Text = "DEMO";
            this.BT_DEMO_SELECT.UseVisualStyleBackColor = true;
            this.BT_DEMO_SELECT.Click += new System.EventHandler(this.BT_DEMO_SELECT_Click);
            // 
            // BT_SET_SELECT
            // 
            this.BT_SET_SELECT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BT_SET_SELECT.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BT_SET_SELECT.Location = new System.Drawing.Point(236, 12);
            this.BT_SET_SELECT.Name = "BT_SET_SELECT";
            this.BT_SET_SELECT.Size = new System.Drawing.Size(200, 121);
            this.BT_SET_SELECT.TabIndex = 1;
            this.BT_SET_SELECT.Text = "SET";
            this.BT_SET_SELECT.UseVisualStyleBackColor = true;
            this.BT_SET_SELECT.Click += new System.EventHandler(this.BT_SET_SELECT_Click);
            // 
            // BT_CLOSE
            // 
            this.BT_CLOSE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BT_CLOSE.BackColor = System.Drawing.Color.LightCoral;
            this.BT_CLOSE.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BT_CLOSE.Location = new System.Drawing.Point(16, 139);
            this.BT_CLOSE.Name = "BT_CLOSE";
            this.BT_CLOSE.Size = new System.Drawing.Size(420, 70);
            this.BT_CLOSE.TabIndex = 2;
            this.BT_CLOSE.Text = "Close";
            this.BT_CLOSE.UseVisualStyleBackColor = false;
            this.BT_CLOSE.Click += new System.EventHandler(this.BT_CLOSE_Click);
            // 
            // DemoSet
            // 
            this.ClientSize = new System.Drawing.Size(456, 221);
            this.ControlBox = false;
            this.Controls.Add(this.BT_CLOSE);
            this.Controls.Add(this.BT_SET_SELECT);
            this.Controls.Add(this.BT_DEMO_SELECT);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DemoSet";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button BT_DEMO_SELECT;
        private System.Windows.Forms.Button BT_SET_SELECT;
        private System.Windows.Forms.Button BT_CLOSE;
    }
}