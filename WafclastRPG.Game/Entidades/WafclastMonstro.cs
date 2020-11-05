using System;
using WafclastRPG.Game.Entidades.Itens;

namespace WafclastRPG.Game.Entidades
{
    public class WafclastMonstro
    {
        public string Nome { get; private set; }
        public int Nivel { get; private set; }
        public double Dano { get; private set; }
        public double Evasao { get; private set; }
        public double Precisao { get; private set; }
        public double Exp { get; private set; }
        public double Vida { get; private set; }

        public WafclastItem ItemDrop { get; private set; }

        public WafclastMonstro(string nome, WafclastItem drop)
        {
            this.Nome = nome;
            this.ItemDrop = drop;
        }

        public void CalcularAtributos()
        {
            this.Dano = CalcularDano();
            this.Evasao = CalcularEvasao();
            this.Precisao = CalcularPrecisao();
            this.Exp = CalcularExp();
            this.Vida = CalcularVida();
        }

        public void SetNivel(int nivel)
        {
            this.Nivel = nivel;
            CalcularAtributos();
        }

        /// <summary>
        /// Retorna verdadeiro caso tenha abatido o monstro.
        /// </summary>
        /// <param name="valor"></param>
        public bool CausarDano(double valor)
        {
            Vida -= valor;
            if (Vida <= 0)
                return true;
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


        //private RPBaseItem SortearItem(int nivel)
        //{
        //    // Separa os itens por nivel
        //    var niveisSeparados = RPMetadata.Itens.Where(x => x.Key <= nivel);

        //    Random r = new Random();
        //    var itens = niveisSeparados.ElementAt(r.Next(0, niveisSeparados.Count()));

        //    var itemSorteado = itens.ElementAt(r.Next(0, itens.Count()));
        //    itemSorteado.ILevel = nivel;
        //    return itemSorteado;
        //}

        //public bool SortearItens(int nivel, double chancePersonagem, out WafclastItem item)
        //{
        //    //itens = new List<RPBaseItem>();
        //    ////Add a raridade do monstro aqui tmb
        //    //double chance = (chancePersonagem + 0.16);
        //    //double integerPart = Math.Truncate(chance);
        //    //double fractionalPart = chance - Math.Truncate(chance);
        //    //while (integerPart >= 1)
        //    //{
        //    //    itens.Add(SortearItem(nivel));
        //    //    integerPart--;
        //    //}

        //    //if (Calculo.Chance(fractionalPart))
        //    //    itens.Add(SortearItem(nivel));

        //    //if (itens.Count == 0) return false;
        //    return true;
        //}

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
    }
}
