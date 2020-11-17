using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using WafclastRPG.Bot.Entidades;
using WafclastRPG.Bot.Extensoes;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Entidades.Itens;
using WafclastRPG.Game.Metadata;
using WafclastRPG.Game.Metadata.Itens;

namespace WafclastRPG.Bot
{
    public class Banco : BotAsyncLock
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }
        public IMongoCollection<WafclastJogador> Jogadores { get; }
        public IMongoCollection<BotServidor> Servidores { get; }
        public IMongoCollection<WafclastRegiao> Regioes { get; }
        public IMongoCollection<WafclastItem> Itens { get; }

        private readonly ConcurrentDictionary<ulong, bool> PrefixLocker = new ConcurrentDictionary<ulong, bool>();

        public Banco()
        {
            Client = new MongoClient("mongodb://localhost");
            Database = Client.GetDatabase("WafclastBeta");
            Jogadores = Database.CriarCollection<WafclastJogador>();
            Servidores = Database.CriarCollection<BotServidor>();
            Regioes = Database.CriarCollection<WafclastRegiao>();
            Itens = Database.CriarCollection<WafclastItem>();

            new Data(this);

            #region Usar no futuro
            //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
            //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
            //ColecaoJogador.Indexes.CreateOne(indexModel);
            #endregion
        }

        public bool IsExecutingInteractivity(ulong userId) => PrefixLocker.TryGetValue(userId, out _);
        public void StopExecutingInteractivity(ulong userId) => PrefixLocker.TryRemove(userId, out _);
        public void StartExecutingInteractivity(ulong userId) => PrefixLocker.TryAdd(userId, true);

        public async Task<BotJogador> GetJogadorAsync(ulong id)
        {
            var jogador = await Jogadores.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (jogador == null)
            {
                jogador = new WafclastJogador(id);
                jogador.Personagem.Mochila.TryAddItem(Armas.BronzeDaggerAb());
            }
            return new BotJogador(jogador, this);
        }

        public Task ReplaceJogadorAsync(ulong id, WafclastJogador jogador)
            => Jogadores.ReplaceOneAsync(x => x.Id == id, jogador, new ReplaceOptions { IsUpsert = true });

        public async Task InsertJogadorAsync(WafclastJogador jogador)
            => await Jogadores.InsertOneAsync(jogador);

        public async Task<string> GetServerPrefixAsync(ulong serverId, string defaultPrefix)
        {
            var svl = await Servidores.Find(x => x.Id == serverId).FirstOrDefaultAsync();
            if (svl == null)
                return defaultPrefix;
            return svl.Prefix;
        }

        public string GetServerPrefix(ulong serverId, string defaultPrefix)
        {
            var svl = Servidores.Find(x => x.Id == serverId).FirstOrDefault();
            if (svl == null)
                return defaultPrefix;
            return svl.Prefix;
        }

        public Task<WafclastRegiao> GetRegiaoAsync(int id)
            => Regioes.Find(x => x.Id == id).FirstOrDefaultAsync();

        public Task ReplaceRegiaoAsync(WafclastRegiao regiao)
            => Regioes.ReplaceOneAsync(x => x.Id == regiao.Id, regiao, new ReplaceOptions { IsUpsert = true });

        public Task ReplaceItemAsync(WafclastItem item)
          => Itens.ReplaceOneAsync(x => x.ItemId == item.ItemId, item, new ReplaceOptions { IsUpsert = true });

        public Task<WafclastItem> GetItemAsync(int id)
          => Itens.Find(x => x.ItemId == id).FirstOrDefaultAsync();
    }
}
