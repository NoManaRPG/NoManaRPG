// This file is part of NoManaRPG project.

using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using NoManaRPG.DiscordEvents;
using NoManaRPG.Extensions;

namespace NoManaRPG.Comandos;

public class InfoComando : ApplicationCommandModule
{
    [SlashCommand("info", "Exibe informações sobre o bot, memoria usada, quantia de servidores etc.")]
    [SlashCooldown(1, 15, SlashCooldownBucketType.User)]
    public async Task InfoCommandAsync(InteractionContext ctx)
    {
        var embed = new DiscordEmbedBuilder();
        embed.WithAuthor(ctx.Client.CurrentUser.Username, "https://discord.gg/MAR4NFq", ctx.Client.CurrentUser.AvatarUrl);
        embed.WithThumbnail(ctx.Client.CurrentUser.AvatarUrl);
        embed.WithTitle("RPG para quem usa muito o discord!");
        //var criador = ctx.Client.CurrentApplication.Owners.First();
        var str = new StringBuilder();
        str.AppendLine("Servidor Oficial".Url("https://discord.gg/Ht2uvSr4NW"));
        str.AppendLine("Servidor Parceiro - Nenhum");
        str.AppendLine();
        str.AppendLine("Proximos updates".Url("https://discord.com/channels/1118002801046474773/1130578972766392471"));
        str.AppendLine("Atualização recente".Url("https://discord.com/channels/1118002801046474773/1130578972766392471"));
        str.AppendLine("Github".Url("https://github.com/NoManaRPG/NoManaRPG"));
        embed.WithDescription(str.ToString());
        var proc = Process.GetCurrentProcess();
        var mem = proc.PrivateMemorySize64;
        embed.AddField("Memoria usada", $"{(mem / 1024) / 1024} Mb", true);
        var str2 = new StringBuilder();
        str2.Append($"{GuildAvailableEvent.Guildas} guildas");
        str2.Append($" com {GuildAvailableEvent.Membros} membros no total");
        embed.AddField("Estou em", str2.ToString(), true);
        embed.WithColor(DiscordColor.HotPink);
        embed.WithFooter($"Tempo de resposta do bot: {ctx.Client.Ping}ms");
        await ctx.CreateResponseAsync(embed: embed.Build());
    }

    [SlashCommand("cade", "Sumiu?")]
    public async Task StartCommandAsync(InteractionContext ctx)
    {
        var d = new DiscordEmbedBuilder();
        d.WithDescription("É o que parece neh.. mas não, o bot não sumiu e em breve deve voltar com coisas novas! " +
            "Foi recriado o nosso servidor de suporte e caso você queria ficar por" +
            " dentro das novidades que estão chegando, não deixe de acompanhar aqui: https://discord.gg/Ht2uvSr4NW");

        //context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        //// do your stuff here
        //context.EditResponseAsync(new DiscordWebhookBuilder());
        // you cannot use an InteractionResponseBuilder here.
        // If you need to send ephemeral responses, pass an empty
        // DiscordInteractionResponseBuilder to the CreateResponseAsync
        // and just call .AsEphemeral() on it

        await ctx.CreateResponseAsync(d, true);
    }
}
