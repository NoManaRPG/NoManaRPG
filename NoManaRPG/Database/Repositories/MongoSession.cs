// This file is part of NoManaRPG project.

using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace NoManaRPG.Database.Repositories;

public class MongoSession : IDisposable
{
    public IClientSessionHandle Session { get; private set; }
    private readonly DbContext _context;

    public MongoSession(DbContext context)
    {
        this._context = context;
    }

    public async Task<MongoSession> StartSessionAsync()
    {
        this.Session = await this._context.Client.StartSessionAsync();
        return this;
    }

    public void Dispose()
    {
        if (this.Session != null)
            this.Session.Dispose();
    }
}
