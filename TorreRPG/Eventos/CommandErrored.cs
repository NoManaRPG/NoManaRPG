using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TorreRPG.Eventos
{
    public static class CommandErrored
    {
        //Envia mensagem ao receber um erro.
        public static async Task EventAsync(CommandsNextExtension cnt, CommandErrorEventArgs e)
        {
            CommandContext ctx = e.Context;
            switch (e.Exception)
            {
                case ChecksFailedException cfe:
                    if (cfe.FailedChecks.FirstOrDefault(x => x is CooldownAttribute) is CooldownAttribute ca)
                    {
                        TimeSpan tempo = TimeSpan.FromSeconds(ca.GetRemainingCooldown(ctx).TotalSeconds);
                        switch (tempo)
                        {
                            case TimeSpan n when (n.Days >= 1):
                                await ctx.RespondAsync($"Aguarde {tempo.Days} dias e {tempo.Hours} horas para usar este comando! {ctx.Member.Mention}.");
                                break;
                            case TimeSpan n when (n.Hours >= 1):
                                await ctx.RespondAsync($"Aguarde {tempo.Hours} horas e {tempo.Minutes} minutos para usar este comando! {ctx.Member.Mention}.");
                                break;
                            case TimeSpan n when (n.Minutes >= 1):
                                await ctx.RespondAsync($"Aguarde {tempo.Minutes} minutos e {tempo.Seconds} segundos para usar este comando! {ctx.Member.Mention}.");
                                break;
                            default:
                                await ctx.RespondAsync($"Aguarde {tempo.Seconds} segundos para usar este comando! {ctx.Member.Mention}.");
                                break;
                        };
                    }
                    break;
                case CommandNotFoundException cnfe:
                    if (e.Command?.Name == "ajuda")
                    {
                        DiscordEmoji x = DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:");
                        await ctx.RespondAsync($"{x} | {ctx.User.Mention} o comando {e.Context.RawArgumentString} não existe.*");
                    }
                    break;
                case NotFoundException nfe:
                    await ctx.RespondAsync($"{ctx.User.Mention}, o usuario informado não foi encontrado.");
                    break;
                case UnauthorizedException ux:
                    break;
                case ServerErrorException se:
                    break;
                case MongoCommandException mce:
                    await ctx.RespondAsync($"{ctx.User.Mention}, o último comando não foi processado corretamente! Tente executar os comandos mais lentamente!");
                    break;
                default:
                    e.Context.Client.Logger.LogDebug(new EventId(601, "Comando Invalido"), $"[{e.Context.User.Username.RemoverAcentos()}({e.Context.User.Id})] tentou usar '{e.Command?.QualifiedName ?? "<comando desconhecido>"}' mas deu erro: {e.Exception}\ninner:{e.Exception?.InnerException}.", DateTime.Now);

                    DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
                    embed.WithAuthor($"{e.Context.User.Username}({e.Context.User.Id})", null, e.Context.User.AvatarUrl);
                    embed.WithTitle($"{e.Command?.QualifiedName ?? "<comando desconhecido>"}");
                    embed.WithDescription($"[{e.Exception}]({e.Context.Message.JumpLink})");
                    embed.WithTimestamp(DateTime.Now);

                    DiscordChannel channel = await ctx.Client.GetChannelAsync(742778666509008956);
                    await ctx.Client.SendMessageAsync(channel, embed: embed.Build());
                    await ctx.RespondAsync("Aconteceu um erro!", embed: embed.Build());
                    break;
            }
        }
    }
}
