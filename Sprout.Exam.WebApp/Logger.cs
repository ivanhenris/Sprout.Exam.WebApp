using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp
{
    public class Logger
    {
        public string CurrentDirectory { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public Logger()
        {
            this.CurrentDirectory = Directory.GetCurrentDirectory();
            this.FileName = "Log.txt";
            this.FilePath = this.CurrentDirectory + "/" + this.FileName;
        }

        public void Log(string message, string stackTrace)
        {
            using (System.IO.StreamWriter w = System.IO.File.AppendText(this.FilePath))
            {
                w.Write("\r\nLog Entry: ");
                w.WriteLine($"{DateTime.Now.ToString()}");
                w.WriteLine($"Error Message:{message}");
                w.WriteLine($"Stack Trace:{stackTrace}");
                w.WriteLine("--------------------------------------------------");
            }
        }
    }
}
