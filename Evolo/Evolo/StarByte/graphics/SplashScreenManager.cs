using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 *  StarByte SplashScreenManager
 *  Author: G. Stewart
 *  Version: 9/24/14
 */

namespace StarByte.graphics
{
    class SplashScreenManager
    {
        Texture2D[] splashImages;
        float[] waitTime;
        float expiredGameTime;
        int currentImage = 0;
        Boolean splashScreenOver;
        float miliSecondsElapsedGameTime;

        public SplashScreenManager(Texture2D[] splashImages, float[] waitTime)
        {
            this.splashImages = splashImages;
            this.waitTime = waitTime;
        }

        public void Update(float miliSecondsElapsedGameTime, float orginalStartTimeInSeconds)
        {
            this.miliSecondsElapsedGameTime = miliSecondsElapsedGameTime;

            if (currentImage == 0)
            {
                expiredGameTime = ((miliSecondsElapsedGameTime / 1000) - orginalStartTimeInSeconds);
            }
            else
            {
                for (int j = 0; j < currentImage; j++)
                {
                    expiredGameTime = ((miliSecondsElapsedGameTime / 1000) - (orginalStartTimeInSeconds + waitTime[j]));
                }
            }

            if (expiredGameTime >= waitTime[currentImage])
            {
                if (currentImage < waitTime.Length - 1)
                    currentImage++;
                else
                {
                    splashScreenOver = true;
                    currentImage = 0;
                }
            }

            //StartGameTime is a float that's sent when the currentImage changes and then grab the expiredGameTime since then?
        }

        public void Update(float miliSecondsElapsedGameTime, float orginalStartTimeInSeconds, Texture2D[] updatedSplashImages)
        {
            this.miliSecondsElapsedGameTime = miliSecondsElapsedGameTime;
            this.splashImages = updatedSplashImages;

            if (currentImage == 0)
            {
                expiredGameTime = ((miliSecondsElapsedGameTime / 1000) - orginalStartTimeInSeconds);
            }
            else
            {
                for (int j = 0; j < currentImage; j++)
                {
                    expiredGameTime = ((miliSecondsElapsedGameTime / 1000) - (orginalStartTimeInSeconds + waitTime[j]));
                }
            }

            if (expiredGameTime >= waitTime[currentImage])
            {
                if (currentImage < waitTime.Length - 1)
                    currentImage++;
                else
                {
                    splashScreenOver = true;
                    currentImage = 0;
                }
            }

            //StartGameTime is a float that's sent when the currentImage changes and then grab the expiredGameTime since then?
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 drawPos, Vector2 scaleSize, SpriteFont font)
        {
            if(splashScreenOver != true)
                spriteBatch.Draw(splashImages[currentImage], drawPos, null, Color.White, 0, new Vector2(0), scaleSize, SpriteEffects.None, 0);

            //DEBUGING USE: spriteBatch.DrawString(font, "SPLASH GAME TIME " + miliSecondsElapsedGameTime.ToString(), new Vector2(100, 100), Color.Black);
        }

        public Boolean getSplashScreenOver()
        {
            return splashScreenOver;
        }
    }
}
