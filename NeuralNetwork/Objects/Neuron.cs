namespace NeuralNetwork.Objects
{
    public class Neuron
    {
        // Conexões de entrada deste neurônio
        public List<double> InputWeights { get; set; }
        public double Bias { get; set; }
        public double Output { get; set; }
        public double DeltaError { get; set; }

        public Neuron(int prevLayerSize)
        {
            this.InputWeights = new List<double>(prevLayerSize);
            this.Bias = 0.0;

            Random rand = new Random();
            for (int i = 0; i < prevLayerSize; i++)
            {
                InputWeights.Add(rand.NextDouble());
            }
            this.Bias = rand.NextDouble();
        }

        public void UpdateInputWeights(double learningRate, List<double> inputs)
        {
            for (int i = 0; i < InputWeights.Count; i++)
            {
                InputWeights[i] += learningRate * DeltaError * inputs[i];
            }
            Bias += learningRate * DeltaError;
        }
    }
}
