using DSharpPlus.Entities;

namespace TorreRPG.Extensoes
{
    public static class Extensoes
    {

        //public static DiscordEmbedBuilder Padrao(this DiscordEmbedBuilder embed)
        //{
        //    embed.WithFooter("Se estiver com dúvidas, escreva z!ajuda");
        //    embed.Timestamp = DateTime.Now;
        //    return embed;
        //}

        //public static DiscordEmbedBuilder Padrao(this DiscordEmbedBuilder embed, string titulo, CommandContext ctx)
        //    => embed.Padrao().WithAuthor($"{titulo} - {ctx.User.Username}", iconUrl: ctx.User.AvatarUrl);

      
        //public static async Task ExecutarComandoAsync(this CommandContext ctx, string comando)
        //{
        //    var cmd = ctx.CommandsNext.FindCommand(comando, out var args);
        //    var cfx = ctx.CommandsNext.CreateContext(ctx.Message, "z!", cmd, args);
        //    await ctx.CommandsNext.ExecuteCommandAsync(cfx);
        //}
    }
}
