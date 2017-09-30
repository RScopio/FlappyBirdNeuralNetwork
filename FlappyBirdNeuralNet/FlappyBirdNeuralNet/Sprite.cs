using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBirdNeuralNet
{
    class Sprite
    {
        public Texture2D Image { get; set; }
        public Vector2 Position;
        public Color Tint { get; set; }
        public Vector2 Scale;
        public Vector2 Origin { get; set; }
        public Vector2 Size => new Vector2(Image.Width * Scale.X, Image.Height * Scale.Y);
        public Rectangle Hitbox => new Rectangle((int)(Position.X - Size.X / 2),
            (int)(Position.Y - Size.Y / 2), (int)(Size.X), (int)(Size.Y));

        public Sprite(Texture2D image, Vector2 position, Color tint)
        {
            Image = image;
            Position = position;
            Tint = tint;
            Scale = Vector2.One;
            Origin = new Vector2((float)image.Width / 2, (float)image.Height / 2);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Position, null, Tint, 0, Origin, Scale, SpriteEffects.None, 0);
        }
    }
}
