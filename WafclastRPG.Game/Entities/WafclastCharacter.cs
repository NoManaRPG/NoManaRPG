// This file is part of WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastCharacter
    {
        // Change?
        public string Race { get; private set; }
        public WafclastStatePoints LifePoints { get; private set; }
        public WafclastStatePoints ManaPoints { get; private set; }


        //this will change 
        public string Job { get; private set; }


        //Skills
        //Available Jobs
        //Equips
        //Titles



        public WafclastCharacter() { }
    }
}
