using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace FatCatGit.GitCommands
{
    internal class GitEnvironmentVariable
    {
        public EnvironmentVariable EnvironmentVariable { get; set; }

        public GitEnvironmentVariable(EnvironmentVariable environmentVariable)
        {
            EnvironmentVariable = environmentVariable;
        }

        public void DetermineHomeVariable()
        {
            if (string.IsNullOrEmpty(EnvironmentVariable.Home))
            {
                if (!string.IsNullOrEmpty(EnvironmentVariable.HomeDrive) && !string.IsNullOrEmpty(EnvironmentVariable.HomePath))
                {
                    EnvironmentVariable.Home = string.Format("{0}{1}", EnvironmentVariable.HomeDrive, EnvironmentVariable.HomePath);
                }
                else
                {
                    EnvironmentVariable.Home = EnvironmentVariable.UserProfile;
                }
            }
        }
    }
}
