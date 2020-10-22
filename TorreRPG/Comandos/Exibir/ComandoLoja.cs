using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using TorreRPG.Atributos;
using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using TorreRPG.Services;

namespace TorreRPG.Comandos.Exibir
{
    public class ComandoLoja : BaseCommandModule
    {
        public Banco banco;

        [Command("loja")]
        [Description("Permite ver os itens que podem ser comprados na loja.")]
        [ComoUsar("loja")]
        [Cooldown(1, 5, CooldownBucketType.Channel)]
        public async Task ComandoLojaAsync(CommandContext ctx)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("1 `Pergaminho de Portal` por 3 `Pergaminho de Sabedoria`");

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username}", iconUrl: ctx.User.AvatarUrl);
            embed.WithDescription(str.ToString());
            embed.WithTimestamp(DateTime.Now);
            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
