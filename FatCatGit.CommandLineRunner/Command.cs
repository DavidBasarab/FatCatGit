namespace FatCatGit.CommandLineRunner
{
    public class Command
    {
        public Command(string commandFullLocation, string arguments = null, string workingDirectory = null)
        {
            CommandFullLocation = commandFullLocation;
            Arguments = arguments;
            WorkingDirectory = workingDirectory;
        }

        public string WorkingDirectory { get; set; }

        public string CommandFullLocation { get; set; }

        public string Arguments { get; set; }
    }
}