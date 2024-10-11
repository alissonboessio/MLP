
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using NeuralNetwork.ViewModel;

namespace NeuralNetwork
{
    public partial class MainWindow : Window
    {
        public ParametersViewModel ParametersViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ParametersViewModel = new ParametersViewModel();
            DataContext = ParametersViewModel;
        }

        public ParametersViewModel Parameters { get; set; }

        private void BT_TreinarRede_Click(object sender, RoutedEventArgs e)
        {
            if (ParametersViewModel.Iterations <= 0)
            {
                MessageBox.Show("Épocas deve ser maior que 0!", "Parâmetros Incorretos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                ParametersViewModel.Train();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);

            }


        }
        private void BT_TestarRede_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParametersViewModel.Test();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);

            }

        }

        private void BTTrainFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Selecione o arquivo de treino";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            openFileDialog.DefaultExt = ".csv";

            if (openFileDialog.ShowDialog() == true)
            {
                ParametersViewModel.TrainFilePath = openFileDialog.FileName;
            }
        }

        private void BTOutputTrainFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Selecione o arquivo dummy de treino";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            openFileDialog.DefaultExt = ".csv";

            if (openFileDialog.ShowDialog() == true)
            {
                ParametersViewModel.TrainDummiesFilePath = openFileDialog.FileName;
            }
        }

        private void BTTestFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Selecione o arquivo de teste";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            openFileDialog.DefaultExt = ".csv";

            if (openFileDialog.ShowDialog() == true)
            {
                ParametersViewModel.TestFilePath = openFileDialog.FileName;
            }
        }

        private void BTTestOutputFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Selecione o arquivo dummy de teste";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            openFileDialog.DefaultExt = ".csv";

            if (openFileDialog.ShowDialog() == true)
            {
                ParametersViewModel.TestDummiesFilePath = openFileDialog.FileName;
            }
        }
    }
}