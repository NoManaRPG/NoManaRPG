using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using WafclastRPG.Bot.Entidades;
using WafclastRPG.Bot.Extensions;
using WafclastRPG.Game.Entities;
using WafclastRPG.Game.Metadata;

namespace WafclastRPG.Bot
{
    public class Banco : BotAsyncLock
    {
        private IMongoClient Client { get; }
        private IMongoDatabase Database { get; }
        private IMongoCollection<WafclastPlayer> Jogadores { get; }
        private IMongoCollection<BotServidor> Servidores { get; }

        private ConcurrentDictionary<ulong, bool> PrefixLocker { get; }
        private ReplaceOptions _replaceOptions = new ReplaceOptions { IsUpsert = true };

        public Banco()
        {
            Client = new MongoClient("mongodb://localhost");
#if DEBUG

            Database = Client.GetDatabase("WafclastV2Debug");
#else
            Database = Client.GetDatabase("WafclastV2");
#endif
            Jogadores = Database.CriarCollection<WafclastPlayer>();
            Servidores = Database.CriarCollection<BotServidor>();
            PrefixLocker = new ConcurrentDictionary<ulong, bool>();

            new Data(this);

            #region Usar no futuro
            //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
            //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
            //ColecaoJogador.Indexes.CreateOne(indexModel);
            #endregion
        }

        #region Interactivity

        public bool IsExecutingInteractivity(ulong userId) => PrefixLocker.TryGetValue(userId, out _);
        public void StopExecutingInteractivity(ulong userId) => PrefixLocker.TryRemove(userId, out _);
        public void StartExecutingInteractivity(ulong userId) => PrefixLocker.TryAdd(userId, true);

        #endregion
        #region Server

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
        public Task DeleteServerAsync(ulong serverId)
            => Servidores.DeleteOneAsync(x => x.Id == serverId);

        public Task ReplaceServerAsync(ulong serverId, BotServidor server)
            => Servidores.ReplaceOneAsync(x => x.Id == serverId, server, _replaceOptions);

        #endregion
        #region Jogador

        public async Task<BotJogador> GetJogadorAsync(ulong id, DiscordUser user)
        {
            var jogador = await Jogadores.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (jogador == null)
                jogador = new WafclastPlayer(id);
            return new BotJogador(jogador, this, user);
        }

        public Task ReplaceJogadorAsync(ulong id, WafclastPlayer jogador)
            => Jogadores.ReplaceOneAsync(x => x.Id == id, jogador, _replaceOptions);

        public Task InsertJogadorAsync(WafclastPlayer jogador)
            => Jogadores.InsertOneAsync(jogador);

        #endregion
    }
}
