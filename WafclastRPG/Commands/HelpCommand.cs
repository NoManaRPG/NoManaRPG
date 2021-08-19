using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static DSharpPlus.CommandsNext.CommandsNextExtension;
using Microsoft.Extensions.DependencyInjection;
using DSharpPlus;
using System.Linq;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;

namespace WafclastRPG.Commands
{
    public class HelpCommand : BaseCommandModule
    {
        [Command("comandos")]
        [Aliases("commands")]
        [Description("Exibe todos os comandos que o bot reconhece.")]
        [Usage("comandos")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task CommandsAsync(CommandContext ctx)
        {
            var str = new StringBuilder();
            str.AppendLine();
            str.AppendLine("[Mercado Geral]");
            str.Append("mgcriarvenda, ");
            str.Append("mgvervendas, ");
            str.Append("mgcomprar, ");
            str.Append("mglista, ");
            str.Append("mgparar, ");

            str.AppendLine();
            str.AppendLine("[Geral]");
            str.Append("comandos, ");
            str.Append("ajuda, ");
            str.Append("info, ");

            str.AppendLine();
            str.AppendLine("[Habilidades]");
            str.Append("habilidades, ");
            //str.Append("minerar, ");
            //str.Append("cozinhar, ");

            str.AppendLine();
            str.AppendLine("[Usuário]");
            str.Append("comecar, ");
            str.Append("olhar, ");
            //str.Append("explorar, ");
            str.Append("atacar, ");
            str.Append("status, ");
            str.Append("inventario, ");
            str.Append("subir, ");
            str.Append("descer, ");
            str.Append("dar, ");
            str.Append("despertador, ");
            str.Append("atributos, ");
            str.Append("atribuir, ");
            str.Append("absorver, ");
            str.Append("examinar, ");

            await ctx.RespondAsync(Formatter.BlockCode(str.ToString(), "css"));
        }

        [Command("ajuda")]
        [Aliases("h", "?", "help")]
        [Description("Explica como usar um comando, suas abreviações e exemplos.")]
        [Usage("ajuda [ comando ]")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task HelpCommanAsync(CommandContext ctx, params string[] comando)
        {
            await ctx.TriggerTypingAsync();
            if (comando.Length == 0)
                await ctx.RespondAsync($"Oi! Eu sou o Wafclast RPG! Para a lista dos comandos que eu conheço, você pode digitar `w.comandos`, ou {ctx.Client.CurrentUser.Mention} comandos");
            else
                await new DefaultHelpModule().DefaultHelpAsync(ctx, comando);
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
