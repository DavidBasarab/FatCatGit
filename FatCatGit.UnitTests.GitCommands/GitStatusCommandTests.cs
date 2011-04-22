using System;
using FatCatGit.CommandLineRunner;
using FatCatGit.GitCommands;
using NUnit.Framework;
using Rhino.Mocks;

namespace FatCatGit.UnitTests.GitCommands
{
    [TestFixture]
    [Category("Git Commands")]
    public class GitStatusCommandTests : BaseCommandTests
    {
        [Test]
        public void GetStatusWillRunForProvidedProject()
        {
            MockGitLocationForConfiguration();

            var command = MockCommandProperties();

            var runner = MockRunner();

            Mocks.ReplayAll();

            runner.Output = "# On branch ";

            var status = new Status(GitTestProjectLocation)
                             {
                                 CommandArguments = command,
                                 Runner = runner
                             };

            status.Run();

            Assert.That(status.Output, Contains.Substring("# On branch "));
            Assert.That(runner.Command, Is.EqualTo(command));
            Assert.That(command.CommandFullLocation, Is.EqualTo(GitInstallLocation));
            Assert.That(command.Arguments, Is.EqualTo("status"));
            Assert.That(command.WorkingDirectory, Is.EqualTo(GitTestProjectLocation));
        }

    }
}