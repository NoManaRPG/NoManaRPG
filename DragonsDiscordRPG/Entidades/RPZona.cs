using DragonsDiscordRPG.Extensoes;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragonsDiscordRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPZona
    {

        public int Nivel { get; set; }
        public int OndaTotal { get; set; }
        public int OndaAtual { get; set; }
        public int Turno { get; set; } // Reseta em outra onda
        public double PontosAcaoTotal { get; set; }
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
                PontosAcaoTotal += item.VelocidadeAtaque;
            PontosAcaoTotal += velocidadeAtaquePersonagem;
            return quantidadeInimigo;
        }

        public void SortearItem()
        {

        }

        public int NovaOnda(double velocidadeAtaquePersonagem)
        {
            if (Monstros.Count == 0)
            {
                if (OndaAtual < OndaTotal)
                {
                    Turno = 0;
                    Monstros = new List<Monstro>();
                    OndaAtual++;

                    int quantidadeInimigo = Calculo.SortearValor(1, 2);
                    for (int i = 0; i < quantidadeInimigo; i++)
                    {
                        var listaNomes = ModuloBanco.MonstrosNomes[Nivel];
                        var nomeSorteado = listaNomes.Nomes[Calculo.SortearValor(0, listaNomes.Nomes.Count - 1)];
                        Monstro m = new Monstro(nomeSorteado, Nivel);
                        Monstros.Add(m);
                    }

                    //Calcula pontos de ação total.
                    foreach (var item in Monstros)
                        PontosAcaoTotal += item.VelocidadeAtaque;
                    PontosAcaoTotal += velocidadeAtaquePersonagem;
                    return quantidadeInimigo;
                }

                Monstros = null;
            }
            return 0;
        }

        public StringBuilder CalcAtaquesInimigos(RPPersonagem personagem)
        {
            StringBuilder resumoBatalha = new StringBuilder();
            do
            {
                foreach (var mob in Monstros)
                {
                    if (mob.Acao(PontosAcaoTotal))
                    {
                        Turno++;
                        if (Calculo.DanoFisicoChanceAcerto(mob.Precisao, personagem.Evasao.Atual))
                        {
                            double dano = personagem.ReceberDanoFisico(mob.Dano);
                            resumoBatalha.AppendLine($"{mob.Nome} causou {dano.Text()} de dano físico.");
                        }
                        else
                            resumoBatalha.AppendLine($"{mob.Nome} errou o ataque!");
                    }
                }


                if (personagem.Acao(PontosAcaoTotal))
                {
                    Turno++;
                    break;
                }
            } while (personagem.Vida.Atual > 0);
            return resumoBatalha;
        }
    }
}
