using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GooglePinger
{
    static class Cmd //I was kinda annoyed by all the trash I had to do so I slaped yet another class
    {
        static private System.Diagnostics.Process cmd = new System.Diagnostics.Process();
        
        public static void Initialize()
        {
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.Verb = "runas";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
        }

        public static void Execute(string command)
        {


            cmd.StandardInput.WriteLine(command);
        }

        public static void Exit()
        {
            cmd.Kill();
        }
    }
}
