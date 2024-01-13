using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RacingGame
{
    public class Button
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        Rectangle sourceRectangle;


        string buttonText;
        SpriteFont buttonFont;
        SpriteFont selectedButtonFont;

        public Button(Texture2D newTexture, Rectangle sourceRect, Vector2 newPosition, string text, SpriteFont font, SpriteFont selectedButtonFont)
        {
            texture = newTexture;
            // The sourceRectangle should cover the full texture if you want to draw the whole texture
            sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            position = newPosition;
            // Make sure the rectangle is the size of the source rectangle for the texture
            rectangle = new Rectangle((int)position.X, (int)position.Y, sourceRect.Width, sourceRect.Height);
            buttonText = text;
            buttonFont = font;
            this.selectedButtonFont = selectedButtonFont;
        }

        public void Draw(SpriteBatch spriteBatch, bool isSelected = false)
        {
            // Draw the texture to fill the rectangle
            spriteBatch.Draw(texture, rectangle, sourceRectangle, Color.White);

            // Calculate the center position for the text
            Vector2 textSize = buttonFont.MeasureString(buttonText);
            Vector2 textPosition = new Vector2(rectangle.Center.X - textSize.X / 2, rectangle.Center.Y - textSize.Y);

            // Draw the text
            spriteBatch.DrawString(isSelected ? selectedButtonFont :
                buttonFont, buttonText, textPosition, isSelected ? Color.Blue : Color.Black);
        }

    }
}
