using System;
using System.Threading;
using FatCatGit.CommandLineRunner;
using FatCatGit.Common.Interfaces;

namespace FatCatGit.UnitTests.Gui.Presenter
{
    internal class TestAsyncResults : IAsyncResult
    {
        public ManualResetEvent StopEvent { get; set; }

        public TestAsyncResults()
        {
            StopEvent = new ManualResetEvent(true);
        }

        public bool IsCompleted { get; set; }

        public WaitHandle AsyncWaitHandle
        {
            get { return StopEvent; }
        }

        public object AsyncState { get; set; }

        public bool CompletedSynchronously
        {
            get { throw new NotImplementedException(); }
        }
    }

    internal class FakeOutput : Output
    {
        public string Output { get; set; }
        public string ErrorOutput { get; set; }
    }
}
