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

            DrawingArea.Background = Brushes.Aqua;
            GraphData gData = vm.Graphs[(sender as ComboBox).SelectedIndex];
            foreach (var inputSegment in gData.InputSegments)
            {
                DrawingArea.Children.Add(new Line()
                {
                    X1 = inputSegment.Start.X,
                    Y1 = inputSegment.Start.Y,
                    X2 = inputSegment.End.X,
                    Y2 = inputSegment.End.Y,
                    Stroke = System.Windows.Media.Brushes.DarkRed
                });
            }

            List<SharpBoostVoronoi.Output.Vertex> ov =  gData.OutputVertices;
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
            }
        }
    }
}
