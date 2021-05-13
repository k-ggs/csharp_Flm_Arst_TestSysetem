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
using Flm_Arst_TestSysetem.View;
using Flm_Arst_TestSysetem.ViewModel;

namespace Flm_Arst_TestSysetem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
          
            InitializeComponent();
          
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            this.r1.IsChecked = true;
            this.Fram_carmotion.Content = Globalvariable.SystemMonitor;
           
            this.Fram_carmotion2.Content = Globalvariable.reportManagement;
            this.Fram_carmotion3.Content = Globalvariable.reports_Page;
            this.Fram_carmotion4.Content = Globalvariable.user_Add;
            this.Bord_motion.Visibility = Visibility.Visible;
            this.Bord_motion2.Visibility = Visibility.Hidden;
            this.Bord_motion3.Visibility = Visibility.Hidden;
            this.Bord_motion4.Visibility = Visibility.Hidden;
            // this.DataContext = new MainViewModel();
            //  this.Fram_carmotion.Content = Globalvariable.SystemMonitor;
        }
        private void Border_MouseMove(object sender, MouseEventArgs e)
        {

            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            this.WindowState = WindowState.Minimized;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (globalvariabel.port != null && globalvariabel.port.IsOpen )
            //{
            //    globalvariabel.port.Close();
            //    globalvariabel.port.Dispose();
            //}
            this.Close();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if
                   (this.WindowState != WindowState.Normal)
            {

                this.WindowState = WindowState.Normal;
              
            }
            else
            {
                this.WindowState = WindowState.Maximized;
              
            }
        }

        private void Bt_close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Bt_min(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Bt_max(object sender, RoutedEventArgs e)
        {
            if
                             (this.WindowState != WindowState.Normal)
            {

                this.WindowState = WindowState.Normal;

            }
            else
            {
                this.WindowState = WindowState.Maximized;

            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void sys_checked(object sender, RoutedEventArgs e)
        {
            //  Globalvariable.SystemMonitor = new SystemMonitor();
            this.Bord_motion.Visibility = Visibility.Visible;
            this.Bord_motion2.Visibility = Visibility.Hidden;
            this.Bord_motion3.Visibility = Visibility.Hidden;
            this.Bord_motion4.Visibility = Visibility.Hidden;
        }

        private void histor_checked(object sender, RoutedEventArgs e)
        {
            this.Bord_motion.Visibility = Visibility.Hidden;
            this.Bord_motion2.Visibility = Visibility.Visible;
            this.Bord_motion3.Visibility = Visibility.Hidden;
            this.Bord_motion4.Visibility = Visibility.Hidden;
            //   Globalvariable.reportManagement = new ReportManagement();

        }

        private void report_checked(object sender, RoutedEventArgs e)
        {
            this.Bord_motion.Visibility = Visibility.Hidden;
            this.Bord_motion2.Visibility = Visibility.Hidden;
            this.Bord_motion3.Visibility = Visibility.Visible;
            this.Bord_motion4.Visibility = Visibility.Hidden;
        }

        private void config_bt(object sender, RoutedEventArgs e)
        {
             this.Bord_motion.Visibility = Visibility.Hidden;
            this.Bord_motion2.Visibility = Visibility.Hidden;
            this.Bord_motion3.Visibility = Visibility.Hidden;
            this.Bord_motion4.Visibility = Visibility.Visible;
        }
    }
}
