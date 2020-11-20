using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using WafclastRPG.Game.Entidades.Proficiencias;
using WafclastRPG.Game.Extensoes;
using WafclastRPG.Game.Enums;
using static WafclastRPG.Game.Enums.ProficienciaType;
using WafclastRPG.Game.Entidades.NPC;
using WafclastRPG.Game.Entidades.Itens;
using System;

namespace WafclastRPG.Game.Entidades
{
    public class WafclastPersonagem
    {
        /*
         * Nível total: Soma de todas as habilidades
         * Nível de combate: Soma de todas as habilidades de combate
         * Exp Total: Soma de todas as experiencias
         * Doador
         * porta-níqueis
         * Porta-Ferramentas: ferramenta / Tipo/item?
         * Missoes
         * Ponto de missoes
         * WafCoins: Ganha ao doar
         * Habilidades
         * Equipamentos
         * Mochila
         * RegiaoId
         */

        public int NivelTotal { get; set; }
        public int NivelCombate { get; set; }
        public int ExperienciaTotal { get; set; }
        public bool Doador { get; set; }
        public int PortaNiqueis { get; set; }
        public int MissaoPontos { get; set; }
        public int RegiaoId { get; set; } = 0;
        public int WafCoins { get; set; }
        public Dictionary<ProficienciaType, WafclastProficiencia> Habilidades { get; set; }
        public Dictionary<EquipamentoType, WafclastItem> Equipamentos { get; set; }
        public WafclastMochila Mochila { get; set; } = new WafclastMochila();
        public WafclastMonstro InimigoMonstro { get; set; }

        public WafclastPersonagem()
        {
            #region Popular habilidades
            Habilidades = new Dictionary<ProficienciaType, WafclastProficiencia>()
                .AddHab(new WafclastProficienciaConstituicao(), Constituicao)
                .AddHab(new WafclastProficienciaAtaque(), Ataque)
                .AddHab(new WafclastProficienciaDefesa(), Defesa)
                .AddHab(new WafclastProficienciaForca(), Forca);
            #endregion
        }

        public void CalcularNivelCombate()
        {
            var ataque = GetHabilidade(Ataque);
            var forca = GetHabilidade(Forca);
            var defesa = GetHabilidade(Defesa);
            var constituicao = GetHabilidade(Constituicao);
            NivelCombate = (int)Math.Truncate(((13.0 / 10.0) * ataque.Nivel + forca.Nivel) + defesa.Nivel + constituicao.Nivel);
        }

        public WafclastProficiencia GetHabilidade(ProficienciaType proficiencia) => Habilidades.GetValueOrDefault(proficiencia);

        public bool TryGetEquipamento(EquipamentoType equipamentoType, out WafclastItem item)
            => Equipamentos.TryGetValue(equipamentoType, out item);

        public bool TryEquiparItem(WafclastItem item)
        {
            switch (item)
            {
                case WafclastItemArma wia:
                    if (TryGetEquipamento(wia.Slot, out var _))
                        return false;
                    var habForca = (WafclastProficienciaForca)GetHabilidade(Forca);
                    var habAtaque = (WafclastProficienciaAtaque)GetHabilidade(Ataque);
                    wia.CalcularDanoArma();
                    wia.CalcularPrecisao();

                    habForca.DanoExtra += wia.DanoMax;
                    habAtaque.PrecisaoExtra += wia.Precisao;
                    Equipamentos.Add(wia.Slot, wia);
                    return true;
            }
            return false;
        }

        public bool TryDesequiparItem(EquipamentoType slot)
        {
            switch (slot)
            {
                case EquipamentoType.SegundaMao:
                case EquipamentoType.PrimeiraMao:
                    if (!TryGetEquipamento(slot, out var itemM))
                        return false;
                    var item = itemM as WafclastItemArma;
                    var habForca = (WafclastProficienciaForca)GetHabilidade(Forca);
                    var habAtaque = (WafclastProficienciaAtaque)GetHabilidade(Ataque);

                    habForca.DanoExtra -= item.DanoMax;
                    habAtaque.PrecisaoExtra -= item.Precisao;
                    Equipamentos.Remove(slot);
                    return Mochila.TryAddItem(itemM);
            }
            return false;
        }

        public bool ReceberDano(int valor)
        {
            var cont = GetHabilidade(Constituicao) as WafclastProficienciaConstituicao;
            cont.AddExperience(valor * 0.133);
            cont.Vida -= valor;
            if (cont.Vida <= 0)
                return true;
            return false;
        }

        public void Morrer()
        {
            Mochila.EspacoAtual = 0;
            Mochila.Itens = new List<WafclastMochila.Item>();
            InimigoMonstro = null;
            var hab = GetHabilidade(Constituicao) as WafclastProficienciaConstituicao;
            hab.AddVida(int.MaxValue);
        }
    }
}


