using NeuralNetwork.Objects;
using NeuralNetwork.Objects.MLP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace NeuralNetwork.ViewModel
{
    public class ParametersViewModel : INotifyPropertyChanged
    {

        public ParametersViewModel()
        {
            Layers = new ObservableCollection<LayerItem>();

            Layers.CollectionChanged += (s, e) =>
            {
                for (int i = 0; i < Layers.Count; i++)
                {
                    Layers[i].Index = i + 1;
                }
            };
        }

        private MLP mlp;

        private int _iterations;
        public int Iterations
        {
            get => _iterations;
            set
            {
                if (_iterations != value)
                {
                    _iterations = value;
                    OnPropertyChanged(nameof(Iterations));
                }
            }
        }

        private decimal _learningRate;
        public decimal LearningRate
        {
            get => _learningRate;
            set
            {
                if (_learningRate != value)
                {
                    _learningRate = value;
                    OnPropertyChanged(nameof(LearningRate));
                }
            }
        }

        private decimal _testThreshold = 0.5M; 
        public decimal TestThreshold
        {
            get => _testThreshold;
            set
            {
                if (_testThreshold != value)
                {
                    _testThreshold = value;
                    OnPropertyChanged(nameof(TestThreshold));
                }
            }
        }

        #region Train Files

        private string _trainFilePath;
        public string TrainFilePath
        {
            get => _trainFilePath;
            set
            {
                if (_trainFilePath != value)
                {
                    _trainFilePath = value;
                    if (_trainFilePath != null)
                    {
                        TrainInput = ProcessFile(TrainFilePath);
                    }
                    OnPropertyChanged(nameof(TrainFilePath));
                }
            }
        }

        private string _trainDummiesFilePath;
        public string TrainDummiesFilePath
        {
            get => _trainDummiesFilePath;
            set
            {
                if (_trainDummiesFilePath != value)
                {
                    _trainDummiesFilePath = value;
                    if (_trainDummiesFilePath != null)
                    {
                        TrainOutput = ProcessFile(TrainDummiesFilePath);
                    }
                    OnPropertyChanged(nameof(TrainDummiesFilePath));
                }
            }
        }

        public List<List<double>> TrainInput { get; set; } 
        public List<List<double>> TrainOutput { get; set; }

        #endregion

        #region Test Files

        private string _testFilePath;
        public string TestFilePath
        {
            get => _testFilePath;
            set
            {
                if (_testFilePath != value)
                {
                    _testFilePath = value;
                    if (_testFilePath != null)
                    {
                        TestInput = ProcessFile(TestFilePath);
                    }
                    OnPropertyChanged(nameof(TestFilePath));
                }
            }
        }

        private string _testDummiesFilePath;
        public string TestDummiesFilePath
        {
            get => _testDummiesFilePath;
            set
            {
                if (_testDummiesFilePath != value)
                {
                    _testDummiesFilePath = value;
                    if (_testDummiesFilePath != null)
                    {
                        TestOutput = ProcessFile(TestDummiesFilePath);
                    }
                    OnPropertyChanged(nameof(TestDummiesFilePath));
                }
            }
        }

        public List<List<double>> TestInput { get; set; }
        public List<List<double>> TestOutput { get; set; }

        #endregion

        private bool _isTrained;
        public bool IsTrained
        {
            get => _isTrained;
            set
            {
                if (_isTrained != value)
                {
                    _isTrained = value;
                    OnPropertyChanged(nameof(IsTrained));
                }
            }
        }

        private TestReturn _testReturnMLP;
        public TestReturn TestReturnMLP
        {
            get => _testReturnMLP;
            set
            {
                if (_testReturnMLP != value)
                {
                    _testReturnMLP = value;
                    OnPropertyChanged(nameof(TestReturnMLP));
                }
            }
        }

        // camada de entrada e saida o usuario nao pode modificar, a de saida é pelo dummies
        private ObservableCollection<LayerItem> _layers;
        public ObservableCollection<LayerItem> Layers
        {
            get => _layers;
            set
            {
                if (_layers != value)
                {
                    _layers = value;
                    OnPropertyChanged(nameof(Layers));
                }
            }
        }
        public void Train()
        {
            IsTrained = false;
            TestReturnMLP = null;

            List<int> layersLocal = new List<int>();

            layersLocal.Add(TrainInput.FirstOrDefault().Count); // camada inicial (qtde neuronios = qtde colunas)
            layersLocal.AddRange(Layers.Select(li => li.QtyNeurons).ToList());
            layersLocal.Add(TrainOutput.FirstOrDefault().Count); // camada final (qtde neuronios = qtde de colunas dummy)

            mlp = new MLP(layersLocal);

            mlp.Train(TrainInput, TrainOutput, Iterations, (double)LearningRate);

            IsTrained = true;

        }

        public void Test()
        {

            TestReturnMLP = mlp.Test(TestInput, TestOutput, (double)TestThreshold);

        }

        public List<List<double>> ProcessFile(string filePath)
        {
            List<List<double>> processedFile = new List<List<double>>();

            List<string> lines = File.ReadAllLines(filePath).ToList();
            lines.RemoveAt(0);

            foreach (var line in lines)
            {
                var columns = line.Split(new[] { "," }, StringSplitOptions.None);
                List<double> row = new List<double>();

                foreach (var column in columns)
                {
                    row.Add(double.Parse(column));
                }

                processedFile.Add(row);
            }

            return processedFile;

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
