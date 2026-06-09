using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace _NScrewMC_C_V1
{
    public class Barcode : MSystem
    {
        private int m_idx;
        public bool IsConnected { get; set; } =false;
        public bool IsDataReceived { get; set; } = false;
        public string BarcodeData = string.Empty;
        // Serial Port 객체
        public SerialPort SerialPort { get; set; } = null;
        public Barcode(int _index) 
        {
            SerialPort = new SerialPort();
            m_idx = _index;
            InforManager.Instance.LoadSetting();
        }
        public bool Open()
        {
            try
            {
                if (SerialPort != null)
                {
                    Close();
                }
                SerialPort.PortName = InforManager.Instance.BarcodePort[m_idx];

                SerialPort.BaudRate = 115200;

                SerialPort.DataBits = 8;
                SerialPort.StopBits = StopBits.One;
                SerialPort.Parity = Parity.None;
                SerialPort.ReadTimeout = 1000;
                SerialPort.WriteTimeout = 1000;

                SerialPort.Open();  //시리얼포트 열기

                if (SerialPort.IsOpen == true)
                {
                    IsConnected = true;
                    SerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
                    return true;
                }
            }
            catch
            {
                MSystem.m_pLogSave.DevLogSave($"Barcode : {InforManager.Instance.BarcodePort[m_idx]} Open Fail - Disconnected");
                IsConnected = false;
                return false;
            }
            return false;
        }
        public bool DisconnectBcrPort()
        {
            try
            {
                if (SerialPort != null)
                {
                    Close();
                }
                SerialPort.PortName = "100";

                SerialPort.BaudRate = 115200;

                SerialPort.DataBits = 8;
                SerialPort.StopBits = StopBits.One;
                SerialPort.Parity = Parity.None;
                SerialPort.ReadTimeout = 1000;
                SerialPort.WriteTimeout = 1000;

                SerialPort.Open();  //시리얼포트 열기

                if (SerialPort.IsOpen == true)
                {
                    IsConnected = true;
                    SerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
                    return true;
                }
            }
            catch
            {
                //  MSystem.m_pLogSave.DevLogSave($"Barcode : {InforManager.Instance.HantasPort[m_idx]} Open Fail - Disconnected");
                MSystem.m_pLogSave.DevLogSave($"Barcode : {InforManager.Instance.BarcodePort[m_idx]} Open Fail - Disconnected");
                IsConnected = false;
                return false;
            }
            return false;
        }

        public void Close()
        {
            try
            {
                SerialPort.DataReceived -= new SerialDataReceivedEventHandler(DataReceived);

                SerialPort.Close();
                MSystem.m_pLogSave.DevLogSave($"Barcode : {InforManager.Instance.BarcodePort[m_idx]} Close Success - Disconnected");
                IsConnected = false;
            }
            catch
            {
                MSystem.m_pLogSave.DevLogSave($"Barcode : {InforManager.Instance.BarcodePort[m_idx]} Close Fail");
            }
        }
        public bool SendBcrTrig()
        {
            BarcodeData = string.Empty;
            IsDataReceived = false;

            if (SerialPort.IsOpen == false)
                return false;

            SerialPort.Write("+");
            return true;
        }
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(10);
                string data = SerialPort.ReadExisting();

                // 바코드 처리
                if (data.Length >= 8)
                {
                    BarcodeData = data.Trim();
                    IsDataReceived = true;
                }
            }
            catch (Exception ex)
            {
                // 예외 처리 및 로그
                MSystem.m_pLogSave.DevLogSave($"Serial DataReceived Error : {ex.Message}");
            }
        }
    }
}
