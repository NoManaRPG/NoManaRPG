using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using WafclastRPG.Game.Entidades.Itens;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Game.Entidades
{
    [BsonIgnoreExtraElements]
    public class WafclastPersonagem
    {
        public WafclastPontoRegenerativo Vida { get; private set; } = new WafclastPontoRegenerativo();
        public WafclastPontoRegenerativo Mana { get; private set; } = new WafclastPontoRegenerativo();

        #region Atributos
        public int Forca { get; private set; }
        public int Destreza { get; private set; }
        public int Inteligencia { get; private set; }
        #endregion

        #region Pontos
        public WafclastPonto Precisao { get; private set; } = new WafclastPonto();
        public WafclastPonto Evasao { get; private set; } = new WafclastPonto();
        public WafclastPonto Armadura { get; private set; } = new WafclastPonto();
        #endregion

        #region Dano físico
        public double DanoFisicoExtraPorcentagem { get; private set; } = 1;
        public WafclastDano DanoFisicoFinal { get; private set; }
        public WafclastDano DanoFisicoExtra { get; private set; }
        public WafclastDano DanoFisicoBase { get; private set; }
        public double DanoFisicoDesviarChance { get; private set; }
        public double DanoFisicoCriticoChance { get; private set; }
        public double DanoFisicoCriticoMultiplicador { get; private set; } = 1.5;
        #endregion

        #region Equipamentos equipados
        public WafclastItem MaoPrincipal { get; private set; }
        public WafclastItem MaoSecundaria { get; private set; }
        #endregion

        #region Outros
        public WafclastZona Zona { get; private set; }
        public WafclastClasse Classe { get; private set; }
        public WafclastNivel Nivel { get; private set; }
        public WafclastMochila Mochila { get; private set; }
        #endregion

        public WafclastPersonagem(WafclastClasse classe, WafclastDano dano,
            int forca, int destreza, int inteligencia)
        {
            this.Classe = classe;
            this.Forca = forca;
            this.Destreza = destreza;
            this.Inteligencia = inteligencia;



            DanoFisicoFinal = dano;
            DanoFisicoBase = dano;

            CalcVida();
            CalcMana();
            CalcEvasao();
            CalcPrecisao();

            Vida.Incrementar(double.MaxValue);
            Mana.Incrementar(double.MaxValue);
        }

        public double ReceberDanoFisico(double danoFisico)
        {
            double porcentagemReducao = Math.Clamp(Armadura.Calculado / (Armadura.Calculado + 10 * danoFisico), 0, 0.9) * danoFisico;
            double danoReduzido = danoFisico - porcentagemReducao;
            Vida.Diminuir(danoReduzido);
            return danoReduzido;
        }

        public void Resetar()
        {
            Zona = new WafclastZona();
            Vida.Incrementar(double.MaxValue);
            Mana.Incrementar(double.MaxValue);
            Nivel.PersonagemMorreu();
        }

        public void Equipar(WafclastItem item)
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
            DanoFisicoFinal.Minimo = (DanoFisicoBase.Minimo + DanoFisicoExtra.Minimo) * DanoFisicoExtraPorcentagem;
            DanoFisicoFinal.Maximo = (DanoFisicoBase.Maximo + DanoFisicoExtra.Maximo) * DanoFisicoExtraPorcentagem;
        }

        public void CalcVida()
        {
            Vida.WithMaximo((38 + (Nivel.Atual * 12) + (Forca / 2)) * Vida.PorcentagemAdicional);
        }

        public void CalcMana()
        {
            Mana.WithMaximo((40 + (Nivel.Atual * 6) + (Inteligencia / 2)) * Mana.PorcentagemAdicional);
            Mana.WithRegen(0.018 * Mana.Maximo);
        }

        public void CalcEvasao()
        {
            Evasao. = (53 + (Nivel.Atual * 3)) * ((Destreza / 5 * 0.01) + 1);
            Evasao.Final = (Evasao.Base + Evasao.Extra) * Evasao.PorcentagemMultiplicador;
        }

        public void CalcPrecisao()
        {
            Precisao.Base = (Destreza * 2) + ((Nivel.Atual - 1) * 2);
            Precisao.Final = (Precisao.Base + Precisao.Extra) * Precisao.PorcentagemMultiplicador;
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
                Vida.Incrementar(double.MaxValue);
                Mana.Incrementar(double.MaxValue);
            }
            return quantEvoluiu;
        }
    }
}
