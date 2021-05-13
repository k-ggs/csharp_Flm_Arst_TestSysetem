using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flm_Arst_TestSysetem.Model
{
   public class Model_zuhuoqi
    {
        public int row_id { get; set; }

        public int Test_index { get; set; }
        public string   Test_name{ get; set; }
        public string senors { get; set; }
        public List<string>  Vals { get; set; }
        public double Clock_Rate{ get; set; }
        public string sval { get; set; }
        public string users_name { get; set; }
        public string senor1 { get; set; }
        public string senor2 { get; set; }
        public string senor3 { get; set; }
        public string senor4 { get; set; }
        public string senor5 { get; set; }
        public string senor6 { get; set; }
        public string senor7 { get; set; }
        public string senor8 { get; set; }
        public string Psenor1 { get; set; }
        public string time { get; set; }
        public string Psenor2 { get; set; }
        public string media_name { get; set; }
    }



    public class Model_data
    {
     
        public int Test_index { get; set; }

        public int idx { get; set; }

        public string Test_Type_No { get; set; }
        public double Fire_sensor1 { get; set; }
        public double Fire_sensor2 { get; set; }
        public double Fire_sensor3 { get; set; }
        public double Fire_sensor4 { get; set; }
        public double Fire_sensor5 { get; set; }
        public double Fire_sensor6 { get; set; }
        public double Fire_sensor7 { get; set; }
        public double Fire_sensor8 { get; set; }
        public double P_sensor1 { get; set; }
      
        public double P_sensor2 { get; set; }
        public string media_name { get; set; }
        public string Create_time { get; set; }
      
        public double Clock_Rate { get; set; }
       
    
        public double sectionLength { get; set; }

    }




    public class Model_word 
    {
        public int Row_id { get; set; }

        public string Test_name { get; set; }
        public string Test_unit { get; set; }
        public string Test_time { get; set; }
        public string Test_users { get; set; }
        public string media_name { get; set; }
        public string sp1 { get; set; }
     
        public string sp2 { get; set; }
        public string sp3 { get; set; }
        public string sp4 { get; set; }
        public string sp5 { get; set; }
        public string sp6 { get; set; }
        public string sp7 { get; set; }
        
    
        public string P1 { get; set; }
        public string P2 { get; set; }
      

    }


    public class Model_Result
    {
        public int Row_id { get; set; }

        public string Test_name { get; set; }
        public string Test_unit { get; set; }
        public string Test_time { get; set; }
        public string Test_users { get; set; }
        public string media_name { get; set; }
        public double clock_rate { get; set; }
        public double selectionlength { get; set; }

        public double maxfs1 { get; set; }
        public double maxfs2 { get; set; }
        public double maxfs3 { get; set; }
        public double maxfs4 { get; set; }
        public double maxfs5 { get; set; }
        public double maxfs6 { get; set; }
        public double maxfs7 { get; set; }
        public double maxfs8 { get; set; }
        public double maxPs1 { get; set; }
        public double maxPs2 { get; set; }
      

        public string sp1 { get; set; }

        public string sp2 { get; set; }
        public string sp3 { get; set; }
        public string sp4 { get; set; }
        public string sp5 { get; set; }
        public string sp6 { get; set; }
        public string sp7 { get; set; }


        public string P1 { get; set; }
        public string P2 { get; set; }

        public int idxsp1 { get; set; }

        public int idxsp2 { get; set; }
        public int idxsp3 { get; set; }
        public int idxsp4 { get; set; }
        public int idxsp5 { get; set; }
        public int idxsp6 { get; set; }
        public int idxsp7 { get; set; }
        public int idxsp8 { get; set; }


        public int idxP1 { get; set; }
        public int idxP2 { get; set; }


    }
}
