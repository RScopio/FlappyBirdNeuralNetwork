using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappyBirdNeuralNet
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Viewport screen;
        KeyboardState ks;
        float gravity = 1;
        float jumpPower = 10;

        Bird bird;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            screen = GraphicsDevice.Viewport;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            bird = new Bird(Content.Load<Texture2D>("circle"), new Vector2(100), Color.Blue);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState lastKs = ks;
            ks = Keyboard.GetState();
            bird.Update(gravity);

            if (ks.IsKeyDown(Keys.Space) && lastKs.IsKeyUp(Keys.Space))
            {
                bird.Jump(jumpPower);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            bird.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
