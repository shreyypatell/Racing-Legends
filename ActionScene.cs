using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RacingGame
{

    public class ActionScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private Texture2D roadTexture;
        private float roadTexture1Y;
        private float roadTexture2Y;

        private Player playerCar;
        private Fuel fuel;
        private double fuelSpawnTimer;
        private double nextFuelSpawnThreshold;

        private float health = 100f;


        private List<Truck> trucks;
        private Random random = new Random();

        private float speedIncreaseAmount = 0.5f;
        private double speedIncreaseInterval = 10.0;
        private double timeSinceLastSpeedIncrease = 0.0;



        private double spawnTimer;
        private double nextSpawnThreshold;

        private Texture2D GameOverTexture;

        private GameState gameState;

        private SpriteFont font;

        private double levelTimeElapsed;

        private Vector2 bannerPosition;
        private Vector2 bannerSize;
        private Color bannerBackgroundColor;

        private float ScrollSpeed = 200;
        private float TruckSpeed = 5;

        Game1 g;

        private bool Intersects(Rectangle rect1, Rectangle rect2)
        {
            return rect1.Intersects(rect2);
        }

        private int currentLevelIndex;



        public ActionScene(Game game) : base(game)
        {
            g = (Game1)game;
            this.spriteBatch = g._spriteBatch;

            GameOverTexture = g.Content.Load<Texture2D>("game-over");
            font = g.Content.Load<SpriteFont>("RegularFont");


            roadTexture = g.Content.Load<Texture2D>("road");


            roadTexture1Y = 0;
            roadTexture2Y = -GraphicsDevice.Viewport.Height;

            currentLevelIndex = 0;

            bannerPosition = new Vector2(20,GraphicsDevice.Viewport.Height - 200);
            bannerSize = new Vector2(190, 100);
            bannerBackgroundColor = new Color(0, 0, 0, 255);

            playerCar = new Player(g, spriteBatch);
            trucks = new List<Truck>();

        }

        public override void Initialize()
        {
            base.Initialize();

            currentLevelIndex = 0;
            levelTimeElapsed = 0;
            health = 100f;


            fuelSpawnTimer = 0;
            SetNextFuelSpawnThreshold();

            SetNextSpawnThreshold();

            if (gameState == GameState.Crashed || gameState == GameState.GameCompleted)
            {
                trucks.Clear();
            }

            gameState = GameState.Playing;

            playerCar.Initialize();

            foreach (var car in trucks)
            {
                car.Initialize();
            }
        }

        private void SetNextFuelSpawnThreshold()
        {
            float randomX = random.Next(20, Game.Window.ClientBounds.Width - 20);
            fuel = new Fuel(g, spriteBatch, new Vector2(randomX, -roadTexture.Height));

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (gameState == GameState.GameCompleted || gameState == GameState.Crashed)
            {
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Enter))
                {
                    Initialize();
                }
            }

            if (gameState == GameState.Playing)
            {
                levelTimeElapsed += gameTime.ElapsedGameTime.TotalSeconds;

            }

            if (gameState == GameState.Playing)
            {
                levelTimeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
                timeSinceLastSpeedIncrease += gameTime.ElapsedGameTime.TotalSeconds;

                // Check if it's time to increase speed
                if (timeSinceLastSpeedIncrease >= speedIncreaseInterval)
                {
                    ScrollSpeed += speedIncreaseAmount;
                    TruckSpeed += speedIncreaseAmount;
                    timeSinceLastSpeedIncrease = 0.0;
                }
            }
            roadTexture1Y += ScrollSpeed  * (float)gameTime.ElapsedGameTime.TotalSeconds;
            roadTexture2Y += ScrollSpeed  * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (roadTexture1Y >= GraphicsDevice.Viewport.Height)
            {
                roadTexture1Y = roadTexture2Y - GraphicsDevice.Viewport.Height;
            }

            if (roadTexture2Y >= GraphicsDevice.Viewport.Height)
            {
                roadTexture2Y = roadTexture1Y - GraphicsDevice.Viewport.Height;
            }




            playerCar.Update(gameTime);
            spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (spawnTimer > nextSpawnThreshold)
            {
                float randomX = random.Next(20, Game.Window.ClientBounds.Width - 20);
                Truck newCar = new Truck(g, spriteBatch, new Vector2(randomX, -roadTexture.Height));

                trucks.Add(newCar);
                spawnTimer = 0;
                SetNextSpawnThreshold();
            }


            for (int i = trucks.Count - 1; i >= 0; i--)
            {
                var car = trucks[i];
                car.Update(gameTime);
                if (car.Position.Y > Game.Window.ClientBounds.Height)
                {
                    trucks.RemoveAt(i);
                }
            }

            foreach (var truck in trucks)
            {
                truck.Speed = TruckSpeed; 
            }

            try { 
            foreach (var truck in trucks)
            {
                if (Intersects(playerCar.BoundingBox, truck.BoundingBox))
                {
                    HandleCollision(playerCar, truck);
                }
            }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            fuelSpawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (fuel == null && fuelSpawnTimer > nextFuelSpawnThreshold)
            {
                float randomX = random.Next(20, Game.Window.ClientBounds.Width - 20);
                fuel = new Fuel(g, spriteBatch, new Vector2(randomX, -roadTexture.Height));
                fuelSpawnTimer = 0;
                SetNextFuelSpawnThreshold();
            }

            if (fuel != null)
            {
                fuel.Update(gameTime);
                if (fuel.Position.Y > Game.Window.ClientBounds.Height)
                {
                    fuel = null;
                }
            }


            if (fuel != null && Intersects(playerCar.BoundingBox, fuel.BoundingBox))
            {
                HandleCollision(playerCar, fuel);
                fuel = null;
            }

        }



        private void HandleCollision(Player playerCar, DrawableGameComponent otherObject)
        {
            if (otherObject is Truck)
            {
                health -= 30;
                otherObject.Dispose();

                // remove the truck from the list
                trucks.Remove((Truck)otherObject);

                if (health <= 0)
                {
                    health = 0;
                    gameState = GameState.Crashed;
                }
            }

            if(otherObject is Fuel)
            {
                health += 10;
                if (health > 100)
                {
                    health = 100;
                }
            }

        }


        private void SetNextSpawnThreshold()
        {
            nextSpawnThreshold = random.NextDouble() * 2.0 + 1.0;
        }

        private void DrawBanner(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Create a texture for the banner background
            Texture2D bannerBackground = new Texture2D(GraphicsDevice, 1, 1);
            bannerBackground.SetData(new[] { Color.White });

            // Draw the banner background
            spriteBatch.Draw(bannerBackground, new Rectangle((int)bannerPosition.X, (int)bannerPosition.Y, (int)bannerSize.X, (int)bannerSize.Y), bannerBackgroundColor);

            // Draw the text
            string infoText = $"\nHealth: {health.ToString("0.00")}%";

            spriteBatch.DrawString(font, infoText, bannerPosition + new Vector2(10, 10), Color.White);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if(gameState == GameState.Crashed)
            {

                spriteBatch.Begin();
                // Draw game over texture
                spriteBatch.Draw(GameOverTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                
                var text = "Press Esc to go back to menu";
                text += $"\nYour survived for {levelTimeElapsed.ToString("0.00")} seconds";

                // Top center
                var textPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, 100);
                var textSize = font.MeasureString(text);
                var textOrigin = textSize / 2;
                spriteBatch.DrawString(font, text, textPosition, Color.White, 0, textOrigin, 1, SpriteEffects.None, 0);

                
                spriteBatch.End();
                return;
            }

            if (gameState == GameState.GameCompleted)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Congratulations! You won!", new Vector2(100, 100), Color.White);
                spriteBatch.DrawString(font, "Press Enter to restart", new Vector2(100, 200), Color.White);
                spriteBatch.End();
                return;
            }

            spriteBatch.Begin();

            // Draw the road texture fullscreen

            spriteBatch.Draw(roadTexture, new Rectangle(0, (int)roadTexture1Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(roadTexture, new Rectangle(0, (int)roadTexture2Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            // Write roadTexture1Y and roadTexture2Y to the console
            Console.WriteLine($"roadTexture1Y: {roadTexture1Y}, roadTexture2Y: {roadTexture2Y}");

            DrawBanner(spriteBatch, gameTime);


            spriteBatch.End();

            playerCar.Draw(gameTime);

            foreach (var truck in trucks)
            {
                truck.Draw(gameTime);
            }


            if (fuel != null)
            {
                fuel.Draw(gameTime);
            }

        }
    }

}
