// This file is part of NoManaRPG project.

using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using NoManaRPG.Exceptions;
using NoManaRPG.Extensions;
using NoManaRPG.Interactivity;

namespace NoManaRPG.DiscordEvents;

public static class CommandErroredEvent
{
    //Envia mensagem ao receber um erro.
    public static async Task EventAsync(CommandsNextExtension cnt, CommandErrorEventArgs e)
    {
        CommandContext ctx = e.Context;
        switch (e.Exception)
        {
            case NotFoundException nfe:
                await ctx.RespondAsync($"{ctx.User.Mention}, o usuario informado não foi encontrado.");
                break;
            case UnauthorizedException ux:
                e.Context.Client.Logger.LogDebug(new EventId(601, "Comando Invalido"), $"[{e.Context.User.Username.RemoverAcentos()}({e.Context.User.Id})] tentou usar '{e.Command?.QualifiedName ?? "<comando desconhecido>"}' mas deu erro: {e.Exception}\ninner:{e.Exception?.InnerException}.", DateTime.Now);
                break;
            case ServerErrorException se:
                e.Context.Client.Logger.LogInformation(new EventId(601, "Comando Invalido"), $"[{e.Context.User.Username.RemoverAcentos()}({e.Context.User.Id})] tentou usar '{e.Command?.QualifiedName ?? "<comando desconhecido>"}' mas deu erro: {e.Exception}\ninner:{e.Exception?.InnerException}.", DateTime.Now);
                break;
            case Newtonsoft.Json.JsonReaderException _:
                e.Context.Client.Logger.LogInformation(new EventId(602, "Discord Problem"), $"[{e.Context.User.Username.RemoverAcentos()}({e.Context.User.Id})] tentou usar '{e.Command?.QualifiedName ?? "<comando desconhecido>"}' mas discord está lento.", DateTime.Now);
                break;
            case ArgumentException ae:
                if (ae.Message == "Could not find a suitable overload for the command.")
                {
                    await ctx.TriggerTypingAsync();
                    var cmd = ctx.CommandsNext.FindCommand($"ajuda {e.Command.QualifiedName}", out var args);
                    if (cmd == null)
                    {
                        await ctx.RespondAsync("Comando não encontrado");
                        return;
                    }

                    var cfx = ctx.CommandsNext.CreateFakeContext(ctx.User, ctx.Channel, "", ".", cmd, args);
                    await ctx.CommandsNext.ExecuteCommandAsync(cfx);
                }
                else
                    e.Context.Client.Logger.LogDebug(new EventId(601, "Argument Error"), $"[{e.Context.User.Username.RemoverAcentos()}({e.Context.User.Id})] tentou usar '{e.Command?.QualifiedName ?? "<comando desconhecido>"}' mas deu erro: {e.Exception}\ninner:{e.Exception?.InnerException}.", DateTime.Now);
                break;
            case PlayerNotCreatedException pne:
                await ctx.RespondAsync(pne.Message);
                break;
            case AnswerTimeoutException ate:
                await ctx.RespondAsync(ate.Message);
                break;
            default:
                e.Context.Client.Logger.LogDebug(new EventId(601, "Command Error"), $"[{e.Context.User.Username.RemoverAcentos()}({e.Context.User.Id})] tentou usar '{e.Command?.QualifiedName ?? "<comando desconhecido>"}' mas deu erro: {e.Exception}\ninner:{e.Exception?.InnerException}.", DateTime.Now);
                break;
        }
        var usersBlocked = ctx.Services.GetService<UsersBlocked>();
        usersBlocked.UnblockUser(ctx);
    }
}
