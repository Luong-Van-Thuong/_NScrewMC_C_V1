using System;
using System.Threading;
using NMCMotionSDK;

namespace _NScrewMC_C_V1
{
    public class SystemIO
    {
        public byte[] m_uInBuffer = new byte[6];
        public byte[] m_uOutBuffer = new byte[6];

        public bool[] InputDataBits;
        public bool[] OutputDataBits;

        public static int Input_Origin = 1000;
        public static int Output_Origin = 2000;

        #region FUNCTION GET DATA IO
        public void OutPutOn(int number)
        {
            //check range output data
            if (number < Output_Origin || number >= IOMap.OUT["OUT_MAX"]) return;
            //Process output data
            OutputDataBits[number - Output_Origin] = true;
            return;
        }

        public void OutPutOff(int number)
        {
            if (number < Output_Origin || number >= IOMap.OUT["OUT_MAX"]) return;
            //Process output data
            OutputDataBits[number - Output_Origin] = false;
            return;
        }

        public bool IsOn(int number)
        {
            bool _value = false;
            if (MSystem.m_pDIO == null) return false;
            try
            {
                if (number >= (int)Input_Origin && number <= IOMap.IN["IN_MAX"])
                {
                    _value = InputDataBits[number - (int)Input_Origin];
                    return _value;
                }
                else if (number >= (int)Output_Origin && number <= IOMap.OUT["OUT_MAX"])
                {
                    _value = OutputDataBits[number - (int)Output_Origin];
                    return _value;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return false;
        }

        public bool IsOff(int number)
        {
            //if (MSystem.SIMULATION) return true;
            bool _value = false;
            if (MSystem.m_pDIO == null) return false;
            try
            {
                if (number >= (int)Input_Origin && number <= IOMap.IN["IN_MAX"])
                {
                    _value = !InputDataBits[number - (int)Input_Origin];
                }
                else if (number >= (int)Output_Origin && number <= IOMap.OUT["OUT_MAX"])
                {
                    _value = !OutputDataBits[number - (int)Output_Origin];
                }
                return _value;
            }
            catch (Exception ex)
            {
                ex.ToString();
                MSystem.MyMessagerBottom("IO Logic Error");
            }

            return false;
        }
        #endregion

        public SystemIO()
        {
            Initialize();

            IOMap.Initialize();
        }

        public void Initialize()
        {
            if (MSystem.SIMULATION) return;

            InputDataBits = new bool[8 * 6];
            OutputDataBits = new bool[8 * 6];

            Thread IOPOLL = new Thread(new ThreadStart(dorunStep));
            IOPOLL.Start();
        }

        public int _index = 0;
        public int m_RetryRead = 0;

        public void dorunStep()
        {
           //if (!MSystem.SIMULATION)
                ReadOutputData();
            while (true)
            {
                //if (!MSystem.SIMULATION)
                    ReadInputData();
                //if (!MSystem.SIMULATION)
                    WriteOutputData();
                Thread.Sleep(1);
            }
        }

        public void ReadOutputData()
        {
            NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_IO_READ(0, 1, 0, 0, 6, m_uOutBuffer);
            Array.Copy(GetDataBitfromByte(m_uOutBuffer), OutputDataBits, OutputDataBits.Length);
        }

        public void ReadInputData()
        {
            NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_IO_READ(0, 1, 1, 4, 6, m_uInBuffer);
            Array.Copy(GetDataBitfromByte(m_uInBuffer), InputDataBits, InputDataBits.Length);
        }

        public void WriteOutputData()
        {
            byte[] bytes = new byte[6];
            for (int i = 0; i < 8 * 6; i++)
            {
                if (OutputDataBits[i])
                    bytes[i / 8] |= (byte)(1 << (i % 8));
            }
            Array.Copy(bytes, m_uOutBuffer, bytes.Length);
            NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_IO_WRITE(0, 1, 0, 6, m_uOutBuffer);
        }

        public bool[] GetDataBitfromByte(byte[] bytes)
        {
            System.Collections.BitArray b = new System.Collections.BitArray(bytes);
            bool[] bitValues = new bool[b.Count];
            b.CopyTo(bitValues, 0);
            return bitValues;
        }
    }
}
