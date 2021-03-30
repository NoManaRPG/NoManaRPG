using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class LookCommand : BaseCommandModule
    {
        public Database banco;

        [Command("olhar")]
        [Aliases("look", "l")]
        [Description("Permite ver os atributos de algum monstro ou a vida de algum jogador.")]
        [Usage("olhar [ #ID | @menção")]
        [Example("olhar #1", "Permite olhar a vida do monstro de #ID 1.")]
        public async Task LookCommandAsync(CommandContext ctx, string monsterIdString = "")
        {
            await ctx.TriggerTypingAsync();
            var player = await banco.FindPlayerAsync(ctx);
            if (player == null)
            {
                await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            if (player.Character.Localization != ctx)
            {
                await ctx.ResponderAsync(Strings.LocalDiferente(ctx.Channel.Name));
                return;
            }

            if (string.IsNullOrWhiteSpace(monsterIdString))
            {
                await ctx.ResponderAsync($"você precisa informar um {Formatter.Bold("#ID")} para olhar");
                return;
            }

            if (monsterIdString.TryParseID(out ulong id))
            {
                var monster = await banco.FindMonsterAsync(ctx.Channel.Id + id);
                if (monster == null)
                {
                    await ctx.ResponderAsync(Strings.MonstroNaoEncontrado(id));
                    return;
                }

                if (monster.DateSpawn > DateTime.UtcNow)
                    await ctx.ResponderAsync($"o monstro {monster.Nome.Titulo()} está morto!");
                else
                {
                    var porcentagemLife = Convert.ToInt32((monster.Life.CurrentValue / monster.Life.MaxValue) * 100);

                    var embed = new DiscordEmbedBuilder();
                    embed.WithTitle(monster.Nome.Titulo());
                    embed.WithDescription("Parece perigoso...");
                    embed.AddField("Vida".Titulo(), $"{Emojis.GerarVidaEmoji(porcentagemLife)} {monster.Life.CurrentValue:N2} / {monster.Life.MaxValue}");
                    await ctx.ResponderAsync(embed.Build());
                }
            }
            else
                await ctx.ResponderAsync(Strings.IdInvalido);
        }

        [Command("olhar")]
        [Example("olhar @Talion", "Permite olhar a vida do jogador mencionado.")]
        [Priority(1)]
        public async Task LookCommandAsync(CommandContext ctx, DiscordUser target)
        {
            await ctx.TriggerTypingAsync();
            var player = await banco.FindPlayerAsync(ctx);
            if (player == null)
            {
                await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            if (player.Character.Localization != ctx)
            {
                await ctx.ResponderAsync(Strings.LocalDiferente(ctx.Channel.Name));
                return;
            }

            var playerTarget = await banco.FindPlayerAsync(target);
            if (playerTarget == null)
            {
                await ctx.ResponderAsync("o jogador ainda não criou um personagem!");
                return;
            }

            if (playerTarget.Character.Localization != player.Character.Localization)
            {
                await ctx.ResponderAsync("vocês não estão no mesmo lugar!");
                return;
            }

            var porcentagemLife = Convert.ToInt32((playerTarget.Character.LifePoints.CurrentValue / playerTarget.Character.LifePoints.MaxValue) * 100);

            var embed = new DiscordEmbedBuilder();
            embed.WithTitle(target.Username.Titulo());
            embed.AddField("Vida".Titulo(), $"{Emojis.GerarVidaEmoji(porcentagemLife)} {playerTarget.Character.LifePoints.CurrentValue:N2} / {playerTarget.Character.LifePoints.MaxValue}");
            await ctx.ResponderAsync($"você olha para {target.Mention}", embed.Build());
        }
    }
}
