namespace FatCatGit.Gui.Presenter.Views
{
    public interface CloneView
    {
        string RepositoryToClone { get; set; }
        string DestinationFolder { get; set; }

        void DisplayDestinationFolder();
        void HideDestinationFolder();

        void ShowCloneButton();
        void HideCloneButton();
    }
}