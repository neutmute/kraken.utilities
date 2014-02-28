using System;
using System.IO;
using System.Data;
using System.Reflection;
using System.Collections;

namespace Kraken.Tests
{
    /// <summary>
    /// Resource provides some methods for working with resources and getting them into files or strings
    /// which is a common task in unit tests
    /// </summary>
    public class ResourceExtractor
    {
        /// <summary>
        /// Extract a resource from this assembly and into a string
        /// </summary>
        /// <param name="resourceFormat">The relative path from the Resources folder to the resource</param>
        /// <param name="args">Allow strng</param>
        public  string ExportToString(string resourceFormat, params object[] args)
        {
            return ExportToString(Assembly.GetCallingAssembly(), string.Format(resourceFormat, args));
        }

        /// <summary>
        /// Extract a resource from a specified assembly and return as string.
        /// </summary>
        public  string ExportToString(Assembly assembly, string resource)
        {
            Stream stream = assembly.GetManifestResourceStream(resource);

            if (stream == null)
            {
                throw new ArgumentException(resource + " resource not found");
            }

            StreamReader streamReader = new StreamReader(stream);
            string content = streamReader.ReadToEnd();

            streamReader.Close();
            stream.Close();

            return content;
        }

        /// <summary>
        /// Extracts a resource from the given assembly and dumps it's contents into the specified file
        /// </summary>
        /// <remarks>
        /// Binary safe method so jpegs etc will work
        /// </remarks>
        public  void ExportToFile(Assembly assembly, string resource, string fileName)
        {
            Stream resourceStream = assembly.GetManifestResourceStream(resource);

            if (resourceStream == null)
            {
                throw new ArgumentException(resource + " resource not found");
            }

            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            BinaryWriter bw = new BinaryWriter(fs);
            BinaryReader br = new BinaryReader(resourceStream);

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
        public  void ExportToFile(string resource, string fileName)
        {
            Assembly callingAssembly = Assembly.GetCallingAssembly();
            ExportToFile(callingAssembly, resource, fileName);
        }

        /// <summary>
        /// Extracts a resource from the given assembly and dumps it's contents into a byte array
        /// </summary>
        public  byte[] ExportToBinary(string resource)
        {
            return ExportToBinary(Assembly.GetCallingAssembly(), resource);
        }

        /// <summary>
        /// Extracts a resource from the given assembly and dumps it's contents into a byte array
        /// </summary>
        public  byte[] ExportToBinary(Assembly assembly, string resource)
        {
            byte[] binaryFile;

            using (Stream resourceStream = assembly.GetManifestResourceStream(resource))
            {
                if (resourceStream == null)
                {
                    throw new ArgumentException(resource + " resource not found");
                }

                int streamLength = Convert.ToInt32(resourceStream.Length);
                binaryFile = new byte[streamLength];
                resourceStream.Read(binaryFile, 0, streamLength);
            }

            return binaryFile;
        }
    }
}
