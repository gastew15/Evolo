using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Evolo.GameClass
{
    class LevelEndScreen
    {
        private Texture2D gameoverScreen;
        private bool gameOver = false;
        private WindowSizeManager window;

        public LevelEndScreen()
        {

        }

        public void LoadContent(ContentManager Content)
        {
            gameoverScreen = Content.Load<Texture2D>("Spirites and Pictures/GameOverScreen");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (gameOver == true)
            {
                spriteBatch.Draw(gameoverScreen, new Vector2(window.GetScreenSize().X, window.GetScreenSize().Y), Color.White);
            }

        }

        public void SetGameOver(bool gameOver)
        {
            this.gameOver = gameOver;
        }
    }
}
