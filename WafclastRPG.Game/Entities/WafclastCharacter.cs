using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace WafclastRPG.Game.Entities
{
    public class WafclastCharacter : WafclastLevel
    {
        public WafclastCoins Coins { get; private set; } = new WafclastCoins(20);
        public ulong LocalId { get; private set; }
        public int VelocidadeMovimento { get; private set; }
        public int VelocidadeAtaque { get; private set; }
        public int Defesa { get; private set; }
        public int Ataque { get; private set; }
        public int Vida { get; private set; }

        public int Concentracao { get; private set; } = 5;
        public int Sorte { get; private set; } = 1;
        public int Estamina { get; private set; } = 5;

        public WafclastCharacter()
        {
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
