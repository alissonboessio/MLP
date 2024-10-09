﻿using NeuralNetwork.Objects.MLP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class ParametersViewModel : INotifyPropertyChanged
    {
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

        private double _learningRate;
        public double LearningRate
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

        // camada de entrada e saida o usuario nao pode modificar, a de saida é pelo dummies
        private List<int> _layers;
        public List<int> Layers
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

            mlp = new MLP(Layers);

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

            mlp.Train(trainingInputs, trainingOutputs, Iterations, LearningRate);

            IsTrained = true;

        }

        public void Test()
        {

            var trainingInputs = new List<List<double>>
            {
                new List<double> { 0, 0, 0, 1, 1, 1, 1 },
                new List<double> { 0, 1, 0, 0, 1, 0 ,1 },
                new List<double> { 1, 0, 1, 0, 1, 0 ,1 },
                new List<double> { 1, 1, 0, 0, 0, 0, 1 }
            };
            foreach (var input in trainingInputs)
            {
                var output = mlp.ForwardPropagate(input);
                Console.WriteLine($"Input: {string.Join(",", input)} => Output: {string.Join(",", output)}");
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
