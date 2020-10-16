using DSharpPlus.Entities;

namespace TorreRPG.Services
{
    public class Emoji
    {
        public static DiscordEmoji OrbVida { get => DiscordEmoji.FromGuildEmote(Bot.Cliente, 750816723372081233); }
        public static DiscordEmoji OrbMana { get => DiscordEmoji.FromGuildEmote(Bot.Cliente, 750818188425560155); }
        public static DiscordEmoji CrossBone { get => DiscordEmoji.FromName(Bot.Cliente, ":skull_crossbones:"); }
        public static DiscordEmoji Up { get => DiscordEmoji.FromName(Bot.Cliente, ":up:"); }
        public static DiscordEmoji ItemNormal { get => DiscordEmoji.FromName(Bot.Cliente, ":ItemNormal:"); }
        public static DiscordEmoji ItemMagico { get => DiscordEmoji.FromName(Bot.Cliente, ":ItemMagico:"); }
        public static DiscordEmoji ItemRaro { get => DiscordEmoji.FromName(Bot.Cliente, ":ItemRaro:"); }
        public static DiscordEmoji ItemUnico { get => DiscordEmoji.FromName(Bot.Cliente, ":ItemUnico:"); }

        public const string QuadradoAzul = ":blue_square:";
        public const string QuadradoNorte = ":regional_indicator_n:";
        public const string QuadradoSul = ":regional_indicator_s:";
        public const string QuadradoLeste = ":regional_indicator_l:";
        public const string QuadradoOeste = ":regional_indicator_o:";
    }
}
