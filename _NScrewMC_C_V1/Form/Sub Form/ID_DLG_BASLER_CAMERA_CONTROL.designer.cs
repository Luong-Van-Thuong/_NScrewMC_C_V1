namespace _NScrewMC_C_V1
{
    partial class BaslerCameraControl
    {
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonGrapStart = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // buttonGrapStart
            // 
            this.buttonGrapStart.Location = new System.Drawing.Point(12, 23);
            this.buttonGrapStart.Name = "buttonGrapStart";
            this.buttonGrapStart.Size = new System.Drawing.Size(103, 38);
            this.buttonGrapStart.TabIndex = 0;
            this.buttonGrapStart.Text = "button1";
            this.buttonGrapStart.UseVisualStyleBackColor = true;
            this.buttonGrapStart.Click += new System.EventHandler(this.buttonGrapStart_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 38);
            this.button1.TabIndex = 1;
            this.button1.Text = "SRC";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(121, 88);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(103, 38);
            this.button2.TabIndex = 2;
            this.button2.Text = "First Pos";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(230, 88);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(103, 38);
            this.button3.TabIndex = 3;
            this.button3.Text = "Last Pos";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(20, 150);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(324, 352);
            this.listBox1.TabIndex = 4;
            // 
            // BaslerCameraControl
            // 
            this.ClientSize = new System.Drawing.Size(1008, 726);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonGrapStart);
            this.Name = "BaslerCameraControl";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button buttonGrapStart;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListBox listBox1;
    }
}