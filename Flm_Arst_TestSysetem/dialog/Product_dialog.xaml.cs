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
using System.Windows.Shapes;

namespace Flm_Arst_TestSysetem.dialog
{
    /// <summary>
    /// Product_dialog.xaml 的交互逻辑
    /// </summary>
    public partial class Product_dialog : Window
    {
        public Product_dialog()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.gas_group.ItemsSource = Globalvariable.industrialBLL.Get_gas_group();
            this.Test_standard.ItemsSource = Globalvariable.industrialBLL.Get_teststandard();
        }

        private void bt_cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            
        }

        private void bt_ok(object sender, RoutedEventArgs e)
        {
            if (   String.IsNullOrEmpty(txt_product_no.Text)
                || String.IsNullOrEmpty(txt_product_Bitno.Text)
                || String.IsNullOrEmpty(txt_model.Text)
                || String.IsNullOrEmpty(txt_spec.Text)
                || String.IsNullOrEmpty(gas_group.Text)
                || String.IsNullOrEmpty(Test_standard.Text))
            {
                MessageBox.Show("输入内容不能为空!...");
                return;

            }
            else
            {
                if (
            Globalvariable.industrialBLL.Insert_products_data(new Model.sys_product() { Product_No = txt_product_no.Text.ToUpper(), Product_BitNo = txt_product_Bitno.Text.ToUpper(), Product_Models = txt_model.Text.ToUpper(), GasGroup_No = gas_group.SelectedValue.ToString(), Product_Spcfts = txt_spec.Text, Teststandard_No = Test_standard.SelectedValue.ToString(), Remake = txt_mark.Text, Create_time = DateTime.Now.ToString(DateTime.Now.ToString("yyyyMMddHHmmss")) })
            )
                {
                    MessageBox.Show("已成功新增!...", "Sucess");

                }
                else
                {

                    MessageBox.Show("新增失败!...", "Sucess");
                }
            }


        }
    }
}
