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
        private IClientSessionHandle Session { get; }
        private BotDatabase Database { get; }

        public BotDatabaseSession(IClientSessionHandle session, BotDatabase database)
        {
            Session = session;
            Database = database;
        }

        public Task<TResult> WithTransactionAsync<TResult>(Func<IClientSessionHandle, CancellationToken, Task<TResult>> callbackAsync) => Session.WithTransactionAsync(callbackAsync: callbackAsync);
        public void Dispose() => Session.Dispose();

        #region Jogador

        /// <summary>
        /// Procura no banco de dados pelo o user informado.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>O BotJogador ou null</returns>
        public async Task<BotJogador> FindPlayerAsync(DiscordUser user) => await FindPlayerAsync(user.Id);

        /// <summary>
        /// Procura no banco de dados pelo o Id informado.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>O BotJogador ou null</returns>
        public async Task<BotJogador> FindPlayerAsync(ulong id)
        {
            var jogador = await Database.CollectionJogadores.Find(Session, x => x.Id == id).FirstOrDefaultAsync();
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
        public Task ReplacePlayerAsync(WafclastPlayer jogador) => Database.CollectionJogadores.ReplaceOneAsync(Session, x => x.Id == jogador.Id, jogador, new ReplaceOptions { IsUpsert = true });

        public Task InsertPlayerAsync(WafclastPlayer jogador) => Database.CollectionJogadores.InsertOneAsync(Session, jogador);

        #endregion

        #region Monstro

        public async Task<WafclastMonster> FindMonsterAsync(ulong id)
        {
            var monster = await Database.CollectionMonsters.Find(Session, x => x.Id == id).FirstOrDefaultAsync();
            if (monster == null)
                return null;
            return monster;
        }

        public Task SaveMonsterAsync(WafclastMonster monster)
            => Database.CollectionMonsters.ReplaceOneAsync(x => x.Id == monster.Id, monster);


        #endregion
    }
}
