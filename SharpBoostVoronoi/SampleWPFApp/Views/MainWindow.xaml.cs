using SampleWPFApp.Models;
using SampleWPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        //Set the colors being used by application
        public SolidColorBrush InputPointColoBrush {get; set; }
        public SolidColorBrush OutputPointColoBrush { get; set; }
        public Brush InputStroke { get; set; }
        public Brush OutputStroke { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            vm = new GraphViewModel();
            DataContext = vm;

            //Set colors used by points
            InputPointColoBrush = this.Resources["InputPointColorBrush"] as SolidColorBrush;
            OutputPointColoBrush = this.Resources["OutputPointColoBrush"] as SolidColorBrush;

            BrushConverter converter = new System.Windows.Media.BrushConverter();

            string inputStrokeColor = converter.ConvertToString(InputPointColoBrush);
            string outputStrokeColor = converter.ConvertToString(OutputPointColoBrush);

            InputStroke = (Brush)converter.ConvertFromString(inputStrokeColor);
            OutputStroke = (Brush)converter.ConvertFromString(outputStrokeColor);


        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Clear
            DrawingArea.Children.Clear();

            int inputPointWidth = 12;
            int inputPointRadius = Convert.ToInt32(inputPointWidth / 2);

            int outputPointWidth = 6;
            int outputPointRadius = Convert.ToInt32(outputPointWidth / 2);

            //Get the graph data
            GraphData gData = vm.Graphs[(sender as ComboBox).SelectedIndex];
            List<SharpBoostVoronoi.Output.Vertex> ov = gData.VoronoiSolution.Vertices;


            foreach (var inputPoint in gData.VoronoiSolution.InputPoints)
                DrawPoint(inputPoint.X, inputPoint.Y, InputPointColoBrush, inputPointWidth, inputPointRadius);

            foreach (var inputSegment in gData.VoronoiSolution.InputSegments)
            {

                //Draw the input segment
                DrawingArea.Children.Add(new Line()
                {
                    X1 = inputSegment.Start.X,
                    Y1 = inputSegment.Start.Y,
                    X2 = inputSegment.End.X,
                    Y2 = inputSegment.End.Y,
                    Stroke = InputStroke
                });


                //Draw the end points
                DrawPoint(inputSegment.Start.X, inputSegment.Start.Y, InputPointColoBrush, inputPointWidth, inputPointRadius);
                DrawPoint(inputSegment.End.X, inputSegment.End.Y, InputPointColoBrush, inputPointWidth, inputPointRadius);

            }

            
            foreach (var outputSegment in gData.VoronoiSolution.Edges)
            {
                //if (outputSegment.Start == -1 || outputSegment.End == -1)
                if (!outputSegment.IsFinite)
                    continue;

                if (outputSegment.IsLinear)
                {
                    DrawingArea.Children.Add(new Line()
                    {
                        X1 = ov[outputSegment.Start].X,
                        Y1 = ov[outputSegment.Start].Y,
                        X2 = ov[outputSegment.End].X,
                        Y2 = ov[outputSegment.End].Y,
                        Stroke = OutputStroke
                    });

                    DrawPoint(ov[outputSegment.Start].X, ov[outputSegment.Start].Y, OutputPointColoBrush, outputPointWidth, outputPointRadius);
                    DrawPoint(ov[outputSegment.End].X, ov[outputSegment.End].Y, OutputPointColoBrush, outputPointWidth, outputPointRadius);
                }
                else
                {
                    List<Vertex> discretizedEdge = gData.VoronoiSolution.SampleCurvedEdge(outputSegment);
                    for (int i = 1; i < discretizedEdge.Count; i++)
                    {
                    DrawingArea.Children.Add(new Line()
                    {
                        X1 = discretizedEdge[i-1].X,
                        Y1 = discretizedEdge[i-1].Y,
                        X2 = discretizedEdge[i].X,
                        Y2 = discretizedEdge[i].Y,
                        Stroke = OutputStroke
                    });
                
                    DrawPoint(ov[outputSegment.Start].X, ov[outputSegment.Start].Y, OutputPointColoBrush, outputPointWidth, outputPointRadius);
                    DrawPoint(ov[outputSegment.End].X, ov[outputSegment.End].Y, OutputPointColoBrush, outputPointWidth, outputPointRadius);                        
                    }
                }
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
