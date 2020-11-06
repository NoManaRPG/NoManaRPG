using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Metadata.Itens;

namespace WafclastRPG.Game.Metadata
{
    public sealed class Data
    {
        public static ConcurrentDictionary<int, WafclastRegiao> Regioes { get; private set; } = new ConcurrentDictionary<int, WafclastRegiao>();
        public Data()
        {
            CarregarRegioes();
        }

        private void CarregarRegioes()
        {
            var regioes = typeof(Regioes).GetMethods();
            for (int i = 0; i < regioes.Length - 4; i++)
            {
                var reg = (WafclastRegiao)regioes[i].Invoke(null, null);
                Regioes.AddOrUpdate(reg.Id, reg, (k, v) => reg);
            }
        }

        public static WafclastRegiao GetRegiao(int id)
        {
            if (Regioes.TryGetValue(id, out var reg))
                return reg;
            return null;
        }
    }
}
