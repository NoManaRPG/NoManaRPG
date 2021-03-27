using MongoDB.Bson.Serialization;
using System;
using System.Globalization;
using System.Threading.Tasks;
using WafclastRPG.DataBases;

namespace WafclastRPG.Entities
{
    public class WafclastPlayer
    {
        public DatabaseSession banco;

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

        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastPlayer>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdMember(c => c.Id);
                cm.UnmapMember(C => C.banco);
            });
        }

        public Task SaveAsync() => this.banco.ReplacePlayerAsync(this);
        public string Mention()
            => $"<@{Id.ToString(CultureInfo.InvariantCulture)}>";
    }
}
