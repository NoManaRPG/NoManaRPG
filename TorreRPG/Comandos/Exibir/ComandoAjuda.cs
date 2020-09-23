using TorreRPG.Atributos;
using TorreRPG.Extensoes;
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

namespace TorreRPG.Comandos.Exibir
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
            embed.WithDescription("Utilize `!ajuda [COMANDO]` para obter mais ajuda sobre o comando específico, por exemplo: `!ajuda ajuda`");
            embed.AddField("Comando principal".Titulo().Bold(), "`!criar-personagem\n!status`", true);
            embed.AddField("Itens".Titulo().Bold(), "`!mochila\n!equipamentos\n!equipar\n!desequipar`", true);
            embed.AddField("Ajuda".Titulo().Bold(), "`!wiki\n!ajuda`", true);
            embed.AddField("Combate".Titulo().Bold(), "`!atacar\n!explorar\n!descer\n!subir\n!usar-pocao\n!olhar\n!pegar`", true);
            embed.AddField("Outros".Titulo().Bold(), "`setimage`", true);
            embed.WithImageUrl("https://cdn.discordapp.com/attachments/750081991046856814/755886689523859536/VaalTower.jpg");
            embed.WithColor(DiscordColor.Violet);
            embed.WithTimestamp(DateTime.Now);
            return embed.Build();
        }
    }
}
