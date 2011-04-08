using System;
using FatCatGit.Gui.Presenter.Presenters;
using FatCatGit.Gui.Presenter.Views;

namespace FatCatGit.Gui.Forms.SubForms
{
    public partial class Clone : BaseSubPage, CloneView
    {
        public ClonePresenter Presenter { get; set; }

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
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
