using Newtonsoft.Json;
using System;
using System.IO;

namespace DragonsDiscordRPG
{
    public class BotInfo
    {
        [JsonIgnore]
        public int QuantidadeMembros { get; set; }

        [JsonIgnore]
        public DateTime TempoAtivo { get; set; } = DateTime.Now;

        [JsonProperty("versaoMaior")]
        public int VersaoMaior { get; set; } = 1;

        [JsonProperty("versaoMinor")]
        public int VersaoMinor { get; set; } = 1;

        [JsonProperty("versaoRevisao")]
        public int VersaoRevisao { get; set; } = 0;

        public static BotInfo LoadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                BotInfo config = new BotInfo();
                config.SaveToFile(path);
                return null;
            }

            using (var sr = new StreamReader(path))
                return JsonConvert.DeserializeObject<BotInfo>(sr.ReadToEnd());
        }

        public void SaveToFile(string path)
        {
            using (var sw = new StreamWriter(path))
                sw.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
