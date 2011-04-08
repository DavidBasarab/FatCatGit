using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace FatCatGit.UnitTests.Gui.Presenter
{
    [TestFixture]
    [Category("HookUp")]
    public class HookUpTests
    {
        [Test]
        public void GitProjectLocationValid()
        {
            var destinationLocation = ConfigurationManager.AppSettings["DestinationLocation"]; ;

            var directoryInfo = new DirectoryInfo(destinationLocation);

            Assert.That(directoryInfo.Exists);
        }

    }
}
