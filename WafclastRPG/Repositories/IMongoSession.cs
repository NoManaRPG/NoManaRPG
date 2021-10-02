// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using MongoDB.Driver;

namespace WafclastRPG.Repositories
{
    public interface IMongoSession
    {
        IClientSessionHandle Session { get; }
        Task<IClientSessionHandle> StartSession();
    }
}
