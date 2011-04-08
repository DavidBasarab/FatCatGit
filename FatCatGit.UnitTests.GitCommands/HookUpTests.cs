using System.Configuration;
using System.IO;
using NUnit.Framework;
using ConfigurationSettings = FatCatGit.Configuration.ConfigurationSettings;

namespace FatCatGit.UnitTests.GitCommands
{
    [TestFixture(Description = "These test are used to make sure you enviorment is set up correctly to develop unit tests for FatCatGit")]
    [Category("HookUp")]
    public class HookUpTests : BaseCommandTests
    {
        private static void VerifyValidGitProject(string projectLocation)
        {
            var projectLocationInfo = new DirectoryInfo(projectLocation);

            Assert.That(projectLocationInfo.Exists);

            var gitFolderLocation = new DirectoryInfo(string.Format(@"{0}\.git", projectLocation));

            Assert.That(gitFolderLocation.Exists);
        }

        [Test]
        public void GitEmptyProjectLocationValid()
        {
            string projectLocation = ConfigurationManager.AppSettings["GitEmptyTestProjectLocation"];

            VerifyValidGitProject(projectLocation);
        }

        [Test]
        public void GitExists()
        {
            MockGitLocationForConfiguration();

            var fileInfo = new FileInfo(ConfigurationSettings.Global.GitExecutableLocation);

            Assert.That(fileInfo.Exists);
        }

        [Test]
        public void GitProjectLocationValid()
        {
            string projectLocation = ConfigurationManager.AppSettings["GitTestProjectLocation"];

            VerifyValidGitProject(projectLocation);
        }
    }
}