using FatCatGit.CommandLineRunner;
using Ninject;

namespace FatCatGit.Modules
{
    public class GitCommandModule : StandardKernel
    {
        public GitCommandModule()
        {
            Bind<Runner>().To<ConsoleRunner>();
            Bind<Command>().To<ConsoleCommand>();
        }
    }
}