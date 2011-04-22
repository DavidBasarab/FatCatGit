namespace FatCatGit.CommandLineRunner
{
    public interface Command
    {
        string WorkingDirectory { get; set; }
        string CommandFullLocation { get; set; }
        string Arguments { get; set; }
    }
}