using System;

namespace FatCatGit.GitCommands
{
    public class WindowsEnvironmentVariable : EnvironmentVariable
    {
        public string Home
        {
            get { return Environment.GetEnvironmentVariable("HOME", EnvironmentVariableTarget.User); }
            set { Environment.SetEnvironmentVariable("HOME", value); }
        }

        public string HomeDrive
        {
            get { return Environment.GetEnvironmentVariable("HOMEDRIVE"); }
            set { return; }
        }

        public string HomePath
        {
            get { return Environment.GetEnvironmentVariable("HOMEPATH"); }
            set { return; }
        }

        public string UserProfile
        {
            get { return Environment.GetEnvironmentVariable("USERPROFILE"); }
            set { return; }
        }
    }
}