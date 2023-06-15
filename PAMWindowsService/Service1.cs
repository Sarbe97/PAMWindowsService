using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace PAMWindowsService
{
    public partial class Service1 : ServiceBase
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string scriptType = ConfigurationManager.AppSettings["script_type"];
        string scriptName = ConfigurationManager.AppSettings["script_name"];
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service started at " + DateTime.Now);
            WriteToFile(scriptType);
            WriteToFile(scriptName);

            var ps1File = @baseDir+ "PAMScript.ps1";
            var startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy unrestricted -file \"{ps1File}\"",
                UseShellExecute = false
            };


            try
            {
                Process.Start(startInfo);
                WriteToFile("Succesfully Ran the script.");
            }
            catch (Exception ex)
            {
                WriteToFile("Could not run the script.");
            }

           
        }

        protected override void OnStop()
        {
            WriteToFile("Service sopped at " + DateTime.Now);

        }

        public void WriteToFile(string message)
        {
           string logDir = baseDir + "\\Logs";
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            string logFilePath = logDir + "\\PamService.log";
            if (!File.Exists(logFilePath)){
                using (StreamWriter sw = File.CreateText(logFilePath))
                {
                        sw.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    sw.WriteLine(message);
                }

            }
        }
    }
}
