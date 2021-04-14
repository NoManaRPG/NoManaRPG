using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Monsters;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.UserCommands
{
    public class LookCommand : BaseCommandModule
    {
        public DataBase banco;

        [Command("olhar")]
        [Aliases("look", "l")]
        [Description("Permite ver a vida de algum monstro, jogador ou examinar uma região.")]
        [Usage("olhar [ ID | @menção ]")]
        [Priority(1)]
        public async Task LookCommandAsync(CommandContext ctx, int monsterId)
        {
            //await ctx.TriggerTypingAsync();
            //var player = await banco.FindAsync(ctx.User);
            //if (player.Character == null)
            //{
            //    //   await ctx.ResponderAsync(Strings.NovoJogador);
            //    return;
            //}

            //var monster = await banco.FindAsync(new WafclastMonsterBase(ctx.Channel.Id, monsterId));
            //if (monster == null)
            //{
            //    await ctx.ResponderAsync("não foi encontrado nenhum monstro com o ID informado!");
            //    return;
            //}

            //if (monster.DateSpawn > DateTime.UtcNow)
            //    await ctx.ResponderAsync($"o monstro {monster.Name.Titulo()} está morto!");
            //else
            //{
            //    var porcentagemLife = Convert.ToInt32(monster.Life.CurrentValue / monster.Life.MaxValue);

            //    var embed = new DiscordEmbedBuilder();
            //    embed.WithTitle(monster.Name.Titulo());
            //    embed.AddField("Vida".Titulo(), $"{Emojis.GerarVidaEmoji(porcentagemLife)} {monster.Life.CurrentValue:N2} / {monster.Life.MaxValue}");
            //    embed.WithDescription("Parece perigoso...");
            //    await ctx.ResponderAsync(embed.Build());
            //}
        }

        [Command("olhar")]
        public async Task LookCommandAsync(CommandContext ctx)
        {
            //await ctx.TriggerTypingAsync();
            //var player = await banco.FindAsync(ctx.User);
            //if (player.Character == null)
            //{
            //    // await ctx.ResponderAsync(Strings.NovoJogador);
            //    return;
            //}

            //var map = await banco.FindAsync(ctx.Channel);

            //var embed = new DiscordEmbedBuilder();
            //embed.WithTitle(ctx.Channel.Name.Titulo());
            //embed.WithDescription($"Coordenadas: {map.Coordinates.X}x, {map.Coordinates.Y}y");
            //embed.AddField("Monstros".Titulo(), $"1 - {map.QuantidadeMonstros}", true);
            //embed.AddField("Tipo".Titulo(), $"{map.Tipo.GetEnumDescription()}", true);
            //await ctx.ResponderAsync($"você olha para {ctx.Channel.Mention}", embed.Build());
        }
    }
}
