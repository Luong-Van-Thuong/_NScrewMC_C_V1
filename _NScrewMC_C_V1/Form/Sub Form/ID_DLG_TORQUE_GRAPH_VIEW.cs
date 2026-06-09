using Modbus.Device;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class TorqueGraphView : Form
    {
        private List<byte> _buffer = new List<byte>();
        private int _unitIndex;
        public TorqueGraphView(int UnitIndex, int pos)
        {
            InitializeComponent();

            _unitIndex = UnitIndex;
            chartTorque.Series[0].Name = $"Torque";
            chartTorque.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chartTorque.Series[0].Points.Clear();
            timer1.Interval = 500;
            timer1.Start();
        }

        private bool Graph_DataReceived()
        {
            byte[] buffer = MSystem.m_pHantas[_unitIndex].SerialPort_DataReceived();

            if (buffer == null || buffer.Length < 4)
            {
                return false;
            }
            int packetLen = buffer[2]; // 패킷 길이
            int frameCount = buffer[3]; // 프레임 카운트
            _buffer.Clear();

            // 첫번째 프레임은 35번지부터 2byte씩 graph data가 들어옴
            for (int i = 35; i < packetLen-32; i++)
            {
                _buffer.Add(buffer[i]);
            }
            for (int i = 0; i < frameCount - 1; i++)
            {
                // 두번째 프레임부터는 5번지부터 2byte씩 graph data가 들어옴
                buffer = MSystem.m_pHantas[_unitIndex].SerialPort_DataReceived();
                packetLen = buffer[2];
                for (int j = 0; j < packetLen - 2; j++)
                {
                    _buffer.Add(buffer[j+5]);
                }
            }
            if (_buffer.Count >= 0)
            {
                ParseGraphData(_buffer.ToArray());
                _buffer.Clear();
                return true;
            }
            return false;
        }

        private void ParseGraphData(byte[] response)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                chartTorque.Series.Clear();
                int ms = 0;
                for (int i = 0; i< response.Length; i+=2)
                {
                    ushort val = (ushort)(response[i] << 8 | (ushort)response[i + 1]);

                    int seconds = (ms % 60000) / 1000; // 초 계산
                    int milliseconds = ms % 1000; // 밀리초 계산

                    // mm:ss:fff 형식으로 문자열 생성
                    string x = $"{seconds:D2}:{milliseconds:D3}";

                    chartTorque.Series[0].Points.AddXY(x, val / 100.0);
                    ms += 10;
                }
            }));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MSystem.m_pHantas[_unitIndex].getTorpueGraphData(0);
            //MSystem.m_pHantas[0].MbWriteRegister(1, 0x8C, data);

            // MSystem.m_pHantas[_unitIndex].RequestGraphData();
            if (Graph_DataReceived()== true)
                timer1.Stop();
        }
    }
}
