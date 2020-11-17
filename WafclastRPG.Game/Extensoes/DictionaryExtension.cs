using System.Collections.Generic;
using WafclastRPG.Game.Entidades.Proficiencias;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Game.Extensoes
{
    public static class DictionaryExtension
    {
        public static Dictionary<ProficienciaType, WafclastProficiencia> AddHab(this Dictionary<ProficienciaType, WafclastProficiencia> dictionary, WafclastProficiencia habilidade, ProficienciaType prof)
        {
            dictionary.Add(prof, habilidade);
            return dictionary;
        }
    }
}
