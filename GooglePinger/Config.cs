using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
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
        public static string Name { get; } = "ipConfig.cfg";

        public static void setup()
        {
            while (true)
            {
                //fixing errors if any were found
                List<byte> err = check();
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
                if (err.Count == 0)
                {
                    Content = File.ReadAllLines(Name);
                    break;
                }    
            }
            
        }


        /* mistake codes and what they mean
        0 - All good
        1 - This file does not exist
        2 - First ip is incorrect or missing(only IPv4 is acceptable)
        3 - Global IP(or link) is incorrect or missing (It's not checking if the link is actually leading somewhere)
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
                //i am returning it instantly cuz i dont wanna deal wit "index out of range" later
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

            //I cant remember any other things i should check
            return errorLog;
        }
    }
}
