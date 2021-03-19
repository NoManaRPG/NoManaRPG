namespace WafclastRPG.Game.Entities
{
    public class WafclastMonsterAtributos
    {
        /// <summary>
        /// Aumenta o dano físico máximo em 3 por cada ponto.
        /// </summary>
        public int Forca { get; private set; }

        /// <summary>
        /// Aumenta a vida máxima em 8 por cada ponto.
        /// </summary>
        public int Resistencia { get; private set; }

        /// <summary>
        /// Aumenta a chance de desviar de um ataque inimigo.
        /// </summary>
        public int Agilidade { get; private set; }

        public WafclastMonsterAtributos(int forca, int resistencia, int agilidade)
        {
            Forca = forca;
            Resistencia = resistencia;
            Agilidade = agilidade;
        }

        public WafclastMonsterAtributos()
        {
        }
    }
}
