using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using FatCatGit.GitCommands;
using NUnit.Framework;

namespace FatCatGit.UnitTests.GitCommands
{
    [TestFixture]
    public class GitCloneCommandTests : BaseCommandTests
    {
        [TearDown]
        public new void TearDown()
        {
            DeleteDestinationDirectory();

            base.TearDown();
        }

        public string RepositoryDestination
        {
            get { return ConfigurationManager.AppSettings["GitTestCloneLocation"]; }
        }

        public void DeleteDestinationDirectory()
        {
            var directoryToDelete = new DirectoryInfo(RepositoryDestination);

            DeleteDirectory(directoryToDelete);
        }

        private static void DeleteDirectory(DirectoryInfo directoryToDelete)
        {
            if (!directoryToDelete.Exists)
            {
                return;
            }

            FileInfo[] files = directoryToDelete.GetFiles();
            DirectoryInfo[] dirs = directoryToDelete.GetDirectories();

            foreach (FileInfo file in files)
            {
                File.SetAttributes(file.FullName, FileAttributes.Normal);
                file.Delete();
            }

            foreach (DirectoryInfo dir in dirs)
            {
                DeleteDirectory(dir);
            }

            directoryToDelete.Delete(true);
        }

        [Test]
        public void ALocalRepoistoryCanBeCloned()
        {
            MockGitLocationForConfiguration();

            var clone = new Clone(GitEmptyTestProjectLocation)
                            {
                                RepositoryToClone = GitEmptyTestProjectLocation,
                                Destination = RepositoryDestination
                            };

            IAsyncResult result = clone.Run();

            result.AsyncWaitHandle.WaitOne();

            Assert.That(clone.Output.Contains(string.Format("Cloning into {0}...", RepositoryDestination)));
        }

        [Test]
        [Ignore("Test is taking too long.  TODO Must figure out a way to speed up")]
        public void WhenCloningARepositoryTheProgressIsProvided()
        {
            MockGitLocationForConfiguration();

            var clone = new Clone(GitTestProjectLocation)
                            {
                                RepositoryToClone = GitTestProjectLocation,
                                Destination = RepositoryDestination
                            };

            string progressMessage = string.Empty;
            bool messageRecieved = false;

            clone.Progress += (s, e) =>
                                  {
                                      if (!messageRecieved)
                                      {
                                          progressMessage = e.Message;
                                          messageRecieved = true; 
                                      }
                                  };

            clone.Run();

            Stopwatch watch = Stopwatch.StartNew();

            while (!messageRecieved && watch.Elapsed < TimeSpan.FromMilliseconds(500))
            {
                System.Threading.Thread.Sleep(1);
            }

            Assert.That(messageRecieved);
            Assert.That(progressMessage.StartsWith("Checking out files: "));
        }
    }
}