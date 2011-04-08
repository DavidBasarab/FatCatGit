using FatCatGit.Configuration;
using NUnit.Framework;
using Rhino.Mocks;

namespace FatCatGit.UnitTests.GitCommands
{
    public class BaseCommandTests
    {
        protected MockRepository Mocks { get; set; }

        public string GitTestProjectLocation
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["GitTestProjectLocation"]; }
        }

        public string GitEmptyTestProjectLocation
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["GitEmptyTestProjectLocation"]; }
        }

        public string GitInstallLocation
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["GitInstallLocation"]; }
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

            globalConfiguration.Expect(v => v.GitExecutableLocation).Return(@"C:\Program Files (x86)\Git\bin\git.exe");

            ConfigurationSettings.Global = globalConfiguration;

            Mocks.ReplayAll();
        }
    }
}
