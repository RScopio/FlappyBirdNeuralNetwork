using System.Collections.Generic;

namespace FlappyBirdNeuralNet.NeuralNet
{
    public class Layer
    {
        public Layer(int numNeurons)
        {
            Neurons = new List<Neuron>(numNeurons);
        }

        public List<Neuron> Neurons { get; set; }
        public int NeuronCount => Neurons.Count;
    }
}