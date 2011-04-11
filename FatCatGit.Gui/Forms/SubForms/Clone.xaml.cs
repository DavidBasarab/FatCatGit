using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using FatCatGit.Gui.Presenter.Presenters;
using FatCatGit.Gui.Presenter.Views;

namespace FatCatGit.Gui.Forms.SubForms
{
    public partial class Clone : BaseSubForm, CloneView
    {
        public Clone()
        {
            InitializeComponent();

            Presenter = new ClonePresenter(this);
        }

        private ClonePresenter Presenter { get; set; }

        private bool DestionationFolderVisible { get; set; }

        public string RepositoryToClone
        {
            get
            {
                string repositoryToClone = string.Empty;

                Action process = () => repositoryToClone = txtRepository.Text;

                Dispatcher.Invoke(process);

                return repositoryToClone;
            }
            set
            {
                Action process = () => txtRepository.Text = value;

                Dispatcher.Invoke(process);
            }
        }

        public string DestinationFolder
        {
            get
            {
                string repositoryToClone = string.Empty;

                Action process = () => repositoryToClone = txtDestination.Text;

                Dispatcher.Invoke(process);

                return repositoryToClone;
            }
            set
            {
                Action process = () => txtDestination.Text = value;

                Dispatcher.Invoke(process);
            }
        }

        public bool DestinationFolderDisplayed { get; set; }

        public void RespistoryToCloneChanged()
        {
            throw new NotImplementedException();
        }

        public void DisplayDestinationFolder()
        {
            DestionationFolderVisible = true;

            var showDestination = (Storyboard)Resources["DestinationShow"];

            showDestination.Begin();
        }

        public void HideDestinationFolder()
        {
            DestionationFolderVisible = false;

            var hideDestination = (Storyboard)Resources["DestinationFolderHide"];

            hideDestination.Begin();
        }

        private void DestinationLostFocus(object sender, RoutedEventArgs e)
        {
            Presenter.SetDestinationFolder(txtDestination.Text);
        }

        private void RepositoryLostFocus(object sender, RoutedEventArgs e)
        {
            Presenter.SetDestinationFolder(txtDestination.Text);
        }

        private void RepositoryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (ShouldShowDestionationFolder())
            {
                DisplayDestinationFolder();
            }

            if (ShouldHideDestinationFolder())
            {
                HideDestinationFolder();
            }
        }

        private bool ShouldHideDestinationFolder()
        {
            return string.IsNullOrEmpty(txtRepository.Text) && DestionationFolderVisible;
        }

        private bool ShouldShowDestionationFolder()
        {
            return !string.IsNullOrEmpty(txtRepository.Text) && !DestionationFolderVisible;
        }
    }
}