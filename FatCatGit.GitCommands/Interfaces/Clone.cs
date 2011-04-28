using System;
using FatCatGit.GitCommands.Args;

namespace FatCatGit.GitCommands.Interfaces
{
    public interface Clone
    {
        string RepositoryToClone { get; set; }
        string Destination { get; set; }
        string ProjectLocation { get; set; }
        string Output { get; set; }
        string ErrorOutput { get; set; }

        event GitCommandProgressEvent Progress;
        
        IAsyncResult Run();
    }
}