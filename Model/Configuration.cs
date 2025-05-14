using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Configuration
    {
        public Configuration(string downloadDir) 
        {
            DownloadDirectory = downloadDir;            
        }

        public string DownloadDirectory { get; private set; }
    }
}
