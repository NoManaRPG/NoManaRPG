using DragonsDiscordRPG.Entidades;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DragonsDiscordRPG.Extensoes
{
    public static class Extensoes
    {
        public static string Titulo(this string titulo)
            => "**⌈" + titulo + "⌋**";

        //public static DiscordEmbedBuilder Padrao(this DiscordEmbedBuilder embed)
        //{
        //    embed.WithFooter("Se estiver com dúvidas, escreva z!ajuda");
        //    embed.Timestamp = DateTime.Now;
        //    return embed;
        //}

        //public static DiscordEmbedBuilder Padrao(this DiscordEmbedBuilder embed, string titulo, CommandContext ctx)
        //    => embed.Padrao().WithAuthor($"{titulo} - {ctx.User.Username}", iconUrl: ctx.User.AvatarUrl);

        public static string FirstUpper(this string texto)
            => texto.First().ToString().ToUpper() + texto.Substring(1);

        public static string Text(this double numero)
          => string.Format("{0:N2}", numero);

        public static string Underline(this string texto)
            => $"__{texto}__";

        public static string Bold(this string texto)
            => $"**{texto}**";

        public static string Italic(this string texto)
            => $"*{texto}*";

        public static string RemoverAcentos(this string texto)
            => Regex.Replace(texto, @"[^\u0000-\u007F]+", string.Empty);
        //public static async Task ExecutarComandoAsync(this CommandContext ctx, string comando)
        //{
        //    var cmd = ctx.CommandsNext.FindCommand(comando, out var args);
        //    var cfx = ctx.CommandsNext.CreateContext(ctx.Message, "z!", cmd, args);
        //    await ctx.CommandsNext.ExecuteCommandAsync(cfx);
        //}
    }

    public static class Emojis
    {
        public static DiscordEmoji PontosVida { get => DiscordEmoji.FromGuildEmote(ModuloCliente.Client, 631907691467636736); }
        public static DiscordEmoji PontosPoder { get => DiscordEmoji.FromGuildEmote(ModuloCliente.Client, 631907691425562674); }
        public const string QuadradoAzul = ":blue_square:";
        public const string QuadradoNorte = ":regional_indicator_n:";
        public const string QuadradoSul = ":regional_indicator_s:";
        public const string QuadradoLeste = ":regional_indicator_l:";
        public const string QuadradoOeste = ":regional_indicator_o:";
        public const string Mago = ":mage:";
    }
}
