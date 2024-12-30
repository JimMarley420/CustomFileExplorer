using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CustomFileExplorer.Services
{
    public class FileService
    {
        public List<string> GetDirectories(string path)
        {
            try
            {
                return Directory.GetDirectories(path).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public List<string> GetFiles(string path)
        {
            try
            {
                return Directory.GetFiles(path).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}
