// This file is part of the WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastCharacter
    {
        public WafclastStatePoints LifePoints { get; set; }
        public int LifePointsLevel { get; set; }
        public int LifePointsRank { get; set; }

        public double AttackPoints { get; set; }
        public int AttackPointsLevel { get; set; }
        public int AttackPointsRank { get; set; }

        public WafclastCharacter(bool conf)
        {
            this.LifePoints = new(1024);
            this.LifePointsLevel = 1;
            this.LifePointsRank = 0;

            this.AttackPoints = 128;
            this.AttackPointsLevel = 1;
            this.AttackPointsRank = 0;
        }
    }
}
