using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flm_Arst_TestSysetem.Base
{
    public enum LogType
    {
        Info = 0, Warn = 1, Fault = 2
    }
    public static class LogInfo
    {
        public static string 
      sucess_check = " 已检测到",
      faial_check = "未检测到",
      sucess_open = "已打开",
      fail_open = "打开失败",
      sucess_ini = "初始化成功",
      fail_ini = "初始化失败",
      sucess_firestop = "阻火成功",
      fail_firestop = "阻火失败";



    }

    public static class deviceName
        {
        public static string firesensor1=  "火焰传感器 1#";
        public static string firesensor2 = "火焰传感器 2#";
        public static string firesensor3 = "火焰传感器 3#";
        public static string firesensor4 = "火焰传感器 4#";
        public static string firesensor5 = "火焰传感器 5#";
        public static string firesensor6 = "火焰传感器 6#";
        public static string firesensor7 = "火焰传感器 7#";
        public static string firesensor8 = "火焰传感器 8#";
        public static string presssensor1 = "压力传感器 1#";
        public static string presssensor2 = "压力传感器 2#"; 
        public static string switch1 = "开关阀 1#";
        public static string switch2 = "开关阀 2#";//点火控制
        public static string fire_contral1 = "点火控制 1#";


        public static string AI = "模拟量输入 1#";
        public static string AO = "模拟量输出 1#";//点火控制
        public static string DO = "数字量输出 1#";

    }
}
