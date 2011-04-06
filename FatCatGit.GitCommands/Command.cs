using System;
using System.Diagnostics;
using FatCatGit.CommandLineRunner;
using FatCatGit.Configuration;
using FatCatGit.GitCommands.Args;

namespace FatCatGit.GitCommands
{
    public class Command
    {
        private CommandLineRunner.Command _command;
        private IAsyncResult _executeCommandResult;

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

            _executeCommandResult = Runner.BeginExecute();

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
            Output = Runner.Output;
            ErrorOutput = Runner.ErrorOutput;
        }

        private void CreateRunner()
        {
            Runner = new Runner(_command);

            Runner.ErrorOutputReceived += ErrorOutputReceivedFromRunner;
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
            _command = new CommandLineRunner.Command(GitExecutableLocation, workingDirectory: ProjectLocation, arguments: GitCommandString);
        }
    }
}