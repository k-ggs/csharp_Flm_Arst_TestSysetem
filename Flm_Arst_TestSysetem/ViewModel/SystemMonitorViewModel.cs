using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flm_Arst_TestSysetem.Base;
using Flm_Arst_TestSysetem.Model;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Geared;
using System.Windows.Media;
using LiveCharts.Configurations;

namespace Flm_Arst_TestSysetem.ViewModel
{
    public class MeasureModel
    {
        public double xvalue { get; set; }
        public double Value { get; set; }
    }
    public class SystemMonitorViewModel:NotifyPropertyBase
    {
      
        public static List<Model_data> Highdatas = new List<Model_data>();
        public Func<double, string> DateTimeFormatter { get; set; }
        public static GearedValues<MeasureModel> ChartValues { get; set; } = new GearedValues<MeasureModel>();
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }
     
        private double _min =0;
        public double Min
        {
            get { return _min; }
            set
            {
                _min = value;
                this.RaisePropertyChanged();
            }
        }

        private double _max;
        public double Max 
        {
            get { return _max; }
            set
            {
                _max = value;
                this.RaisePropertyChanged();
            }
        }
        public string Test_name = string.Empty;
        public string users_name = string.Empty;
        public string media = string.Empty;
        private int _data_count;
        public int data_count
        {
            get { return _data_count; }
            set
            {
                _data_count = value;
                this.RaisePropertyChanged();
            }
        }
        public double clock_rate;
        private int _data_C=0;
        public int data_C
        {
            get { return _data_C; }
            set
            {
                _data_C = value;
                this.RaisePropertyChanged();
            }
        }


        private string _txt_unit;
        public string txt_unit
        {
            get { return _txt_unit; }
            set
            {
                _txt_unit = value;
                this.RaisePropertyChanged();
            }
        }
        private int _mTexidx = 0;
        public int mTexidx
        {
            get { return _mTexidx; }
            set { _mTexidx = value; this.RaisePropertyChanged(); }
        }

        private int _S1;
        private int _S2;
        private int _S3;
        private int _S4;
        private int _S5;
        private int _S6;
        private int _S7;
        private double _p1_prec;
        private double _p2_prec;

        public double p1_prec
        {
            get { return _p1_prec; }
            set { _p1_prec = value; this.RaisePropertyChanged(); }
        }
        public double p2_prec
        {
            get { return _p2_prec; }
            set { _p2_prec = value; this.RaisePropertyChanged(); }
        }

        public int S1 { get { return _S1; } set { _S1 = value; this.RaisePropertyChanged(); } }
        public int S2 {get {return  _S2; } set { _S2 = value; this.RaisePropertyChanged(); } }
        public int S3 {
            get { return _S3; }
            set { _S3 = value; this.RaisePropertyChanged(); } }
        public int S4 {
            get { return _S4; }
            set { _S4 = value; this.RaisePropertyChanged(); } }
        public int S5 {
            get { return _S5; }
            set { _S5 = value; this.RaisePropertyChanged(); } }
        public int S6 {
            get { return _S6; }
            set { _S6 = value; this.RaisePropertyChanged(); } }

        public int S7
        {
            get { return _S7; }
            set { _S7 = value; this.RaisePropertyChanged(); }
        }



        public static GearedValues<double> x { get; set; } = new GearedValues<double>();
 
