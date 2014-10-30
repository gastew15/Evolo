#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

/** Class Discription
 * Evolo class to create and handle player objects in the game
 * Author: Gavin
 * Version: 10/21/14
 */

namespace Evolo.GameClass
{
    class Player
    {
        Vector2 drawPoint, scaleSize, playerPos;
        Texture2D playerTexture;
        Color drawColor;

        public Player(Texture2D playerTexture)
        {
            this.playerTexture = playerTexture;
        }

        public void Update(Vector2 drawPoint, Vector2 scaleSize, Color drawColor)
        {
            this.drawPoint = drawPoint;
            this.scaleSize = scaleSize;
            this.drawColor = drawColor;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(playerTexture, drawPoint, null, drawColor, 0f, new Vector2(0, 0), scaleSize, spriteEffects, 1f); 
        }
    }
}
