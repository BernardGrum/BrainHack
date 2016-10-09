using Arction.WPF.LightningChartUltimate;
using Arction.WPF.LightningChartUltimate.Axes;
using Arction.WPF.LightningChartUltimate.SeriesXY;
using Arction.WPF.LightningChartUltimate.Views.ViewXY;
using NIKBCI.Common;
using NIKBCI.Keyboard;
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

namespace NIKBCI.WpfTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        INBDevice device;
        ViewModel VM;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            device = new NBKeyboard();
            device.ActionArrived += device_ActionArrived;
            device.StartEverything();
            VM = new ViewModel();
            VM.ChartPointCollection = new System.Collections.ObjectModel.ObservableCollection<NBDataItem>();

            DataContext = VM;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            device.StopEverything();
        }

        void device_ActionArrived(object sender, NBResult e)
        {
            txtBox.Text += e.ToString() + Environment.NewLine;
            txtBox.ScrollToEnd();
        }

        private async void Train_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;
            string actionName = (sender as Button).Tag.ToString();
            NBAction action = (NBAction)Enum.Parse(typeof(NBAction), actionName, true);

            string str = await device.TrainActionAsync(action);

            MessageBox.Show(str);
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            device.LoadTrainedData(null);
            foreach (var akt in ((sender as Button).Parent as Grid).Children)
            {
                if (akt is Button)
                {
                    (akt as Button).IsEnabled = false;
                }
            }
        }

        private void Testme_Click(object sender, RoutedEventArgs e)
        {
            NBDataReader reader = new NBDataReader();
            List<NBDataItem> items = reader.GetDataItems();
            MessageBox.Show(items.Count.ToString());

            VM.ChartPointCollection = new System.Collections.ObjectModel.ObservableCollection<NBDataItem>(items);

            //Initialize chart
            myChart.BeginUpdate();
            //Get XY view
            ViewXY chartView = myChart.ViewXY;
            //Get default x-axis and set the range and ValueType
            AxisX axisX = chartView.XAxes[0];
            axisX.SetRange((double)items.Min(x => x.TimeStamp), (double)items.Max(x => x.TimeStamp));
            axisX.ValueType = AxisValueType.Number;
            //Get default y-axis and set the range.
            AxisY axisY = chartView.YAxes[0];
            axisY.SetRange((double)items.Min(x => x.Channels.Min()), (double)items.Max(x => x.Channels.Max()));
            //Add point line series
            for (int i = 0; i < reader.Channels; i++)
            {
                PointLineSeries pls = new PointLineSeries(chartView, axisX, axisY);
                pls.PointsVisible = false;
                SeriesPoint[] aPoints = new SeriesPoint[items.Count];
                for (int iPoint = 0; iPoint < items.Count; iPoint++)
                {
                    aPoints[iPoint].X = items[iPoint].TimeStamp;
                    aPoints[iPoint].Y = items[iPoint].Channels[i];
                }
                //Assign the data for the point line series
                pls.Points = aPoints;
                pls.Title = new Arction.WPF.LightningChartUltimate.Titles.SeriesTitle();
                pls.Title.Text = "CHANNEL " + i;
                //Add series to the PointLineSeries container in the view
                chartView.PointLineSeries.Add(pls);
            }
            //Apply chart property changes, which causes chart to be painted
            myChart.EndUpdate();

            MessageBox.Show("OK");
        }

    }
}
