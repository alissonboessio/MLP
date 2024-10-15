

using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

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
                inputs = layer.Propagate(inputs);  // Propaga os inputs em cada Layer
            }
            return inputs; // Retorna ao final, o output calculado
        }

        public void Train(List<List<double>> trainingInputs, List<List<double>> trainingOutputs, 
            int iterations, double learningRate, IProgress<double> progress = null)
        {
            for (int iter = 0; iter < iterations; iter++)
            {
                for (int i = 0; i < trainingInputs.Count; i++)
                {
                    var output = ForwardPropagate(trainingInputs[i]);

                    // Apos realizar a propagacao nas camadas, realiza o backpropagation para corrigir erros
                    BackPropagate(output, trainingOutputs[i], learningRate, trainingInputs[i]);
                }
                if (progress != null)
                {
                    progress?.Report(iter);
                }
            }
        }
        
        public TestReturn Test(List<List<double>> testInputs, List<List<double>> testOutputs, double threshold)
        {
            List<List<double>> outputsMLP = new List<List<double>>();

            foreach (List<double> input in testInputs)
            {
                outputsMLP.Add(ForwardPropagate(input));
            }

            TestReturn testReturn = new TestReturn();
            testReturn.TotalCases = outputsMLP.Count;
            testReturn.calculatedOutput.AddRange(outputsMLP);
            
            for (int i = 0; i < outputsMLP.Count; i++)
            {
                List<double> classifiedOutputs = ApplyThreshold(outputsMLP[i], threshold);
                testReturn.classifiedOutput.Add(classifiedOutputs);

                bool isCorrect = true;
                for (int j = 0; j < classifiedOutputs.Count; j++)
                {
                    if ((int)classifiedOutputs[j] != (int)testOutputs[i][j])
                    {
                        isCorrect = false;
                        break;
                    }
                }
                string keyClass = string.Join(",", testOutputs[i]);
                if (!testReturn.TotalByClass.ContainsKey(keyClass))
                {
                    testReturn.TotalByClass[keyClass] = new TotalByClass
                    {
                        total = 0,
                        totalCorrect = 0,
                        totalWrong = 0
                    };
                }
                testReturn.TotalByClass[keyClass].total += 1;
                if (isCorrect)
                {
                    testReturn.TotalByClass[keyClass].totalCorrect += 1;
                    testReturn.QtyCorrect++;
                }
                else
                {
                    testReturn.TotalByClass[keyClass].totalWrong += 1;
                    testReturn.QtyWrong++;
                }
            }
            foreach (var classPrecision in testReturn.TotalByClass)
            {
                testReturn.Precision[classPrecision.Key] = (decimal)classPrecision.Value.totalCorrect / classPrecision.Value.total;
            }
            testReturn.Accuracy = (decimal)testReturn.QtyCorrect / testReturn.TotalCases;

            return testReturn;

        }

        public List<double> ApplyThreshold(List<double> inputs, double threshold)
        {
            List<double> thresholdInputs = new List<double>();

            foreach (var input in inputs)
            {
                if (input > threshold)
                {
                    thresholdInputs.Add(1.0); 
                }
                else
                {
                    thresholdInputs.Add(0.0);
                }
            }

            return thresholdInputs;
        }

        private void BackPropagate(List<double> realOutput, List<double> expectedOutput, double learningRate, List<double> realInputs)
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
                    ? realInputs  // Se for a primeira camada, utiliza o input de entrada
                    : Layers[layerIndex - 1].Neurons.Select(neuron => neuron.Output).ToList();

                for (int neuronIndex = 0; neuronIndex < layer.Neurons.Count; neuronIndex++)
                {
                    Neuron neuron = layer.Neurons[neuronIndex];
                    neuron.UpdateInputWeights(learningRate, inputs);
                }
            }
        }
    }
}
