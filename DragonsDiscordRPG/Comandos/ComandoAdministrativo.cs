using DragonsDiscordRPG.Entidades;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MongoDB.Driver;
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
    }
}
