// This file is part of WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;

namespace NoManaRPG.Game.Entities
{
    [BsonIgnoreExtraElements]
    public class Character
    {
        // Change?
        public string Race { get; private set; }
        public StatePoints LifePoints { get; private set; }
        public StatePoints ManaPoints { get; private set; }


        //this will change 
        public string Job { get; private set; }


        //Skills
        //Available Jobs
        //Equips
        //Titles



        public Character() { }
    }
}
