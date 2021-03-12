using MongoDB.Bson.Serialization;

namespace WafclastRPG.Game.Entities
{
    public class WafclastCharacter : WafclastLevel
    {
        public WafclastCoins Coins { get; private set; } = new WafclastCoins(20);
        public WafclastCharacterAtributos Atributo { get; private set; } = new WafclastCharacterAtributos();
        public ulong LocalId { get; set; } = 0;
        public ulong ServerId { get; set; } = 0;
        public ulong LocalIDSpawn { get; set; } = 0;
        public ulong ServerIdSpawn { get; set; } = 0;
        public decimal Defesa { get; private set; } = 0;
        public decimal Ataque { get; private set; } = 0;

        public decimal VidaAtual { get; private set; } = 0;
        public decimal VidaMaxima { get; private set; } = 0;

        public int Karma { get; set; } = 0;

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

        public new bool ReceberExperiencia(decimal exp)
        {
            int levelUps = base.ReceberExperiencia(exp);
            for (int i = 0; i < levelUps; i++)
            {
                Level++;
                VidaAtual = VidaMaxima;
                if (Level > LevelBloqueado)
                    Atributo.PontosLivreAtributo += 4;
            }
            if (levelUps >= 1)
                return true;
            return false;
        }

        /// <summary>
        /// Retorna true caso tenha sido abatido.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public bool ReceberDano(decimal valor)
        {
            VidaAtual -= valor;
            if (VidaAtual <= 0)
            {
                VidaAtual = VidaMaxima;
                LocalId = LocalIDSpawn;
                Karma = 0;
                DiminuirLevel();
                return true;
            }
            return false;
        }

        public void ReceberVida(decimal valor)
        {
            VidaAtual += valor;
            if (VidaAtual >= VidaMaxima)
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
