using System;
using System.Diagnostics;
using System.Threading;
using FatCatGit.CommandLineRunner;
using FatCatGit.GitCommands;
using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExtensions;

namespace FatCatGit.UnitTests.GitCommands
{
    [TestFixture]
    [Category("Git Commands")]
    public class GitCloneCommandTests : BaseCommandTests
    {
        public string RepositoryDestination
        {
            get { return @"C:\SomeFakeUnitTestDirectory\MoreUnitTestFakey"; }
        }

        [Test]
        public void ALocalRepoistoryCanBeCloned()
        {
            MockGitLocationForConfiguration();

            Command command = MockCommandProperties();

            Runner runner = MockRunner();

            Mocks.ReplayAll();

            runner.Output = string.Format("Cloning into {0}...", RepositoryDestination);

            var clone = new CloneCommand
                            {
                                ProjectLocation = GitEmptyTestProjectLocation,
                                RepositoryToClone = GitEmptyTestProjectLocation,
                                Destination = RepositoryDestination,
                                Runner = runner,
                                CommandArguments = command,
                                EnvironmentVariable = StubEnvironmentVariable()
                            };

            IAsyncResult result = clone.Run();

            result.AsyncWaitHandle.WaitOne();

            Assert.That(clone.Output.Contains(string.Format("Cloning into {0}...", RepositoryDestination)));
            Assert.That(runner.Command, Is.EqualTo(command));
            Assert.That(command.CommandFullLocation, Is.EqualTo(GitInstallLocation));
            Assert.That(command.Arguments, Is.EqualTo("clone -v --verbose --progress \"C:\\Empty\\GitProject\" \"C:\\SomeFakeUnitTestDirectory\\MoreUnitTestFakey\""));
            Assert.That(command.WorkingDirectory, Is.EqualTo(GitEmptyTestProjectLocation));
        }

        [Test]
        public void WhenACloneIsPerformedTheEnvorionmentVariableHomeIsSetToHomeDriveAndPath()
        {
            CloneCommand clone;
            EnvironmentVariable envVar = SetUpForEnvironmentVariableTest(out clone);

            envVar.HomeDrive = "R:";
            envVar.HomePath = @"\FatCat\Path\";

            clone.Run();

            Assert.That(envVar.Home, Is.EqualTo(@"R:\FatCat\Path\"));
        }

        [Test]
        public void IfEnvironmentHomeDriveIsNullUseUserProfileForHomeValue()
        {
            CloneCommand clone;
            EnvironmentVariable envVar = SetUpForEnvironmentVariableTest(out clone);

            envVar.HomeDrive = null;
            envVar.HomePath = @"\users\FatCat\Faker";
            envVar.UserProfile = @"E:\FatCatProfile";

            clone.Run();

            Assert.That(envVar.Home, Is.EqualTo(@"E:\FatCatProfile"));
        }

        [Test]
        public void EnvironmentVariableHomeWillNotBeChangedIfPreviouslySet()
        {
            CloneCommand clone;
            EnvironmentVariable envVar = SetUpForEnvironmentVariableTest(out clone);

            const string customHomeValue = "I_Set_This_To_Custom_Value";

            envVar.Home = customHomeValue;
            envVar.HomeDrive = "E:";
            envVar.HomePath = @"\users\FatCat\Faker";

            clone.Run();

            Assert.That(envVar.Home, Is.EqualTo(customHomeValue));
        }

        [Test]
        public void IfEnvironmentHomePathIsNullUseUserProfileForHomeValue()
        {
            CloneCommand clone;
            EnvironmentVariable envVar = SetUpForEnvironmentVariableTest(out clone);

            envVar.HomeDrive = "E:";
            envVar.HomePath = null;
            envVar.UserProfile = @"E:\FatCatProfile";

            clone.Run();

            Assert.That(envVar.Home, Is.EqualTo(@"E:\FatCatProfile"));
        }

        private EnvironmentVariable SetUpForEnvironmentVariableTest(out CloneCommand clone)
        {
            MockGitLocationForConfiguration();

            var command = MockCommandProperties();

            var runner = MockRunner();

            var envVar = Mocks.StrictMock<EnvironmentVariable>();

            envVar.SetPropertyAsBehavior(v => v.Home);
            envVar.SetPropertyAsBehavior(v => v.HomeDrive);
            envVar.SetPropertyAsBehavior(v => v.HomePath);
            envVar.SetPropertyAsBehavior(v => v.UserProfile);

            Mocks.ReplayAll();

            clone = new CloneCommand
                        {
                            ProjectLocation = GitEmptyTestProjectLocation,
                            RepositoryToClone = GitEmptyTestProjectLocation,
                            Destination = RepositoryDestination,
                            Runner = runner,
                            CommandArguments = command,
                            EnvironmentVariable = envVar
                        };
            return envVar;
        }

        [Test]
        public void WhenCloningARepositoryTheProgressIsProvided()
        {
            MockGitLocationForConfiguration();

            Command command = MockCommandProperties();

            Runner runner = MockRunner();

            Mocks.ReplayAll();

            var clone = new CloneCommand
                            {
                                ProjectLocation = GitTestProjectLocation,
                                RepositoryToClone = GitTestProjectLocation,
                                Destination = RepositoryDestination,
                                Runner = runner,
                                CommandArguments = command,
                                EnvironmentVariable = StubEnvironmentVariable()
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

            var eventArgs = new OutputReceivedArgs
                                {
                                    Data = "This is a progress message"
                                };

            runner.Raise(v => v.ErrorOutputReceived += null, eventArgs);

            Stopwatch watch = Stopwatch.StartNew();

            while (!messageRecieved && watch.Elapsed < TimeSpan.FromMilliseconds(500))
            {
                Thread.Sleep(1);
            }

            Assert.That(messageRecieved);
            Assert.That(progressMessage.StartsWith("This is a progress message"));
        }
    }
}