using System;
using System.Threading;
using FatCatGit.Common.Interfaces;

namespace FatCatGit.UnitTests.GitCommands
{
    internal class TestAsyncResults : IAsyncResult
    {
        public TestAsyncResults()
        {
            StopEvent = new ManualResetEvent(true);
        }

        public ManualResetEvent StopEvent { get; set; }

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