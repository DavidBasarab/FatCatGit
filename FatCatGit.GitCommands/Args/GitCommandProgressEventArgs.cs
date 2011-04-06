using System;

namespace FatCatGit.GitCommands.Args
{
    public delegate void GitCommandProgressEvent(object sender, GitCommandProgressEventArgs e);

    public class GitCommandProgressEventArgs : EventArgs
    {
        public string Message { get; internal set; }
    }
}
