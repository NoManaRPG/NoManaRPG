using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Enuns;
using System.Collections.Generic;
using System.Linq;

namespace DragonsDiscordRPG.BancoItens
{
    public static class RPBancoItens
    {
        public static IEnumerable<IGrouping<int, RPItem>> Itens;

        public static void Carregar()
        {
            var i = new List<RPItem>();

            #region Frascos de vida

            i.Add(new RPItem(RPTipo.PocaoVida, "Frasco de vida pequeno", 1, 2)
            {
                CargasUso = 7,
                CargasMax = 21,
                LifeRegen = 70,
                Tempo = 6,
            });
            i.Add(new RPItem(RPTipo.PocaoVida, "Frasco de vida médio", 3, 2)
            {
                LifeRegen = 150,
                Tempo = 6.5,
                CargasUso = 8,
                CargasMax = 28,
            });
            i.Add(new RPItem(RPTipo.PocaoVida, "Frasco de vida grande", 6, 2)
            {
                LifeRegen = 250,
                Tempo = 7,
                CargasUso = 9,
                CargasMax = 30,
            });

            #endregion Frascos de vida

            #region Arcos

            i.Add(new RPItem(RPTipo.Arco, "Arco bruto", 1, 6)
            {
                Destreza = 14,
                DanoFisico = new RPDano(5, 13),
                ChanceCritico = 0.05,
                VelocidadeAtaque = 1.4
            });
            i.Add(new RPItem(RPTipo.Arco, "Arco curto", 5, 6)
            {
                Destreza = 26,
                DanoFisico = new RPDano(6, 16),
                ChanceCritico = 0.05,
                VelocidadeAtaque = 1.5
            });

            #endregion Arcos

            Itens = i.GroupBy(x => x.Nivel);
        }
    }
}
