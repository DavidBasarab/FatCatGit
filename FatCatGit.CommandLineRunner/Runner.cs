using System.Diagnostics;
using System.IO;

namespace FatCatGit.CommandLineRunner
{
    public class Runner
    {
        public Runner(Command command)
        {
            Command = command;
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

            Process.Start();

            SaveProcessOutput();
        }

        private void SaveProcessOutput()
        {
            using (StreamReader outputStream = Process.StandardOutput)
            {
                Process.WaitForExit();

                Output = outputStream.ReadToEnd();
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
                                WindowStyle = ProcessWindowStyle.Hidden,
                                CreateNoWindow = true,
                                UseShellExecute = false,
                                Arguments = Command.Arguments
                            };
        }
    }
}