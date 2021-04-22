using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class AttackCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("atacar")]
        [Aliases("at")]
        [Description("Permite executar um ataque físico em um monstro.")]
        [Usage("atacar")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task AttackCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    if (player.Character.CurrentFloor == 0)
                        return new Response("você procura na cidade toda, mas não encontra nenhum monstro.. talvez seja melhor subir alguns andares na Torre.", player.Reminder);

                    if (player.Character.Monster == null)
                        return new Response($"este monstro já está morto! Tente procurar por outro!", player.Reminder);

                    //Combat
                    var rd = new Random();
                    var str = new StringBuilder();
                    var embed = new DiscordEmbedBuilder();
                    var target = player.Character.Monster;

                    embed.WithColor(DiscordColor.Red);
                    embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}]", iconUrl: ctx.User.AvatarUrl);
                    embed.WithTitle($"Relatorio do combate contra {target.Name}.");

                    if (!rd.Chance(player.Character.HitChance(target.Evasion.MaxValue)))
                    {
                        var playerDamage = rd.Sortear(player.Character.PhysicalDamage.CurrentValue / 2, player.Character.PhysicalDamage.CurrentValue);
                        playerDamage = target.DamageReduction(playerDamage);

                        var isTargetDead = target.ReceberDano(playerDamage);
                        str.AppendLine($"{target.Name} não conseguiu desviar!");
                        str.AppendLine($"{target.Name} recebeu {playerDamage:N2} de dano.");

                        if (isTargetDead)
                        {
                            player.MonsterKill++;
                            str.AppendLine($"{Emojis.CrossBone} {target.Name.Titulo()} {Emojis.CrossBone}");

                            foreach (var drop in target.DropChances)
                            {
                                if (rd.Chance(drop.Chance))
                                {
                                    var item = await session.FindItemAsync(drop.Id);
                                    if (item == null)
                                    {
                                        ctx.Client.Logger.LogInformation(new EventId(608, "ERROR"), $"{target.Name} está com o drop {drop.Id} errado!", DateTime.Now);
                                        continue;
                                    }
                                    var quantity = Convert.ToUInt64(rd.Sortear(drop.MinQuantity, drop.MaxQuantity));

                                    str.AppendLine($"**+ {quantity} {item.Name.Titulo()}.**");

                                    await player.AddItemAsync(item, quantity);
                                }
                            }
                            player.Character.Monster = null;

                            await session.ReplaceAsync(player);
                            await session.ReplaceAsync(target);

                            embed.WithDescription(str.ToString());
                            return new Response(embed, player.Reminder);
                        }
                    }
                    else
                        str.AppendLine($"{target.Name} conseguiu desviar!");

                    embed.AddField(target.Name, $"{Emojis.GerarVidaEmoji(target.Life.CurrentValue / target.Life.MaxValue)} {target.Life.CurrentValue:N2} ", true);

                    var isPlayerDead = false;

                    if (rd.Chance(player.Character.DodgeChance(target.Accuracy.MaxValue)))
                        str.AppendLine($"{player.Mention} desviou do ataque!");
                    else
                    {
                        var targetDamage = rd.Sortear(target.PhysicalDamage.MaxValue);
                        targetDamage = player.Character.DamageReduction(targetDamage);

                        str.AppendLine($"{player.Mention} não conseguiu desviar!");
                        str.AppendLine($"{player.Mention} recebeu {targetDamage:N2} de dano.");

                        isPlayerDead = player.Character.ReceiveDamage(targetDamage);

                        if (isPlayerDead)
                        {
                            str.AppendLine($"{player.Mention} morreu!");
                            player.Character.Coins.Coins = Convert.ToUInt32(player.Character.Coins.Coins * 0.95);
                            player.Deaths++;
                        }
                    }

                    if (isPlayerDead)
                        embed.AddField(ctx.User.Username, $"{Emojis.GerarVidaEmoji(0 / player.Character.Life.MaxValue)} 0 ", true);
                    else
                        embed.AddField(ctx.User.Username, $"{Emojis.GerarVidaEmoji(player.Character.Life.CurrentValue / player.Character.Life.MaxValue)} {player.Character.Life.CurrentValue:N2} ", true);

                    await session.ReplaceAsync(player);
                    await session.ReplaceAsync(target);
                    return new Response(embed.WithDescription(str.ToString()), player.Reminder);
                });

            if (!string.IsNullOrWhiteSpace(response.Message))
                await ctx.ResponderAsync(response.Message);

            if (response.Embed != null)
                await ctx.ResponderAsync(response.Embed?.Build());

            if (response.Reminder)
            {
                await Task.Delay(15000);
                await ctx.ResponderAsync($"{Messages.Reminder} `atacar`");
            }
        }
    }
}