
namespace NeuralNetwork.Objects
{
    public class Layer
    {
        public List<Neuron> Neurons { get; set; }
        public Layer(int layerSize, int prevLayerSize)
        {
            this.Neurons = new List<Neuron>(layerSize);

            for (int i = 0; i < layerSize; i++)
            {
                Neurons.Add(new Neuron(prevLayerSize));
            }

        }

        public List<double> ForwardPropagate(List<double> inputs)
        {
            List<double> outputs = new List<double>(Neurons.Count);
            this.Neurons.ForEach(n =>
            {
                n.Output = ActivateSigmoidal(CalculateOutput(n, inputs));
                outputs.Add(n.Output);
            });

            return outputs;
        }

        public double CalculateOutput(Neuron Neuron, List<double> inputs)
        {
            double output = 0;
            for (int i = 0; i < inputs.Count; i++)
            {
                output += Neuron.InputWeights[i] * inputs[i];
            }

            return output + Neuron.Bias;

        }
        public double ActivateSigmoidal(double value)
        {
            return 1 / (1 + Math.Exp(-value));
        }

    }
}
