using System;
using FatCatGit.Common.Interfaces;
using FatCatGit.GitCommands.Interfaces;
using FatCatGit.Gui.Presenter.Exceptions;
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

        [Inject]
        public Clone Clone { get; set; }

        public event Action<Output> CloneComplete;

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
            VerifyCloneCanBePerformed();

            SetUpClone();

            IAsyncResult result = Clone.Run();

            MonitorCloneProcess(result);
        }

        private void VerifyCloneCanBePerformed()
        {
            if (string.IsNullOrEmpty(View.RepositoryToClone))
            {
                throw new CannotCloneException("RepositoryToClone is required.");
            }

            if (string.IsNullOrEmpty(View.DestinationFolder))
            {
                throw new CannotCloneException("DestinationFolder is required.");
            }
        }

        private void MonitorCloneProcess(IAsyncResult result)
        {
            Action<IAsyncResult> completeProcess = WaitForCloneProcessToComplete;

            completeProcess.BeginInvoke(result, null, null);
        }

        private void WaitForCloneProcessToComplete(IAsyncResult result)
        {
            result.AsyncWaitHandle.WaitOne();

            var output = (Output) result.AsyncState;

            FireCloneCompleteEvent(output);
        }

        private void FireCloneCompleteEvent(Output output)
        {
            if (CloneComplete != null)
            {
                CloneComplete(output);
            }
        }

        private void SetUpClone()
        {
            Clone.Destination = View.DestinationFolder;
            Clone.RepositoryToClone = View.RepositoryToClone;
        }
    }
}