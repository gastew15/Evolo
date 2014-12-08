using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using StarByte.io;

namespace Evolo.GameClass
{
    class OptionsHandler
    {
        private string[] optionsArray;
        private static string[] optionsDefinitionArray = new string[14] { "ScreenWidth:", "ScreenHeight:", "keyPlayerLeft:", "keyPlayerRight:", "keyPlayerUp:", "keyTetrominoLeft:", "keyTetrominoRight:", "keyTetrominoRotate:", "keyTetrominoDown:", "HardwareCursor:", "FPSOverlay:", "DebugInfo:", "SoundToggle:", "MusicToggle:" };
        private string folderLocation;
        private ErrorHandler errorHandler = new ErrorHandler(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Evolo");

        public OptionsHandler(string folderLocation)
        {
            this.folderLocation = folderLocation;
            //TEMP / Defualt Values
            optionsArray = new string[14] { "1280", "720", "A", "D", "W", "Left", "Right", "Up", "Down", "false", "false", "false", "true", "true"};
        }

        public void writeOptions(string[] optionsArray)
        {
            this.optionsArray = optionsArray;
            //Write to File
            try
            {
                //System.IO.Directory.CreateDirectory(folderLocation);
                StreamWriter sw = new StreamWriter(folderLocation + "\\Options.txt", false, Encoding.ASCII);

                for (int j = 0; j < optionsArray.Length; j++)
                {
                    sw.WriteLine(optionsDefinitionArray[j] + " " + optionsArray[j]);
                }

                sw.Close();
            }
            catch(Exception e)
            {
                errorHandler.WriteError(2, 146, "Options Write Failure! " + e);
            }
        }

        public string[] loadOptions()
        {
            //Load File
            try
            {
                StreamReader sr = new StreamReader(folderLocation + "\\Options.txt");

                string line = sr.ReadLine();
                int lineReadNumber = 0;

                while(line != null)
                {
                    //Use Line Data Here
                    List<string> words = new List<string>(line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));

                    optionsArray[lineReadNumber] = words[1];

                    line = sr.ReadLine();
                    lineReadNumber++;
                }

                sr.Close();
            }
            catch(Exception e)
            {
                errorHandler.WriteError(1, 159, "Options Load Failure Attempting New File Creation(Ignore If First Start) " + e);
                try
                {
                    StreamWriter sw = new StreamWriter(folderLocation + "\\Options.txt", false, Encoding.ASCII);

                    for (int j = 0; j < optionsArray.Length; j++)
                    {
                        sw.WriteLine(optionsDefinitionArray[j] + " " + optionsArray[j]);
                    }

                    sw.Close();
                }
                catch(Exception o)
                {
                    errorHandler.WriteError(2, 174, "Options Creation Failure! " + o);
                }
            }

            return optionsArray;
        }
    }
}
