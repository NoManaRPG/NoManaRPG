// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using MongoDB.Driver;
using WafclastRPG.Database;

namespace WafclastRPG.Repositories
{
    public class MongoSession : IMongoSession
    {
        public IClientSessionHandle Session { get; private set; }
        private readonly MongoDbContext _context;

        public MongoSession(MongoDbContext context)
        {
            this._context = context;
        }

        public async Task<IClientSessionHandle> StartSession()
        {
            this.Session = await this._context.Client.StartSessionAsync();
            return this.Session;
        }
    }
}
