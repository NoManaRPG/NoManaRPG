namespace WafclastRPG.DataBases
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
        public const string EspadasCruzadas = ":crossed_swords:";
        public const string Escudo = ":shield:";

        public const string CarinhaNervoso = ":rage:";
        public const string CarinhaDesapontado = ":disappointed_relieved:";

        public const string ExplacamaoDupla = ":bangbang:";

        public const string Fome = ":poultry_leg:";
        public const string Sede = ":cup_with_straw:";

        public const string Coins = "<a:ee:826260040741552128>";
        public const string Exp = "<:xp:826263422202544139>";
        public const string Nada = "<:nada:826438756193533953>";

        public const string Relogio = ":stopwatch:";
        public const string Demonio = ":smiling_imp:";

        public const string Mago = ":man_mage:";

        public const string Um = "<:um:826799998367825940>";
        public const string Dois = "<:dois:826799998372020264>";
        public const string Tres = "<:tres:826799998380671076>";
        public const string Quatro = "<:quatro:826799998590255114>";
        public const string Cinco = "<:cinco:826799998372937728>";
        public const string Direita = "<:direita:826799998159159337>";
        public const string Esquerda = "<:esquerda:826799998360485938>";

        public const string Mapa = ":map:";
        public const string DiamanteLaranjaPequeno = ":small_orange_diamond:";
        public const string Aviso = ":warning:";

        public static string GerarVidaEmoji(decimal porcentagem)
        {
            switch (porcentagem)
            {
                case decimal n when (n > 0.75m):
                    return Emojis.CoracaoVerde;
                case decimal n when (n > 0.50m):
                    return Emojis.CoracaoAmarelo;
                case decimal n when (n > 0.25m):
                    return Emojis.CoracaoLaranja;
            }
            return Emojis.CoracaoVermelho;
        }

        public static string GerarNumber(int numero)
        {
            switch (numero)
            {
                case 1:
                    return Um;
                case 2:
                    return Dois;
                case 3:
                    return Tres;
                case 4:
                    return Quatro;
                case 5:
                    return Cinco;
            }
            return "Emoji não encontrado";
        }
    }
}
