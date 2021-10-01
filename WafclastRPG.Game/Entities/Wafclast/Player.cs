using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Globalization;
using System.Text;
using WafclastRPG.Game.Extensions;

namespace WafclastRPG.Game.Entities.Wafclast {
  [BsonIgnoreExtraElements]
  public class Player {
    [BsonId]
    public ulong Id { get; private set; }
    public DateTime DateAccountCreation { get; private set; }
    public BaseCharacter Character { get; private set; }

    public ulong MonsterKills { get; private set; }
    public ulong Deaths { get; private set; }

    public string Language { get; set; } = "pt-BR";

    public string LifePoints { get => Character.LifePoints.Current.ToString("N2"); }

    public Player(ulong id, BaseCharacter Character) {
      this.Id = id;
      this.Character = Character;
      DateAccountCreation = DateTime.UtcNow;
    }

    public string Mention { get => $"<@{Id.ToString(CultureInfo.InvariantCulture)}>"; }

    static (string key, int value) Split(string text) {
      var split = text.Split(":");
      var key = split[0];
      var value = int.Parse(split[1]);
      return (key, value);
    }

    public string BasicAttackMonster() {
      var monster = Character.Room.Monster;

      if (monster == null)
        return "você não está visualizando nenhum monstro para atacar!";

      if (monster.IsDead)
        return "o monstro que você está tentando atacar, já está morto!";

      var rd = new Random();
      var str = new StringBuilder();
      double damage;

      //Combat
      var asd = Split("asd");

      var attacking = Character.NextAttack();

      if (attacking.isMonster) {
        if (Mathematics.CalculateHitChance(monster.PrecisionPoints, Character.EvasionPoints)) {
          damage = Character.ReceiveDamage(rd.Sortear(monster.Damage));
          str.AppendLine($"{Mention} recebeu {damage:N2} de dano!");

          if (Character.IsDead) {
            str.AppendLine($"{Mention} {Emojis.CrossBone} morreu! ");
            str.AppendLine($"{Mention} perdeu nível!");
            Deaths++;
            Character.RemoveOneLevel();
            return str.ToString();
          }
        } else {
          str.AppendLine($"{Mention} desviou!");
        }
      }
      if (attacking.isPlayer) {
        if (Mathematics.CalculateHitChance(Character.PrecisionPoints, monster.EvasionPoints)) {
          damage = monster.TakeDamage(rd.Sortear(Character.Damage));
          str.AppendLine($"{monster.Mention} recebeu {damage:N2} {Character.EmojiAttack} de dano!");

          if (monster.IsDead) {
            str.AppendLine($"{monster.Mention} {Emojis.CrossBone} morreu!");
            MonsterKills++;
            Character.AddExperience(monster.Level * monster.Level);
          }
        } else {
          str.AppendLine($"{monster.Mention} desviou!");
        }
      }

      return str.ToString();
    }
  }
}
