using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using FatCatGit.CommandLineRunner;
using FatCatGit.GitCommands;
using NUnit.Framework;
using Rhino.Mocks;

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

            var clone = new Clone(GitEmptyTestProjectLocation)
                            {
                                RepositoryToClone = GitEmptyTestProjectLocation,
                                Destination = RepositoryDestination,
                                Runner = runner,
                                CommandArguments = command
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
        public void WhenCloningARepositoryTheProgressIsProvided()
        {
            MockGitLocationForConfiguration();

            Command command = MockCommandProperties();

            Runner runner = MockRunner();

            Mocks.ReplayAll();

            var clone = new Clone(GitTestProjectLocation)
                            {
                                RepositoryToClone = GitTestProjectLocation,
                                Destination = RepositoryDestination,
                                Runner = runner,
                                CommandArguments = command
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