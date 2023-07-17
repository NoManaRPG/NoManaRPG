// This file is part of NoManaRPG project.

using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using NoManaRPG.Database.Response;

namespace NoManaRPG.Database.Repositories;

public class MongoSession : IDisposable
{
    public IClientSessionHandle Session { get; private set; }
    private readonly MongoDbContext _context;

    public MongoSession(MongoDbContext context)
    {
        this._context = context;
    }

    public async Task<MongoSession> StartSessionAsync()
    {
        this.Session = await this._context.Client.StartSessionAsync();
        return this;
    }
    public Task<IResponse> WithTransactionAsync(Func<IClientSessionHandle, CancellationToken, Task<IResponse>> callbackAsync)
             => this.Session.WithTransactionAsync(callbackAsync: callbackAsync);
    public void Dispose()
    {
        if (this.Session != null)
            this.Session.Dispose();
    }
}
