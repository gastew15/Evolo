using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Evolo.StarByte.graphics
{
    class ColorGradient
    {
        Color drawColor;
        //Set Variables Testing
        Vector3 blockColor = new Vector3(0, 0, 0);
        Vector3 orginalBlockColor = new Vector3(0, 0, 0);
        Vector3 endBlockColor = new Vector3(255, 255, 255);
        Boolean trippedRed, trippedGreen, trippedBlue;

        //Variables
        Vector3 pixelsRequiredToIncrease = new Vector3(0, 0, 0);
        int totalDistanceToTravel;

        public ColorGradient()
        {

        }

        public void Initilize()
        {

        }

        public void Update(float millisecondsElapsedGameTime, float distanceToTravelTotal)
        {
            totalDistanceToTravel = (int)(GlobalVar.ScreenSize.Y);

            pixelsRequiredToIncrease.X = (float)(totalDistanceToTravel / (endBlockColor.X - orginalBlockColor.X));
            pixelsRequiredToIncrease.Y = (float)(totalDistanceToTravel / (endBlockColor.Y - orginalBlockColor.Y));
            pixelsRequiredToIncrease.Z = (float)(totalDistanceToTravel / (endBlockColor.Z - orginalBlockColor.Z));

            if (pixelsRequiredToIncrease.X > 1 && pixelsRequiredToIncrease != null)
            {

                if (distanceToTravelTotal % Math.Round(pixelsRequiredToIncrease.X) == 0)
                {
                    if (blockColor.X + 1 <= 255 && trippedRed == false)
                    {
                        blockColor.X = blockColor.X + 1;
                        trippedRed = true;
                    }
                }
                else
                {
                    trippedRed = false;
                }

            }
            if (pixelsRequiredToIncrease.Y > 1 && pixelsRequiredToIncrease != null)
            {

                if (distanceToTravelTotal % Math.Round(pixelsRequiredToIncrease.Y) == 0)
                {
                    if (blockColor.Y + 1 <= 255 && trippedGreen == false)
                    {
                        blockColor.Y = blockColor.Y + 1;
                        trippedGreen = true;
                    }
                }
                else
                {
                    trippedGreen = false;
                }

            }
            if (pixelsRequiredToIncrease.Z > 1 && pixelsRequiredToIncrease != null)
            {

                if (distanceToTravelTotal % Math.Round(pixelsRequiredToIncrease.Z) == 0)
                {
                    if (blockColor.Z + 1 <= 255 && trippedBlue == false)
                    {
                        blockColor.Z = blockColor.Z + 1;
                        trippedBlue = true;
                    }
                }
                else
                {
                    trippedBlue = false;
                }

            }

            drawColor = new Color(blockColor.X / 255, blockColor.Y / 255, blockColor.Z / 255);
        }

        public Color getDrawColor()
        {
            return drawColor;
        }

    }
}
