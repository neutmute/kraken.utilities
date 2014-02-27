using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;

namespace Kraken.Core
{
    public class FileArchiver
    {
        #region Fields
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        #endregion
        
        #region Static Methods

        public static void Move(string sourceFilePath, string destinationDirectory)
        {
            Directory.CreateDirectory(destinationDirectory);
            string fileName = Path.GetFileName(sourceFilePath);
            string destinationFilePath = Path.Combine(destinationDirectory, fileName);
            Log.Info("Moving {0} to {1}", fileName, new DirectoryInfo(destinationDirectory).Name);
            File.Move(sourceFilePath, destinationFilePath);
        }

        public static void Copy(string sourceFilePath, string destinationDirectory)
        {
            Directory.CreateDirectory(destinationDirectory);
            string fileName = Path.GetFileName(sourceFilePath);
            string destinationFilePath = Path.Combine(destinationDirectory, fileName);
            Log.Info("Copying {0} to {1}", fileName, new DirectoryInfo(destinationDirectory).Name);
            File.Copy(sourceFilePath, destinationFilePath);
        }

        /// <summary>
        /// Allows the same filename to be sent twice and archived to the same folder 
        /// </summary>
        public static void ArchiveFile(string sourceFilePath, string destinationDirectory)
        {
            Directory.CreateDirectory(destinationDirectory);
            string fileName = Path.GetFileName(sourceFilePath);
            string destinationFilePath = Path.Combine(destinationDirectory, fileName);


            int attemptCounter = 0;
            while (File.Exists(destinationFilePath) && attemptCounter++ < 1000)
            {
                Log.Trace("File {0} already exists. Adjusting archive target.", destinationFilePath);
                destinationFilePath += "." + attemptCounter;
            }

            Log.Info("Archiving {0} to {1}", sourceFilePath, destinationFilePath);

            File.Move(sourceFilePath, destinationFilePath);
        }

        #endregion
    }
}
