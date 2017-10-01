using System;
using System.Collections.Generic;

namespace FlappyBirdNeuralNet.NeuralNet
{
    public class NeuralNetwork
    {
        public NeuralNetwork(double learningRate, int[] layers)
        {
            if (layers.Length < 2) return;

            LearningRate = learningRate;
            Layers = new List<Layer>();

            for (var i = 0; i < layers.Length; i++)
            {
                var layer = new Layer(layers[i]);
                Layers.Add(layer);

                for (var j = 0; j < layers[i]; j++)
                    layer.Neurons.Add(new Neuron());

                layer.Neurons.ForEach(nn =>
                    {
                        if (i == 0)
                            nn.Bias = 0;
                        else
                            for (var d = 0; d < layers[i - 1]; d++)
                                nn.Dendrites.Add(new Dendrite());
                    }
                );
            }
        }

        public NeuralNetwork(int[] layers) : this(0, layers)
        {
        }

        public List<Layer> Layers { get; set; }
        public double LearningRate { get; set; } // Non adaptive
        public int LayerCount => Layers.Count;

        private double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        public double[] Run(List<double> input)
        {
            if (input.Count != Layers[0].NeuronCount) return null;
            for (var i = 0; i < LayerCount; i++)
            {
                var layer = Layers[i];

                for (var n = 0; n < layer.NeuronCount; n++)
                {
                    var neuron = layer.Neurons[n];

                    if (i == 0)
                    {
                        //set input layers
                        neuron.Value = input[n];
                    }
                    else
                    {
                        //calculate hidden & output layers
                        neuron.Value = 0;
                        for (var np = 0; np < Layers[i - 1].NeuronCount; np++)
                        {
                            neuron.Value = neuron.Value +
                                           Layers[i - 1].Neurons[np].Value * neuron.Dendrites[np].Weight;

                            neuron.Value = Sigmoid(neuron.Value + neuron.Bias);
                        }
                    }
                }
            }

            //copy the last layer into output and return
            var last = Layers[LayerCount - 1];
            var numOutput = last.NeuronCount;
            var output = new double[numOutput];
            for (var i = 0; i < last.NeuronCount; i++)
                output[i] = last.Neurons[i].Value;

            return output;
        }

        /// <summary>
        ///     This is a supervised learning algo
        /// </summary>
        /// <param name="input">test input</param>
        /// <param name="output">expected output</param>
        /// <returns></returns>
        public bool Train(List<double> input, List<double> output)
        {
            if (input.Count != Layers[0].NeuronCount ||
                output.Count != Layers[LayerCount - 1].NeuronCount) return false;

            //Propogate forward through the network to generate the output
            Run(input);

            for (var i = 0; i < Layers[LayerCount - 1].NeuronCount; i++)
            {
                //calculation of the cost (error term)
                Neuron neuron = Layers[LayerCount - 1].Neurons[i];
                neuron.Delta = neuron.Value * (1 - neuron.Value) * (output[i] - neuron.Value);

                //Propagation of the output activations back through the network using the training 
                //pattern target in order to generate the deltas (the difference between the targeted
                //and actual output values) of all output and hidden neurons.
                for (int j = LayerCount - 2; j > 2; j--)
                {
                    for (int k = 0; k < Layers[j].NeuronCount; k++)
                    {
                        Neuron n = Layers[j].Neurons[k];

                        n.Delta = n.Value *
                                  (1 - n.Value) *
                                  Layers[j + 1].Neurons[i].Dendrites[k].Weight *
                                  Layers[j + 1].Neurons[i].Delta;
                    }
                }
            }


            for (int i = LayerCount - 1; i > 1; i--)
            {
                for (int j = 0; j < Layers[i].NeuronCount; j++)
                {
                    Neuron n = Layers[i].Neurons[j];
                    n.Bias = n.Bias + (LearningRate * n.Delta);

                    for (int k = 0; k < n.DendriteCount; k++)
                    {
                        n.Dendrites[k].Weight = n.Dendrites[k].Weight +
                                                (LearningRate * Layers[i - 1].Neurons[k].Value * n.Delta);
                    }
                }
            }


            return true;
        }

        /// <summary>
        /// Every dendrite has a chance of mutating to a new random value
        /// </summary>
        /// <param name="mutationRate">chance of mutation</param>
        public void Mutate(double mutationRate)
        {
            var n = new Random(Guid.NewGuid().GetHashCode());
            foreach (var layer in Layers)
            {
                foreach (var layerNeuron in layer.Neurons)
                {
                    // mutate some 'bias' information
                    if (n.NextDouble() <= mutationRate)
                    {
                        layerNeuron.Bias = n.Next(-1, 1) + n.NextDouble();
                    }

                    // mutate some 'weights' information
                    foreach (var layerNeuronDendrite in layerNeuron.Dendrites)
                    {
                        if (n.NextDouble() <= mutationRate)
                        {
                            layerNeuronDendrite.Weight = n.Next(-1, 1) + n.NextDouble();
                        }
                    }
                }
            }
        }

        public NeuralNetwork Crossover(NeuralNetwork other)
        {
            //create a child
            int[] iLayer = new int[LayerCount];
            for (int i = 0; i < LayerCount; i++)
            {
                iLayer[i] = Layers[i].NeuronCount;
            }
            NeuralNetwork child = new NeuralNetwork(iLayer);

            //inherate from parents
            Random n = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 1; i < LayerCount; i++)
            {
                var cutPoint = n.Next(0, Layers[i].NeuronCount);
                // left side of cut point from this parent
                for (int j = 0; j < cutPoint; j++)
                {
                    child.Layers[i].Neurons[j].Bias = Layers[i].Neurons[j].Bias;
                }
                // right side of cut point from other parent
                for (int j = cutPoint; j < Layers[i].NeuronCount; j++)
                {
                    child.Layers[i].Neurons[j].Bias = other.Layers[i].Neurons[j].Bias;
                }
            }
            return child;
        }
    }
}