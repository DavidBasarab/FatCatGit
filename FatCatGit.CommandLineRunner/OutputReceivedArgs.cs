using System;
using System.Diagnostics;

namespace FatCatGit.CommandLineRunner
{
    public class OutputReceivedArgs : EventArgs
    {
        public OutputReceivedArgs(DataReceivedEventArgs dataReceivedEventArgs)
        {
            Data = dataReceivedEventArgs.Data;
        }

        public OutputReceivedArgs()
        {
            
        }

        public string Data { get; set; }
    }
}