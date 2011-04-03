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
    public class GitStatusCommandTests
    {
        const string GitProjectLocation = @"C:\FatCatGit";

        private MockRepository Mocks { get; set; }

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

        [Test]
        public void GetStatusWillRunForProvidedProject()
        {
            GlobalConfiguration globalConfiguration = Mocks.DynamicMock<GlobalConfiguration>();

            globalConfiguration.Expect(v => v.GitExecutableLocation).Return(@"C:\Program Files (x86)\Git\bin\git.exe");

            ConfigurationSettings.Global = globalConfiguration;

            Mocks.ReplayAll();

            Status status = new Status(GitProjectLocation);

            status.Run();

            Assert.That(status.Output.Contains("# On branch "), Is.True);
        }
    }
}
