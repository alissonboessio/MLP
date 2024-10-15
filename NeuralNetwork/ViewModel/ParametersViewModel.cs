using NeuralNetwork.Objects;
using NeuralNetwork.Objects.MLP;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;

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
                    if (value != null)
                    {
                        TrainInput = ProcessFile(value);
                    }
                    _trainFilePath = value;
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
                    if (value != null)
                    {
                        TrainOutput = ProcessFile(value);
                    }
                    _trainDummiesFilePath = value;
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
                    if (value != null)
                    {
                        TestInput = ProcessFile(value);
                    }
                    _testFilePath = value;
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
                    if (value != null)
                    {
                        TestOutput = ProcessFile(value);
                    }
                    _testDummiesFilePath = value;
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

        private double _trainingProgress;
        public double TrainingProgress
        {
            get => _trainingProgress;
            set
            {
                _trainingProgress = value;
                OnPropertyChanged(nameof(TrainingProgress));
            }
        }

        private bool _isTraining;
        public bool IsTraining
        {
            get => _isTraining;
            set
            {
                _isTraining = value;
                OnPropertyChanged(nameof(IsTraining));
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
            IsTraining = true;

            List<int> layersLocal = new List<int>();

            layersLocal.Add(TrainInput.FirstOrDefault().Count); // camada inicial (qtde neuronios = qtde colunas)
            layersLocal.AddRange(Layers.Select(li => li.QtyNeurons).ToList());
            layersLocal.Add(TrainOutput.FirstOrDefault().Count); // camada final (qtde neuronios = qtde de colunas dummy)

            mlp = new MLP(layersLocal);

            var progress = new Progress<double>(value =>
            {
                TrainingProgress = value;
            });

            Task.Run(() =>
            {
                TrainingProgress = 0;
                mlp.Train(TrainInput, TrainOutput, Iterations, (double)LearningRate, progress);
                IsTraining = false; 
                IsTrained = true;
            });

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
                    if (double.TryParse(column, NumberStyles.Any, CultureInfo.InvariantCulture, out double columnParsed))
                    {
                        row.Add(columnParsed);

                    }else
                    {
                        row.Add(0.0);

                    }
                }

                processedFile.Add(row);
            }          

            return processedFile;

        }

        public void SaveTestReturnToTXT(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"TotalCases: {TestReturnMLP.TotalCases}");
                writer.WriteLine($"QtyCorrect: {TestReturnMLP.QtyCorrect}");
                writer.WriteLine($"QtyWrong: {TestReturnMLP.QtyWrong}");
                writer.WriteLine($"Accuracy: {TestReturnMLP.Accuracy}");

                writer.WriteLine("Precision");
                foreach (var item in TestReturnMLP.Precision)
                {
                    writer.WriteLine($"{item.Key}: {item.Value}");
                }
            }
        }
        public void SaveListToCsvFile(List<List<double>> data, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var row in data)
                {
                    var rowString = string.Join(",", row);
                    writer.WriteLine(rowString);
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
