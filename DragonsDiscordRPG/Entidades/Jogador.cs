using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class Jogador
    {
        [BsonId]
        public ulong Id { get; private set; }
       

        public Jogador(ulong id)
        {
            Id = id;
        }

        public async Task SalvarAsync()
            => await ModuloBanco.ColecaoJogador.ReplaceOneAsync(x => x.Id == Id, this);
    }
}

