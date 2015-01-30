using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
/**
 * Evolo Window Size Manager: Manages the size of the window and make sure the sprites are scaled correctly
 * Author: Gavin
 * Virsion: 10/30/14
 */
namespace Evolo.GameClass
{
    class WindowSizeManager
    {
        private Vector2 screenSize;
        private GraphicsDeviceManager graphics;
        private GameWindow Window;

        public WindowSizeManager(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }

        public void SetScreenSize(Vector2 screenSizeUpdate, Boolean toggleFullScreen)
        {
            this.screenSize = screenSizeUpdate;

            graphics.PreferredBackBufferWidth = (int)screenSizeUpdate.X;
            graphics.PreferredBackBufferHeight = (int)screenSizeUpdate.Y;
            if (toggleFullScreen == true)
            {
                graphics.IsFullScreen = true;
            }
            else
            {
                graphics.IsFullScreen = false;
            }
            graphics.ApplyChanges();
        }

        public Vector2 GetScreenSize()
        {
            return screenSize;
        }

        public Boolean GetFullscreenState()
        {
            return graphics.IsFullScreen;
        }

    }
}
