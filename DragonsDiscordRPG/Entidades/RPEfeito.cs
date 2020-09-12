using DragonsDiscordRPG.Enuns;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragonsDiscordRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPEfeito
    {
        public Enuns.RPItemTipo Tipo { get; set; }
        public string Nome { get; set; }
        public double Duracao { get; set; }
        public double DuracaoSubtrair { get; set; }
        public double Quantidade { get; set; }

        public RPEfeito(Enuns.RPItemTipo tipo, string nome, double duracao, double quantidade, double duracaoSubtrair)
        {
            Tipo = tipo;
            Nome = nome;
            Duracao = duracao;
            Quantidade = quantidade;
            DuracaoSubtrair = duracaoSubtrair;
        }

        public bool Usar()
        {
            Duracao -= DuracaoSubtrair;
            if (Duracao <= 0) return true;
            return false;
        }
    }
}
