using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ActUtlTypeLib;

namespace Program
{
    #region enum
    #region Font
    public enum e_Font
    {
        Tahoma,
        맑은_고딕
    }
    #endregion

    #region Parameter
    public enum e_Parameter
    {
        System,
        DB,
        LoadCell,
        Monitor,
        ETC,
        // ==================================== PLC_Interface
        PLC_InterfaceB_IN,
        PLC_InterfaceB_OUT,
        PLC_InterfaceF_Alarm,
        PLC_InterfaceR_IN,
        PLC_InterfaceR_OUT,
        // ==================================================
        Recipe
    }

    public enum e_DF_System
    {
        No,
        Name,
        PLC_No,
        LastFileName,
        적용,
        Range
    }
    public enum e_DF_DB
    {
        No,
        Type,
        Server,
        Port,
        Database,
        Uid,
        Pwd,
        Timeout
    }
    public enum e_DF_LoadCell
    {
        No,
        USE,
        Scale_Ux,
        Scale_Uy,
        Scale_Lx,
        Scale_Ly,
        Logging
    }
    public enum e_DF_Monitor
    {
        No,
        Location_X,
        Location_Y,
        Size_X,
        Size_Y
    }
    public enum e_DF_ETC
    {
        No,
        Name,
        Value0,
        Value1,
        Value2,
        Value3,
        Value4,
        Value5,
        Value6,
        Value7,
        Value8,
        Value9
    }

    public enum e_DF_PLC_InterfaceB_IN
    {
        No,
        Address,
        Description
    }
    public enum e_DF_PLC_InterfaceB_OUT
    {
        No,
        Address,
        Description
    }
    public enum e_DF_PLC_InterfaceF_Alarm
    {
        No,
        Address,
        Description
    }
    public enum e_DF_PLC_InterfaceR_IN
    {
        No,
        Address,
        Count,
        Description
    }
    public enum e_DF_PLC_InterfaceR_OUT
    {
        No,
        Address,
        Count,
        Description
    }

    public enum e_DF_Recipe
    {
        No,
        Type,
        Name,
        MIN,
        MAX,
        Data,
        Unit,
        Address,
        Count,
        Format,
        C
    }
    #endregion

    #region DB
    public enum e_DBTableList
    {
        Alarm_MC,
        Result_MC
    }

    public enum e_DBAlarm_MC
    {
        StartTime,
        EndTime,
        Code,
        Name
    }
    public enum e_DBResult_MC
    {
        Time,
        Number,
        Position,
        PPLoad,
        PLoad,
        MLoad,
        MMLoad,
        No_1_LoadValue,
        No_2_LoadValue,
        No_3_LoadValue,
        No_4_LoadValue,
        No_1_Ux,
        No_1_Uy,
        No_1_Lx,
        No_1_Ly,
        No_2_Ux,
        No_2_Uy,
        No_2_Lx,
        No_2_Ly,
        No_3_Ux,
        No_3_Uy,
        No_3_Lx,
        No_3_Ly,
        No_4_Ux,
        No_4_Uy,
        No_4_Lx,
        No_4_Ly,
        No_1_Result,
        No_2_Result,
        No_3_Result,
        No_4_Result,
        No_1_Temp,
        No_2_Temp,
        No_3_Temp,
        No_4_Temp
    }
    #endregion

    #region Device
    public enum e_Device
    {
        PLC
    }

