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
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;

namespace WafclastRPG.Bot.Comandos.Exibir
{
    public class ComandoAjuda : BaseCommandModule
    {
        [Command("ajuda")]
        [Aliases("h", "?", "help")]
        [Description("Explica como usar um comando, suas abreviações e exemplos.")]
        [ComoUsar("ajuda [comando]")]
        [Exemplo("ajuda status")]
        public async Task ComandoAjudaAsync(CommandContext ctx, params string[] comando)
        {
            await ctx.TriggerTypingAsync();
            await new DefaultHelpModule().DefaultHelpAsync(ctx, comando);
        }
    }

    public class IAjudaComando : BaseHelpFormatter
    {
        private DiscordEmbedBuilder _embed;
        private StringBuilder _srExemplos;
        private StringBuilder _srUsos;
        private StringBuilder _srSubCommands;
        private bool _comandoAjuda;
        public IAjudaComando(CommandContext ctx) : base(ctx)
        {
            if (ctx.RawArguments.Count == 0)
                _comandoAjuda = true;
            else
                _comandoAjuda = false;

            if (!_comandoAjuda)
            {
                _embed = new DiscordEmbedBuilder();
                _srExemplos = new StringBuilder();
                _srUsos = new StringBuilder();
            }
        }


        public override BaseHelpFormatter WithCommand(Command command)
        {
            if (!_comandoAjuda)
            {
                foreach (var item in command.ExecutionChecks)
                {
                    switch (item)
                    {
                        case ExemploAttribute e:
                            _srExemplos.Append($"`!{e.Mensagem}`\n");
                            break;
                        case ComoUsarAttribute u:
                            _srUsos.Append($"`!{u.Mensagem}`\n");
                            break;
                    }
                }

                StringBuilder strAliases = new StringBuilder();
                foreach (var al in command.Aliases)
                    strAliases.Append($"`{al}` ,");
                if (strAliases.Length != 0)
                    _embed.AddField($"**Atalhos**", strAliases.ToString());
                _embed.WithTitle($"**{command.Name.FirstUpper()}**");
                _embed.WithDescription(command.Description);
                if (_srUsos.Length != 0)
                    _embed.AddField("**Como usar**", _srUsos.ToString());
                if (_srExemplos.Length != 0)
                    _embed.AddField("**Exemplos**", _srExemplos.ToString());
            }
            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            if (!_comandoAjuda)
            {
                _srSubCommands = new StringBuilder();
                foreach (var item in subcommands)
                    _srSubCommands.Append($"`{item.Name}` , ");
                _embed.AddField("**Comandos**", _srSubCommands.ToString());
            }
            return this;
        }

        public override CommandHelpMessage Build()
        {
            if (!_comandoAjuda)
            {
                _embed.WithColor(DiscordColor.CornflowerBlue);
                return new CommandHelpMessage(embed: _embed.Build());
            }
            return new CommandHelpMessage(embed: MensagemAjuda());
        }

        public DiscordEmbed MensagemAjuda()
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithDescription("Digite `w.ajuda [comando]` para mais informações. Por exemplo: `w.ajuda bot`.");

            var str = new StringBuilder();

            str.Append($"**Core** - ");
            str.Append($"{FormatarURLComando("criar-personagem", "Permite criar o seu personagem escolhendo uma das 7 classes disponíveis")} ");
            str.Append($"{FormatarURLComando("bot", "Exibe informações sobre o bot")} ");
            str.AppendLine();
            str.Append($"**Itens** - ");
            str.Append($"{FormatarURLComando("mochila", "Permite ver os itens da mochila")} ");
            str.Append($"{FormatarURLComando("usar", "Permite usar os itens da mochila")} ");
            str.Append($"{FormatarURLComando("examinar", "Permite examinar os itens da mochila")} ");
            str.Append($"");
            str.AppendLine();
            str.Append($"**Combate** - ");
            str.Append($"{FormatarURLComando("status", "Permite ver o seu status ou a de outra pessoa")} ");
            str.Append($"{FormatarURLComando("explorar", "Permite explorar e atacar monstros")}");
            str.AppendLine();
            str.Append($"**Outros** - ");
            str.Append($"{FormatarURLComando("ajuda", "Mostra todos os comandos e ajuda do comando especificado")} ");
            str.Append($"{FormatarURLComando("top", "Exibe as pessoas mais ricas")} ");
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
