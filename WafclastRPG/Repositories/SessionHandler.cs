using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Commands.CommandResponse;

namespace WafclastRPG.Entities {
  public class SessionHandler : IDisposable {
    public IClientSessionHandle Session { get => _session; }
    private IClientSessionHandle _session;

    public SessionHandler(IClientSessionHandle session) {
      _session = session;
    }

    public Task<IResponse> WithTransactionAsync(Func<IClientSessionHandle, CancellationToken, Task<IResponse>> callbackAsync)
      => _session.WithTransactionAsync(callbackAsync: callbackAsync);
    public void Dispose()
      => _session.Dispose();
  }
}
