using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Bot.Entidades;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Bot.Database
{
    public class BotDatabaseSession : IDisposable
    {
        public IClientSessionHandle Session { get; }
        private IMongoCollection<WafclastPlayer> CollectionJogadores { get; }

        public BotDatabaseSession(IClientSessionHandle clientSessionHandle,
            IMongoCollection<WafclastPlayer> collectionJogadores)
        {
            Session = clientSessionHandle;
            CollectionJogadores = collectionJogadores;
        }

        public Task<TResult> WithTransactionAsync<TResult>(Func<IClientSessionHandle, CancellationToken, Task<TResult>> callbackAsync, TransactionOptions transactionOptions = null, CancellationToken cancellationToken = default) => Session.WithTransactionAsync(callbackAsync: callbackAsync);
        public void Dispose() => Session.Dispose();

        #region Jogador

        /// <summary>
        /// Procura no banco de dados pelo o user informado.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>O BotJogador ou null</returns>
        public async Task<BotJogador> FindJogadorAsync(DiscordUser user) => await FindJogadorAsync(user.Id);

        /// <summary>
        /// Procura no banco de dados pelo o Id informado.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>O BotJogador ou null</returns>
        public async Task<BotJogador> FindJogadorAsync(ulong id)
        {
            var jogador = await CollectionJogadores.Find(Session, x => x.Id == id).FirstOrDefaultAsync();
            if (jogador == null)
                return null;
            return new BotJogador(jogador, this);
        }

        /// <summary>
        /// Substitui as informações anteriores pela nova informada.
        /// <br><br>Caso não tenha o jogador salvo, cria um novo.</br></br>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jogador"></param>
        /// <returns></returns>
        public Task ReplaceJogadorAsync(WafclastPlayer jogador) => CollectionJogadores.ReplaceOneAsync(Session, x => x.Id == jogador.Id, jogador, new ReplaceOptions { IsUpsert = true });

        public Task InsertJogadorAsync(WafclastPlayer jogador) => CollectionJogadores.InsertOneAsync(Session, jogador);

    }
    #endregion
}
