using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using System.Linq;
using System.Text;
using System;
using System.Diagnostics;
using static WafclastRPG.Bot.Utilities;

namespace WafclastRPG.Bot.Comandos.Exibir
{
    public class InfoCommand : BaseCommandModule
    {
        public BotInfo botInfo;

        [Command("info")]
        [Description("Exibe informações sobre o bot, memoria usada, quantia de servidores etc.")]
        public async Task InfoCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor(ctx.Client.CurrentUser.Username + " v1.0", "https://discord.gg/MAR4NFq", ctx.Client.CurrentUser.AvatarUrl);
            embed.WithThumbnail(ctx.Client.CurrentUser.AvatarUrl);
            embed.WithTitle("RPG para quem usa muito o discord!");
            var criador = ctx.Client.CurrentApplication.Owners.First();
            var str = new StringBuilder();
            str.AppendLine(FormatarURL("Servidor Oficial", "https://discord.gg/MAR4NFq"));
            str.AppendLine(FormatarURL("Vote no bot", "https://top.gg/bot/732598033962762402"));
            str.AppendLine(FormatarURL("Proximos updates", "https://github.com/WafclastRPG/WafclastRPG/issues"));
            str.AppendLine(FormatarURL("Github", "https://github.com/WafclastRPG/WafclastRPG"));
            embed.WithDescription(str.ToString());
            embed.AddField("Apoia.se", FormatarURL("Doe R$1 real para que o desenvolvimento do bot não pare!", "https://apoia.se/wafclastrpg"));     
            embed.AddField("Tempo ativo", $"Online por: **{(DateTime.Now - botInfo.TempoAtivo).Days} dias, {(DateTime.Now - botInfo.TempoAtivo).Hours} horas e {(DateTime.Now - botInfo.TempoAtivo).Minutes} minutos.**", true);
            Process proc = Process.GetCurrentProcess();
            var mem = proc.PrivateMemorySize64;
            embed.AddField("Memoria usada", $"{(mem / 1024) / 1024} Mb", true);
            var str2 = new StringBuilder();
            str2.Append($"{botInfo.Guildas} guildas");
            str2.Append($" com {botInfo.Membros} membros no total");
            embed.AddField("Estou em", str2.ToString(), true);
            embed.WithColor(DiscordColor.HotPink);
            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
