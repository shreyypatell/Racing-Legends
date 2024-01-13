using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RacingGame
{
    public class Fuel: DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D fuelTexture;
        private Vector2 position;
        private float speed = 3f;

        public Vector2 Position { get => position; }

        public float Speed { get => speed; set => speed = value; }

        public Rectangle BoundingBox
        {
            get
            {
                var centerX = position.X + fuelTexture.Width / 2;
                return new Rectangle((int)centerX, (int)position.Y, (int)(fuelTexture.Width), (int)(fuelTexture.Height));
            }
        }

        public Fuel(Game game, SpriteBatch spriteBatch, Vector2 position) : base(game)
        {
            this.spriteBatch = spriteBatch;

            this.position = position;
            fuelTexture = game.Content.Load<Texture2D>("fuel");



        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Move car down
            position.Y += speed;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.Draw(fuelTexture, position, Color.White);
            spriteBatch.End();
        }



    }

}
