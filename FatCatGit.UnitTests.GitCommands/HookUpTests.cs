using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ConfigurationSettings = FatCatGit.Configuration.ConfigurationSettings;

namespace FatCatGit.UnitTests.GitCommands
{
    [TestFixture(Description = "These test are used to make sure you enviorment is set up correctly to develop unit tests for FatCatGit")]
    public class HookUpTests : BaseCommandTests
    {
        [Test]
        public void GitExists()
        {
            MockGitLocationForConfiguration();

            FileInfo fileInfo = new FileInfo(ConfigurationSettings.Global.GitExecutableLocation);

            Assert.That(fileInfo.Exists);
        }

        [Test]
        public void GitProjectLocationValid()
        {
            string projectLocation = ConfigurationManager.AppSettings["GitTestProjectLocation"];
            var projectLocationInfo = new DirectoryInfo(projectLocation);

            Assert.That(projectLocationInfo.Exists);

            var gitFolderLocation = new DirectoryInfo(string.Format(@"{0}\.git", projectLocation));

            Assert.That(gitFolderLocation.Exists);
        }


    }
}
