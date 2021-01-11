using Newtonsoft.Json;
using System.IO;

namespace WafclastRPG.Bot
{
    public class Config
    {
        internal string TokenRelease = "Your token...";
        internal string TokenDebug = "Your token...";

        internal string PrefixRelease = "Your prefix...";
        internal string PrefixDebug = "Your prefix...";

        public static Config LoadFromJsonFile(string path)
        {
            if (!File.Exists(path))
            {
                Config config = new Config();
                config.SaveToJsonFile(path);
                return null;
            }

            using (var sr = new StreamReader(path))
                return JsonConvert.DeserializeObject<Config>(sr.ReadToEnd());
        }

        public void SaveToJsonFile(string path)
        {
            using (var sw = new StreamWriter(path))
                sw.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
