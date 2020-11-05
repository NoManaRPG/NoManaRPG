using System;
using System.Text;
using WafclastRPG.Game.Metadata;
using WafclastRPG.Game.Services;

namespace WafclastRPG.Game.Entidades
{
    public class WafclastZona
    {
        public int Turno { get; set; }
        public WafclastMonstro Monstro { get; set; }

        public WafclastZona() { }

        /// <summary>
        /// Retorna verdadeiro caso tenha abatido o personagem.
        /// </summary>
        /// <param name="personagem"></param>
        /// <param name="batalha"></param>
        public bool MonstroAtacar(WafclastPersonagem personagem, out StringBuilder batalha)
        {
            batalha = new StringBuilder();
            Turno++;
            if (Calculo.DanoFisicoChanceAcerto(Monstro.Precisao, personagem.Evasao.Calculado))
            {
                double dano = personagem.CausarDanoFisico(Monstro.Dano);
                batalha.AppendLine($"{Emoji.Escudo} {Monstro.Nome} causou {dano:N2} de dano físico.");
                if (personagem.Vida.Atual <= 0)
                    return true;
            }
            else
                batalha.AppendLine($"{Emoji.CarinhaNervoso} {Monstro.Nome} errou o ataque!");
            return false;
        }

        public void SortearMonstro(int nivel)
        {
            Random random = new Random();
            var sorteado = random.Next(0, Data.Monstros.Count - 1);
            Monstro = Data.Monstros[sorteado];
            Monstro.SetNivel(nivel);
        }
    }
}
