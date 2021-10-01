using MongoDB.Driver;
using System.Threading.Tasks;

namespace WafclastRPG.Database.Repositories {
  public class MongoSession : IMongoSession {
    public IClientSessionHandle Session { get; private set; }
    private readonly MongoDbContext _context;

    public MongoSession(MongoDbContext context) {
      _context = context;
    }

    public async Task<IClientSessionHandle> StartSession() {
      Session = await _context.Client.StartSessionAsync();
      return Session;
    }
  }
}
