using System;
using System.Collections.Generic;

namespace FlappyBirdNeuralNet.NeuralNet
{
    /// <summary>
    ///     Think of this as the vertex of a graph.
    /// </summary>
    public class Neuron
    {
        public Neuron()
        {
            var n = new Random(Environment.TickCount);
            Bias = n.NextDouble();
            Dendrites = new List<Dendrite>();
        }

        public List<Dendrite> Dendrites { get; set; }
        public double Bias { get; set; } //allows you to shift activation
        public double Delta { get; set; } // how correct the neuron is compared to expected output. used for training.
        public double Value { get; set; } // activation value of the neuron

        public int DendriteCount => Dendrites.Count;
    }
}