// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using MongoDB.Driver;

namespace WafclastRPG.Database.Repositories
{
    public interface IMongoSession
    {
        IClientSessionHandle Session { get; }
        Task<IClientSessionHandle> StartSession();
    }
}
