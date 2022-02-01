// This file is part of WafclastRPG project.

using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands
{
    public class InfoCommand : BaseCommandModule
    {

        [Command("info")]
        [Description("Exibe informações sobre o bot, memoria usada, quantia de servidores etc.")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task InfoCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor(ctx.Client.CurrentUser.Username, "https://discord.gg/MAR4NFq", ctx.Client.CurrentUser.AvatarUrl);
            embed.WithThumbnail(ctx.Client.CurrentUser.AvatarUrl);
            embed.WithTitle("RPG para quem usa muito o discord!");
            //var criador = ctx.Client.CurrentApplication.Owners.First();
            var str = new StringBuilder();
            str.AppendLine("Servidor Oficial".Url("https://discord.gg/MAR4NFq"));
            str.AppendLine("Servidor Parceiro".Url("https://discord.gg/7x5YkZY"));
            str.AppendLine();
            str.AppendLine("Vote".Url("https://top.gg/bot/732598033962762402"));
            str.AppendLine("Proximos updates".Url("https://github.com/WafclastRPG/WafclastRPG/issues"));
            str.AppendLine("Atualização recente".Url("https://discord.gg/d6c6XfGCpU"));
            str.AppendLine("Anuncios".Url("https://discord.gg/PYZ5Muj2GT"));
            str.AppendLine("Github".Url("https://github.com/WafclastRPG/WafclastRPG"));
            embed.WithDescription(str.ToString());
            embed.AddField("Convite", "Adicione o bot no seu Servidor!".Url("https://discord.com/api/oauth2/authorize?client_id=732598033962762402&permissions=0&scope=bot"));
            //embed.AddField("Tempo ativo", $"Online por: **{(DateTime.Now - botInfo.TempoAtivo).Days} dias, {(DateTime.Now - botInfo.TempoAtivo).Hours} horas e {(DateTime.Now - botInfo.TempoAtivo).Minutes} minutos.**", true);
            Process proc = Process.GetCurrentProcess();
            var mem = proc.PrivateMemorySize64;
            embed.AddField("Memoria usada", $"{(mem / 1024) / 1024} Mb", true);
            // var str2 = new StringBuilder();
            //  str2.Append($"{botInfo.Guildas} guildas");
            //  str2.Append($" com {botInfo.Membros} membros no total");
            //embed.AddField("Estou em", str2.ToString(), true);
            embed.WithColor(DiscordColor.HotPink);
            embed.WithFooter($"Tempo de resposta do bot: {ctx.Client.Ping}ms");
            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
