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
        private Boolean trippedRed, trippedGreen, trippedBlue;
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

            blockColor = orginalBlockColor + pixelsRequiredToChange * Math.Abs(startPosition.Y - currentPosition.Y);
            /**
            //Red Update Color Main Logic Block
            if ((int)pixelsRequiredToChange.X != 0 && pixelsRequiredToChange != null)
            {
                //Checks to see if the required distance of pixels has passed
                if ((float)Math.Abs(startPosition.Y - currentPosition.Y) % (pixelsRequiredToChange.X) == 0)
                {
                    //Checks if a color is able & ready to increase/decrease
                    if (blockColor.X + 1 <= 255 && trippedRed == false)
                    {
                        //Decides if it should increment up or down in value
                        if (pixelsRequiredToChange.X > 0)
                            blockColor.X = blockColor.X + 1;
                        else
                            blockColor.X = blockColor.X - 1;

                        //checks to see if the value is less than zero, so that it will set it to the top. (For decreasing)
                        if (blockColor.X < 0)
                            blockColor.X = blockColor.X + 255;

                        trippedRed = true;
                    }
                }
                else
                {
                    trippedRed = false;
                }

            } //End of Red Update

            //Green Update Color Main Logic Block
            if ((int)pixelsRequiredToChange.Y != 0 && pixelsRequiredToChange != null)
            {
                //Checks to see if the required distance of pixels has passed
                if ((float)Math.Abs(startPosition.Y - currentPosition.Y) % (pixelsRequiredToChange.Y) == 0)
                {
                    //Checks if a color is able & ready to increase/decrease
                    if (blockColor.Y + 1 <= 255 && trippedGreen == false)
                    {
                        //Decides if it should increment up or down in value
                        if(pixelsRequiredToChange.Y > 0)
                            blockColor.Y = blockColor.Y + 1;
                        else
                            blockColor.Y = blockColor.Y - 1;

                        //checks to see if the value is less than zero, so that it will set it to the top. (For decreasing)
                        if (blockColor.Y < 0)
                            blockColor.Y = blockColor.Y + 255;

                        trippedGreen = true;
                    }
                }
                else
                {
                    trippedGreen = false;
                }

            }

            //Blue Update Color Main Logic Block
            if ((int)pixelsRequiredToChange.Z != 0 && pixelsRequiredToChange != null)
            {
                //Checks to see if the required distance of pixels has passed
                if ((float)Math.Abs(startPosition.Y - currentPosition.Y) % (pixelsRequiredToChange.Z) == 0)
                {
                    //Checks if a color is able & ready to increase/decrease
                    if (blockColor.Z + 1 <= 255 && trippedBlue == false)
                    {
                        //Decides if it should increment up or down in value
                        if (pixelsRequiredToChange.Z > 0)
                            blockColor.Z = blockColor.Z + 1;
                        else
                            blockColor.Z = blockColor.Z - 1;

                        //checks to see if the value is less than zero, so that it will set it to the top. (For decreasing)
                        if (blockColor.Z < 0)
                            blockColor.Z = blockColor.Z + 255;

                        trippedBlue = true;
                    }
                }
                else
                {
                    trippedBlue = false;
                }

            }
            */
            //Sets the color to selected R.G.B. Values (Color ranges from 0.0 to 1.0, so it's divided by 255)
            bBlockDrawColor = new Color(blockColor.X / 255, blockColor.Y / 255, blockColor.Z / 255);
        }

        public Color GetColor()
        {
            return bBlockDrawColor;
        }
        public int getDist()
        {
            return totalDistanceToTravel;
        }
        public void SetColor(Vector3 blockColor)
        {
            this.blockColor = blockColor;
        }
    }
}
