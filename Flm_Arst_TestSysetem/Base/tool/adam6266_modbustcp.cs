using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyModbus;
namespace Flm_Arst_TestSysetem.Base.tool
{
 public static   class adam6266_modbustcp
    {

      static  ModbusClient ModbusClient=new ModbusClient();
        // public static string ipAddress = "10.0.0.1";
        //private int port = 502;
        public static bool connect(string ipAddress, int port)
        {
              //  ModbusClient = new ModbusClient(ipAddress,port);
                ModbusClient.Connect(ipAddress, port);
            return ModbusClient.Connected;
        
        }

        public static bool close()
        {
            ModbusClient.Disconnect();


            return !ModbusClient.Connected;
        }


        public static bool[] ReadCoils(int startingAddress=17, int quantity=4)
        {

            return ModbusClient.ReadCoils(startingAddress, quantity);
        }
       
        public static void WriteMultipleCoils(int startingAddress , bool[] values )
        {
             ModbusClient.WriteMultipleCoils(startingAddress, values);


        }
        /// <summary>
        /// 点火
        /// </summary>
        public static void fire()
        {
            if (!ModbusClient.Connected)
            {
                connect("10.0.0.1", 502);
            }

            WriteMultipleCoils(16, new bool[] { true,false,false, false });
            Thread.Sleep(50);
            //WriteMultipleCoils(startingAddress, values);


        }
        /// <summary>
        /// 关火
        /// </summary>
        public static void unfire()
        {

            WriteMultipleCoils(16, new bool[] { false, false, false, false });
            //  Thread.Sleep(100);
            //WriteMultipleCoils(startingAddress, values);


        }

    }
}
