using MongoDB.Bson.Serialization;

namespace WafclastRPG.Game.Entities
{
    public class WafclastCharacter : WafclastLevel
    {
        public WafclastCoins Coins { get; private set; } = new WafclastCoins(20);
        public WafclastCharacterAtributos Atributo { get; private set; } = new WafclastCharacterAtributos();
        public ulong LocalId { get; set; } = 0;
        public decimal Defesa { get; private set; } = 0;
        public decimal Ataque { get; private set; } = 0;

        public decimal VidaAtual { get; private set; } = 0;
        public decimal VidaMaxima { get; private set; } = 0;

        public WafclastCharacter()
        {
            CalcStats();
            VidaAtual = VidaMaxima;
        }

        public void CalcStats()
        {
            Ataque = Atributo.Forca * 2;
            VidaMaxima = Atributo.Resistencia * 4;
        }

        public void AddLevel()
        {
            Level++;
            VidaAtual = VidaMaxima;
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
