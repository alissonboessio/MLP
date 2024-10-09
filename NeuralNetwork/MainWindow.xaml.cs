
using System.Windows;
using Microsoft.Win32;

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
            ParametersViewModel.Train();

        }
        private void BT_TestarRede_Click(object sender, RoutedEventArgs e)
        {
            ParametersViewModel.Test();

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
                ParametersViewModel.DataFilePath = openFileDialog.FileName;
            }
        }

        private void BTOutputTrainFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Selecione o arquivo dummy";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            openFileDialog.DefaultExt = ".csv";

            if (openFileDialog.ShowDialog() == true)
            {
                ParametersViewModel.OutputFilePath = openFileDialog.FileName;
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
    }
}