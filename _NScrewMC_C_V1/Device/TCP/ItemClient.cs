using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _NScrewMC_C_V1.Device.TCP
{
    public class ItemClient
    {
        enum Cmd_Pase
        {
            CMD_Name,
            CMD_Value,
            LR_Name,
            LR_Value,
            X_Name,
            X_Value,
            Y_Name,
            Y_Value,
            Result_Name,
            Result_Value
        }
        /// <summary>
        /// 통신 프로토콜에서 사용하는 명령 종류
        /// </summary>

        public string mIdString;            // 식별문자열 (IP주소를 주로 사용)

        /// <summary>
        /// TCP 연결의 통신 스트림을 관리
        /// </summary>
        private NetworkStream mStream;
        public NetworkStream stream
        {
            set
            {
                mStream = value;
            }
        }
        /// <summary>
        /// 현재 Item의 통신을 닫음
        /// </summary>
        public void Close()
        {
            try
            {
                mIdString = "";
                if (mStream != null)
                {
                    mStream.Close();
                }
            }
            catch (Exception e)
            {
                //Global.ExceptionLog.Error($"{System.Reflection.MethodBase.GetCurrentMethod().Name} - {e}");
            }
        }

        /// <summary>
        /// Client Item 생성자
        /// </summary>
        /// <param name="strId">IP주소를 주로 사용</param>
        public ItemClient(string strId = "")
        {
            mIdString = strId;
        }

        /// <summary>
        /// 통신 스트림을 통하여 메시지를 전송하는 함수
        /// </summary>
        /// <param name="sendString"></param>
        public bool SendStream(int Index, string messageBody, bool ReturnFlag = false)
        {
            if (mStream != null)
            {
                byte[] sendBuff = Encoding.UTF8.GetBytes(messageBody);
                mStream.Write(sendBuff, 0, sendBuff.Length);

                // 전송 데이터를 디버깅 로그에 저장
                //Global.Mlog.Info($"[{mIdString}-S]  {messageBody.Replace('\n', ' ').Trim()}");
                bool result = true;
                if (ReturnFlag)
                {
                    result = ReceiveWait(Index);
                }

                return result;
            }
            return false; // 스트림이 null인 경우 실패
        }
        private bool ReceiveWait(int Index)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            while (true)
            {
                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    return false; // 3초 이상 대기 시 실패
                }
                if (MSystem.vinterface.VR[Index].Result == "PASS")
                {
                    return true; // 성공적으로 응답을 받음
                }
                else if (MSystem.vinterface.VR[Index].Result == "FAIL")
                        return false; // 응답이 실패로 설정됨
                Thread.Sleep(2);
            }
        }

        //------------------------------------------------
        /// <summary>
        /// Client로부터 받은 메시지를 처리하는 함수 ** 젤로 중요 **
        /// </summary>
        /// <param name="strProtocol">받은 메시지 문자열</param>
        /// <returns>응답 데이터</returns>
        public string receiveMsgProc(string strProtocol)
        {
            utilProtocol revData = new utilProtocol(strProtocol);

            strProtocol = strProtocol.Trim(); // 문자열 양쪽의 공백 제거
            if (string.IsNullOrEmpty(strProtocol))
                return "FAIL"; // FAIL 반환
            string[] Splits = strProtocol.Split('=', ',');
            string values = string.Empty;
            if (Splits.Length > 0)
            {
                try
                {
                    if (Splits[1] == "SET_T")
                    {
                        if (Splits[3] == "1")
                        {
                            // Open CV
                            if(InforManager.Instance.m_bVisionType == false)
                            {
                                Splits[8] = Splits[8].Trim();
                                if (Splits[8] == "RESULT_PASS")
                                {
                                    MSystem.vinterface.VR[0].X = (double)Math.Round(Convert.ToDouble(Splits[5]), 2);
                                    MSystem.vinterface.VR[0].Y = (double)Math.Round(Convert.ToDouble(Splits[7]), 2);
                                    MSystem.vinterface.VR[0].Result = "PASS";
                                }
                                else
                                {
                                    MSystem.vinterface.VR[0].X = 0.00;
                                    MSystem.vinterface.VR[0].Y = 0.00;
                                    MSystem.vinterface.VR[0].Result = "FAIL";
                                }
                            }
                            else if (InforManager.Instance.m_bVisionType == true)
                            {
                                Splits[10] = Splits[10].Trim();
                                if (Splits[10] == "RESULT_PASS")
                                {
                                    MSystem.vinterface.VR[0].X = (double)Math.Round(Convert.ToDouble(Splits[7]), 2);
                                    MSystem.vinterface.VR[0].Y = (double)Math.Round(Convert.ToDouble(Splits[9]), 2);
                                    MSystem.vinterface.VR[0].Result = "PASS";
                                }
                                else
                                {
                                    MSystem.vinterface.VR[0].X = 0.00;
                                    MSystem.vinterface.VR[0].Y = 0.00;
                                    MSystem.vinterface.VR[0].Result = "FAIL";
                                }
                            }
                        }
                        if (Splits[3] == "2")
                        {
                            // 2 point
                            if (InforManager.Instance.m_bVisionType == false)
                            {
                                Splits[8] = Splits[8].Trim();
                                if (Splits[8] == "RESULT_PASS")
                                {
                                    MSystem.vinterface.VR[1].X = (double)Math.Round(Convert.ToDouble(Splits[5]), 2);
                                    MSystem.vinterface.VR[1].Y = (double)Math.Round(Convert.ToDouble(Splits[7]), 2);
                                    MSystem.vinterface.VR[1].Result = "PASS";
                                }
                                else
                                {
                                    MSystem.vinterface.VR[1].X = 0.00;
                                    MSystem.vinterface.VR[1].Y = 0.00;
                                    MSystem.vinterface.VR[1].Result = "FAIL";
                                }
                            }
                            else if (InforManager.Instance.m_bVisionType == true)// all point
                            {
                                Splits[10] = Splits[10].Trim();
                                if (Splits[10] == "RESULT_PASS")
                                {
                                    MSystem.vinterface.VR[1].X = (double)Math.Round(Convert.ToDouble(Splits[7]), 2);
                                    MSystem.vinterface.VR[1].Y = (double)Math.Round(Convert.ToDouble(Splits[9]), 2);
                                    MSystem.vinterface.VR[1].Result = "PASS";
                                }
                                else
                                {
                                    MSystem.vinterface.VR[1].X = 0.00;
                                    MSystem.vinterface.VR[1].Y = 0.00;
                                    MSystem.vinterface.VR[1].Result = "FAIL";
                                }
                            }
                        }
                    }
                    else if (Splits[(int)Cmd_Pase.CMD_Value] == "MDL_C")
                    {
                    }
                    else if (Splits[(int)Cmd_Pase.CMD_Value] == "TOTAL_POINTS")
                    {
                        string message = $"FUNCTION=TOTAL_POINTS,NUMBER={InforTeaching.Instance.P_Screw[0].Count}";
                        byte[] sendBuff = Encoding.UTF8.GetBytes(message);
                        mStream.Write(sendBuff, 0, sendBuff.Length);
                    }
                    else if (Splits[(int)Cmd_Pase.CMD_Value] == "LIGHT_ON"
                        || Splits[(int)Cmd_Pase.CMD_Value] == "LIGHT_OFF")
                    {
                        Splits[5] = Splits[5].Trim();
                        int index = Convert.ToInt32(Splits[3]) - 1;
                        MSystem.vinterface.VR[index].Result = Splits[5];
                    }
                    else if (Splits[(int)Cmd_Pase.CMD_Value] == "PM")
                    {
                        int unit = 0;
                        if (Splits[3] == "1")
                            unit = 0;
                        else
                            unit = 1;
                        Splits[5] = Splits[5].Trim();
                        int point = Convert.ToInt32(Splits[5]) - 1;
                        string sendPacket = String.Empty;

                        try
                        {
                            if (MSystem.m_pTrsScrew[unit].VisionPointMove(point) == true)
                            {
                                sendPacket = $"FUNCTION_RPY=PM,CAM={Splits[3]},POINT={(point + 1)},RESULT=PASS";
                            }
                            else
                            {
                                sendPacket = $"FUNCTION_RPY=PM,CAM={Splits[3]},POINT={(point + 1)},RESULT=FAIL";
                            }
                            SendStream(unit, sendPacket);
                        }
                        catch
                        {
                            sendPacket = $"FUNCTION_RPY=PM,CAM={Splits[3]},POINT={(point + 1)},RESULT=FAIL";
                            SendStream(unit, sendPacket);
                        }
                    }
                }
                catch (Exception e)
                {
                    return "FAIL"; // FAIL 반환
                }
            }

            return "FAIL";
        }
    }
}
