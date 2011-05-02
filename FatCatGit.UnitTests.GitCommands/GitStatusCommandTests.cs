using FatCatGit.CommandLineRunner;
using FatCatGit.GitCommands;
using NUnit.Framework;
using RhinoMocksExtensions;

namespace FatCatGit.UnitTests.GitCommands
{
    [TestFixture]
    [Category("Git Commands")]
    public class GitStatusCommandTests : BaseCommandTests
    {
        private EnvironmentVariable SetUpForEnvironmentTests(out Status status)
        {
            MockGitLocationForConfiguration();

            Command command = MockCommandProperties();

            Runner runner = MockRunner();

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
        public void EnvironmentVariableHomeWillBeSetToUserHomeDriveAndPathIfNotFound()
        {
            Status status;
            EnvironmentVariable environmentVariable = SetUpForEnvironmentTests(out status);

            environmentVariable.HomeDrive = "E:";
            environmentVariable.HomePath = @"\users\FatCat\Faker";

            status.Run();

            Assert.That(environmentVariable.Home, Is.EqualTo(@"E:\users\FatCat\Faker"));
        }

        [Test]
        public void EnvironmentVariableHomeWillNotBeChangedIfPreviouslySet()
        {
            Status status;
            EnvironmentVariable environmentVariable = SetUpForEnvironmentTests(out status);

            const string customHomeValue = "I_Set_This_To_Custom_Value";

            environmentVariable.Home = customHomeValue;
            environmentVariable.HomeDrive = "E:";
            environmentVariable.HomePath = @"\users\FatCat\Faker";

            status.Run();

            Assert.That(environmentVariable.Home, Is.EqualTo(customHomeValue));
        }

        [Test]
        public void GetStatusWillRunForProvidedProject()
        {
            MockGitLocationForConfiguration();

            Command command = MockCommandProperties();

            Runner runner = MockRunner();

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

        [Test]
        public void IfEnvironmentHomePathIsNullUseUserProfileForHomeValue()
        {
            Status status;
            EnvironmentVariable environmentVariable = SetUpForEnvironmentTests(out status);

            environmentVariable.HomeDrive = "E:";
            environmentVariable.HomePath = null;
            environmentVariable.UserProfile = @"E:\FatCatProfile";

            status.Run();

            Assert.That(environmentVariable.Home, Is.EqualTo(@"E:\FatCatProfile"));
        }
    }
}