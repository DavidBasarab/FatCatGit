using NUnit.Framework;
using Rhino.Mocks;

namespace FatCatGit.UnitTests.Gui.Presenter
{
    public class BasePresenterTests
    {
        public MockRepository Mocks { get; set; }

        [SetUp]
        public void SetUp()
        {
            Mocks = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            Mocks.ReplayAll();
            Mocks.VerifyAll();
        }
    }
}