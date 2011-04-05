using System;
using System.Threading;

namespace FatCatGit.CommandLineRunner
{
    internal class CommandAsyncResult : IAsyncResult
    {
        private ManualResetEvent _asyncWaitHandle;

        private CommandOutput Output { get; set; }

        public bool IsCompleted
        {
            get { return AsyncWaitHandle.WaitOne(0); }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return _asyncWaitHandle ?? (_asyncWaitHandle = new ManualResetEvent(false)); }
        }

        public object AsyncState
        {
            get { return Output; }
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public ManualResetEvent StopEvent
        {
            get { return _asyncWaitHandle ?? (_asyncWaitHandle = new ManualResetEvent(false)); }
        }

        public void ProcessComplete(string normalOutput, string errorOutput)
        {
            Output = new CommandOutput()
                         {
                             Output = normalOutput,
                             ErrorOutput = errorOutput
                         };

            StopEvent.Set();
        }
    }
}