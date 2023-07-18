// This file is part of NoManaRPG project.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NoManaRPG.Attributes;
using NoManaRPG;
using static DSharpPlus.CommandsNext.CommandsNextExtension;

namespace NoManaRPG.Commands;

public class HelpCommand : ApplicationCommandModule
{
    [Command("comandos")]
    [Aliases("commands")]
    [Description("Exibe todos os comandos que o bot reconhece.")]
    [Usage("comandos")]
    [Cooldown(1, 5, CooldownBucketType.User)]
    public async Task CommandsAsync(InteractionContext ctx)
    {
        var str = new StringBuilder();
        str.AppendLine();
        str.AppendLine("[Geral]");
        str.Append("comandos, ");
        str.Append("ajuda, ");
        str.Append("info, ");

        //str.AppendLine();
        //str.AppendLine("[Habilidades]");
        //str.Append("habilidades, ");
        ////str.Append("minerar, ");
        ////str.Append("cozinhar, ");

        str.AppendLine();
        str.AppendLine("[Usuário]");
        str.Append("comecar, ");
        str.Append("olhar, ");
        str.Append("explorar, ");
        str.Append("ataque-basico, ");
        str.Append("status, ");
        //str.Append("inventario, ");
        //str.Append("examinar, ");
        str.Append("atributos, ");
        str.Append("atribuir, ");

        await ctx.CreateResponseAsync(Formatter.BlockCode(str.ToString(), "css"));
    }

    [Command("ajuda")]
    [Aliases("h", "?", "help")]
    [Description("Explica como usar um comando, suas abreviações e exemplos.")]
    [Usage("ajuda [ comando ]")]
    [Cooldown(1, 5, CooldownBucketType.User)]
    public async Task HelpCommanAsync(CommandContext ctx, params string[] comando)
    {
        if (comando.Length == 0)
            await ctx.RespondAsync($"Oi! Eu sou o Wafclast RPG! Para a lista dos comandos que eu conheço, você pode digitar `w.comandos`, ou {ctx.Client.CurrentUser.Mention} comandos");
        else
            await new DefaultHelpModule().DefaultHelpAsync(ctx, comando);
    }
}

public class IHelpCommand : BaseHelpFormatter
{
    DiscordEmbedBuilder _embed;

    public IHelpCommand(CommandContext ctx) : base(ctx)
    {

        var banco = ctx.Services.GetService<MongoDbContext>();
        this._embed = new DiscordEmbedBuilder();
        this._embed.WithAuthor("Menu de ajuda do Wafclast", null, ctx.Client.CurrentUser.AvatarUrl);
    }

    public override BaseHelpFormatter WithCommand(Command command)
    {
        this._embed.WithTitle(Formatter.Bold($"{command.Name}"));
        this._embed.WithDescription($"```{command.Description}```");

        var usage = command.CustomAttributes.Where(x => x.GetType() == typeof(UsageAttribute)).FirstOrDefault();

        if (usage != null)
            this._embed.AddField(Formatter.Bold(Formatter.Italic("Como usar")), Formatter.InlineCode($"{(usage as UsageAttribute).Command}"), true);

        StringBuilder strAliases = new StringBuilder();
        foreach (var al in command.Aliases)
            strAliases.Append($"__*{al}*__ ,");
        this._embed.AddField(Formatter.Bold(Formatter.Italic("Atalhos")), $"{(string.IsNullOrWhiteSpace(strAliases.ToString()) ? "__*nenhum*__" : strAliases.ToString())}");
        return this;
    }

    public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands) => this;

    public override CommandHelpMessage Build()
    {
        this._embed.WithColor(DiscordColor.CornflowerBlue);
        return new CommandHelpMessage(embed: this._embed.Build());
    }
}
