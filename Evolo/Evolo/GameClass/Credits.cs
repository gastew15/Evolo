using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
/**
 * Evolo Credits: Displays the Credits and the a scroll speed
 * Author: Gavin
 * Version: 10/30/14
 */
namespace Evolo.GameClass
{
    class Credits
    {
        private String[] lines;
        private SpriteFont font;
        private float scrollSpeed;

        public Credits(String[] lines, SpriteFont font)
        {
            this.lines = lines;
            this.font = font;
        }

        public void DrawCredits(SpriteBatch spriteBatch)
        {
            
            scrollSpeed += .75f * GlobalVar.ScaleSize.Y;

            for (int j = 0; j < lines.Length; j++)
            {
                spriteBatch.DrawString(font, lines[j], new Vector2((GlobalVar.ScreenSize.X / 2) - ((font.MeasureString(lines[j]).X * GlobalVar.ScaleSize.X) / 2), (GlobalVar.ScreenSize.Y + (j * 50 * GlobalVar.ScaleSize.Y)) - scrollSpeed), Color.White, 0f, new Vector2(0, 0), GlobalVar.ScaleSize, SpriteEffects.None, 1f);
            }

            if ((GlobalVar.ScreenSize.Y + (lines.Length * 50 * GlobalVar.ScaleSize.Y)) - scrollSpeed < -10)
            {
                GlobalVar.GameState = "MenuScreen";
                scrollSpeed = 0;
            }
        }

        public void resetCreditRoll()
        {
            scrollSpeed = 0;
        }
    }
}
