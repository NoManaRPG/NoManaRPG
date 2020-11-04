using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Extensoes;

namespace WafclastRPG.Game.Services
{
    public class Banco
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }
        public IMongoCollection<RPJogador> Jogadores { get; }

        public Banco()
        {
            Client = new MongoClient();
            Database = Client.GetDatabase("Wafclast");
            Jogadores = Database.CriarCollection<RPJogador>();

            #region Usar no futuro
            //BsonSerializer.RegisterSerializer(typeof(float),
            //    new SingleSerializer(BsonType.Double, new RepresentationConverter(
            //    true, //allow truncation
            //    true // allow overflow, return decimal.MinValue or decimal.MaxValue instead
            //)));


            //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
            //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
            //ColecaoJogador.Indexes.CreateOne(indexModel);
            #endregion
        }

        public Task<RPJogador> GetJogadorAsync(DiscordUser user)
              => Jogadores.Find(x => x.Id == user.Id).FirstOrDefaultAsync();

        public Task<RPJogador> GetJogadorAsync(CommandContext ctx)
           => GetJogadorAsync(ctx.User);

        public async Task<Tuple<bool, RPPersonagem>> VerificarJogador(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            RPJogador jogador = await GetJogadorAsync(ctx);
            if (jogador == null)
            {
                await ctx.RespondAsync($"Bem-vindo! {ctx.User.Mention} antes de começar, crie um personagem digitando `!criar-personagem`.");
                return new Tuple<bool, RPPersonagem>(true, null);
            }
            return new Tuple<bool, RPPersonagem>(false, jogador.Personagem);
        }

        
        public Task<IClientSessionHandle> StartSessionAsync() => Client.StartSessionAsync();
    }
}
