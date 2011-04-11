using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FatCatGit.Gui.Presenter.Views
{
    public interface CloneView
    {
        string RepositoryToClone { get; set; }
        string DestinationFolder { get; set; }

        void DisplayDestinationFolder();
        void HideDestinationFolder();
    }
}
