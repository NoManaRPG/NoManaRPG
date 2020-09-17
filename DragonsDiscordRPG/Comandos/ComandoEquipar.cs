using DragonsDiscordRPG.Atributos;
using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Enuns;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Linq;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
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
                    switch (item.Tipo)
                    {
                        case RPTipo.PocaoVida:
                            // Todas os slots estão equipados?
                            var pocaoEquipada = personagem.Pocoes.ElementAtOrDefault(4);
                            if (pocaoEquipada != null)
                            {
                                // Avisa
                                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa remover uma poção antes de tentar equipar outra!");
                                return;
                            }
                            else
                            {
                                // Equipa
                                personagem.Pocoes.Add(item);
                                if (personagem.Zona.Nivel == 0)
                                    item.CargasAtual = item.CargasMax;
                                equipou = true;
                            }
                            break;
                        case RPTipo.Arco:
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
                    await ctx.RespondAsync($"{ctx.User.Mention}, o item {item.Nome.Titulo().Bold()} foi equipado!");
            }
        }
    }
}