    #region PLC
    public enum e_PLC_R_IN
    {
          _1_RAM_1_1_HEIGHT,
          _2_ORI_1_1_TURN,
          _3_RAM_1_2_HEIGHT,
          _4_ORI_1_2_TURN,
          _5_RAM_2_1_HEIGHT,
          _6_ORI_2_1_TURN,
          _7_RAM_2_2_HEIGHT,
          _8_ORI_2_2_TURN_,
          _9_RAM_1_1_SPRING_TEMP,
         _10_RAM_1_2_SPRING_TEMP,
         _11_RAM_2_1_SPRING_TEMP,
         _12_RAM_2_2_SPRING_TEMP,
         _13_RAM_1_1_OFFSET,
         _14_RAM_1_2_OFFSET,
         _15_RAM_2_1_OFFSET,
         _16_RAM_2_2_OFFSET,
         _17_AUTO_MODE,
         _18_MANUAL_MODE,
         _19_RAM_1_TEST_COMPLETED,
         _20_RAM_2_TEST_COMPLETED,
         _21_RAM_1_1_TEST_RESULT,
         _22_RAM_1_1_RESULT_B_LOAD,
         _23_RAM_1_1_RESULT_B_Rx,
         _24_RAM_1_1_RESULT_B_Ry,
         _25_RAM_1_1_RESULT_A_LOAD,
         _26_RAM_1_1_RESULT_A_Rx,
         _27_RAM_1_1_RESULT_A_Ry,
         _28_RAM_1_1_TEMPERATURE,
         _29_RAM_1_2_TEST_RESULT,
         _30_RAM_1_2_RESULT_B_LOAD,
         _31_RAM_1_2_RESULT_B_Rx,
         _32_RAM_1_2_RESULT_B_Ry,
         _33_RAM_1_2_RESULT_A_LOAD,
         _34_RAM_1_2_RESULT_A_Rx,
         _35_RAM_1_2_RESULT_A_Ry,
         _36_RAM_1_2_TEMPERATURE,
         _37_RAM_2_1_TEST_RESULT,
         _38_RAM_2_1_RESULT_B_LOAD,
         _39_RAM_2_1_RESULT_B_Rx,
         _40_RAM_2_1_RESULT_B_Ry,
         _41_RAM_2_1_RESULT_A_LOAD,
         _42_RAM_2_1_RESULT_A_Rx,
         _43_RAM_2_1_RESULT_A_Ry,
         _44_RAM_2_1_TEMPERATURE,
         _45_RAM_2_2_TEST_RESULT,
         _46_RAM_2_2_RESULT_B_LOAD,
         _47_RAM_2_2_RESULT_B_Rx,
         _48_RAM_2_2_RESULT_B_Ry,
         _49_RAM_2_2_RESULT_A_LOAD,
         _50_RAM_2_2_RESULT_A_Rx,
         _51_RAM_2_2_RESULT_A_Ry,
         _52_RAM_2_2_TEMPERATURE,
         _53_,
         _54_1_1_TARE_REQUEST_IN_AUTO_MODE,
         _55_1_2_TARE_REQUEST_IN_AUTO_MODE,
         _56_2_1_TARE_REQUEST_IN_AUTO_MODE,
         _57_2_2_TARE_REQUEST_IN_AUTO_MODE,
         _58_DOOR_OPEN_MONITOR,
         _59_,
         _60_RAM_1_1_ACTUAL_B_LOAD,
         _61_RAM_1_1_ACTUAL_B_Rx,
         _62_RAM_1_1_ACTUAL_B_Ry,
         _63_RAM_1_1_ACTUAL_A_LOAD,
         _64_RAM_1_1_ACTUAL_A_Rx,
         _65_RAM_1_1_ACTUAL_A_Ry,
         _66_RAM_1_2_ACTUAL_B_LOAD,
         _67_RAM_1_2_ACTUAL_B_Rx,
         _68_RAM_1_2_ACTUAL_B_Ry,
         _69_RAM_1_2_ACTUAL_A_LOAD,
         _70_RAM_1_2_ACTUAL_A_Rx,
         _71_RAM_1_2_ACTUAL_A_Ry,
         _72_RAM_2_1_ACTUAL_B_LOAD,
         _73_RAM_2_1_ACTUAL_B_Rx,
         _74_RAM_2_1_ACTUAL_B_Ry,
         _75_RAM_2_1_ACTUAL_A_LOAD,
         _76_RAM_2_1_ACTUAL_A_Rx,
         _77_RAM_2_1_ACTUAL_A_Ry,
         _78_RAM_2_2_ACTUAL_B_LOAD,
         _79_RAM_2_2_ACTUAL_B_Rx,
         _80_RAM_2_2_ACTUAL_B_Ry,
         _81_RAM_2_2_ACTUAL_A_LOAD,
         _82_RAM_2_2_ACTUAL_A_Rx,
         _83_RAM_2_2_ACTUAL_A_Ry,
         _84_,
         _85_시험_높이,
         _86_PP_하중값,
         _87_P__하중값,
         _88_M__하중값,
         _89_MM_하중값,
         _90_A_Rx_M_오차값,
         _91_A_Rx_P_오차값,
         _92_A_Ry_M_오차값,
         _93_A_Ry_P_오차값,
         _94_B_Rx_M_오차값,
         _95_B_Rx_P_오차값,
         _96_B_Ry_M_오차값,
         _97_B_Ry_P_오차값,
         _98_,
         _99_,
        _100_,
        _101_,
        _102_,
        _103_,
        _104_,
        _105_,
        _106_,
        _107_,
        _108_Upper_Sheet,
        _109_Lower_Sheet,
        _110_
    }
    public enum e_PLC_R_OUT
    {
          _1_LOAD_DATA_TRANSFER_COMPLETED,
          _2_,
          _3_,
          _4_,
          _5_,
          _6_RAM_1_1_ACTUAL_B_LOAD,
          _7_RAM_1_1_ACTUAL_B_Rx,
          _8_RAM_1_1_ACTUAL_B_Ry,
          _9_RAM_1_1_ACTUAL_A_LOAD,
         _10_RAM_1_1_ACTUAL_A_Rx,
         _11_RAM_1_1_ACTUAL_A_Ry,
         _12_RAM_1_2_ACTUAL_B_LOAD,
         _13_RAM_1_2_ACTUAL_B_Rx,
         _14_RAM_1_2_ACTUAL_B_Ry,
         _15_RAM_1_2_ACTUAL_A_LOAD,
         _16_RAM_1_2_ACTUAL_A_Rx,
         _17_RAM_1_2_ACTUAL_A_Ry,
         _18_RAM_2_1_ACTUAL_B_LOAD,
         _19_RAM_2_1_ACTUAL_B_Rx,
         _20_RAM_2_1_ACTUAL_B_Ry,
         _21_RAM_2_1_ACTUAL_A_LOAD,
         _22_RAM_2_1_ACTUAL_A_Rx,
         _23_RAM_2_1_ACTUAL_A_Ry,
         _24_RAM_2_2_ACTUAL_B_LOAD,
         _25_RAM_2_2_ACTUAL_B_Rx,
         _26_RAM_2_2_ACTUAL_B_Ry,
         _27_RAM_2_2_ACTUAL_A_LOAD,
         _28_RAM_2_2_ACTUAL_A_Rx,
         _29_RAM_2_2_ACTUAL_A_Ry,
         _30_,
         _31_ALIVE,
         _32_RAM_1_TARE_COMPLETED,
         _33_RAM_2_TARE_COMPLETED,
         _34_RAM_3_TARE_COMPLETED,
         _35_RAM_4_TARE_COMPLETED,
         _36_,
         _37_,
         _38_,
         _39_,
         _40_,
         _41_Ram_1_1_B_Fx,
         _42_Ram_1_1_B_Fy,
         _43_Ram_1_1_B_Fz,
         _44_Ram_1_1_B_Mx,
         _45_Ram_1_1_B_My,
         _46_Ram_1_1_B_Mz,
         _47_Ram_1_1_A_Fx,
         _48_Ram_1_1_A_Fy,
         _49_Ram_1_1_A_Fz,
         _50_Ram_1_1_A_Mx,
         _51_Ram_1_1_A_My,
         _52_Ram_1_1_A_Mz,
         _53_Ram_1_2_B_Fx,
         _54_Ram_1_2_B_Fy,
         _55_Ram_1_2_B_Fz,
         _56_Ram_1_2_B_Mx,
         _57_Ram_1_2_B_My,
         _58_Ram_1_2_B_Mz,
         _59_Ram_1_2_A_Fx,
         _60_Ram_1_2_A_Fy,
         _61_Ram_1_2_A_Fz,
         _62_Ram_1_2_A_Mx,
         _63_Ram_1_2_A_My,
         _64_Ram_1_2_A_Mz,
         _65_,
         _66_Ram_2_1_B_Fx,
         _67_Ram_2_1_B_Fy,
         _68_Ram_2_1_B_Fz,
         _69_Ram_2_1_B_Mx,
         _70_Ram_2_1_B_My,
         _71_Ram_2_1_B_Mz,
         _72_Ram_2_1_A_Fx,
         _73_Ram_2_1_A_Fy,
         _74_Ram_2_1_A_Fz,
         _75_Ram_2_1_A_Mx,
         _76_Ram_2_1_A_My,
         _77_Ram_2_1_A_Mz,
         _78_Ram_2_2_B_Fx,
         _79_Ram_2_2_B_Fy,
         _80_Ram_2_2_B_Fz,
         _81_Ram_2_2_B_Mx,
         _82_Ram_2_2_B_My,
         _83_Ram_2_2_B_Mz,
         _84_Ram_2_2_A_Fx,
         _85_Ram_2_2_A_Fy,
         _86_Ram_2_2_A_Fz,
         _87_Ram_2_2_A_Mx,
         _88_Ram_2_2_A_My,
         _89_Ram_2_2_A_Mz,
         _90_,
         _91_,
         _92_,
         _93_,
         _94_,
         _95_,
         _96_,
         _97_,
         _98_,
         _99_,
        _100_,
        _101_RAM_1_DATA_READ_COMPLETED,
        _102_RAM_2_DATA_READ_COMPLETED
    }
    #endregion
    #endregion

