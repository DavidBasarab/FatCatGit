using FatCatGit.CommandLineRunner;
using FatCatGit.GitCommands;
using FatCatGit.GitCommands.Interfaces;
using Ninject;

namespace FatCatGit.Modules
{
    public class GitCommandModule : StandardKernel
    {
        public GitCommandModule()
        {
            Bind<Runner>().To<ConsoleRunner>();
            Bind<Command>().To<ConsoleCommand>();
            Bind<Clone>().To<CloneCommand>();
            Bind<EnvironmentVariable>().To<WindowsEnvironmentVariable>();
        }
    }
}