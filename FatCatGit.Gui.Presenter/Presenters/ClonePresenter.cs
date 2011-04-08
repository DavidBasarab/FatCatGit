using System;
using System.IO;
using FatCatGit.Gui.Presenter.Exceptions;
using FatCatGit.Gui.Presenter.Views;

namespace FatCatGit.Gui.Presenter.Presenters
{
    public class ClonePresenter
    {
        private CloneView View { get; set; }

        public ClonePresenter(CloneView view)
        {
            View = view;
        }

        public void SetDestinationFolder(string destinationFolderLocation)
        {
            var gitDirectory = new GitDirectory(destinationFolderLocation);

            if (!gitDirectory.Exists)
            {
                throw new InvalidDirectoryException();
            }

            View.DestinationFolder = string.Format("{0}{1}", gitDirectory.FullName, FindRepositoryProject());
        }

        private string FindRepositoryProject()
        {
            var directoryInfo = new DirectoryInfo(View.RepositoryToClone);

            return directoryInfo.Name;
        }
    }
}