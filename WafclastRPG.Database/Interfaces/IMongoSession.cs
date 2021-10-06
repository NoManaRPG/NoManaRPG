// This file is part of the WafclastRPG project.

using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using WafclastRPG.Database.Response;

namespace WafclastRPG.Database.Interfaces
{
    public interface IMongoSession : IDisposable
    {
        [Obsolete("Session is obsolete, please use Get() instead.")]
        IClientSessionHandle Session { get; }
        IClientSessionHandle Get();
        Task<IMongoSession> StartSessionAsync();
        Task<IResponse> WithTransactionAsync(Func<IClientSessionHandle, CancellationToken, Task<IResponse>> callbackAsync);
    }
}
