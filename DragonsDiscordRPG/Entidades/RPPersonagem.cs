using System;
using System.Collections.Generic;
using System.Text;

namespace DragonsDiscordRPG.Entidades
{
    public class RPPersonagem
    {
        public RPPersonagem(string nome, string classe, RPAtributo atributos, RPDano danoBase)
        {
            Nome = nome;
            Classe = classe;
            Nivel = new RPNivel();
            Atributos = atributos;

            Vida = new RPPontoRegenerativo();
            Mana = new RPPontoRegenerativo();

            Precisao = new RPPontoEstatico();
            Evasao = new RPPontoEstatico();
            Armadura = new RPPontoEstatico();
            VelocidadeAtaque = new RPPontoEstatico(1.2);

            DanoFisico = danoBase;
            MaoPrincipal = new RPItem();
            MaoSecundaria = new RPItem();

            Efeitos = new List<RPEfeito>();

            Zona = new RPZona();

            CalcVida();
            CalcMana();
            CalcEvasao();
            CalcPrecisao();
        }

        public string Nome { get; set; }
        public string Classe { get; set; }
        public RPNivel Nivel { get; set; }
        public RPAtributo Atributos { get; set; }

        public RPPontoRegenerativo Vida { get; private set; }
        public RPPontoRegenerativo Mana { get; private set; }

        public RPPontoEstatico Precisao { get; set; }
        public RPPontoEstatico Evasao { get; set; }
        public RPPontoEstatico Armadura { get; set; }
        public RPPontoEstatico VelocidadeAtaque { get; set; } // Ganha de itens e habilidades. Base sempre será 1.2

        public double PontosDeAcao { get; set; } // Usado no ataque

        public double PorcentagemDesviar { get; set; }

        public RPDano DanoFisico { get; set; }

        public RPItem MaoPrincipal { get; set; }
        public RPItem MaoSecundaria { get; set; }

        public List<RPEfeito> Efeitos { get; set; }

        public RPZona Zona { get; set; }
        public int ZonasDescoberta { get; set; }


        public void CalcVida()
        {
            Vida.Maximo = (38 + (Nivel.Atual * 12) + (Atributos.Forca / 2)) * Vida.PorcentagemAdicional;
        }

        public void CalcMana()
        {
            Mana.Maximo = (40 + (Nivel.Atual * 6) + (Atributos.Inteligencia / 2)) * Mana.PorcentagemAdicional;
            Mana.RegenPorSegundo = 0.018 * Mana.Maximo;
        }

        public void CalcEvasao()
        {
            Evasao.Atual = (53 + (Nivel.Atual * 3)) * (((Atributos.Destreza / 5) * 0.01) + 1) * Evasao.PorcentagemAdicional;
        }

        public void CalcPrecisao()
        {
            Precisao.Atual = ((Atributos.Destreza * 2) + ((Nivel.Atual - 1) * 2)) * Precisao.PorcentagemAdicional;
        }

        public int AddExp(double exp)
        {
            int quantEvoluiu = Nivel.AddExp(exp);
            if (quantEvoluiu != 0)
            {
                CalcVida();
                CalcMana();
                CalcEvasao();
                CalcPrecisao();
                Vida.Adicionar(double.MaxValue);
                Mana.Adicionar(double.MaxValue);
            }
            return quantEvoluiu;
        }
    }
}


//[BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]