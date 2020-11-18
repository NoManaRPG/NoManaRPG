using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using WafclastRPG.Game.Entidades.Proficiencias;
using WafclastRPG.Game.Extensoes;
using WafclastRPG.Game.Enums;
using static WafclastRPG.Game.Enums.ProficienciaType;
using MongoDB.Bson.Serialization.Options;
using WafclastRPG.Game.Entidades.NPC;
using WafclastRPG.Game.Entidades.Itens;
using System;

namespace WafclastRPG.Game.Entidades
{
    [BsonIgnoreExtraElements]
    public class WafclastPersonagem
    {
        public int RegiaoId { get; set; } = 0;

        public WafclastMochila Mochila { get; set; } = new WafclastMochila();
        public WafclastMonstro InimigoMonstro { get; set; }
        public ulong Moedas { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)] public Dictionary<ProficienciaType, WafclastProficiencia> Habilidades { get; private set; }
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)] public Dictionary<EquipamentoType, WafclastItem> Equipamentos { get; private set; } = new Dictionary<EquipamentoType, WafclastItem>();

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


