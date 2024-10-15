
using System.IO;
using System.Windows;
using Microsoft.Win32;
using NeuralNetwork.Objects;
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

            if (ParametersViewModel.TrainFilePath == null || ParametersViewModel.TrainDummiesFilePath == null)
            {
                MessageBox.Show("Defina os arquivos de treino!", "Parâmetros Incorretos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                progressBar.Minimum = 0;
                progressBar.Maximum = ParametersViewModel.Iterations;
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
                if (ParametersViewModel.TestFilePath == null || ParametersViewModel.TestDummiesFilePath == null)
                {
                    MessageBox.Show("Defina os arquivos de teste!", "Parâmetros Incorretos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

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
                try
                {
                    ParametersViewModel.TrainFilePath = openFileDialog.FileName;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro ao processar arquivo!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
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
                try
                {
                    ParametersViewModel.TrainDummiesFilePath = openFileDialog.FileName;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro ao processar arquivo!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
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
                try
                {
                    ParametersViewModel.TestFilePath = openFileDialog.FileName;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro ao processar arquivo!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
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
                try
                {
                    ParametersViewModel.TestDummiesFilePath = openFileDialog.FileName;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro ao processar arquivo!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void BT_SalvarResultados_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "results");
            var di = Directory.CreateDirectory(savePath);

            ParametersViewModel.SaveTestReturnToTXT(Path.Combine(savePath, $"{dt.ToString("yyyy-MM-dd-hh-mm-ss")}-summary.txt"));
            ParametersViewModel.SaveListToCsvFile(ParametersViewModel.TestReturnMLP.calculatedOutput, Path.Combine(savePath, $"{dt.ToString("yyyy-MM-dd-hh-mm-ss")}-CalculatedOutput.csv"));
            ParametersViewModel.SaveListToCsvFile(ParametersViewModel.TestReturnMLP.classifiedOutput, Path.Combine(savePath, $"{dt.ToString("yyyy-MM-dd-hh-mm-ss")}-ClassifiedOutput.csv"));
    
        }
    }
}