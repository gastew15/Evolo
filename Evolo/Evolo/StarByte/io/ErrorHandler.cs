using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using System.Management;
using Microsoft.Win32;

/*
 *  StarByte ErrorHandler (Have to refrence System.Management.dll)
 *  Author: G. Stewart
 *  Version: 9/24/14
 */

namespace StarByte.io
{
    public class ErrorHandler
    {

        private String errorMessage, folderLocation, gameName;
        private int errorNumber, errorLevel;
        private Boolean firstTimeFileCreation;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        public ErrorHandler(String folderLocation, String gameName)
        {
            this.folderLocation = folderLocation;
            this.gameName = gameName;
        }

        public void WriteError(int errorLevel, int errorNumber, String errorMessage)
        {
            this.errorLevel = errorLevel;
            this.errorNumber = errorNumber;
            this.errorMessage = errorMessage;

            DateTime nowDateTime = DateTime.Now;
            string nowString = nowDateTime.ToString();

            try
            {
                if (File.Exists(folderLocation + "\\" + gameName + "\\Log.txt") == false)
                {
                    System.IO.Directory.CreateDirectory(folderLocation + "\\" + gameName);
                    firstTimeFileCreation = true;
                }

                StreamWriter sw = new StreamWriter(folderLocation + "\\" + gameName + "\\Log.txt", true, Encoding.Unicode);

                if (firstTimeFileCreation == true)
                {
                    ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
                    RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);

                    foreach (ManagementObject managementObject in mos.Get())
                    {
                        sw.WriteLine(gameName + " Error Handler");
                        sw.WriteLine("");

                        sw.WriteLine("Operating System Info:");
                        if (managementObject["Caption"] != null)
                        {
                            sw.WriteLine("-" + managementObject["Caption"].ToString());   //Display operating system caption
                        }
                        if (managementObject["OSArchitecture"] != null)
                        {
                            sw.WriteLine("-" + managementObject["OSArchitecture"].ToString());   //Display operating system architecture.
                        }
                        if (managementObject["CSDVersion"] != null)
                        {
                            sw.WriteLine("-" + managementObject["CSDVersion"].ToString());     //Display operating system version.
                        }

                        sw.WriteLine("");
                        sw.WriteLine("Processor Info:");

                        if (processor_name != null)
                        {
                            if (processor_name.GetValue("ProcessorNameString") != null)
                            {
                                sw.WriteLine("-" + processor_name.GetValue("ProcessorNameString"));   //Display processor ingo.
                                sw.WriteLine("-" + processor_name.GetValue("Identifier"));
                                sw.WriteLine("-" + processor_name.GetValue("~MHz") + " Mhz");
                            }
                        }

                        sw.WriteLine("");
                        sw.WriteLine("Error Messages:");
                        firstTimeFileCreation = false;
                    }

                }

                if (errorLevel == 1)
                {
                    sw.WriteLine("");
                    sw.WriteLine(nowString + " Non Error" + " Message Number: " + errorNumber + " Message: " + errorMessage);
                }
                else if (errorLevel == 2)
                {
                    sw.WriteLine("");
                    sw.WriteLine( nowString + " Non- Critical Error" + " Error Number: " + errorNumber + " Error Message: " + errorMessage);
                }
                else if (errorLevel == 3)
                {
                    sw.WriteLine("");
                    sw.WriteLine(nowString + " Critical Error " + " Error Number: " + errorNumber + " Error Message: " + errorMessage);
                }
                sw.Close();

                if(errorLevel >= 2)
                    MessageBox(new IntPtr(0), "Oh no something went Wrong! " + "Error Number " + errorNumber + "\nConsult the log at " + folderLocation + "\\" + gameName + " for more detailed information", gameName + " Prompt", 0);

                if (errorLevel >= 3)
                    System.Diagnostics.Process.GetCurrentProcess().Kill();

            }
            catch (Exception e)
            {
                MessageBox(new IntPtr(0), "ERROR: Could not log event data - Non-Crit Faliure\nWARNING!-No Debugging data is being generated." + "\nException: " + e.Message, gameName + " Prompt", 0);
            }
        }
    }
}
