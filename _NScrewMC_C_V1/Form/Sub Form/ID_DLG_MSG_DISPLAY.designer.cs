
namespace _NScrewMC_C_V1
{
    partial class FMsgDisplay
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
            this.LB_msg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LB_msg
            // 
            this.LB_msg.BackColor = System.Drawing.Color.White;
            this.LB_msg.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_msg.ForeColor = System.Drawing.Color.Red;
            this.LB_msg.Location = new System.Drawing.Point(3, 3);
            this.LB_msg.Name = "LB_msg";
            this.LB_msg.Size = new System.Drawing.Size(698, 88);
            this.LB_msg.TabIndex = 0;
            this.LB_msg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FMsgDisplay
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(704, 94);
            this.Controls.Add(this.LB_msg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FMsgDisplay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FMsgDisplay";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FMsgDisplay_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LB_msg;
    }
}