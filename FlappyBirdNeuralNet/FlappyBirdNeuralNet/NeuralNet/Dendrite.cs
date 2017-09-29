using System;

namespace FlappyBirdNeuralNet.NeuralNet
{
    /// <summary>
    ///     Dendrites act as the input to a neuron.
    ///     Think of these as the edges of the graph.
    /// </summary>
    public class Dendrite
    {
        public Dendrite()
        {
            var n = new Random(Environment.TickCount);
            Weight = n.NextDouble();
        }

        public double Weight { get; set; }
    }
}