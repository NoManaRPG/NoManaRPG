using DragonsDiscordRPG.Entidades;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Eventos
{
    public static class VoiceStateUpdated
    {
        public static async Task EventAsync(VoiceStateUpdateEventArgs e)
        {
            DiscordMember member;
            DiscordRole role;
            Regiao reg;

            if (e.Channel != null)
            {
                reg = await ModuloBanco.ColecaoRegiao.Find(x => x.IdVoz == e.Channel.Id).FirstAsync();
                if (reg != null)
                {
                    member = await e.Guild.GetMemberAsync(e.User.Id);
                    role = e.Guild.GetRole(reg.IdCargoTexto);
                    var f = new UpdateDefinitionBuilder<Jogador>().Set(x => x.IdVoz, reg.IdVoz);
                    await member.GrantRoleAsync(role);
                    await ModuloBanco.ColecaoJogador.UpdateOneAsync(x => x.Id == e.User.Id,
                        new UpdateDefinitionBuilder<Jogador>().Set(x => x.IdVoz, reg.IdVoz),
                        new UpdateOptions() { IsUpsert = true });

                }
                return;
            }

            member = await e.Guild.GetMemberAsync(e.User.Id);
            Jogador jogador = await ModuloBanco.ColecaoJogador.Find(x => x.Id == e.User.Id).FirstAsync();
            reg = await ModuloBanco.ColecaoRegiao.Find(x => x.IdVoz == jogador.IdVoz).FirstAsync();
            role = e.Guild.GetRole(reg.IdCargoTexto);
            await member.RevokeRoleAsync(role);
            return;
        }
    }
}
