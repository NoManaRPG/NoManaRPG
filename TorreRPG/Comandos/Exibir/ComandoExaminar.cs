using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Linq;
using System.Threading.Tasks;
using TorreRPG.Atributos;
using TorreRPG.Services;
using System;
using TorreRPG.Enuns;
using System.Text;

namespace TorreRPG.Comandos.Exibir
{
    public class ComandoExaminar : BaseCommandModule
    {
        public Banco banco { private get; set; }

        [Command("examinar")]
        [Aliases("ex")]
        [Description("Permite examinar um item.\n`#ID` se contra na mochila.")]
        [ComoUsar("examinar [#ID]")]
        [Exemplo("examinar #1")]
        public async Task ComandoExaminarAsync(CommandContext ctx, string idEscolhido = "0")
        {
            // Verifica se existe o jogador,
            var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            if (naoCriouPersonagem) return;

            if (personagemNaoModificar.Mochila.Itens.Count == 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa de itens na mochila para examinar!");
                return;
            }

            // Converte o id informado.
            if (idEscolhido.TryParseID(out int id))
            {
                var descricao = ItemDescricao(personagemNaoModificar, id);
                if (descricao != null)
                {
                    descricao.WithAuthor($"{ctx.User.Username} - {personagemNaoModificar.Nome}", iconUrl: ctx.User.AvatarUrl);
                    await ctx.RespondAsync(embed: descricao.Build());
                }
                else
                    await ctx.RespondAsync($"{ctx.User.Mention}, `#ID` não encontrado!");
            }
            else
                await ctx.RespondAsync($"{ctx.User.Mention}, o `#ID` precisa ser numérico. Digite `!mochila` para encontrar `#ID`s.");
        }

        public static DiscordEmbedBuilder ItemDescricao(RPPersonagem personagem, int id)
        {
            var item = personagem.Mochila.Itens.ElementAtOrDefault(id);
            if (item != null)
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
                embed.WithTitle($"`#{id}` {GerarEmojiRaridade(item.Raridade)} {item.TipoBaseModificado.Titulo().Bold()}");
                embed.WithColor(GerarColorRaridade(item.Raridade));
                StringBuilder str = new StringBuilder();
                switch (item)
                {
                    case RPFrascoVida frascoVida:
                        str.AppendLine("Frascos de Vida");
                        str.AppendLine($"Ocupa {item.Espaco} espaço");
                        str.AppendLine($"Recupera {frascoVida.Regen} de Vida por {frascoVida.Tempo} Segundo");
                        str.AppendLine($"Consome {frascoVida.CargasUso} de {frascoVida.CargasMax} Carga na utilização");
                        str.AppendLine($"Atualmente tem {frascoVida.CargasAtual} Carga");
                        str.AppendLine($"Requer nível {frascoVida.ILevel}");
                        str.AppendLine();
                        str.AppendLine("██████████████");
                        str.AppendLine();
                        str.AppendLine("Só é possível manter cargas no cinto. Recarrega conforme você mata monstros.");
                        break;
                    case RPBaseItemArma arma:
                        switch (arma)
                        {
                            case RPArmaAdaga _:
                                str.AppendLine("Adaga");
                                break;
                            case RPArmaArco _:
                                str.AppendLine("Arco");
                                break;
                            case RPArmaCetro _:
                                str.AppendLine("Cetro");
                                break;
                            case RPArmaEspada _:
                                str.AppendLine("Espada");
                                break;
                            case RPArmaVarinha _:
                                str.AppendLine("Varinha");
                                break;
                        }
                        str.AppendLine($"Ocupa {item.Espaco} espaço");
                        str.AppendLine($"Dano Físico: {arma.DanoFisicoBase.Minimo}-{arma.DanoFisicoBase.Maximo}");
                        str.AppendLine($"Chance de Crítico: { arma.ChanceCritico * 100}% ");
                        str.AppendLine($"Ataques por Segundo: {arma.VelocidadeAtaque}");
                        str.AppendLine();
                        str.AppendLine("██████████████");
                        str.AppendLine();
                        str.AppendLine($"Requer Nível {arma.ILevel}, {(arma.Inteligencia == 0 ? "" : $"{arma.Inteligencia} Int,")} {(arma.Destreza == 0 ? "" : $"{arma.Destreza} Des,")} {(arma.Forca == 0 ? "" : $"{arma.Forca} For")}");
                        break;
                    case RPMoedaEmpilhavel moeda:
                        str.AppendLine("Moedas Empilháveis");
                        str.AppendLine($"Ocupa {item.Espaco} espaço");
                        str.AppendLine($"Tamanho da pilha: {moeda.PilhaAtual}/{moeda.PilhaMaxima}");
                        str.AppendLine();
                        str.AppendLine("██████████████");
                        str.AppendLine();
                        switch (moeda.Classe)
                        {
                            case RPClasse.FragmentoPergaminho:
                                str.AppendLine("Uma pilha de 5 fragmentos forma um pergaminho de sabedoria.");
                                break;
                            case RPClasse.PergaminhoSabedoria:
                                str.AppendLine("Identifica um item");
                                str.AppendLine("Digite `!identificar #ID` para identificar.");
                                break;
                        }
                        break;
                }
                embed.WithDescription(str.ToString());
                return embed;
            }
            return null;
        }

        private static DiscordEmoji GerarEmojiRaridade(RPRaridade raridade)
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

        private static DiscordColor GerarColorRaridade(RPRaridade raridade)
        {
            switch (raridade)
            {
                case RPRaridade.Normal:
                    return DiscordColor.LightGray;
                case RPRaridade.Magico:
                    return DiscordColor.CornflowerBlue;
                case RPRaridade.Raro:
                    return DiscordColor.HotPink;
                case RPRaridade.Unico:
                    return DiscordColor.Orange;
                default:
                    return DiscordColor.Wheat;
            }
        }
    }
}
