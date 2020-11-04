using System.Collections.Generic;
using WafclastRPG.Game.Entidades.Itens;

namespace WafclastRPG.Game.Entidades
{
    public class WafclastMochila
    {
        public List<WafclastItem> Itens { get; set; } = new List<WafclastItem>();

        public int EspacoAtual { get; private set; }

        public int EspacoMax { get; private set; } = 64;

        public WafclastMochila() { }
    }
}
