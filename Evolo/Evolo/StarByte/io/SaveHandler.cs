using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using StarByte.io;
using System.Security.Cryptography;

/*
 *  StarByte SaveHandler
 *  Author: G. Stewart
 *  Version: 1/30/15
 */

namespace StarByte.io
{
    class SaveHandler
    {
        private int numberOfSaveSlots, numberOfLinesPerSlot;
        private String writeLocation;

        // The Encoder data for encyption
        private EncoderSystem encoder;
        private const int keysize = 256;
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("j8hek4rl93hjtq9b");
        private static readonly string passPhrase = "Hg564jkDhyt3";

        /*
         * Main Constructor for the SaveHandler class
         * Handles all of the first time start up information
         */
        public SaveHandler(int numberOfSaveSlots, String[] defualtData, String fileLocation, String fileName)
        {
            this.numberOfSaveSlots = numberOfSaveSlots;
            this.numberOfLinesPerSlot = defualtData.Length;
            this.writeLocation = fileLocation + "\\" + fileName;

            encoder = new EncoderSystem(initVectorBytes, keysize);

            Boolean fileOk;

            //Checks to see if a file exists at the given location, and if it's a good file
            try
            {
                StreamReader sr = new StreamReader(writeLocation);

                String line = sr.ReadLine();
                int lineReadNumber = 0;

                while (line != null)
                {
                    line = sr.ReadLine();
                    lineReadNumber++;
                }

                sr.Close();

                if (lineReadNumber >= numberOfSaveSlots * numberOfLinesPerSlot)
                    fileOk = true;
                else
                    fileOk = false;
            }
            catch
            {
                fileOk = false;
            }

            //Creates a new Blank file if it's deemed not ok
            if (fileOk == false)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(writeLocation, false, Encoding.ASCII);

                    for (int j = 0; j < numberOfSaveSlots; j++)
                    {
                        for (int i = 0; i < numberOfLinesPerSlot; i++)
                        {
                            sw.WriteLine(encoder.EncryptData(defualtData[i], passPhrase));
                        }
                    }

                    sw.Close();
                }
                catch
                {

                }
            }
        }

        /*
         * Method to save a string array of information to write in a specific line area in the save file
         */
        public void saveData(String[] saveLineData, int saveSlot)
        {

            //Loads in old Line data  that's currently in the file 
            String[] previousLineData = new String[numberOfSaveSlots * numberOfLinesPerSlot];

            try
            {
                StreamReader sr = new StreamReader(writeLocation);

                int lineReadNumber = 0;
                String line = sr.ReadLine();

                while (line != null)
                {
                    previousLineData[lineReadNumber] = line;
                    line = sr.ReadLine();
                    lineReadNumber++;
                }

                sr.Close();
            }
            catch
            {

            }

            //Appends the new String array to the old String array in the respective location
            String[] writeLineData = new String[numberOfSaveSlots * numberOfLinesPerSlot];
            int runThroughs = 0;

            for (int j = 0; j < previousLineData.Length; j++)
            {
                writeLineData[j] = encoder.DecryptData(previousLineData[j], passPhrase);
            }

            for (int i = (saveSlot - 1) * numberOfLinesPerSlot; i < (saveSlot - 1) * numberOfLinesPerSlot + numberOfLinesPerSlot; i++)
            {
                writeLineData[i] = saveLineData[runThroughs];
                runThroughs++;
            }

            //Writes new LineData along with old Line Data to File

            try
            {
                StreamWriter sw = new StreamWriter(writeLocation, false, Encoding.ASCII);

                for (int j = 0; j < writeLineData.Length; j++)
                {
                    sw.WriteLine(encoder.EncryptData(writeLineData[j], passPhrase)); 
                }

                sw.Close();
            }
            catch
            {

            }
        }

        /*
         * Method to load and present back a string array of information from a specific line area in the save file
         */
        public String[] loadData(int saveSlot)
        {
            //Loads in old Line data  that's currently in the file 
            String[] previousLineData = new String[numberOfSaveSlots * numberOfLinesPerSlot];

            try
            {
                StreamReader sr = new StreamReader(writeLocation);

                int lineReadNumber = 0;
                String line = sr.ReadLine();

                while (line != null)
                {
                    previousLineData[lineReadNumber] = encoder.DecryptData(line, passPhrase);
                    line = sr.ReadLine();    
                    lineReadNumber++;
                }

                sr.Close();
            }
            catch
            {

            }

            //Only returns the given slot number instead of the whole file.
            String[] returnData = new String[numberOfLinesPerSlot];

            int runThroughs = 0;

            for (int i = (saveSlot - 1) * numberOfLinesPerSlot; i < ((saveSlot - 1) * numberOfLinesPerSlot) + numberOfLinesPerSlot; i++)
            {
                returnData[runThroughs] = previousLineData[i];
                runThroughs++;
            }

            return returnData;
        }   
    }
}