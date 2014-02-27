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
        //public static byte[] GetBytes(string relativeLocation)
        //{
        //    return GetBytes(Assembly.GetCallingAssembly(), relativeLocation);
        //}

        //public static byte[] GetString(string relativeLocation)
        //{
        //    return GetBytes(Assembly.GetCallingAssembly(), relativeLocation);
        //}

        //public static byte[] GetBytes(Assembly assembly, string relativeLocation)
        //{
        //    FileInfo assemblyFile = new FileInfo(assembly.Location);
        //    string possibleFileLocation = Path.Combine(assemblyFile.DirectoryName, relativeLocation);
        //    FileInfo fileInfo = new FileInfo(possibleFileLocation);

        //    byte[] bytes;

        //    if (fileInfo.Exists)
        //    {
        //        bytes = File.ReadAllBytes(fileInfo.FullName);
        //    }
        //    else
        //    {
        //        throw new NotImplementedException("embedded resource needs implementation or file not found - " + possibleFileLocation);
        //    }

        //    return bytes;
        //}

        ///// <summary>
        ///// Extracts a resource from the given assembly and dumps it's contents into a byte array
        ///// </summary>
        //private static byte[] GetBytesFromEmbedded(Assembly assembly, string resource)
        //{
        //    byte[] bytes;

        //    using (var resourceStream = GetStream(assembly, resource))
        //    {
        //        int streamLength = Convert.ToInt32(resourceStream.Length);
        //        bytes = new byte[streamLength];
        //        resourceStream.Read(bytes, 0, streamLength);
        //    }

        //    return bytes;
        //}

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
            StreamReader streamReader = new StreamReader(stream);

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

    }
}
