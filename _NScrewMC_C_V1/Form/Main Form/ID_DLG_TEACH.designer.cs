using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    partial class FormTeach : Form
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
            this.BT_SCREW_LEFT = new MyButton.ButtonPress();
            this.BT_SCREW_RIGHT = new MyButton.ButtonPress();
            this.BT_TEST = new MyButton.ButtonPress();
            this.SuspendLayout();
            // 
            // BT_SCREW_LEFT
            // 
            this.BT_SCREW_LEFT.BorderLineColor = System.Drawing.Color.Blue;
            this.BT_SCREW_LEFT.Checked = false;
            this.BT_SCREW_LEFT.CheckedButtonColor = System.Drawing.Color.Navy;
            this.BT_SCREW_LEFT.ColorPress = System.Drawing.Color.GreenYellow;
            this.BT_SCREW_LEFT.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_SCREW_LEFT.ForeColor = System.Drawing.Color.Black;
            this.BT_SCREW_LEFT.GradientBottom = System.Drawing.Color.Khaki;
            this.BT_SCREW_LEFT.GradientTop = System.Drawing.Color.Khaki;
            this.BT_SCREW_LEFT.ImagePress = null;
            this.BT_SCREW_LEFT.ImageRelease = null;
            this.BT_SCREW_LEFT.Location = new System.Drawing.Point(224, 119);
            this.BT_SCREW_LEFT.Name = "BT_SCREW_LEFT";
            this.BT_SCREW_LEFT.Offset_Image_X = 0;
            this.BT_SCREW_LEFT.RectCornerRadius = 5;
            this.BT_SCREW_LEFT.SetPress = false;
            this.BT_SCREW_LEFT.Size = new System.Drawing.Size(262, 167);
            this.BT_SCREW_LEFT.TabIndex = 465;
            this.BT_SCREW_LEFT.Text = "Screw Left";
            this.BT_SCREW_LEFT.UseVisualStyleBackColor = true;
            this.BT_SCREW_LEFT.Click += new System.EventHandler(this.BT_SCREW_LEFT_Click);
            // 
            // BT_SCREW_RIGHT
            // 
            this.BT_SCREW_RIGHT.BorderLineColor = System.Drawing.Color.Blue;
            this.BT_SCREW_RIGHT.Checked = false;
            this.BT_SCREW_RIGHT.CheckedButtonColor = System.Drawing.Color.Navy;
            this.BT_SCREW_RIGHT.ColorPress = System.Drawing.Color.GreenYellow;
            this.BT_SCREW_RIGHT.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_SCREW_RIGHT.ForeColor = System.Drawing.Color.Black;
            this.BT_SCREW_RIGHT.GradientBottom = System.Drawing.Color.Khaki;
            this.BT_SCREW_RIGHT.GradientTop = System.Drawing.Color.Khaki;
            this.BT_SCREW_RIGHT.ImagePress = null;
            this.BT_SCREW_RIGHT.ImageRelease = null;
            this.BT_SCREW_RIGHT.Location = new System.Drawing.Point(518, 119);
            this.BT_SCREW_RIGHT.Name = "BT_SCREW_RIGHT";
            this.BT_SCREW_RIGHT.Offset_Image_X = 0;
            this.BT_SCREW_RIGHT.RectCornerRadius = 5;
            this.BT_SCREW_RIGHT.SetPress = false;
            this.BT_SCREW_RIGHT.Size = new System.Drawing.Size(262, 167);
            this.BT_SCREW_RIGHT.TabIndex = 466;
            this.BT_SCREW_RIGHT.Text = "Screw Right";
            this.BT_SCREW_RIGHT.UseVisualStyleBackColor = true;
            this.BT_SCREW_RIGHT.Click += new System.EventHandler(this.BT_SCREW_MIDDLE_ClickEvent);
            // 
            // BT_TEST
            // 
            this.BT_TEST.BorderLineColor = System.Drawing.Color.Blue;
            this.BT_TEST.Checked = false;
            this.BT_TEST.CheckedButtonColor = System.Drawing.Color.Navy;
            this.BT_TEST.ColorPress = System.Drawing.Color.GreenYellow;
            this.BT_TEST.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_TEST.ForeColor = System.Drawing.Color.Black;
            this.BT_TEST.GradientBottom = System.Drawing.Color.Khaki;
            this.BT_TEST.GradientTop = System.Drawing.Color.Khaki;
            this.BT_TEST.ImagePress = null;
            this.BT_TEST.ImageRelease = null;
            this.BT_TEST.Location = new System.Drawing.Point(386, 292);
            this.BT_TEST.Name = "BT_TEST";
            this.BT_TEST.Offset_Image_X = 0;
            this.BT_TEST.RectCornerRadius = 5;
            this.BT_TEST.SetPress = false;
            this.BT_TEST.Size = new System.Drawing.Size(262, 167);
            this.BT_TEST.TabIndex = 467;
            this.BT_TEST.Text = "Pickup Test";
            this.BT_TEST.UseVisualStyleBackColor = true;
            this.BT_TEST.Click += new System.EventHandler(this.BT_TEST_Click);
            // 
            // FormTeach
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1024, 600);
            this.Controls.Add(this.BT_TEST);
            this.Controls.Add(this.BT_SCREW_RIGHT);
            this.Controls.Add(this.BT_SCREW_LEFT);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormTeach";
            this.Text = "FormLog";
            this.ResumeLayout(false);

        }

        #endregion
        private MyButton.ButtonPress BT_SCREW_LEFT;
        private MyButton.ButtonPress BT_SCREW_RIGHT;
        private MyButton.ButtonPress BT_TEST;
    }
}