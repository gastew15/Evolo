using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Evolo.GameClass
{
    class WindowSizeManager
    {
        private Vector2 screenSize;
        private GraphicsDeviceManager graphics;
        private Boolean isFullScreen;
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
            return isFullScreen;
        }

    }
}
