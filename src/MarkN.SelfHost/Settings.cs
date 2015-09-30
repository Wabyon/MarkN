using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MarkN.SelfHost
{
    public class Settings
    {
        private const string FileName = @"settings.json";

        private static Settings _instance;

        private Settings()
        {
        }

        public static Settings Instance
        {
            get { return _instance ?? (_instance = ReadJson()); }
        }

        private int _port = 8080;

        public int Port
        {
            get { return _port; }
            set
            {
                if (_port == value) return;

                _port = value;
                WriteJson();
            }
        }

        private void WriteJson()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented,
                new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});
            File.WriteAllText(FileName, json, Encoding.UTF8);
        }

        private static Settings ReadJson()
        {
            if (!ExistsFile) return new Settings();

            var json = File.ReadAllText(FileName);
            return JsonConvert.DeserializeObject<Settings>(json);
        }

        public static bool ExistsFile
        {
            get { return File.Exists(FileName); }
        }
    }
}
