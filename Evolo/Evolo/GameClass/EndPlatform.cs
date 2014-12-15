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

namespace Evolo.GameClass
{
    class Platform
    {
        private Texture2D endPlatform;
        private Texture2D startPlatform;
        private Vector2 origin;

        public Platform(Texture2D endPlatform, Texture2D startPlatform)
        {
            this.endPlatform = endPlatform;
            this.startPlatform = startPlatform;
        }
        

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 endPosition, Vector2 startPosition)
        {
            spriteBatch.Draw(endPlatform, endPosition, Color.White);
            spriteBatch.Draw(startPlatform, startPosition, Color.White);
        }
    }
}
