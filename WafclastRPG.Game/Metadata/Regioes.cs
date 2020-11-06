using System.Collections.Generic;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Game.Metadata
{
    public sealed class Regioes
    {
        public static WafclastRegiao BelsierAb()
            => new WafclastRegiao(1, WafclastRegioes.Meizhou, "Belsier", null,
                new List<WafclastMonstro>()
                { Monstros.VacaAb(),
                  Monstros.SlimeAb(),
                  Monstros.BandidoAb()
                });
    }
}
