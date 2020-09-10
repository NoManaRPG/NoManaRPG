using DragonsDiscordRPG.Entidades;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
{
    public class ComandoAdministrativo : BaseCommandModule
    {
        [Command("purge")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task PurgeAsync(CommandContext ctx, int quantidade)
            => await ctx.Channel.DeleteMessagesAsync(await ctx.Channel.GetMessagesAsync(quantidade + 1));

        [Command("atualizar")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task Atualizar(CommandContext ctx)
        {
            FilterDefinition<RPJogador> filter = FilterDefinition<RPJogador>.Empty;
            FindOptions<RPJogador> options = new FindOptions<RPJogador>
            {
                BatchSize = 8,
                NoCursorTimeout = false
            };

            using (IAsyncCursor<RPJogador> cursor = await ModuloBanco.ColecaoJogador.FindAsync(filter, options))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<RPJogador> usuarios = cursor.Current;

                    foreach (RPJogador user in usuarios)
                    {
                        await ModuloBanco.ColecaoJogador.ReplaceOneAsync(x => x.Id == user.Id, user);
                    }
                }
            }
            await ctx.RespondAsync("Banco foi atualizado com sucesso!");
        }

        [Command("sudo")]
        [RequireOwner]
        public async Task Sudo(CommandContext ctx, DiscordUser member, [RemainingText] string command)
        {
            await ctx.TriggerTypingAsync();
            var invocation = command.Substring(1);
            var cmd = ctx.CommandsNext.FindCommand(invocation, out var args);
            if (cmd == null)
            {
                await ctx.RespondAsync("Comando não encontrado");
                return;
            }

            var cfx = ctx.CommandsNext.CreateFakeContext(member, ctx.Channel, "", "!", cmd, args);
            await ctx.CommandsNext.ExecuteCommandAsync(cfx);
        }

        [Command("restaurarPot")]
        [RequireOwner]
        public async Task GivePot(CommandContext ctx, DiscordUser member)
        {
            try
            {
                using (var session = await ModuloBanco.Cliente.StartSessionAsync())
                {
                    BancoSession banco = new BancoSession(session);
                    RPJogador jogador = await banco.GetJogadorAsync(ctx);
                    RPPersonagem personagem = jogador.Personagem;

                    personagem.Pocoes[0].AddCarga(double.MaxValue);

                    await banco.EditJogadorAsync(jogador);
                    await session.CommitTransactionAsync();
                    await ctx.RespondAsync($"Poções restaurada para {member.Mention}!");

                }
            }
            catch (Exception ex)
            {
                await MensagensStrings.ComandoSendoProcessado(ctx);
                throw ex;
            }
        }
    }
}
