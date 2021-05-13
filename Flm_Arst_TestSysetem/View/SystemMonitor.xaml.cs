using Flm_Arst_TestSysetem.Base;
using Flm_Arst_TestSysetem.Base.tool;
using Flm_Arst_TestSysetem.DAL;
using Flm_Arst_TestSysetem.Model;
using Flm_Arst_TestSysetem.ViewModel;
using LiveCharts;
using LiveCharts.Geared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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


namespace Flm_Arst_TestSysetem.View
{
    /// <summary>
    /// SystemMonitor.xaml 的交互逻辑
    /// </summary>
    
        public partial class SystemMonitor : UserControl
        {
            public SystemMonitor()
            {
                InitializeComponent();
         
        }
        Regex reg = new Regex(@"^[A-Za-z\u4e00-\u9fa5]+$");
        private void LogList_CollectionChanged1(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //scroll_car2.ScrollToBottom(); 
            App.Current.Dispatcher.Invoke(new Action(() => { scroll_car2.ScrollToBottom(); }));
           
        }

        //private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        //    {
        //        double newWidth = this.mainView.ActualWidth + e.Delta;
        //        double newHeight = this.mainView.ActualHeight + e.Delta;

        //        if (newWidth < 500) newWidth = 500;
        //        if (newHeight < 100) newHeight = 100;

        //        this.mainView.Width = newWidth;
        //        this.mainView.Height = newHeight;

        //        this.mainView.SetValue(Canvas.LeftProperty, (this.RenderSize.Width - this.mainView.Width) / 2);
        //        //进行扩展，使用鼠标光标位置以中心进行缩放
        //        //自己扩展
        //    }

            bool _isMoving = false;
            Point _downPoint = new Point(0, 0);
            double left = 0, top = 0;
            //private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            //{
            //    _isMoving = true;
            //    _downPoint = e.GetPosition(sender as Canvas);

            //    left = double.Parse(this.mainView.GetValue(Canvas.LeftProperty).ToString());
            //    top = double.Parse(this.mainView.GetValue(Canvas.TopProperty).ToString());

            //    (sender as Canvas).CaptureMouse();
            //    e.Handled = true;
            //}

            private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                _isMoving = false;
                (sender as Canvas).ReleaseMouseCapture();
                e.Handled = true;
            }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {

        
           // Task.Factory.StartNew(() => { Globalvariable.pCI_1816H_API.start(); });

        }
      
        private void grid_systemMonitor_Loaded(object sender, RoutedEventArgs e)
        {
            Globalvariable.systemMonitorViewModel.CurrentDevice = new DeviceModel();
            this.grid_systemMonitor.DataContext = Globalvariable.systemMonitorViewModel;
         //   Globalvariable.systemMonitorViewModel = new SystemMonitorViewModel();
           
           
         
         //   Test_name_People.ItemsSource = Globalvariable.industrialBLL.Get_userNams();

         
           
            Globalvariable.systemMonitorViewModel.LogList.CollectionChanged += LogList_CollectionChanged1;

            Load_date();
        }

        private void bt_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               

                    if (txt_add_user.Visibility == Visibility.Collapsed)
            {
                txt_add_user.Visibility = Visibility.Visible;


            }
            else
                {
                    if (!reg.IsMatch(txt_add_user.Text))
                    {
                        return;
                    }

                    Globalvariable.industrialBLL.add_user(txt_add_user.Text);
               txt_add_user.Visibility = Visibility.Collapsed;
                txt_add_user.Text = string.Empty;

            }

