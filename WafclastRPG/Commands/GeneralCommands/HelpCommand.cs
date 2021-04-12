using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static DSharpPlus.CommandsNext.CommandsNextExtension;
using System;
using Microsoft.Extensions.DependencyInjection;
using DSharpPlus;
using System.Linq;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using WafclastRPG.DataBases;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class HelpCommand : BaseCommandModule
    {
        [Command("ajuda")]
        [Aliases("h", "?", "help")]
        [Description("Explica como usar um comando, suas abreviações e exemplos.")]
        [Usage("ajuda [ comando ]")]
        public async Task HelpCommanAsync(CommandContext ctx, params string[] comando)
        {
            await ctx.TriggerTypingAsync();
            if (comando.Length == 0)
                await ctx.ResponderAsync($"Oi! Eu sou Wafclast RPG! Para a lista dos comandos que eu conheço, você pode digitar `w.comandos`, ou {ctx.Client.CurrentUser.Mention} comandos");
            else
                await new DefaultHelpModule().DefaultHelpAsync(ctx, comando);
        }

        [Command("admin")]
        [Description("Exibe os comandos de Administrador.")]
        public async Task HelpCommandAdminAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithDescription("Digite `w.ajuda [comando]` para mais informações. Por exemplo: `w.ajuda mapa`.");

            embed.AddField("Mapa".Titulo(), $"{Formatter.InlineCode("mapa-criar")} {Formatter.InlineCode("mapa-editar")}", true);
            embed.AddField("Monstro".Titulo(), $"{Formatter.InlineCode("monstro-criar")} {Formatter.InlineCode("monstro-editar")}", true);
            embed.AddField("Item".Titulo(), $"{Formatter.InlineCode("item-criar")} {Formatter.InlineCode("item-editar")} {Formatter.InlineCode("loja-adicionar-item")}", true);

            embed.WithColor(DiscordColor.Violet);
            embed.WithTimestamp(DateTime.Now);
            await ctx.RespondAsync(embed.Build());
        }
    }

    public class IComandoAjuda : BaseHelpFormatter
    {
        DiscordEmbedBuilder embed;
        string prefix;

        public IComandoAjuda(CommandContext ctx) : base(ctx)
        {
            var defaultPrefix = ctx.Services.GetService<Config>().PrefixRelease;
            var banco = ctx.Services.GetService<DataBase>();
            prefix = banco.GetServerPrefix(ctx.Guild.Id, defaultPrefix);

            embed = new DiscordEmbedBuilder();
            embed.WithAuthor("Menu de ajuda do Wafclast", null, ctx.Client.CurrentUser.AvatarUrl);
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            embed.WithTitle(Formatter.Bold($"{prefix}{command.Name}"));
            embed.WithDescription($"```{command.Description}```");

            var usage = command.CustomAttributes.Where(x => x.GetType() == typeof(UsageAttribute)).FirstOrDefault();

            if (usage != null)
                embed.AddField(Formatter.Bold(Formatter.Italic("Usos")), Formatter.InlineCode($"{prefix}{(usage as UsageAttribute).Command}"), true);

            StringBuilder strAliases = new StringBuilder();
            foreach (var al in command.Aliases)
                strAliases.Append($"__*{al}*__ ,");
            embed.AddField(Formatter.Bold(Formatter.Italic("Atalhos")), $"{ (string.IsNullOrWhiteSpace(strAliases.ToString()) ? "__*nenhum*__" : strAliases.ToString()) }");
            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands) => this;

        public override CommandHelpMessage Build()
        {
            embed.WithColor(DiscordColor.CornflowerBlue);
            return new CommandHelpMessage(embed: embed.Build());
        }
    }
}
