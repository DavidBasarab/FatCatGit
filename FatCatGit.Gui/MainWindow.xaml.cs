using System.Windows;
using FatCatGit.Gui.Forms.SubForms;

namespace FatCatGit.Gui
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            ShowCloneForm();
        }

        private void ShowCloneForm()
        {
            Visibility = Visibility.Hidden;

            var cloneForm = new Clone
                                {
                                    Owner = this
                                };

            cloneForm.ShowDialog();
        }

        private void FormLoaded(object sender, RoutedEventArgs e)
        {
            Global.LoadModules();
        }
    }
}