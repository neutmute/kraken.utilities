using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Kraken.Core.Extensions;

namespace Kraken.Core
{
    public static class Resource
    {
        public static string GetStringFromEmbedded(string resource)
        {
            return GetStringFromEmbedded(Assembly.GetCallingAssembly(), resource);
        }

        public static string GetStringFromEmbedded(string resourcePrefix, string resource)
        {
            return GetStringFromEmbedded(Assembly.GetCallingAssembly(), resourcePrefix + resource);
        }

        public static string GetStringFromEmbedded(Assembly assembly, string resourcePrefix, string resource)
        {
            return GetStringFromEmbedded(assembly, resourcePrefix + resource);
        }

        /// <summary>
        /// Extract a resource from a specified assembly and return as string.
        /// </summary>
        public static string GetStringFromEmbedded(Assembly assembly, string resource)
        {
            var stream = GetStream(assembly, resource);
            var streamReader = new StreamReader(stream);

            string content = streamReader.ReadToEnd();

            streamReader.Close();
            stream.Close();

            return content;
        }

        private static Stream GetStream(Assembly assembly, string resource)
        {
            Stream stream = assembly.GetManifestResourceStream(resource);
            if (stream == null)
            {
                //Whale.Domain.Messaging.Resources.AlarmNotificationTemplate.cshtml
                //Whale.Domain.Messaging.Resources.AlarmNotificationEmail.cshtml
                throw KrakenException.Create("Embedded Resource was not found: {0}::{1}", assembly.GetName().Name, resource);
            }
            return stream;
        }


        /// <summary>
        /// Extracts a resource from the given assembly and dumps it's contents into the specified file
        /// </summary>
        /// <remarks>
        /// Binary safe method so jpegs etc will work
        /// </remarks>
        public static void ExportToFile(Assembly assembly, string resource, string fileName)
        {
            Stream resourceStream = assembly.GetManifestResourceStream(resource);

            if (resourceStream == null)
            {
                throw KrakenException.Create("Resource '{0}' was expected in assembly '{1}' but was not found.", resource, assembly.Location);
            }

            var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            var bw = new BinaryWriter(fs);
            var br = new BinaryReader(resourceStream);

            bw.Write(br.ReadBytes((int)resourceStream.Length));

            bw.Close();
            br.Close();
            fs.Close();
        }

        /// <summary>
        /// Extracts a resource from the calling assembly and dumps it's contents into the specified file
        /// </summary>
        /// <remarks>
        /// Binary safe method so jpegs etc will work
        /// </remarks>
        public static void ExportToFile(string resource, string fileName)
        {
            Assembly callingAssembly = Assembly.GetCallingAssembly();
            ExportToFile(callingAssembly, resource, fileName);
        }

        /// <summary>
        /// Extracts a resource from the given assembly and dumps it's contents into a byte array
        /// </summary>
        public static byte[] ExportToBinary(string resource)
        {
            return ExportToBinary(Assembly.GetCallingAssembly(), resource);
        }

        /// <summary>
        /// Extracts a resource from the given assembly and dumps it's contents into a byte array
        /// </summary>
        public static byte[] ExportToBinary(Assembly assembly, string resource)
        {
            byte[] binaryFile;

            using (Stream resourceStream = assembly.GetManifestResourceStream(resource))
            {
                if (resourceStream == null)
                {
                    throw KrakenException.Create("Resource '{0}' was expected in assembly '{1}' but was not found.", resource, assembly.Location);
                }

                int streamLength = Convert.ToInt32(resourceStream.Length);
                binaryFile = new byte[streamLength];
                resourceStream.Read(binaryFile, 0, streamLength);
            }

            return binaryFile;
        }
    }
}
