using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence;

namespace Model
{
    internal class ConfigurationManager
    {
        private string configFile;
        public Configuration Configuration { get; set; }
        public ConfigurationManager() 
        {
            configFile = Path.Combine(Persistence.System.DefaultDataDirectory(),"config.conf");
            Configuration = DefaultConfiguration();
        }

        public async Task SaveAsync() 
        {
            await Persistence.System.SaveFileAsync(configFile,ConfigToString());
        }

        public async Task LoadAsync() 
        {
            Configuration = ParseConfig(await Persistence.System.LoadFileAsync(configFile));
        }
        public void Save()
        {
            try
            {
                Persistence.System.SaveFile(configFile, ConfigToString());
            }
            catch (Exception) { }
        }

        public void Load()
        {
            try {
                Configuration = ParseConfig(Persistence.System.LoadFile(configFile));            
            }
            catch(Exception) { }            
        }

        private Configuration ParseConfig(string str)
        {
            try
            {
                string[] pars = str.Split('#');
                if (pars[0] == "download")
                {
                    return new Configuration(pars[1]);
                }
                return DefaultConfiguration();
            }
            catch(Exception)
            {
                return DefaultConfiguration();
            }
        }

        private string ConfigToString()
        {
            return "download#" + Configuration.DownloadDirectory;
        }

        private Configuration DefaultConfiguration() => new Configuration(Persistence.System.DefaultDataDirectory());
    }
}
