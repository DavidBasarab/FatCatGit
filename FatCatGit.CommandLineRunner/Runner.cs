using System;
using System.Diagnostics;

namespace FatCatGit.CommandLineRunner
{
    public interface Runner
    {
        ConsoleCommand ConsoleCommand { get; set; }
        string Output { get; set; }
        string ErrorOutput { get; set; }
        event Action<DataReceivedEventArgs> OutputReceived;
        void Execute();
        IAsyncResult BeginExecute();
        event Action<DataReceivedEventArgs> ErrorOutputReceived;
    }
}