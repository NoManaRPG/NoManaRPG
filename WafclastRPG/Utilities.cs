using DSharpPlus;
using System;

namespace WafclastRPG.Bot
{
    public static class Utilities
    {
        public static string FormatarURLComando(string texto, string hover, string site = "https://discord.gg/MAR4NFq")
            => Formatter.MaskedUrl($"`!{texto}` ", new Uri(site), hover);

        public static string FormatarURL(string texto, string site)
          => Formatter.MaskedUrl($"`{texto}` ", new Uri(site));
    }
}
