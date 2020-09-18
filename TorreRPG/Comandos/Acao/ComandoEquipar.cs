using TorreRPG.Atributos;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Linq;
using System.Threading.Tasks;

namespace TorreRPG.Comandos.Acao
{
    public class ComandoEquipar : BaseCommandModule
    {
        [Command("equipar")]
        [Aliases("e")]
        [Description("Permite equipar um item.\n#ID se contra na mochila.")]
        [ComoUsar("equipar [Item #ID]")]
        [Exemplo("equipar #24")]
        public async Task ComandoEquiparAsync(CommandContext ctx, string stringIndexItem = "")
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            if (!stringIndexItem.TryParseID(out int indexItem))
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, informe um #ID que se encontra na mochila!");
                return;
            }

            using (var session = await ModuloBanco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

                bool equipou = false;
                // Tenta remover o item
                if (personagem.Mochila.TryRemoveItem(indexItem, out RPItem item))
                {
                    // Verifica se o nível do item
                    if (item.ILevel > personagem.Nivel.Atual)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, o seu personagem não tem o nível {item.ILevel.Bold()} para equipar este item!");
                        return;
                    }

                    // Verifica o tipo do item
                    switch (item)
                    {
                        case RPFrascoVida frascoVida:
                            // Todas os slots estão equipados?
                            var pocaoEquipada = personagem.Frascos.ElementAtOrDefault(4);
                            if (pocaoEquipada != null)
                            {
                                // Avisa
                                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa retirar um frasco que está equipado, antes de tentar equipar outro!");
                                return;
                            }

                            // Equipa
                            personagem.Frascos.Add(frascoVida);
                            if (personagem.Zona.Nivel == 0)
                                frascoVida.Resetar();
                            equipou = true;
                            break;
                        case RPArco arco:
                            // Verifica se as duas mão estão equipadas
                            if (personagem.MaoPrincipal != null)
                            {
                                // Avisa
                                await ctx.RespondAsync($"{ctx.User.Mention}, as suas duas mãos já estão ocupadas segurando outro item!");
                                return;
                            }
                            else if (personagem.MaoSecundaria != null)
                            {
                                // Avisa
                                await ctx.RespondAsync($"{ctx.User.Mention}, as suas duas mãos já estão ocupadas segurando outro item!");
                                return;
                            }

                            // Equipa
                            personagem.Equipar(arco);
                            equipou = true;
                            break;
                        default:
                            await ctx.RespondAsync($"{ctx.User.Mention}, este item não é equipável!");
                            return;
                    }

                }
                else
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, #ID não encontrado!");
                    return;
                }

                await banco.EditJogadorAsync(jogador);
                await session.CommitTransactionAsync();

                if (equipou)
                    await ctx.RespondAsync($"{ctx.User.Mention}, você equipou {item.TipoBaseModificado.Titulo().Bold()}!");
            }
        }
    }
}
