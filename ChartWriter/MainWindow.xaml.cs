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
using System.Windows.Threading;

namespace ChartWriter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const double MinMV = 0.0;
        public const double MaxMV = 100.0;
        public const double cBorderTop = 0.05; //%
        public const double cBorderBottom = 0.05; //%
        public const double cBorderLeft = 0.15; //%
        public const double cBorderRight = 0.05; //%
        public DispatcherTimer SamplingTimer { get; set; }
        public Random MV_Generator { get; set; }
        public List<double> MVList { get; set; }
        public Polyline? plGraph { get; set; } = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MV_Generator = new Random();
            MVList = new List<double>();

            SamplingTimer = new DispatcherTimer() {
                Interval = new TimeSpan(0, 0, 0, 0, 500),
                IsEnabled = false
            };
            SamplingTimer.Tick += SamplingTimer_Tick;
            SamplingTimer.Start();
        }

        private void SamplingTimer_Tick(object? sender, EventArgs e)
        {
            Double mv = MV_Generator.NextDouble() * 100;
            MVList.Add(mv);

            PointCollection chartPoints = new PointCollection();

            for (int i = 0; i < MVList.Count; i++)
            {
                chartPoints.Add(new Point(i * 5, MV2PixY(MVList[i])));
            }

            if (plGraph != null)
            {
                canvasChart.Children.Remove(plGraph);
            }

            plGraph = new Polyline()
            {
                Stroke = Brushes.Yellow,
                Points = chartPoints
            };

            canvasChart.Children.Add(plGraph);
        }

        private double MV2PixY(double mv)
        {
            double percent = (mv - MinMV) / (MaxMV - MinMV);
            return canvasChart.Height - (canvasChart.Height * cBorderBottom + canvasChart.Height * (1.0 - cBorderBottom - cBorderTop) * percent);
        }
    }
}
