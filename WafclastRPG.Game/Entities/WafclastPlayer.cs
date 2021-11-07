// This file is part of the WafclastRPG project.

using System;
using System.Globalization;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastPlayer : WafclastLevel
    {
        [BsonId]
        public ulong Id { get; private set; }
        public DateTime DateAccountCreation { get; private set; }
        public WafclastStatePoints Energy { get; private set; }
        public int Coins { get; private set; }
        public int Gens { get; private set; }
        public ulong MonstersKills { get; set; }
        public ulong Deaths { get; set; }
        public WafclastCharacter Character { get; private set; }


        public string Mention => $"<@{this.Id.ToString(CultureInfo.InvariantCulture)}>";
        public string Language { get; set; } = "pt-BR";

        public WafclastPlayer(ulong id)
        {
            this.Id = id;
            this.DateAccountCreation = DateTime.UtcNow;
            this.Energy = new WafclastStatePoints(50);
            this.Character = new WafclastCharacter(true);
        }

        public void CoinsAdd(int value) => this.Coins += value;
        public bool CoinsRemove(int value)
        {
            if (value > this.Coins)
                return false;
            this.Coins -= value;
            return true;
        }

        public void GensAdd(int value) => this.Gens += value;
        public bool GensRemove(int value)
        {
            if (value > this.Gens)
                return false;
            this.Gens -= value;
            return true;
        }
    }
}
