using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Text;
using System.Threading.Tasks;
using TorreRPG.Entidades.Itens.Currency;
using TorreRPG.Entidades.Itens;
using TorreRPG.Services;
using System;
using TorreRPG.Enuns;

namespace TorreRPG.Comandos.Exibir
{
    public class ComandoMochila : BaseCommandModule
    {
        public Banco banco { private get; set; }

        [Command("mochila")]
        [Description("Permite ver os itens que estão na mochila. Você consegue itens de monstros, vendendo e trocando com outros jogadores.")]
        public async Task ComandoMochilaAsync(CommandContext ctx)
        {
            // Verifica se existe o jogador,
            var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            if (naoCriouPersonagem) return;

            RPJogador jogador = await banco.GetJogadorAsync(ctx);
            RPPersonagem personagem = jogador.Personagem;

            StringBuilder str = new StringBuilder();
            for (int i = 0; i < personagem.Mochila.Itens.Count; i++)
            {
                var item = personagem.Mochila.Itens[i];
                str.Append($"`#{i}` ");
                str.Append(GerarEmojiRaridade(item.Raridade));

                str.Append($" {item.TipoBaseModificado.Titulo().Bold()} ");
                switch (item)
                {
                    case RPBaseCurrency rcp:
                        str.Append($"*x{rcp.PilhaAtual}*");
                        break;
                }
                str.AppendLine();
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe}", iconUrl: ctx.User.AvatarUrl);
            embed.WithDescription($"**Espaço da mochila: {personagem.Mochila.Espaco}/64**\n\n{str}");

            await ctx.RespondAsync(embed: embed.Build());
        }

        private DiscordEmoji GerarEmojiRaridade(RPRaridade raridade)
        {
            switch (raridade)
            {
                case RPRaridade.Normal:
                    return Emoji.ItemNormal;
                case RPRaridade.Magico:
                    return Emoji.ItemMagico;
                case RPRaridade.Raro:
                    return Emoji.ItemRaro;
                case RPRaridade.Unico:
                    return Emoji.ItemUnico;
                default:
                    return null;
            }
        }
    }
}
