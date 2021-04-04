using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Extensions;


namespace WafclastRPG.Commands.GeneralCommands
{
    public class AttributesCommand : BaseCommandModule
    {
        public Database database;

        [Command("atributos")]
        [Description("Permite ver os atributos do seu personagem")]
        [Usage("atributos")]
        public async Task AttributesCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var player = await database.FindPlayerAsync(ctx);
            if (player == null)
            {
                await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}] ", iconUrl: ctx.User.AvatarUrl);
            embed.WithColor(DiscordColor.Blue);
            embed.AddField("Força".Titulo(), player.Character.Atributos.Forca.ToString(), true);
            embed.AddField("Resistencia".Titulo(), player.Character.Atributos.Resistencia.ToString(), true);
            embed.AddField("Agilidade".Titulo(), player.Character.Atributos.Agilidade.ToString(), true);
            embed.AddField("Vitalidade".Titulo(), player.Character.Atributos.Vitalidade.ToString(), true);
            embed.AddField("Pontos Livres".Titulo(), player.Character.Atributos.PontosLivreAtributo.ToString(), true);
            await ctx.ResponderAsync(embed.Build());
        }
    }
}
