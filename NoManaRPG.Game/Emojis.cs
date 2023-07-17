// This file is part of WafclastRPG project.

using NoManaRPG.Game.Entities;

namespace NoManaRPG
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

        public static string GerarVidaEmoji(StatePoints lifeAttribute)
        {
            switch (lifeAttribute.Current / lifeAttribute.Max)
            {
                case double n when (n > 0.75):
                    return Emojis.CoracaoVerde;
                case double n when (n > 0.50):
                    return Emojis.CoracaoAmarelo;
                case double n when (n > 0.25):
                    return Emojis.CoracaoLaranja;
            }
            return Emojis.CoracaoVermelho;
        }

        public static string TesteEmoji(StatePoints lifeAttribute) =>
           (lifeAttribute.Current / lifeAttribute.Max) switch
           {
               double n when (n > 0.75) => Emojis.CoracaoVerde,
               double n when (n > 0.50) => Emojis.CoracaoAmarelo,
               double n when (n > 0.25) => Emojis.CoracaoLaranja,
               _ => Emojis.CoracaoVermelho,
           };
    }
}
