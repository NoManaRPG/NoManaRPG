using DragonsDiscordRPG.Enuns;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragonsDiscordRPG.Entidades
{
    public class RPEfeito
    {
        public Tipo Tipo { get; set; }
        public string Nome { get; set; }
        public double Duracao { get; set; }
        public double Quantidade { get; set; }

        public RPEfeito(Tipo tipo, string nome, double duracao, double quantidade)
        {
            Tipo = tipo;
            Nome = nome;
            Duracao = duracao;
            Quantidade = quantidade;
        }
    }
}
