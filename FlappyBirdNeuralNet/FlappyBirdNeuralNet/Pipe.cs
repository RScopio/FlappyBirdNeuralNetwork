using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBirdNeuralNet
{
    class Pipe
    {
        public Sprite Top;
        public Sprite Bot;
        public Vector2 Position;

        public Pipe(Texture2D image, float x, Color tint, Viewport screen, float gap)
        {
            Position = Vector2.Zero;
            var n = new Random(Guid.NewGuid().GetHashCode());
            float port = n.Next(100, screen.Height - 100);
            Position.Y = port;

            Top = new Sprite(image, Vector2.Zero, tint) { Scale = new Vector2(0.5f, 1) };
            Top.Position = new Vector2(x, port - gap / 2 - Top.Size.Y / 2);

            Bot = new Sprite(image, Vector2.Zero, tint) { Scale = new Vector2(0.5f, 1) };
            Bot.Position = new Vector2(x, port + gap / 2 + Bot.Size.Y / 2);

            Position.X = x;
        }

        public void Update(float speed)
        {
            Position.X -= speed;
            Top.Position.X = Position.X;
            Bot.Position.X = Position.X;
        }

        public bool Intersects(Rectangle other)
        {
            return other.Intersects(Top.Hitbox) || other.Intersects(Bot.Hitbox);
        }

        public void Draw(SpriteBatch batch)
        {
            Top.Draw(batch);
            Bot.Draw(batch);
        }

    }
}
