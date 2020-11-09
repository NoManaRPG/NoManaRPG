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
using WafclastRPG.Bot.Extensoes;
using Microsoft.Extensions.DependencyInjection;
using WafclastRPG.Bot.Config;
using DSharpPlus;

namespace WafclastRPG.Bot.Comandos.Exibir
{
    public class ComandoAjuda : BaseCommandModule
    {
        [Command("ajuda")]
        [Aliases("h", "?", "help")]
        [Description("Explica como usar um comando, suas abreviações e exemplos.")]
        public async Task ComandoAjudaAsync(CommandContext ctx, params string[] comando)
        {
            await ctx.TriggerTypingAsync();
            await new DefaultHelpModule().DefaultHelpAsync(ctx, comando);
        }
    }

    public class IComandoAjuda : BaseHelpFormatter
    {
        DiscordEmbedBuilder embed;
        StringBuilder srSubCommands;
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
                isCommandHelp = false;
            }
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            if (!isCommandHelp)
            {
                var methods = command.Overloads;
                var srHow = new StringBuilder();
                var srArguments = new StringBuilder();
                srHow.Append($"**{prefix}{command.Name}");
                foreach (var item in methods)
                {

                    foreach (var arg in item.Arguments)
                    {
                        srHow.Append($" {arg.Name.ToUpper()} ");
                        srArguments.AppendLine($"{arg.Name.ToUpper()} -> `{arg.Description}`");
                        srArguments.AppendLine($"É opcional? {(arg.IsOptional ? "Sim" : "Não")}");
                        if (arg.DefaultValue != null)
                            srArguments.AppendLine($"Por padrão é {arg.DefaultValue}.");
                    }
                    srHow.AppendLine("**");
                    srHow.AppendLine();
                }

                StringBuilder strAliases = new StringBuilder();
                foreach (var al in command.Aliases)
                    strAliases.Append($"`{al}` ,");
                if (strAliases.Length != 0)
                    embed.AddField($"**Atalhos**", strAliases.ToString());
                embed.WithTitle(Formatter.Bold($"{command.Name.FirstUpper()}"));
                embed.WithDescription(command.Description);
                embed.AddField("**Como usar**", srHow.ToString() + srArguments.ToString());
            }
            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            if (!isCommandHelp)
            {
                srSubCommands = new StringBuilder();
                foreach (var item in subcommands)
                    srSubCommands.Append($"`{item.Name}` , ");
                embed.AddField("**Comandos**", srSubCommands.ToString());
            }
            return this;
        }

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

            str.Append($"**Core** - ");
            str.Append($"{FormatarURLComando(prefix, "criar-personagem", "Permite criar o seu personagem escolhendo uma das 7 classes disponíveis")} ");
            str.Append($"{FormatarURLComando(prefix, "bot", "Exibe informações sobre o bot")} ");
            str.AppendLine();
            str.Append($"**Itens** - ");
            str.Append($"{FormatarURLComando(prefix, "mochila", "Permite ver os itens da mochila")} ");
            str.Append($"{FormatarURLComando(prefix, "usar", "Permite usar os itens da mochila")} ");
            str.Append($"{FormatarURLComando(prefix, "examinar", "Permite examinar os itens da mochila")} ");
            str.Append($"");
            str.AppendLine();
            str.Append($"**Combate** - ");
            str.Append($"{FormatarURLComando(prefix, "status", "Permite ver o seu status ou a de outra pessoa")} ");
            str.Append($"{FormatarURLComando(prefix, "explorar", "Permite explorar e atacar monstros")}");
            str.AppendLine();
            str.Append($"**Outros** - ");
            str.Append($"{FormatarURLComando(prefix, "ajuda", "Mostra todos os comandos e ajuda do comando especificado")} ");
            str.Append($"{FormatarURLComando(prefix, "top", "Exibe as pessoas mais ricas")} ");
            str.AppendLine();

            //embed.AddField("Mercado".Titulo(), FormatarURLComando("vender", "Permite vender itens") +
            //       FormatarURLComando("comprar", "Permite comprar os itens que estão a venda") +
            //    FormatarURLComando("loja", "Permite ver os itens que estão a venda"), true);

            embed.WithDescription(str.ToString());
            embed.WithColor(DiscordColor.Violet);
            embed.WithTimestamp(DateTime.Now);
            embed.WithFooter("Passe o mouse em cima para mais info!");
            return embed.Build();
        }
    }
}
