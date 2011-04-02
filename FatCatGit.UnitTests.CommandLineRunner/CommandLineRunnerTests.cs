using FatCatGit.CommandLineRunner;
using NUnit.Framework;

namespace FatCatGit.UnitTests.CommandLineRunner
{
    [TestFixture]
    public class CommandLineRunnerTests
    {
        [Test]
        public void CommandWillReturnOutputAsString()
        {
            var command = new Command("ipconfig");

            var runner = new Runner(command);

            runner.Execute();

            Assert.That(runner.Output.Contains("Windows IP Configuration"), Is.True);
        }

        [Test]
        public void CommandWithArguments()
        {
            const string gitFullLocation = @"C:\Program Files (x86)\Git\bin\git.exe";

            var command = new Command(gitFullLocation, "--version");

            var runner = new Runner(command);

            runner.Execute();

            Assert.That(runner.Output.Contains("git version"), Is.True);
        }
    }
}