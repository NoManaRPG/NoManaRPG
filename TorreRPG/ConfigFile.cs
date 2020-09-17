using Newtonsoft.Json;
using System.IO;

namespace TorreRPG
{
    public class ConfigFile
    {
        /// <summary>
        /// O token do seu bot
        /// </summary>
        [JsonProperty("clientToken")]
        internal string Token = "Seu token...";

        /// <summary>
        /// O token de teste do seu bot
        /// </summary>
        [JsonProperty("clientTokenTeste")]
        internal string TokenTeste = "Seu token de teste...";

        /// <summary>
        /// O prefix do seu bot
        /// </summary>
        [JsonProperty("prefix")]
        internal string Prefix = "O prefix do seu bot...";

        /// <summary>
        /// O prefix de teste do seu bot
        /// </summary>
        [JsonProperty("prefixTeste")]
        internal string PrefixTeste = "O prefix de teste do seu bot...";

        /// <summary>
        /// O client secret
        /// </summary>
        [JsonProperty("clientSecret")]
        internal string Secret = "Seu Client Secret...";

        /// <summary>
        /// O client id
        /// </summary>
        [JsonProperty("clientId")]
        internal string Client = "Seu Client ID...";

        /// <summary>
        /// A sua key do DiscordBots.org
        /// </summary>
        [JsonProperty("discordBotsKey")]
        internal string DiscordBotsKey = "Sua key do DiscordBots";

        /// <summary>
        /// Carrega a config de um arquivo JSON.
        /// </summary>
        /// <param name="path">Caminho para o arquivo config.</param>
        /// <returns></returns>
        public static ConfigFile LoadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                ConfigFile config = new ConfigFile();
                config.SaveToFile(path);
                return null;
            }

            using (var sr = new StreamReader(path))
                return JsonConvert.DeserializeObject<ConfigFile>(sr.ReadToEnd());
        }

        /// <summary>
        /// Salva as config para um arquivo JSON.
        /// </summary>
        /// <param name="path">Caminho para o arquivo config.</param>
        public void SaveToFile(string path)
        {
            using (var sw = new StreamWriter(path))
                sw.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
