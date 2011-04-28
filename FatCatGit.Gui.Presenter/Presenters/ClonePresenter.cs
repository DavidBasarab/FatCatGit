using System;
using FatCatGit.CommandLineRunner;
using FatCatGit.GitCommands.Interfaces;
using FatCatGit.Gui.Presenter.Views;
using Ninject;

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

        public bool DestinationFolderDisplayed { get; private set; }

        public bool IsCloneButtonShown { get; set; }

        public event Action<string> CloneComplete;

        [Inject]
        public Clone Clone { get; set; }

        public void SetDestinationFolder(string destinationFolderLocation)
        {
            if (string.IsNullOrEmpty(destinationFolderLocation))
            {
                return;
            }

            DetermineDestinationFolder(destinationFolderLocation);
        }

        private void HandleCloneButtonDisplay()
        {
            if (ShouldCloneButtonBeDisplayed())
            {
                ShowCloneButton();
            }

            if (!IsRepositryAndDestionationPopulated() && IsCloneButtonShown)
            {
                HideCloneButton();
            }
        }

        private bool ShouldCloneButtonBeDisplayed()
        {
            return IsRepositryAndDestionationPopulated() && !IsCloneButtonShown;
        }

        private void HideCloneButton()
        {
            View.HideCloneButton();

            IsCloneButtonShown = false;
        }

        private void ShowCloneButton()
        {
            View.ShowCloneButton();

            IsCloneButtonShown = true;
        }

        private bool IsRepositryAndDestionationPopulated()
        {
            return !string.IsNullOrEmpty(View.RepositoryToClone) && !string.IsNullOrEmpty(View.DestinationFolder);
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

        public void RepositoryToCloneChanged()
        {
            if (ShouldDestinationFolderBeDisplayed())
            {
                DisplayDestionFolder();
            }

            if (ShouldDestinationFolderBeHidden())
            {
                HideDestionationFolder();
            }
        }

        private void HideDestionationFolder()
        {
            View.HideDestinationFolder();

            DestinationFolderDisplayed = false;
        }

        private void DisplayDestionFolder()
        {
            View.DisplayDestinationFolder();

            DestinationFolderDisplayed = true;
        }

        private bool ShouldDestinationFolderBeHidden()
        {
            return string.IsNullOrEmpty(View.RepositoryToClone) && DestinationFolderDisplayed;
        }

        private bool ShouldDestinationFolderBeDisplayed()
        {
            return !string.IsNullOrEmpty(View.RepositoryToClone) && !DestinationFolderDisplayed;
        }

        public void DestionFolderTextChanged()
        {
            HandleCloneButtonDisplay();
        }

        public void PerformClone()
        {
            Clone.Destination = View.DestinationFolder;
            Clone.RepositoryToClone = View.RepositoryToClone;

            var result = Clone.Run();

            Action completeProcess = () =>
                                         {
                                             result.AsyncWaitHandle.WaitOne();

                                             if (CloneComplete != null)
                                             {
                                                 var output = (Output) result.AsyncState;

                                                 CloneComplete(output == null ? string.Empty : output.Output);
                                             }
                                         };

            completeProcess.BeginInvoke(null, null);
        }
    }
}