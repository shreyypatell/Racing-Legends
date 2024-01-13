using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RacingGame
{
    public class CreditScene : GameScene
    {
        private SpriteBatch spriteBatch;

        private Rectangle rectangle, creditRect;

        private string creditText = "The Credit Scene";
        public CreditScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            this.spriteBatch = g._spriteBatch;
 }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            Game.GraphicsDevice.Clear(Color.DarkSlateBlue);

            creditRect = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);

            spriteBatch.DrawString(Game.Content.Load<SpriteFont>("TitleFont"), "Credit", new Vector2(creditRect.Center.X - Game.Content.Load<SpriteFont>("TitleFont").MeasureString("Credit").X / 2, creditRect.Center.Y - Game.Content.Load<SpriteFont>("TitleFont").MeasureString("Credit").Y), Color.White);
            spriteBatch.DrawString(Game.Content.Load<SpriteFont>("RegularFont"), creditText, new Vector2(creditRect.Center.X - Game.Content.Load<SpriteFont>("RegularFont").MeasureString(creditText).X / 2, creditRect.Center.Y), Color.White);
            spriteBatch.DrawString(Game.Content.Load<SpriteFont>("RegularFont"), "Press Esc to return to menu", new Vector2(creditRect.Center.X - Game.Content.Load<SpriteFont>("RegularFont").MeasureString("Press Esc to return to menu").X / 2, creditRect.Center.Y + Game.Content.Load<SpriteFont>("RegularFont").MeasureString(creditText).Y), Color.White);



            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
