using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBirdNeuralNet
{
    public class Edge<T>
    {
        public float Weight;
        public Vertex<T> Parent;
        public Vertex<T> Child;

        public Edge(Vertex<T> parent, Vertex<T> child, float weight)
        {
            this.Parent = parent;
            this.Child = child;
            this.Weight = weight;
        }
    }

    public class Vertex<T>
    {
        public T Value { get; set; }
        public List<Edge<T>> Neighbors { get; set; }
        public bool Visited { get; set; }

        public Vertex(T value)
        {
            this.Value = value;
            this.Visited = false;
            this.Neighbors = new List<Edge<T>>();
        }
    }

    public class Graph<T>
    {
        public Vertex<T> Root;
        public List<Vertex<T>> Vertices = new List<Vertex<T>>();

        public void AddVertex(T value)
        {
            Vertices.Add(new Vertex<T>(value));
        }

        public void AddEdge(Vertex<T> parent, Vertex<T> child, float weight)
        {
            
        }
    }
}
