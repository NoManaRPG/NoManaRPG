using MongoDB.Driver;
using System.Threading.Tasks;

namespace WafclastRPG.Repositories.Interfaces {
  public interface IMongoSession {
    IClientSessionHandle Session { get; }
    Task<IClientSessionHandle> StartSession();
  }
}
