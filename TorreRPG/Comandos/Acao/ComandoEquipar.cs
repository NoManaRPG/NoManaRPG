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
                await ctx.RespondAsync($"{ctx.User.Mention}, #ID não encontrado na mochila!");
                return;
            }

            using (var session = await ModuloBanco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

                // Pega o item
                bool equipou = false;
                if (personagem.Mochila.TryRemoveItem(indexItem, out RPItem item))
                {
                    switch (item)
                    {
                        case RPFrascoVida frascoVida:
                            // Todas os slots estão equipados?
                            var pocaoEquipada = personagem.Frascos.ElementAtOrDefault(4);
                            if (pocaoEquipada != null)
                            {
                                // Avisa
                                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa remover um frasco antes de tentar equipar outro!");
                                return;
                            }
                            else
                            {
                                // Equipa
                                personagem.Frascos.Add(frascoVida);
                                if (personagem.Zona.Nivel == 0)
                                    frascoVida.Resetar();
                                equipou = true;
                            }
                            break;
                        case RPArco arco:
                            //if (personagem.MaoPrincipal == null && personagem.MaoSecundaria == null)
                            // Todos os slots estão equipados?
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
                    await ctx.RespondAsync($"{ctx.User.Mention}, o item {item.TipoBaseModificado.Titulo().Bold()} foi equipado!");
            }
        }
    }
}
