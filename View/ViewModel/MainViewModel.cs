using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using Model;
using static Model.DownloadResolvedEventArgs;

namespace FYTP.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<Page>? PageOpened;
        private string URL;
        private string downloadState;
        private double downloadProgress;
        private bool downloadInProgress;
        private bool audioOnly;
        private Color downloadButtonColor;
        private ApplicationModel model;
        private SettingsViewModel settingsViewModel;

        #endregion

        #region Attributes
        public ICommand DownloadCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }
        public string VideoURL
        {
            get => URL;
            set
            {
                if (URL != value)
                {
                    URL = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DownloadOutput
        {
            get => downloadState;
            private set
            {
                downloadState = value;
                OnPropertyChanged();
            }
        }

        public double DownloadProgress 
        {   get => downloadProgress;
            private set 
            {
                downloadProgress = value;
                OnPropertyChanged();
            } 
        }

        public Color DownloadButtonColor
        {
            get => downloadButtonColor;
            private set
            {
                downloadButtonColor = value;
                OnPropertyChanged();
            }
        }

        public bool AudioOnly
        {
            get => audioOnly;
            set
            {
                audioOnly = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public MainViewModel(ApplicationModel model)
        {
            downloadInProgress = false;
            audioOnly = true;
            URL = string.Empty;
            downloadState = string.Empty;
            downloadProgress = 0.0;
            downloadButtonColor = Color.FromArgb("#512BD4");
            this.model = model;
            settingsViewModel = new SettingsViewModel(model.Configuration.DownloadDirectory);
            model.downloader.DownloadResolved += OnDownloadResolved;
            settingsViewModel.ConfigurationChanged += (object? sender, Configuration configuration) => model.downloader.Configuration = configuration;
            DownloadCommand = new Command(() => StartDownload());
            SettingsCommand = new Command(() => Settings());
            model.downloader.DownloadProgressCallback = (progress) => DownloadProgress = progress;
        }

        #region EventHandlers
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void OnDownloadResolved(object? sender, DownloadResolvedEventArgs e)
        {
            DownloadButtonColor = Color.FromArgb("#512BD4");
            downloadInProgress = false;
            if (e.state == DownloadState.Success)
            {
                DownloadOutput = "Download successful";
            }
            else
            {
                DownloadOutput = "Error: " + e.errorMessage;
            }
        }

        #endregion

        #region Private Methods
        private void StartDownload()
        {
            if (downloadInProgress)
                return;
            downloadInProgress = true;
            DownloadButtonColor = Color.FromArgb("#808080");
            DownloadOutput = "Download started";
            DownloadProgress = 0.0;
            Task.Run(async () => await model.downloader.RequestDownload(URL,audioOnly));
        }

        private void Settings()
        {
            PageOpened?.Invoke(this,View.App.CreateSettingsPage(settingsViewModel));
        }

        #endregion
    }
}
