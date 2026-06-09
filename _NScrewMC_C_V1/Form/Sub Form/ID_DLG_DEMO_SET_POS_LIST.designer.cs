namespace _NScrewMC_C_V1
{
    partial class DemoSetPosList
    {
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LIST_DEMO_SET = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BT_FILE_SAVE = new System.Windows.Forms.Button();
            this.BT_CLOSE = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LIST_DEMO_SET
            // 
            this.LIST_DEMO_SET.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LIST_DEMO_SET.GridLines = true;
            this.LIST_DEMO_SET.HideSelection = false;
            this.LIST_DEMO_SET.Location = new System.Drawing.Point(24, 51);
            this.LIST_DEMO_SET.MultiSelect = false;
            this.LIST_DEMO_SET.Name = "LIST_DEMO_SET";
            this.LIST_DEMO_SET.Size = new System.Drawing.Size(663, 335);
            this.LIST_DEMO_SET.TabIndex = 0;
            this.LIST_DEMO_SET.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.DarkGray;
            this.label1.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(24, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(255, 33);
            this.label1.TabIndex = 1;
            this.label1.Text = "DEMO";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightBlue;
            this.label2.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(280, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 33);
            this.label2.TabIndex = 2;
            this.label2.Text = "SET";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.DodgerBlue;
            this.label3.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(485, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(202, 33);
            this.label3.TabIndex = 3;
            this.label3.Text = "DIFF";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BT_FILE_SAVE
            // 
            this.BT_FILE_SAVE.BackColor = System.Drawing.Color.LightSteelBlue;
            this.BT_FILE_SAVE.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BT_FILE_SAVE.Location = new System.Drawing.Point(23, 390);
            this.BT_FILE_SAVE.Name = "BT_FILE_SAVE";
            this.BT_FILE_SAVE.Size = new System.Drawing.Size(328, 55);
            this.BT_FILE_SAVE.TabIndex = 4;
            this.BT_FILE_SAVE.Text = "File Save";
            this.BT_FILE_SAVE.UseVisualStyleBackColor = false;
            this.BT_FILE_SAVE.Click += new System.EventHandler(this.BT_FILE_SAVE_Click);
            // 
            // BT_CLOSE
            // 
            this.BT_CLOSE.BackColor = System.Drawing.Color.Salmon;
            this.BT_CLOSE.Font = new System.Drawing.Font("HY견고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BT_CLOSE.Location = new System.Drawing.Point(359, 391);
            this.BT_CLOSE.Name = "BT_CLOSE";
            this.BT_CLOSE.Size = new System.Drawing.Size(328, 55);
            this.BT_CLOSE.TabIndex = 5;
            this.BT_CLOSE.Text = "Close";
            this.BT_CLOSE.UseVisualStyleBackColor = false;
            this.BT_CLOSE.Click += new System.EventHandler(this.BT_CLOSE_Click);
            // 
            // DemoSetPosList
            // 
            this.ClientSize = new System.Drawing.Size(714, 449);
            this.Controls.Add(this.BT_CLOSE);
            this.Controls.Add(this.BT_FILE_SAVE);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LIST_DEMO_SET);
            this.Name = "DemoSetPosList";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.ListView LIST_DEMO_SET;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BT_FILE_SAVE;
        private System.Windows.Forms.Button BT_CLOSE;
    }
}