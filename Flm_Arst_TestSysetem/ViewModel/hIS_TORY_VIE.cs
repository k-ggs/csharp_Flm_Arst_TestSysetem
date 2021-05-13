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
    public class hIS_TORY_VIE : NotifyPropertyBase
    {








        private SeriesCollection _seriesViews;
        private SeriesCollection _seriesViews_press;
        public SeriesCollection H_seriesViews { get { return _seriesViews; } set { _seriesViews = value; this.RaisePropertyChanged(); } }

        public SeriesCollection H_seriesViews_press { get { return _seriesViews_press; } set { _seriesViews_press = value; this.RaisePropertyChanged(); } }




        #region 历史记录
        public static GLineSeries H_LineSeries1 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low),Title = "火焰传感器1", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };
            public static GLineSeries H_LineSeries2 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "火焰传感器2", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };
            public static GLineSeries H_LineSeries3 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "火焰传感器3", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };
            public static GLineSeries H_LineSeries4 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "火焰传感器4", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };
            public static GLineSeries H_LineSeries5 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low),Title = "火焰传感器5", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };
            public static GLineSeries H_LineSeries6 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "火焰传感器6", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };
            public static GLineSeries H_LineSeries7 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "火焰传感器7", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };
            public static GLineSeries H_LineSeries8 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>().WithQuality(Quality.Low), Title = "火焰传感器8", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };

            public static GLineSeries H_pressure1 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>(), Title = "压力传感器1", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };
            public static GLineSeries H_pressure2 = new GLineSeries { StrokeThickness = 2, Values = new GearedValues<double>(), Title = "压力传感器2", PointGeometry = null, Fill = Brushes.Transparent, LineSmoothness = 0 };

            #endregion

            

          //  public SeriesCollection H_seriesViews { get; set; } = new SeriesCollection { H_LineSeries1, H_LineSeries2, H_LineSeries3, H_LineSeries4,
           //                                                                        H_LineSeries5, H_LineSeries6, H_LineSeries7, H_LineSeries8};

         //   public SeriesCollection H_seriesViews_press { get; set; } = new SeriesCollection { H_pressure1, H_pressure2 };


         
          



         

          

       

          

          

         
            

           
    }

}

