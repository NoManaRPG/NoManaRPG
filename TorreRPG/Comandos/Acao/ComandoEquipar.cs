using TorreRPG.Atributos;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Linq;
using System.Threading.Tasks;
using TorreRPG.Services;
using System;

namespace TorreRPG.Comandos.Acao
{
    public class ComandoEquipar : BaseCommandModule
    {
        public Banco banco { private get; set; }

        [Command("equipar")]
        [Aliases("e")]
        [Description("Permite equipar um item.\n`#ID` se contra na mochila.")]
        [ComoUsar("equipar [#ID]")]
        [Exemplo("equipar #1")]
        public async Task ComandoEquiparAsync(CommandContext ctx, string stringIndexItem = "0")
        {
            // Verifica se existe o jogador,
            var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            if (naoCriouPersonagem) return;

            if (!stringIndexItem.TryParseID(out int indexItem))
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, o `#ID` precisa ser numérico. Digite `!mochila` para encontrar `#ID`s.");
                return;
            }

            using (var session = await banco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

                bool equipou = false;
                // Tenta remover o item
                if (personagem.Mochila.TryRemoveItem(indexItem, out RPBaseItem item))
                {
                    // Verifica se tem o nível suficiente para equipar.
                    if (item.ILevel > personagem.Nivel.Atual)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, o seu personagem não tem o nível {item.ILevel.Bold()} para equipar este item!");
                        return;
                    }

                    // Verifica o tipo do item
                    switch (item.Classe)
                    {
                        case RPClasse.Frasco:
                            #region Frascos
                            // Todas os slots estão equipados?
                            var pocaoEquipada = personagem.Frascos.ElementAtOrDefault(4);
                            if (pocaoEquipada != null)
                            {
                                // Avisa
                                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa retirar um frasco equipados, antes de tentar equipar outro!");
                                return;
                            }

                            // Equipa
                            personagem.Frascos.Add(item as RPBaseFrasco);
                            if (personagem.Zona.Nivel == 0)
                                (item as RPBaseFrasco).ResetarCargas();
                            equipou = true;
                            break;
                        case RPClasse.DuasMao:
                            // Verifica se as duas mão estão equipadas
                            if (personagem.MaoPrincipal != null)
                            {
                                // Avisa
                                await ctx.RespondAsync($"{ctx.User.Mention}, este item precisa das duas mãos livres!");
                                return;
                            }

                            if (personagem.MaoSecundaria != null)
                            {
                                // Avisa
                                await ctx.RespondAsync($"{ctx.User.Mention}, este item precisa das duas mãos livres!");
                                return;
                            }

                            // Equipa
                            personagem.Equipar(item);
                            personagem.MaoPrincipal = item;
                            equipou = true;
                            break;
                        #endregion
                        case RPClasse.UmaMao:
                            #region Armas de uma mão
                            // Verificar se a primeira mão está vazias.
                            if (personagem.MaoPrincipal == null)
                            {
                                // Equipa
                                personagem.Equipar(item);
                                personagem.MaoPrincipal = item;
                                equipou = true;
                                break;
                            }
                            // Verifica se a segunda mão está vazia
                            if (personagem.MaoSecundaria == null && personagem.MaoPrincipal.Classe != RPClasse.DuasMao)
                            {
                                // Equipa
                                personagem.Equipar(item);
                                personagem.MaoSecundaria = item;
                                equipou = true;
                                break;
                            }

                            // As duas estão ocupadas? Avisa
                            await ctx.RespondAsync($"{ctx.User.Mention}, as suas duas mãos já estão ocupadas segurando outro item!");
                            return;
                        #endregion
                        default:
                            await ctx.RespondAsync($"{ctx.User.Mention}, este item não é equipável!");
                            return;
                    }
                }
                else
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, `#ID` não encontrado!");
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
