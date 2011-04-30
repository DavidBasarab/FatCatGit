namespace FatCatGit.GitCommands
{
    public interface EnvironmentVariable
    {
        string Home { get; set; }
        string HomeDrive { get; set; }
        string HomePath { get; set; }
        string UserProfile { get; set; }
    }
}