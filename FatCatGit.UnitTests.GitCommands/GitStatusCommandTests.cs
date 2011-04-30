using System;
using FatCatGit.CommandLineRunner;
using FatCatGit.GitCommands;
using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExtensions;

namespace FatCatGit.UnitTests.GitCommands
{
    [TestFixture]
    [Category("Git Commands")]
    public class GitStatusCommandTests : BaseCommandTests
    {
        [Test]
        public void EnvironmentVariableHomeWillBeSetToUserHomeDriveAndPathIfNotFound()
        {
            Status status;
            var environmentVariable = SetUpForEnvironmentTests(out status);

            environmentVariable.HomeDrive = "E:";
            environmentVariable.HomePath = @"\users\FatCat\Faker";

            status.Run();

            Assert.That(environmentVariable.Home, Is.EqualTo(@"E:\users\FatCat\Faker"));
        }

        [Test]
        public void EnvironmentVariableHomeWillNotBeChangedIfPreviouslySet()
        {
            Status status;
            var environmentVariable = SetUpForEnvironmentTests(out status);

            const string customHomeValue = "I_Set_This_To_Custom_Value";

            environmentVariable.Home = customHomeValue;
            environmentVariable.HomeDrive = "E:";
            environmentVariable.HomePath = @"\users\FatCat\Faker";

            status.Run();

            Assert.That(environmentVariable.Home, Is.EqualTo(customHomeValue));
        }

        [Test]
        public void IfEnvironmentHomeDriveIsNullUseUserProfileForHomeValue()
        {
            Status status;
            var environmentVariable = SetUpForEnvironmentTests(out status);

            environmentVariable.HomeDrive = null;
            environmentVariable.HomePath = @"\users\FatCat\Faker";
            environmentVariable.UserProfile = @"E:\FatCatProfile";

            status.Run();

            Assert.That(environmentVariable.Home, Is.EqualTo(@"E:\FatCatProfile"));
        }

        private EnvironmentVariable SetUpForEnvironmentTests(out Status status)
        {
            MockGitLocationForConfiguration();

            var command = MockCommandProperties();

            var runner = MockRunner();

            var environmentVariable = Mocks.StrictMock<EnvironmentVariable>();

            environmentVariable.SetPropertyAsBehavior(v => v.Home);
            environmentVariable.SetPropertyAsBehavior(v => v.HomeDrive);
            environmentVariable.SetPropertyAsBehavior(v => v.HomePath);
            environmentVariable.SetPropertyAsBehavior(v => v.UserProfile);

            Mocks.ReplayAll();

            status = new Status
                         {
                             ProjectLocation = GitTestProjectLocation,
                             CommandArguments = command,
                             Runner = runner,
                             EnvironmentVariable = environmentVariable
                         };

            return environmentVariable;
        }

        [Test]
        [Ignore("Waiting on refactoring")]
        public void HomeEnvironmentVariableWillNotBeChangedIfPreviouslySet()
        {
            Assert.Fail();
        }

        [Test]
        public void GetStatusWillRunForProvidedProject()
        {
            MockGitLocationForConfiguration();

            var command = MockCommandProperties();

            var runner = MockRunner();

            Mocks.ReplayAll();

            runner.Output = "# On branch ";

            var status = new Status
                             {
                                 ProjectLocation = GitTestProjectLocation,
                                 CommandArguments = command,
                                 Runner = runner,
                                 EnvironmentVariable = StubEnvironmentVariable()
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