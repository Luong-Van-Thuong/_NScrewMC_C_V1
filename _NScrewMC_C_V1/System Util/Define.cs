using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace _NScrewMC_C_V1
{
    public enum Inspection
    {
        TYPE = 3,
    }
    public enum MC_VISION_STATE
    {
        eNONE = 0,
        eSTOP = 1,
        eREADY = 2,
        eTEACH = 3,
        eRUN = 4,
        eUNKNOW
    }

    public enum GMES_TYPE
    {
        eFRONT,
        eFRONT_UB,
        eUB_FRONT,
        eMAX
    };
    public struct PointD
    {
        public double X { get; set; }
        public double Y { get; set; }

        // Constructor
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        // Method to calculate the distance between two PointD objects
        public static double Distance(PointD p1, PointD p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        // Overriding ToString for easy display
        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }

    public enum ProductState
    {
        eNONE = 0,
        eLOAD,
        eGUN,
        eDONE,
        eOUT,
        eMAX
    };

    public enum _NSC
    {
        eLEFT,
        eRIGHT,
        eMax
    };

    static class Constants
    {
        public const int left = 0;
        public const int right = 1;
    }

    public struct PointAxis
    {
        public string PointName;
        public double X;
        public double Y;
        public double Z;
        public double VisionZ;
    }

    public class PointScrew
    {
        public string P_Name;
        public PointAxis PScrew;          
        public bool Skip;
        public int Channel = 1;
        public int Light = 1;
        public int FasNumbers = 1;
        public bool Retry;
        public int status;
        public bool Vision;
    }


   

    public class ProductDetail
    {
        public string posName;
        public int m_dCountFail;
        public int m_dCountFailVision;
        //public string[,] DetailsFile1 = new string[5, 3]; // Details File List count 5th, 1:dete 2:toque 3:speed
        public Stack<string> DetailStack = new Stack<string>();
    }
    public enum PScrewMain
    {
        ePOS_MASTER_SCREW, 
        ePOS_MASTER_VISION,
        ePOS_READY,
        ePOS_TRASH,
        ePOS_PICKUP,
        ePOS_JIG_CLAMP,
        ePOS_Z_SAFETY,
        ePOS_CHECK_SLEVER,
        ePOS_BARCODE,
        ePOS_COVER,
        ePOS_MAX
    }
    public class sAxisInfo
    {
        public string AxisName;

        public double dOriginLimitTime;
        public double dLimitMinusValue;
        public double dLimitPluseValue;
        public double dOriginPoisionSet;
        public double dOriginOffset;
        public double dOriginSearchSpeed;
        public int iGain;
        public double dOriginSpeed;
        public double dJogFastSpeed;
        public double dJogMidAccDec;
        public double dJogMidSpeed;
        public double dJogSlowAccDec;
        public double dJogSlowSpeed;
        public double dAxisDec;
        public double dAxisAcc;
        public double dAxisMaxSpeed;
        public double dJogFastAccDec;
        public long MotionDir;
        public long OrgDir;
    }
    public struct UNITINFOR
    {
        public Head uMcIdx;
        public MC_RESULT uResult;
        public MC_RESULT uResult_L;
        public MC_RESULT uResult_M;
        public MC_RESULT uResult_R;
        public MC_NGTYPE uNGType;
        public MC_NGTYPE uNGType_L;
        public MC_NGTYPE uNGType_M;
        public MC_NGTYPE uNGType_R;
        public bool PressDone;
        public bool ScrewDone;
        public bool ScrewRun;
        public bool LoadDone;
        public bool VisionDone;
        public double m_dLeft;
        public double m_dRight;
        public string uPointNG;
        public string BCR_Front;
        public string BCR_BGlass;
        public string BCR_Left;
        public string BCR_Right;
        public string BCR_Data;
        public string uMessager;
        public string uMessager_L;
        public string uMessager_M;
        public string uMessager_R;
        public bool m_bLoadSet;
        public bool m_bLoadSet_L;
        public bool m_bLoadSet_M;
        public bool m_bLoadSet_R;
        public bool m_bBCR;
        public bool m_bBCR_L;
        public bool m_bBCR_M;
        public bool m_bBCR_R;
        public bool m_bVision_L;
        public bool m_bVision_M;
        public bool m_bVision_R;
        public bool m_bHRM_L;
        public bool m_bHRM_M;
        public bool m_bHRM_R;
        public bool m_bRetest;
        public bool m_bRetestFail;
        public int m_idxScrewJig;
    }
    public enum Head
    {
        eHEAD_LEFT = 0,
        eHEAD_RIGHT = 1,
        eHEAD_MAX = 2
    }
    public enum MCPRESS_STEP
    {
        ePRESS_MC_STEP_ANYTHING,
        ePRESS_MC_STEP_SET_RUN,
        ePRESS_MC_STEP_SET_RUNING,
        ePRESS_MC_STEP_GET_READY,
        ePRESS_MC_STEP_SET_PRESSURE,
        ePRESS_MC_STEP_SET_TIME,
        ePRESS_MC_STEP_GET_TIME_AND_PRESSURE,
        ePRESS_MC_STEP_SET_CALIBRATION,
        ePRESS_MC_STEP_MAX
    }
    public enum MC_RESULT
    {
        eNOT_YET = 0,
        ePASS = 1,
        eFAIL = 2,
        eRETRY = 3,
        eDONE = 4,
        eWORK = 5,
        eUNKNOW
    }
    public enum VS_RESULT
    {
        IDLE = 0,
        READY,
        START,
        PASS,
        FAIL,
        DONE
    }
    public enum BCR_RESULT
    {
        IDLE = 0,
        READY,
        START,
        PASS,
        FAIL,
        DONE
    }
    public enum MC_STATE
    {
        eMC_STATE_NOT_READY,
        eMC_STATE_READY,
        eMC_STATE_WORKING,
        eMC_STATE_COMPLETE,
        eMC_STATE_MAX
    }

    public enum MC_NGTYPE
    {
        eMC_NG_TYPE_NONE,
        eMC_NG_TYPE_CYL,
        eMC_NG_TYPE_ITV,
        eMC_NG_TYPE_PRE,
        eMC_NG_BCR,
        eMC_NG_GMES,
        eMC_PRESS_TILT_LEFT,
        eMC_PRESS_TILT_RIGHT,
        eMC_NG_REVERSE,
        eMC_NG_ALIGNMMENT,
        eMC_NG_SCREW,
        eMC_NG_TYPE_MAX
    }

    public enum Axis
    {
        AXIS_X1,
        AXIS_X2,
        AXIS_Y1,
        AXIS_Y2,
        AXIS_Z1,
        AXIS_Z2,
        eAXIS_MAX,
    }
    public enum IO
    {
        //Card 1
        IN_FRONT_OP_BOX_E_STOP_SW = 1000,//X00
        IN_FRONT_OP_BOX_START_SW            = 1001,//X01
        IN_FRONT_OP_BOX_STOP_SW             = 1002,//X02
        IN_FRONT_OP_BOX_RESET_SW            = 1003,//X03
        IN_DOOR_SENSOR                      = 1004,//X04
        IN_FRONT_LEFT_DOOR_DETECT           = 1005,//X05
        IN_FRONT_RIGHT_DOOR_DETECT          = 1006,//X06
        IN_REAR_LEFT_DOOR_DETECT            = 1007,//X07
        IN_REAR_RIGHT_DOOR_DETECT           = 1008,//X08
        IN_LEFT_LIGHT_CURTAIN_DETECT        = 1009,//X09
        IN_RIGHT_LIGHT_CURTAIN_DETECT       = 1010,//X0A
        IN_LEFT_SCREW_DRV_FASTEN_OK         = 1011,//X0B
        IN_LEFT_SCREW_DRV_READY             = 1012,//X0C
        IN_LEFT_SCREW_DRV_ALARM             = 1013,//X0D
        IN_LEFT_SCREW_DRV_MOTOR_RUN         = 1014,//X0E
        IN_RIGHT_SCREW_DRV_FASTEN_OK        = 1015,//X0F
        //Card 2
        IN_RIGHT_SCREW_DRV_READY            = 1016,//X10
        IN_RIGHT_SCREW_DRV_ALARM            = 1017,//X11
        IN_RIGHT_SCREW_DRV_MOTOR_RUN        = 1018,//X12
        IN_LEFT_DOOR_1_DETECT               = 1019,//X13
        IN_LEFT_JIG_DETECT_SET_SENS         = 1020,//X14// detect set left
        IN_LEFT_Z_SCREW_VACUM_SENS          = 1021,//X15
        IN_RIGHT_Z_SCREW_VACUM_SENS         = 1022,//X16
        IN_RIGHT_JIG_DETECT_SET_SENS        = 1023,//X17// detect set right
        IN_RIGHT_DOOR_2_DETECT              = 1024,//X18
        IN_LEFT_FEEDER_READY                = 1025,//X19
        IN_RIGHT_FEEDER_READY               = 1026,//X1A
        IN_SPARE_X01B                       = 1027,//X1B
        IN_LEFT_JIG_START_SW_L              = 1028,//X1C
        IN_LEFT_JIG_STOP_SW_L               = 1029,//X1D
        IN_RIGHT_JIG_START_SW_R             = 1030,//X1E
        IN_RIGHT_JIG_STOP_SW_R              = 1031,//X1F
        //Card 3
        IN_LEFT_JIG_DETECT_SENSOR           = 1032,//X20
        IN_LEFT_JIG_FWD_SENSOR              = 1033,//X21
        IN_LEFT_JIG_BWD_SENSOR              = 1034,//X22
        IN_LEFT_JIG_DOWN_SENSOR             = 1035,//X23
        IN_LEFT_JIG_CENTER_SENSOR           = 1036,//X24
        IN_LEFT_FEEDER_BLOT_DETECT          = 1037,//X25
        IN_RIGHT_FEEDER_BLOT_DETECT         = 1038,//X26
        IN_RIGHT_JIG_DETECT_SENSOR          = 1039,//X27
        IN_RIGHT_JIG_FWD_SENSOR             = 1040,//X28
        IN_RIGHT_JIG_BWD_SENSOR             = 1041,//X29
        IN_RIGHT_JIG_DOWN_SENSOR            = 1042,//X2A
        IN_RIGHT_JIG_CENTER_SENSOR          = 1043,//X2B
        IN_SPARE_X01C                       = 1044,//X2C
        IN_SPARE_X01D                       = 1045,//X2D
        IN_SPARE_X01E                       = 1046,//X2E
        IN_SPARE_X01F                       = 1047,//X2F
        IN_MAX                              = 1048,

        //CARD 1
        OUT_BUZZER                          = 2000,//Y00
        OUT_FRONT_OP_START_SW_LAMP          = 2001,//Y01
        OUT_FRONT_OP_STOP_SW_LAMP           = 2002,//Y02
        OUT_FRONT_OP_RESET_SW_LAMP          = 2003,//Y03
        OUT_TOWER_LAMP_GREEN                = 2004,//Y04
        OUT_TOWER_LAMP_RED                  = 2005,//Y05 OUT_TOWER_LAMP_RED
        OUT_TOWER_LAMP_YELLOW               = 2006,//Y06 
        OUT_SAFETY_PLC_RESET                = 2007,//Y07
        OUT_LEFT_CURTAIN_MUTE_1             = 2008,//Y08
        OUT_LEFT_CURTAIN_MUTE_2             = 2009,//Y09
        OUT_LEFT_DRV_TORQUE_SELECT_1        = 2010,//Y0A
        OUT_LEFT_DRV_TORQUE_SELECT_2        = 2011,//Y0B
        OUT_LEFT_DRV_TORQUE_SELECT_3        = 2012,//Y0C
        OUT_LEFT_SCREW_DRV_START            = 2013,//Y0D
        OUT_LEFT_SCREW_DRV_ALARM_RESET      = 2014,//Y0E
        OUT_LEFT_SCREW_DRV_FASTEN_LOOSEN    = 2015,//Y0F

        //CARD 2
        OUT_RIGHT_DRV_TORQUE_SELECT_1       = 2016,//Y10
        OUT_RIGHT_DRV_TORQUE_SELECT_2       = 2017,//Y11
        OUT_RIGHT_DRV_TORQUE_SELECT_3       = 2018,//Y12
        OUT_RIGHT_SCREW_DRV_START           = 2019,//Y13
        OUT_RIGHT_SCREW_DRV_ALARM_RESET     = 2020,//Y14
        OUT_RIGHT_SCREW_DRV_FASTEN_LOOSEN   = 2021,//Y15
        OUT_RIGHT_CURTAIN_MUTE_1            = 2022,//Y16
        OUT_RIGHT_CURTAIN_MUTE_2            = 2023,//Y17
        OUT_LEFT_Z_VACUM_ONOFF_SOL          = 2024,//Y18
        OUT_RIGHT_Z_VACUM_ONOFF_SOL         = 2025,//Y19
        OUT_LEFT_Z_VACCUM_BLOW_SOL          = 2026,//Y1A
        OUT_RIGHT_Z_VACCUM_BLOW_SOL         = 2027,//Y1B
        OUT_LEFT_JIG_START_SW_L             = 2028,//Y1C
        OUT_LEFT_JIG_STOP_SW_L              = 2029,//Y1D
        OUT_RIGHT_JIG_START_SW_R            = 2030,//Y1E
        OUT_RIGHT_JIG_STOP_SW_R             = 2031,//Y1F
        //CARD 3
        OUT_LEFT_JIG_FWD_D_SOL = 2032,//Y20
        OUT_LEFT_JIG_BWD_D_SOL              = 2033,//Y21
        OUT_LEFT_JIG_DOWN_S_SOL             = 2034,//Y22
        OUT_LEFT_JIG_CENTERING_S_SOL        = 2035,//Y23
        OUT_SPARE_Y024                      = 2036,//Y24
        OUT_SPARE_Y025                      = 2037,//Y25
        OUT_RIGHT_JIG_FWD_D_SOL             = 2038,//Y26
        OUT_RIGHT_JIG_BWD_D_SOL             = 2039,//Y27
        OUT_RIGHT_JIG_DOWN_S_SOL            = 2040,//Y28
        OUT_RIGHT_JIG_CENTERING_S_SOL       = 2041,//Y29
        OUT_SPARE_Y02A                      = 2042,//Y2A
        OUT_SPARE_Y02B                      = 2043,//Y2B
        OUT_RUN_LEFT_FEEDER                 = 2044,//Y2C
        OUT_LEFT_FEEDER_VACCUM_ON           = 2045,//Y2D
        OUT_RUN_RIGHT_FEEDER                = 2046,//Y2E //Run Feeder 
        OUT_RIGHT_FEEDER_VACCUM_ON          = 2047,//Y2F  //Run Feeder R
        OUT_MAX = 2048,

    }
    public enum IO_TABLET
    {
        //Card 1
        IN_FRONT_OP_BOX_E_STOP_SW = 1000,//X00
        IN_FRONT_OP_BOX_START_SW = 1001,//X01
        IN_FRONT_OP_BOX_STOP_SW = 1002,//X02
        IN_FRONT_OP_BOX_RESET_SW = 1003,//X03
        IN_DOOR_SENSOR = 1004,//X04
        IN_FRONT_LEFT_DOOR_DETECT = 1005,//X05
        IN_FRONT_RIGHT_DOOR_DETECT = 1006,//X06
        IN_REAR_LEFT_DOOR_DETECT = 1007,//X07
        IN_REAR_RIGHT_DOOR_DETECT = 1008,//X08
        IN_LEFT_LIGHT_CURTAIN_DETECT = 1009,//X09
        IN_RIGHT_LIGHT_CURTAIN_DETECT = 1010,//X0A
        IN_LEFT_SCREW_DRV_FASTEN_OK = 1011,//X0B
        IN_LEFT_SCREW_DRV_READY = 1012,//X0C
        IN_LEFT_SCREW_DRV_ALARM = 1013,//X0D
        IN_LEFT_SCREW_DRV_MOTOR_RUN = 1014,//X0E
        IN_RIGHT_SCREW_DRV_FASTEN_OK = 1015,//X0F
        //Card 2
        IN_RIGHT_SCREW_DRV_READY = 1016,//X10
        IN_RIGHT_SCREW_DRV_ALARM = 1017,//X11
        IN_RIGHT_SCREW_DRV_MOTOR_RUN = 1018,//X12
        IN_LEFT_DOOR_1_DETECT = 1019,//X13
        IN_LEFT_JIG_DETECT_SET_SENS = 1020,//X14// detect set left
        IN_LEFT_Z_SCREW_VACUM_SENS = 1021,//X15
        IN_RIGHT_Z_SCREW_VACUM_SENS = 1022,//X16
        IN_RIGHT_JIG_DETECT_SET_SENS = 1023,//X17// detect set right
        IN_RIGHT_DOOR_2_DETECT = 1024,//X18
        IN_LEFT_FEEDER_READY = 1025,//X19
        IN_RIGHT_FEEDER_READY = 1026,//X1A
        IN_SPARE_X01B = 1027,//X1B
        IN_LEFT_JIG_START_SW_L = 1028,//X1C
        IN_LEFT_JIG_STOP_SW_L = 1029,//X1D
        IN_RIGHT_JIG_START_SW_R = 1030,//X1E
        IN_RIGHT_JIG_STOP_SW_R = 1031,//X1F
        //Card 3
        IN_LEFT_JIG_COVER_DETECT_SS = 1032,//X20
        IN_LEFT_JIG_FRONT_COVER_FIX_SS_1_2 = 1033,//X21
        IN_LEFT_JIG_REAR_COVER_FIX_SS_3_4 = 1034,//X22
        IN_LEFT_JIG_LEFT_COVER_UP_SS = 1035,//X23
        IN_LEFT_JIG_LEFT_COVER_DOWN_SS = 1036,//X24
        IN_LEFT_JIG_RIGHT_COVER_UP_SS = 1037,//X25
        IN_LEFT_JIG_RIGHT_COVER_DOWN_SS = 1038,//X26
        IN_RIGHT_JIG_COVER_DETECT_SS = 1039,//X27
        IN_RIGHT_JIG_FRONT_COVER_FIX_SS_1_2 = 1040,//X28
        IN_RIGHT_JIG_REAR_COVER_FIX_SS_3_4 = 1041,//X29
        IN_RIGHT_JIG_LEFT_COVER_UP_SS = 1042,//X2A
        IN_RIGHT_JIG_LEFT_COVER_DOWN_SS = 1043,//X2B
        IN_RIGHT_JIG_RIGHT_COVER_UP_SS = 1044,//X2C
        IN_RIGHT_JIG_RIGHT_COVER_DOWN_SS = 1045,//X2D
        IN_LEFT_SLEEVE_DETECT = 1046,//X2E
        IN_RIGHT_SLEEVE_DETECT = 1047,//X2F
        IN_MAX = 1048,

        //CARD 1
        OUT_BUZZER = 2000,//Y00
        OUT_FRONT_OP_START_SW_LAMP = 2001,//Y01
        OUT_FRONT_OP_STOP_SW_LAMP = 2002,//Y02
        OUT_FRONT_OP_RESET_SW_LAMP = 2003,//Y03
        OUT_TOWER_LAMP_GREEN = 2004,//Y04
        OUT_TOWER_LAMP_RED = 2005,//Y05 OUT_TOWER_LAMP_RED
        OUT_TOWER_LAMP_YELLOW = 2006,//Y06 
        OUT_SAFETY_PLC_RESET = 2007,//Y07
        OUT_LEFT_CURTAIN_MUTE_1 = 2008,//Y08
        OUT_LEFT_CURTAIN_MUTE_2 = 2009,//Y09
        OUT_LEFT_DRV_TORQUE_SELECT_1 = 2010,//Y0A
        OUT_LEFT_DRV_TORQUE_SELECT_2 = 2011,//Y0B
        OUT_LEFT_DRV_TORQUE_SELECT_3 = 2012,//Y0C
        OUT_LEFT_SCREW_DRV_START = 2013,//Y0D
        OUT_LEFT_SCREW_DRV_ALARM_RESET = 2014,//Y0E
        OUT_LEFT_SCREW_DRV_FASTEN_LOOSEN = 2015,//Y0F

        //CARD 2
        OUT_RIGHT_DRV_TORQUE_SELECT_1 = 2016,//Y10
        OUT_RIGHT_DRV_TORQUE_SELECT_2 = 2017,//Y11
        OUT_RIGHT_DRV_TORQUE_SELECT_3 = 2018,//Y12
        OUT_RIGHT_SCREW_DRV_START = 2019,//Y13
        OUT_RIGHT_SCREW_DRV_ALARM_RESET = 2020,//Y14
        OUT_RIGHT_SCREW_DRV_FASTEN_LOOSEN = 2021,//Y15
        OUT_RIGHT_CURTAIN_MUTE_1 = 2022,//Y16
        OUT_RIGHT_CURTAIN_MUTE_2 = 2023,//Y17
        OUT_LEFT_Z_VACUM_ONOFF_SOL = 2024,//Y18
        OUT_RIGHT_Z_VACUM_ONOFF_SOL = 2025,//Y19
        OUT_LEFT_Z_VACCUM_BLOW_SOL = 2026,//Y1A
        OUT_RIGHT_Z_VACCUM_BLOW_SOL = 2027,//Y1B
        OUT_LEFT_JIG_START_SW_L = 2028,//Y1C
        OUT_LEFT_JIG_STOP_SW_L = 2029,//Y1D
        OUT_RIGHT_JIG_START_SW_R = 2030,//Y1E
        OUT_RIGHT_JIG_STOP_SW_R = 2031,//Y1F
        //Card 3
        OUT_LIGHT_ON = 2032,//Y20
        OUT_LEFT_JIG_COVER_FIX_SOL = 2033,//Y21
        OUT_LEFT_JIG_COVER_UNFIX_SOL = 2034,//Y22
        OUT_LEFT_JIG_LR_COVER_UP_SOL = 2035,//Y23
        OUT_LEFT_JIG_LR_COVER_DOWN_SOL = 2036,//Y24
        OUT_SPARE_Y025 = 2037,//Y25
        OUT_SPARE_Y026 = 2038,//Y26
        OUT_SPARE_Y027 = 2039,//Y27
        OUT_RIGHT_JIG_COVER_FIX_SOL = 2040,//Y28
        OUT_RIGHT_JIG_COVER_UNFIX_SOL = 2041,//Y29
        OUT_RIGHT_JIG_LR_COVER_UP_SOL = 2042,//Y2A
        OUT_RIGHT_JIG_LR_COVER_DOWN_SOL = 2043,//Y2B
        OUT_RUN_LEFT_FEEDER = 2044,//Y2C
        OUT_LEFT_FEEDER_VACCUM_ON = 2045,//Y2D
        OUT_RUN_RIGHT_FEEDER = 2046,//Y2E //Run Feeder 
        OUT_RIGHT_FEEDER_VACCUM_ON = 2047,//Y2F  //Run Feeder R
        OUT_MAX = 2048

    }
    public enum IO_VST
    {
        // card 1
        IN_FRONT_OP_BOX_E_STOP_SW = 1000,//X00
        IN_FRONT_OP_BOX_START_SW = 1001,//X01
        IN_FRONT_OP_BOX_STOP_SW = 1002,//X02
        IN_FRONT_OP_BOX_RESET_SW = 1003,//X03
        IN_DOOR_SENSOR = 1004,//X04
        IN_FRONT_LEFT_DOOR_DETECT = 1005,//X05
        IN_FRONT_RIGHT_DOOR_DETECT = 1006,//X06
        IN_REAR_LEFT_DOOR_DETECT = 1007,//X07
        IN_REAR_RIGHT_DOOR_DETECT = 1008,//X08
        IN_LEFT_LIGHT_CURTAIN_DETECT = 1009,//X09
        IN_LEFT_CURTAIN_DETECT_2 = 1010,//X0A
        IN_LEFT_SCREW_DRV_FASTEN_OK = 1011,//X0B
        IN_LEFT_SCREW_DRV_READY = 1012,//X0C
        IN_LEFT_SCREW_DRV_ALARM = 1013,//X0D
        IN_LEFT_SCREW_DRV_MOTOR_RUN = 1014,//X0E
        IN_RIGHT_SCREW_DRV_FASTEN_OK = 1015,//X0F

        //Card 2
        IN_RIGHT_SCREW_DRV_READY = 1016,//X10
        IN_RIGHT_SCREW_DRV_ALARM = 1017,//X11
        IN_RIGHT_SCREW_DRV_MOTOR_RUN = 1018,//X12
        IN_LEFT_DOOR_1_DETECT = 1019,//X13
        IN_LEFT_JIG_DETECT_SET_SENS = 1020,//X14// detect set left
        IN_LEFT_Z_SCREW_VACUM_SENS = 1021,//X15
        IN_RIGHT_Z_SCREW_VACUM_SENS = 1022,//X16
        IN_RIGHT_JIG_DETECT_SET_SENS = 1023,//X17// detect set right
        IN_RIGHT_DOOR_2_DETECT = 1024,//X18
        IN_LEFT_FEEDER_READY = 1025,//X19
        IN_RIGHT_FEEDER_READY = 1026,//X1A
        IN_SPARE_X01B = 1027,//X1B
        IN_LEFT_JIG_START_SW_L = 1028,//X1C
        IN_LEFT_JIG_STOP_SW_L = 1029,//X1D
        IN_RIGHT_JIG_START_SW_R = 1030,//X1E
        IN_RIGHT_JIG_STOP_SW_R = 1031,//X1F
                                      // Card 3
        IN_RIGHT_LIGHT_CURTAIN_DETECT = 1032,//X20
        IN_RIGHT_CURTAIN_DETECT_2 = 1033,//X21
        IN_LEFT_FEEDER_ALARM = 1034,//X22
        IN_RIGHT_FEEDER_ALARM = 1035,//X23
        IN_SPARE_X024 = 1036,//X24
        IN_SPARE_X025 = 1037,//X25
        IN_SPARE_X026 = 1038,//X26
        IN_SPARE_X027 = 1039,//X27
        IN_SPARE_X028 = 1040,//X28
        IN_SPARE_X029 = 1041,//X29
        IN_SPARE_X02A = 1042,//X2A
        IN_SPARE_X02B = 1043,//X2B
        IN_SPARE_X02C = 1044,//X2C
        IN_SPARE_X02D = 1045,//X2D
        IN_LEFT_SLEEVE_DETECT = 1046,//X2E
        IN_RIGHT_SLEEVE_DETECT = 1047,//X2F
        IN_MAX = 1048,

        //CARD 1
        OUT_BUZZER = 2000,//Y00
        OUT_FRONT_OP_START_SW_LAMP = 2001,//Y01
        OUT_FRONT_OP_STOP_SW_LAMP = 2002,//Y02
        OUT_FRONT_OP_RESET_SW_LAMP = 2003,//Y03
        OUT_TOWER_LAMP_GREEN = 2004,//Y04
        OUT_TOWER_LAMP_RED = 2005,//Y05 OUT_TOWER_LAMP_RED
        OUT_TOWER_LAMP_YELLOW = 2006,//Y06 
        OUT_SAFETY_PLC_RESET = 2007,//Y07
        OUT_LEFT_CURTAIN_MUTE_1 = 2008,//Y08
        OUT_LEFT_CURTAIN_MUTE_2 = 2009,//Y09
        OUT_LEFT_DRV_TORQUE_SELECT_1 = 2010,//Y0A
        OUT_LEFT_DRV_TORQUE_SELECT_2 = 2011,//Y0B
        OUT_LEFT_DRV_TORQUE_SELECT_3 = 2012,//Y0C
        OUT_LEFT_SCREW_DRV_START = 2013,//Y0D
        OUT_LEFT_SCREW_DRV_ALARM_RESET = 2014,//Y0E
        OUT_LEFT_SCREW_DRV_FASTEN_LOOSEN = 2015,//Y0F

        //CARD 2
        OUT_RIGHT_DRV_TORQUE_SELECT_1 = 2016,//Y10
        OUT_RIGHT_DRV_TORQUE_SELECT_2 = 2017,//Y11
        OUT_RIGHT_DRV_TORQUE_SELECT_3 = 2018,//Y12
        OUT_RIGHT_SCREW_DRV_START = 2019,//Y13
        OUT_RIGHT_SCREW_DRV_ALARM_RESET = 2020,//Y14
        OUT_RIGHT_SCREW_DRV_FASTEN_LOOSEN = 2021,//Y15
        OUT_RIGHT_CURTAIN_MUTE_1 = 2022,//Y16
        OUT_RIGHT_CURTAIN_MUTE_2 = 2023,//Y17
        OUT_LEFT_Z_VACUM_ONOFF_SOL = 2024,//Y18
        OUT_RIGHT_Z_VACUM_ONOFF_SOL = 2025,//Y19
        OUT_LEFT_Z_VACCUM_BLOW_SOL = 2026,//Y1A
        OUT_RIGHT_Z_VACCUM_BLOW_SOL = 2027,//Y1B
        OUT_LEFT_JIG_START_SW_L = 2028,//Y1C
        OUT_LEFT_JIG_STOP_SW_L = 2029,//Y1D
        OUT_RIGHT_JIG_START_SW_R = 2030,//Y1E
        OUT_RIGHT_JIG_STOP_SW_R = 2031,//Y1F

        //CARD 3
        OUT_LIGHT_ON = 2032,//Y20
        OUT_SUCTION_ON = 2033,//Y21
        OUT_LEFT_JIG_DOWN_S_SOL = 2034,//Y22
        OUT_LEFT_JIG_CENTERING_S_SOL = 2035,//Y23
        OUT_SPARE_Y024 = 2036,//Y24
        OUT_SPARE_Y025 = 2037,//Y25
        OUT_RIGHT_JIG_FWD_D_SOL = 2038,//Y26
        OUT_RIGHT_JIG_BWD_D_SOL = 2039,//Y27
        OUT_RIGHT_JIG_DOWN_S_SOL = 2040,//Y28
        OUT_RIGHT_JIG_CENTERING_S_SOL = 2041,//Y29
        OUT_SPARE_Y02A = 2042,//Y2A
        OUT_SPARE_Y02B = 2043,//Y2B
        OUT_RUN_LEFT_FEEDER = 2044,//Y2C
        OUT_LEFT_FEEDER_VACCUM_ON = 2045,//Y2D
        OUT_RUN_RIGHT_FEEDER = 2046,//Y2E //Run Feeder 
        OUT_RIGHT_FEEDER_VACCUM_ON = 2047,//Y2F  //Run Feeder R
        OUT_MAX = 2048
    }
    public enum IO_H8
    {
        IN_FRONT_OP_BOX_E_STOP_SW = 1000,//X00
        IN_FRONT_OP_BOX_START_SW = 1001,//X01
        IN_FRONT_OP_BOX_STOP_SW = 1002,//X02
        IN_FRONT_OP_BOX_RESET_SW = 1003,//X03
        IN_DOOR_SENSOR = 1004,//X04
        IN_FRONT_LEFT_DOOR_DETECT = 1005,//X05
        IN_FRONT_RIGHT_DOOR_DETECT = 1006,//X06
        IN_REAR_LEFT_DOOR_DETECT = 1007,//X07
        IN_REAR_RIGHT_DOOR_DETECT = 1008,//X08
        IN_LEFT_LIGHT_CURTAIN_DETECT = 1009,//X09
        IN_LEFT_CURTAIN_DETECT_2 = 1010,//X0A
        IN_LEFT_SCREW_DRV_FASTEN_OK = 1011,//X0B
        IN_LEFT_SCREW_DRV_READY = 1012,//X0C
        IN_LEFT_SCREW_DRV_ALARM = 1013,//X0D
        IN_LEFT_SCREW_DRV_MOTOR_RUN = 1014,//X0E
        IN_RIGHT_SCREW_DRV_FASTEN_OK = 1015,//X0F
                                            //Card 2
        IN_RIGHT_SCREW_DRV_READY = 1016,//X10
        IN_RIGHT_SCREW_DRV_ALARM = 1017,//X11
        IN_RIGHT_SCREW_DRV_MOTOR_RUN = 1018,//X12
        IN_LEFT_DOOR_1_DETECT = 1019,//X13
        IN_LEFT_JIG_DETECT_SET_SENS = 1020,//X14
        IN_LEFT_Z_SCREW_VACUM_SENS = 1021,//X15
        IN_RIGHT_Z_SCREW_VACUM_SENS = 1022,//X16
        IN_RIGHT_JIG_DETECT_SET_SENS = 1023,//X17
        IN_RIGHT_DOOR_2_DETECT = 1024,//X18
        IN_LEFT_FEEDER_READY = 1025,//X19
        IN_RIGHT_FEEDER_READY = 1026,//X1A
        IN_SPARE_X01B = 1027,//X1B
        IN_LEFT_JIG_START_SW_L = 1028,//X1C
        IN_LEFT_JIG_STOP_SW_L = 1029,//X1D
        IN_RIGHT_JIG_START_SW_R = 1030,//X1E
        IN_RIGHT_JIG_STOP_SW_R = 1031,//X1F
        // Card 3
        IN_LEFT_JIG_DETECT_SENSOR = 1032,//X20
        IN_LEFT_JIG_FWD_SENSOR = 1033,//X21
        IN_LEFT_JIG_BWD_SENSOR = 1034,//X22
        IN_LEFT_JIG_DOWN_SENSOR = 1035,//X23
        IN_LEFT_JIG_CENTER_SENSOR = 1036,//X24
        IN_LEFT_FEEDER_BLOT_DETECT = 1037,//X25
        IN_RIGHT_FEEDER_BLOT_DETECT = 1038,//X26
        IN_RIGHT_JIG_DETECT_SENSOR = 1039,//X27
        IN_RIGHT_JIG_FWD_SENSOR = 1040,//X28
        IN_RIGHT_JIG_BWD_SENSOR = 1041,//X29
        IN_RIGHT_JIG_DOWN_SENSOR = 1042,//X2A
        IN_RIGHT_JIG_CENTER_SENSOR = 1043,//X2B
        IN_RIGHT_LIGHT_CURTAIN_DETECT = 1044,//X2C
        IN_RIGHT_CURTAIN_DETECT_2 = 1045,//X2D
        IN_SPARE_X02E = 1046,//X2E
        IN_SPARE_X02F = 1047,//X2F
        IN_MAX = 1048,



        //CARD 1
        OUT_BUZZER = 2000,//Y00
        OUT_FRONT_OP_START_SW_LAMP = 2001,//Y01
        OUT_FRONT_OP_STOP_SW_LAMP = 2002,//Y02
        OUT_FRONT_OP_RESET_SW_LAMP = 2003,//Y03
        OUT_TOWER_LAMP_GREEN = 2004,//Y04
        OUT_TOWER_LAMP_RED = 2005,//Y05 
        OUT_TOWER_LAMP_YELLOW = 2006,//Y06 
        OUT_SAFETY_PLC_RESET = 2007,//Y07
        OUT_LEFT_CURTAIN_MUTE_1 = 2008,//Y08
        OUT_LEFT_CURTAIN_MUTE_2 = 2009,//Y09
        OUT_LEFT_DRV_TORQUE_SELECT_1 = 2010,//Y0A
        OUT_LEFT_DRV_TORQUE_SELECT_2 = 2011,//Y0B
        OUT_LEFT_DRV_TORQUE_SELECT_3 = 2012,//Y0C
        OUT_LEFT_SCREW_DRV_START = 2013,//Y0D
        OUT_LEFT_SCREW_DRV_ALARM_RESET = 2014,//Y0E
        OUT_LEFT_SCREW_DRV_FASTEN_LOOSEN = 2015,//Y0F
                                                //CARD 2
        OUT_RIGHT_DRV_TORQUE_SELECT_1 = 2016,//Y10
        OUT_RIGHT_DRV_TORQUE_SELECT_2 = 2017,//Y11
        OUT_RIGHT_DRV_TORQUE_SELECT_3 = 2018,//Y12
        OUT_RIGHT_SCREW_DRV_START = 2019,//Y13
        OUT_RIGHT_SCREW_DRV_ALARM_RESET = 2020,//Y14
        OUT_RIGHT_SCREW_DRV_FASTEN_LOOSEN = 2021,//Y15
        OUT_RIGHT_CURTAIN_MUTE_1 = 2022,//Y16
        OUT_RIGHT_CURTAIN_MUTE_2 = 2023,//Y17
        OUT_LEFT_Z_VACUM_ONOFF_SOL = 2024,//Y18
        OUT_RIGHT_Z_VACUM_ONOFF_SOL = 2025,//Y19
        OUT_LEFT_Z_VACCUM_BLOW_SOL = 2026,//Y1A
        OUT_RIGHT_Z_VACCUM_BLOW_SOL = 2027,//Y1B
        OUT_LEFT_JIG_START_SW_L = 2028,//Y1C
        OUT_LEFT_JIG_STOP_SW_L = 2029,//Y1D
        OUT_RIGHT_JIG_START_SW_R = 2030,//Y1E
        OUT_RIGHT_JIG_STOP_SW_R = 2031,//Y1F
                                       //CARD 3
        OUT_LEFT_JIG_FWD_D_SOL = 2032,//Y20
        OUT_LEFT_JIG_BWD_D_SOL = 2033,//Y21
        OUT_LEFT_JIG_DOWN_S_SOL = 2034,//Y22
        OUT_LEFT_JIG_CENTERING_S_SOL = 2035,//Y23
        OUT_SPARE_Y024 = 2036,//Y24
        OUT_SPARE_Y025 = 2037,//Y25
        OUT_RIGHT_JIG_FWD_D_SOL = 2038,//Y26
        OUT_RIGHT_JIG_BWD_D_SOL = 2039,//Y27
        OUT_RIGHT_JIG_DOWN_S_SOL = 2040,//Y28
        OUT_RIGHT_JIG_CENTERING_S_SOL = 2041,//Y29
        OUT_SPARE_Y02A = 2042,//Y2A
        OUT_SPARE_Y02B = 2043,//Y2B
        OUT_SPARE_Y02C = 2044,//Y2C
        OUT_SPARE_Y02D = 2045,//Y2D
        OUT_SPARE_Y02E = 2046,//Y2E
        OUT_SPARE_Y02F = 2047,//Y2F 
        OUT_MAX = 2048

    }
    public static class IOMap
    {
        public static Dictionary<string, int> IN;
        public static Dictionary<string, int> OUT;
        public static Dictionary<int, string> IN_Name;
        public static Dictionary<int, string> OUT_Name;
        public static void Initialize()
        {
            IN = new  Dictionary<string, int>();
            OUT = new Dictionary<string, int>();
            IN_Name = new Dictionary<int, string>();
            OUT_Name = new Dictionary<int, string>();
            if (InforManager.Instance.InspectionType == "Mobile")
            {
                foreach (IO io in Enum.GetValues(typeof(IO)))
                {
                    string ioName = io.ToString();
                    int ioValue = (int)io;
                    if (ioName.StartsWith("IN_"))
                    {
                        IN.Add(ioName, ioValue);
                        IN_Name.Add(ioValue,ioName);
                    }
                    else if (ioName.StartsWith("OUT_"))
                    {
                        OUT.Add(ioName, ioValue);
                        OUT_Name.Add(ioValue, ioName);
                    }
                }
            }
            else if (InforManager.Instance.InspectionType == "Tablet"
                || InforManager.Instance.InspectionType == "Tablet Normal")
            {
                foreach (IO_TABLET io in Enum.GetValues(typeof(IO_TABLET)))
                {
                    string ioName = io.ToString();
                    int ioValue = (int)io;
                    if (ioName.StartsWith("IN_"))
                    {
                        IN.Add(ioName, ioValue);
                        IN_Name.Add(ioValue, ioName);
                    }
                    else if (ioName.StartsWith("OUT_"))
                    {
                        OUT.Add(ioName, ioValue);
                        OUT_Name.Add(ioValue, ioName);
                    }
                }
            }
            else if (InforManager.Instance.InspectionType == "VST")
            {
                foreach (IO_VST io in Enum.GetValues(typeof(IO_VST)))
                {
                    string ioName = io.ToString();
                    int ioValue = (int)io;
                    if (ioName.StartsWith("IN_"))
                    {
                        IN.Add(ioName, ioValue);
                        IN_Name.Add(ioValue, ioName);
                    }
                    else if (ioName.StartsWith("OUT_"))
                    {
                        OUT.Add(ioName, ioValue);
                        OUT_Name.Add(ioValue, ioName);
                    }
                }
            }
            else if (InforManager.Instance.InspectionType == "H8")
            {
                foreach (IO_H8 io in Enum.GetValues(typeof(IO_H8)))
                {
                    string ioName = io.ToString();
                    int ioValue = (int)io;
                    if (ioName.StartsWith("IN_"))
                    {
                        IN.Add(ioName, ioValue);
                        IN_Name.Add(ioValue, ioName);
                    }
                    else if (ioName.StartsWith("OUT_"))
                    {
                        OUT.Add(ioName, ioValue);
                        OUT_Name.Add(ioValue, ioName);
                    }
                }
            }
        }
    }
    public enum Unit
    {
        eUNIT_SCREW_1,
        eUNIT_SCREW_2,
        eUNIT_MAX
    }
    public enum msgIcon
    {
        Question = 0,
        Infor = 1,
        Error = 2,
        Success = 3,
        eMessgMax = 4
    }
    public enum msgButton
    {
        YESNO = 0,
        OK = 1,
        eBtMax = 2
    }
    public enum StatusRun
    {
        RUN = 0,
        STOP = 1,
        ERROR = 2,
        OPRATOR_CALL = 3
    }
    public enum ModeRun
    {
        Auto = 0,
        DryRun = 1,
        eModeMax = 2
    }
    public enum NumViewMain
    {
        eViewAuto = 1,
        eViewTeach = 2,
        eViewData = 3,
        eViewLog = 4,
        eViewHiden = 5,
        eViewExit = 6,
        eViewVision = 10,
        eViewTeachLoad = 11,
        eviewTeadUnload = 12,
        eviewTeadScrew = 13,
        eViewMax = 7
    }
    public enum InitResult
    {
        UNIT_INIT_SUCCESS = 0,
        UINT_INIT_FALSE = 1,
        UNIT_INIT_MAX = 2
    }
    public enum BCR_MODE
    {
        eBCR_LEFT,
        eBCR_RIGHT,
        eBCR_BOTH,
        eMC_MAX
    }
    public enum JIG_STATUS
    {
        eNONE,
        eREADY,
        eRUN,
        ePASS,
        eFAIL,
        eMC_MAX
    }
    
}