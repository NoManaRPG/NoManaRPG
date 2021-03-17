namespace WafclastRPG.Game.Entities
{
    public class WafclastCharacterAtributos
    {
        /// <summary>
        /// Aumenta o ataque máximo em 2 por cada ponto atribuído.
        /// </summary>
        public int Forca { get; private set; } = 4;

        /// <summary>
        /// Aumenta a vida máxima em 4 e o vigor em 5 por cada ponto atribuído.
        /// </summary>
        public int Resistencia { get; private set; } = 4;

        /// <summary>
        /// Aumenta a chance de desviar de um ataque inimigo.
        /// </summary>
        public int Agilidade { get; private set; } = 4;

        /// <summary>
        /// Aumenta regeneração por mensagem. Cada ponto recupera 0.2 de vida e 0.1 de vigor.
        /// </summary>
        public int Vitalidade { get; private set; } = 4;

        /// <summary>
        /// Pontos de atributo livre para alocação.
        /// </summary>
        public int PontosLivreAtributo { get; set; } = 4;
    }
}
