namespace _NScrewMC_C_V1
{
    partial class DetailView
    {
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textBoxDetailPos = new System.Windows.Forms.TextBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonPageUp = new System.Windows.Forms.Button();
            this.buttonPageDown = new System.Windows.Forms.Button();
            this.labelPageCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.Location = new System.Drawing.Point(5, 40);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(235, 285);
            this.dataGridView1.TabIndex = 0;
            // 
            // textBoxDetailPos
            // 
            this.textBoxDetailPos.BackColor = System.Drawing.SystemColors.HotTrack;
            this.textBoxDetailPos.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBoxDetailPos.Location = new System.Drawing.Point(5, 3);
            this.textBoxDetailPos.Name = "textBoxDetailPos";
            this.textBoxDetailPos.Size = new System.Drawing.Size(301, 35);
            this.textBoxDetailPos.TabIndex = 2;
            this.textBoxDetailPos.Text = "RIGHT - P00";
            this.textBoxDetailPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonReset
            // 
            this.buttonReset.BackColor = System.Drawing.Color.DarkOrange;
            this.buttonReset.Location = new System.Drawing.Point(5, 351);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(301, 49);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.Text = "RESET";
            this.buttonReset.UseVisualStyleBackColor = false;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonPageUp
            // 
            this.buttonPageUp.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonPageUp.Location = new System.Drawing.Point(240, 40);
            this.buttonPageUp.Name = "buttonPageUp";
            this.buttonPageUp.Size = new System.Drawing.Size(66, 133);
            this.buttonPageUp.TabIndex = 4;
            this.buttonPageUp.Text = "▲PageUp";
            this.buttonPageUp.UseVisualStyleBackColor = true;
            this.buttonPageUp.Click += new System.EventHandler(this.buttonPageUp_Click);
            // 
            // buttonPageDown
            // 
            this.buttonPageDown.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonPageDown.Location = new System.Drawing.Point(240, 192);
            this.buttonPageDown.Name = "buttonPageDown";
            this.buttonPageDown.Size = new System.Drawing.Size(66, 133);
            this.buttonPageDown.TabIndex = 5;
            this.buttonPageDown.Text = "PageDown▼";
            this.buttonPageDown.UseVisualStyleBackColor = true;
            this.buttonPageDown.Click += new System.EventHandler(this.buttonPageDown_Click);
            // 
            // labelPageCount
            // 
            this.labelPageCount.AutoSize = true;
            this.labelPageCount.Location = new System.Drawing.Point(84, 329);
            this.labelPageCount.Name = "labelPageCount";
            this.labelPageCount.Size = new System.Drawing.Size(76, 19);
            this.labelPageCount.TabIndex = 7;
            this.labelPageCount.Text = "1 / 100";
            // 
            // DetailView
            // 
            this.ClientSize = new System.Drawing.Size(309, 407);
            this.Controls.Add(this.labelPageCount);
            this.Controls.Add(this.buttonPageDown);
            this.Controls.Add(this.buttonPageUp);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.textBoxDetailPos);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "DetailView";
            this.Text = "Detail View";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBoxDetailPos;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonPageUp;
        private System.Windows.Forms.Button buttonPageDown;
        private System.Windows.Forms.Label labelPageCount;
    }
}