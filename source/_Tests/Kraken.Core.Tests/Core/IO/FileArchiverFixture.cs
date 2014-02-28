using System.IO;
using Kraken.Core;
using Kraken.Core.Tests;
using Kraken.Tests;
using NLog.Targets;
using NUnit.Framework;

namespace Kraken.Core.Tests
{
    [TestFixture]
    public class FileArchiverFixture : Fixture
    {
        #region Test Methods

        [Test]
        public void ArchiveFile()
        {
            MemoryTarget target = Log.GetMemoryTarget();

            // Create Source file
            string sourceFilePath = Path.Combine(TestTempDirectory, "TestFile.txt");
            File.WriteAllText(sourceFilePath, "Test Content");

            // Create Destination directory
            string destinationDirectory = Path.Combine(TestTempDirectory, "Archive");
            Directory.CreateDirectory(destinationDirectory);

            FileArchiver.ArchiveFile(sourceFilePath, destinationDirectory);

            // CodeGen.GenerateAssertions(target.Logs, "target.Logs"); // The following assertions were generated on 21-Feb-2011
            #region CodeGen Assertions
            Assert.AreEqual(1, target.Logs.Count);
            string expected = string.Format("Archiving {0}\\TestFile.txt to {0}\\Archive\\TestFile.txt", TestTempDirectory);
            Assert.AreEqual(expected, target.Logs[0]);
            #endregion

            string[] files = Directory.GetFiles(destinationDirectory);

            // CodeGen.GenerateAssertions(files, "files"); // The following assertions were generated on 21-Feb-2011
            #region CodeGen Assertions
            Assert.AreEqual(1, files.Length);
            Assert.AreEqual(TestTempDirectory + "\\Archive\\TestFile.txt", files[0]);
            #endregion
        }

        [Test]
        public void HandlesExistingFileName()
        {
            MemoryTarget target = Log.GetMemoryTarget();

            // Create Source file
            string sourceFilePath = Path.Combine(TestTempDirectory, "TestFile.txt");
            File.WriteAllText(sourceFilePath, "Test Content");
            
            // Create Destination file
            string destinationDirectory = Path.Combine(TestTempDirectory, "Archive");
            Directory.CreateDirectory(destinationDirectory);

            File.WriteAllText(Path.Combine(destinationDirectory, "TestFile.txt"), "Test Content");

            FileArchiver.ArchiveFile(sourceFilePath, destinationDirectory);

            // CodeGen.GenerateAssertions(target.Logs, "target.Logs"); // The following assertions were generated on 21-Feb-2011
            #region CodeGen Assertions
            Assert.AreEqual(1, target.Logs.Count);
            string expected = string.Format("Archiving {0}\\TestFile.txt to {0}\\Archive\\TestFile.txt.1", TestTempDirectory);
            Assert.AreEqual(expected, target.Logs[0]);
            #endregion

            string[] files = Directory.GetFiles(destinationDirectory);

            // CodeGen.GenerateAssertions(files, "files"); // The following assertions were generated on 21-Feb-2011
            #region CodeGen Assertions
            Assert.AreEqual(2, files.Length);
            Assert.AreEqual(TestTempDirectory + "\\Archive\\TestFile.txt", files[0]);
            Assert.AreEqual(TestTempDirectory + "\\Archive\\TestFile.txt.1", files[1]);
            #endregion
        }

        #endregion
    }
}
