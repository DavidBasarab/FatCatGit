using FatCatGit.CommandLineRunner;
using FatCatGit.Configuration;

namespace FatCatGit.GitCommands
{
    public class Command
    {
        private CommandLineRunner.Command _command;

        public Command(string projectLocation)
        {
            ProjectLocation = projectLocation;
        }

        public string ProjectLocation { get; set; }

        private Runner Runner { get; set; }

        protected virtual string GitCommandString
        {
            get { return string.Empty; }
        }

        public string Output { get; set; }

        protected virtual string Arguments { get; set; }

        public string CompiledArugments
        {
            get { return string.Format("{0} {1}", GitCommandString, Arguments); }
        }

        private static string GitExecutableLocation
        {
            get { return ConfigurationSettings.Global.GitExecutableLocation; }
        }

        public void Run()
        {
            ExecuteCommand();

            SaveOutput();
        }

        private void ExecuteCommand()
        {
            CreateRunnerCommand();

            CreateRunner();

            Runner.Execute();
        }

        private void SaveOutput()
        {
            Output = Runner.Output;
        }

        private void CreateRunner()
        {
            Runner = new Runner(_command);
        }

        private void CreateRunnerCommand()
        {
            _command = new CommandLineRunner.Command(GitExecutableLocation, workingDirectory: ProjectLocation, arguments: CompiledArugments);
        }
    }
}