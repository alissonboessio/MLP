

namespace NeuralNetwork.Objects.MLP
{
    public class MLP
    {
        public List<Layer> Layers { get; private set; }

        public MLP(List<int> layerSizes)
        {
            Layers = new List<Layer>();

            for (int i = 0; i < layerSizes.Count - 1; i++)
            {
                Layers.Add(new Layer(layerSizes[i + 1], layerSizes[i]));
            }
        }

        // Propaga os inputs em cada Camada, e retorna o output calculado
        public List<double> ForwardPropagate(List<double> inputs)
        {
            foreach (var layer in Layers)
            {
                inputs = layer.ForwardPropagate(inputs);  // Propaga os inputs em cada Layer
            }
            return inputs; // Retorna ao final, o output calculado
        }

        public void Train(List<List<double>> trainingInputs, List<List<double>> trainingOutputs, int iterations, double learningRate)
        {
            for (int iter = 0; iter < iterations; iter++)
            {
                for (int i = 0; i < trainingInputs.Count; i++)
                {
                    var output = ForwardPropagate(trainingInputs[i]);

                    // Apos realizar a propagacao nas camadas, realiza o backpropagation para corrigir erros
                    Backpropagate(output, trainingOutputs[i], learningRate, trainingInputs[i]);
                }
            }
        }
        
        private void Backpropagate(List<double> realOutput, List<double> expectedOutput, double learningRate, List<double> realInputs)
        {


            // camada de saida
            for (int i = 0; i < Layers.Last().Neurons.Count; i++)
            {
                Neuron neuron = Layers.Last().Neurons[i];
                double error = expectedOutput[i] - realOutput[i];
                neuron.DeltaError = neuron.Output * (1 - neuron.Output) * error;
            }

            // Calcula o erro para as camadas internas
            for (int layerIndex = Layers.Count - 2; layerIndex >= 0; layerIndex--)
            {
                Layer layer = Layers[layerIndex];
                Layer nextLayer = Layers[layerIndex + 1];

                for (int j = 0; j < layer.Neurons.Count; j++)
                {
                    Neuron neuron = layer.Neurons[j];
                    double error = 0;

                    foreach (var nextNeuron in nextLayer.Neurons)
                    {
                        error += nextNeuron.InputWeights[j] * nextNeuron.DeltaError;
                    }

                    neuron.DeltaError = neuron.Output * (1 - neuron.Output) * error;
                }
            }

            // Atualiza os pesos e o Bias de cada Neuron em cada camada
            for (int layerIndex = 0; layerIndex < Layers.Count; layerIndex++)
            {
                Layer layer = Layers[layerIndex];

                List<double> inputs = (layerIndex == 0)
                    ? realInputs  // Se for a primeira camada, pega o input de entrada
                    : Layers[layerIndex - 1].Neurons.Select(neuron => neuron.Output).ToList(); // Para camadas ocultas, usa os outputs da camada anterior como input

                for (int neuronIndex = 0; neuronIndex < layer.Neurons.Count; neuronIndex++)
                {
                    Neuron neuron = layer.Neurons[neuronIndex];
                    neuron.UpdateInputWeights(learningRate, inputs); // Passa os inputs corretos
                }
            }
        }
    }
}
