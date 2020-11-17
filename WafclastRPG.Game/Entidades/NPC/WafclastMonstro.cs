using System;
using System.Collections.Generic;

namespace WafclastRPG.Game.Entidades.NPC
{
    public class WafclastMonstro
    {
        public int Nivel { get => (AtaqueNivel + DefesaNivel) / 2; }
        public string Nome { get; set; }
        public double Vida { get; set; }
        public double VidaMax { get; set; } // Calc de xp
        public int DanoMax { get; set; }
        public int AtaqueVelocidade { get; set; } = 0;
        public int AtaqueVelocidadeMax { get; set; }
        public int AtaqueNivel { get; set; }
        public int DefesaNivel { get; set; }
        public List<WafclastMonstroDrop> Drops { get; set; } = new List<WafclastMonstroDrop>();

        public WafclastMonstro(string nome, double vida, int danoMax, int ataqueVelocidade, int ataqueNivel, int defesaNivel)
        {
            this.Nome = nome;
            this.Vida = vida;
            this.VidaMax = vida;
            this.DanoMax = danoMax;
            this.AtaqueVelocidadeMax = ataqueVelocidade;
            this.AtaqueNivel = ataqueNivel;
            this.DefesaNivel = defesaNivel;
        }

        /// <summary>
        /// Retorna verdadeiro caso tenha abatido o monstro.
        /// </summary>
        /// <param name="valor"></param>
        public bool ReceberDano(double valor)
        {
            Vida -= valor;
            if (Vida <= 0)
                return true;
            return false;
        }

        public int GetPrecisao()
             => (int)Math.Truncate((0.0008 * Math.Pow(this.AtaqueNivel, 3)) + (4 * this.AtaqueNivel) + 40);

        public int GetDefesa()
            => (int)Math.Truncate(2.5 * ((0.0008 * Math.Pow(this.DefesaNivel, 3)) + (4 * this.DefesaNivel) + 40));
    }
}
