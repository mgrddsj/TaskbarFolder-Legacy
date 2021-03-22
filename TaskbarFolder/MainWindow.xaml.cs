using TaskbarFolder.Explorer;
using TaskbarFolder.Files;
using TaskbarFolder.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskbarFolder.Helpers;

namespace TaskbarFolder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool closing = false;

        public MainViewModel Model
        {
            get => this.DataContext as MainViewModel;
            set => this.DataContext = value;
        }

        public MainWindow()
        {
            InitializeComponent();
            string path = @"C:\Users\Jesse\Downloads";

            Model.TryNavigateToPath(path);
            Title = System.IO.Path.GetFileName(path);
            Icon = IconHelper.GetIconOfFile(path, true, true).ToImageSource();

            Reposition_Window();
        }

        private void Reposition_Window()
        {
            this.Left = SystemParameters.WorkArea.Left;
            this.Top = SystemParameters.WorkArea.Top;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (!closing)
                this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
        }
    }
}
