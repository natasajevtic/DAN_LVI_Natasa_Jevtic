using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zadatak_1.Command;
using System.IO.Compression;

namespace Zadatak_1.ViewModel
{
    class MainWindowViewModel : BaseViewModel
    {
        MainWindow mainWindow;

        private string url;

        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
                OnPropertyChanged("Url");
            }
        }

        private ICommand download;
        public ICommand Download
        {
            get
            {
                if (download == null)
                {
                    download = new RelayCommand(param => DownloadExecute(), param => CanDownloadExecute());
                }
                return download;
            }
        }

        static int counterOfFiles = 0;

        string directory = @"../../HTML_files";
        string zippedFile = @"../../HTML_files.zip";

        public MainWindowViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            //if directory exists delete all files in directory
            if (Directory.Exists(directory))
            {
                DirectoryInfo di = new DirectoryInfo(directory);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }
            //if directory doesn't exist create directory
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(directory);
            }            
        }
        public void DownloadExecute()
        {
            if (String.IsNullOrEmpty(Url))
            {
                MessageBox.Show("Please first enter url.", "Notification");
            }
            else
            {
                if (Uri.IsWellFormedUriString(Url, UriKind.Absolute))
                {
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            string source = string.Format(@"{0}/file{1}.html", directory, ++counterOfFiles);
                            client.DownloadFile(Url, source);
                            string sourceToCheck = string.Format(@"{0}/file{1}.html", directory, counterOfFiles);
                            if (File.Exists(sourceToCheck))
                            {
                                MessageBox.Show("HTML is downloaded.", "Notification");
                            }
                            else
                            {
                                MessageBox.Show("HTML isn't downloaded.", "Notification");
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Entered URL doesn't exist.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You entered an invalid URL.", "Warning", MessageBoxButton.OK , MessageBoxImage.Warning);
                }
            }
        }

        public bool CanDownloadExecute()
        {
            return true;
        }        
    }
}