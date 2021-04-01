using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;
using WafclastRPG.Enums;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class StartCommand : BaseCommandModule
    {
        public Database banco;

        [Command("comecar")]
        [Aliases("start")]
        [Description("Permite criar um personagem para poder usar os comandos.")]
        [Usage("comecar")]
        public async Task StartCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var map = await banco.CollectionMaps.Find(x => x.Id == ctx.Channel.Id).FirstOrDefaultAsync();
            if (map == null || map.Tipo != MapType.Cidade)
            {
                await ctx.ResponderAsync("você precisa criar um personagem na cidade!");
                return;
            }

            var player = await banco.FindPlayerAsync(ctx.User);
            if (player == null)
            {
                player = new WafclastPlayer(ctx.User.Id);
                player.Character.Localization = new WafclastLocalization(map.Id, ctx.Guild.Id);
                player.Character.LocalizationSpawnPoint = new WafclastLocalization(map.Id, ctx.Guild.Id);
                await ctx.ResponderAsync($"personagem criado com sucesso! Obrigado por escolher Wafclast!");
                await banco.CollectionJogadores.InsertOneAsync(player);
                return;
            }

            var embed = new DiscordEmbedBuilder();
            bool escolha;

            if (player.Character.Localization.ServerId == ctx.Guild.Id)
            {
                embed.WithDescription($"{Emojis.Aviso} Você já tem um personagem neste servidor! Você deseja recomeçar?");
                embed.AddField("Responda sim", "Sim, quero criar um novo personagem!");
                embed.AddField("Responda não", "Claro que não, como vim parar aqui?!");
                embed.WithColor(DiscordColor.Red);
                escolha = await ctx.WaitForBoolAsync(banco, embed.Build());
                if (escolha)
                {
                    await RecreatePlayer(player, ctx);
                    await ctx.ResponderAsync($"personagem recriado com sucesso! Obrigado por escolher Wafclast!");
                }
                return;
            }

            embed = new DiscordEmbedBuilder();
            embed.WithDescription($"{Emojis.Aviso} Você já tem um personagem em outro servidor! Se você deseja criar um neste, o outro precisa ser deletado!");
            embed.AddField("Responda sim", "Sim, quero criar um novo personagem neste servidor!");
            embed.AddField("Responda não", "Claro que não, nem gosto deste servidor!!!");
            embed.WithColor(DiscordColor.Red);
            escolha = await ctx.WaitForBoolAsync(banco, embed.Build());
            if (escolha)
            {
                await RecreatePlayer(player, ctx);
                await ctx.ResponderAsync($"personagem criado com sucesso! Obrigado por escolher Wafclast!");
            }
        }

        public async Task RecreatePlayer(WafclastPlayer player, CommandContext ctx)
        {
            player = new WafclastPlayer(ctx.User.Id);
            player.Character.Localization = new WafclastLocalization(ctx.Channel.Id, ctx.Guild.Id);
            player.Character.LocalizationSpawnPoint = new WafclastLocalization(ctx.Channel.Id, ctx.Guild.Id);
            await banco.CollectionJogadores.ReplaceOneAsync(x => x.Id == player.Id, player);
            await banco.CollectionItens.DeleteManyAsync(x => x.PlayerId == player.Id);
        }
    }
}
