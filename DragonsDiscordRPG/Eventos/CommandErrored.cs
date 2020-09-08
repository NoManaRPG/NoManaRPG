using DragonsDiscordRPG.Extensoes;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Eventos
{
    public static class CommandErrored
    {
        //Envia mensagem ao receber um erro.
        public static async Task EventAsync(CommandErrorEventArgs e)
        {
            CommandContext ctx = e.Context;
            switch (e.Exception)
            {
                case ChecksFailedException ex:
                    if (!(ex.FailedChecks.FirstOrDefault(x => x is CooldownAttribute) is CooldownAttribute my))
                        return;
                    else
                    {
                        TimeSpan t = TimeSpan.FromSeconds(my.GetRemainingCooldown(ctx).TotalSeconds);
                        if (t.Days >= 1)
                            await ctx.RespondAsync($"Aguarde {t.Days} dias e ({t.Hours} horas para usar este comando! {ctx.Member.Mention}.");
                        else if (t.Hours >= 1)
                            await ctx.RespondAsync($"Aguarde {t.Hours} horas e {t.Minutes} minutos para usar este comando! {ctx.Member.Mention}.");
                        else if (t.Minutes >= 1)
                            await ctx.RespondAsync($"Aguarde {t.Minutes} minutos e {t.Seconds} segundos para usar este comando! {ctx.Member.Mention}.");
                        else
                            await ctx.RespondAsync($"Aguarde {t.Seconds} segundos para usar este comando! {ctx.Member.Mention}.");
                    }
                    return;
                case CommandNotFoundException cf:
                    if (e.Command != null)
                        if (e.Command.Name == "ajuda")
                        {
                            DiscordEmoji x = DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:");
                            await ctx.RespondAsync($"{x} | {ctx.User.Mention} o comando {e.Context.RawArgumentString} não existe.*");
                        }
                    return;
                case NotFoundException nx:
                    await ctx.RespondAsync($"{ctx.User.Mention}, usuario não encontrado.");
                    break;
                case UnauthorizedException ux:
                    break;
                case ArgumentException ax:
                    //await ctx.ExecutarComandoAsync("ajuda " + e.Command.Name);
                    break;
                default:
                    e.Context.Client.DebugLogger.LogMessage(LogLevel.Debug, "Erro", $"[{e.Context.User.Username.RemoverAcentos()}({e.Context.User.Id})] tentou usar '{e.Command?.QualifiedName ?? "<comando desconhecido>"}' mas deu erro: {e.Exception.ToString()}\nstack:{e.Exception.StackTrace}\ninner:{e.Exception?.InnerException}.", DateTime.Now);
                    DiscordChannel channel = await ctx.Client.GetChannelAsync(742778666509008956);
                    await ctx.Client.SendMessageAsync(channel, $"[{e.Context.User.Username.RemoverAcentos()}({e.Context.User.Id})] tentou usar '{e.Command?.QualifiedName ?? "<comando desconhecido>"}' mas deu erro: {e.Exception.ToString()}\nstack:{e.Exception.StackTrace}\ninner:{e.Exception?.InnerException}.\n{e.Context.Message.JumpLink}");
                    break;
            }
        }
    }
}
