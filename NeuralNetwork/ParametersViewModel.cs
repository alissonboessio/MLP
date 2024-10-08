using NeuralNetwork.Objects.MLP;
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

        private string _iterations;
        public string Iterations
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

        private string _learningRate;
        public string LearningRate
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

        public void Train()
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
