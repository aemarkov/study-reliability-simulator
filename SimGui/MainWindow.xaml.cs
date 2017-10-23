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
using GraphX.Controls;
using GraphX.PCL.Common.Enums;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;
using GraphX.PCL.Logic.Algorithms.OverlapRemoval;
using SimGui.Graph;

namespace SimGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Set minimap (overview) window to be visible by default
            ZoomControl.SetViewFinderVisibility(zoomctrl, Visibility.Visible);

            //Lets setup GraphArea settings
            GraphAreaExample_Setup();

        }

        private GraphExample GraphExample_Setup()
        {
            var dataGraph = new GraphExample();
            
            for (int i = 1; i < 100; i++)
            {
                var dataVertex = new DataVertex(i.ToString());
                dataGraph.AddVertex(dataVertex);
            }


            var vlist = dataGraph.Vertices.ToList();

            Random Rand = new Random();
            foreach (var item in vlist)
            {
                if (Rand.Next(0, 50) > 25) continue;
                var vertex2 = vlist[Rand.Next(0, dataGraph.VertexCount - 1)];
                dataGraph.AddEdge(new DataEdge(item, vertex2, Rand.Next(1, 50))
                    { Text = $"{item} -> {vertex2}"});
            }

            
            return dataGraph;
        }

        private void GraphAreaExample_Setup()
        {
            // Настройки лейоута - какие алгоритмы импользуются, чтобы оптимально расположить граф
            var logicCore = new GXLogicCoreExample() { Graph = GraphExample_Setup() };
            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;
            logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);
            ((KKLayoutParameters)logicCore.DefaultLayoutAlgorithmParams).MaxIterations = 100;

            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            logicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            logicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;

            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;
            logicCore.AsyncAlgorithmCompute = false;
            Area.LogicCore = logicCore;

            // Генерируруем граф
            Area.GenerateGraph(true, true);

            // Настройки            
            Area.ShowAllEdgesArrows(false);
            Area.ShowAllEdgesLabels(false);
            zoomctrl.ZoomToFill();
        }
    }
}
