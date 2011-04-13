﻿using System;
using FatCatGit.GitCommands;
using NUnit.Framework;

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

            var status = new Status(GitTestProjectLocation);

            IAsyncResult result = status.Run();

            result.AsyncWaitHandle.WaitOne();

            Console.WriteLine(status.ErrorOutput);

            Assert.That(status.Output, Contains.Substring("# On branch "));
        }
    }
}