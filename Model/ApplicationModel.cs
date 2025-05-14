using AngleSharp.Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Search;

namespace Model
{
    public class ApplicationModel
    {
        public VideoDownloader downloader { get; private set; }
        private ConfigurationManager configManager;
        private YoutubeClient client;
        public Configuration Configuration
        {
            get => configManager.Configuration;

            set
            {
                configManager.Configuration = value;
                Task.Run(async () => await configManager.SaveAsync());
            }
        }

        public ApplicationModel()
        {
            configManager = new ConfigurationManager();
            client = new YoutubeClient();
            downloader = new VideoDownloader(client,configManager);
            configManager.Load();            
        }

        public List<VideoSearchResult> Search(string searchString)
        {
            return client.Search.GetResultsAsync(searchString).ToBlockingEnumerable().Where(x => x is VideoSearchResult).Select(x => (VideoSearchResult)x).ToList();
        }

        public async Task SaveConfig() => await configManager.SaveAsync();

        public async Task RequestDownload(string url, bool audioOnly = false) => await downloader.RequestDownload(url, audioOnly);
    }
}
