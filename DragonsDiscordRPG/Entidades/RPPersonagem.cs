using DragonsDiscordRPG.Extensoes;
using DSharpPlus.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragonsDiscordRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPPersonagem
    {
        public RPPersonagem(string classe, string nome, RPAtributo atributos, RPDano danoBase)
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

            Efeitos = new List<RPEfeito>();

            Zona = new RPZona();

            CalcVida();
            CalcMana();
            CalcEvasao();
            CalcPrecisao();

            Vida.Adicionar(double.MaxValue);
            Mana.Adicionar(double.MaxValue);

            Pocoes = new List<RPItem>();
            Pocoes.Add(Itens.Pocoes.RPPocoes.PocoesVida[0]);
            Pocoes[0].AddCarga(50);
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

        public List<RPItem> Pocoes { get; set; }


        public double ReceberDanoFisico(double danoFisico)
        {
            double porcentagemReducao = Math.Clamp(Armadura.Atual / (Armadura.Atual + 10 * danoFisico), 0, 0.9) * danoFisico;
            double danoReduzido = danoFisico - porcentagemReducao;
            Vida.Diminuir(danoReduzido);
            return danoReduzido;
        }

        public bool Acao(double pontosAcaoTotal)
        {
            PontosDeAcao += VelocidadeAtaque.Atual;
            if (PontosDeAcao >= pontosAcaoTotal)
            {
                PontosDeAcao = 0;
                return true;
            }
            return false;
        }

        public void Resetar()
        {
            Zona = new RPZona();
            Vida.Adicionar(double.MaxValue);
            Mana.Adicionar(double.MaxValue);
            foreach (var item in Pocoes)
                item.AddCarga(double.MaxValue);
            Efeitos = new List<RPEfeito>();
            Nivel.PersonagemMorreu();
        }

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

        public void CalcEfeitos(StringBuilder ataquesInimigos)
        {
            // Os efeitos só podem ser ativados 1 vez.
            bool regen = false;

            // For ao contrario para poder remover o efeito sem dar erro.
            for (int i = Efeitos.Count - 1; i >= 0; i--)
            {
                switch (Efeitos[i].Tipo)
                {
                    // Efeito poção de vida.
                    case Enuns.RPTipo.PocaoVida:
                        if (regen) continue;
                        Vida.Adicionar(Efeitos[i].Quantidade);
                        ataquesInimigos.AppendLine($"{Efeitos[i].Quantidade} de vida regenerado.");
                        if (Efeitos[i].Usar())
                        {
                            Efeitos.RemoveAt(i);
                            ataquesInimigos.AppendLine($"Regen de vida acabou.");
                        }
                        break;
                }
            }
        }
    }
}


//[BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]