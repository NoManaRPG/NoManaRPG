using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Characters;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;
using static WafclastRPG.Mathematics;

namespace WafclastRPG.Commands.UserCommands.CombatCommands {
  public class AttackCommand : BaseCommandModule {
    public DataBase database;

    [Command("atacar")]
    [Aliases("at", "attack")]
    [Description("Permite executar um ataque em um monstro.")]
    [Usage("atacar")]
    public async Task AttackCommandAsync(CommandContext ctx) {
      await ctx.TriggerTypingAsync();
      Response response;
      using (var session = await database.StartDatabaseSessionAsync())
        response = await session.WithTransactionAsync(async (s, ct) => {
          var player = await session.FindPlayerAsync(ctx.User);
          if (player == null)
            return new Response(Messages.AindaNaoCriouPersonagem);

          var character = player.Character;
          var monster = player.Character.Region.Monster;

          if (monster == null)
            return new Response($"você não está visualizando nenhum monstro para atacar!");

          if (monster.IsDead)
            return new Response($"o monstro que você está tentando atacar, já está morto!");

          var rd = new Random();
          var str = new StringBuilder();
          var embed = new DiscordEmbedBuilder();
          double damage;

          //Combat

          var attacking = CalculateNextAttack(character);

          if (attacking.isMonster) {
            if (CalculateHitChance(monster.PrecisionPoints, character.EvasionPoints)) {

              damage = character.ReceiveDamage(rd.Sortear(monster.Damage));
              str.AppendLine($"você recebeu {damage} de dano!");

              if (character.IsDead) {
                str.AppendLine($"você morreu!");
                str.AppendLine($"você perdeu 1 nível!");
                character.RemoveOneLevel();
                goto EndCombat;
              }

            } else {
              str.AppendLine($"{monster.Name} [Nv. {monster.Level}] errou o ataque!");
            }
          }

          if (attacking.isPlayer) {
            if (CalculateHitChance(character.PrecisionPoints, monster.EvasionPoints)) {

              damage = monster.ReceiveDamage(rd.Sortear(character.Damage));
              str.AppendLine($"você deu {damage} {character.EmojiAttack} de dano!");

              if (monster.IsDead)
                str.AppendLine($"{Emojis.CrossBone} {monster.Name} {Emojis.CrossBone}");

            } else {
              str.AppendLine($"você errou o ataque!");
            }
          }

        EndCombat:

          embed.WithColor(DiscordColor.Red);
          embed.WithAuthor($"{ctx.User.Username} [Nv.{character.Level}]", iconUrl: ctx.User.AvatarUrl);
          embed.WithTitle("Relatorio de Combate");
          embed.WithDescription(str.ToString());

          //loot em outro comando!

          await player.SaveAsync();

          return new Response(embed);
        });

      if (response.Message != null)
        await ctx.ResponderAsync(response.Message);
      else
        await ctx.ResponderAsync(response.Embed);
    }

    public static (bool isPlayer, bool isMonster) CalculateNextAttack(WafclastBaseCharacter character) {
      bool isPlayerAttacking = false;
      bool isMonsterAttacking = false;

      while (isPlayerAttacking == false || isMonsterAttacking == false) {
        character.Region.PlayerAttackSpeedPoints += character.AttackSpeed;
        character.Region.MonsterAttackSpeedPoints += character.Region.Monster.AttackSpeed;

        if (character.Region.PlayerAttackSpeedPoints / character.Region.TotalAttackSpeedPoints >= 1) {
          character.Region.PlayerAttackSpeedPoints -= character.Region.TotalAttackSpeedPoints;
          isPlayerAttacking = true;
        }

        if (character.Region.MonsterAttackSpeedPoints / character.Region.TotalAttackSpeedPoints >= 1) {
          character.Region.MonsterAttackSpeedPoints -= character.Region.TotalAttackSpeedPoints;
          isMonsterAttacking = true;
        }
      }

      return (isPlayerAttacking, isMonsterAttacking);
    }
  }
}
