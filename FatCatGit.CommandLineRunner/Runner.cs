using System;
using System.Diagnostics;
using System.IO;

namespace FatCatGit.CommandLineRunner
{
    public class Runner
    {
        public Runner(Command command)
        {
            Command = command;
            Output = string.Empty;
        }

        public Command Command { get; set; }
        public string Output { get; set; }

        private ProcessStartInfo StartInfo { get; set; }
        private Process Process { get; set; }

        public void Execute()
        {
            CreateProcessStartInfo();

            FindCommandOutput();
        }

        private void FindCommandOutput()
        {
            CreateProcess();

            RegisterForOutputEvents();

            StartProcess();

            Process.WaitForExit();
        }

        private void StartProcess()
        {
            Process.Start();

            Process.BeginOutputReadLine();
        }

        private void RegisterForOutputEvents()
        {
            Process.OutputDataReceived += OutputDataRecieved;
        }

        private void OutputDataRecieved(object sender, DataReceivedEventArgs e)
        {
            Output += e.Data;
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
                                WindowStyle = ProcessWindowStyle.Hidden,
                                CreateNoWindow = true,
                                UseShellExecute = false,
                                Arguments = Command.Arguments,
                                WorkingDirectory = Command.WorkingDirectory
                            };
        }
    }
}