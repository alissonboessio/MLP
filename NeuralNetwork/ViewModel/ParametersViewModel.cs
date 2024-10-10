﻿using NeuralNetwork.Objects;
using NeuralNetwork.Objects.MLP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private string _dataFilePath;
        public string DataFilePath
        {
            get => _dataFilePath;
            set
            {
                if (_dataFilePath != value)
                {
                    _dataFilePath = value;
                    OnPropertyChanged(nameof(DataFilePath));
                }
            }
        }


        private string _outputFilePath;
        public string OutputFilePath
        {
            get => _outputFilePath;
            set
            {
                if (_outputFilePath != value)
                {
                    _outputFilePath = value;
                    OnPropertyChanged(nameof(OutputFilePath));
                }
            }
        }


        private string _testFilePath;
        public string TestFilePath
        {
            get => _testFilePath;
            set
            {
                if (_testFilePath != value)
                {
                    _testFilePath = value;
                    OnPropertyChanged(nameof(TestFilePath));
                }
            }
        }

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

        public List<List<double>> TrainInput { get; set; } = new List<List<double>>
            {
                new List<double> { 0, 0, 0, 1, 1, 1, 1 },
                new List<double> { 0, 1, 0, 0, 1, 0 ,1 },
                new List<double> { 1, 0, 1, 0, 1, 0 ,1 },
                new List<double> { 1, 1, 0, 0, 0, 0, 1 }
            };
        public List<List<double>> TrainOutput { get; set; } = new List<List<double>>
            {
                new List<double> { 0, 1 },
                new List<double> { 1, 0 },
                new List<double> { 1, 0 },
                new List<double> { 0, 1 }
            };
        public List<List<double>> TestInput { get; set; } = new List<List<double>>
            {
                new List<double> { 0, 0, 0, 1, 1, 1, 1 },
                new List<double> { 0, 1, 0, 0, 1, 0 ,1 },
                new List<double> { 1, 0, 1, 0, 1, 0 ,1 },
                new List<double> { 1, 1, 0, 0, 0, 0, 1 }
            };
        public List<List<double>> TestOutput { get; set; } = new List<List<double>>
            {
                new List<double> { 0, 1 },
                new List<double> { 1, 0 },
                new List<double> { 1, 0 },
                new List<double> { 0, 1 }
            };

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

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
