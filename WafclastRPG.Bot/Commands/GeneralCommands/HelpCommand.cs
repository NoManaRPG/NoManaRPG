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
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Database;

namespace WafclastRPG.Bot.Comandos.Exibir
{
    public class HelpCommand : BaseCommandModule
    {
        [Command("ajuda")]
        [Aliases("h", "?", "help")]
        [Description("Explica como usar um comando, suas abreviações e exemplos.")]
        [Example("ajuda status", "Exibe o que é o comando *status*, seus argumentos e abreviações..")]
        [Example("ajuda", "Exibe todos os comandos.")]
        [Usage("ajuda [ comando ]")]
        public async Task HelpCommanAsync(CommandContext ctx, params string[] comando)
        {
            await ctx.TriggerTypingAsync();
            await new DefaultHelpModule().DefaultHelpAsync(ctx, comando);
        }

        [Command("admin")]
        [Description("Exibe os comandos de Administrador.")]
        public async Task HelpCommandAdminAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();

            var str = new StringBuilder();

            str.Append($"**Geral** -");
            str.Append($"{Formatter.InlineCode("deletar-user")} - ");
            str.Append($"{Formatter.InlineCode("criar-mapa")} - ");
            str.Append($"{Formatter.InlineCode("criar-monstro")} - ");
            str.Append($"{Formatter.InlineCode("everyone-role")} - ");


            embed.WithDescription(str.ToString());
            embed.WithColor(DiscordColor.Violet);
            embed.WithTimestamp(DateTime.Now);

            await ctx.RespondAsync(embed.Build());
        }
    }

    public class IComandoAjuda : BaseHelpFormatter
    {
        DiscordEmbedBuilder embed;
        bool isCommandHelp;
        string prefix;

        public IComandoAjuda(CommandContext ctx) : base(ctx)
        {
            var defaultPrefix = ctx.Services.GetService<Config>().PrefixRelease;
            var banco = ctx.Services.GetService<BotDatabase>();
            prefix = banco.GetServerPrefix(ctx.Guild.Id, defaultPrefix);

            if (ctx.RawArguments.Count == 0)
                isCommandHelp = true;
            else
            {
                embed = new DiscordEmbedBuilder();
                embed.WithAuthor("Menu de ajuda do Wafclast", null, ctx.Client.CurrentUser.AvatarUrl);
                isCommandHelp = false;
            }
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            if (!isCommandHelp)
            {
                embed.WithTitle(Formatter.Bold($"{prefix}{command.Name}"));
                embed.WithDescription($"```{command.Description}```");

                var str = new StringBuilder();
                var examples = command.CustomAttributes.Where(x => x.GetType() == typeof(ExampleAttribute));
                var usage = command.CustomAttributes.Where(x => x.GetType() == typeof(UsageAttribute)).FirstOrDefault();

                foreach (var item in examples)
                {
                    str.AppendLine(Formatter.InlineCode($"{prefix}{(item as ExampleAttribute).Command}"));
                    str.AppendLine((item as ExampleAttribute).Description);
                    str.AppendLine();
                }

                if (!string.IsNullOrWhiteSpace(str.ToString()))
                    embed.AddField(Formatter.Bold(Formatter.Italic("Exemplos")), str.ToString(), true);

                if (usage != null)
                    embed.AddField(Formatter.Bold(Formatter.Italic("Usos")), Formatter.InlineCode($"{prefix}{(usage as UsageAttribute).Command}"), true);

                StringBuilder strAliases = new StringBuilder();
                foreach (var al in command.Aliases)
                    strAliases.Append($"__*{al}*__ ,");
                embed.AddField(Formatter.Bold(Formatter.Italic("Atalhos")), $"{ (string.IsNullOrWhiteSpace(strAliases.ToString()) ? "__*nenhum*__" : strAliases.ToString()) }");
            }
            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands) => this;

        public override CommandHelpMessage Build()
        {
            if (!isCommandHelp)
            {
                embed.WithColor(DiscordColor.CornflowerBlue);
                return new CommandHelpMessage(embed: embed.Build());
            }
            return new CommandHelpMessage(embed: MensagemAjuda());
        }

        public DiscordEmbed MensagemAjuda()
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithDescription("Digite `w.ajuda [comando]` para mais informações. Por exemplo: `w.ajuda bot`.");

            var str = new StringBuilder();

            str.Append($"**Geral** -");
            str.Append($"{Formatter.InlineCode("comecar")} - ");
            str.Append($"{Formatter.InlineCode("ajuda")} - ");
            str.Append($"{Formatter.InlineCode("status")} - ");
            str.Append($"{Formatter.InlineCode("viajar")} - ");
            str.AppendLine($"{Formatter.InlineCode("info")} ");

            str.Append($"**Combate** -");
            str.Append($"{Formatter.InlineCode("atacar")} - ");
            str.Append($"{Formatter.InlineCode("olhar")} - ");


            embed.WithDescription(str.ToString());
            embed.WithColor(DiscordColor.Violet);
            embed.WithTimestamp(DateTime.Now);
            return embed.Build();
        }
    }
}
