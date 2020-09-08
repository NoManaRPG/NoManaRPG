using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragonsDiscordRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPPontoRegenerativo
    {
        public double Atual { get; set; }
        public double Maximo { get; set; }
        public double PorcentagemAdicional { get; set; }
        public double RegenPorSegundo { get; set; }

        public RPPontoRegenerativo()
        {
            PorcentagemAdicional = 1;
        }

        public RPPontoRegenerativo(double atual, double maximo) : this()
        {
            Maximo = maximo;
            Atual = atual;
        }

        public void Diminuir(double valor)
        {
            if (Atual < valor)
            {
                Atual = 0;
                return;
            }
            Atual -= valor;
        }

        public void Adicionar(double valor)
        {
            Atual += valor;
            if (Atual > Maximo) Atual = Maximo;
        }
    }
}
