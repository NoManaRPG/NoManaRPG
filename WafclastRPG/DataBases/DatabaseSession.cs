using DSharpPlus.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.Monsters;

namespace WafclastRPG.DataBases
{
    public class DatabaseSession : IDisposable
    {
        private IClientSessionHandle Session { get; }
        private Database ds { get; }

        public DatabaseSession(IClientSessionHandle session, Database database)
        {
            Session = session;
            ds = database;
        }

        public Task<TResult> WithTransactionAsync<TResult>(Func<IClientSessionHandle, CancellationToken, Task<TResult>> callbackAsync) => Session.WithTransactionAsync(callbackAsync: callbackAsync);
        public void Dispose() => Session.Dispose();

        #region Jogador

        /// <summary>
        /// Procura no banco de dados pelo o user informado.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>O BotJogador ou null</returns>
        public Task<WafclastPlayer> FindPlayerAsync(DiscordUser user)
            => FindPlayerAsync(user.Id);

        /// <summary>
        /// Procura no banco de dados pelo o Id informado.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>O BotJogador ou null</returns>
        public async Task<WafclastPlayer> FindPlayerAsync(ulong id)
        {
            var jogador = await ds.CollectionJogadores.Find(Session, x => x.Id == id).FirstOrDefaultAsync();
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
        public Task ReplacePlayerAsync(WafclastPlayer jogador)
            => ds.CollectionJogadores.ReplaceOneAsync(Session, x => x.Id == jogador.Id, jogador, new ReplaceOptions { IsUpsert = true });

        public Task InsertPlayerAsync(WafclastPlayer jogador)
            => ds.CollectionJogadores.InsertOneAsync(Session, jogador);

        #endregion

        #region Monstro

        public Task<WafclastMonster> FindMonsterAsync(string id)
            => ds.CollectionMonsters.Find(Session, x => x.Id == id).FirstOrDefaultAsync();

        public Task SaveMonsterAsync(WafclastMonster monster)
            => ds.CollectionMonsters.ReplaceOneAsync(x => x.Id == monster.Id, monster);

        #endregion

        #region Mapa

        public Task<WafclastMapa> FindMapAsync(ulong id)
            => ds.CollectionMaps.Find(Session, x => x.Id == id).FirstOrDefaultAsync();

        public Task SaveMapAsync(WafclastMapa map)
            => ds.CollectionMaps.ReplaceOneAsync(x => x.Id == map.Id, map);

        #endregion

        #region Itens

        public Task<WafclastBaseItem> FindItemAsync(string name, ulong playerId)
          => ds.CollectionItens.Find(Session, x => x.PlayerId == playerId && x.Name == name).FirstOrDefaultAsync();

        public Task ReplaceItemAsync(WafclastBaseItem item)
            => ds.CollectionItens.ReplaceOneAsync(Session, x => x.Id == item.Id, item, new ReplaceOptions { IsUpsert = true });

        public Task InsertItemAsync(WafclastBaseItem item)
        {
            item.Id = ObjectId.Empty;
            return ds.CollectionItens.InsertOneAsync(Session, item);
        }

        public Task RemoveItemAsync(WafclastBaseItem item)
           => ds.CollectionItens.DeleteOneAsync(Session, x => x.Id == item.Id);

        public Task<WafclastBaseItem> FindItemByNameAsync(string name, ulong playerId)
          => ds.CollectionItens.Find(Session, x => x.PlayerId == playerId && x.Name == name, new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Secondary) }).FirstOrDefaultAsync();

        #endregion
    }
}
