using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace _NScrewMC_C_V1
{
   
    
    public class ModbusIO : MSystem
    {
        #region // Variable
        public  int _TotalStation = 1;
         TcpClient tcpClient;
         ModbusIpMaster mbTCP;
         SerialPort serialPort;
         ModbusSerialMaster mbRTU;
        int m_idx = 0;
        ////////////////////////////////
        public  bool Mb_Connection,Mb_Temp_Connect;
        public  string PortName = "COM23";
        public  int baudRate = 57600;
        public  int databit = 8;
        public  Parity parity = Parity.None;
        public  StopBits stopBits = StopBits.One;

        public List<byte> GraphData = new List<byte>();
        #endregion

        #region //Modbus RTU
        public ModbusIO(int _midx) 
        {
            m_idx = _midx;
            //InforManager.Instance.LoadSetting();
            PortName = InforManager.Instance.HantasPort[m_idx];
            //CreateMbRTU();
        }
        public  bool CreateMbRTU()
        {
            if (mbRTU != null)
            {
                mbRTU.Dispose();
                mbRTU = null;
            }
            mbRTU = null;
            if (serialPort != null)
            {
                serialPort.Close();
                serialPort.Dispose();
                serialPort = null;
            } 
            try
            {
                serialPort = new SerialPort();
                serialPort.PortName = PortName;
                serialPort.BaudRate = baudRate;
                serialPort.DataBits = databit;
                serialPort.Parity = parity;
                serialPort.StopBits = stopBits;
                if (serialPort.IsOpen) serialPort.Close();
                serialPort.Open();
                Mb_Connection = serialPort.IsOpen;

                //serialPort.DataReceived += new SerialDataReceivedEventHandler(ScrewDrv_DataReceived);
                //serialPort.Open();

                //// Define Modbus RTU request (example: read holding register)
                //byte[] request = { 0x01, 0x04, 0x0C, 0x80, 0x00, 0x0D, 0x33, 0x77 };

                //// Send request
                //serialPort.Write(request, 0, request.Length);

                //// Wait for response
                //System.Threading.Thread.Sleep(100); // Adjust delay as needed

                //// Read response
                //byte[] response = new byte[serialPort.BytesToRead];
                //serialPort.Read(response, 0, response.Length);

                mbRTU = ModbusSerialMaster.CreateRtu(serialPort);
                mbRTU.Transport.Retries = 1;
                mbRTU.Transport.ReadTimeout = 100;
                mbRTU.Transport.WriteTimeout = 100;
                mbRTU.Transport.RetryOnOldResponseThreshold = 100;

                //
                ushort[] temp = mbRTU.ReadInputRegisters(1, 0, 1);
                if (temp == null)
                    return false;

                return true;
            }
            catch (Exception e)
            {
                //MSystem.m_pLogSave.DevLogSave("Modbus COM Port Open Error : " + e.Message);
                return false;
            }
        }
        private void ScrewDrv_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(5);
                byte[] buffer = new byte[100];// = Encoding.Default.GetBytes(data);

                int readCount;// = buffer.Length;
                readCount = serialPort.Read(buffer, 0, 100);
                if (buffer.Length >= 10)
                {
                    ushort values = 0x00;
                    switch (buffer[1])
                    {
                        case 0x03: // Read Set Torque Value
                            if (readCount > 16)
                            {
                                values = (ushort)((buffer[3] << 8) | buffer[4]);
                                values = (ushort)((buffer[15] << 8) | buffer[16]);
                            }
                            break;
                        case 0x04: // Read Last Torque Value
                            if (readCount > 12)
                            {
                                values = (ushort)((buffer[9] << 8) | buffer[10]);
                                values = (ushort)((buffer[11] << 8) | buffer[12]);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MSystem.m_pLogSave.DevLogSave("ScrewDrv_DataReceived Error : " + ex.Message);
            }
        }
        public bool MbWriteCoil(byte SlaveID, ushort startadd, bool value)
        {
            try
            {
                mbRTU.WriteSingleCoil(SlaveID, startadd, value);
                return true;
            }
            catch (Exception e)
            {
                MSystem.m_pLogSave.DevLogSave("MbWriteCoil Error : " + e.Message);
                return false;
            }
        }
        public bool MbWriteMultiCoil(byte SlaveID, ushort startadd, bool[] value)
        {
            try
            {
                mbRTU.WriteMultipleCoils(SlaveID, startadd, value);
                return true;
            }
            catch (Exception e)
            {
                MSystem.m_pLogSave.DevLogSave("MbWriteMultiCoil Error : " + e.Message);
                return false;
            }
        }
        public bool[] MBReadCoil(byte SlaveID, ushort startadd, ushort numberCoil)
        {
            try
            {
                return mbRTU.ReadCoils(SlaveID, startadd, numberCoil);
            }
            catch (Exception e)
            {
                MSystem.m_pLogSave.DevLogSave("MBReadCoil Error : " + e.Message);
                return null;
            }
        }
        public bool[] MbReadInputContact(byte SlaveID, ushort startadd, ushort numberCoil)
        {
            try
            {
                return mbRTU.ReadInputs(SlaveID, startadd, numberCoil);
            }
            catch (Exception e)
            {
                MSystem.m_pLogSave.DevLogSave("MbReadInputContact Error : " + e.Message);
                return null;
            }
        }
        public  ushort[] MbReadInputRegister(byte SlaveID, ushort firstAddress, ushort num)
        {
            try
            {
                if (mbRTU == null) return null;

               return mbRTU.ReadInputRegistersAsync(SlaveID, firstAddress, num).Result;
            }
            catch (Exception e)
            {
                MSystem.m_pLogSave.DevLogSave("MbReadInputRegister Error : " + e.Message);
                return null;
            }
        }
        public  ushort[] MbReadHoldingRegister(byte SlaveID, ushort firstAdd, ushort num)
        {
            try
            {
                if (mbRTU == null) return null;
                return mbRTU.ReadHoldingRegistersAsync(SlaveID, firstAdd, num).Result;
            }
            catch (Exception e)
            {
                MSystem.m_pLogSave.DevLogSave("MbReadHoldingRegister Error : " + e.Message);
                return null;
            }
        }
        public  bool MbWriteHoldingRegister(byte SlaveID, ushort firstAdd, ushort[] data,int startPoint,int numPoint)
        {
            try{
                ushort[] DataOut = new ushort[numPoint];
                Array.Copy(data, startPoint, DataOut, 0, numPoint);
                mbRTU.WriteMultipleRegisters(SlaveID, firstAdd, DataOut);
                return true;
            }
            catch(Exception e) 
            {
                MSystem.m_pLogSave.DevLogSave("MbWriteHoldingRegister Error : " + e.Message);
                return false;
            }
        }
        public bool MbWriteRegister(byte SlaveID, ushort firstAdd, ushort[] data)
        {
            if (serialPort.IsOpen == false) return false;
            try
            {
                mbRTU.WriteMultipleRegisters(SlaveID, firstAdd, data);
                return true;
            }
            catch (Exception e)
            {
                MSystem.m_pLogSave.DevLogSave("MbWriteRegister Error : " + e.Message);
                return false;
            }
        }
        public void getTorpueGraphData(int frameIndex)
        {
            byte[] frame = new byte[5];//[data.Length*2 +6];
            frame[0] = 0x01; // Slave ID
            frame[1] = 0xC8; // Custom function code (6)
            frame[2] = (byte)frameIndex;
            ushort crc = ModRTU_CRC(frame, 3);
            frame[3] = (byte)(crc & 0xFF);
            frame[4] = (byte)(crc >> 8);
            serialPort.Write(frame, 0, frame.Length);
        }
        public bool setTorpueData(int preset, int _tueque)
        {
            if (serialPort == null)
                return false;
            int address = 0;
            if (preset == 1)
                address = 2;
            else if (preset == 2)
                address = 17;
            else if (preset == 3)
                address = 32;
            byte[] frame = new byte[8];
            frame[0] = 0x01; // Slave ID
            frame[1] = 0x06; // Custom function code (6)
            frame[2] = (byte)(address >> 8);
            frame[3] = (byte)(address & 0xFF);
            frame[4] = (byte)(_tueque >> 8);
            frame[5] = (byte)(_tueque & 0xFF);
            ushort crc = ModRTU_CRC(frame, 6);
            frame[6] = (byte)(crc & 0xFF);
            frame[7] = (byte)(crc >> 8);
            serialPort.Write(frame, 0, frame.Length);
            return true;
        }

        public byte[] SerialPort_DataReceived()
        {
            int bytes = serialPort.BytesToRead;
            byte[] buffer = new byte[bytes];
            serialPort.Read(buffer, 0, bytes);
            
            return buffer;
        }
        #region //CRC TABLE
        private ushort[] TABLE_CRC = new ushort[]
                                            {
                                            0X0000, 0XC0C1, 0XC181, 0X0140, 0XC301, 0X03C0, 0X0280, 0XC241,
                                            0XC601, 0X06C0, 0X0780, 0XC741, 0X0500, 0XC5C1, 0XC481, 0X0440,
                                            0XCC01, 0X0CC0, 0X0D80, 0XCD41, 0X0F00, 0XCFC1, 0XCE81, 0X0E40,
                                            0X0A00, 0XCAC1, 0XCB81, 0X0B40, 0XC901, 0X09C0, 0X0880, 0XC841,
                                            0XD801, 0X18C0, 0X1980, 0XD941, 0X1B00, 0XDBC1, 0XDA81, 0X1A40,
                                            0X1E00, 0XDEC1, 0XDF81, 0X1F40, 0XDD01, 0X1DC0, 0X1C80, 0XDC41,
                                            0X1400, 0XD4C1, 0XD581, 0X1540, 0XD701, 0X17C0, 0X1680, 0XD641,
                                            0XD201, 0X12C0, 0X1380, 0XD341, 0X1100, 0XD1C1, 0XD081, 0X1040,
                                            0XF001, 0X30C0, 0X3180, 0XF141, 0X3300, 0XF3C1, 0XF281, 0X3240,
                                            0X3600, 0XF6C1, 0XF781, 0X3740, 0XF501, 0X35C0, 0X3480, 0XF441,
                                            0X3C00, 0XFCC1, 0XFD81, 0X3D40, 0XFF01, 0X3FC0, 0X3E80, 0XFE41,
                                            0XFA01, 0X3AC0, 0X3B80, 0XFB41, 0X3900, 0XF9C1, 0XF881, 0X3840,
                                            0X2800, 0XE8C1, 0XE981, 0X2940, 0XEB01, 0X2BC0, 0X2A80, 0XEA41,
                                            0XEE01, 0X2EC0, 0X2F80, 0XEF41, 0X2D00, 0XEDC1, 0XEC81, 0X2C40,
                                            0XE401, 0X24C0, 0X2580, 0XE541, 0X2700, 0XE7C1, 0XE681, 0X2640,
                                            0X2200, 0XE2C1, 0XE381, 0X2340, 0XE101, 0X21C0, 0X2080, 0XE041,
                                            0XA001, 0X60C0, 0X6180, 0XA141, 0X6300, 0XA3C1, 0XA281, 0X6240,
                                            0X6600, 0XA6C1, 0XA781, 0X6740, 0XA501, 0X65C0, 0X6480, 0XA441,
                                            0X6C00, 0XACC1, 0XAD81, 0X6D40, 0XAF01, 0X6FC0, 0X6E80, 0XAE41,
                                            0XAA01, 0X6AC0, 0X6B80, 0XAB41, 0X6900, 0XA9C1, 0XA881, 0X6840,
                                            0X7800, 0XB8C1, 0XB981, 0X7940, 0XBB01, 0X7BC0, 0X7A80, 0XBA41,
                                            0XBE01, 0X7EC0, 0X7F80, 0XBF41, 0X7D00, 0XBDC1, 0XBC81, 0X7C40,
                                            0XB401, 0X74C0, 0X7580, 0XB541, 0X7700, 0XB7C1, 0XB681, 0X7640,
                                            0X7200, 0XB2C1, 0XB381, 0X7340, 0XB101, 0X71C0, 0X7080, 0XB041,
                                            0X5000, 0X90C1, 0X9181, 0X5140, 0X9301, 0X53C0, 0X5280, 0X9241,
                                            0X9601, 0X56C0, 0X5780, 0X9741, 0X5500, 0X95C1, 0X9481, 0X5440,
                                            0X9C01, 0X5CC0, 0X5D80, 0X9D41, 0X5F00, 0X9FC1, 0X9E81, 0X5E40,
                                            0X5A00, 0X9AC1, 0X9B81, 0X5B40, 0X9901, 0X59C0, 0X5880, 0X9841,
                                            0X8801, 0X48C0, 0X4980, 0X8941, 0X4B00, 0X8BC1, 0X8A81, 0X4A40,
                                            0X4E00, 0X8EC1, 0X8F81, 0X4F40, 0X8D01, 0X4DC0, 0X4C80, 0X8C41,
                                            0X4400, 0X84C1, 0X8581, 0X4540, 0X8701, 0X47C0, 0X4680, 0X8641,
                                            0X8201, 0X42C0, 0X4380, 0X8341, 0X4100, 0X81C1, 0X8081, 0X4040
                                            };
        #endregion
        public byte[] CalcCRC(byte[] Buffer, int length)
        {
            byte nTemp;
            ushort wCRCWord = 0xFFFF;
            for (int i = 0; i < length; i++)
            {
                nTemp = (byte)(wCRCWord ^ (Buffer[i]));
                wCRCWord >>= 8;
                wCRCWord ^= TABLE_CRC[nTemp];
            }
            return new byte[] { (byte)wCRCWord, (byte)(wCRCWord >> 8) };
        }
        private ushort ModRTU_CRC(byte[] buf, int len)
        {
            
            ushort crc = 0xFFFF;
            for (int pos = 0; pos < len; pos++)
            {
                crc ^= (ushort)buf[pos];
                for (int i = 8; i != 0; i--)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                        crc >>= 1;
                }
            }
            return crc;
        }
        #endregion

        #region //Modbus TCP
        public bool CreateMbTCP(string IPaddress, int port)
        {
            ////
            if (tcpClient != null)
            {
                tcpClient.Close();
                tcpClient = null;
            }
            tcpClient = new TcpClient();
            ////
            try
            {
                IAsyncResult result = tcpClient.BeginConnect(IPaddress, port, null, null);
                result.AsyncWaitHandle.WaitOne();
                result.AsyncWaitHandle.Close();
            }
            catch
            {
                return false;
            }
            ////
            if (mbTCP != null)
            {
                mbTCP.Dispose();
            }
            ////
            try
            {
                mbTCP = ModbusIpMaster.CreateIp(tcpClient);
                mbTCP.Transport.Retries = 1;
                mbTCP.Transport.ReadTimeout = 1500;
                mbTCP.Transport.RetryOnOldResponseThreshold = 1000;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public ushort[] MbTCPReadInputRegister()
        {
            try
            {
                return mbTCP.ReadInputRegistersAsync(3200, 14).Result;
            }
            catch
            {
                return null;
            }
        }
        public bool MbTCPWriteSigleRegister(ushort staradd, ushort num)
        {
            try
            {
                mbTCP.WriteSingleRegisterAsync(staradd, num);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
