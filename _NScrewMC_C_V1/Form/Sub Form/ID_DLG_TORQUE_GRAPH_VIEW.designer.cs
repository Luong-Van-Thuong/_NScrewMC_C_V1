namespace _NScrewMC_C_V1
{
    partial class TorqueGraphView
    {
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartTorque = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chartTorque)).BeginInit();
            this.SuspendLayout();
            // 
            // chartTorque
            // 
            chartArea1.Name = "ChartArea1";
            this.chartTorque.ChartAreas.Add(chartArea1);
            this.chartTorque.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartTorque.Legends.Add(legend1);
            this.chartTorque.Location = new System.Drawing.Point(0, 0);
            this.chartTorque.Name = "chartTorque";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.MarkerBorderColor = System.Drawing.Color.Black;
            series1.Name = "Series1";
            this.chartTorque.Series.Add(series1);
            this.chartTorque.Size = new System.Drawing.Size(747, 278);
            this.chartTorque.TabIndex = 0;
            this.chartTorque.Text = "Torque";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TorqueGraphView
            // 
            this.ClientSize = new System.Drawing.Size(747, 278);
            this.Controls.Add(this.chartTorque);
            this.Name = "TorqueGraphView";
            ((System.ComponentModel.ISupportInitialize)(this.chartTorque)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.DataVisualization.Charting.Chart chartTorque;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.IContainer components;
    }
}