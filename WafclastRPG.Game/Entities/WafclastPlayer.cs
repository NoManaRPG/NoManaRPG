// This file is part of the WafclastRPG project.

using System;
using System.Globalization;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Characters;
using WafclastRPG.Game.Extensions;

namespace WafclastRPG.Game.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastPlayer
    {
        [BsonId]
        public ulong Id { get; private set; }
        public DateTime DateAccountCreation { get; private set; }
        public WafclastBaseCharacter Character { get; private set; }

        public string Mention => $"<@{this.Id.ToString(CultureInfo.InvariantCulture)}>";
        public string Language { get; set; } = "pt-BR";

        public WafclastPlayer(ulong id, WafclastBaseCharacter Character)
        {
            this.Id = id;
            this.Character = Character;
            this.DateAccountCreation = DateTime.UtcNow;
        }

        public string BasicAttackMonster()
        {
            var monster = this.Character.Room.Monster;

            if (monster == null)
                return "você não está visualizando nenhum monstro para atacar!";

            if (monster.LifePoints.Current <= 0)
                return "o monstro que você está tentando atacar, já está morto!";

            var rd = new Random();
            var str = new StringBuilder();
            double damage;

            //Combat

            var attacking = this.Character.NextAttack();

            if (attacking.isMonster)
            {
                if (Mathematics.CalculateHitChance(monster.PrecisionPoints, this.Character.EvasionPoints))
                {
                    damage = this.Character.ReceiveDamage(rd.Sortear(monster.Damage));
                    str.AppendLine($"{this.Mention} recebeu {damage:N2} de dano!");

                    if (this.Character.IsDead)
                    {
                        str.AppendLine($"{this.Mention} {Emojis.CrossBone} morreu! ");
                        str.AppendLine($"{this.Mention} perdeu nível!");
                        this.Character.Deaths++;
                        this.Character.RemoveOneLevel();
                        return str.ToString();
                    }
                }
                else
                {
                    str.AppendLine($"{this.Mention} desviou!");
                }
            }
            if (attacking.isPlayer)
            {
                if (Mathematics.CalculateHitChance(this.Character.PrecisionPoints, monster.EvasionPoints))
                {
                    damage = monster.TakeDamage(rd.Sortear(this.Character.Damage));
                    str.AppendLine($"{monster.Mention} recebeu {damage:N2} {this.Character.EmojiAttack} de dano!");

                    if (monster.LifePoints.Current <= 0)
                    {
                        str.AppendLine($"{monster.Mention} {Emojis.CrossBone} morreu!");
                        this.Character.MonsterKills++;
                        this.Character.AddExperience(monster.Level * monster.Level);
                    }
                }
                else
                {
                    str.AppendLine($"{monster.Mention} desviou!");
                }
            }

            return str.ToString();
        }
    }
}
