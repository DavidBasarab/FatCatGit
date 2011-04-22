using System;
using System.Diagnostics;

namespace FatCatGit.CommandLineRunner
{
    public delegate void OutputReceived(OutputReceivedArgs receivedArgs);

    public interface Runner
    {
        Command Command { get; set; }
        string Output { get; set; }
        string ErrorOutput { get; set; }
        
        event OutputReceived StandardOutputReceived;
        
        void Execute();
        
        IAsyncResult BeginExecute();

        event OutputReceived ErrorOutputReceived;
    }
}