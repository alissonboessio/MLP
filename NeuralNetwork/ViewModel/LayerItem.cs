
using System.ComponentModel;

namespace NeuralNetwork.ViewModel
{
    public class LayerItem : INotifyPropertyChanged
    {

        private int _index;
        public int Index
        {
            get => _index;
            set
            {
                if (_index != value)
                {
                    _index = value;
                    OnPropertyChanged(nameof(Index));
                }
            }
        }
        
        private int _qtyNeurons;
        public int QtyNeurons
        {
            get => _qtyNeurons;
            set
            {
                if (_qtyNeurons != value)
                {
                    _qtyNeurons = value;
                    OnPropertyChanged(nameof(QtyNeurons));
                }
            }
        }

        public LayerItem()
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
