using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using FatCatGit.Common.Interfaces;
using FatCatGit.Gui.Presenter.Presenters;
using FatCatGit.Gui.Presenter.Views;
using Ninject;
using Ninject.Parameters;

namespace FatCatGit.Gui.Forms.SubForms
{
    public partial class Clone : BaseSubForm, CloneView
    {
        public Clone()
        {
            InitializeComponent();

            CreatePresenter();
        }

        private void CreatePresenter()
        {
            Presenter = Global.GitCommandModule.Get<ClonePresenter>(new ConstructorArgument("view", this));
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

        public void RespositoryToCloneChanged()
        {
            Presenter.RepositoryToCloneChanged();
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

        public void ShowCloneButton()
        {
            var anmination = (Storyboard)Resources["ShowCloneButton"];

            anmination.Begin();
        }

        public void HideCloneButton()
        {
            var anmination = (Storyboard)Resources["HideCloneButton"];

            anmination.Begin();
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
            RespositoryToCloneChanged();
        }

        private void DestinationTextChanged(object sender, TextChangedEventArgs e)
        {
            Presenter.DestionFolderTextChanged();
        }

        private void CloneClick(object sender, RoutedEventArgs e)
        {
            Presenter.PerformClone();

            Action<Output> cloneCompleteProcess = o =>
                                                      {
                                                          Action displayBoxes = () =>
                                                                                    {
                                                                                        MessageBox.Show(string.Format("Output: {0}", o.Output));
                                                                                        MessageBox.Show(string.Format("Error: {0}", o.ErrorOutput));
                                                                                    };

                                                          Dispatcher.Invoke(displayBoxes);
                                                      };

            Presenter.CloneComplete += cloneCompleteProcess;
        }
    }
}