using System.Text;
using CommunityToolkit.Maui.Storage;
using Microsoft.Maui.Storage;

namespace Persistence
{
    public class System
    {
        public static async Task SaveFileAsync(string filename, string data)
        {
            await File.WriteAllBytesAsync(filename, Encoding.Default.GetBytes(data));            
        }

        public static async Task<string> LoadFileAsync(string filename)
        {
            return Encoding.Default.GetString(await File.ReadAllBytesAsync(filename));
        }

        public static void SaveFile(string filename, string data)
        {
            File.WriteAllBytes(filename, Encoding.Default.GetBytes(data));            
        }

        public static string LoadFile(string filename)
        {
            return Encoding.Default.GetString(File.ReadAllBytes(filename));
        }

        public static string DefaultDataDirectory()
        {
            return FileSystem.Current.AppDataDirectory;
        }
    }
}
