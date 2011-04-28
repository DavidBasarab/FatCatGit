using System.Configuration;
using FatCatGit.CommandLineRunner;
using FatCatGit.Configuration;
using NUnit.Framework;
using Rhino.Mocks;
using RhinoMocksExtensions;
using ConfigurationSettings = FatCatGit.Configuration.ConfigurationSettings;

namespace FatCatGit.UnitTests.GitCommands
{
    public class BaseCommandTests
    {
        protected MockRepository Mocks { get; set; }

        public string GitTestProjectLocation
        {
            get { return @"C:\SomeFakeProject\Location"; }
        }

        public string GitEmptyTestProjectLocation
        {
            get { return @"C:\Empty\GitProject"; }
        }

        public string GitInstallLocation
        {
            get { return @"C:\git\InstallLocation\git.exe"; }
        }

        [SetUp]
        public void SetUp()
        {
            ConfigurationSettings.Global = null;

            Mocks = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            Mocks.ReplayAll();

            Mocks.VerifyAll();
        }

        protected void MockGitLocationForConfiguration()
        {
            var globalConfiguration = Mocks.DynamicMock<GlobalConfiguration>();

            globalConfiguration.Expect(v => v.GitExecutableLocation).Return(GitInstallLocation);

            ConfigurationSettings.Global = globalConfiguration;

            Mocks.ReplayAll();
        }

        protected Runner MockRunner()
        {
            var runner = Mocks.StrictMock<Runner>();

            runner.SetPropertyAsBehavior(v => v.Command);

            var asyncResults = new TestAsyncResults();

            runner.Expect(v => v.BeginExecute()).Return(asyncResults);

            runner.ErrorOutputReceived += null;
            LastCall.IgnoreArguments();

            runner.SetPropertyAsBehavior(v => v.ErrorOutput);
            runner.SetPropertyAsBehavior(v => v.Output);

            return runner;
        }

        protected Command MockCommandProperties()
        {
            var command = Mocks.StrictMock<Command>();

            command.SetPropertyAsBehavior(v => v.Arguments);
            command.SetPropertyAsBehavior(v => v.CommandFullLocation);
            command.SetPropertyAsBehavior(v => v.WorkingDirectory);

            return command;
        }
    }
}