using Flm_Arst_TestSysetem.Base.tool;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Flm_Arst_TestSysetem.View
{
    /// <summary>
    /// reports_page.xaml 的交互逻辑
    /// </summary>
    public partial class reports_page : UserControl
    {
        public reports_page()
        {
            InitializeComponent();
        }

        private void bt_select(object sender, RoutedEventArgs e)
        {
            //  dataGrid1.ItemsSource = Globalvariable.industrialBLL.Get_ResultData(this.Min_date.SelectedDate.Value.ToString("yyyyMMdd"), this.Max_date.SelectedDate.Value.ToString("yyyyMMdd"), Com_TNMS.Text);

            dataGrid1.ItemsSource = Globalvariable.industrialBLL.GET_TEST_GROUP();

            dataGrid1.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //   dt_word_exc.ExportExcel_multi(Globalvariable.industrialBLL.dtGet_ResultData);

            //  (Model.sys_testres_record)dataGrid1.SelectedItem

            Model.sys_testres_record sys = (Model.sys_testres_record)dataGrid1.SelectedItem;
            sys.Test_Type_No = "GZW";//GZ-II
         DataTable  dt1=  Globalvariable.industrialBLL.get_GW(sys);
            //if (dt1 == null)
            //{ return; }
            //else
            //{
            //    if (dt1.Rows.Count < 1)
            //    { return; }
            //}
            sys.Test_Type_No = "GZ-II";//GZ-II
            DataTable dt2 = Globalvariable.industrialBLL.get_GW(sys);
            DataTable dt3 = Globalvariable.industrialBLL.get_GW_param(sys);
            dt_word_exc.ExportExcel_tag(dt3,dt1, dt2);
        }

        private void get_chart(object sender, RoutedEventArgs e)
        {

        }

        private void Max_date_CalendarClosed(object sender, RoutedEventArgs e)
        {
            Com_TNMS.ItemsSource = Globalvariable.industrialBLL.Get_TestNams(this.Min_date.SelectedDate.Value.ToString("yyyyMMdd"), this.Max_date.SelectedDate.Value.ToString("yyyyMMdd"));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid1.ItemsSource = Globalvariable.industrialBLL.GET_TEST_GROUP();
        }
    }
}
