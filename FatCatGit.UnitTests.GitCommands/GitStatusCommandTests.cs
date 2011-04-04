using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FatCatGit.Configuration;
using FatCatGit.GitCommands;
using NUnit.Framework;
using Rhino.Mocks;

namespace FatCatGit.UnitTests.GitCommands
{
    [TestFixture]
    public class GitStatusCommandTests : BaseCommandTests
    {
        

        [Test]
        public void GetStatusWillRunForProvidedProject()
        {
            MockGitLocationForConfiguration();

            Status status = new Status(GitProjectLocation);

            status.Run();

            Assert.That(status.Output.Contains("# On branch "), Is.True);
        }

        
    }
}
