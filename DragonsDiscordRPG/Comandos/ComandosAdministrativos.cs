using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Game.Entidades;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;
using static DragonsDiscordRPG.Entidades.ModuloBanco;

namespace DragonsDiscordRPG.Comandos
{
    public class ComandosAdministrativos : BaseCommandModule
    {
        [Command("linkar")]
        [RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
        public async Task LinkarAsync(CommandContext ctx, DiscordRole cargo)
        {
            if (ctx.Member.VoiceState == null)
            {
                await ctx.RespondAsync("Entre em um canal de voz antes!");
                return;
            }

            var reg = ColecaoRegiao.Find(x => x.IdVoz == ctx.Member.VoiceState.Channel.Id).FirstOrDefault();
            if (reg == null)
                reg = new Regiao()
                {
                    IdVoz = ctx.Member.VoiceState.Channel.Id,
                    IdCargoTexto = cargo.Id,
                    Nome = ctx.Channel.Parent.Name
                };
            else
            {
                await ctx.RespondAsync("Canal de voz já linkado!");
                return;
            }

            await ColecaoRegiao.InsertOneAsync(reg);
            await ctx.RespondAsync("Linkado!");
        }
    }
}
