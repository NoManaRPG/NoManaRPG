using MongoDB.Bson.Serialization;

namespace WafclastRPG.Game.Entities
{
    public class WafclastCharacter : WafclastLevel
    {
        public WafclastCoins Coins { get; private set; } = new WafclastCoins(20);
        public WafclastCharacterAtributos Atributo { get; private set; } = new WafclastCharacterAtributos();
        public ulong LocalId { get; private set; } = 0;
        public decimal Defesa { get; private set; } = 0;
        public decimal Ataque { get; private set; } = 0;
        public decimal Vida { get; private set; } = 0;

        public WafclastCharacter()
        {
            CalcStats();
        }

        public void CalcStats()
        {
            Ataque = Atributo.Forca * 2;
            Vida = Atributo.Resistencia * 4;
        }

        public void AddLevel()
        {
            Level++;
        }

        public void RemoveLevel()
        {
            Level--;
        }

        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastCharacter>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}
