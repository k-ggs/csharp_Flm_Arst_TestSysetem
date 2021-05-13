using Flm_Arst_TestSysetem.BLL;
using Flm_Arst_TestSysetem.PCIE_1816H;
using Flm_Arst_TestSysetem.View;
using Flm_Arst_TestSysetem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flm_Arst_TestSysetem.tool;
using Flm_Arst_TestSysetem.dialog;

namespace Flm_Arst_TestSysetem
{
 public  static  class Globalvariable
    {
        public static SystemMonitor SystemMonitor = new SystemMonitor();

        public static ReportManagement reportManagement = new ReportManagement();
        public static SystemMonitorViewModel systemMonitorViewModel=new SystemMonitorViewModel();
        public static hIS_TORY_VIE systemMonitorViewModel2;
        public static PCI_1816H_API pCI_1816H_API = new PCI_1816H_API();

        public static IndustrialBLL industrialBLL = new IndustrialBLL();
        public static string word_temp_p = @"./word_temp/word_temp.docx";
        public static string savefile = @"./Save_Path/word_temp.docx";
        public static WordUtility WordUtility = new WordUtility(word_temp_p, savefile);

        public static reports_page reports_Page = new reports_page();

        public static User_add user_Add = new User_add();
        public static Product_dialog product_Dialog = new Product_dialog();
    }
}
