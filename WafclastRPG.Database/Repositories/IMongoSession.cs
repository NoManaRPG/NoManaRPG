using MongoDB.Driver;
using System.Threading.Tasks;

namespace WafclastRPG.Database.Repositories {
  public interface IMongoSession {
    IClientSessionHandle Session { get; }
    Task<IClientSessionHandle> StartSession();
  }
}
