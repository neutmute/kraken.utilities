using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kraken.Core.Extensions
{
    public static class FileInfoExtensions
    {
        public static MemoryStream ToMemoryStream(this FileInfo fileInfo)
        {
            Stream fileStream = File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Write);
            BinaryReader br = new BinaryReader(fileStream);
            MemoryStream memoryStream = new MemoryStream(fileStream.ReadAllBytes());
            br.Close();
            fileStream.Close();
            fileStream.Dispose();

            return memoryStream;
        }
    }
}
