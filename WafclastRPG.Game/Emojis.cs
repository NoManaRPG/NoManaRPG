// This file is part of the WafclastRPG project.

using System;
using WafclastRPG.Game.Entities;

namespace WafclastRPG
{
    public static class Emojis
    {
        public const string CrossBone = ":skull_crossbones:";
        public const string Up = ":up:";

        public const string CoracaoVerde = ":green_heart:";
        public const string CoracaoAmarelo = ":yellow_heart:";
        public const string CoracaoLaranja = ":orange_heart:";
        public const string CoracaoVermelho = ":heart:";
        public const string CoracaoPreto = ":black_heart:";

        public const string CirculoVerde = ":green_circle:";
        public const string CirculoAmarelo = ":yellow_circle:";
        public const string CirculoLaranja = ":orange_circle:";
        public const string CirculoVermelho = ":red_circle:";

        public const string Adaga = ":dagger:";
        public const string Dardo = ":dart:";
        public const string EspadasCruzadas = ":crossed_swords:";
        public const string Escudo = ":shield:";

        public const string CarinhaNervoso = ":rage:";
        public const string CarinhaDesapontado = ":disappointed_relieved:";

        public const string ExplacamaoDupla = ":bangbang:";

        public const string Fome = ":poultry_leg:";
        public const string Sede = ":cup_with_straw:";

        public const string Coins = "<a:coins:828775068540469269>";
        public const string Exp = "<:exp:828775431499022366>";

        public const string Relogio = ":stopwatch:";
        public const string Demonio = ":smiling_imp:";

        public const string Mago = ":man_mage:";

        public const string Mapa = ":tokyo_tower:";
        public const string DiamanteLaranjaPequeno = ":small_orange_diamond:";
        public const string Aviso = ":warning:";

        public static string DinamicHeartEmoji(WafclastStatePoints attribute) =>
            (attribute.Current / attribute.Max) switch
            {
                double n when n > 0.75 => CoracaoVerde,
                double n when n > 0.50 => CoracaoAmarelo,
                double n when n > 0.25 => CoracaoLaranja,
                _ => CoracaoVermelho,
            };
    }
}
