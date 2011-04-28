using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FatCatGit.Configuration;
using FatCatGit.Modules;

namespace FatCatGit.Gui
{
    public static class Global
    {
        public static GitCommandModule GitCommandModule { get; private set; }

        public static void LoadModules()
        {
            if (GitCommandModule == null)
            {
                GitCommandModule = new GitCommandModule();
            }
        }

        public static void LoadConfiguration()
        {
            ConfigurationSettings.Global.GitExecutableLocation = @"C:\Program Files (x86)\Git\bin\git.exe";
        }
    }
}
