using System.Configuration;
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

            var clone = new Clone(GitTestProjectLocation)
                            {
                                RepositoryToClone = GitTestProjectLocation,
                                Destination = RepositoryDestination
                            };

            clone.Run();

            Assert.That(clone.Output.Contains(string.Format("Cloning into {0}...", RepositoryDestination)));
        }

        [Test]
        [Ignore("Waiting for Error Stream to be implemented.")]
        public void WhenCloningARepositoryTheProgressIsProvided()
        {
            MockGitLocationForConfiguration();

            var clone = new Clone(GitTestProjectLocation)
                            {
                                RepositoryToClone = GitTestProjectLocation,
                                Destination = RepositoryDestination
                            };

            clone.Run();

            Assert.Fail();
        }
    }
}