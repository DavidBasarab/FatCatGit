using System;
using FatCatGit.CommandLineRunner;
using FatCatGit.Configuration;
using FatCatGit.GitCommands.Args;
using Ninject;

namespace FatCatGit.GitCommands
{
    public class BaseCommand
    {
        private IAsyncResult _executeCommandResult;

        public string ProjectLocation { get; set; }

        [Inject]
        public Runner Runner { get; set; }

        [Inject]
        public Command CommandArguments { get; set; }

        [Inject]
        public EnvironmentVariable EnvironmentVariable { get; set; }

        protected virtual string GitCommandString
        {
            get { return string.Empty; }
        }

        public string Output { get; set; }
        public string ErrorOutput { get; set; }

        private static string GitExecutableLocation
        {
            get { return ConfigurationSettings.Global.GitExecutableLocation; }
        }

        public event GitCommandProgressEvent Progress;

        public IAsyncResult Run()
        {
            SetUpHomeEnvironmentVariable();

            ExecuteCommand();

            return _executeCommandResult;
        }

        private void SetUpHomeEnvironmentVariable()
        {
            var gitEv = new GitEnvironmentVariable(EnvironmentVariable);

            gitEv.DetermineHomeVariable();
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
            Runner.Command = CommandArguments;

            Runner.ErrorOutputReceived += ErrorOutputReceivedFromRunner;
        }

        private void ErrorOutputReceivedFromRunner(OutputReceivedArgs obj)
        {
            if (Progress != null)
            {
                var args = new GitCommandProgressEventArgs
                               {
                                   Message = obj.Data
                               };

                Progress(this, args);
            }
        }

        private void CreateRunnerCommand()
        {
            CommandArguments.Arguments = GitCommandString;
            CommandArguments.CommandFullLocation = GitExecutableLocation;
            CommandArguments.WorkingDirectory = ProjectLocation;
        }
    }
}