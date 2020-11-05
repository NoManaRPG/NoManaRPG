using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Extensoes;
using WafclastRPG.Game.Metadata;

namespace WafclastRPG.Game
{
    public class Banco
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }
        public IMongoCollection<WafclastJogador> Jogadores { get; }

        public ConcurrentDictionary<ulong, SemaphoreSlim> Baldes { get; }

        public Banco()
        {
            Client = new MongoClient();
            Database = Client.GetDatabase("Wafclast");
            Jogadores = Database.CriarCollection<WafclastJogador>();
            Baldes = new ConcurrentDictionary<ulong, SemaphoreSlim>();

            new Data();

            #region Usar no futuro
            //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
            //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
            //ColecaoJogador.Indexes.CreateOne(indexModel);
            #endregion
        }

        public SemaphoreSlim GetBalde(ulong id)
        {
            if (!Baldes.TryGetValue(id, out var balde))
            {
                balde = new SemaphoreSlim(1, 1);
                Baldes.AddOrUpdate(id, balde, (k, v) => balde);
            }
            return balde;
        }

        public Task<WafclastJogador> GetJogadorAsync(ulong id)
              => Jogadores.Find(x => x.Id == id).FirstOrDefaultAsync();

        public Task ReplaceJogadorAsync(ulong id, WafclastJogador jogador)
            => Jogadores.ReplaceOneAsync(x => x.Id == id, jogador);

        public async Task AddJogadorAsync(WafclastJogador jogador)
        {
            if (jogador == null)
                throw new ArgumentNullException("jogador", "Jogador não pode ser nulo");
            await Jogadores.InsertOneAsync(jogador);
        }
    }
}
