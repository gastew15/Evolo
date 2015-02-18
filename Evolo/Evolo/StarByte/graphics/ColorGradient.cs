using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 *  StarByte Color Gradient System
 *  Author: G. Stewart
 *  Version: 11/3/14
 */

namespace StarByte.graphics
{
    class ColorGradient
    {
        private Color bBlockDrawColor;
        private Vector3 blockColor = new Vector3(0, 0, 0);
        private Vector3 orginalBlockColor;
        private Vector3 endBlockColor;
        private Vector3 pixelsRequiredToChange = new Vector3(0, 0, 0);
        private int totalDistanceToTravel;
        private Vector2 startPosition, currentPosition;
        public ColorGradient(Vector3 orginalBlockColor, Vector3 endBlockColor)
        {
            this.orginalBlockColor = orginalBlockColor;
            this.endBlockColor = endBlockColor;
        }

        public void Update(Vector2 startPosition, Vector2 currentPosition, int totalDistanceToTravel)
        {
            this.totalDistanceToTravel = totalDistanceToTravel;
            this.startPosition = startPosition;
            this.currentPosition = currentPosition;
            //Amount of pixels that are required to go by before the color changes values either up or down.
            pixelsRequiredToChange.X = (float)((endBlockColor.X - orginalBlockColor.X) / totalDistanceToTravel);
            pixelsRequiredToChange.Y = (float)((endBlockColor.Y - orginalBlockColor.Y) / totalDistanceToTravel);
            pixelsRequiredToChange.Z = (float)((endBlockColor.Z - orginalBlockColor.Z) / totalDistanceToTravel);
            if (Math.Abs(startPosition.Y - currentPosition.Y) > totalDistanceToTravel)
                blockColor = orginalBlockColor + pixelsRequiredToChange * Math.Abs(startPosition.Y + totalDistanceToTravel);
            else
                blockColor = orginalBlockColor + pixelsRequiredToChange * Math.Abs(startPosition.Y - currentPosition.Y);

            //Sets the color to selected R.G.B. Values (Color ranges from 0.0 to 1.0, so it's divided by 255)
            // they are now devided by 1024 becasue I don't know but it works and 255 makes it 4.1023.... times brighter than the actual values should represent
            bBlockDrawColor = new Color(blockColor.X / 255f, blockColor.Y / 255f, blockColor.Z / 255f);
        }

        public Color GetColor()
        {
            return bBlockDrawColor;
        }
        public float GetRed()
        {
            return bBlockDrawColor.R;
        }
        public float GetGreen()
        {
            return bBlockDrawColor.G;
        }
        public float GetBlue()
        {
            return bBlockDrawColor.B;
        }
        public int getDist()
        {
            return totalDistanceToTravel;
        }
        public void SetColor()//Vector3 blockColor)
        {
            this.blockColor = new Vector3(255,255,255);
        }
        public void SetStar()
        {
            this.bBlockDrawColor = new Color(255 / 255f, 255 / 255f, 255 / 255f);
        }
    }
}
