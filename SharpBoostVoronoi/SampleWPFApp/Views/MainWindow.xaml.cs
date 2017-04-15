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
            List<SharpBoostVoronoi.Output.Vertex> ov = new List<SharpBoostVoronoi.Output.Vertex>();
            for (long i = 0; i < gData.VoronoiSolution.CountVertices; i++)
                ov.Add(gData.VoronoiSolution.GetVertex(i));


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


            for (long edgeIndex = 0; edgeIndex < gData.VoronoiSolution.CountEdges; edgeIndex++)
            {
                Edge outputSegment = gData.VoronoiSolution.GetEdge(edgeIndex);
                if (!outputSegment.IsFinite)
                    continue;

                Vertex start = gData.VoronoiSolution.GetVertex(outputSegment.Start);
                Vertex end = gData.VoronoiSolution.GetVertex(outputSegment.End);

                if (outputSegment.IsLinear)
                {


                    DrawingArea.Children.Add(new Line()
                    {
                        X1 = start.X,
                        Y1 = start.Y,
                        X2 = end.X,
                        Y2 = end.Y,
                        Stroke = OutputStroke
                    });

                    DrawPoint(start.X, start.Y, OutputPointColoBrush, outputPointWidth, outputPointRadius);
                    DrawPoint(end.X, end.Y, OutputPointColoBrush, outputPointWidth, outputPointRadius);
                }
                else
                {
                    List<Vertex> discretizedEdge = gData.VoronoiSolution.SampleCurvedEdge(outputSegment, 2);
                    for (int i = 1; i < discretizedEdge.Count; i++)
                    {
                        DrawingArea.Children.Add(new Line()
                        {
                            X1 = discretizedEdge[i - 1].X,
                            Y1 = discretizedEdge[i - 1].Y,
                            X2 = discretizedEdge[i].X,
                            Y2 = discretizedEdge[i].Y,
                            Stroke = OutputStroke
                        });

                        DrawPoint(start.X, start.Y, OutputPointColoBrush, outputPointWidth, outputPointRadius);
                        DrawPoint(end.X, end.Y, OutputPointColoBrush, outputPointWidth, outputPointRadius);
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
