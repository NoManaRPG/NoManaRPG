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
using WafclastRPG.Extensions;
using System.IO;
using DSharpPlus;
using System.Text;
using WafclastRPG.DataBases;

namespace WafclastRPG.DiscordEvents
{
    public static class CommandErroredEvent
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
                                await ctx.RespondAsync($"{ctx.Member.Mention}, você precisa esperar {tempo.Seconds} segundos para usar este comando!");
                                break;
                        };
                    }
                    break;
                case CommandNotFoundException cnfe:
                    if (e.Command?.Name == "ajuda")
                        await ctx.RespondAsync($":no_entry_sign: | {ctx.User.Mention} o comando {e.Context.RawArgumentString} não existe.*");
                    break;
                case NotFoundException nfe:
                    await ctx.RespondAsync($"{ctx.User.Mention}, o usuario informado não foi encontrado.");
                    break;
                case UnauthorizedException ux:
                    e.Context.Client.Logger.LogDebug(new EventId(601, "Comando Invalido"), $"[{e.Context.User.Username.RemoverAcentos()}({e.Context.User.Id})] tentou usar '{e.Command?.QualifiedName ?? "<comando desconhecido>"}' mas deu erro: {e.Exception}\ninner:{e.Exception?.InnerException}.", DateTime.Now);
                    break;
                case ServerErrorException se:
                    e.Context.Client.Logger.LogDebug(new EventId(601, "Comando Invalido"), $"[{e.Context.User.Username.RemoverAcentos()}({e.Context.User.Id})] tentou usar '{e.Command?.QualifiedName ?? "<comando desconhecido>"}' mas deu erro: {e.Exception}\ninner:{e.Exception?.InnerException}.", DateTime.Now);
                    break;
                case Newtonsoft.Json.JsonReaderException _:
                    e.Context.Client.Logger.LogDebug(new EventId(602, "Discord Problem"), $"[{e.Context.User.Username.RemoverAcentos()}({e.Context.User.Id})] tentou usar '{e.Command?.QualifiedName ?? "<comando desconhecido>"}' mas discord está lento.", DateTime.Now);
                    break;
                default:
                    e.Context.Client.Logger.LogDebug(new EventId(601, "Command Error"), $"[{e.Context.User.Username.RemoverAcentos()}({e.Context.User.Id})] tentou usar '{e.Command?.QualifiedName ?? "<comando desconhecido>"}' mas deu erro: {e.Exception}\ninner:{e.Exception?.InnerException}.", DateTime.Now);

                    //var embed = new DiscordEmbedBuilder();
                    //var str = new StringBuilder();
                    //var channel = await ctx.Client.GetChannelAsync(742778666509008956);
                    //var error = e.Exception.ToString();

                    //if (error.Length >= 2000)
                    //{
                    //    str.AppendLine($"ID do Usuario: {e.Context.User.Id}");
                    //    str.AppendLine($"ID do Servidor: {e.Context.Guild.Id}");
                    //    str.AppendLine("Pela mensagem ser muito grande, foi anexado um log acima.");
                    //    str.AppendLine($"({Formatter.MaskedUrl("MENSAGEM", e.Context.Message.JumpLink)})");

                    //    embed.WithAuthor($"{e.Context.User.Username}", e.Context.User.AvatarUrl, e.Context.User.AvatarUrl);
                    //    embed.WithTitle($"Comando executado: {e.Command?.QualifiedName ?? "<comando desconhecido>"}");
                    //    embed.WithTimestamp(DateTime.Now);
                    //    embed.WithDescription(str.ToString());

                    //    var stream = GenerateStreamFromString(error);
                    //    await channel.SendFileAsync("Log.txt", stream, embed: embed.Build());
                    //}
                    //else
                    //{
                    //    str.AppendLine($"ID do Usuario: {e.Context.User.Id}");
                    //    str.AppendLine($"ID do Servidor: {e.Context.Guild.Id}");
                    //    str.AppendLine(e.Exception.ToString());
                    //    str.AppendLine($"({Formatter.MaskedUrl("MENSAGEM", e.Context.Message.JumpLink)})");

                    //    embed.WithAuthor($"{e.Context.User.Username}", e.Context.User.AvatarUrl, e.Context.User.AvatarUrl);
                    //    embed.WithTitle($"Comando executado: {e.Command?.QualifiedName ?? "<comando desconhecido>"}");
                    //    embed.WithTimestamp(DateTime.Now);
                    //    embed.WithDescription(str.ToString());
                    //    await channel.SendMessageAsync(embed: embed.Build());
                    //}
                    //await ctx.RespondAsync("Aconteceu um erro! Reporte no servidor oficial o que você fez!");
                    break;
            }
            var banco = (DataBase)ctx.Services.GetService(typeof(DataBase));
            banco.StopExecutingInteractivity(ctx.User.Id);
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
