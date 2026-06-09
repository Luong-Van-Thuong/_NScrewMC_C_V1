using NMCMotionSDK;
using System;

namespace _NScrewMC_C_V1
{
    public partial class AxisManager : MSystem
    {
        public readonly object LockAxisManager = new object();
        public bool _connected = false;
        public bool m_bBdInit;

        public MMCEtherCATAxis[] m_pMmcEtherCatAxis = new MMCEtherCATAxis[(int)Axis.eAXIS_MAX];

        public string[] strAxisName = {
            "Left X",
            "Right X",
            "Left Y",
            "Right Y",
            "Left Z",
            "Right Z",
        };

        double[] scale_FAS = {
            10000.0 / 10,  
            10000.0 / 10,
            10000.0 / 10,
            10000.0 / 10,
            20000.0 / 20,
            20000.0 / 20,
        };

        int[] prior_FAS = {
            1,
            1,
            1,
            1,
            0,
            0,
	    };

        public AxisManager()
        {

        }

        ~AxisManager()
        {

        }

        public void Initialize()
        {
            try
            {
                for (ushort i = 0; i < eAXIS_MAX; i++)
                {
                    m_pMmcEtherCatAxis[i] = new MMCEtherCATAxis(strAxisName[i], i, scale_FAS[i], prior_FAS[i]);
                    m_pMmcEtherCatAxis[i].PowerOn();
                }
            }
            catch
            {
                FMsgMemo.Show($"Some Servo Connection Fail.\r\nProgram Will Exit...", "Error", msgButton.OK, msgIcon.Error);
                //NumDisplay = (int)NumViewMain.eViewExit;
            }
        }
    }
}
