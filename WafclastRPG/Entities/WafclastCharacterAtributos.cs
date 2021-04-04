namespace WafclastRPG.Entities
{
    public class WafclastCharacterAtributos
    {
        /// <summary>
        /// Aumenta o ataque máximo em 3 por cada ponto atribuído.
        /// </summary>
        public int Forca { get; set; } = 4;

        /// <summary>
        /// Aumenta a vida máxima em 8 e o vigor em 4 por cada ponto atribuído.
        /// </summary>
        public int Resistencia { get; set; } = 4;

        /// <summary>
        /// Aumenta a chance de desviar de um ataque inimigo.
        /// </summary>
        public int Agilidade { get; set; } = 4;

        /// <summary>
        /// Aumenta regeneração por mensagem. Cada ponto recupera 0.2 de vida e 0.1 de vigor.
        /// </summary>
        public int Vitalidade { get; set; } = 4;

        /// <summary>
        /// Pontos de atributo livre para alocação.
        /// </summary>
        public int PontosLivreAtributo { get; set; } = 4;
    }
}
