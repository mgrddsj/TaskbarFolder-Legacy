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
using System.Diagnostics;

namespace TaskbarFolder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool closing = false;
        readonly string path = @"C:\Users\Jesse\Downloads";

        public MainViewModel Model
        {
            get => this.DataContext as MainViewModel;
            set => this.DataContext = value;
        }

        public MainWindow()
        {
            InitializeComponent();

            Model.NavigateOnStartup(path);
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

        private void OpenInExplorer_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(path);
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void HandleScrollSpeed(object sender, MouseWheelEventArgs e)
        {
            try
            {
                if (!(sender is DependencyObject))
                    return;

                ScrollViewer scrollViewer = (((DependencyObject)sender)).GetScrollViewer() as ScrollViewer;
                ListBox lbHost = sender as ListBox; //Or whatever your UI element is

                if (scrollViewer != null && lbHost != null)
                {
                    double scrollSpeed = 0.1;
                    //you may check here your own conditions
                    if (lbHost.Name == "SourceListBox" || lbHost.Name == "TargetListBox")
                        scrollSpeed = 2;

                    double offset = scrollViewer.VerticalOffset - (e.Delta * scrollSpeed/6);
                    if (offset < 0)
                        scrollViewer.ScrollToVerticalOffset(0);
                    else if (offset > scrollViewer.ExtentHeight)
                        scrollViewer.ScrollToVerticalOffset(scrollViewer.ExtentHeight);
                    else
                        scrollViewer.ScrollToVerticalOffset(offset);

                    e.Handled = true;
                }
                else
                    throw new NotSupportedException("ScrollSpeed Attached Property is not attached to an element containing a ScrollViewer.");
            }
            catch (Exception ex)
            {
                //Do something...
            }
        }
    }
}
