using System.ComponentModel;
using System.Windows;

namespace FatCatGit.Gui.Forms.SubForms
{
    public class BaseSubPage : Window
    {
        private void ShowParentWindow()
        {
            var parentWindow = GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Owner.Visibility = Visibility.Visible;
            }
        }

        protected void FormClosing(object sender, CancelEventArgs e)
        {
            ShowParentWindow();
        }
    }
}