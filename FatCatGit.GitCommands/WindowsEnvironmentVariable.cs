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
            set { throw new InvalidOperationException("Cannot set the HOMEDRIVE Environment variable in windows."); }
        }

        public string HomePath
        {
            get { return Environment.GetEnvironmentVariable("HOMEPATH"); }
            set { throw new InvalidOperationException("Cannot set the HOMEPATH Environment variable in windows."); }
        }

        public string UserProfile
        {
            get { return Environment.GetEnvironmentVariable("USERPROFILE"); }
            set { throw new InvalidOperationException("Cannot set the USERPROFILE Environment variable in windows."); }
        }
    }
}