using DragonsDiscordRPG.Enuns;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DragonsDiscordRPG.Entidades
{
    public class Monstro
    {
        public string Nome { get; set; }
        public int Nivel { get; set; }
        public double Dano { get; set; }
        public double Evasao { get; set; }
        public double Precisao { get; set; }
        public double Acao { get; set; }
        public double Exp { get; set; }
        public double Vida { get; set; }

        public double VelocidadeAtaque { get; set; }
        public Raridade Tipo { get; set; }

        public Monstro(string nome, int nivel)
        {
            Nome = nome;
            Nivel = nivel;
            Dano = CalcularDano();
            Evasao = CalcularEvasao();
            Precisao = CalcularPrecisao();
            Acao = 0;
            Exp = CalcularExp();
            Vida = CalcularVida();
            VelocidadeAtaque = 1;
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

        //Pode dropar equipamento ou dinheiro
        //Se for equipamento pode dropar um dos varios tipos
        //Se for dinheiro pode dropar um dos varios tipos

        public int ChanceDropTotal { get; set; }
        public List<MobItemDropRPG> Drops { get; set; }

        public MobItemDropRPG SortearDrop()
        {
            var rand = Calculo.SortearValor(0, ChanceDropTotal);
            var top = 0;
            for (int i = 0; i < Drops.Count; i++)
            {
                top += Drops[i].ChanceDrop;
                if (rand <= top)
                    return Drops[i];
            }
            return null;
        }
    }

    [BsonIgnoreExtraElements]
    public class MobItemDropRPG
    {
        public int ChanceDrop { get; set; }
    }
}
