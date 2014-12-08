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
    class EndPlatform
    {
        private Texture2D endPlatform;
        private Vector2 origin;

        public EndPlatform(Texture2D endPlatform)
        {
            this.endPlatform = endPlatform;
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(endPlatform, position, Color.White);
        }
    }
}
