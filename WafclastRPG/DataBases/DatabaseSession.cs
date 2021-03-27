using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Entities;

namespace WafclastRPG.DataBases
{
    public class DatabaseSession : IDisposable
    {
        private IClientSessionHandle Session { get; }
        private Database Database { get; }

        public DatabaseSession(IClientSessionHandle session, Database database)
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
        public async Task<WafclastPlayer> FindPlayerAsync(DiscordUser user) => await FindPlayerAsync(user.Id);

        /// <summary>
        /// Procura no banco de dados pelo o Id informado.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>O BotJogador ou null</returns>
        public async Task<WafclastPlayer> FindPlayerAsync(ulong id)
        {
            var jogador = await Database.CollectionJogadores.Find(Session, x => x.Id == id).FirstOrDefaultAsync();
            if (jogador == null)
                return null;
            jogador.banco = this;
            return jogador;
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

        #region Mapa

        public async Task<WafclastMapa> FindMapAsync(ulong id)
        {
            var map = await Database.CollectionMaps.Find(Session, x => x.Id == id).FirstOrDefaultAsync();
            if (map == null)
                return null;
            return map;
        }

        public Task SaveMapAsync(WafclastMapa map)
            => Database.CollectionMaps.ReplaceOneAsync(x => x.Id == map.Id, map);

        #endregion
    }
}
