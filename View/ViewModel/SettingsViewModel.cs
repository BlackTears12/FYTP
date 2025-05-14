using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Storage;

namespace FYTP.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private string downloadDirectory;
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<Configuration>? ConfigurationChanged;

        public ICommand SelectDirectory { get; private set; }
        public string DownloadDirectory
        {
            get => downloadDirectory;
            private set
            {
                if (value != downloadDirectory)
                {
                    downloadDirectory = value;
                    OnPropertyChanged();
                    ConfigurationChanged?.Invoke(this, new Configuration(DownloadDirectory));
                }
            }
        }

        public SettingsViewModel(string downloadDir)
        {
            downloadDirectory = downloadDir;
            SelectDirectory = new Command(async() => await SelectDownloadDirectory());
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private async Task SelectDownloadDirectory()
        {
            try
            {
                var result = await FolderPicker.Default.PickAsync();
                if (result.IsSuccessful)
                {
                    DownloadDirectory = result.Folder.Path;
                }
            }catch(Exception)
            {

            }
        }
    }
}
