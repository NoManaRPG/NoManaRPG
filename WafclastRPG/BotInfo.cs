using System;

namespace WafclastRPG
{
    public class BotInfo
    {
        public int Membros;
        public int Guildas;
        public string Versão = "1.8";
        public DateTime TempoAtivo { get; set; } = DateTime.Now;
    }
}