        public static GLineSeries LineSeries1 = new GLineSeries { StrokeThickness= 2,Values = new  GearedValues <double>().WithQuality(Quality.Low), Title = "火焰传感器1" ,PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0};
        public static GLineSeries LineSeries2 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "火焰传感器2", PointGeometry = null,Fill = Brushes.Transparent, LineSmoothness = 0};
        public static GLineSeries LineSeries3 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "火焰传感器3", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };
        public static GLineSeries LineSeries4 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "火焰传感器4", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0};
        public static GLineSeries LineSeries5 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "火焰传感器5", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0};
        public static GLineSeries LineSeries6 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "火焰传感器6", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };
        public static GLineSeries LineSeries7 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "火焰传感器7", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };
        public static GLineSeries LineSeries8 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "火焰传感器8", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };

        public static GLineSeries pressure1 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "压力传感器1", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0};
        public static GLineSeries pressure2 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "压力传感器2", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };



        //  = new SeriesCollection { LineSeries1, LineSeries2, LineSeries3, LineSeries4,
        //                                                                    LineSeries5, LineSeries6, LineSeries7, LineSeries8
        // };
        public ObservableCollection<LogModel> LogList { get; set; } = new ObservableCollection<LogModel>();
        private SeriesCollection _seriesViews;
        private SeriesCollection _seriesViews_press;
        public SeriesCollection  seriesViews { get { return _seriesViews; } set { _seriesViews = value; this.RaisePropertyChanged(); } }

        public SeriesCollection seriesViews_press { get { return _seriesViews_press; } set { _seriesViews_press = value; this.RaisePropertyChanged(); } }

        public ObservableCollection<LogModel> LogList_press { get; set; } = new ObservableCollection<LogModel>();
    //    public SeriesCollection seriesViews_press { get; set; } = new SeriesCollection { pressure1, pressure2 };
       
        private DeviceModel _currentDevice;
       

        public DeviceModel CurrentDevice
        {
            get { return _currentDevice; }
            set { _currentDevice = value; this.RaisePropertyChanged(); }
        }

        private bool _isShowDetail = true;

        public bool IsShowDetail
        {
            get { return _isShowDetail; }
            set { _isShowDetail = value; this.RaisePropertyChanged(); }
        }


        public DeviceModel TestDevice { get; set; }

        public CommandBase ComponentCommand { get; set; }
       

        public SystemMonitorViewModel()
        {
         //   DateTimeFormatter = value => new DateTime((long)value).ToString("mm:ss:ffffff");
          //  var mapper = Mappers.Xy<MeasureModel>()
          //  .X(model => model.xvalue)   //use DateTime.Ticks as X
          //  .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
       //     Charting.For<MeasureModel>(mapper);


            AxisUnit = TimeSpan.TicksPerSecond;
            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            InitLogInfo();

            #region 测试数据，为了设置数据模板
            TestDevice = new DeviceModel();
            TestDevice.DeviceName = "阻火器系统控制 1#";
            TestDevice.IsRunning = true;
            TestDevice.IsWarning = true;
            TestDevice.MonitorValueList.Add(new MonitorValueModel
            {
                
                ValueId = "1",
                ValueName = "火焰传感器1",
                Unit = "m",
                CurrentValue = 45,

               
                Values = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservableValue> { new LiveCharts.Defaults.ObservableValue(2), new LiveCharts.Defaults.ObservableValue(10) }
            }); 
            TestDevice.MonitorValueList.Add(new MonitorValueModel
            {
                ValueId = "1",
                ValueName = "火焰传感器2",
                Unit = "Mpa",
                CurrentValue = 34,
                Values = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservableValue> { new LiveCharts.Defaults.ObservableValue(1), new LiveCharts.Defaults.ObservableValue(10) }
            });
            TestDevice.MonitorValueList.Add(new MonitorValueModel
            {
                ValueId = "1",
                ValueName = "火焰传感器3",
                Unit = "℃",
                CurrentValue = 34,
                Values = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservableValue> { new LiveCharts.Defaults.ObservableValue(1), new LiveCharts.Defaults.ObservableValue(10) }
            });
            TestDevice.MonitorValueList.Add(new MonitorValueModel
            {
                ValueId = "1",
                ValueName = "火焰传感器4",
                Unit = "Mpa",
                CurrentValue = 34,
                Values = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservableValue> { new LiveCharts.Defaults.ObservableValue(1), new LiveCharts.Defaults.ObservableValue(10) }
            });
            TestDevice.MonitorValueList.Add(new MonitorValueModel
            {
                ValueId = "1",
                ValueName = "火焰传感器5",
                Unit = "Mpa",
                CurrentValue = 34,
                Values = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservableValue> { new LiveCharts.Defaults.ObservableValue(1), new LiveCharts.Defaults.ObservableValue(10) }
            });
            TestDevice.MonitorValueList.Add(new MonitorValueModel
            {
                ValueId = "1",
                ValueName = "火焰传感器6",
                Unit = "Mpa",
                CurrentValue = 34,
                Values = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservableValue> { new LiveCharts.Defaults.ObservableValue(1), new LiveCharts.Defaults.ObservableValue(10) }
            });
            TestDevice.MonitorValueList.Add(new MonitorValueModel
            {
                ValueId = "1",
                ValueName = "出口温度",
                Unit = "℃",
                CurrentValue = 34,
                Values = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservableValue> { new LiveCharts.Defaults.ObservableValue(1), new LiveCharts.Defaults.ObservableValue(10) }
            });
            TestDevice.MonitorValueList.Add(new MonitorValueModel
            {
                ValueId = "1",
                ValueName = "补水压力",
                Unit = "Mpa",
                CurrentValue = 34,
                Values = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservableValue> { new LiveCharts.Defaults.ObservableValue(1), new LiveCharts.Defaults.ObservableValue(10) }
            });

            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#液位极低，当前值：0" });
            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#入口压力极低，当前值：0" });
            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#入口温度极低，当前值：0" });

            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#液位极低，当前值：0" });
            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#入口压力极低，当前值：0" });
            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#入口温度极低，当前值：0" });
            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#液位极低，当前值：0" });
            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#入口压力极低，当前值：0" });
            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#入口温度极低，当前值：0" });
            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#液位极低，当前值：0" });
            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#入口压力极低，当前值：0" });
            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#入口温度极低，当前值：0" });
            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#液位极低，当前值：0" });
            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#入口压力极低，当前值：0" });
            TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = "系统#入口温度极低，当前值：0" });

            #endregion


            this.ComponentCommand = new CommandBase(new Action<object>(DoTowerCommand));
        }
        void InitLogInfo()
        {
            this.LogList.Add(new LogModel { RowNumber = 1, DeviceName = "火焰传感器 1#", LogInfo = "已检测到", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 2, DeviceName = "火焰传感器 2#", LogInfo = "已检测到", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 3, DeviceName = "火焰传感器 3#", LogInfo = "未检测到", LogType = Base.LogType.Warn });
            this.LogList.Add(new LogModel { RowNumber = 4, DeviceName = "火焰传感器 4#", LogInfo = "未检测到", LogType = Base.LogType.Warn });
            this.LogList.Add(new LogModel { RowNumber = 5, DeviceName = "火焰传感器 5#", LogInfo = "已检测到", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "火焰传感器 6#", LogInfo = "已检测到", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 5, DeviceName = "火焰传感器 7#", LogInfo = "已检测到", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "火焰传感器 8#", LogInfo = "已检测到", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 5, DeviceName = "压力传感器 1#", LogInfo = "已检测到", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "压力传感器 2#", LogInfo = "已检测到", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 5, DeviceName = "开关阀 1#", LogInfo = "已打开", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "开关阀 2#", LogInfo = "已打开", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "点火控制 1#", LogInfo = "已打开", LogType = Base.LogType.Info });

            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "压力传感器 2#", LogInfo = "已检测到", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 5, DeviceName = "开关阀 1#", LogInfo = "已打开", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "开关阀 2#", LogInfo = "已打开", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "点火控制 1#", LogInfo = "已打开", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "压力传感器 2#", LogInfo = "已检测到", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 5, DeviceName = "开关阀 1#", LogInfo = "已打开", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "开关阀 2#", LogInfo = "已打开", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "点火控制 1#", LogInfo = "已打开", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "压力传感器 2#", LogInfo = "已检测到", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 5, DeviceName = "开关阀 1#", LogInfo = "已打开", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "开关阀 2#", LogInfo = "已打开", LogType = Base.LogType.Info });
            this.LogList.Add(new LogModel { RowNumber = 6, DeviceName = "点火控制 1#", LogInfo = "已打开", LogType = Base.LogType.Info });
        }
        private int rowid = 0;
        public void LogInfoAdd(string deviceName, string logInfo, LogType logType,string msg="")
        {
            rowid++;

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                this.LogList.Add(new LogModel { RowNumber = rowid, DeviceName = deviceName, LogInfo = logInfo, LogType = logType });
            }));

            if (msg != "")
            {
                TestDevice.WarningMessageList.Add(new WarningMessageModel { Message = msg });


            }

        }

        private void DoTowerCommand(object param)
        {
            CurrentDevice = param as DeviceModel;
            this.IsShowDetail = true;
        }




        #region 20210429 数采界面属性配置
        private string _Product_Nos=string.Empty;
        private string _Product_BitNos = string.Empty;
        private string _Product_Models = string.Empty;
        private string _GasGroup_No = string.Empty;
        private string _Teststandard_No = string.Empty;
        private string _Remake = string.Empty;

        //试验参数
        private string _Media_names = string.Empty;
        private double _CBTB_G_C = 0;
        private double _Media_C = 0;
        private int _frequency = 0;
        private string _Test_Type_No = string.Empty;
        //试验数据
        private double _initial_pressure = 0;
        private double _explore_pressure = 0;
        private double _flame_V = 0;

        private int _test_maxRow_id =0;

        private double _Va_pressure = 0;
        private bool _FPB_RN = false;
        #endregion
        public int test_maxRow_id
        {
            get { return _test_maxRow_id; }
            set { _test_maxRow_id = value; RaisePropertyChanged(); }
        }
        public string Product_Nos   { 
            get { return _Product_Nos; }
            set { _Product_Nos = value; RaisePropertyChanged(); } 
        }
        public string Product_BitNos { 
            get { return _Product_BitNos; }
            set { _Product_BitNos = value; RaisePropertyChanged(); }
        }
        public string Product_Models { 
            get { return _Product_Models; } 
            set { _Product_Models = value; RaisePropertyChanged(); } 
        }
        public string GasGroup_No    { 
            get { return _GasGroup_No; } 
            set { _GasGroup_No = value; RaisePropertyChanged(); }
        }
        public string Teststandard_No {
            get { return _Teststandard_No; }
            set { _Teststandard_No = value; RaisePropertyChanged(); } 
        }
        public string Remake         {
            get { return _Remake; } 
            set { _Remake = value; RaisePropertyChanged(); }
        }
        public string Media_names    {
            get { return _Media_names; }
            set { _Media_names = value; RaisePropertyChanged(); }
        }
        public double CBTB_G_C      { 
            get { return _CBTB_G_C; } 
            set { _CBTB_G_C = value; RaisePropertyChanged(); }
        }
        public double Media_C       { 
            get { return _Media_C; }
            set { _Media_C = value; RaisePropertyChanged(); }
        }
        public int frequency     { 
            get { return _frequency; } 
            set { _frequency = value; RaisePropertyChanged(); }
        }
        public string Test_Type_No { 
            get { return _Test_Type_No; } 
            set { _Test_Type_No = value; RaisePropertyChanged(); } 
        }
    public double Initial_pressure {
            get { return _initial_pressure; } 
            set { _initial_pressure = value; RaisePropertyChanged(); } 
        }
    public double Explore_pressure { 
            get { return _explore_pressure; }
            set { _explore_pressure = value; RaisePropertyChanged(); }
        }
        public double Flame_V      { 
            get { return _flame_V; } 
            set { _flame_V = value; RaisePropertyChanged(); }
        }
        public double Va_pressure { 
            get { return _Va_pressure; }
            set { _Va_pressure = value; RaisePropertyChanged(); } }
        public bool FPB_RN        { 
            get { return _FPB_RN; } 
            set { _FPB_RN = value; RaisePropertyChanged(); } 
        }
    }
}
