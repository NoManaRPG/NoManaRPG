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

namespace WafclastRPG.Comandos.Exibir
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
            str.Append($"{Formatter.InlineCode("monstro-criar")} - ");
            str.Append($"{Formatter.InlineCode("monstro-atributos")} - ");
            str.Append($"{Formatter.InlineCode("atualizar")} - ");
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
            var banco = ctx.Services.GetService<Database>();
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


            embed.AddField("Bot".Titulo(), $"{Formatter.InlineCode("ajuda")} {Formatter.InlineCode("info")}", true);
            embed.AddField("Jogador".Titulo(), $"{Formatter.InlineCode("comecar")} {Formatter.InlineCode("status")} {Formatter.InlineCode("postura")} {Formatter.InlineCode("inventario")}", true);
            embed.AddField("Canal de texto".Titulo(), $"{Formatter.InlineCode("atacar")} {Formatter.InlineCode("viajar")} {Formatter.InlineCode("olhar")}", true);
            embed.AddField("Ranks".Titulo(), $"{Formatter.InlineCode("rank-moedas")} {Formatter.InlineCode("rank-nivel")}", true);

            embed.WithDescription("Para mais informações a cerca de algum comando. O comando [ajuda + comado] mostra novas informações.");
            embed.WithThumbnail("https://naomesmo.com.br/wp-content/uploads/2013/01/me-ajuda.gif");
            embed.WithColor(DiscordColor.Violet);
            embed.WithTimestamp(DateTime.Now);
            return embed.Build();
        }
    }
}
