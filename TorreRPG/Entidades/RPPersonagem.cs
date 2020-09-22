using TorreRPG.Entidades.Itens;
using TorreRPG.Extensoes;
using DSharpPlus.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TorreRPG.Enuns;

namespace TorreRPG.Entidades
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

            DanoFisicoModificado = danoBase;
            DanoFisicoBase = danoBase;
            DanoFisicoPorcentagem = 1;
            DanoFisicoExtra = new RPDano(0, 0);

            Efeitos = new List<RPEfeito>();

            Zona = new RPZona();

            CalcVida();
            CalcMana();
            CalcEvasao();
            CalcPrecisao();

            Vida.Adicionar(double.MaxValue);
            Mana.Adicionar(double.MaxValue);

            Frascos = new List<RPBaseFrasco>();
            Mochila = new RPMochila();
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

        public double PorcentagemCritico { get; set; } = 0;
        public double PorcentagemCriticoDano { get; set; } = 1.5;

        /// <summary>
        /// Dano fisico final, após toda as somas.
        /// </summary>
        public RPDano DanoFisicoModificado { get; set; }
        /// <summary>
        /// Dano físico extra, como de uma arma.
        /// </summary>
        public RPDano DanoFisicoExtra { get; set; }
        /// <summary>
        /// Porcetagem que será multiplicada no dano final.
        /// </summary>
        public double DanoFisicoPorcentagem { get; set; }
        /// <summary>
        /// Dano físico base, não alterar.
        /// </summary>
        public RPDano DanoFisicoBase { get; set; }

        public RPBaseItem MaoPrincipal { get; set; }
        public RPBaseItem MaoSecundaria { get; set; }

        public List<RPEfeito> Efeitos { get; set; }

        public RPZona Zona { get; set; }
        public int ZonasDescoberta { get; set; }

        public List<RPBaseFrasco> Frascos { get; set; }

        public double ChanceDrop { get; set; }

        public RPMochila Mochila { get; set; }


        public double ReceberDanoFisico(double danoFisico)
        {
            double porcentagemReducao = Math.Clamp(Armadura.Modificado / (Armadura.Modificado + 10 * danoFisico), 0, 0.9) * danoFisico;
            double danoReduzido = danoFisico - porcentagemReducao;
            Vida.Diminuir(danoReduzido);
            return danoReduzido;
        }

        public bool Acao(double pontosAcaoTotal)
        {
            PontosDeAcao += VelocidadeAtaque.Modificado;
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
            Efeitos = new List<RPEfeito>();
            Vida.Adicionar(double.MaxValue);
            Mana.Adicionar(double.MaxValue);
            Nivel.PersonagemMorreu();
            foreach (var frasco in Frascos)
                frasco.ResetarCargas();
        }

        public void Equipar(RPBaseItem item)
        {
            switch (item)
            {
                case RPBaseItemArma arma:
                    DanoFisicoExtra.Minimo += arma.DanoFisicoModificado.Minimo;
                    DanoFisicoExtra.Maximo += arma.DanoFisicoModificado.Maximo;
                    break;
            }

            CalcDano();
        }

        public void CalcDano()
        {
            DanoFisicoModificado.Minimo = (DanoFisicoBase.Minimo + DanoFisicoExtra.Minimo) * DanoFisicoPorcentagem;
            DanoFisicoModificado.Maximo = (DanoFisicoBase.Maximo + DanoFisicoExtra.Maximo) * DanoFisicoPorcentagem;
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
            Evasao.Base = (53 + (Nivel.Atual * 3)) * ((Atributos.Destreza / 5 * 0.01) + 1);
            Evasao.Modificado = (Evasao.Base + Evasao.Extra) * Evasao.PorcentagemAdicional;
        }

        public void CalcPrecisao()
        {
            Precisao.Base = (Atributos.Destreza * 2) + ((Nivel.Atual - 1) * 2);
            Precisao.Modificado = (Precisao.Base + Precisao.Extra) * Precisao.PorcentagemAdicional;
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
                    case Enuns.RPClasse.Frasco:
                        if (regen) continue;
                        regen = true;
                        Vida.Adicionar(Efeitos[i].Quantidade);
                        ataquesInimigos.AppendLine($"{Efeitos[i].Quantidade.Text()} de vida regenerado.");
                        if (Efeitos[i].Usar())
                        {
                            Efeitos.RemoveAt(i);
                            ataquesInimigos.AppendLine($"Regen de vida acabou.".Bold());
                        }
                        break;
                }
            }
        }
    }
}


