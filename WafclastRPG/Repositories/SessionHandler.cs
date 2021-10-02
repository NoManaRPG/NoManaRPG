// This file is part of the WafclastRPG project.

using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using WafclastRPG.Database.Response;

namespace WafclastRPG.Entities
{
    public class SessionHandler : IDisposable
    {
        public IClientSessionHandle Session { get => this._session; }
        private IClientSessionHandle _session;

        public SessionHandler(IClientSessionHandle session)
        {
            this._session = session;
        }

        public Task<IResponse> WithTransactionAsync(Func<IClientSessionHandle, CancellationToken, Task<IResponse>> callbackAsync)
          => this._session.WithTransactionAsync(callbackAsync: callbackAsync);
        public void Dispose()
          => this._session.Dispose();
    }
}
