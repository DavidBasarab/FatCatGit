using FatCatGit.GitCommands.Interfaces;

namespace FatCatGit.GitCommands
{
    public class CloneCommand : BaseCommand, Clone
    {
        // git.exe clone -v --progress "C:/Test/Repo1" "C:/Test/Test2"

        public CloneCommand(string projectLocation) : base(projectLocation)
        {
        }

        public CloneCommand()
        {
            
        }

        public string RepositoryToClone { get; set; }

        public string Destination { get; set; }

        protected override string GitCommandString
        {
            get { return string.Format("clone -v --verbose --progress \"{0}\" \"{1}\"", RepositoryToClone, Destination); }
        }
    }
}