using System;
using System.Collections.Generic;
using System.Text;

namespace WafclastRPG.Game.Entities
{
    public class WafclastMonsterAtributos
    {
        /// <summary>
        /// Aumenta o dano físico máximo em 2 por cada ponto.
        /// </summary>
        public int Forca { get; private set; }
        /// <summary>
        /// Aumenta a vida máxima em 4 por cada ponto.
        /// </summary>
        public int Resistencia { get; private set; }
        /// <summary>
        /// Aumenta a velocidade de recuperação de vida por mensagem. Cada ponto recupera 0.2 de vida.
        /// </summary>
        public int Defesa { get; private set; }
    }
}
