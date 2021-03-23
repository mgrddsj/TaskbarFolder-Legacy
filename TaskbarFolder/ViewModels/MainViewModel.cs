using TaskbarFolder.Explorer;
using TaskbarFolder.Files;
using NamespaceHere;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace TaskbarFolder.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<FilesControl> FileItems { get; set; }

        public MainViewModel()
        {
            FileItems = new ObservableCollection<FilesControl>();
        }

        #region Navigation

        public void TryNavigateToPath(string path)
        {
            // is a drive
            if (path == string.Empty)
            {
                ClearFiles();

                foreach(FileModel drive in Fetcher.GetDrives())
                {
                    FilesControl fc = CreateFileControl(drive);
                    AddFile(fc);
                }
            }

            else if (path.IsFile())
            {
                // Open the file
                // MessageBox.Show($"Opening {path}");
                Process.Start(path);
            }

            else if (path.IsDirectory())
            {
                Process.Start(path);
            }

            else
            {
                // something bad has happened...
            }
        }

        public void NavigateFromModel(FileModel file)
        {
            TryNavigateToPath(file.Path);
        }

        public void NavigateOnStartup(string path)
        {
            ClearFiles();

            List<FileModel> filesAndFolders = Fetcher.GetDirectories(path);
            filesAndFolders.AddRange(Fetcher.GetFiles(path));
            filesAndFolders.Sort((x, y) => DateTime.Compare(x.DateModified, y.DateModified));
            filesAndFolders.Reverse();

            foreach (FileModel file in filesAndFolders)
            {
                FilesControl fc = CreateFileControl(file);
                AddFile(fc);
            }
        }

        #endregion

        public void AddFile(FilesControl file)
        {
            FileItems.Add(file);
        }

        public void RemoveFile(FilesControl file)
        {
            FileItems.Remove(file);
        }

        public void ClearFiles()
        {
            FileItems.Clear();
        }

        public FilesControl CreateFileControl(FileModel fModel)
        {
            FilesControl fc = new FilesControl(fModel);
            SetupFileControlCallbacks(fc);
            return fc;
        }

        public void SetupFileControlCallbacks(FilesControl fc)
        {
            fc.NavigateToPathCallback = NavigateFromModel;
        }
    }
}
