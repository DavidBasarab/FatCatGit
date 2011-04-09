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
            if (string.IsNullOrEmpty(destinationFolderLocation))
            {
                return;
            }

            DetermineDestinationFolder(destinationFolderLocation);
        }

        private void DetermineDestinationFolder(string destinationFolderLocation)
        {
            var gitDirectory = new GitDirectory(destinationFolderLocation);

            if (DoesSelectedFolderHaveSameNameAsRepo(destinationFolderLocation))
            {
                View.DestinationFolder = destinationFolderLocation;
            }
            else
            {
                AddRepoNameToDestinationFolder(gitDirectory);
            }
        }

        private void AddRepoNameToDestinationFolder(GitDirectory gitDirectory)
        {
            View.DestinationFolder = string.Format("{0}{1}", gitDirectory.FullName, GitProject.RepostoryName);
        }

        private bool DoesSelectedFolderHaveSameNameAsRepo(string destinationFolderLocation)
        {
            return destinationFolderLocation.EndsWith(GitProject.RepostoryName) || destinationFolderLocation.EndsWith(string.Format("{0}\\", GitProject.RepostoryName));
        }
    }
}