using Flm_Arst_TestSysetem.Model;
using Flm_Arst_TestSysetem.ViewModel;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts.Geared;
using System.Data;

namespace Flm_Arst_TestSysetem.View
{
    /// <summary>
    /// ReportManagement.xaml 的交互逻辑
    /// </summary>
    public partial class ReportManagement : UserControl
    {
        public ReportManagement()
        {
            InitializeComponent();
            dataGrid1.DataContext = Globalvariable.industrialBLL;
          
        }
        string word_temp_p = @"./word_temp/word_temp.docx";
        private void bt_select(object sender, RoutedEventArgs e)
        {
            Com_TNMS.ItemsSource= Globalvariable.industrialBLL.Get_TestNams(this.Min_date.SelectedDate.Value.ToString("yyyyMMdd"),this.Max_date.SelectedDate.Value.ToString("yyyyMMdd"));
        }

        private void get_chart(object sender, RoutedEventArgs e)
        {

      //    List<Model_data> monitorValueModels=  Globalvariable.industrialBLL.Get_Data_bulk(Com_TNMS.Text);
            hIS_TORY_VIE.H_LineSeries1.Values.Clear();
            hIS_TORY_VIE.H_LineSeries2.Values.Clear();
            hIS_TORY_VIE.H_LineSeries3.Values.Clear();
            hIS_TORY_VIE.H_LineSeries4.Values.Clear();
            hIS_TORY_VIE.H_LineSeries5.Values.Clear();
            hIS_TORY_VIE.H_LineSeries6.Values.Clear();
            hIS_TORY_VIE.H_LineSeries7.Values.Clear();
            hIS_TORY_VIE.H_LineSeries8.Values.Clear();
            hIS_TORY_VIE.H_pressure1.Values.Clear();
            hIS_TORY_VIE.H_pressure2.Values.Clear();
            //foreach (Model_zuhuoqi m in monitorValueModels)
            //{
            //    hIS_TORY_VIE.H_LineSeries1.Values.Add(Convert.ToDouble( m.Vals[0]));
            //    hIS_TORY_VIE.H_LineSeries2.Values.Add(Convert.ToDouble(m.Vals[1]));
            //    hIS_TORY_VIE.H_LineSeries3.Values.Add(Convert.ToDouble(m.Vals[2]));
            //    hIS_TORY_VIE.H_LineSeries4.Values.Add(Convert.ToDouble(m.Vals[3]));
            //    hIS_TORY_VIE.H_LineSeries5.Values.Add(Convert.ToDouble(m.Vals[4]));
            //    hIS_TORY_VIE.H_LineSeries6.Values.Add(Convert.ToDouble(m.Vals[5]));
            //    hIS_TORY_VIE.H_LineSeries7.Values.Add(Convert.ToDouble(m.Vals[6]));
            //    hIS_TORY_VIE.H_LineSeries8.Values.Add(Convert.ToDouble(m.Vals[7]));
            //    hIS_TORY_VIE.H_pressure1.Values.Add(Convert.ToDouble(m.Vals[8]));
            //    hIS_TORY_VIE.H_pressure2.Values.Add(Convert.ToDouble(m.Vals[9]));

            //}
            get_charts_data();
       //     dataGrid1.ItemsSource = Globalvariable.industrialBLL.model_M_data;

        }

