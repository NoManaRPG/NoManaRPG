using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Exceptions;

namespace WafclastRPG.DataBases {
  public class DatabaseSession : IDisposable {
    public IClientSessionHandle Session { get; }
    public DataBase Database { get; }

    public DatabaseSession(IClientSessionHandle session, DataBase database) {
      Session = session;
      Database = database;
    }

    public Task<Response> WithTransactionAsync(Func<IClientSessionHandle, CancellationToken, Task<Response>> callbackAsync) => Session.WithTransactionAsync(callbackAsync: callbackAsync);

    public async Task<WafclastPlayer> FindPlayerAsync(DiscordUser user, bool errorPlayerNull = true) {
      var player = await Database.CollectionPlayers.Find(Session, x => x.Id == user.Id).FirstOrDefaultAsync();
      if (player == null) {
        if (errorPlayerNull)
          throw new PlayerNotCreated();
      } else
        player.dataSession = this;
      return player;
    }

    public async Task<WafclastPlayer> FindPlayerAsync(CommandContext ctx, bool errorPlayerNull = true) {
      await ctx.TriggerTypingAsync();
      return await FindPlayerAsync(ctx.User, errorPlayerNull);
    }

    /// <summary>
    /// Procura um item do Usuário pelo o Id Global do Item.
    /// </summary>
    /// <param name="globalItemId">Global Item Id</param>
    /// <param name="user">Usuário Id</param>
    /// <returns></returns>
    public Task<WafclastBaseItem> FindItemAsync(int globalItemId, DiscordUser user)
     => Database.CollectionItems.Find(Session, x => x.GlobalItemId == globalItemId && x.PlayerId == user.Id).FirstOrDefaultAsync();

    public Task<WafclastBaseItem> FindItemAsync(string itemName, DiscordUser user)
     => Database.CollectionItems.Find(Session, x => x.Name == itemName && x.PlayerId == user.Id, new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();

    public Task<WafclastFabrication> FindFabricationAsync(string name)
        => Database.CollectionFabrication.Find(Session, x => x.Name == name, new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();

    public Task<WafclastRegion> FindRegionAsync(int id)
      => Database.CollectionRegions.Find(x => x.Id == id).FirstOrDefaultAsync();

    public Task ReplaceAsync(WafclastPlayer jogador)
         => Database.CollectionPlayers.ReplaceOneAsync(Session, x => x.Id == jogador.Id, jogador, new ReplaceOptions { IsUpsert = true });
    public Task ReplaceAsync(WafclastServer server)
      => Database.CollectionGuilds.ReplaceOneAsync(Session, x => x.Id == server.Id, server, new ReplaceOptions { IsUpsert = true });
    public Task ReplaceAsync(WafclastBaseItem item)
      => Database.CollectionItems.ReplaceOneAsync(Session, x => x.Id == item.Id, item, new ReplaceOptions { IsUpsert = true });
    public Task ReplaceAsync(WafclastFabrication fabrication)
    => Database.CollectionFabrication.ReplaceOneAsync(x => x.Name == fabrication.Name, fabrication, new ReplaceOptions { IsUpsert = true });



    public Task InsertAsync(WafclastBaseItem item)
        => Database.CollectionItems.InsertOneAsync(Session, item);
    public Task RemoveAsync(WafclastBaseItem item)
       => Database.CollectionItems.DeleteOneAsync(Session, x => x.Id == item.Id);
    public Task RemoveAsync(WafclastFabrication fabrication)
       => Database.CollectionFabrication.DeleteOneAsync(Session, x => x.Name == fabrication.Name);

    public void Dispose() => Session.Dispose();
  }

  public class Response {
    public DiscordEmbedBuilder Embed { get; set; }
    public string Message { get; set; }

    public Response(DiscordEmbedBuilder embed) => Embed = embed;

    public Response(string message) => Message = message;
    public Response() { }
  }
}
