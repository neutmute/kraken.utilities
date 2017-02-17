using System;
using System.IO;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Linq;

namespace Kraken.Tests
{
	/// <summary>
	/// Provide some more assertion helpers.
	/// </summary>
	public class AssertTool
	{
        #region Properties
        /// <summary>
        /// Set the temporary directory so that files being compared are created in this folder and not the temp folder
        /// </summary>
        public static string TemporaryDirectory
	    {
    	    get;set;   
	    }
        #endregion

		#region ResourceEquals
		/// <summary>
		/// Compares a resource against a file that has been generated
		/// </summary>
		public static void ResourceEquals(string expectedResource, FileInfo file, params int[] ignoreLineNumbers)
		{
			ResourceEquals(Assembly.GetCallingAssembly(), expectedResource, file, ignoreLineNumbers);
		}

		/// <summary>
		/// Compares a resource against an object. 
		/// The object will have ToString() called on it to produce the content to compare against.
		/// </summary>
		public static void ResourceEquals(string expectedResource, object objectValue, params int[] ignoreLineNumbers)
		{
			ResourceEquals(Assembly.GetCallingAssembly(), expectedResource, objectValue.ToString(), ignoreLineNumbers);
		}

		/// <summary>
		/// Compare a string against a resource
		/// </summary>
		public static void ResourceEquals(string expectedResource, string actualValue, params int[] ignoreLineNumbers)
		{
			ResourceEquals(Assembly.GetCallingAssembly(), expectedResource, actualValue, ignoreLineNumbers);
		}

        /// <summary>
        /// Compare a string against a resource
        /// </summary>
        public static void ResourceEquals(Assembly assembly, string expectedResource, string actualValue, params int[] ignoreLineNumbers)
		{
			string tempFilename = Path.GetTempFileName();

            if (!string.IsNullOrEmpty(TemporaryDirectory))
            {
                tempFilename = Path.Combine(TemporaryDirectory, Path.GetFileName(tempFilename));
            }

            File.WriteAllText(tempFilename, actualValue);
            ResourceEquals(assembly, expectedResource, new FileInfo(tempFilename), ignoreLineNumbers);
		}

		/// <summary>
		/// Compare a file against a resource, specifying the assembly
		/// </summary>
		public static void ResourceEquals(Assembly assembly, string expectedResource,  FileInfo file, params int[] ignoreLineNumbers)
		{
            if (file == null)
                throw new ArgumentNullException("file");

			string resourceFile = Path.Combine(file.Directory.FullName, "expected.txt");
            new ResourceExtractor().ExportToFile(assembly, expectedResource, resourceFile);
			FilesEqual(resourceFile, file.FullName, ignoreLineNumbers);
		}
		#endregion

		#region FilesEqual
		/// <summary>
		/// Compares two files and asserts that they are identical in content
		/// </summary>
		public static void FilesEqual(string expectedFileName, string actualFileName)
		{
			FilesEqual(expectedFileName, actualFileName, Path.GetFileName(expectedFileName));
		}

		/// <summary>
		/// Compares two files and asserts that they are identical in content
		/// </summary>
		public static void FilesEqual(string expectedFileName, string actualFileName, params int[] ignoreLineNumbers)
		{
			FilesEqual(expectedFileName, actualFileName, Path.GetFileName(expectedFileName), ignoreLineNumbers);
		}

		/// <summary>
		/// Extracts a resource from the calling assembly and dumps it's contents into the specified file
		/// </summary>
		/// <param name="expectedFileName">The full path to the file that contains the expected content</param>
		/// <param name="actualFileName">The full path to the file that contains the actual content generated during the test</param>
		/// <param name="friendlyName">The friendly name will be used during error output</param>
		/// <param name="ignoreLineNumbers">A list of line numbers where mismatches should be ignored</param>
		public static void FilesEqual(string expectedFileName, string actualFileName, string friendlyName, params int[] ignoreLineNumbers)
		{
			string expectedLine = String.Empty;
			string actualLine	= String.Empty;

			string failHeader	= "\r\nFile Check Failed:\t" + friendlyName
				+ "\r\nExpected:\t\t" + expectedFileName
				+ "\r\nActual:\t\t" + actualFileName;
			string failError;

            string[] expectedContent = File.ReadAllLines(expectedFileName);
            string[] actualContent = File.ReadAllLines(actualFileName);
		    
            for(int lineIndex = 0; lineIndex < expectedContent.Length && lineIndex < actualContent.Length; lineIndex++)
			{
                expectedLine    = expectedContent[lineIndex];
				actualLine	    = actualContent[lineIndex];
				failError		= failHeader + "\r\nLine Number:\t\t" + (lineIndex+1);

				if (!ignoreLineNumbers.Contains(lineIndex+1))
				{
                    TestFrameworkFacade.AssertEqual(expectedLine, actualLine, failError);
				}
			}

            if (expectedContent.Length != actualContent.Length)
		    {
                TestFrameworkFacade.AssertFail(
                    "{0}\r\nExpected line count={1}, actual line count={2}, although they matched until the end of the shortest file was reached"
                    , failHeader
                    , expectedContent.Length
                    , actualContent.Length);
		    }
		}
		#endregion

		#region Static Methods

        /// <summary>
        /// Someone wrote a unit test with ReferenceEquals assertions and so wrote something pretty useless.
        /// This prevents the accident
        /// </summary>
        public static new bool ReferenceEquals(object objectA, object objectB)
        {
            throw TestMonkeyException.Create("You probably meant to call ResourceEquals not ReferenceEquals. Try again cowboy");
        }
		#endregion
	}
}