            Test_name_People.ItemsSource = Globalvariable.industrialBLL.Get_userNams();

            }
            catch (Exception ex)
            {
                Globalvariable.systemMonitorViewModel.LogInfoAdd("ErrorCode #", "Error# 123" , LogType.Fault);
               // throw;
            }
        }


        private void Load_date()
        {



            this.Product_Nos.ItemsSource = Globalvariable.industrialBLL.Get_products_No();

            this.Media_names.ItemsSource = Globalvariable.industrialBLL.Get_MediaNams();
            this.Test_Type_No.ItemsSource = Globalvariable.industrialBLL.Get_Test_type();
           // this.Test_Type_No.SelectedValuePath = "Test_Type_No";
          //  this.Test_Type_No.DisplayMemberPath = "Test_Type_Desc";

        }
        private void Test_name_People_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {



            if (Test_name_People.SelectedItem == null)
            {
                return;
            }
            string[] names;
            List<string> listnames=new List<string>();
            if (txt_people_name.Text.Length > 0)
            {
                names = txt_people_name.Text.Split(',');
                listnames = names.ToList();
                if (listnames.Contains(Test_name_People.SelectedItem.ToString()))
                {
                    return;
                }
                else
                {
                    txt_people_name.Text += Test_name_People.SelectedItem.ToString() + ",";


                }
            }
            else
            { 
                txt_people_name.Text += Test_name_People.SelectedItem.ToString() + ","; }
            

        }
        private bool check_string()
        {
            Regex reg2 = new Regex(@"^[A-Za-z0-9\u4e00-\u9fa5^,\x22]+$");
            string name = Test_name_add.Text + txt_people_name.Text;
            if (string.IsNullOrEmpty(Test_name_add.Text) || string.IsNullOrEmpty(txt_people_name.Text) || !reg2.IsMatch(name))

            {
                Globalvariable.systemMonitorViewModel.LogInfoAdd("字符检查 #",  "字符不合法,请检查!", LogType.Fault);

                return false;
            }
            else
            {
                Globalvariable.systemMonitorViewModel.LogInfoAdd("字符检查 #", "字符成功!", LogType.Info);
                return true; }


        }



        private bool check_string_01()
        {
            Regex reg2 = new Regex(@"^[0-9^.\x22]+$");
         
            if (string.IsNullOrEmpty(Globalvariable.systemMonitorViewModel.Test_Type_No) || string.IsNullOrEmpty(Globalvariable.systemMonitorViewModel.Product_Nos) || string.IsNullOrEmpty(Globalvariable.systemMonitorViewModel.Media_names) 
                || !reg2.IsMatch(Globalvariable.systemMonitorViewModel.Media_C.ToString())
                || !reg2.IsMatch(Globalvariable.systemMonitorViewModel.CBTB_G_C.ToString())

                )

            {
                Globalvariable.systemMonitorViewModel.LogInfoAdd("字符检查 #", "字符不合法,请检查!", LogType.Fault);

                return false;
            }
            else
            {
                Globalvariable.systemMonitorViewModel.LogInfoAdd("字符检查 #", "字符成功!", LogType.Info);
                return true;
            }


        }

        private void bt_mediaaddClick(object sender, RoutedEventArgs e)
        {
            try
            {


                if (txt_add_media.Visibility == Visibility.Collapsed)
                {
                    txt_add_media.Visibility = Visibility.Visible;


                }
                else
                {
                    if (!reg.IsMatch(txt_add_media.Text))
                    {
                        return;
                    }

                    Globalvariable.industrialBLL.add_Media(txt_add_media.Text);
                    txt_add_media.Visibility = Visibility.Collapsed;
                    txt_add_media.Text = string.Empty;

                }

                Test_media_name.ItemsSource = Globalvariable.industrialBLL.Get_MediaNams();

            }
            catch (Exception ex)
            {
                Globalvariable.systemMonitorViewModel.LogInfoAdd("ErrorCode #", "Error# 216", LogType.Fault);
                // throw;
            }
        }

        //private void ToggleButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!check_string())
        //    {
        //        tg_record.IsChecked = false;
        //        return;


        //    }




        //    if (tg_record.IsChecked == true)
        //    {
        //        this.msg.Text ="";
        //        bt_save_his.IsEnabled = false;
        //        Globalvariable.systemMonitorViewModel.CurrentDevice.IsRecord = true;
        //        Globalvariable.systemMonitorViewModel.CurrentDevice.TestName = DataMysql.Get_test_name();
        //        Globalvariable.pCI_1816H_API.PCI_ini();



        //        ini_value();


        //        Globalvariable.systemMonitorViewModel.Test_name = this.Test_name.Text + "." + Test_name_add.Text;
        //        Globalvariable.systemMonitorViewModel.media= this.Test_media_name.Text;
        //        Globalvariable.systemMonitorViewModel.users_name= txt_people_name.Text;
        //        Globalvariable.systemMonitorViewModel.clock_rate = Globalvariable.pCI_1816H_API.rate;
        //    }
        //    else
        //    {

        //        Globalvariable.systemMonitorViewModel.CurrentDevice.IsRecord = false;
        //        Globalvariable.pCI_1816H_API.stop();
        //        bt_save_his.IsEnabled = true;

        //        Thread.Sleep(2000);
        //      //  Task.Factory.StartNew(() => { 

        //            get_charts_data();
        //        //});   

        //    }

        //}



        /// <summary>
        /// 开始采集
        /// </summary>
        public bool start_data_collection()
        {

            if (!check_string_01())
            {
           //     tg_record.IsChecked = false;
            //    Globalvariable.systemMonitorViewModel.CurrentDevice.IsRecord = false;
                return false;


            }




            if (tg_record.IsChecked == true)
            {
                this.msg.Text = "";
                bt_save_his.IsEnabled = false;
                Globalvariable.systemMonitorViewModel.CurrentDevice.IsRecord = true;
                // Globalvariable.systemMonitorViewModel.CurrentDevice.TestName = DataMysql.Get_test_name();
                Globalvariable.systemMonitorViewModel.test_maxRow_id = Globalvariable.industrialBLL.Get_test_maxRow_id();
             this.frequency.Text = (Globalvariable.industrialBLL.Get_test_frequency(this.Product_Nos.Text,this.Product_BitNos.Text ,this.Test_Type_No.SelectedValue.ToString(),Media_names.Text,CBTB_G_C.Text,Media_C.Text)).ToString();
                Globalvariable.systemMonitorViewModel.frequency = Convert.ToInt32(this.frequency.Text);

                 Globalvariable.pCI_1816H_API.PCI_ini();


                SystemMonitorViewModel.Highdatas = new List<Model_data>();
                ini_value();


           //     Globalvariable.systemMonitorViewModel.Test_name = this.Test_name.Text + "." + Test_name_add.Text;
                Globalvariable.systemMonitorViewModel.media = this.Test_media_name.Text;
                Globalvariable.systemMonitorViewModel.users_name = txt_people_name.Text;
                Globalvariable.systemMonitorViewModel.clock_rate = Globalvariable.pCI_1816H_API.rate;

              //  Globalvariable.
                return true;
            }
            else
            {

                Globalvariable.systemMonitorViewModel.CurrentDevice.IsRecord = false;
                Globalvariable.pCI_1816H_API.stop();
                bt_save_his.IsEnabled = true;

                Thread.Sleep(1000);
                //  Task.Factory.StartNew(() => { 

                get_charts_data();


                sys_testres_record str2 = new sys_testres_record();
                str2.Row_id = Globalvariable.systemMonitorViewModel.test_maxRow_id;
                if (Globalvariable.systemMonitorViewModel.Test_Type_No != "GZW")
                {
                    Globalvariable.systemMonitorViewModel.Explore_pressure= str2.explosion_pressure = SystemMonitorViewModel.Highdatas.Max(m => m.P_sensor1);
                    double max4 = SystemMonitorViewModel.Highdatas.Max(m => m.Fire_sensor4);
                    double max1 = SystemMonitorViewModel.Highdatas.Max(m => m.Fire_sensor1);
                 int t1=    SystemMonitorViewModel.Highdatas.Where(a => a.Fire_sensor4 == max4).Select(m => m.idx).FirstOrDefault();
                 int t2 = SystemMonitorViewModel.Highdatas.Where(a => a.Fire_sensor1 == max1).Select(m => m.idx).FirstOrDefault();
                    Globalvariable.systemMonitorViewModel.Flame_V= str2.Flame_V = Math.Round((Globalvariable.systemMonitorViewModel.S1 + Globalvariable.systemMonitorViewModel.S2 + Globalvariable.systemMonitorViewModel.S3) * 0.001 / Math.Abs(t2 - t1) * 0.000001,3);
                    Globalvariable.systemMonitorViewModel.Va_pressure= str2.Average_pressure = Math.Round(SystemMonitorViewModel.Highdatas.Sum(m => m.P_sensor1) / SystemMonitorViewModel.Highdatas.Max(m => m.idx) + 1,2);
                   str2.Anti_Fire_flag = SystemMonitorViewModel.Highdatas.Max(a => a.Fire_sensor5) > 0 ? 1 : 0;
                    Globalvariable.systemMonitorViewModel.FPB_RN = str2.Anti_Fire_flag == 1 ? true : false;
                }
                else
                {
                    Globalvariable.systemMonitorViewModel.Explore_pressure = str2.explosion_pressure = SystemMonitorViewModel.Highdatas.Max(m => m.P_sensor2);

                    double max7 = SystemMonitorViewModel.Highdatas.Max(m => m.Fire_sensor7);
                    double max6 = SystemMonitorViewModel.Highdatas.Max(m => m.Fire_sensor6);
                    int t1 = SystemMonitorViewModel.Highdatas.Where(a => a.Fire_sensor7 == max7).Select(m => m.idx).FirstOrDefault();
                    int t2 = SystemMonitorViewModel.Highdatas.Where(a => a.Fire_sensor6 == max6).Select(m => m.idx).FirstOrDefault();
                    Globalvariable.systemMonitorViewModel.Flame_V = str2.Flame_V = Math.Round((Globalvariable.systemMonitorViewModel.S4) * 0.001 / Math.Abs(t2 - t1) * 0.000001,3);
                    Globalvariable.systemMonitorViewModel.Va_pressure = str2.Average_pressure = Math.Round(SystemMonitorViewModel.Highdatas.Sum(m => m.P_sensor2) / SystemMonitorViewModel.Highdatas.Max(m => m.idx) + 1,2);
                    double p = SystemMonitorViewModel.Highdatas.Max(a => a.Fire_sensor8) - SystemMonitorViewModel.Highdatas.Min(a => a.Fire_sensor8) / SystemMonitorViewModel.Highdatas.Max(a => a.Fire_sensor8);
                    str2.Anti_Fire_flag = SystemMonitorViewModel.Highdatas.Max(a => a.Fire_sensor8) > 0 ? 1 : 0;
                        Globalvariable.systemMonitorViewModel.FPB_RN = str2.Anti_Fire_flag == 1 ? true: false;
                }
             
                Globalvariable.industrialBLL.update_test_record(str2);
               
                

                return false;
            }
        }

        private void get_charts_data()
        {
            //.AddRange(SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor1).ToArray()
            try
            {
                if (Globalvariable.systemMonitorViewModel.seriesViews_press!=null)
                { 
                Globalvariable.systemMonitorViewModel.seriesViews_press.Clear();
                Globalvariable.systemMonitorViewModel.seriesViews.Clear();
                }
                //  App.Current.Dispatcher.Invoke(new Action(() =>
                //  {

                if (Globalvariable.systemMonitorViewModel.Test_Type_No != "GZW")
                {
                    SystemMonitorViewModel.LineSeries1.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor1).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    SystemMonitorViewModel.LineSeries2.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor2).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    SystemMonitorViewModel.LineSeries3.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor3).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    SystemMonitorViewModel.LineSeries4.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor4).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    SystemMonitorViewModel.LineSeries5.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor5).ToArray().AsGearedValues().WithQuality(Quality.Low);
                  //  SystemMonitorViewModel.LineSeries6.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor6).ToArray().AsGearedValues().WithQuality(Quality.Low);
                 //   SystemMonitorViewModel.LineSeries7.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor7).ToArray().AsGearedValues().WithQuality(Quality.Low);
                  //  SystemMonitorViewModel.LineSeries8.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor8).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    SystemMonitorViewModel.pressure1.Values = SystemMonitorViewModel.Highdatas.Select(m => m.P_sensor1).ToArray().AsGearedValues().WithQuality(Quality.Low);
                   // SystemMonitorViewModel.pressure2.Values = SystemMonitorViewModel.Highdatas.Select(m => m.P_sensor2).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    Globalvariable.systemMonitorViewModel.seriesViews = new SeriesCollection { SystemMonitorViewModel.LineSeries1, SystemMonitorViewModel.LineSeries2, SystemMonitorViewModel.LineSeries3, SystemMonitorViewModel.LineSeries4,
                                                                         SystemMonitorViewModel.LineSeries5, };


                    Globalvariable.systemMonitorViewModel.seriesViews_press = new SeriesCollection { SystemMonitorViewModel.pressure1 };
                }
                else
                {
                  //  SystemMonitorViewModel.LineSeries1.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor1).ToArray().AsGearedValues().WithQuality(Quality.Low);
                   // SystemMonitorViewModel.LineSeries2.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor2).ToArray().AsGearedValues().WithQuality(Quality.Low);
                  //  SystemMonitorViewModel.LineSeries3.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor3).ToArray().AsGearedValues().WithQuality(Quality.Low);
                   // SystemMonitorViewModel.LineSeries4.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor4).ToArray().AsGearedValues().WithQuality(Quality.Low);
                   // SystemMonitorViewModel.LineSeries5.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor5).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    SystemMonitorViewModel.LineSeries6.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor6).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    SystemMonitorViewModel.LineSeries7.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor7).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    SystemMonitorViewModel.LineSeries8.Values = SystemMonitorViewModel.Highdatas.Select(m => m.Fire_sensor8).ToArray().AsGearedValues().WithQuality(Quality.Low);
                   // SystemMonitorViewModel.pressure1.Values = SystemMonitorViewModel.Highdatas.Select(m => m.P_sensor1).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    SystemMonitorViewModel.pressure2.Values = SystemMonitorViewModel.Highdatas.Select(m => m.P_sensor2).ToArray().AsGearedValues().WithQuality(Quality.Low);
                    Globalvariable.systemMonitorViewModel.seriesViews = new SeriesCollection {  SystemMonitorViewModel.LineSeries6, SystemMonitorViewModel.LineSeries7, SystemMonitorViewModel.LineSeries8 };


                    Globalvariable.systemMonitorViewModel.seriesViews_press = new SeriesCollection {  SystemMonitorViewModel.pressure2 };


                }
              //      }));
                //foreach (Model_data md in SystemMonitorViewModel.Highdatas)
                //{
                // //   SystemMonitorViewModel.LineSeries1.Values = new GearedValues<double>() { 1, 2, 3 }.WithQuality(Quality.Low);
                //    SystemMonitorViewModel.LineSeries1.Values.Add(md.Fire_sensor1);
                //    SystemMonitorViewModel.LineSeries2.Values.Add(md.Fire_sensor2);
                //    SystemMonitorViewModel.LineSeries3.Values.Add(md.Fire_sensor3);
                //    SystemMonitorViewModel.LineSeries4.Values.Add(md.Fire_sensor4);
                //    SystemMonitorViewModel.LineSeries5.Values.Add(md.Fire_sensor5);
                //    SystemMonitorViewModel.LineSeries6.Values.Add(md.Fire_sensor6);
                //    SystemMonitorViewModel.LineSeries7.Values.Add(md.Fire_sensor7);
                //    SystemMonitorViewModel.LineSeries8.Values.Add(md.Fire_sensor8);
                //    SystemMonitorViewModel.pressure1.Values.Add(md.P_sensor1);
                //    SystemMonitorViewModel.pressure2.Values.Add(md.P_sensor2);
                //}
            }
            catch (Exception ex)
            { 
            
            
            }

            //App.Current.Dispatcher.Invoke(new Action(() =>
            //{


            //    this.Chart.DataContext = Globalvariable.systemMonitorViewModel;
            //    this.Chart_pressure.DataContext = Globalvariable.systemMonitorViewModel;
            //}));
        }

        private void save_history(object sender, RoutedEventArgs e)
        {
            try {
                if (SystemMonitorViewModel.Highdatas.Count < 1)
                {
                    return;
                }
                Globalvariable.systemMonitorViewModel.data_count = 0;
                this.bt_save_his.IsEnabled = false;
                this.msg.Text = "存储中。。。";
             
                Task.Factory.StartNew(() => {Globalvariable.systemMonitorViewModel.data_count= DataMysql.BulkInsert(SystemMonitorViewModel.Highdatas, "fr_record");


                    Globalvariable.industrialBLL.Insert_result_bulk();
                    App.Current.Dispatcher.Invoke(() => {
                        Globalvariable.systemMonitorViewModel.seriesViews_press.Clear();
                        Globalvariable.systemMonitorViewModel.seriesViews.Clear();
                        SystemMonitorViewModel.Highdatas.Clear();
                        this.bt_save_his.IsEnabled = true;
                        this.msg.Text = "已完成。";
                    });   
                });
            
            
            }
            catch (Exception ex)
            {
                this.bt_save_his.IsEnabled = true;

            }
            
        }

        private void Product_Nos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               // this.Product_BitNos.Items.Clear();
             this.Product_BitNos.ItemsSource=   Globalvariable.industrialBLL.Get_products_BitNo(Product_Nos.Items[Product_Nos.SelectedIndex].ToString());
                this.Product_BitNos.SelectedIndex = 0;
          List<sys_product> lss=     Globalvariable.industrialBLL.Get_products_data(Product_Nos.Items[Product_Nos.SelectedIndex].ToString(), Product_BitNos.Items[Product_BitNos.SelectedIndex].ToString());
                Globalvariable.systemMonitorViewModel.Product_Models=  this.Product_Models.Text = lss[0].Product_Models;
