using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Model
{
    public class VideoDownloader
    {
        private YoutubeClient client = new YoutubeClient();
        private ConfigurationManager configManager;
        public event EventHandler<DownloadResolvedEventArgs>? DownloadResolved;
        public Action<double>? DownloadProgressCallback { get; set; }
        public Configuration Configuration 
        {
            get => configManager.Configuration;
            
            set 
            {
                configManager.Configuration = value;
                Task.Run(async() => await configManager.SaveAsync());
            }
        }

        internal VideoDownloader(YoutubeClient client,ConfigurationManager configManager)
        {
            this.client = client;
            this.configManager = configManager;           
        }       

        public async Task RequestDownload(string url,bool audioOnly=false)
        {
            try
            {
                string filename;
                if (audioOnly)
                    filename = await DownloadAudioOnly(url);
                else
                    filename = await DownloadFullVideo(url);
                DownloadResolved?.Invoke(this,new DownloadResolvedEventArgs(filename,DownloadResolvedEventArgs.DownloadState.Success));
            }

            catch (YoutubeExplode.Exceptions.VideoUnavailableException e) 
            {
                DownloadResolved?.Invoke(this,new DownloadResolvedEventArgs(string.Empty, DownloadResolvedEventArgs.DownloadState.VideoUnavailable,e.Message));
            }

            catch (YoutubeExplode.Exceptions.YoutubeExplodeException e)
            {
                DownloadResolved?.Invoke(this,new DownloadResolvedEventArgs(string.Empty, DownloadResolvedEventArgs.DownloadState.UnknownError, e.Message));
            }

            catch(ArgumentException)
            {
                DownloadResolved?.Invoke(this, new DownloadResolvedEventArgs(string.Empty, DownloadResolvedEventArgs.DownloadState.VideoUnavailable, url + " is not a valid yt link"));
            }
            catch(UnauthorizedAccessException)
            {
                DownloadResolved?.Invoke(this, new DownloadResolvedEventArgs(string.Empty, DownloadResolvedEventArgs.DownloadState.VideoUnavailable, "writing to file is disallowed by the OS" ));
            }
        }

        private struct VideoDescriptor 
        {
            public VideoId Id { get; set; }
            public StreamManifest Manifest { get; set; }
            public String Title {  get; set; } 
        }

        private async Task<VideoDescriptor> GetVideoDescriptor(string url)
        {
            VideoId videoId = url;
            var manifest = await client.Videos.Streams.GetManifestAsync(videoId);
            var videoTitle = client.Videos.GetAsync(videoId).Result.Title;
            return new VideoDescriptor {Id = videoId, Title = videoTitle, Manifest = manifest};
        }

        private async Task DownloadStream(IStreamInfo stream,string filename)
        {
            IProgress<double>? p = null;
            if (DownloadProgressCallback is not null)
            {
                p = new Progress<double>(progress =>
                {
                    DownloadProgressCallback!.Invoke(progress);
                });
            }

            await client.Videos.Streams.DownloadAsync(stream, filename, p);
        }

        private async Task<string> DownloadFullVideo(string url)
        {
            VideoDescriptor descriptor = await GetVideoDescriptor(url);
            string filename = Configuration.DownloadDirectory + "/" + NormalizeTitle(descriptor.Title) + ".mp4";
            var selectedStreamOption = descriptor.Manifest.GetMuxedStreams().FirstOrDefault()!;

            await DownloadStream(selectedStreamOption, filename);
            return filename;
        }

        private async Task<string> DownloadAudioOnly(string url)
        { 
            VideoDescriptor descriptor = await GetVideoDescriptor(url);
            string filename = Configuration.DownloadDirectory + "/"+NormalizeTitle(descriptor.Title) + ".mp3";
            var selectedStreamOption = descriptor.Manifest.GetAudioOnlyStreams().FirstOrDefault()!;

            await DownloadStream(selectedStreamOption, filename);
            return filename;
        }

        private string NormalizeTitle(string title) 
        {
            char[] illegalChars = {
                ' ','\t','\n',
                '.',':','-'
            };

            foreach(char c in illegalChars)
            {
                title = title.Replace(c, '_');
            }

            return title;
        }
    }
}
