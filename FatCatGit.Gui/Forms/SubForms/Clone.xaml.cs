using System;
using System.Windows.Threading;
using FatCatGit.Gui.Presenter.Presenters;
using FatCatGit.Gui.Presenter.Views;
using System.Windows.Media.Animation;

namespace FatCatGit.Gui.Forms.SubForms
{
    public partial class Clone : BaseSubForm, CloneView
    {
        private ClonePresenter Presenter { get; set; }

        private bool DestionationFolderVisible { get; set; }

        public Clone()
        {
            InitializeComponent();

            Presenter = new ClonePresenter(this);
        }

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

        private void DestinationLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            Presenter.SetDestinationFolder(txtDestination.Text);
        }

        private void RepositoryLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            Presenter.SetDestinationFolder(txtDestination.Text);
        }

        private void RepositoryTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (ShouldShowDestionationFolder())
            {
                ShowDestinationFolder();
            }

            if (ShouldHideDestinationFolder())
            {
                HideDestinationFolder();
            }
        }

        private void HideDestinationFolder()
        {
            DestionationFolderVisible = false;

            var hideDestination = (Storyboard)Resources["DestinationFolderHide"];

            hideDestination.Begin();
        }

        private void ShowDestinationFolder()
        {
            DestionationFolderVisible = true;

            var showDestination = (Storyboard)Resources["DestinationShow"];

            showDestination.Begin();
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
