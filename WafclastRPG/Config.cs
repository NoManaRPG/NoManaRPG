using Newtonsoft.Json;
using System.IO;

namespace WafclastRPG
{
    public class Config
    {
        [JsonProperty("TokenRelease")]
        internal string TokenRelease = "Your token...";
        [JsonProperty("TokenDebug")]
        internal string TokenDebug = "Your token...";

        [JsonProperty("PrefixRelease")]
        internal string PrefixRelease = "Your prefix...";
        [JsonProperty("PrefixDebug")]
        internal string PrefixDebug = "Your prefix...";

        public static Config LoadFromJsonFile(string path)
        {
            if (!File.Exists(path))
            {
                Config config = new Config();
                config.SaveToJsonFile(path);
                return null;
            }

            using var sr = new StreamReader(path);
            return JsonConvert.DeserializeObject<Config>(sr.ReadToEnd());
        }

        public void SaveToJsonFile(string path)
        {
            using var sw = new StreamWriter(path);
            sw.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
