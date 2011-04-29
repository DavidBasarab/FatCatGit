using System;
using System.Diagnostics;
using Ninject;

namespace FatCatGit.CommandLineRunner
{
    public class ConsoleRunner : Runner
    {
        public ConsoleRunner(Command command)
        {
            Command = command;
            Output = string.Empty;
            ErrorOutput = string.Empty;
        }

        public ConsoleRunner()
        {
            Output = string.Empty;
            ErrorOutput = string.Empty;
        }

        private ProcessStartInfo StartInfo { get; set; }
        private Process Process { get; set; }

        private CommandAsyncResult Result { get; set; }

        [Inject]
        public Command Command { get; set; }

        public string Output { get; set; }
        public string ErrorOutput { get; set; }

        public event OutputReceived StandardOutputReceived;

        public void Execute()
        {
            IAsyncResult result = BeginExecute();

            result.AsyncWaitHandle.WaitOne();
        }

        public IAsyncResult BeginExecute()
        {
            StartAsyncExecute();

            return Result;
        }

        public event OutputReceived ErrorOutputReceived;

        private void ExecuteProcess()
        {
            CreateProcessStartInfo();

            FindCommandOutput();
        }

        private void FindCommandOutput()
        {
            try
            {
                CreateProcess();

                RegisterForOutputEvents();

                Console.WriteLine("Starting process");

                StartProcess();

                Process.WaitForExit();

                Console.WriteLine("Process complete");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:      {0}", ex.Message);
                Console.WriteLine("Stacktrace: {0}", ex.StackTrace);
            }
        }

        private void StartProcess()
        {
            Process.Start();

            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
        }

        private void RegisterForOutputEvents()
        {
            Process.OutputDataReceived += OutputDataRecieved;
            Process.ErrorDataReceived += ErrorDataRecieved;
        }

        private void ErrorDataRecieved(object sender, DataReceivedEventArgs e)
        {
            ErrorOutput += e.Data;

            TriggerErrorReceivedEvent(e);
        }

        private void TriggerErrorReceivedEvent(DataReceivedEventArgs e)
        {
            if (ErrorOutputReceived != null)
            {
                var args = new OutputReceivedArgs(e);

                ErrorOutputReceived(args);
            }
        }

        private void OutputDataRecieved(object sender, DataReceivedEventArgs e)
        {
            Output += e.Data;

            TriggerOutputReceivedEvent(e);
        }

        private void TriggerOutputReceivedEvent(DataReceivedEventArgs e)
        {
            if (StandardOutputReceived != null)
            {
                var args = new OutputReceivedArgs(e);

                StandardOutputReceived(args);
            }
        }

        private void CreateProcess()
        {
            Process = new Process
                          {
                              StartInfo = StartInfo
                          };
        }

        private void CreateProcessStartInfo()
        {
            StartInfo = new ProcessStartInfo(Command.CommandFullLocation)
                            {
                                RedirectStandardOutput = true,
                                RedirectStandardInput = true,
                                RedirectStandardError = true,
                                WindowStyle = ProcessWindowStyle.Hidden,
                                CreateNoWindow = true,
                                UseShellExecute = false,
                                Arguments = Command.Arguments,
                                WorkingDirectory = Command.WorkingDirectory
                            };
        }

        private void StartAsyncExecute()
        {
            var lockObj = new object();

            lock (lockObj)
            {
                CreateCommandResult();

                StartExecutionProcessThread();
            }
        }

        private void StartExecutionProcessThread()
        {
            Action executationProcess = RunExecuteProcess;

            executationProcess.BeginInvoke(null, null);
        }

        private void CreateCommandResult()
        {
            Result = new CommandAsyncResult();
        }

        private void RunExecuteProcess()
        {
            try
            {
                ExecuteProcess();

                Result.ProcessComplete(Output, ErrorOutput);
            }
            catch (Exception ex)
            {
                Result.ProcessComplete(null, string.Format("{0}|{1}", ex.Message, ex.StackTrace));
            }
        }
    }
}