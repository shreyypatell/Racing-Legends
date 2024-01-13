using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RacingGame
{
    public class Player : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D CarTexture;
        private Vector2 position;
        private float speed = 5f;

        Vector2 scale = new Vector2(0.8f, 0.8f);

        public Rectangle BoundingBox
        {
            get
            {
                var centerX = position.X + CarTexture.Width * scale.X / 6;
                return new Rectangle((int)centerX, (int)position.Y, (int)(CarTexture.Width * scale.X * 0.75), (int)(CarTexture.Height * scale.Y));
            }
        }


        public Player(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
            CarTexture = game.Content.Load<Texture2D>("car");
            position = new Vector2(Game.Window.ClientBounds.Width / 2 - CarTexture.Width / 2,
                                   Game.Window.ClientBounds.Height - CarTexture.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Left))
            {
                position.X -= speed;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                position.X += speed;
            }

            // Prevent the car from going off-screen
            position.X = Math.Clamp(position.X, 0, Game.Window.ClientBounds.Width - CarTexture.Width);
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

           
            spriteBatch.Begin();

            spriteBatch.Draw(CarTexture, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            spriteBatch.End();
        }
    }


}
