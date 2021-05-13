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
    /// User_add.xaml 的交互逻辑
    /// </summary>
    public partial class User_add : UserControl
    {
        public User_add()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            pcie_group.DataContext = Globalvariable.pCI_1816H_API;
              DataTable s = Globalvariable.industrialBLL.Get_sLength();
            string unit= Globalvariable.industrialBLL.Get_unit();
            Globalvariable.systemMonitorViewModel.S1 = Convert.ToInt32(s.Rows[0]["s1"].ToString());
            Globalvariable.systemMonitorViewModel.S2 = Convert.ToInt32(s.Rows[0]["s2"].ToString());
            Globalvariable.systemMonitorViewModel.S3 = Convert.ToInt32(s.Rows[0]["s3"].ToString());
            Globalvariable.systemMonitorViewModel.S4 = Convert.ToInt32(s.Rows[0]["s4"].ToString());
            Globalvariable.systemMonitorViewModel.S5 = Convert.ToInt32(s.Rows[0]["s5"].ToString());
            Globalvariable.systemMonitorViewModel.S6 = Convert.ToInt32(s.Rows[0]["s6"].ToString());
            Globalvariable.systemMonitorViewModel.S7 = Convert.ToInt32(s.Rows[0]["s7"].ToString());
            s1.Text = s.Rows[0]["s1"].ToString();
           s2.Text=  s.Rows[0]["s2"].ToString();
          s3.Text = s.Rows[0]["s3"].ToString();
           s4.Text = s.Rows[0]["s4"].ToString();
            s5.Text = s.Rows[0]["s5"].ToString();
           s6.Text = s.Rows[0]["s6"].ToString();
            s7.Text = s.Rows[0]["s7"].ToString();
            Globalvariable.pCI_1816H_API.rate =Convert.ToDouble( s.Rows[0]["clock_rate"]);
            Globalvariable.pCI_1816H_API.chanCount = Convert.ToInt32(s.Rows[0]["channelcount"]);
            Globalvariable.pCI_1816H_API.startChannel = Convert.ToInt32(s.Rows[0]["start_channel"]);
            Globalvariable.pCI_1816H_API.sectionLength = Convert.ToInt32(s.Rows[0]["selection"]);


            Globalvariable.systemMonitorViewModel.txt_unit= txt_unit.Text = unit;

            this.dataGrid1.ItemsSource = Globalvariable.industrialBLL.Get_products_data();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Globalvariable.industrialBLL.update_Unit(txt_unit.Text))
            {
                MessageBox.Show("更新成功！");
                string unit = Globalvariable.industrialBLL.Get_unit();
                Globalvariable.systemMonitorViewModel.txt_unit = txt_unit.Text = unit;
            }
            else {
                MessageBox.Show("更新失败！");

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<string> ss = new List<string> { s1.Text,
                                                 s2.Text,
                                                  s3.Text,
                                                   s4.Text,
                                                    s5.Text,
                                                     s6.Text,
                                                      s7.Text,
                                                      
            };

            if (Globalvariable.industrialBLL.update_sLength(ss))
            {

                MessageBox.Show("更新成功！");

                DataTable s = Globalvariable.industrialBLL.Get_sLength();

                Globalvariable.systemMonitorViewModel.S1 = Convert.ToInt32(s.Rows[0]["s1"].ToString());
                Globalvariable.systemMonitorViewModel.S2 = Convert.ToInt32(s.Rows[0]["s2"].ToString());
                Globalvariable.systemMonitorViewModel.S3 = Convert.ToInt32(s.Rows[0]["s3"].ToString());
                Globalvariable.systemMonitorViewModel.S4 = Convert.ToInt32(s.Rows[0]["s4"].ToString());
                Globalvariable.systemMonitorViewModel.S5 = Convert.ToInt32(s.Rows[0]["s5"].ToString());
                Globalvariable.systemMonitorViewModel.S6 = Convert.ToInt32(s.Rows[0]["s6"].ToString());
                Globalvariable.systemMonitorViewModel.S7 = Convert.ToInt32(s.Rows[0]["s7"].ToString());
                s1.Text = s.Rows[0]["s1"].ToString();
                s2.Text = s.Rows[0]["s2"].ToString();
                s3.Text = s.Rows[0]["s3"].ToString();
                s4.Text = s.Rows[0]["s4"].ToString();
                s5.Text = s.Rows[0]["s5"].ToString();
                s6.Text = s.Rows[0]["s6"].ToString();
                s7.Text = s.Rows[0]["s7"].ToString();
            }
            else { MessageBox.Show("更新失败！"); }

          


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
          if( Globalvariable.industrialBLL.update_pcie((int)Globalvariable.pCI_1816H_API.rate,Globalvariable.pCI_1816H_API.sectionLength))

            {

                MessageBox.Show("更新成功！");
                DataTable s = Globalvariable.industrialBLL.Get_sLength();
                Globalvariable.pCI_1816H_API.rate = Convert.ToDouble(s.Rows[0]["clock_rate"]);
                Globalvariable.pCI_1816H_API.chanCount = Convert.ToInt32(s.Rows[0]["channelcount"]);
                Globalvariable.pCI_1816H_API.startChannel = Convert.ToInt32(s.Rows[0]["start_channel"]);
                Globalvariable.pCI_1816H_API.sectionLength = Convert.ToInt32(s.Rows[0]["selection"]);
            }
            else { MessageBox.Show("更新失败！"); }

          
        }

        private void bt_delete(object sender, RoutedEventArgs e)
        {

            Model.sys_product sys_Prdt = (Model.sys_product)dataGrid1.SelectedItem;
            int row_id = sys_Prdt.Row_id;
            if (row_id >= 0)
            { 
           if(MessageBox.Show("确定删除索引:"+row_id+"的产品","删除",MessageBoxButton.OKCancel,MessageBoxImage.Warning,MessageBoxResult.Cancel)==MessageBoxResult.OK)
                {


                    if (Globalvariable.industrialBLL.delete_products_data(row_id))
                    {
                        MessageBox.Show("删除成功!..","sucess");
                        this.dataGrid1.ItemsSource = Globalvariable.industrialBLL.Get_products_data();

                    }
                    else
                    {


                        MessageBox.Show("删除失败!..","Fail");
                        this.dataGrid1.ItemsSource = Globalvariable.industrialBLL.Get_products_data();

                    }

                }
            
            }


        }

        private void bt_add(object sender, RoutedEventArgs e)
        {
            Globalvariable.product_Dialog = new dialog.Product_dialog();
            if (Globalvariable.product_Dialog.ShowDialog() == true)
            { 
            
            }

        }
    }
}
