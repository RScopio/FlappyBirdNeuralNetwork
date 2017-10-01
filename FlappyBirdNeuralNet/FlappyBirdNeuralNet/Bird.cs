using System.Collections.Generic;
using FlappyBirdNeuralNet.NeuralNet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBirdNeuralNet
{
    internal class Bird : Sprite
    {
        //NN
        public NeuralNetwork Brain;

        private double distanceToClosest;

        private double totalDistance;

        public Bird(Texture2D image, Vector2 position, Color tint, float jumpPower)
            : base(image, position, tint)
        {
            Alive = true;
            Velocity = 0;
            JumpPower = jumpPower;
            Scale = new Vector2(0.5f);
            Brain = new NeuralNetwork(new[] {2, 6, 1});
            totalDistance = 0;
        }

        public float Velocity { get; set; }
        public float JumpPower { get; set; }
        public bool Alive { get; set; }
        public double Fitness => totalDistance - distanceToClosest;

        public void Update(float gravity, List<Pipe> pipes, float pipeSpeed)
        {
            if (!Alive) return;

            Velocity += gravity;
            Position.Y += Velocity;
            totalDistance += pipeSpeed;

            //calculate input
            double horz = 0; //also acts as distance to closest
            double vert = 0;
            for (var i = 0; i < pipes.Count; i++)
                if (pipes[i].Position.X + pipes[i].Top.Size.X > Position.X)
                {
                    horz = pipes[i].Position.X - Position.X;
                    vert = pipes[i].Position.Y - Position.Y;
                    break;
                }
            distanceToClosest = horz;
            var targetDeltaX = Normalize(horz, 1050);
            var targetDeltaY = Normalize(vert, 800);
            var output = Brain.Run(new List<double> {targetDeltaX, targetDeltaY})[0];
            if (output > 0.5f)
                Jump();
        }

        public void Jump()
        {
            Velocity = JumpPower;
        }

        public void ResetTotal()
        {
            totalDistance = 0;
            distanceToClosest = 0;
        }

        public double Normalize(double value, double max)
        {
            if (value < -max) value = -max;
            else if (value > max) value = max;

            return value / max;
        }
    }
}