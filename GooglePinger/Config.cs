using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GooglePinger
{
    static class Config
    {
        //0 - local
        //1 - global
        public static string[] Content { get; set; }
        public static string Name { get; } = System.IO.Directory.GetCurrentDirectory() + "\\" + "ipConfig.cfg";
        public static bool FWRon { get; set; }

        public static void setup()
        {
            while (true)
            {
                //fixing errors if any were found
                List<byte> err = check();
                bool FWerr = false;

                if (err.Contains(1))
                {
                    File.Create(Name).Close();
                    NetworkInterface[] cabels = NetworkInterface.GetAllNetworkInterfaces();
                    Stuff choosingWindow = new Stuff(cabels);
                    choosingWindow.ShowDialog();
                    Content[1] = "google.com";
                    File.WriteAllLines(Name, Content);

                }
                if (err.Contains(2))
                {
                    //why cant i name them the same way, doesn't switch work like if?
                    NetworkInterface[] cabelss = NetworkInterface.GetAllNetworkInterfaces();
                    Stuff choosingWindoww = new Stuff(cabelss);
                    choosingWindoww.ShowDialog();
                    File.WriteAllLines(Name, Content);
                }
                if (err.Contains(3))
                {
                    Content = File.ReadAllLines(Name);
                    Content[1] = "google.com";
                    File.WriteAllLines(Name, Content);
                    
                }
                if(err.Contains(4))
                {
                    MessageBox.Show("Seems like firewall rule is not set.\nMore about that on GitHub");
                    System.Diagnostics.Process.Start("explorer.exe", System.IO.Directory.GetCurrentDirectory());
                    FWerr = true;
                }
                if(err.Contains(5))
                {
                    MessageBox.Show("Insufficient privileges to change FW rules\nIf you set cfg string to false ignore this message");
                    FWerr = true;
                }
                if(err.Contains(6))
                {
                    MessageBox.Show("No default action\nMore about that on GitHub");
                    FWerr = true;
                }
                if (err.Count == 0)
                {
                    Content = File.ReadAllLines(Name);
                    break;
                }
                else if (hasOnlyFWErr(err))
                {
                    Content = File.ReadAllLines(Name);
                    try
                    { Content[2] = "false"; }
                    catch(IndexOutOfRangeException)
                    {
                        string[] ex = Content;
                        Content = new string[3];
                        Content[0] = ex[0];
                        Content[1] = ex[1];
                        Content[2] = "false";
                    }
                     //well, thats kinda dirty
                    break;
                }
            }
            
        }



        /* mistake codes and what they mean
        0 - All good
        1 - This file does not exist
        2 - First ip is incorrect or missing(only IPv4 is acceptable)
        3 - Global IP(or link) is incorrect or missing (It's not checking if the link is actually leading somewhere)
        4 - Policy name is not stated (and it is not false)
        5 - Policy name is stated but Software wasn't ran as administrator
        6 - Default sync method (action with firewall rule) is not set
        */

        private static List<byte> check()
        {
            List<byte> errorLog = new List<byte>();
            if (!File.Exists(Name))
            {
                errorLog.Add(1);
                return errorLog;
            }

            //amount of strings checked and etc is hardcoded (no sense in doing it other way)
            string[] preload = File.ReadAllLines(Name);

            //checking local IP
            int oneNum = 0, dots = 0;
            bool gotErrorErlier = false;
            //if cfg is completely empety "for" will crash, so I slapped try catch on top of this
            try
            {
                for (int i = 0; i < preload[0].Length; i++)
                {
                    //this has a down side of an end of the string counting as dot, but no that someone cared
                    if (preload[0][i] == '.')
                    {
                        dots++;
                        if (oneNum / 10 < 0 || oneNum / 10 > 255)
                        {
                            errorLog.Add(2);
                            gotErrorErlier = true;
                            break;
                            
                        }
                        oneNum = 0;
                    }
                    else
                    {
                        oneNum += preload[0][i] - 48;
                        oneNum *= 10;
                    }
                }
                if (oneNum / 10 < 0 || oneNum / 10 > 255)
                {
                    errorLog.Add(2);
                }
            }
            catch(Exception)
            {
                errorLog.Add(2);
                //I am returning it instantly cuz I dont wanna deal wit "index out of range" later
                return errorLog;
            }
            
            

            if (dots != 3 && !gotErrorErlier)
                errorLog.Add(2);
            //checking global IP
            try
            {
                if (!preload[1].Contains('.'))
                    errorLog.Add(3);
            }
            catch(Exception)
            {
                errorLog.Add(3);
            }

            if(preload.Length < 4)
            {
                errorLog.Add(4);
            }
            else
            {
                if (preload[3] == "false")
                {
                    errorLog.Add(6);
                } 
                else
                {
                    if (preload[2] == "")
                        errorLog.Add(4);
                    if (!IsAdministrator())
                        errorLog.Add(5);
                    if (preload[3] != "yes" && preload[3] != "no")
                        errorLog.Add(6);
                    //there should also be a "rule existing check", but it was hard to implement, maybe later...

                }
            }

            
            //I cant remember any other things I should check
            return errorLog;
        }

        private static bool hasOnlyFWErr(List<byte> err)
        {
            for(int i = 0; i < err.Count; i++)
            {
                if (err[i] != 4 && err[i] != 5 && err[i] != 6)
                    return false;
            }
            return true;
        }

        static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }  
}
