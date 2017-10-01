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
        private SpriteFont font;

        float gravity = 0.5f;
        float jumpPower = -9;
        private float pipeSpeed = 4;
        private TimeSpan spawnTimer = TimeSpan.Zero;
        private int spawnTime = 2000;
        private int generation;

        private List<Bird> birds;
        private Bird bestBird;
        private Bird lastBest;
        List<Pipe> pipes;
        private Texture2D pipeImage;
        private Texture2D birdImage;

        private int rate = 1;

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

            font = Content.Load<SpriteFont>("Font");
            birds = new List<Bird>();
            birdImage = Content.Load<Texture2D>("circle");

            generation = 1;
            for (int i = 0; i < 100; i++)
            {
                birds.Add(new Bird(birdImage, new Vector2(200, (float)screen.Height / 2), Color.Blue, jumpPower));
            }
            bestBird = birds[0];
            lastBest = bestBird;

            pipes = new List<Pipe>();
            pipeImage = Content.Load<Texture2D>("bar");
            pipes.Add(new Pipe(pipeImage, screen.Width + pipeImage.Width, Color.DarkGreen, screen, birdImage.Height * 2));
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            for (int z = 0; z < rate; z++)
            {

                var lastKs = ks;
                ks = Keyboard.GetState();
                spawnTimer += gameTime.ElapsedGameTime;

                if (ks.IsKeyDown(Keys.Space) && lastKs.IsKeyUp(Keys.Space))
                {
                    if (rate == 10) rate = 1;
                    else if (rate == 1) rate = 10;
                }

                //spawn pipes
                if (spawnTimer >= TimeSpan.FromMilliseconds(spawnTime))
                {
                    spawnTimer = TimeSpan.Zero;
                    pipes.Add(new Pipe(pipeImage, screen.Width + pipeImage.Width, Color.DarkGreen, screen,
                        birdImage.Height * 2));
                }

                //update birds
                foreach (var bird in birds)
                {
                    bird.Update(gravity, pipes, pipeSpeed);
                }

                //check bird death here
                foreach (Bird bird in birds)
                {
                    //out of bounds
                    if (bird.Position.Y < 0 || bird.Position.Y > screen.Height)
                    {
                        bird.Alive = false;
                    }
                    else
                    {
                        //collide with pipes
                        foreach (var pipe in pipes)
                        {
                            if (pipe.Intersects(bird.Hitbox))
                            {
                                bird.Alive = false;
                            }
                        }
                    }
                }

                //update pipes
                foreach (var pipe in pipes)
                {
                    pipe.Update(pipeSpeed);
                }
                for (int i = 0; i < pipes.Count; i++)
                {
                    if (pipes[i].Position.X < -pipeImage.Width)
                    {
                        pipes.RemoveAt(i);
                    }
                }

                //check if all birds are dead, create next generation
                int deathCount = 0;
                foreach (var bird in birds)
                {
                    if (!bird.Alive) deathCount++;
                }
                if (deathCount == birds.Count)
                {
                    //select fittest birds
                    bestBird = birds[0];
                    double topFitness = bestBird.Fitness;
                    foreach (var bird in birds)
                    {
                        if (bird.Fitness > topFitness)
                        {
                            topFitness = bird.Fitness;
                            bestBird = bird;
                        }
                    }

                    if (bestBird.Fitness < lastBest.Fitness)
                    {
                        bestBird = lastBest;
                    }
                    lastBest = bestBird;

                    //crossover
                    for (int i = 0; i < birds.Count; i++)
                    {
                        if (bestBird == birds[i]) continue;
                        birds[i].Brain = bestBird.Brain.Crossover(birds[i].Brain);
                    }


                    foreach (var bird in birds)
                    {
                        //mutate
                        if (bestBird != bird)
                        {
                            bird.Brain.Mutate(0.25f);
                        }

                        //reset bird
                        bird.Position = new Vector2(200, (float)screen.Height / 2);
                        bird.Alive = true;
                        bird.ResetTotal();
                    }
                    generation++;
                    pipes.Clear();
                }




                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            int index = 0;
            foreach (var pipe in pipes)
            {
                pipe.Draw(spriteBatch);
                spriteBatch.DrawString(font, $"{index}", pipe.Position, Color.White);
                index++;
            }

            float dy = 0;
            index = 0;
            foreach (var bird in birds)
            {
                bird.Draw(spriteBatch);
                spriteBatch.DrawString(font, $"Fitness: {bird.Fitness}", new Vector2(0, dy), Color.White);
                spriteBatch.DrawString(font, $"{index}", bird.Position, Color.White);
                dy += 25;
                index++;
            }

            spriteBatch.DrawString(font, $"Generation: {generation}", new Vector2((float)screen.Width / 2, 0), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
