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
        [Description("Permite ver a vida de algum monstro, jogador ou examinar uma região.")]
        [Usage("olhar [ ID | @menção ]")]
        [Example("olhar 1", "Permite olhar a vida do monstro de ID 1.")]
        [Priority(1)]
        public async Task LookCommandAsync(CommandContext ctx, string monsterIdString)
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

            if (monsterIdString.TryParseID(out int id))
            {
                var monster = await banco.FindMonsterAsync(player.Character.Localization.ChannelId, id);
                if (monster == null)
                {
                    await ctx.ResponderAsync(Strings.MonstroNaoEncontrado(id));
                    return;
                }

                if (monster.DateSpawn > DateTime.UtcNow)
                    await ctx.ResponderAsync($"o monstro {monster.Nome.Titulo()} está morto!");
                else
                {
                    var porcentagemLife = Convert.ToInt32(monster.Life.CurrentValue / monster.Life.MaxValue);

                    var embed = new DiscordEmbedBuilder();
                    embed.WithTitle(monster.Nome.Titulo());
                    embed.AddField("Vida".Titulo(), $"{Emojis.GerarVidaEmoji(porcentagemLife)} {monster.Life.CurrentValue:N2} / {monster.Life.MaxValue}");
                    embed.AddField("Força".Titulo(), $"{monster.Atributos.ForcaMin} ~ {monster.Atributos.ForcaMax}");
                    embed.AddField("Resistencia".Titulo(), $"{monster.Atributos.ResistenciaMin} ~ {monster.Atributos.ResistenciaMax}");
                    embed.AddField("Agilidade".Titulo(), $"{monster.Atributos.AgilidadeMin} ~ {monster.Atributos.AgilidadeMax}");
                    embed.AddField("Experiencia".Titulo(), $"{monster.Atributos.ExpMin} ~ {monster.Atributos.ExpMax}");
                    embed.AddField("Respawn a cada".Titulo(), $"{monster.RespawnTime}");
                    embed.WithDescription("Parece perigoso...");
                    await ctx.ResponderAsync(embed.Build());
                }
            }
            else
                await ctx.ResponderAsync(Strings.IdInvalido);
        }

        [Command("olhar")]
        [Example("olhar @Talion", "Permite olhar a vida do jogador mencionado.")]
        [Priority(2)]
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

            decimal porcentagemLife = Convert.ToInt32(playerTarget.Character.LifePoints.CurrentValue / playerTarget.Character.LifePoints.MaxValue);

            var embed = new DiscordEmbedBuilder();
            embed.WithTitle(target.Username.Titulo());
            embed.AddField("Vida".Titulo(), $"{Emojis.GerarVidaEmoji(porcentagemLife)} {playerTarget.Character.LifePoints.CurrentValue:N2} / {playerTarget.Character.LifePoints.MaxValue:N2}");
            await ctx.ResponderAsync($"você olha para {target.Mention}", embed.Build());
        }

        [Command("olhar")]
        [Example("olhar", "Permite olhar o mapa.")]
        public async Task LookCommandAsync(CommandContext ctx)
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

            var map = await banco.FindMapAsync(ctx);

            var embed = new DiscordEmbedBuilder();
            embed.WithTitle(ctx.Channel.Name.Titulo());
            embed.WithDescription($"Coordenadas: {map.Coordinates.X}x, {map.Coordinates.Y}y");
            embed.AddField("Monstros".Titulo(), $"1 - {map.QuantidadeMonstros}", true);
            embed.AddField("Tipo".Titulo(), $"{map.Tipo.GetEnumDescription()}", true);
            await ctx.ResponderAsync($"você olha para {ctx.Channel.Mention}", embed.Build());
        }
    }
}
