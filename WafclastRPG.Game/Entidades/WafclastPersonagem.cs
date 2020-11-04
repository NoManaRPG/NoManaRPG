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
        [BsonIgnore]
        public WafclastDano DanoFisicoCalculado
        {
            get
            {
                Random rng = new Random();
                WafclastDano dano = new WafclastDano(DanoFisicoBase);
                dano.Minimo *= DanoFisicoExtraPorcentagem;
                dano.Maximo *= DanoFisicoExtraPorcentagem;
                dano.Somar(DanoFisicoExtra);
                //rng.NextDouble() * (dano.Maximo - dano.Minimo) + dano.Minimo
                return dano;
            }
        }
        public double DanoFisicoExtraPorcentagem { get; private set; } = 1;
        public WafclastDano DanoFisicoExtra { get; private set; } = new WafclastDano();
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
        public WafclastZona Zona { get; private set; } = new WafclastZona();
        public WafclastClasse Classe { get; private set; }
        public WafclastNivel Nivel { get; private set; } = new WafclastNivel();
        public WafclastMochila Mochila { get; private set; } = new WafclastMochila();
        #endregion

        public WafclastPersonagem(WafclastClasse classe, WafclastDano dano,
            int forca, int destreza, int inteligencia)
        {
            this.Classe = classe;
            this.Forca = forca;
            this.Destreza = destreza;
            this.Inteligencia = inteligencia;

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
                case WafclastItemArma arma:
                    DanoFisicoExtra.Somar(arma.DanoFisicoCalculado);
                    break;
            }
        }

        public void CalcVida()
        {
            Vida.WithBase(38 + (Nivel.Atual * 12) + (Forca / 2));
        }

        public void CalcMana()
        {
            Mana.WithBase(40 + (Nivel.Atual * 6) + (Inteligencia / 2));
            Mana.WithRegen(0.018 * Mana.Maximo);
        }

        public void CalcEvasao()
        {
            Evasao.WithBase((53 + (Nivel.Atual * 3)) * ((Destreza / 5 * 0.01) + 1));
        }

        public void CalcPrecisao()
        {
            Precisao.WithBase((Destreza * 2) + ((Nivel.Atual - 1) * 2));
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
