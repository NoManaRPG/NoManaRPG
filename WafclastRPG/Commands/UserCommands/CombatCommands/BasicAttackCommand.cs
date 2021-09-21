using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Wafclast;
using WafclastRPG.Extensions;
using static WafclastRPG.Mathematics;

namespace WafclastRPG.Commands.UserCommands.CombatCommands {
  [ModuleLifespan(ModuleLifespan.Transient)]
  public class BasicAttackCommand : BaseCommandModule {
    public Response Res { private get; set; }
    public DataBase Data { private get; set; }

    [Command("ataque-basico")]
    [Aliases("at", "attack", "ab")]
    [Description("Permite executar um ataque em um monstro.")]
    [Usage("atacar")]
    public async Task BasicAttackCommandAsync(CommandContext ctx) {
      using (var session = await Data.StartDatabaseSessionAsync())
        Res = await session.WithTransactionAsync(async (s, ct) => {
          var player = await session.FindPlayerAsync(ctx);

          var character = player.Character;
          var monster = player.Character.Room.Monster;

          if (monster == null)
            return new Response($"você não está visualizando nenhum monstro para atacar!");

          if (monster.IsDead)
            return new Response($"o monstro que você está tentando atacar, já está morto!");

          var rd = new Random();
          var str = new StringBuilder();
          var embed = new DiscordEmbedBuilder();
          double damage;

          //Combat

          var attacking = RoomAttackOrder.CalculateNextAttack(character);

          if (attacking.isMonster) {
            if (CalculateHitChance(monster.PrecisionPoints, character.EvasionPoints)) {

              damage = character.ReceiveDamage(rd.Sortear(monster.Damage));
              str.AppendLine($"{player.Mention} recebeu {damage:N2} de dano!");

              embed.AddField(ctx.User.Username, $"{Emojis.GerarVidaEmoji(character.LifePoints.Current / character.LifePoints.Max)} {character.LifePoints.Current:N2} ", true);

              if (character.IsDead) {
                str.AppendLine($"{player.Mention} {Emojis.CrossBone} morreu! ");
                str.AppendLine($"{player.Mention} perdeu nível!");
                player.Deaths++;

                character.RemoveOneLevel();
                character.LifePoints.Restart();
                goto EndCombat;
              }

            } else {
              embed.AddField(ctx.User.Username, $"{Emojis.GerarVidaEmoji(character.LifePoints.Current / character.LifePoints.Max)} {character.LifePoints.Current:N2} ", true);
              str.AppendLine($"{player.Mention} desviou!");
            }
          }

          if (attacking.isPlayer) {
            if (CalculateHitChance(character.PrecisionPoints, monster.EvasionPoints)) {

              damage = monster.TakeDamage(rd.Sortear(character.Damage));
              str.AppendLine($"{monster.Mention} recebeu {damage:N2} {character.EmojiAttack} de dano!");


              if (monster.IsDead) {
                str.AppendLine($"{monster.Mention} {Emojis.CrossBone} morreu!");
                player.MonsterKills++;
                character.AddExperience(monster.Level);
              }

            } else {
              str.AppendLine($"{monster.Mention} desviou!");
            }
          }

        EndCombat:

          embed.WithColor(DiscordColor.Red);
          embed.WithAuthor($"{ctx.User.Username} [Nv.{character.Level}]", iconUrl: ctx.User.AvatarUrl);
          embed.WithTitle("Relatório de Combate");
          embed.WithDescription(str.ToString());

          embed.AddField(monster.Mention, $"{Emojis.GerarVidaEmoji(monster.LifePoints.Current / monster.LifePoints.Max)} {monster.LifePoints.Current:N2} ", true);

          //loot em outro comando!

          await player.SaveAsync();
          return new Response(embed);
        });
      await ctx.ResponderAsync(Res);
    }
  }
}
