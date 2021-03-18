using MongoDB.Bson.Serialization;
using System;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Game.Entities
{
    public class WafclastCharacter : WafclastLevel
    {
        public WafclastCoins Coins { get; private set; } = new WafclastCoins(20);
        public WafclastCharacterAtributos Atributo { get; private set; } = new WafclastCharacterAtributos();
        public WafclastLocalization Localization { get; set; } = new WafclastLocalization();
        public WafclastLocalization LocalizationSpawnPoint { get; set; } = new WafclastLocalization();

        public WafclastStatePoints LifePoints { get; set; } = new WafclastStatePoints();
        public WafclastStatePoints Stamina { get; set; } = new WafclastStatePoints();

        public decimal Ataque { get; private set; } = 0;
        public decimal Defesa { get; private set; } = 0;

        public int Karma { get; set; } = 0;

        public DateTime RegenDate { get; set; } = DateTime.UtcNow;

        public StanceType Stance { get; set; } = StanceType.Parry;


        public WafclastCharacter()
        {
            CalcStats();
            LifePoints.Restart();
            Stamina.Restart();
        }

        public void CalcStats()
        {
            Ataque = Atributo.Forca * 3;
            LifePoints.MaxValue = Atributo.Resistencia * 8;
            Stamina.MaxValue = Atributo.Resistencia * 4;
        }

        public new bool ReceberExperiencia(decimal exp)
        {
            int levelUps = base.ReceberExperiencia(exp);
            for (int i = 0; i < levelUps; i++)
            {
                LifePoints.Restart();
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
            
            if (LifePoints.Remove(valor))
            {
                LifePoints.Restart();
                Karma = 0;
                DiminuirLevel();
                Localization = LocalizationSpawnPoint;
                return true;
            }
            return false;
        }

        public void ReceberVida(decimal valor) => LifePoints.Add(valor);

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
