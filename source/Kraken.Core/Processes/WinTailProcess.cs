using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kraken.Core.Processes
{
    public class WinTailProcess : KrakenProcess
    {
        public WinTailProcess()
        {
            FriendlyName = "WinTail";
            Filename = GetWinTailPath();
        }

        public void Start(string logFile)
        {
            Arguments = string.Format("\"{0}\"", logFile);
            Start();
        }

        /// <summary>
        /// Don't use the debug version in VStudio cause then you can't recompile
        /// </summary>
        private static string GetWinTailPath()
        {
            string path = null;
            string tailerSearchPaths = ConfigurationManager.AppSettings["Tailer.SearchPaths"];

            if (!string.IsNullOrEmpty(tailerSearchPaths))
            {
                if (File.Exists(tailerSearchPaths))
                {
                    path = tailerSearchPaths;
                }
            }

            if (string.IsNullOrEmpty(path))
            {
                string exePath = Assembly.GetEntryAssembly().Location;
                string exeFolder = Path.GetDirectoryName(exePath);
                path = Path.Combine(exeFolder, @"Tools\WinTail.exe");
            }
            
            return path;
        }
    }
}
