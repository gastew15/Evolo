using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

/*
 *  StarByte FPSManager
 *  Author: G. Stewart
 *  Version: 9/24/14
 */

namespace StarByte.misc
{
    class FPSManager
    {
        private int FPS, FrameCount;
        private double ElapsedMilliseconds;

        public FPSManager()
        {

        }

        public void Update(GameTime gameTime)
        {
            ElapsedMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (ElapsedMilliseconds > 1000)
            {
                ElapsedMilliseconds -= 1000;
                FPS = FrameCount;
                FrameCount = 0;
            }
        }

        public int getFPS()
        {
            return FPS;
        }

        public int getFrameCount()
        {
            return FrameCount;
        }

        public void updateFrameCount()
        {
            this.FrameCount += 1;
        }

    }
}
