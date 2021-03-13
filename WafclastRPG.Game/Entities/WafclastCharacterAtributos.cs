namespace WafclastRPG.Game.Entities
{
    public class WafclastCharacterAtributos
    {
        /// <summary>
        /// Aumenta o dano físico máximo em 2 por cada ponto.
        /// </summary>
        public int Forca { get; private set; } = 4;

        /// <summary>
        /// Aumenta a vida máxima em 4 por cada ponto.
        /// </summary>
        public int Resistencia { get; private set; } = 4;

        /// <summary>
        /// Aumenta a velocidade de recuperação de vida por mensagem. Cada ponto recupera 0.2 de vida.
        /// </summary>
        public int Vitalidade { get; private set; } = 4;
        public int PontosLivreAtributo { get; set; } = 4;
    }
}
