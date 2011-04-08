using System;
using System.IO;

namespace FatCatGit.Gui.Presenter
{
    public class GitDirectory
    {
        public GitDirectory(string path)
        {
            DirectoryInfo = new DirectoryInfo(path);
        }

        private DirectoryInfo DirectoryInfo { get; set; }

        public string FullName
        {
            get { return GetFormattedDiretory(); }
        }

        public bool Exists
        {
            get { return DirectoryInfo.Exists; }
        }

        private string GetFormattedDiretory()
        {
            if (DirectoryInfo.FullName.EndsWith("\\"))
            {
                return DirectoryInfo.FullName;
            }

            return string.Format("{0}\\", DirectoryInfo.FullName);
        }
    }
}