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
        private Vector3 pixelsRequiredToIncrease = new Vector3(0, 0, 0);
        private int totalDistanceToTravel;

        public ColorGradient(Vector3 orginalBlockColor, Vector3 endBlockColor)
        {
            this.orginalBlockColor = orginalBlockColor;
            this.endBlockColor = endBlockColor;
        }

        public void Update(Vector2 position, int totalDistanceToTravel)
        {
            this.totalDistanceToTravel = totalDistanceToTravel;

            //Amount of pixels that are required to go by before the color changes values either up or down.
            pixelsRequiredToIncrease.X = (float)(totalDistanceToTravel / (endBlockColor.X - orginalBlockColor.X));
            pixelsRequiredToIncrease.Y = (float)(totalDistanceToTravel / (endBlockColor.Y - orginalBlockColor.Y));
            pixelsRequiredToIncrease.Z = (float)(totalDistanceToTravel / (endBlockColor.Z - orginalBlockColor.Z));

            //Red Update Color Main Logic Block
            if ((int)pixelsRequiredToIncrease.X != 0 && pixelsRequiredToIncrease != null)
            {
                //Checks to see if the required distance of pixels has passed
                if (position.Y % Math.Round(pixelsRequiredToIncrease.X) == 0)
                {
                    //Checks if a color is able & ready to increase/decrease
                    if (blockColor.X + 1 <= 255 && trippedRed == false)
                    {
                        //Decides if it should increment up or down in value
                        if (pixelsRequiredToIncrease.X > 0)
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
            if ((int)pixelsRequiredToIncrease.Y != 0 && pixelsRequiredToIncrease != null)
            {
                //Checks to see if the required distance of pixels has passed
                if (position.Y % Math.Round(pixelsRequiredToIncrease.Y) == 0)
                {
                    //Checks if a color is able & ready to increase/decrease
                    if (blockColor.Y + 1 <= 255 && trippedGreen == false)
                    {
                        //Decides if it should increment up or down in value
                        if(pixelsRequiredToIncrease.Y > 0)
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
            if ((int)pixelsRequiredToIncrease.Z != 0 && pixelsRequiredToIncrease != null)
            {
                //Checks to see if the required distance of pixels has passed
                if (position.Y % Math.Round(pixelsRequiredToIncrease.Z) == 0)
                {
                    //Checks if a color is able & ready to increase/decrease
                    if (blockColor.Z + 1 <= 255 && trippedBlue == false)
                    {
                        //Decides if it should increment up or down in value
                        if (pixelsRequiredToIncrease.Z > 0)
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

            //Sets the color to selected R.G.B. Values (Color ranges from 0.0 to 1.0, so it's divided by 255)
            bBlockDrawColor = new Color(blockColor.X / 255, blockColor.Y / 255, blockColor.Z / 255);
        }

        public Color GetColor()
        {
            return bBlockDrawColor;
        }

        public void SetColor(Vector3 blockColor)
        {
            this.blockColor = blockColor;
        }
    }
}
