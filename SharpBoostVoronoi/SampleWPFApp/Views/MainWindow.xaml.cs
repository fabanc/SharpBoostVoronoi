using SampleWPFApp.Models;
using SampleWPFApp.ViewModels;
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
using SharpBoostVoronoi.Output;

namespace SampleWPFApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GraphViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new GraphViewModel();
            DataContext = vm;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Clear
            DrawingArea.Children.Clear();

            //Set colors used by points
            SolidColorBrush inputPointColoBrush = new SolidColorBrush();
            inputPointColoBrush.Color = Color.FromArgb(255, 255, 255, 0);

            SolidColorBrush outputPointColoBrush = new SolidColorBrush();
            outputPointColoBrush.Color = Color.FromArgb(255,0,0,255);

            int inputPointWidth = 12;
            int inputPointRadius = Convert.ToInt32(inputPointWidth / 2);

            int outputPointWidth = 6;
            int outputPointRadius = Convert.ToInt32(outputPointWidth / 2);

            //Get the graph data
            GraphData gData = vm.Graphs[(sender as ComboBox).SelectedIndex];
            List<SharpBoostVoronoi.Output.Vertex> ov = gData.OutputVertices;

            foreach (var inputSegment in gData.InputSegments)
            {

                //Draw the input segment
                DrawingArea.Children.Add(new Line()
                {
                    X1 = inputSegment.Start.X,
                    Y1 = inputSegment.Start.Y,
                    X2 = inputSegment.End.X,
                    Y2 = inputSegment.End.Y,
                    Stroke = System.Windows.Media.Brushes.DarkRed
                });


                //Draw the end points
                DrawPoint(inputSegment.Start.X, inputSegment.Start.Y, inputPointColoBrush, inputPointWidth, inputPointRadius);
                DrawPoint(inputSegment.End.X, inputSegment.End.Y, inputPointColoBrush, inputPointWidth, inputPointRadius);

            }

            
            foreach (var outputSegment in gData.OutputEdges)
            {
                if (outputSegment.Start == -1 || outputSegment.End == -1)
                    continue;
                
                DrawingArea.Children.Add(new Line()
                {
                    X1 = ov[outputSegment.Start].X,
                    Y1 = ov[outputSegment.Start].Y,
                    X2 = ov[outputSegment.End].X,
                    Y2 = ov[outputSegment.End].Y,
                    Stroke = System.Windows.Media.Brushes.DarkBlue
                });

                DrawPoint(ov[outputSegment.Start].X, ov[outputSegment.Start].Y, outputPointColoBrush, outputPointWidth, outputPointRadius);
                DrawPoint(ov[outputSegment.End].X, ov[outputSegment.End].Y, outputPointColoBrush, outputPointWidth, outputPointRadius);
            }
        }

        private void DrawPoint(double x, double y, SolidColorBrush mySolidColorBrush, int pointWidth, int pointRadius)
        {
            var inputStart = new Ellipse();
            inputStart.StrokeThickness = 2;
            inputStart.Stroke = Brushes.Black;
            inputStart.Width = pointWidth;
            inputStart.Height = pointWidth;
            inputStart.Fill = mySolidColorBrush;
            Canvas.SetLeft(inputStart, x - pointRadius);
            Canvas.SetTop(inputStart, y - pointRadius);
            DrawingArea.Children.Add(inputStart);
        }
    }
}
