using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kraken.Core.Processes
{
    public class WindowsExplorerProcess : KrakenProcess
    {
        public WindowsExplorerProcess()
        {
            FriendlyName = "Windows Explorer";
            Filename = Path.Combine(Environment.GetEnvironmentVariable("windir"), "explorer.exe");
        }

        public void Start(DirectoryInfo directoryInfo)
        {
            Arguments = string.Format(" \"{0}\"", directoryInfo.FullName);
            Start();
        }

        public void Start(FileInfo fileInfo)
        {
            Start(fileInfo.FullName);
        }

        public void Start(string path)
        {
            Arguments = string.Format(" /select,\"{0}\"", path);
            Start();
        }
    }
}
