using System;
using System.Collections.Generic;
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

        float gravity = 0.5f;
        float jumpPower = -9;
        private float pipeSpeed = 4;
        private TimeSpan spawnTimer = TimeSpan.Zero;
        private int spawnTime = 2000;

        Bird bird;
        List<Pipe> pipes;
        private Texture2D pipeImage;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 800;
        }

        protected override void Initialize()
        {
            screen = GraphicsDevice.Viewport;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            bird = new Bird(Content.Load<Texture2D>("circle"), new Vector2(200), Color.Blue, jumpPower);
            pipes = new List<Pipe>();
            pipeImage = Content.Load<Texture2D>("bar");
            pipes.Add(new Pipe(pipeImage, screen.Width + pipeImage.Width, Color.DarkGreen, screen, bird.Image.Height * 2));
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var lastKs = ks;
            ks = Keyboard.GetState();
            spawnTimer += gameTime.ElapsedGameTime;
            if (spawnTimer >= TimeSpan.FromMilliseconds(spawnTime))
            {
                spawnTimer = TimeSpan.Zero;
                pipes.Add(new Pipe(pipeImage, screen.Width + pipeImage.Width, Color.DarkGreen, screen, bird.Image.Height * 2));
            }

            if (ks.IsKeyDown(Keys.Space) && lastKs.IsKeyUp(Keys.Space))
            {
                bird.Jump();
            }
            bird.Update(gravity);
            foreach (var pipe in pipes)
            {
                pipe.Update(pipeSpeed);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            bird.Draw(spriteBatch);

            foreach (var pipe in pipes)
            {
                pipe.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
