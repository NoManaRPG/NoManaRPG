using Newtonsoft.Json;
using System;
using System.IO;

namespace DragonsDiscordRPG.Entidades
{
    public class BotCore
    {
        [JsonIgnore]
        public const ulong Id = 459873132975620134;

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

        public static BotCore LoadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                BotCore config = new BotCore();
                config.SaveToFile(path);
                return null;
            }

            using (var sr = new StreamReader(path))
            {
                return JsonConvert.DeserializeObject<BotCore>(sr.ReadToEnd());
            }
        }

        public void SaveToFile(string path)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
            }
        }
    }
}
