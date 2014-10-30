using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

/*
 *  StarByte SaveHandler
 *  Author: G. Stewart
 *  Version: 10/15/14
 */

namespace StarByte.io
{
    class SaveHandler
    {
        private int numberOfSaveSlots, numberOfLinesPerSlot;
        private String writeLocation;
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("j8hek4rl93hjtq9b");
        private static readonly string passPhrase = "Hg564jkDhyt3";

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        /*
         * Main Constructor for the SaveHandler class
         * Handles all of the first time start up information
         */
        public SaveHandler(int numberOfSaveSlots, int numberOfLinesPerSlot, String fileLocation, String fileName)
        {
            this.numberOfSaveSlots = numberOfSaveSlots;
            this.numberOfLinesPerSlot = numberOfLinesPerSlot;
            this.writeLocation = fileLocation + "\\" + fileName;

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

                    for (int j = 0; j < numberOfSaveSlots * numberOfLinesPerSlot; j++)
                    {
                        sw.WriteLine(" ");
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

            writeLineData = previousLineData;

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
                    sw.WriteLine(EncryptData(writeLineData[j], passPhrase)); 
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
                    previousLineData[lineReadNumber] = DecryptData(line, passPhrase);
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

        /*
         * Method that takes in plain String data and converts it into cypherText
         */
        private static string EncryptData(string plainText, string passPhrase)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        /*
         * Method that takes in cypher text and converts it into plain text
         */
        private static string DecryptData(string cipherText, string passPhrase)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }
      
    }
}