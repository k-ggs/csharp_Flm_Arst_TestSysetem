using Automation.BDaq;
using Flm_Arst_TestSysetem.Base;
using Flm_Arst_TestSysetem.Model;
using Flm_Arst_TestSysetem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Flm_Arst_TestSysetem.PCIE_1816H
{
    public class PCI_1816H_API:NotifyPropertyBase
    {

       public WaveformAiCtrl waveformAiCtrl=new WaveformAiCtrl();
        public InstantAiCtrl instantAiCtrl=new InstantAiCtrl();
        public InstantDoCtrl instantDoCtrl=new InstantDoCtrl();


        public int _chanCount;
        public int _startChannel;
        public int _sectionLength;
        public double _rate;
        public int chanCount
        {

            get { return _chanCount;  }
            set { _chanCount = value;RaisePropertyChanged(); }
        }
        public int startChannel
        {
            get { return _startChannel; }
            set { _startChannel = value; RaisePropertyChanged(); }
        }
        public int sectionLength
        {

            get { return _sectionLength; }
            set { _sectionLength = value; RaisePropertyChanged(); }
        }
        public double rate
        {

            get { return _rate; }
            set { _rate = value; RaisePropertyChanged(); }
        }
        int x_len = 100;
        double[] m_dataScaled;
        double m_xInc;
        public string strCardConfigPath = string.Empty;
   
        public void PCI_ini()
        {
            cout_t = 0;
            Globalvariable.systemMonitorViewModel.Max = x_len;
            waveformAiCtrl.SelectedDevice = new DeviceInformation("PCIE-1816H,BID#0");
          //  instantAiCtrl.SelectedDevice = new DeviceInformation("PCIE-1816H,BID#0");
            instantDoCtrl.SelectedDevice = new DeviceInformation("PCIE-1816H,BID#0");
            ErrorCode ret = waveformAiCtrl.LoadProfile(strCardConfigPath = "./pcie-1816h_profile/pci-1816h.xml");
       //     ErrorCode ret1 = instantAiCtrl.LoadProfile(strCardConfigPath = "./pcie-1816h_profile/pci-1816h.xml");
            ErrorCode ret2 = instantDoCtrl.LoadProfile(strCardConfigPath = "./pcie-1816h_profile/pci-1816h.xml");
            //if (instantAiCtrl.Initialized == false)
            //{
              
            //    Globalvariable.systemMonitorViewModel.LogInfoAdd(deviceName.AI,LogInfo.fail_ini,LogType.Fault);
            //}
            if (waveformAiCtrl.Initialized == false)
            {

                Globalvariable.systemMonitorViewModel.LogInfoAdd(deviceName.AI, LogInfo.fail_ini, LogType.Fault);
            }

            if (instantDoCtrl.Initialized == false)
            {
              
                Globalvariable.systemMonitorViewModel.LogInfoAdd(deviceName.DO, LogInfo.fail_ini, LogType.Fault);
            }
           waveformAiCtrl.Conversion.ClockRate=rate;
            waveformAiCtrl.Record.SectionCount = 0;
            waveformAiCtrl.Record.SectionLength =sectionLength;
            waveformAiCtrl.Conversion.ChannelCount =chanCount;
            waveformAiCtrl.Conversion.ChannelStart = startChannel;

            
            Globalvariable.systemMonitorViewModel.data_C = 0;
            SystemMonitorViewModel.Highdatas.Clear();

            chanCount = waveformAiCtrl.Conversion.ChannelCount;
             sectionLength = waveformAiCtrl.Record.SectionLength;
            m_dataScaled = new double[chanCount * sectionLength];
            this.waveformAiCtrl.DataReady += WaveformAiCtrl_DataReady;
            this.waveformAiCtrl.Prepare();
            this.waveformAiCtrl.Start();

        }
        private void HandleError(ErrorCode err)
        {
            if ((err >= ErrorCode.ErrorHandleNotValid) && (err != ErrorCode.Success))
            {
            
                Globalvariable.systemMonitorViewModel.LogInfoAdd("ErrorCode #", "Error"+ err.ToString(), LogType.Fault);
            }
        }
        public void start()
        {
          

            int channelCountMax = instantAiCtrl.Features.ChannelCountMax;
            double[] scaledData = new double[chanCount];//the count of elements in this array should not be less than the value of the variable channelCount
     
            do
            {
                string start = DateTime.Now.ToString("fffffff");
                // read samples, just scaled data in this demo
                ErrorCode errorCode = instantAiCtrl.Read(startChannel, chanCount, scaledData);


                if (BioFailed(errorCode))
                {
                    throw new Exception();
                }
             
               
                    SystemMonitorViewModel.LineSeries1.Values.Add(scaledData[0]);
                    SystemMonitorViewModel.LineSeries2.Values.Add(scaledData[1]);
                    SystemMonitorViewModel.LineSeries3.Values.Add(scaledData[2]);
                    SystemMonitorViewModel.LineSeries4.Values.Add(scaledData[3]);
                    SystemMonitorViewModel.LineSeries5.Values.Add(scaledData[4]);
                    SystemMonitorViewModel.LineSeries6.Values.Add(scaledData[5]);
                    SystemMonitorViewModel.LineSeries7.Values.Add(scaledData[6]);
                    SystemMonitorViewModel.LineSeries8.Values.Add(scaledData[7]);
                    SystemMonitorViewModel.pressure1.Values.Add(scaledData[8]);
                    SystemMonitorViewModel.pressure2.Values.Add(scaledData[9]);

                if (SystemMonitorViewModel.LineSeries1.Values.Count > 1000) {

                    SystemMonitorViewModel.LineSeries1.Values.RemoveAt(0);
                    SystemMonitorViewModel.LineSeries2.Values.RemoveAt(0);
                    SystemMonitorViewModel.LineSeries3.Values.RemoveAt(0); 
                    SystemMonitorViewModel.LineSeries4.Values.RemoveAt(0); 
                    SystemMonitorViewModel.LineSeries5.Values.RemoveAt(0); 
                    SystemMonitorViewModel.LineSeries6.Values.RemoveAt(0); 
                    SystemMonitorViewModel.LineSeries7.Values.RemoveAt(0); 
                    SystemMonitorViewModel.LineSeries8.Values.RemoveAt(0); 
                    SystemMonitorViewModel.pressure1.Values.RemoveAt(0); 
                    SystemMonitorViewModel.pressure2.Values.RemoveAt(0); 

                };
                //  }));
                //   Console.Write("\n");
                Thread.Sleep(0);
                string end = DateTime.Now.ToString("fffffff");
                Console.Write((Convert.ToInt32(end) - Convert.ToInt32(start)).ToString() + "\n");
            } while (true);


        }
       
        private void WaveformAiCtrl_DataReady2(object sender, BfdAiEventArgs e)
        {

            try
            {
               
                WaveformAiCtrl waveformAiCtrl = (WaveformAiCtrl)sender;
                Int32 channelCountMax = waveformAiCtrl.Features.ChannelCountMax;
                Int32 startChan = waveformAiCtrl.Conversion.ChannelStart;
                Int32 channelCount = waveformAiCtrl.Conversion.ChannelCount;
                Int32 sectionLength = waveformAiCtrl.Record.SectionLength;
            
                Int32 remainingCount = e.Count;
                Int32 bufSize = sectionLength * channelCount;
             
              
                    Double[] sectionBuffer = new Double[e.Count];

              
                    waveformAiCtrl.GetData(e.Count, sectionBuffer);
             
             
                            App.Current.Dispatcher.BeginInvoke(new Action(() => {
                            SystemMonitorViewModel.LineSeries1.Values.Add(sectionBuffer[0] );
                          
                            SystemMonitorViewModel.LineSeries2.Values.Add(sectionBuffer[1] );
                            SystemMonitorViewModel.LineSeries3.Values.Add(sectionBuffer[2] );
                            SystemMonitorViewModel.LineSeries4.Values.Add(sectionBuffer[3] );
                            SystemMonitorViewModel.LineSeries5.Values.Add( sectionBuffer[4] );
                            SystemMonitorViewModel.LineSeries6.Values.Add( sectionBuffer[5] );
                            SystemMonitorViewModel.LineSeries7.Values.Add( sectionBuffer[6] );
                            SystemMonitorViewModel.LineSeries8.Values.Add( sectionBuffer[7] );
                            SystemMonitorViewModel.pressure1.Values.Add(sectionBuffer[8] );
                            SystemMonitorViewModel.pressure2.Values.Add( sectionBuffer[9] );
                            SystemMonitorViewModel.LineSeries1.Title = "火焰传感器1:" + Math.Round(sectionBuffer[0], 3).ToString();
                            SystemMonitorViewModel.LineSeries2.Title = "火焰传感器2:" + Math.Round(sectionBuffer[1], 3).ToString();
                            SystemMonitorViewModel.LineSeries3.Title = "火焰传感器3:" + Math.Round(sectionBuffer[2], 3).ToString();
                            SystemMonitorViewModel.LineSeries4.Title = "火焰传感器4:" + Math.Round(sectionBuffer[3], 3).ToString();
                            SystemMonitorViewModel.LineSeries5.Title = "火焰传感器5:" + Math.Round(sectionBuffer[4], 3).ToString();
                            SystemMonitorViewModel.LineSeries6.Title = "火焰传感器6:" + Math.Round(sectionBuffer[5], 3).ToString();
                            SystemMonitorViewModel.LineSeries7.Title = "火焰传感器7:" + Math.Round(sectionBuffer[6], 3).ToString();
                            SystemMonitorViewModel.LineSeries8.Title = "火焰传感器8:" + Math.Round(sectionBuffer[7], 3).ToString();
                            SystemMonitorViewModel.pressure1.Title = "压力传感器1:" +   Math.Round(sectionBuffer[8], 3).ToString();
                            SystemMonitorViewModel.pressure2.Title = "压力传感器2:" +   Math.Round(sectionBuffer[9], 3).ToString();
                            //    SystemMonitorViewModel.x.Add(cout );
                            //    cout += 1;

                            Globalvariable.systemMonitorViewModel.Min += 1;
                        
                            if (SystemMonitorViewModel.LineSeries1.Values.Count > x_len)
                            {
                              
                             
                                //SystemMonitorViewModel.LineSeries1.Values.RemoveAt(0);
                                //SystemMonitorViewModel.LineSeries2.Values.RemoveAt(0);
                                //SystemMonitorViewModel.LineSeries3.Values.RemoveAt(0);
                                //SystemMonitorViewModel.LineSeries4.Values.RemoveAt(0);
                                //SystemMonitorViewModel.LineSeries5.Values.RemoveAt(0);
                                //SystemMonitorViewModel.LineSeries6.Values.RemoveAt(0);
                                //SystemMonitorViewModel.LineSeries7.Values.RemoveAt(0);
                                //SystemMonitorViewModel.LineSeries8.Values.RemoveAt(0);
                                //SystemMonitorViewModel.pressure1.Values.RemoveAt(0);
                                //SystemMonitorViewModel.pressure2.Values.RemoveAt(0);
                             
                            };
                     }));
                       

               
                    
                 
                
            }
            catch (System.Exception ex)
            {

                Globalvariable.systemMonitorViewModel.LogInfoAdd("Error #", "Error" + ex.ToString(), LogType.Fault);
            }
        }


        private int cout_t = 0;

        private void WaveformAiCtrl_DataReady(object sender, BfdAiEventArgs e)
        {

            try
            {
                int slen = sectionLength;

                WaveformAiCtrl waveformAiCtrl = (WaveformAiCtrl)sender;
                //Int32 channelCountMax = waveformAiCtrl.Features.ChannelCountMax;
                //Int32 startChan = waveformAiCtrl.Conversion.ChannelStart;
                //Int32 channelCount = waveformAiCtrl.Conversion.ChannelCount;
                //Int32 sectionLength = waveformAiCtrl.Record.SectionLength;

                //Int32 remainingCount = e.Count;
                //Int32 bufSize = sectionLength * channelCount;


                Double[] sectionBuffer = new Double[e.Count];


                waveformAiCtrl.GetData(e.Count, sectionBuffer);

                slen = (int)Math.Floor((double)e.Count/chanCount) ;
               
                for (int i = 0; i < slen; i++)
                {

                    if (Globalvariable.systemMonitorViewModel.Test_Type_No != "GZW")
                    {
                        SystemMonitorViewModel.Highdatas.Add(new Model_data
                        {
                            idx = cout_t++,
                            Test_index = Globalvariable.systemMonitorViewModel.test_maxRow_id, //(int)Globalvariable.systemMonitorViewModel.Max++,
                            Fire_sensor1 = Math.Round(sectionBuffer[i * chanCount + 0], 2),

                            Fire_sensor2 = Math.Round(sectionBuffer[i * chanCount + 1], 2),
                            Fire_sensor3 = Math.Round(sectionBuffer[i * chanCount + 2], 2),
                            Fire_sensor4 = Math.Round(sectionBuffer[i * chanCount + 3], 2),
                            Fire_sensor5 = Math.Round(sectionBuffer[i * chanCount + 4], 2),
                            //  Fire_sensor6 = Math.Round(sectionBuffer[i * chanCount + 5], 2),
                            // Fire_sensor7 = Math.Round(sectionBuffer[i * chanCount + 6], 2),
                            // Fire_sensor8 = Math.Round(sectionBuffer[i * chanCount + 7], 2),
                            P_sensor1 = Math.Round(sectionBuffer[i*chanCount+8] * 6.895, 2),
                            //  P_sensor2 = Math.Round(sectionBuffer[i * chanCount + 9], 2),
                            Clock_Rate = rate,
                            media_name = Globalvariable.systemMonitorViewModel.media,
                         
                            Create_time = DateTime.Now.ToString("yyyyMMddHHmmss"),
                          
                            sectionLength = sectionLength



                        });
                    }


                    else
                    {

                        SystemMonitorViewModel.Highdatas.Add(new Model_data
                        {
                            idx = cout_t++,
                            Test_index = Globalvariable.systemMonitorViewModel.test_maxRow_id, //(int)Globalvariable.systemMonitorViewModel.Max++,
                          //  Fire_sensor1 = Math.Round(sectionBuffer[i * chanCount + 0], 2),

                          //  Fire_sensor2 = Math.Round(sectionBuffer[i * chanCount + 1], 2),
                           // Fire_sensor3 = Math.Round(sectionBuffer[i * chanCount + 2], 2),
                           // Fire_sensor4 = Math.Round(sectionBuffer[i * chanCount + 3], 2),
                           // Fire_sensor5 = Math.Round(sectionBuffer[i * chanCount + 4], 2),
                              Fire_sensor6 = Math.Round(sectionBuffer[i * chanCount + 5], 2),
                             Fire_sensor7 = Math.Round(sectionBuffer[i * chanCount + 6], 2),
                            Fire_sensor8 = Math.Round(sectionBuffer[i * chanCount + 7], 2),
                           // P_sensor1 = Math.Round(sectionBuffer[i * chanCount + 8], 2),
                              P_sensor2 = Math.Round(sectionBuffer[i * chanCount + 9] * 6.895, 2),
                            Clock_Rate = rate,
                            media_name = Globalvariable.systemMonitorViewModel.media,
                         
                            Create_time = DateTime.Now.ToString("yyyyMMddHHmmss"),
                          
                            sectionLength = sectionLength



                        });


                    }
                
                
                
                
                
                
                
                
                }
                







                if (Globalvariable.systemMonitorViewModel.data_C == 0)
                {

                    sys_testres_record str = new sys_testres_record();
                    if (Globalvariable.systemMonitorViewModel.Test_Type_No != "GZW")
                    {
                     Globalvariable.systemMonitorViewModel.Initial_pressure=   str.Initial_pressure = Math.Round(SystemMonitorViewModel.Highdatas.Sum(m => m.P_sensor1) / slen,2);
                    }
                    else
                    {
                        Globalvariable.systemMonitorViewModel.Initial_pressure= str.Initial_pressure = Math.Round(SystemMonitorViewModel.Highdatas.Sum(m => m.P_sensor2) / slen,2);
                    }

                    str.Product_No = Globalvariable.systemMonitorViewModel.Product_Nos;
                    str.Product_BitNo = Globalvariable.systemMonitorViewModel.Product_BitNos;
                    str.Media_name = Globalvariable.systemMonitorViewModel.Media_names;
                    str.Test_Type_No = Globalvariable.systemMonitorViewModel.Test_Type_No;
                    str.Media_C = Convert.ToDouble(Globalvariable.systemMonitorViewModel.Media_C);
                    str.CBTB_G_C = Convert.ToDouble(Globalvariable.systemMonitorViewModel.CBTB_G_C);
                    str.frequency = (Convert.ToInt32(Globalvariable.systemMonitorViewModel.frequency)+ 1).ToString();
                    str.Create_time = DateTime.Now.ToString("yyyyMMddHHmmss");
                    str.Test_index = Globalvariable.systemMonitorViewModel.mTexidx;
                    Globalvariable.systemMonitorViewModel.mTexidx = 0;
                    Globalvariable.industrialBLL.insert_test_record(str);




                }



                Globalvariable.systemMonitorViewModel.data_C += 1 * slen;
            }
            catch (System.Exception ex)
            {

                Globalvariable.systemMonitorViewModel.LogInfoAdd("Error #336", "Error" + ex.ToString(), LogType.Fault);
            }
        }


        public void log_list_add(int count,double[] data)
        { 
        
        
        }
        static bool BioFailed(ErrorCode err)
        {
            return err < ErrorCode.Success && err >= ErrorCode.ErrorHandleNotValid;
        }
        public void stop()
        {

            waveformAiCtrl.Stop();
        }

    }
}

