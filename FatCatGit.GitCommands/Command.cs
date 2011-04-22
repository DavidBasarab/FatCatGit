using System;
using System.Diagnostics;
using FatCatGit.CommandLineRunner;
using FatCatGit.Configuration;
using FatCatGit.GitCommands.Args;

namespace FatCatGit.GitCommands
{
    public class Command
    {
        private CommandLineRunner.ConsoleCommand _consoleCommand;
        private IAsyncResult _executeCommandResult;

        public Command(string projectLocation)
        {
            ProjectLocation = projectLocation;
        }

        public string ProjectLocation { get; set; }

        private ConsoleRunner ConsoleRunner { get; set; }

        protected virtual string GitCommandString
        {
            get { return string.Empty; }
        }

        public event GitCommandProgressEvent Progress;

        public string Output { get; set; }
        public string ErrorOutput { get; set; }

        private static string GitExecutableLocation
        {
            get { return ConfigurationSettings.Global.GitExecutableLocation; }
        }

        public IAsyncResult Run()
        {
            ExecuteCommand();

            return _executeCommandResult;
        }

        private void ExecuteCommand()
        {
            CreateRunnerCommand();

            CreateRunner();

            _executeCommandResult = ConsoleRunner.BeginExecute();

            StartSaveOutputProcessThread();
        }

        private void StartSaveOutputProcessThread()
        {
            Action saveOutputProcess = () =>
                                           {
                                               _executeCommandResult.AsyncWaitHandle.WaitOne();

                                               SaveOutput();
                                           };


            saveOutputProcess();
        }

        private void SaveOutput()
        {
            Output = ConsoleRunner.Output;
            ErrorOutput = ConsoleRunner.ErrorOutput;
        }

        private void CreateRunner()
        {
            ConsoleRunner = new ConsoleRunner(_consoleCommand);

            ConsoleRunner.ErrorOutputReceived += ErrorOutputReceivedFromRunner;
        }

        private void ErrorOutputReceivedFromRunner(DataReceivedEventArgs obj)
        {
            if (Progress != null)
            {
                var args = new GitCommandProgressEventArgs();
                args.Message = obj.Data;

                Progress(this, args);
            }
        }

        private void CreateRunnerCommand()
        {
            _consoleCommand = new CommandLineRunner.ConsoleCommand(GitExecutableLocation, workingDirectory: ProjectLocation, arguments: GitCommandString);
        }
    }
}