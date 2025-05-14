using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DownloadResolvedEventArgs : EventArgs
    {
        public enum DownloadState { Success,VideoUnavailable,UnknownError }
        public DownloadResolvedEventArgs(string filename, DownloadState state, string errorMessage="") 
        {
            this.filename = filename;
            this.errorMessage = errorMessage;
            this.state = state;
        }

        public string filename { get; private set; }
        public string errorMessage {  get; private set; }
        public DownloadState state { get; private set; }
    }
}