        private void get_charts_data()
        {
           
            //.AddRange(SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor1).ToArray()
            try
            {
                if (Globalvariable.systemMonitorViewModel2.H_seriesViews_press != null)
                {
                    Globalvariable.systemMonitorViewModel2.H_seriesViews_press.Clear();
                  
                }
                if (Globalvariable.systemMonitorViewModel2.H_seriesViews != null)
                { Globalvariable.systemMonitorViewModel2.H_seriesViews.Clear(); }
               
               
            String test_type= Lmd.Select(m => m.Test_Type_No).FirstOrDefault();

                if (test_type == "GZ-II")
                {
                    hIS_TORY_VIE.H_LineSeries1.Values = Lmd.Select(m => m.Fire_sensor1).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    hIS_TORY_VIE.H_LineSeries2.Values = Lmd.Select(m => m.Fire_sensor2).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    hIS_TORY_VIE.H_LineSeries3.Values = Lmd.Select(m => m.Fire_sensor3).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    hIS_TORY_VIE.H_LineSeries4.Values = Lmd.Select(m => m.Fire_sensor4).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    hIS_TORY_VIE.H_LineSeries5.Values = Lmd.Select(m => m.Fire_sensor5).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    hIS_TORY_VIE.H_pressure1.Values = Lmd.Select(m => m.P_sensor1).ToArray().AsGearedValues().WithQuality(Quality.Low);

                    Globalvariable.systemMonitorViewModel2.H_seriesViews = new SeriesCollection {
                    hIS_TORY_VIE.H_LineSeries1,
                    hIS_TORY_VIE.H_LineSeries2,
                   hIS_TORY_VIE.H_LineSeries3,
                    hIS_TORY_VIE.H_LineSeries4,

                  hIS_TORY_VIE.H_LineSeries5,
                  //  hIS_TORY_VIE.H_LineSeries6,
                  //  hIS_TORY_VIE.H_LineSeries7,
                //  hIS_TORY_VIE.H_LineSeries8,
                    };


                    Globalvariable.systemMonitorViewModel2.H_seriesViews_press = new SeriesCollection { hIS_TORY_VIE.H_pressure1
                      //  ,hIS_TORY_VIE.H_pressure2
                    };


                }
                else
                {
                    hIS_TORY_VIE.H_LineSeries6.Values = Lmd.Select(m => m.Fire_sensor6).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    hIS_TORY_VIE.H_LineSeries7.Values = Lmd.Select(m => m.Fire_sensor7).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    hIS_TORY_VIE.H_LineSeries8.Values = Lmd.Select(m => m.Fire_sensor8).ToArray().AsGearedValues().WithQuality(Quality.Low);
                  
                    hIS_TORY_VIE.H_pressure2.Values = Lmd.Select(m => m.P_sensor2).ToArray().AsGearedValues().WithQuality(Quality.Low);



                    Globalvariable.systemMonitorViewModel2.H_seriesViews = new SeriesCollection {
                  
                    hIS_TORY_VIE.H_LineSeries6,
                   hIS_TORY_VIE.H_LineSeries7,
                 hIS_TORY_VIE.H_LineSeries8,
                    };


                    Globalvariable.systemMonitorViewModel2.H_seriesViews_press = new SeriesCollection { //hIS_TORY_VIE.H_pressure1
                        hIS_TORY_VIE.H_pressure2
                    };
                }


            }
            catch (Exception ex)
            {


            }

        
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Globalvariable.systemMonitorViewModel2 = new hIS_TORY_VIE();


            this.DataContext = Globalvariable.systemMonitorViewModel2;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
       
        Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("$Test_name$", "Test_name");
            dic.Add("$Test_users$", "Test_users");
            dic.Add("$Test_unit$", "Test_unit");
            dic.Add("$Test_time$", "Test_time");
            dic.Add("$sp1$", "sp1");
            dic.Add("$sp2$", "sp2");
            dic.Add("$sp3$", "sp3");
            dic.Add("$sp4$", "sp4");
            dic.Add("$sp5$", "sp5");
            dic.Add("$sp6$", "sp6");
            dic.Add("$sp7$", "sp7");
            dic.Add("$sp8$", "sp8");
            dic.Add("$P1$", "P1");
            dic.Add("$P2$", "P2");
            dic.Add("$media$", "media_name");
            //    dic.Add("$remark$", "remark");
        //    Globalvariable.WordUtility.GenerateWord(Globalvariable.industrialBLL.model_Zuhuoqis, dic,null);
        }

        private void dataGrid0_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid0.ItemsSource = Globalvariable.industrialBLL.get_testres_record();
        }
        List<Model_data> Lmd;
        private void dataGrid0_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            sys_testres_record dgr = (sys_testres_record)dataGrid0.SelectedItem;
            int value = dgr.Row_id;
            if (value >= 0)
            {
                Lmd = null;
                   Lmd = Globalvariable.industrialBLL.get_fr_record(value);
                dataGrid1.ItemsSource = Lmd;
if(H_Chart.Series!=null)
                { H_Chart.Series.Clear(); }

                if (H_Chart_pressure.Series != null)
                {
                    H_Chart_pressure.Series.Clear();
                }
               
                get_chart(sender, e);

            }
        }

        private void bt_ck(object sender, RoutedEventArgs e)
        {
            dataGrid0.ItemsSource = Globalvariable.industrialBLL.get_testres_record();
           
        }
    }
}
