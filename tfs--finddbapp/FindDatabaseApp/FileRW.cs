using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace FindDatabaseApp
{
    public class FileRW
    {
        public string path { get; set; }

    }
    public class FileData
    {
        public string serverName;
        public IDictionary<string, string> UserPassDict = new Dictionary<string, string>();
        //public List<FileData> fileData = new List<FileData>();

        public FileData(string servername, IDictionary<string,string> userpassdict)
        {
            serverName = servername;
            UserPassDict = userpassdict;
        }
        public FileData ()
        {
            serverName = "";
            UserPassDict = new Dictionary<string, string>();
        }
    }

    public static class FileOperations
    {
        public static List<FileData> readFromFile(FileRW obj )
        {
            List<FileData> filedt = new List<FileData>();
            obj.path = Assembly.GetEntryAssembly().Location;
            string cleanedPath = Path.GetDirectoryName(obj.path);
            //string json = "";
            string newPath = cleanedPath + "\\date.json";
            if (File.Exists(newPath))
            {
                using (StreamReader r = new StreamReader(newPath))
                {
                    string json = r.ReadToEnd();
                    filedt = JsonConvert.DeserializeObject<List<FileData>>(json);
                }
            }
            else
            {
               obj.path = Assembly.GetEntryAssembly().Location;
               string cleanedPathh = Path.GetDirectoryName(obj.path);
               string json = JsonConvert.SerializeObject(filedt);
               File.WriteAllText(cleanedPathh + "\\date.json", json);
            }

            return filedt;
        }

        public static string writeToFile(FileRW obj, List<FileData> fileData)
        {
            //List<FileData> fileData = new List<FileData>();
            obj.path = Assembly.GetEntryAssembly().Location;
            string cleanedPath = Path.GetDirectoryName(obj.path);
            string json = JsonConvert.SerializeObject(fileData);
            File.WriteAllText(cleanedPath + "\\date.json", json);
            return cleanedPath + "\\date.json";
        }

      
    }
}
