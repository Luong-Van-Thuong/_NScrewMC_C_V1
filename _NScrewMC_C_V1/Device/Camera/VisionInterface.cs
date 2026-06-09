using _NScrewMC_C_V1.Device.TCP;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _NScrewMC_C_V1
{
    public struct VesionResult
    {
        public double X;
        public double Y;
        public string Result;
    }
    public class VisionInterface : TcpClient
    {
        TcpClient Tcp = new TcpClient();
        public NetworkStream networkStream;

        public bool UpdateLeft = false;
        public bool UpdateRight = false;

        public readonly object LockPressMC = new object();
        public bool _bConnect = false;

        public MC_RESULT m_iReady = new MC_RESULT();
        public VS_RESULT[] m_VisionResult = { new VS_RESULT(), new VS_RESULT() };
        // For Vision
        //public MC_VISION_STATE m_VsState = new MC_VISION_STATE();
        public static Thread _mRun;
        public static readonly object LockTcpSocket = new object();


        public VesionResult[] VR = new VesionResult[2];
        public TCP_Server[] Server = new TCP_Server[2];
        public VisionInterface()
        {
            Server[(int)MSystem.eLEFT] = new TCP_Server((int)MSystem.eLEFT);
            Server[(int)MSystem.eRIGHT] = new TCP_Server((int)MSystem.eRIGHT);
            Server[(int)MSystem.eLEFT].TcpClientServerStart();
            Server[(int)MSystem.eRIGHT].TcpClientServerStart();
        }
        private void VariableClear(int UnitIndex)
        {
            VR[UnitIndex].X = 0.00;
            VR[UnitIndex].Y = 0.00;
            VR[UnitIndex].Result = string.Empty;
        }
        public void ModelChange(string model)
        {
            string sendPacket = $"FUNCTION=MDL_C,NO={model}";

            Server[0].clientItemList.SendStream(0, sendPacket, true);
        }
        public bool SendToSET_T(int UnitIndex, int ImgIndex, bool receiveFlag = false)
        {
            lock (LockTcpSocket)
            {
                VariableClear(UnitIndex);
                if (InforManager.Instance.m_bVisionType == true)
                {
                    if (Server[UnitIndex].clientItemList != null)
                    {
                        string sendPacket = $"FUNCTION=SET_T,CAM={(UnitIndex + 1).ToString()},P={(ImgIndex + 1).ToString()},PORT=900{UnitIndex.ToString()}";
                        return Server[UnitIndex].clientItemList.SendStream(UnitIndex, sendPacket, receiveFlag);
                    }
                }
                else
                {
                    if (Server[0].clientItemList != null)
                    {
                        string sendPacket = $"FUNCTION=SET_T,CAM={(UnitIndex + 1).ToString()},P={(ImgIndex + 1).ToString()}";
                        return Server[0].clientItemList.SendStream(UnitIndex, sendPacket, receiveFlag);
                    }
                }
                return false;
            }
        }
        public bool SendToLIGHT_CTRL(int UnitIndex, bool OnOff, bool receiveFlag = false)
        {
            lock (LockTcpSocket)
            {
                VariableClear(UnitIndex);
                string cmd = OnOff ? "LIGHT_ON" : "LIGHT_OFF";
                string sendPacket = $"FUNCTION={cmd},CAM={(UnitIndex + 1).ToString()}";
                if (InforManager.Instance.m_bVisionType == true)
                {
                    if (Server[UnitIndex].clientItemList != null)
                    {
                        return Server[UnitIndex].clientItemList.SendStream(UnitIndex, sendPacket, receiveFlag);
                    }
                }
                else
                {
                    if (Server[0].clientItemList != null)
                    {
                        return Server[0].clientItemList.SendStream(UnitIndex, sendPacket, receiveFlag);
                    }
                }
                return false;
            }
        }
    }
}
