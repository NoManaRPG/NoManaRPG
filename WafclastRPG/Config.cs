// This file is part of the WafclastRPG project.

using System.IO;
using Newtonsoft.Json;

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

        [JsonProperty("FirstRoom")]
        internal string FirstRoom = "Ulong Id";

        [JsonProperty("MapUrl")]
        internal string MapUrl = "Map Url";

        private const string Path = "Config.json";

        public static Config LoadFromJsonFile()
        {
            if (!File.Exists(Path))
            {
                Config config = new Config();
                config.SaveToJsonFile();
                return null;
            }

            using var sr = new StreamReader(Path);
            return JsonConvert.DeserializeObject<Config>(sr.ReadToEnd());
        }

        public void SaveToJsonFile()
        {
            using var sw = new StreamWriter(Path);
            sw.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}

