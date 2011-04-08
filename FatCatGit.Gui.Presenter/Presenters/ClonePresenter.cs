using System.IO;
using FatCatGit.Gui.Presenter.Exceptions;
using FatCatGit.Gui.Presenter.Views;

namespace FatCatGit.Gui.Presenter.Presenters
{
    public class ClonePresenter
    {
        private GitProject _gitProject;

        public ClonePresenter(CloneView view)
        {
            View = view;
        }

        private CloneView View { get; set; }

        public GitProject GitProject
        {
            get { return _gitProject ?? (_gitProject = new GitProject(View.RepositoryToClone)); }
            set { _gitProject = value; }
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
            return GitProject.RepostoryName;
        }
    }
}