using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kraken.Core.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// http://stackoverflow.com/questions/221925/creating-a-byte-array-from-a-stream
        /// </summary>
        public static byte[] ReadAllBytes(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
