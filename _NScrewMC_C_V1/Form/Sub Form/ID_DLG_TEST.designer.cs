namespace _NScrewMC_C_V1
{
    partial class ScrewTest
    {
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BT_LEFT_PICUP_TEST_START = new System.Windows.Forms.Button();
            this.BT_LEFT_PICUP_TEST_STOP = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelPickupCountL = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelPickupCountR = new System.Windows.Forms.Label();
            this.BT_RIGHT_PICUP_TEST_STOP = new System.Windows.Forms.Button();
            this.BT_RIGHT_PICUP_TEST_START = new System.Windows.Forms.Button();
            this.BT_LEFT_VACCUM = new System.Windows.Forms.Button();
            this.BT_LEFT_BLOW = new System.Windows.Forms.Button();
            this.BT_LEFT_READY = new System.Windows.Forms.Button();
            this.BT_RIGHT_READY = new System.Windows.Forms.Button();
            this.BT_RIGHT_BLOW = new System.Windows.Forms.Button();
            this.BT_RIGHT_VACCUM = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // BT_LEFT_PICUP_TEST_START
            // 
            this.BT_LEFT_PICUP_TEST_START.BackColor = System.Drawing.Color.LimeGreen;
            this.BT_LEFT_PICUP_TEST_START.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BT_LEFT_PICUP_TEST_START.Location = new System.Drawing.Point(138, 40);
            this.BT_LEFT_PICUP_TEST_START.Name = "BT_LEFT_PICUP_TEST_START";
            this.BT_LEFT_PICUP_TEST_START.Size = new System.Drawing.Size(121, 52);
            this.BT_LEFT_PICUP_TEST_START.TabIndex = 0;
            this.BT_LEFT_PICUP_TEST_START.Text = "START";
            this.BT_LEFT_PICUP_TEST_START.UseVisualStyleBackColor = false;
            this.BT_LEFT_PICUP_TEST_START.Click += new System.EventHandler(this.BT_LEFT_PICUP_TEST_START_Click);
            // 
            // BT_LEFT_PICUP_TEST_STOP
            // 
            this.BT_LEFT_PICUP_TEST_STOP.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold);
            this.BT_LEFT_PICUP_TEST_STOP.Location = new System.Drawing.Point(137, 92);
            this.BT_LEFT_PICUP_TEST_STOP.Name = "BT_LEFT_PICUP_TEST_STOP";
            this.BT_LEFT_PICUP_TEST_STOP.Size = new System.Drawing.Size(121, 52);
            this.BT_LEFT_PICUP_TEST_STOP.TabIndex = 1;
            this.BT_LEFT_PICUP_TEST_STOP.Text = "STOP";
            this.BT_LEFT_PICUP_TEST_STOP.UseVisualStyleBackColor = true;
            this.BT_LEFT_PICUP_TEST_STOP.Click += new System.EventHandler(this.BT_LEFT_PICUP_TEST_STOP_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BT_LEFT_READY);
            this.groupBox1.Controls.Add(this.BT_LEFT_BLOW);
            this.groupBox1.Controls.Add(this.BT_LEFT_VACCUM);
            this.groupBox1.Controls.Add(this.labelPickupCountL);
            this.groupBox1.Controls.Add(this.BT_LEFT_PICUP_TEST_STOP);
            this.groupBox1.Controls.Add(this.BT_LEFT_PICUP_TEST_START);
            this.groupBox1.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(18, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 273);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "LEFT";
            // 
            // labelPickupCountL
            // 
            this.labelPickupCountL.BackColor = System.Drawing.Color.White;
            this.labelPickupCountL.Location = new System.Drawing.Point(11, 40);
            this.labelPickupCountL.Name = "labelPickupCountL";
            this.labelPickupCountL.Size = new System.Drawing.Size(121, 104);
            this.labelPickupCountL.TabIndex = 2;
            this.labelPickupCountL.Text = "--";
            this.labelPickupCountL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelPickupCountL.Click += new System.EventHandler(this.labelPickupCountL_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.BT_RIGHT_READY);
            this.groupBox2.Controls.Add(this.BT_RIGHT_BLOW);
            this.groupBox2.Controls.Add(this.labelPickupCountR);
            this.groupBox2.Controls.Add(this.BT_RIGHT_VACCUM);
            this.groupBox2.Controls.Add(this.BT_RIGHT_PICUP_TEST_STOP);
            this.groupBox2.Controls.Add(this.BT_RIGHT_PICUP_TEST_START);
            this.groupBox2.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(292, 35);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 273);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RIGHT";
            // 
            // labelPickupCountR
            // 
            this.labelPickupCountR.BackColor = System.Drawing.Color.White;
            this.labelPickupCountR.Location = new System.Drawing.Point(12, 44);
            this.labelPickupCountR.Name = "labelPickupCountR";
            this.labelPickupCountR.Size = new System.Drawing.Size(121, 104);
            this.labelPickupCountR.TabIndex = 3;
            this.labelPickupCountR.Text = "--";
            this.labelPickupCountR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelPickupCountR.Click += new System.EventHandler(this.labelPickupCountR_Click);
            // 
            // BT_RIGHT_PICUP_TEST_STOP
            // 
            this.BT_RIGHT_PICUP_TEST_STOP.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold);
            this.BT_RIGHT_PICUP_TEST_STOP.Location = new System.Drawing.Point(137, 96);
            this.BT_RIGHT_PICUP_TEST_STOP.Name = "BT_RIGHT_PICUP_TEST_STOP";
            this.BT_RIGHT_PICUP_TEST_STOP.Size = new System.Drawing.Size(121, 52);
            this.BT_RIGHT_PICUP_TEST_STOP.TabIndex = 1;
            this.BT_RIGHT_PICUP_TEST_STOP.Text = "STOP";
            this.BT_RIGHT_PICUP_TEST_STOP.UseVisualStyleBackColor = true;
            this.BT_RIGHT_PICUP_TEST_STOP.Click += new System.EventHandler(this.BT_RIGHT_PICUP_TEST_STOP_Click);
            // 
            // BT_RIGHT_PICUP_TEST_START
            // 
            this.BT_RIGHT_PICUP_TEST_START.BackColor = System.Drawing.Color.LimeGreen;
            this.BT_RIGHT_PICUP_TEST_START.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold);
            this.BT_RIGHT_PICUP_TEST_START.Location = new System.Drawing.Point(137, 44);
            this.BT_RIGHT_PICUP_TEST_START.Name = "BT_RIGHT_PICUP_TEST_START";
            this.BT_RIGHT_PICUP_TEST_START.Size = new System.Drawing.Size(122, 52);
            this.BT_RIGHT_PICUP_TEST_START.TabIndex = 0;
            this.BT_RIGHT_PICUP_TEST_START.Text = "START";
            this.BT_RIGHT_PICUP_TEST_START.UseVisualStyleBackColor = false;
            this.BT_RIGHT_PICUP_TEST_START.Click += new System.EventHandler(this.BT_RIGHT_PICUP_TEST_START_Click);
            // 
            // BT_LEFT_VACCUM
            // 
            this.BT_LEFT_VACCUM.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold);
            this.BT_LEFT_VACCUM.Location = new System.Drawing.Point(11, 160);
            this.BT_LEFT_VACCUM.Name = "BT_LEFT_VACCUM";
            this.BT_LEFT_VACCUM.Size = new System.Drawing.Size(121, 52);
            this.BT_LEFT_VACCUM.TabIndex = 3;
            this.BT_LEFT_VACCUM.Text = "Vaccum";
            this.BT_LEFT_VACCUM.UseVisualStyleBackColor = true;
            this.BT_LEFT_VACCUM.Click += new System.EventHandler(this.BT_LEFT_VACCUM_Click);
            // 
            // BT_LEFT_BLOW
            // 
            this.BT_LEFT_BLOW.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold);
            this.BT_LEFT_BLOW.Location = new System.Drawing.Point(138, 160);
            this.BT_LEFT_BLOW.Name = "BT_LEFT_BLOW";
            this.BT_LEFT_BLOW.Size = new System.Drawing.Size(121, 52);
            this.BT_LEFT_BLOW.TabIndex = 4;
            this.BT_LEFT_BLOW.Text = "Blow";
            this.BT_LEFT_BLOW.UseVisualStyleBackColor = true;
            this.BT_LEFT_BLOW.Click += new System.EventHandler(this.BT_LEFT_BLOW_Click);
            // 
            // BT_LEFT_READY
            // 
            this.BT_LEFT_READY.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold);
            this.BT_LEFT_READY.Location = new System.Drawing.Point(11, 213);
            this.BT_LEFT_READY.Name = "BT_LEFT_READY";
            this.BT_LEFT_READY.Size = new System.Drawing.Size(247, 52);
            this.BT_LEFT_READY.TabIndex = 5;
            this.BT_LEFT_READY.Text = "Ready";
            this.BT_LEFT_READY.UseVisualStyleBackColor = true;
            this.BT_LEFT_READY.Click += new System.EventHandler(this.BT_LEFT_READY_Click);
            // 
            // BT_RIGHT_READY
            // 
            this.BT_RIGHT_READY.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold);
            this.BT_RIGHT_READY.Location = new System.Drawing.Point(12, 213);
            this.BT_RIGHT_READY.Name = "BT_RIGHT_READY";
            this.BT_RIGHT_READY.Size = new System.Drawing.Size(247, 52);
            this.BT_RIGHT_READY.TabIndex = 8;
            this.BT_RIGHT_READY.Text = "Ready";
            this.BT_RIGHT_READY.UseVisualStyleBackColor = true;
            this.BT_RIGHT_READY.Click += new System.EventHandler(this.BT_RIGHT_READY_Click);
            // 
            // BT_RIGHT_BLOW
            // 
            this.BT_RIGHT_BLOW.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold);
            this.BT_RIGHT_BLOW.Location = new System.Drawing.Point(139, 160);
            this.BT_RIGHT_BLOW.Name = "BT_RIGHT_BLOW";
            this.BT_RIGHT_BLOW.Size = new System.Drawing.Size(121, 52);
            this.BT_RIGHT_BLOW.TabIndex = 7;
            this.BT_RIGHT_BLOW.Text = "Blow";
            this.BT_RIGHT_BLOW.UseVisualStyleBackColor = true;
            this.BT_RIGHT_BLOW.Click += new System.EventHandler(this.BT_RIGHT_BLOW_Click);
            // 
            // BT_RIGHT_VACCUM
            // 
            this.BT_RIGHT_VACCUM.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold);
            this.BT_RIGHT_VACCUM.Location = new System.Drawing.Point(12, 160);
            this.BT_RIGHT_VACCUM.Name = "BT_RIGHT_VACCUM";
            this.BT_RIGHT_VACCUM.Size = new System.Drawing.Size(121, 52);
            this.BT_RIGHT_VACCUM.TabIndex = 6;
            this.BT_RIGHT_VACCUM.Text = "Vaccum";
            this.BT_RIGHT_VACCUM.UseVisualStyleBackColor = true;
            this.BT_RIGHT_VACCUM.Click += new System.EventHandler(this.BT_RIGHT_VACCUM_Click);
            // 
            // ScrewTest
            // 
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(583, 320);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ScrewTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pickup Test";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button BT_LEFT_PICUP_TEST_START;
        private System.Windows.Forms.Button BT_LEFT_PICUP_TEST_STOP;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button BT_RIGHT_PICUP_TEST_STOP;
        private System.Windows.Forms.Button BT_RIGHT_PICUP_TEST_START;
        private System.Windows.Forms.Label labelPickupCountL;
        private System.Windows.Forms.Label labelPickupCountR;
        private System.Windows.Forms.Button BT_LEFT_READY;
        private System.Windows.Forms.Button BT_LEFT_BLOW;
        private System.Windows.Forms.Button BT_LEFT_VACCUM;
        private System.Windows.Forms.Button BT_RIGHT_READY;
        private System.Windows.Forms.Button BT_RIGHT_BLOW;
        private System.Windows.Forms.Button BT_RIGHT_VACCUM;
    }
}