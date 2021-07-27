using DSharpPlus;
using System;
using WafclastRPG.Entities;

namespace WafclastRPG.DataBases
{
    public class DatabaseRegions
    {
        public WafclastRegion Region0()
        {
            var reg = new WafclastRegion(0, "um Milharal")
            {
                Description = $"Os {Formatter.MaskedUrl("milho", new Uri("https://i.imgur.com/Ok72efh.jpg"))}s ainda estão verde. Tem um {Formatter.MaskedUrl("espantalho", new Uri("https://i.imgur.com/OlzufGa.jpg"))} olhando para mim"
            };
            reg.Monsters.Add(new DatabaseMonsters().Espantalho1Ab());

            return reg;
        }
    }
}
