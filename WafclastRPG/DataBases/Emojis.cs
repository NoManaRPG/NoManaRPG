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

        public const string Coins = "<a:coins:828775068540469269>";
        public const string Exp = "<:exp:828775431499022366>";

        public const string Relogio = ":stopwatch:";
        public const string Demonio = ":smiling_imp:";

        public const string Mago = ":man_mage:";

        public const string Um = "<:um:828775068917956668>";
        public const string Dois = "<:dois:828775068955574342>";
        public const string Tres = "<:tres:828775068858712124>";
        public const string Quatro = "<:quatro:828775068828958750>";
        public const string Cinco = "<:cinco:828775068485025793>";
        public const string Direita = "<:direita:828775068942991370>";
        public const string Esquerda = "<:esquerda:828775068921233408>";

        public const string Mapa = ":tokyo_tower:";
        public const string DiamanteLaranjaPequeno = ":small_orange_diamond:";
        public const string Aviso = ":warning:";

        public static string GerarVidaEmoji(double porcentagem)
        {
            switch (porcentagem)
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
