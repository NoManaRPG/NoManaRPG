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
        public DataBase database;

        [Command("atributos")]
        [Description("Permite ver os atributos do seu personagem")]
        [Usage("atributos")]
        public async Task AttributesCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var player = await database.FindAsync(ctx.User);
            if (player.Character == null)
            {
               // await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}] ", iconUrl: ctx.User.AvatarUrl);
            embed.WithColor(DiscordColor.Blue);
            embed.AddField("Força".Titulo(), player.Character.Strength.CurrentValue.ToString(), true);
            embed.AddField("Destreza".Titulo(), player.Character.Dexterity.CurrentValue.ToString(), true);
            embed.AddField("Inteligencia".Titulo(), player.Character.Intelligence.CurrentValue.ToString(), true);
            embed.AddField("Pontos Livres".Titulo(), player.Character.AttributePoints.ToString(), true);
            await ctx.ResponderAsync(embed.Build());
        }
    }
}
