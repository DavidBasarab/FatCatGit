using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace FatCatGit.Gui.Forms.SubForms
{
    public class BaseSubPage : Window
    {
        private void ShowParentWindow()
        {
            Window parentWindow = Window.GetWindow(this);

            parentWindow.Owner.Visibility = Visibility.Visible;
        }

        private void FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShowParentWindow();
        }
    }
}
