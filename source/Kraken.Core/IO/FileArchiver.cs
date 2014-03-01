using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Logging;

namespace Kraken.Core
{
    public class FileArchiver
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion
        
        #region Static Methods

        public static void Move(string sourceFilePath, string destinationDirectory)
        {
            Directory.CreateDirectory(destinationDirectory);
            string fileName = Path.GetFileName(sourceFilePath);
            string destinationFilePath = Path.Combine(destinationDirectory, fileName);
            Log.Info(m => m("Moving {0} to {1}", fileName, new DirectoryInfo(destinationDirectory).Name));
            File.Move(sourceFilePath, destinationFilePath);
        }

        public static void Copy(string sourceFilePath, string destinationDirectory)
        {
            Directory.CreateDirectory(destinationDirectory);
            string fileName = Path.GetFileName(sourceFilePath);
            string destinationFilePath = Path.Combine(destinationDirectory, fileName);
            Log.Info(m => m("Copying {0} to {1}", fileName, new DirectoryInfo(destinationDirectory).Name));
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
                Log.Trace(m => m("File {0} already exists. Adjusting archive target.", destinationFilePath));
                destinationFilePath += "." + attemptCounter;
            }

            Log.Info(m => m("Archiving {0} to {1}", sourceFilePath, destinationFilePath));

            File.Move(sourceFilePath, destinationFilePath);
        }

        #endregion
    }
}