    #region DataGridView
    public enum e_DGV_PLC1
    {
        No,
        Value,
        Address,
        Description
    }

    public enum e_DGV_PLC2
    {
        No,
        Value,
        Address,
        Count,
        Description
    }
    #endregion

    #region PCAlarm
    public enum e_PCAlarm
    {
        ALARM_PLC_STATUS_R_IN,                 // 
        ERROR_START_DEVICE,                    // 
        ERROR_LOADING_PARAMETER,               // 
        ERROR_LOADING_PARAMETER_RECIPE,        // 
        ERROR_SAVE_DATA,                       // 
        ERROR_SAVE_DATA_RPQ,                   // 
        ERROR_SAVE_PARAMETER,                  // 
        ERROR_SAVE_PARAMETER_RECIPE,           // 
        ERROR_SAVE_PARAMETER_RECIPE_DOWNLOAD,  // 
        ERROR_UPDATE_PARAMETER_RECIPE,         // GlobalValue.Parameter[(int)e_Parameter.Recipe]
        ERROR_DOWNLOAD_PARAMETER,              // 
        ERROR_DOWNLOAD_PARAMETER_OFFSET,       // 
        ERROR_UPLOAD_PARAMETER,                // 
    }
    #endregion
    #endregion

    public static class GlobalValue
    {
        public static Dictionary<string, string[]>[] Parameter = new Dictionary<string, string[]>[Enum.GetNames(typeof(e_Parameter)).Length];

