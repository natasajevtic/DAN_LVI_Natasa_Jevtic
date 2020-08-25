using System;
using System.IO;
using System.Net;
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

        private ICommand zip;
        public ICommand Zip
        {
            get
            {
                if (zip == null)
                {
                    zip = new RelayCommand(param => ZipExecute(), param => CanZipExecute());
                }
                return zip;
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
            //if zipped directory exists delete directory
            if (File.Exists(zippedFile))
            {
                File.Delete(zippedFile);
            }
        }
        /// <summary>
        /// This method downloads URL and saves in .html file in the directory.
        /// </summary>
        public void DownloadExecute()
        {
            //checking if user enter URL
            if (String.IsNullOrEmpty(Url))
            {
                MessageBox.Show("Please first enter URL.", "Notification");
            }
            else
            {
                //checking if user enter valid URL
                if (Uri.IsWellFormedUriString(Url, UriKind.Absolute))
                {
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            string source = string.Format(@"{0}/file{1}.html", directory, ++counterOfFiles);
                            //invoking method for downloading
                            client.DownloadFile(Url, source);
                            string sourceToCheck = string.Format(@"{0}/file{1}.html", directory, counterOfFiles);
                            //checking if downloading was successful
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
                            counterOfFiles--;
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
        /// <summary>
        /// This method zips the directory.
        /// </summary>
        public void ZipExecute()
        {            
            string[] fileNames = Directory.GetFiles(directory);
            //checking if directory contains files
            if (fileNames.Length != 0)
            {                
                try
                {                    
                    if (File.Exists(zippedFile))
                    {
                        File.Delete(zippedFile);
                    }
                    //invoking method for zipping
                    ZipFile.CreateFromDirectory(directory, zippedFile);
                    //checking if zipping was successful
                    if (File.Exists(zippedFile))
                    {
                        MessageBox.Show("Successful zipped.", "Notification");
                    }
                    else
                    {
                        MessageBox.Show("Cannot be zipped.", "Notification.");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Cannot be zipped.", "Notification.");
                }
            }
            else
            {
                MessageBox.Show("Please first download a file.", "Notification");
            }
        }

        public bool CanZipExecute()
        {
            return true;
        }
    }
}