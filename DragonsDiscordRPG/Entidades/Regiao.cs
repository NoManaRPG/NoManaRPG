using MongoDB.Bson.Serialization.Attributes;

namespace DragonsDiscordRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class Regiao
    {
        [BsonId]
        public ulong IdVoz { get; set; } //Id canal de voz
        public ulong IdCargoTexto { get; set; } //Id cargo do canal de texto
        public string Nome { get; set; } //Nome categoria
    }
}
