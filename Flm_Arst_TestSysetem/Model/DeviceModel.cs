using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flm_Arst_TestSysetem.Base;

namespace Flm_Arst_TestSysetem.Model
{
    public class DeviceModel : NotifyPropertyBase
    {
        public string DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string _TestName;

     
        public string TestName
        {
            get { return _TestName; }
            set { this._TestName = value; this.RaisePropertyChanged(); }
        }

        private bool _isRunning=false;
        private bool _isRecord=false;
        public bool IsRecord {
            get 
            {return _isRecord; } set  
            { 
                _isRecord = value; this.RaisePropertyChanged(); } }

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                this.RaisePropertyChanged();
            }
        }

        private bool _isWarning = false;

        public bool IsWarning
        {
            get { return _isWarning; }
            set
            {
                _isWarning = value;
                this.RaisePropertyChanged();
            }
        }


        public ObservableCollection<MonitorValueModel> MonitorValueList { get; set; } = new ObservableCollection<MonitorValueModel>();

        public ObservableCollection<WarningMessageModel> WarningMessageList { get; set; } = new ObservableCollection<WarningMessageModel>();
      
    }
}