this.GasGroup_No.Text= lss[0].GasGroup_No;
                Globalvariable.systemMonitorViewModel.Teststandard_No  =  this.Teststandard_No.Text = lss[0].Teststandard_No;
                Globalvariable.systemMonitorViewModel.Remake =    this.Remake.Text = lss[0].Remake;




                Media_names_SelectionChanged(sender, e);
            }
            catch (Exception ex)
            {

              //  throw;
            }
        }

        private void Product_BitNos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               
                List<sys_product> lss = Globalvariable.industrialBLL.Get_products_data(Product_Nos.Items[Product_Nos.SelectedIndex].ToString(), Product_BitNos.Items[Product_BitNos.SelectedIndex].ToString());
                Globalvariable.systemMonitorViewModel.Product_Models  =   this.Product_Models.Text = lss[0].Product_Models;
                Globalvariable.systemMonitorViewModel.GasGroup_No     = this.GasGroup_No.Text = lss[0].GasGroup_No;
                Globalvariable.systemMonitorViewModel.Teststandard_No  =   this.Teststandard_No.Text = lss[0].Teststandard_No;
                Globalvariable.systemMonitorViewModel.Remake =     this.Remake.Text = lss[0].Remake;
               this.Media_names.Items.Clear();
               this.Media_names.ItemsSource = Globalvariable.industrialBLL.Get_MediaNams();

                this.Media_names.SelectedIndex = 0;
            }
            catch (Exception)
            {

                //  throw;
            }
        }

        private void Media_names_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
           /*Globalvariable.systemMonitorViewModel.CBTB_G_C=      this.CBTB_G_C.Text = string.Empty;
                Globalvariable.systemMonitorViewModel.Media_C=      this.Media_C.Text = string.Empty;
                Globalvariable.systemMonitorViewModel.frequency = this.frequency.Text = string.Empty;
                List<sys_testres_record> lss = Globalvariable.industrialBLL.get_testres_record(Product_Nos.Items[Product_Nos.SelectedIndex].ToString(), Product_BitNos.Items[Product_BitNos.SelectedIndex].ToString(),this.Media_names.Items[Media_names.SelectedIndex].ToString());
                Globalvariable.systemMonitorViewModel.CBTB_G_C = this.CBTB_G_C.Text = lss[0].CBTB_G_C.ToString();
               Globalvariable.systemMonitorViewModel.Media_C =this.Media_C.Text = lss[0].Media_C.ToString();
           */
                this.CBTB_G_C.Text = string.Empty;
               this.Media_C.Text = string.Empty;
                 this.frequency.Text = string.Empty;
                List<sys_testres_record> lss = Globalvariable.industrialBLL.get_testres_record(Product_Nos.Items[Product_Nos.SelectedIndex].ToString(), Product_BitNos.Items[Product_BitNos.SelectedIndex].ToString(), this.Media_names.Items[Media_names.SelectedIndex].ToString());
               this.CBTB_G_C.Text = lss[0].CBTB_G_C.ToString();
                this.Media_C.Text = lss[0].Media_C.ToString();
            }
            catch (Exception)
            {

                //  throw;
            }
        }

 

        private void tg_record_Click(object sender, RoutedEventArgs e)
        {
            if (start_data_collection())
            {

                tg_record.IsChecked = true;
                return;
            }
            else { tg_record.IsChecked = false; return; }
        }

        private void Test_Type_No_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           this.frequency.Text = (Globalvariable.industrialBLL.Get_test_frequency(this.Product_Nos.Text, this.Product_BitNos.Text, this.Test_Type_No.SelectedValue.ToString(), Media_names.Text, CBTB_G_C.Text, Media_C.Text)).ToString();
            Globalvariable.systemMonitorViewModel.frequency = Convert.ToInt32(this.frequency.Text);
        }

        private void fire_click(object sender, RoutedEventArgs e)
        {
            adam6266_modbustcp.fire();
            Thread.Sleep(100);
            adam6266_modbustcp.unfire();
        }

        /// <summary>
        /// 初始化参数
        /// 
        /// </summary>
        private void ini_value()
        {

        //   Chart.DataContext = null;

       //    Chart_pressure.DataContext = null;
            SystemMonitorViewModel.LineSeries1.Values.Clear();
            SystemMonitorViewModel.LineSeries2.Values.Clear();
            SystemMonitorViewModel.LineSeries3.Values.Clear();
            SystemMonitorViewModel.LineSeries4.Values.Clear();
            SystemMonitorViewModel.LineSeries5.Values.Clear();
            SystemMonitorViewModel.LineSeries6.Values.Clear();
            SystemMonitorViewModel.LineSeries7.Values.Clear();
            SystemMonitorViewModel.LineSeries8.Values.Clear();
            SystemMonitorViewModel.pressure1.Values.Clear();
            SystemMonitorViewModel.pressure2.Values.Clear();

          //  Chart.AxisX.Clear();
          //  Chart_pressure.AxisX.Clear();
            

        }

       
    }
    }

