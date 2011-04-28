using System;
using FatCatGit.GitCommands.Args;

namespace FatCatGit.GitCommands.Interfaces
{
    public interface Clone
    {
        string RepositoryToClone { get; set; }
        string Destination { get; set; }
        string ProjectLocation { get; set; }
        string Output { get;  }
        string ErrorOutput { get; }

        event GitCommandProgressEvent Progress;
        
        IAsyncResult Run();
    }
}