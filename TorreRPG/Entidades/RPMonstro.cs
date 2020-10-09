using TorreRPG.BancoItens;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using TorreRPG.Services;

namespace TorreRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPMonstro
    {
        public string Nome { get; set; }
        public int Nivel { get; set; }
        public double Dano { get; set; }
        public double Evasao { get; set; }
        public double Precisao { get; set; }
        public double PontosAacao { get; set; }
        public double Exp { get; set; }
        public double Vida { get; set; }

        public double VelocidadeAtaque { get; set; }
        public RPRaridade Tipo { get; set; }

        public RPMonstro(string nome, int nivel)
        {
            Nome = nome;
            Nivel = nivel;
            Dano = CalcularDano();
            Evasao = CalcularEvasao();
            Precisao = CalcularPrecisao();
            PontosAacao = 0;
            Exp = CalcularExp();
            Vida = CalcularVida();
            VelocidadeAtaque = 1;
        }

        public bool Acao(double pontosAcaoTotal)
        {
            PontosAacao += VelocidadeAtaque;
            if (PontosAacao >= pontosAcaoTotal)
            {
                PontosAacao = 0;
                return true;
            }
            return false;
        }

        public double CalcularExp()
        {
            return ((Nivel / 20) + 0.40) * (Nivel + 20);
        }

        public double CalcularDano()
        {
            return 0.0015 * Math.Pow(Nivel, 3) + 0.2 * Nivel + 6;
        }

        public double CalcularPrecisao()
        {
            return 0.0015 * Math.Pow(Nivel, 3) + 0.2 * Nivel + 15;
        }

        public double CalcularEvasao()
        {
            return 0.0015 * Math.Pow(Nivel, 4) + 0.2 * Nivel + 53;
        }

        public double CalcularVida()
        {
            return 0.0015 * Math.Pow(Nivel, 6) + 0.2 * Nivel + 15;
        }


        private RPBaseItem SortearItem(int nivel)
        {
            // Separa os itens por nivel
            var niveisSeparados = RPMetadata.Itens.Where(x => x.Key <= nivel);

            Random r = new Random();
            var itens = niveisSeparados.ElementAt(r.Next(0, niveisSeparados.Count()));

            var itemSorteado = itens.ElementAt(r.Next(0, itens.Count()));
            itemSorteado.ILevel = nivel;
            return itemSorteado;
        }

        public bool SortearItens(int nivel, double chancePersonagem, out List<RPBaseItem> itens)
        {
            itens = new List<RPBaseItem>();
            //Add a raridade do monstro aqui tmb
            double chance = (chancePersonagem + 0.16);
            double integerPart = Math.Truncate(chance);
            double fractionalPart = chance - Math.Truncate(chance);
            while (integerPart >= 1)
            {
                itens.Add(SortearItem(nivel));
                integerPart--;
            }

            if (Calculo.Chance(fractionalPart))
                itens.Add(SortearItem(nivel));

            if (itens.Count == 0) return false;
            return true;
        }

        //    //Pode dropar equipamento ou dinheiro
        //    //Se for equipamento pode dropar um dos varios tipos
        //    //Se for dinheiro pode dropar um dos varios tipos

        //    public int ChanceDropTotal { get; set; }
        //    public List<MobItemDropRPG> Drops { get; set; }

        //    public MobItemDropRPG SortearDrop()
        //    {
        //        var rand = Calculo.SortearValor(0, ChanceDropTotal);
        //        var top = 0;
        //        for (int i = 0; i < Drops.Count; i++)
        //        {
        //            top += Drops[i].ChanceDrop;
        //            if (rand <= top)
        //                return Drops[i];
        //        }
        //        return null;
        //    }
        //}

        //[BsonIgnoreExtraElements]
        //public class MobItemDropRPG
        //{
        //    public int ChanceDrop { get; set; }
        //}
    }
}
