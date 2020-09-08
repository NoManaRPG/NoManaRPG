using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DragonsDiscordRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPZona
    {

        public int Nivel { get; set; }
        public int OndaTotal { get; set; }
        public int OndaAtual { get; set; }
        public int Turno { get; set; } // Reseta em outra onda
        public double PontosDeAcaoTotal { get; set; }
        public List<RPItem> ItensNoChao { get; set; }
        public List<Monstro> Monstros { get; set; }

        public RPZona()
        {
            Nivel = 0;
        }

        public int TrocarZona(double velocidadeAtaquePersonagem, int nivel)
        {
            Monstros = new List<Monstro>();
            ItensNoChao = new List<RPItem>();
            Turno = 0;
            Nivel = nivel;
            OndaAtual = 1;
            OndaTotal = 3 * Nivel;
            int quantidadeInimigo = Calculo.SortearValor(1, 4);
            for (int i = 0; i < quantidadeInimigo; i++)
            {
                var f = ModuloBanco.MonstrosNomes[Nivel];
                var sorteio = Calculo.SortearValor(0, f.Nomes.Count - 1);
                var g = f.Nomes[sorteio];
                Monstro m = new Monstro(g, nivel);
                Monstros.Add(m);
            }

            foreach (var item in Monstros)
                PontosDeAcaoTotal += item.VelocidadeAtaque;
            PontosDeAcaoTotal += velocidadeAtaquePersonagem;
            return quantidadeInimigo;
        }

        public void SortearItem()
        {

        }

        public int NovaOnda(double velocidadeAtaquePersonagem)
        {
            if (Monstros.Count == 0)
            {
                // Sorteia o chefao , vai sortear só 1
                //    if (OndaAtual + ModuloBanco.MonstrosNomes[Nivel].Chefoes.Count == OndaTotal)
                //    {

                //        return 0;
                //    }
                // 3 é maior que 3? não
                if (OndaAtual < OndaTotal)
                {
                    Turno = 0;
                    Monstros = new List<Monstro>();
                    OndaAtual++;

                    int quantidadeInimigo = Calculo.SortearValor(1, 3);
                    for (int i = 0; i < quantidadeInimigo; i++)
                    {
                        var f = ModuloBanco.MonstrosNomes[Nivel];
                        var sorteio = Calculo.SortearValor(0, f.Nomes.Count - 1);
                        var g = f.Nomes[sorteio];
                        Monstro m = new Monstro(g, Nivel);
                        Monstros.Add(m);
                    }

                    foreach (var item in Monstros)
                        PontosDeAcaoTotal += item.VelocidadeAtaque;
                    PontosDeAcaoTotal += velocidadeAtaquePersonagem;
                    return quantidadeInimigo;
                }

                Monstros = null;
                return 0;
            }
            return 0;
        }
    }
}
