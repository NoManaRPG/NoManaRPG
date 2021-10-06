// This file is part of the WafclastRPG project.

using System.Collections.ObjectModel;
using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Entities.Skills.Effects;

namespace WafclastRPG.Game.Entities.Skills
{
    [BsonIgnoreExtraElements]
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(WafclastPlayerSkill))]
    public class WafclastBaseSkill : WafclastLevel
    {
        public string Name { get; set; }

        public Collection<WafclastSkillBaseEffect> Effects { get; set; }

        public WafclastBaseSkill(string name)
        {
            this.Name = name;
            this.Effects = new();
        }
    }
}
