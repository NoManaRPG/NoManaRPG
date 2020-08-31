using DragonsDiscordRPG.Entidades;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Extensoes
{
    public static class DiscordUserExtension
    {
        public static Task<Jogador> GetJogador(this DiscordUser discordUser)
            => ModuloBanco.ColecaoJogador.Find(x => x.Id == discordUser.Id).FirstOrDefaultAsync();
    }
}