        // TODO : Array.Clear(GlobalValue.PCAlarm, 0, GlobalValue.PCAlarm.Length);
        //        FrmAlarm_FormClosing
        public static bool[] PCAlarm = new bool[Enum.GetNames(typeof(e_PCAlarm)).Length];

        public static class Directory
        {
            public static string Application = System.Windows.Forms.Application.StartupPath;

            public static string Parameter = Application + "\\CFG\\Parameter";
            public static string Recipe = Application + "\\CFG\\Recipe";

            public static string bakParameter = Application + "\\BAK\\Parameter";
            public static string bakRecipe = Application + "\\BAK\\Recipe";

            public static string Resource = Application + "\\Resource";

            public static string CSV = Application + "\\CSV";

            public static string Download = Application + "\\Download";
        }

        public static class File
        {
            public static class Status
            {
                public static string Green = Directory.Resource + "\\Status_Green.png";
                public static string Red = Directory.Resource + "\\Status_Red.png";
                public static string White = Directory.Resource + "\\Status_White.png";
                public static string Yellow = Directory.Resource + "\\Status_Yellow.png";
            }
        }

        public static class Form
        {
            public static FrmAlarm FormAlarm = null;
            public static FrmAlarmHistory FormAlarmHistory = null;
            public static FrmConverter FormConverter = null;
            public static FrmData FormData = null;
            public static FrmHidden FormHidden = null;
            public static FrmMdi FormMdi = null;
            public static FrmParameter FormParameter = null;
            public static FrmSearchData FormSearchData = null;
            public static FrmUxyLxy FormUxyLxy = null;
        }

        public static class Test
        {
            public static string Actual1 = string.Empty;
            public static string Actual2 = string.Empty;
            public static string Actual3 = string.Empty;

            public static string Ram1_1Result = string.Empty;
            public static string Ram1_2Result = string.Empty;
            public static string Ram2_1Result = string.Empty;
            public static string Ram2_2Result = string.Empty;

            public static double Ram1_1LoadTestValue = 0;
            public static double Ram1_1_B_Rx = 0;
            public static double Ram1_1_B_Ry = 0;
            public static double Ram1_1_A_Rx = 0;
            public static double Ram1_1_A_Ry = 0;
            public static double Ram1_1_temp = 0;

            public static double Ram1_2LoadTestValue = 0;
            public static double Ram1_2_B_Rx = 0;
            public static double Ram1_2_B_Ry = 0;
            public static double Ram1_2_A_Rx = 0;
            public static double Ram1_2_A_Ry = 0;
            public static double Ram1_2_temp = 0;

            public static double Ram2_1LoadTestValue = 0;
            public static double Ram2_1_B_Rx = 0;
            public static double Ram2_1_B_Ry = 0;
            public static double Ram2_1_A_Rx = 0;
            public static double Ram2_1_A_Ry = 0;
            public static double Ram2_1_temp = 0;

            public static double Ram2_2LoadTestValue = 0;
            public static double Ram2_2_B_Rx = 0;
            public static double Ram2_2_B_Ry = 0;
            public static double Ram2_2_A_Rx = 0;
            public static double Ram2_2_A_Ry = 0;
            public static double Ram2_2_temp = 0;

            // 2022-02-10 : 다운로드된 ++, -- 하중 값 사용
            public static double PP하중 = 0;
            public static double MM하중 = 0;
        }
    }

    public static class GlobalPLC
    {
        #region MELSEC (MX Component)
        public static ActUtlType MX = null;
        // Returned value
        public static int Status = -1;

        public struct TBit
        {
            public string StartAddress;
            public int Count;
            public short[] Value;
            public bool[] Data;
            public string[] Description;
        }
        public static TBit B_IN;
        public static TBit B_OUT;
        public static TBit F_Alarm;

        public struct TWord
        {
            public string StartAddress;
            public int Count;
            public short[] Value;
            public string[] Description;
        }
        public static TWord R_IN;
        public static TWord R_OUT;
        #endregion
    }
}
