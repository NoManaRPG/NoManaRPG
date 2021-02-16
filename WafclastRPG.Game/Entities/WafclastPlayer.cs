using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Game.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastPlayer
    {
        [BsonId]
        public ulong Id { get; private set; }
        public DateTime DateAccountCreation { get; private set; }
        public WafclastCharacter Character { get; private set; }

        public WafclastPlayer(ulong id)
        {
            this.Id = id;
            this.DateAccountCreation = DateTime.UtcNow;
            this.Character = new WafclastCharacter();
        }

        public WafclastPlayer(WafclastPlayer jogador)
        {
            this.Id = jogador.Id;
            this.DateAccountCreation = jogador.DateAccountCreation;
            this.Character = jogador.Character;
        }
    }
}
