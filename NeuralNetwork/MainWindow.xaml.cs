using NeuralNetwork.Objects.MLP;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeuralNetwork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public ParametersViewModel Parameters { get; set; }

        private void BT_TreinarRede_Click(object sender, RoutedEventArgs e)
        {
            // camada de entrada e saida o usuario nao pode modificar, a de saida é pelo dummies
            List<int> layerSizes = new List<int> { 7, 2, 2 };
            MLP mlp = new MLP(layerSizes);

            var trainingInputs = new List<List<double>>
            {
                new List<double> { 0, 0, 0, 1, 1, 1, 1 },
                new List<double> { 0, 1, 0, 0, 1, 0 ,1 },
                new List<double> { 1, 0, 1, 0, 1, 0 ,1 },
                new List<double> { 1, 1, 0, 0, 0, 0, 1 }
            };

            var trainingOutputs = new List<List<double>>
            {
                new List<double> { 0, 1 },  
                new List<double> { 1, 0 },  
                new List<double> { 1, 0 },  
                new List<double> { 0, 1 }   
            };

            //var trainingOutputs = new List<List<double>>
            //{
            //    new List<double> { 0 },  
            //    new List<double> { 1 },  
            //    new List<double> { 1 },  
            //    new List<double> { 1 }   
            //};

            // XOR
            //var trainingInputs = new List<List<double>>
            //{
            //    new List<double> { 0, 0 },
            //    new List<double> { 0, 1 },
            //    new List<double> { 1, 0 },
            //    new List<double> { 1, 1 }
            //};

            //var trainingOutputs = new List<List<double>>
            //{
            //    new List<double> { 0 },  
            //    new List<double> { 1 },  
            //    new List<double> { 1 },  
            //    new List<double> { 0 }   
            //};

            mlp.Train(trainingInputs, trainingOutputs, 10000, 0.5);

            foreach (var input in trainingInputs)
            {
                var output = mlp.ForwardPropagate(input);
                Console.WriteLine($"Input: {string.Join(",", input)} => Output: {string.Join(",", output)}");
            }

        }

        public int iterations { get; set; }
    }
}