using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _NScrewMC_C_V1
{
    public class SerialHelper : SerialPort
    {
        byte[] CMD_GET_INPUT = { 0x02, 0x46, 0x31, 0x03 };
        public int TimeRetry = 0;
        public TimerDelay LifeTime = new TimerDelay();
        private int timeOut;
        public bool FlagRecive = false;
        public int _index = 0;

        public SemaphoreSlim ReadLock = new SemaphoreSlim(1, 1);
        public byte[] buffer = new byte[100];
        public byte[] ReceivedByte = new byte[100];
        public byte[] buffTmp = new byte[100];
        public double _ReadOut = 0.1;
        private string NameDevice = "";

        public SerialHelper(string Port, int TimeOut, string InstanceName)
        {
            try
            {
                NameDevice = InstanceName;
                PortName = Port;
                BaudRate = 115200;
                Parity = Parity.None;
                DataBits = 8;
                StopBits = StopBits.One;
                Handshake = Handshake.None;

                ReadTimeout = 5;
                WriteTimeout = 10;
                Open();
                timeOut = TimeOut;
                string str = "Connect to " + NameDevice + " " + Port.ToString() + " OK";
            }
            catch (Exception ex)
            {
                ex.ToString();
                string str = "Connect to " + NameDevice + " " + Port.ToString() + " NG";
            }
        }
        public SerialHelper(string Port, int baudRate, int TimeOut, string InstanceName)
        {
            try
            {
                NameDevice = InstanceName;
                PortName = Port;
                BaudRate = baudRate;
                Parity = Parity.None;
                DataBits = 8;
                StopBits = StopBits.One;
                Handshake = Handshake.None;

                ReadTimeout  = 5000;
                WriteTimeout = 5000;
                Open();
                timeOut = TimeOut;

                string str = "Connect to " + NameDevice + " " + Port.ToString() + " OK";
            }
            catch (Exception ex)
            {
                string str = "Connect to " + NameDevice + " " + Port.ToString() + " NG" + "\r\n" + ex.ToString();

            }
        }
        public SerialHelper(string Port, int baudRate, int WriteTimeOut, int ReadTimeOut, string InstanceName)
        {
            try
            {
                NameDevice = InstanceName;
                PortName = Port;
                BaudRate = baudRate;
                Parity = Parity.None;
                DataBits = 8;
                StopBits = StopBits.One;
                Handshake = Handshake.None;

                ReadTimeout = WriteTimeOut;
                WriteTimeout = ReadTimeOut;
                Open();
                string str = "Connect to " + NameDevice + " " + Port.ToString() + " OK";

            }
            catch (Exception ex)
            {
                ex.ToString();
                ex.ToString();
                string str = "Connect to " + NameDevice + " " + Port.ToString() + " NG";
            }
        }
        public async Task<byte[]> ReadUCTAsync()
        {
            try
            {
                if (IsOpen)
                {
                    int TimeOut = 100;
                    var receiveTask = this.BaseStream.ReadAsync(buffer, 0, buffer.Length);
                    var isReceived = await Task.WhenAny(receiveTask, Task.Delay(TimeOut)) == receiveTask;
                    if (isReceived)
                    {
                        int Count = receiveTask.Result;
                        if (Count > 4)
                        {
                            ReceivedByte = new byte[Count];
                            Array.Copy(buffer, 0, ReceivedByte, 0, Count);
                            return ReceivedByte;
                        }
                    }
                }
                return null;
            }
            catch (IOException ex)
            {
                string s = ex.Message;
                return null;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return null;
            }
        }
        public async Task<byte[]> ReadAsyncInput(double _TimeOut)
        {
            //Initial Variable
            if (_TimeOut == 0) _TimeOut = _ReadOut;
            var ReciveCount = 0;
            ////////////////////////////////////////////////////
            try
            {
                if (IsOpen)
                {
                    if (BytesToRead != 0)
                    {

                        LifeTime.SetTimer();
                        while (true)
                        {
                            //check for sure data exits
                            if (BytesToRead != 0)
                            {
                                ReciveCount = await BaseStream.ReadAsync(buffer, _index, 1);
                                _index += ReciveCount;
                                if (buffer[_index - 1] == 0x03)
                                {
                                    if (_index != 12) return null;
                                    ReceivedByte = new byte[_index];
                                    Array.Copy(buffer, _index - 12, ReceivedByte, 0, 12);
                                    _index = 0;
                                    return ReceivedByte;
                                }
                            }
                            else
                            {//Time Out for exit
                                await Task.Delay(1);
                                TimeRetry++;
                                if (TimeRetry > _TimeOut * 1000)
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    Open();
                    await Task.Delay(100);
                }
                return null;
            }
            catch (IOException error)
            {
                string s = error.Message;
                return null;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return null;
            }
            finally
            {
            }
        }
        public async Task WriteAsync(byte[] Bytes)
        {
            await ReadLock.WaitAsync();
            try
            {
                if (IsOpen)
                {
                    await BaseStream.WriteAsync(Bytes, 0, Bytes.Length);
                    await BaseStream.FlushAsync();
                    await Task.Delay(1);
                }
            }
            catch (IOException error)
            {
                string s = error.Message;
                ReadLock.Release();

            }
            catch (Exception ex)
            {
                string s = ex.Message;
                ReadLock.Release();
            }
            ReadLock.Release();
            return;
        }
        public async Task WriteAsync(string _Data, int _leng)
        {
            byte[] Bytes = Encoding.ASCII.GetBytes(_Data);
            await ReadLock.WaitAsync();
            try
            {
                if (IsOpen)
                {
                    await BaseStream.WriteAsync(Bytes, 0, _leng);
                    await BaseStream.FlushAsync();
                    Thread.Sleep(1);
                    ReadLock.Release();
                    return;
                }
            }
            catch (IOException error)
            {
                string s = error.Message;
                ReadLock.Release();
                return;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                ReadLock.Release();
                return;
            }
            ReadLock.Release();
        }
        public void CloseDevice()
        {
            Close();
            Dispose();
        }

        #region //CRC TABLE
        private static ushort[] TABLE_CRCVALUE = new ushort[]
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
        public static byte[] CalcCRC(byte[] Buffer)
        {
            byte nTemp;
            ushort wCRCWord = 0xFFFF;
            for (int i = 0; i < Buffer.Length; i++)
            {
                nTemp = (byte)(wCRCWord ^ (Buffer[i]));
                wCRCWord >>= 8;
                wCRCWord ^= TABLE_CRCVALUE[nTemp];
            }
            return new byte[] { (byte)wCRCWord, (byte)(wCRCWord >> 8) };
        }
        void Hex2Dec(byte[] hex, ref byte[] dec)
        {
            byte ipos = 0;
            byte dpos = 0;
            int temp = 0;
            for (int i = 0; i < 5; i++)
            {
                if (hex[ipos + 1] >= 'A' && hex[ipos + 1] <= 'F')
                {
                    temp = (((hex[ipos + 1] - 'A') + 10) << 4) & 0xF0;
                    dec[dpos] = (byte)temp;
                    temp = 0;
                }
                else
                {
                    temp = ((hex[ipos + 1] - '0') << 4) & 0xF0;
                    dec[dpos] = (byte)(temp);
                    temp = 0;
                }
                ipos++;
                if (hex[ipos + 1] >= 'A' && hex[ipos + 1] <= 'F')
                {
                    temp = ((hex[ipos + 1] - 'A') + 10) & 0x0F;
                    dec[dpos] |= (byte)temp;
                    temp = 0;
                }
                else
                {
                    temp = (hex[ipos + 1] - '0') & 0x0F;
                    dec[dpos] |= (byte)temp;
                    temp = 0;
                }
                ipos++;
                dpos++;
            }
            return;
        }
        byte DecToHex(byte p_intValue)
        {
            if (p_intValue >= 0x0A && p_intValue <= 0x0F) //"A"&B
            {
                p_intValue = (byte)(p_intValue + 0x37);
            }
            else
            {
                p_intValue = (byte)(p_intValue + 0x30);
            }
            return p_intValue;
        }
        public async Task<byte[]> ReadInput(double _TimeOut)
        {

            /*Send command read Data*/
            if (LifeTime.MoreThan(_TimeOut))
            {
                await WriteAsync(CMD_GET_INPUT);
                LifeTime.SetTimer();
                _index = 0;
            }

            /*Wait for Read Input Data*/
            var DataRecive = await ReadAsyncInput(_TimeOut);
            byte[] tmpData1 = new byte[5];
            byte[] tmpData2 = new byte[5];
            //check Data OKey
            if (DataRecive == null) return null;
            if (DataRecive[0] != 0x02) return null;
            if (DataRecive[11] != 0x03) return null;

            //Start Process convert Data Input from Board 
            Hex2Dec(DataRecive, ref tmpData1);
            byte csum = (byte)(tmpData1[0] + tmpData1[1] + tmpData1[2] + tmpData1[3]);
            if (csum != tmpData1[4])
            {
                return null;
            }
            tmpData2[0] = tmpData1[3];
            tmpData2[1] = tmpData1[2];
            tmpData2[2] = tmpData1[1];
            tmpData2[3] = tmpData1[0];
            tmpData2[4] = tmpData1[4];
            return tmpData2;
        }

        public async Task WriteOutput(byte[] src)
        {

            byte[] req = new byte[14];
            int pos = 0;
            req[pos++] = 0x02;
            req[pos++] = 0x46;
            req[pos++] = 0x30;

            byte csum = (byte)(src[0] + src[1] + src[2] + src[3]);

            req[pos++] = DecToHex((byte)((byte)(src[3] >> 4) & 0x0F));
            req[pos++] = DecToHex((byte)(src[3] & 0x0F));
            req[pos++] = DecToHex((byte)((byte)(src[2] >> 4) & 0x0F));
            req[pos++] = DecToHex((byte)(src[2] & 0x0F));
            req[pos++] = DecToHex((byte)((byte)(src[1] >> 4) & 0x0F));
            req[pos++] = DecToHex((byte)(src[1] & 0x0F));
            req[pos++] = DecToHex((byte)((byte)(src[0] >> 4) & 0x0F));
            req[pos++] = DecToHex((byte)(src[0] & 0x0F));

            req[pos++] = DecToHex((byte)((byte)(csum >> 4) & 0x0F));
            req[pos++] = DecToHex((byte)(csum & 0x0F));
            req[pos++] = 0x03;

            await WriteAsync(req);

            return;
        }
        public async Task<byte[]> ReadAsyncPressMC(double _TimeOut)
        {
            //Initial Variable
            var ReciveCount = 0;
            ////////////////////////////////////////////////////
            try
            {
                if (IsOpen)
                {
                    while (true)
                    {
                        if (BytesToRead != 0)  //check for sure data exits
                        {
                            ReciveCount = await BaseStream.ReadAsync(buffer, _index, 1);
                            _index += ReciveCount;
                            if (buffer[_index - 1] == 0x03 || buffer[_index - 1] == 0x0D)
                            {
                                ReceivedByte = new byte[_index];
                                Array.Copy(buffer, 0, ReceivedByte, 0, _index);
                                _index = 0;
                                return ReceivedByte;

                            }
                        }
                        else
                        {
                            await Task.Delay(100);
                            TimeRetry++;
                            if (TimeRetry > _TimeOut * 10)
                                break;
                        }
                    }
                }
                else
                {
                    Open();
                    await Task.Delay(100);
                }
                return null;
            }
            catch (IOException error)
            {
                string s = error.Message;
                return null;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return null;
            }
            finally
            {
            }
        }
        public async Task<byte[]> WriteAsyncPressMC(byte[] Bytes, int _Timout)
        {
            await ReadLock.WaitAsync();
            try
            {
                ReceivedByte = null;
                if (IsOpen)
                {
                    await BaseStream.WriteAsync(Bytes, 0, Bytes.Length);
                    await BaseStream.FlushAsync();
                    await Task.Delay(1);
                    int ReciveCount = 0;
                    while (true)
                    {
                        //check for sure data exits
                        if (BytesToRead != 0)
                        {
                            ReciveCount = await BaseStream.ReadAsync(buffer, _index, 1);
                            _index += ReciveCount;
                            if (buffer[_index - 1] == 0x0D)
                            {
                                ReceivedByte = new byte[_index];
                                Array.Copy(buffer, 0, ReceivedByte, 0, _index);
                                _index = 0;
                                break;
                            }
                            else
                            {//Time Out for exit
                                await Task.Delay(1);
                                TimeRetry++;
                                if (TimeRetry > _Timout)
                                    break;
                            }
                        }
                    }
                }
            }
            catch (IOException error)
            {
                string s = error.Message;
                ReceivedByte = null;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                ReceivedByte = null;
            }
            ReadLock.Release();
            return ReceivedByte;
        }
    }
}
