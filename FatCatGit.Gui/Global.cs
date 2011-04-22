using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FatCatGit.Modules;

namespace FatCatGit.Gui
{
    public static class Global
    {
        private static GitCommandModule GitCommandModule { get; set; }

        public static void LoadModules()
        {
            if (GitCommandModule != null)
            {
                GitCommandModule = new GitCommandModule();
            }
        }
    }
}
