using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class AttackCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("atacar")]
        [Aliases("at", "attack")]
        [Description("Permite executar um ataque básico em um monstro.")]
        [Usage("atacar")]
        [Cooldown(1, 5, CooldownBucketType.User)]
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

                    var cha = player.Character;
                    var target = cha.CurrentFightingMonster;

                    if (target == null)
                        return new Response($"você não tem um alvo para atacar!");
                    else if (target.LifePoints <= 0)
                        return new Response($"{target.Name} já está morto!");

                    //Combat
                    var rd = new Random();
                    var str = new StringBuilder();
                    var embed = new DiscordEmbedBuilder();


                    embed.WithColor(DiscordColor.Red);
                    embed.WithAuthor($"{ctx.User.Username}", iconUrl: ctx.User.AvatarUrl);
                    embed.WithTitle($"Relatório do Combate.");

                    //Plyer Attack
                    if (!rd.Chance(cha.CalculateHitChance(target.ArmorTotal)))
                    {
                        var playerDamage = rd.Sortear(1, cha.Damage);

                        var isTargetDead = target.TakeDamage(playerDamage);
                        str.AppendLine($"{target.Name} recebeu {playerDamage:N2} de dano.");

                        if (isTargetDead)
                        {
                            player.MonsterKills++;
                            str.AppendLine($"{Emojis.CrossBone} {target.Name.Title()} {Emojis.CrossBone}");

                            foreach (var drop in target.Drops)
                            {
                                if (rd.Chance(drop.Chance))
                                {
                                    var item = await session.FindItemAsync(drop.GlobalItemId, ctx.Client.CurrentUser);

                                    var quantity = Convert.ToUInt64(rd.Sortear(drop.MinQuantity, drop.MaxQuantity));

                                    str.AppendLine($"**+ {quantity} {item.Name.Title()}.**");
                                    if (cha.Inventory.Count >= 19)
                                        break;
                                    cha.Inventory.Add(item);
                                }
                            }

                            await player.SaveAsync();

                            embed.WithDescription(str.ToString());
                            return new Response(embed);
                        }
                    }
                    else
                        str.AppendLine($"{target.Name} conseguiu desviar!");

                    embed.AddField(target.Name, $"{target.LifePoints:N2} ", true);

                    //Monster Attack
                    var isPlayerDead = false;

                    if (rd.Chance(target.CalculateHitChance(cha.Armor)))
                        str.AppendLine($"{player.Mention} desviou do ataque!");
                    else
                    {
                        var targetDamage = rd.Sortear(1, target.MaxDamage);

                        str.AppendLine($"{player.Mention} recebeu {targetDamage:N2} de dano.");

                        isPlayerDead = player.Character.ReceiveDamage(targetDamage);

                        if (isPlayerDead)
                        {
                            str.AppendLine($"{player.Mention} morreu!");
                            player.Character.Inventory = new List<WafclastBaseItem>();
                            player.Character.RegionId = 0;
                            player.Deaths++;
                            embed.AddField(ctx.User.Username, $"{Emojis.GerarVidaEmoji(0 / player.Character.LifePoints.CurrentValue)} 0 ", true);
                            player.Character.LifePoints.Restart();
                        }
                        else
                            embed.AddField(ctx.User.Username, $"{Emojis.GerarVidaEmoji(player.Character.LifePoints.BaseValue / player.Character.LifePoints.CurrentValue)} {player.Character.LifePoints.CurrentValue:N2} ", true);
                    }

                    await player.SaveAsync();

                    return new Response(embed.WithDescription(str.ToString()));
                });

            if (!string.IsNullOrWhiteSpace(response.Message))
                await ctx.ResponderAsync(response.Message);

            if (response.Embed != null)
                await ctx.ResponderAsync(response.Embed?.Build());
        }
    }
}