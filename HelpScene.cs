using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RacingGame
{
    public class HelpScene : GameScene
    {
        private SpriteBatch spriteBatch;

        private string helpText = "" +
            "Press Left Key to move left\n" +
            "Press Right Key to move right\n" +
            "Press Esc Key to exit\n";

        private SpriteFont regularFont;


        public HelpScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            this.spriteBatch = g._spriteBatch;

            regularFont = g.Content.Load<SpriteFont>("RegularFont");
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.DrawString(regularFont, helpText, new Vector2(Shared.stage.X / 2 - regularFont.MeasureString(helpText).X / 2, Shared.stage.Y / 2 - regularFont.MeasureString(helpText).Y / 2), Color.Black);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
