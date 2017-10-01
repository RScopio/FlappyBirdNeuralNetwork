using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBirdNeuralNet
{
    internal class Sprite
    {
        public Vector2 Position;
        public Vector2 Scale;

        public Sprite(Texture2D image, Vector2 position, Color tint)
        {
            Image = image;
            Position = position;
            Tint = tint;
            Scale = Vector2.One;
            Origin = new Vector2((float) image.Width / 2, (float) image.Height / 2);
        }

        public Texture2D Image { get; set; }
        public Color Tint { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Size => new Vector2(Image.Width * Scale.X, Image.Height * Scale.Y);

        public Rectangle Hitbox => new Rectangle((int) (Position.X - Size.X / 2),
            (int) (Position.Y - Size.Y / 2), (int) Size.X, (int) Size.Y);

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, Position, null, Tint, 0, Origin, Scale, SpriteEffects.None, 0);
        }
    }
}