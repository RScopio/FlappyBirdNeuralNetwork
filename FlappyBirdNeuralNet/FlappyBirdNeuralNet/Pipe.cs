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

        public Pipe(Texture2D image, float position, Color tint, Viewport screen, float gap)
        {
            var n = new Random(Guid.NewGuid().GetHashCode());
            float port = n.Next(100, screen.Height - 100);

            Top = new Sprite(image, Vector2.Zero, tint) { Scale = new Vector2(0.5f, 1) };
            Top.Position = new Vector2(position, port - gap / 2 - Top.Size.Y / 2);

            Bot = new Sprite(image, Vector2.Zero, tint) { Scale = new Vector2(0.5f, 1) };
            Bot.Position = new Vector2(position, port + gap / 2 + Bot.Size.Y / 2);
        }

        public void Update(float speed)
        {
            Top.Position.X -= speed;
            Bot.Position.X -= speed;
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
