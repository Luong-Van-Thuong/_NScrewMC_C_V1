using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _NScrewMC_C_V1.Device.TCP
{
    public class TCP_Server
    {
        private bool isServerThreadRun = false;
        private bool isClientThreadRun = false;
        private List<TcpClient> tcpClientList;
        public ItemClient clientItemList;
        public bool IsServerOpen = false;
        // Interface Client 속성
        public bool IsClient_Connected = false;
        private int SocketIndex = 0;
        private int SocketPort = 0;

        public TCP_Server(int _index)
        {
            tcpClientList = new List<TcpClient>();
            clientItemList = new ItemClient();
            SocketIndex = _index;
            if (_index == 0)
                SocketPort = 9000;
            if (_index == 1)
                SocketPort = 9001;
            if (_index == 2)
                SocketPort = 9002;
        }
        public void TcpClientServerStart()
        {
            //var myIni = new IniFile(Global.instance.IniSystemPath);
            isServerThreadRun = true;
            isClientThreadRun = true;

            Task.Factory.StartNew(AsyncServerListen);
        }
        public void TcpClientServerStop()
        {
            isServerThreadRun = false;
            isClientThreadRun = false;
            IsServerOpen = false;

            // 모든 클라이언트 연결 종료
            foreach (var client in tcpClientList.ToArray())
            {
                try
                {
                    client.Close();
                }
                catch { }
            }
            tcpClientList.Clear();

        }
        async Task AsyncServerListen()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, SocketPort);
            listener.Start();
            listener.Server.ReceiveTimeout = 200;
            IsServerOpen = true;

            while (isServerThreadRun)
            {
                try
                {
                    TcpClient tc = await listener.AcceptTcpClientAsync().ConfigureAwait(false);

                    // 클라이언트 IP 확인
                    IPEndPoint rmtIpep = tc.Client.RemoteEndPoint as IPEndPoint;
                    string clientIp = rmtIpep.Address.ToString();

                    if (isServerThreadRun)
                    {
                        Thread ClientThread = new Thread(() => workClient(tc));
                        ClientThread.Start();
                    }
                }
                catch (Exception e)
                {
                    Thread.Sleep(1);
                    //Global.ExceptionLog.Error($"{System.Reflection.MethodBase.GetCurrentMethod().Name} - {e}");
                }
                Thread.Sleep(1);
            }
        }
        async Task workClient(TcpClient tc)
        {
            IPEndPoint rmtIpep = tc.Client.RemoteEndPoint as IPEndPoint;                                    // IP주소 저장
                                                                                                            // 연결 시 상태 변경
            IsClient_Connected = true;
            //Global.Mlog.Info($"[{rmtIpep.Address.ToString()}-C]  {"{STATUS=CONNECT},"}");                   // Client 연결을 로그에 저장

            try
            {
                tcpClientList.Add(tc);                                                                      // TCP Client List에 넘겨받은 TCP Client 연결 추가
                ItemClient clientItem = new ItemClient(rmtIpep.Address.ToString());                         // 검사기 Item 생성
                //AddClientItem(clientItem);                                                                  // 검사기 Item 전역 관리 List에 추가
                clientItemList = clientItem;
                IsClient_Connected = true;

                NetworkStream stream = tc.GetStream();                                                      // 통신 스트림 가져옴
                clientItem.stream = stream;                                                                 // 통신 스트림을 검사기 Item에 할당

                stream.ReadTimeout = 3600000;
                while ((isClientThreadRun) && (tc.Connected) && (tc.GetStream() != null) && (clientItem.mIdString != ""))
                {
                    try
                    {
                        StringBuilder myCompleteMessage = new StringBuilder();
                        if (stream.CanRead)
                        {
                            byte[] myReadBuffer = new byte[1024];
                            int numberOfBytesRead = 0;

                            // Incoming message may be larger than the buffer size.
                            do
                            {
                                numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                                myCompleteMessage.AppendFormat("{0}", Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead));
                            }
                            while (stream.DataAvailable);

                            // 클라이언트가 연결을 종료함
                            if (numberOfBytesRead == 0)
                            {
                                IsClient_Connected = false;
                                break;
                            }
                        }

                        // 검사기로부터 받은 데이터가 있으면
                        string rcvString = myCompleteMessage.ToString();

                        // 프로토콜의 끝은 줄바꿈이므로 줄바꿈이 있는 경우 여러개의 메시지로 구분
                        if (!string.IsNullOrEmpty(rcvString) && rcvString.Contains("FUNCTION"))
                        {
                            // 메시지 마다 동작 한번씩 하도록 루프
                            //Global.Mlog.Info($"[{rmtIpep.Address.ToString()}-R] : {rcvString}");

                            string rtnMsg = clientItem.receiveMsgProc(rcvString);  // 메시지에 대한 처리함수 (처리 후 보내야 하는 데이터가 있으면 반환됨) ** 주요 동작 함수 **

                            if (string.IsNullOrEmpty(rtnMsg) == false)
                            {
                                IsClient_Connected = true;
                            }
                        }

                        stream.ReadTimeout = 3600000;

                    }
                    catch (IOException)
                    {
                        Thread.Sleep(1000);
                    }
                    catch (Exception ee)
                    {
                        Thread.Sleep(1);

                        //Global.ExceptionLog.Error($"{System.Reflection.MethodBase.GetCurrentMethod().Name} - {ee}");
                    }
                }

                if (stream != null)
                {
                    stream.Close();
                }
                tc.Close();

                // 연결 종료됨을 로그와 DB에 저장
                //Global.Mlog.Info($"[{rmtIpep.Address.ToString()}-C] : {"{STATUS=DISCONNECT},"}");
                tcpClientList.Remove(tc);
            }
            catch (Exception e)
            {
                Thread.Sleep(1);

                //Global.ExceptionLog.Error($"{System.Reflection.MethodBase.GetCurrentMethod().Name} - {e}");
            }
            finally
            {
                // 연결 해제 시 상태 변경
                IsClient_Connected = false;
            }
        }
    }
}
