using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using TorreRPG.Atributos;

namespace TorreRPG.Comandos.Acao
{
    public class ComandoDesequipar : BaseCommandModule
    {
        [Command("desequipar")]
        [Description("Permite desequipar um item. Veja no equipamentos os ⌈SLOTS⌋ disponíveis.")]
        [ComoUsar("desequipar [SLOT]")]
        [Exemplo("desequipar mão principal")]
        [Exemplo("desequipar segunda mão")]
        public async Task ComandoDesequiparAsync(CommandContext ctx, [RemainingText] string itemString)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            using (var session = await ModuloBanco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

                RPBaseItem item = null;
                switch (itemString.RemoverAcentos().ToLower())
                {
                    case "primeira mao":
                    case "mao principal":
                    case "primeira":
                        // Verifica se tem algo equipado
                        if (personagem.MaoPrincipal == null)
                        {
                            // Avisa
                            await ctx.RespondAsync($"{ctx.User.Mention}, você não tem nada equipado na primeira mão.");
                            return;
                        }
                        // Desequipa
                        item = personagem.MaoPrincipal;
                        personagem.MaoPrincipal = null;
                        break;
                    case "segunda mao":
                    case "segunda":
                    case "mao secundaria":
                        // Verifica se tem algo equipado
                        if (personagem.MaoSecundaria == null)
                        {
                            // Avisa
                            await ctx.RespondAsync($"{ctx.User.Mention}, você não tem nada equipado na segunda mão.");
                            return;
                        }
                        // Desequipa
                        item = personagem.MaoSecundaria;
                        personagem.MaoSecundaria = null;
                        break;
                }

                // Tenta guardar na mochila
                if (personagem.Mochila.TryAddItem(item))
                {
                    // Remove os atributos
                    RemoverItemAtributos(personagem, item);
                }
                else
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, você não tem espaço o suficiente na mochila para desequipar {item.TipoBaseModificado.Titulo().Bold()}!");
                    return;
                }

                await banco.EditJogadorAsync(jogador);
                await session.CommitTransactionAsync();

                await ctx.RespondAsync($"{ctx.User.Mention}, você desequipou {item.TipoBaseModificado.Titulo().Bold()}.");
            }
        }

        private void RemoverItemAtributos(RPPersonagem personagem, RPBaseItem item)
        {
            switch (item)
            {
                case RPBaseItemArma arma:
                    personagem.DanoFisicoExtra.Subtrair(arma.DanoFisicoModificado);
                    break;
            }
            personagem.CalcDano();
        }
    }
}
