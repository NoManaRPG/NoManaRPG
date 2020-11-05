using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Metadata.Itens;

namespace WafclastRPG.Game.Metadata
{
    public class Data
    {
        public static List<WafclastMonstro> Monstros { get; set; } = new List<WafclastMonstro>();

        public Data()
        {
            CarregarMonstros();
        }

        public void CarregarMonstros()
        {
            var montros = typeof(Monstros).GetMethods();
            for (int i = 0; i < montros.Length - 4; i++)
            {
                Monstros.Add((WafclastMonstro)montros[i].Invoke(null, null));
            }
        }
    }
}
