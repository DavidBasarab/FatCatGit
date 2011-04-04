using System.IO;
using FatCatGit.GitCommands;
using NUnit.Framework;

namespace FatCatGit.UnitTests.GitCommands
{
    [TestFixture]
    public class GitCloneCommandTests : BaseCommandTests
    {
        #region Setup/Teardown

        [TearDown]
        public new void TearDown()
        {
            DeleteDestinationDirectory();

            base.TearDown();
        }

        #endregion

        private const string RepositoryDestination = @"C:\Test\UnitTestRepo";

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

            var clone = new Clone(GitProjectLocation);

            const string repoToClone = @"C:\Test\Repo1";

            clone.RepositoryToClone = repoToClone;
            clone.Destination = RepositoryDestination;

            clone.Run();

            Assert.That(clone.Output.Contains(string.Format("Cloning into {0}...", RepositoryDestination)), Is.True);
        }
    }
}