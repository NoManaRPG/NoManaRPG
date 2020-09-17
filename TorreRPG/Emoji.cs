using DSharpPlus.Entities;

namespace TorreRPG
{
    public class Emoji
    {
        public static DiscordEmoji OrbVida { get => DiscordEmoji.FromGuildEmote(ModuloCliente.Client, 750816723372081233); }
        public static DiscordEmoji OrbMana { get => DiscordEmoji.FromGuildEmote(ModuloCliente.Client, 750818188425560155); }
        public static DiscordEmoji CrossBone { get => DiscordEmoji.FromName(ModuloCliente.Client, ":skull_crossbones:"); }
        public static DiscordEmoji Up { get => DiscordEmoji.FromName(ModuloCliente.Client, ":up:"); }
        public const string QuadradoAzul = ":blue_square:";
        public const string QuadradoNorte = ":regional_indicator_n:";
        public const string QuadradoSul = ":regional_indicator_s:";
        public const string QuadradoLeste = ":regional_indicator_l:";
        public const string QuadradoOeste = ":regional_indicator_o:";
        public const string Mago = ":mage:";
    }
}
