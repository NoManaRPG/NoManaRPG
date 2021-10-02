// This file is part of the WafclastRPG project.

using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using WafclastRPG.Database.Response;

namespace WafclastRPG.Database.Repositories
{
    public class MongoSession : IMongoSession
    {
        public IClientSessionHandle Session { get; private set; }
        private readonly MongoDbContext _context;

        public MongoSession(MongoDbContext context)
        {
            this._context = context;
        }

        public async Task<IMongoSession> StartSessionAsync()
        {
            this.Session = await this._context.Client.StartSessionAsync();
            return this;
        }

        public Task<IResponse> WithTransactionAsync(Func<IClientSessionHandle, CancellationToken, Task<IResponse>> callbackAsync)
                 => this.Session.WithTransactionAsync(callbackAsync: callbackAsync);
        public void Dispose() => this.Session.Dispose();
    }
}
