using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static DSharpPlus.CommandsNext.CommandsNextExtension;
using static WafclastRPG.Bot.Utilities;
using System;
using Microsoft.Extensions.DependencyInjection;
using WafclastRPG.Bot.Config;
using DSharpPlus;
using System.Linq;
using WafclastRPG.Bot.Atributos;

namespace WafclastRPG.Bot.Comandos.Exibir
{
    public class ComandoAjuda : BaseCommandModule
    {
        [Command("ajuda")]
        [Aliases("h", "?", "help")]
        [Description("Explica como usar um comando, suas abreviações e exemplos.")]
        [Example("ajuda status", "Exibe o que é o comando *status*, seus argumentos e abreviações..")]
        [Example("ajuda", "Exibe todos os comandos.")]
        [Usage("ajuda [ comando ]")]
        public async Task ComandoAjudaAsync(CommandContext ctx, params string[] comando)
        {
            await ctx.TriggerTypingAsync();
            await new DefaultHelpModule().DefaultHelpAsync(ctx, comando);
        }
    }

    public class IComandoAjuda : BaseHelpFormatter
    {
        DiscordEmbedBuilder embed;
        bool isCommandHelp;
        string prefix;

        public IComandoAjuda(CommandContext ctx) : base(ctx)
        {
            var defaultPrefix = ctx.Services.GetService<ConfigFile>().Prefix;
            var banco = ctx.Services.GetService<Banco>();
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

            str.Append($"**Geral** - ");
            str.Append($"{FormatarURLComando(prefix, "ajuda", "Mostra todos os comandos e ajuda do comando especificado")} ");
            //str.Append($"{FormatarURLComando(prefix, "top", "Exibe as pessoas mais ricas")} ");
            str.AppendLine();
            str.Append($"**Core** - ");
            //str.Append($"{FormatarURLComando(prefix, "criar-personagem", "Permite criar o seu personagem escolhendo uma das 7 classes disponíveis")} ");
            str.Append($"{FormatarURLComando(prefix, "bot", "Exibe informações sobre o bot")} ");
            str.Append($"{FormatarURLComando(prefix, "prefixo", "Permite editar o prefixo do bot no servidor atual")} ");
            str.AppendLine();
            str.Append($"**Usuario** - ");
            str.Append($"{FormatarURLComando(prefix, "mochila", "Permite ver os itens da mochila")} ");
            //str.Append($"{FormatarURLComando(prefix, "usar", "Permite usar os itens da mochila")} ");
            //str.Append($"{FormatarURLComando(prefix, "examinar", "Permite examinar os itens da mochila")} ");
            //str.Append($"{FormatarURLComando(prefix, "status", "Permite ver o seu status ou a de outra pessoa")} ");
            str.Append($"{FormatarURLComando(prefix, "explorar", "Permite explorar e atacar monstros")}");
            //str.Append($"{FormatarURLComando(prefix, "atribuir", "Permite atribuir pontos")}");
            str.Append($"{FormatarURLComando(prefix, "habilidade", "Exibe todas as habilidades")}");
            str.AppendLine();

            embed.WithDescription(str.ToString());
            embed.WithColor(DiscordColor.Violet);
            embed.WithTimestamp(DateTime.Now);
            embed.WithFooter("Passe o mouse em cima para mais info!");
            return embed.Build();
        }
    }
}
