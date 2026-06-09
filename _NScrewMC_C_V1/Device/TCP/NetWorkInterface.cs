using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _NScrewMC_C_V1
{
    public class NetWorkInterface : TcpClient
    {
        TcpClient Tcp = new TcpClient();
        public NetworkStream networkStream;
        public NetWorkInterface() {
            try {

                Tcp.Connect("127.0.0.1", 5000);
                Thread.Sleep(100);
                networkStream = Tcp.GetStream();
            }
            catch (Exception ex) {
                ex.ToString();
            }
        }
        public async Task<int>  WriteAsyncData(string _strData) {

            if (Tcp == null || networkStream == null) return -1;
            if (Tcp.Connected)
            {
                try
                {
                    byte[] testByte = ASCIIEncoding.ASCII.GetBytes(_strData);
                    await networkStream.WriteAsync(testByte, 0, testByte.Length);
                    await networkStream.FlushAsync();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    return -1;
                }
            }
            return 0;
        }
        byte[] DataReceive = new byte[4096]; 
        public async Task<string> ReadAsyncData() {
            
            if (Tcp == null || networkStream == null) return null;
            string strRecive = null;
            try
            {
                //Start Process Get Data for Network interface when Data avaiable
                if (networkStream.DataAvailable)
                {
                    var iResult = await networkStream.ReadAsync(DataReceive, 0, DataReceive.Length);
                    byte[] IsReceiveByte = new byte[iResult];
                    Array.Copy(DataReceive, 0, IsReceiveByte, 0, iResult);
                    strRecive = ASCIIEncoding.ASCII.GetString(IsReceiveByte);
                }
                //End process get data from GMES
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
            //End process receive data
            return strRecive;
        }
        public static bool PingHost(string hostUri, int portNumber)
        {
            try
            {
                using (var client = new TcpClient(hostUri, portNumber))
                    return true;
            }
            catch (SocketException ex)
            {
                ex.ToString();
                return false;
            }
        }
        private TcpState GetState(TcpClient tcpClient)
        {
            try {
                var foo = IPGlobalProperties.GetIPGlobalProperties()
                 .GetActiveTcpConnections()
                 .SingleOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint)
                                    && x.RemoteEndPoint.Equals(tcpClient.Client.RemoteEndPoint)
                 );
                return foo != null ? foo.State : TcpState.Unknown;
            }

            catch (Exception ex)
            {
                ex.ToString();
                return TcpState.Unknown;
            }
           
        }
        public bool checkTCpClose(TcpClient _tcp) {
            try
            {
                TcpState _check = GetState(_tcp);
                if (_check == TcpState.CloseWait || _check == TcpState.Unknown)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                ex.ToString();
                return false;
            }
        }
        public async void dorunStep() {
            while (true) {
                try {
                    if (Tcp != null && Tcp.Connected)
                    {
                        var strData = await ReadAsyncData();
                        if (strData != "" && strData != null) {
                            //await WriteAsyncData(strData); 
                        }
                        if (checkTCpClose(Tcp)) {
                            networkStream.Close();
                            Tcp.Close();
                        }  
                    }
                    #region //Reconnection
                    else
                    {
                        if (Tcp != null) {
                            Tcp.Close();
                            Tcp.Dispose();
                        }
                        if (networkStream != null) {
                            networkStream.Close();
                            networkStream.Dispose();
                        }

                        MSystem.MyMessagerBottom("Reconnect to GMES");
                        Tcp = new TcpClient();
                        await Tcp.ConnectAsync("127.0.0.1", 5000);
                        await Task.Delay(2000);

                        if (Tcp.Connected) {
                            networkStream = Tcp.GetStream();
                            MSystem.MyMessagerBottom("Connect to GMES OK");
                        }
                    }
                    #endregion
                }
                catch (Exception ex) {
                    ex.ToString();
                }
                await Task.Delay(15);
            }
        } 
    }
}
