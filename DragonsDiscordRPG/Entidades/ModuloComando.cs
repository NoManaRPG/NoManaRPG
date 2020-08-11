using DragonsDiscordRPG.Comandos;
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

namespace DragonsDiscordRPG.Entidades
{
    public class ModuloComandos
    {
        public static CommandsNextExtension Comandos { get; private set; }

        public ModuloComandos(CommandsNextConfiguration ccfg, DiscordClient client)
        {
            Comandos = client.UseCommandsNext(ccfg);
            Comandos.CommandExecuted += ComandoExecutado;
            Comandos.CommandErrored += ComandoAconteceuErro;
            // Comandos.SetHelpFormatter<IAjudaComando>();

            Comandos.RegisterCommands<Gmae>();
            Comandos.RegisterCommands<ComandosAdministrativos>();
        }

        //Envia mensagem ao receber um erro.
        private async Task ComandoAconteceuErro(CommandErrorEventArgs e)
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
                    var MundoZayn = await ModuloCliente.Client.GetGuildAsync(420044060720627712);
                    var CanalRPG = MundoZayn.GetChannel(600736364484493424);
                    await CanalRPG.SendMessageAsync($"[{e.Context.User.Username.RemoverAcentos()}({e.Context.User.Id})] tentou usar '{e.Command?.QualifiedName ?? "<comando desconhecido>"}' mas deu erro: {e.Exception.ToString()}\nstack:{e.Exception.StackTrace}\ninner:{e.Exception?.InnerException}.\n{e.Context.Message.JumpLink}");
                    break;
            }
        }

        private Task ComandoExecutado(CommandExecutionEventArgs e)
        {
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Info, $"({e.Context.Guild.Id}) {e.Context.Guild.Name.RemoverAcentos()}", $"({e.Context.User.Id}) {e.Context.User.Username.RemoverAcentos()} executou '{e.Command.QualifiedName}'", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
