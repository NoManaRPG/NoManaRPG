using MongoDB.Bson.Serialization;
using System;
using System.Globalization;

namespace WafclastRPG.Entities
{
    public class WafclastPlayer
    {
        public ulong Id { get; private set; }
        public DateTime DateAccountCreation { get; private set; }
        public WafclastCharacter Character { get; private set; }

        public ulong MonsterKill { get; set; }
        public ulong PlayerKill { get; set; }
        public ulong Deaths { get; set; }

        public string Language { get; set; } = "pt-BR";

        public WafclastPlayer(ulong id, bool createCharacter = true)
        {
            Id = id;
            DateAccountCreation = DateTime.UtcNow;
            if (createCharacter)
                Character = new WafclastCharacter();
        }

        public void NewCharacter() => Character = new WafclastCharacter();

        public string Mention { get => $"<@{Id.ToString(CultureInfo.InvariantCulture)}>"; }

        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastPlayer>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdMember(c => c.Id);
            });
        }
    }
}
