using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBirdNeuralNet
{
    class Bird : Sprite
    {
        //NN
        public float Velocity { get; set; }
        public float JumpPower { get; set; }

        public Bird(Texture2D image, Vector2 position, Color tint, float jumpPower)
            : base(image, position, tint)
        {
            Velocity = 0;
            JumpPower = jumpPower;
            Scale = new Vector2(0.5f);
        }

        public void Update(float gravity) //probably pass in jumpPower when implement NN
        {
            Velocity += gravity;
            Position.Y += Velocity;

            //Run NN to determine when to jump
        }

        public void Jump()
        {
            Velocity = JumpPower;
        }
    }
}
