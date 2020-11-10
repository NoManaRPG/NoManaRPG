using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using WafclastRPG.Game.Entidades.Itens;
using WafclastRPG.Game.Enums;
using WafclastRPG.Game.Services;

namespace WafclastRPG.Game.Entidades
{
    [BsonIgnoreExtraElements]
    public class WafclastPersonagem
    {
        public WafclastPontoRegenerativo Vida { get; private set; } = new WafclastPontoRegenerativo();
        public WafclastPontoRegenerativo Mana { get; private set; } = new WafclastPontoRegenerativo();

        public WafclastPontoRegenerativo Vigor { get; private set; } = new WafclastPontoRegenerativo();
        public DateTime DataUltimoComando { get; set; } = DateTime.UtcNow;
        public WafclastPontoRegenerativo Fome { get; private set; } = new WafclastPontoRegenerativo();
        public WafclastPontoRegenerativo Sede { get; private set; } = new WafclastPontoRegenerativo();

        #region Atributos
        public int Forca { get; set; }
        public int Destreza { get; set; }
        public int Inteligencia { get; set; }
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
        public WafclastBatalha Zona { get; private set; } = new WafclastBatalha();
        public WafclastClasse Classe { get; private set; }
        public WafclastNivel Nivel { get; private set; } = new WafclastNivel();
        public WafclastMochila Mochila { get; private set; } = new WafclastMochila();
        public int IdRegiao { get; set; } = 1;
        public int Pontos { get; set; } = 0;
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
            Vigor.WithBase(100);
            Fome.WithBase(100);
            Sede.WithBase(100);

            Vida.Incrementar(double.MaxValue);
            Mana.Incrementar(double.MaxValue);
            Vigor.Incrementar(double.MaxValue);
            Fome.Incrementar(double.MaxValue);
            Sede.Incrementar(double.MaxValue);
        }

        public double GetVigor()
        {
            var tempoSegundos = DateTime.UtcNow - DataUltimoComando;
            var vigorRestaurado = tempoSegundos.TotalSeconds / 30;
            Vigor.Incrementar(vigorRestaurado);
            return Vigor.Atual;
        }

        public double CausarDanoFisico(double danoFisico)
        {
            double porcentagemReducao = Math.Clamp(Armadura.Calculado / (Armadura.Calculado + 10 * danoFisico), 0, 0.9) * danoFisico;
            double danoReduzido = danoFisico - porcentagemReducao;
            Vida.Diminuir(danoReduzido);
            return danoReduzido;
        }

        public void Resetar()
        {
            Vida.Incrementar(double.MaxValue);
            Mana.Incrementar(double.MaxValue);
            Nivel.Penalizar();
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

        public bool AddExp(double exp)
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
                Pontos += 5 * quantEvoluiu;
                return true;
            }
            return false;
        }

        public StringBuilder AtacarMonstro(out Resultado resultado, int ataque = 0)
        {
            resultado = Resultado.InimigoAbatido;
            var batalha = new StringBuilder();
            if (!DiminuirEstamina())
            {
                resultado = Resultado.SemVigor;
                batalha.AppendLine(":cold_sweat: **Você está sem vigor para explorar!**");
                return batalha;
            }
            if (ataque == 1)
                batalha.AppendLine($"**{Zona.SortearMonstro(IdRegiao, Nivel.Atual)} apareceu!**\n");
            if (Zona.Monstro == null)
                batalha.AppendLine($"**{Zona.SortearMonstro(IdRegiao, Nivel.Atual)} apareceu!**\n");
            Zona.Turno++;
            batalha.AppendLine($"Turno {Zona.Turno}");
            if (Zona.MonstroAtacar(this, batalha, out batalha))
            {
                Resetar();
                batalha.AppendLine($"**{Emoji.CrossBone} Você morreu!!! {Emoji.CrossBone}**");
                resultado = Resultado.PersonagemAbatido;
                return batalha;
            }

            // Chance acerto.
            Zona.Turno++;
            batalha.AppendLine($"\nTurno {Zona.Turno}");
            if (Calculo.DanoFisicoChanceAcerto(Precisao.Calculado, Zona.Monstro.Evasao))
            {
                var dp = DanoFisicoCalculado;
                var dano = Calculo.SortearValor(dp.Minimo, dp.Maximo);
                batalha.AppendLine($"{Emoji.Adaga} Você causou {dano:N2} de dano no {Zona.Monstro.Nome}!");
                // Monstro morto.
                if (Zona.Monstro.CausarDano(dano))
                {
                    batalha.AppendLine($"{Emoji.CrossBone} **{Zona.Monstro.Nome}** ️{Emoji.CrossBone}");
                    batalha.AppendLine($"**{Emoji.Exp}+{Zona.Monstro.Exp:N2}.**");
                    if (AddExp(Zona.Monstro.Exp))
                        resultado = Resultado.Evoluiu;

                    int quantMoedas = Nivel.Atual * 2;
                    batalha.AppendLine($"**{Emoji.Coins}+{quantMoedas}**");
                    Mochila.AdicionarMoeda(quantMoedas);
                    if (Calculo.Chance(0.5))
                    {
                        if (Mochila.TryAddItem(Zona.Monstro.ItemDrop))
                            batalha.AppendLine($"**:school_satchel:+{Zona.Monstro.ItemDrop.Nome}**");
                    }
                    Zona.Monstro = null;
                }
            }
            else
                batalha.AppendLine($"{Emoji.CarinhaDesapontado} Você errou o ataque!");
            return batalha;
        }

        public bool DiminuirEstamina()
        {
            _ = GetVigor();
            DataUltimoComando = DateTime.UtcNow;
            if (Vigor.Atual < 1)
                return false;
            Vigor.Diminuir(1);
            Fome.Diminuir(0.4);
            if (Fome.Atual == 0)
                Vida.Diminuir(2);
            Sede.Diminuir(0.8);
            if (Sede.Atual == 0)
                Vida.Diminuir(1);
            return true;
        }

        public static string VidaEmoji(double porcentagem)
        {
            switch (porcentagem)
            {
                case double n when (n > 0.75):
                    return Emoji.CoracaoVerde;
                case double n when (n > 0.50):
                    return Emoji.CoracaoAmarelo;
                case double n when (n > 0.25):
                    return Emoji.CoracaoLaranja;
            }
            return Emoji.CoracaoVermelho;
        }

        public static string ManaEmoji(double porcentagem)
        {
            switch (porcentagem)
            {
                case double n when (n > 0.75):
                    return Emoji.CirculoVerde;
                case double n when (n > 0.50):
                    return Emoji.CirculoAmarelo;
                case double n when (n > 0.25):
                    return Emoji.CirculoLaranja;
            }
            return Emoji.CirculoVermelho;
        }
    }

    public enum Resultado
    {
        InimigoAbatido,
        PersonagemAbatido,
        Evoluiu,
        SemVigor,
    }
}
