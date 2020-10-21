using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;
using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using TorreRPG.Services;

namespace TorreRPG.Comandos.Acao
{
    public class ComandoSetImage : BaseCommandModule
    {
        public Banco banco;

        [Command("setimage")]
        [Description("Altera a foto do retrato, indique um número de 0 a 13...")]
        public async Task SetImageAsync(CommandContext ctx, int numero = 1)
        {
            // Verifica se existe o jogador,
            var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            if (naoCriouPersonagem) return;

            using (var session = await banco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);

                switch (numero)
                {
                    case 1:
                        jogador.UrlFoto = "https://cdn.discordapp.com/attachments/758139402219159562/758139472927391774/ed02e456967fb64468a6e3eab6349380.png";
                        break;
                    case 2:
                        jogador.UrlFoto = "https://cdn.discordapp.com/attachments/758139402219159562/758140309028470795/dnd_portrait_yvessia_solemnbranch_web.png";
                        break;
                    case 3:
                        jogador.UrlFoto = "https://cdn.discordapp.com/attachments/758139402219159562/758140508488073216/honeycutt_genasibard_finalsm-600x750.png";
                        break;
                    case 4:
                        jogador.UrlFoto = "https://cdn.discordapp.com/attachments/758139402219159562/758142577663803432/92774dd69ef2fe7adfdd0fc3986afebd.png";
                        break;
                    case 5:
                        jogador.UrlFoto = "https://cdn.discordapp.com/attachments/758139402219159562/758142670996242482/Krubio_monk_final.png";
                        break;
                    case 6:
                        jogador.UrlFoto = "https://cdn.discordapp.com/attachments/758139402219159562/758142967332077648/Honeycutt_Tabaxi_finalsm.png";
                        break;
                    case 7:
                        jogador.UrlFoto = "https://cdn.discordapp.com/attachments/758139402219159562/758143053202063411/b3466e895734b14bc4c020bf11615e88.png";
                        break;
                    case 8:
                        jogador.UrlFoto = "https://cdn.discordapp.com/attachments/758139402219159562/758143271842611210/kiri_leonard_vetis_web.png";
                        break;
                    case 9:
                        jogador.UrlFoto = "https://cdn.discordapp.com/attachments/758139402219159562/758144663839703040/Joa_BaernDwarf_finalsm.png";
                        break;
                    case 10:
                        jogador.UrlFoto = "https://cdn.discordapp.com/attachments/758139402219159562/758145489627250718/d7615c419b5ca4b1f6ce569ab70409dc.png";
                        break;
                    case 11:
                        jogador.UrlFoto = "https://cdn.discordapp.com/attachments/758139402219159562/758145499521089546/7ce8f755d2ba24ab02971e5c00441e13.png";
                        break;
                    case 12:
                        jogador.UrlFoto = "https://cdn.discordapp.com/attachments/758139402219159562/758145531024244766/a39e1f7d5369c3310fed1c6ad2d83628.png";
                        break;
                    case 13:
                        jogador.UrlFoto = "https://cdn.discordapp.com/attachments/758139402219159562/758145622636232704/e56d4f7cbfb4d2551caf2f38ef9dc94d.png";
                        break;
                    default:
                        await ctx.RespondAsync($"{ctx.User.Mention}, id do retrato não foi encontrado.");
                        return;
                }
                await banco.EditJogadorAsync(jogador);
                await session.CommitTransactionAsync();

                await ctx.RespondAsync($"{ctx.User.Mention}, você alterou o retrato.");
            }
        }
    }
}