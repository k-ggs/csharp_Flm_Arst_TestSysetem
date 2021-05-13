using Flm_Arst_TestSysetem.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flm_Arst_TestSysetem.Model
{
    public class Cm_box
    {

        public string text { get; set; }
        public int idx { get; set; }
    }
    /// <summary>
    /// 产品参数
    /// </summary>
    public class sys_product 
    {
        public int Row_id { get; set; }
        public string Product_No { get; set; }
        public string Product_Models { get; set; }
        public string Product_Spcfts { get; set; }
        public string Product_BitNo { get; set; }
        public string GasGroup_No  { get; set; }
        public string Teststandard_No{ get; set; }
        public string Remake{ get; set; }
        public string Create_time{ get; set; }
    }


    /// <summary>
    /// 气体组别
    /// </summary>
    public class sys_gasgroup
    {
        public int Row_id { get; set; }
        public string GasGroup_No { get; set; }
        public string GasGroup_Desc { get; set; }
        public string Create_time { get; set; }

    }

    /// <summary>
    /// 试验标准
    /// </summary>
    public class sys_teststandard
    {
        public int Row_id { get; set; }
        public string Teststandard_No { get; set; }
        public string Teststandard_Desc { get; set; }
        public string Create_time { get; set; }


    }
    /// <summary>
    /// 试验类型
    /// </summary>
    public class sys_test_type
    {
        public int Row_id { get; set; }
        public string Test_Type_No { get; set; }
        public string Test_Type_Desc { get; set; }
        public string Create_time { get; set; }

    }

    public class sys_testres_record
    {
        public int Row_id { get; set; }
        public int Test_index { get; set; }
        public string Product_No { get; set; }
        public string Product_BitNo { get; set; }
        public string Test_Type_No { get; set; }
        public string Media_name { get; set; }
        public double CBTB_G_C { get; set; }
        public double Media_C { get; set; }
        public string frequency { get; set; }
        public double Initial_pressure { get; set; }
        public double explosion_pressure { get; set; }
        public double Flame_V { get; set; }
        public double Average_pressure { get; set; }
        public double Anti_Fire_flag { get; set; }
        public string Create_time { get; set; }

    }
}
