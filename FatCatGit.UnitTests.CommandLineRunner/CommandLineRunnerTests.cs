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
            var command = new Command("CommandLineUnitTester");

            var runner = new Runner(command);

            runner.Execute();

            Assert.That(runner.Output.Contains("This is echo base."), Is.True);
        }

        [Test]
        public void CommandWillRunInGivenDirectory()
        {
            var command = new Command("CommandLineUnitTester", "We read you red 5", @"C:\Program Files");

            var runner = new Runner(command);

            runner.Execute();

            Assert.That(runner.Output.Contains(@"C:\Program Files"), Is.True);
        }

        [Test]
        public void CommandWithArguments()
        {
            var command = new Command("CommandLineUnitTester", "We read you red 5");

            var runner = new Runner(command);

            runner.Execute();

            Assert.That(runner.Output.Contains("Wereadyoured5"), Is.True);
        }

        [Test]
        public void CommandRunnerWillHaveErrorOutput()
        {
            var command = new Command("CommandLineUnitTester", "We read you red 5");

            var runner = new Runner(command);

            runner.Execute();

            Assert.That(runner.ErrorOutput.Contains("This is on the Error Stream"));
        }
    }
}