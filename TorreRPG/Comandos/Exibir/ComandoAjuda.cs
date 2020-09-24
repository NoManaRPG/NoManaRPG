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
using DSharpPlus;

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
            //!equipamentos!equipar!desequipar
            embed.WithDescription("Escreva `!ajuda [comando]` para mais ajuda. Por exemplo: `!ajuda status`");

            embed.AddField("Comando principal".Titulo().Bold(), FormatarURL("criar-personagem", "Permite criar o seu personagem escolhendo uma das 7 classes disponíveis") +
                FormatarURL("status", "Permite ver o seu status ou a de outra pessoa"), true);

            embed.AddField("Itens".Titulo().Bold(), FormatarURL("mochila", "Permite ver os itens da mochila") +
                FormatarURL("equipamentos", "Permite ver os itens equipados") +
                FormatarURL("examinar", "Permite ver a descrição de um item") +
                FormatarURL("equipar", "Permite equipar itens") +
                FormatarURL("desequipar", "Permite desequipr itens equipados"), true);

            embed.AddField("Ajuda".Titulo().Bold(), FormatarURL("wiki", "Wiki oficial do bot") +
                FormatarURL("ajuda", "Mostra todos os comandos e ajuda do comando especificado"), true);

            embed.AddField("Combate".Titulo().Bold(), FormatarURL("atacar", "Permite atacar um monstro que encontra na sua frente\nAtalhos: !at") +
                FormatarURL("explorar", "Permite explorar o andar por mais monstros") +
                FormatarURL("descer", "Permite descer um andar") +
                FormatarURL("subir", "Permite subir um andar") +
                FormatarURL("usar-pocao", "Permite usar uma poção que se encontra no cinto") +
                FormatarURL("olhar", "Permite olhar os itens no chão") +
                FormatarURL("pegar", "Permite pegar um item que está no chão"), true);

            embed.AddField("Outros".Titulo().Bold(), FormatarURL("setimage", "Permite escolher uma imagem para o retrato"), true);

            embed.WithImageUrl("https://cdn.discordapp.com/attachments/750081991046856814/755886689523859536/VaalTower.jpg");
            embed.WithThumbnail("https://cdn.discordapp.com/attachments/736163626934861845/742671714386968576/help_animated_x4_1.gif");
            embed.WithColor(DiscordColor.Violet);
            embed.WithTimestamp(DateTime.Now);
            embed.WithFooter("Passe o mouse em cima para mais info!");
            return embed.Build();
        }

        public string FormatarURL(string comando, string hover)
            => Formatter.MaskedUrl($"`!{comando}` ", new Uri("https://discord.gg/MAR4NFq"), hover);
    }
}
