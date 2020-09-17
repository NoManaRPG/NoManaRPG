using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorreRPG.Comandos.Exibir
{
    public class ComandoExaminar : BaseCommandModule
    {
        [Command("examinar")]
        public async Task ComandoExaminarAsync(CommandContext ctx, string idEscolhido = "")
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            RPJogador jogador = await ModuloBanco.GetJogadorAsync(ctx);
            RPPersonagem personagem = jogador.Personagem;

            // Converte o id informado.
            if (idEscolhido.TryParseID(out int id))
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa informar um Item #ID.");
                return;
            }

            if (personagem.Mochila.TryRemoveItem(id, out RPItem item))
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
                embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe}", iconUrl: ctx.User.AvatarUrl);
                embed.WithTitle($"*#{id}* - {item.TipoBaseModificado.Titulo().Bold()}");
                embed.WithDescription(ExaminarItem(item).ToString());

                await ctx.RespondAsync(embed: embed.Build());
            }
            else
                await ctx.RespondAsync($"{ctx.User.Mention}, #ID não encontrado na mochila!");
        }

        public static StringBuilder ExaminarItem(RPItem item)
        {
            StringBuilder str = new StringBuilder();
            //switch (item.Tipo)
            //{
            //    case RPTipo.PocaoVida:
            //        embed.WithDescription("Frascos de Vida\n" +
            //            $"Ocupa {item.Espaco} de espaço\n" +
            //            $"Recupera {item.LifeRegen} de vida por {item.Tempo} segundos\n" +
            //            $"Consome {item.CargasUso} de {item.CargasMax} cargas na utilização\n" +
            //            $"Atualmente tem 0 carga\n" +
            //            $"------\n" +
            //            "Só é possível manter cargas no cinto. Recarrega conforme você mata monstros.");
            //        break;
            //    case RPTipo.Arco:
            //        StringBuilder str = new StringBuilder();
            //        if (item.Destreza != 0)
            //            str.Append($"{item.Destreza} des, ");
            //        if (item.Inteligencia != 0)
            //            str.Append($"{item.Inteligencia} int, ");
            //        if (item.Forca != 0)
            //            str.Append($"{item.Forca} for");
            //        embed.WithDescription("Arcos\n" +
            //            $"Ocupa {item.Espaco} de espaço\n" +
            //            $"Dano físico: {item.DanoFisico.Minimo}-{item.DanoFisico.Maximo}\n" +
            //            $"Chance de crítico: {item.ChanceCritico * 100}%\n" +
            //            $"Ataques por segundo: {item.VelocidadeAtaque}\n" +
            //            $"------\n" +
            //            $"Requer nível {item.Nivel}, {str}");
            //        break;
            //}
            return str;
        }
    }
}
