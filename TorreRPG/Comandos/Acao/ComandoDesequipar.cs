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
    public class ComandoDesequipar : BaseCommandModule
    {
        [Command("desequipar")]
        public async Task ComandoDesequiparAsync(CommandContext ctx, string itemString)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            var item = string.Empty;
            switch (itemString.RemoverAcentos().ToLower())
            {
                case "primeira mao":
                case "mao principal":
                case "primeira":
                    using (var session = await ModuloBanco.Cliente.StartSessionAsync())
                    {
                        BancoSession banco = new BancoSession(session);
                        RPJogador jogador = await banco.GetJogadorAsync(ctx);
                        RPPersonagem personagem = jogador.Personagem;

                        // Verifica se tem algo equipado
                        if (personagem.MaoPrincipal == null)
                        {
                            // Avisa
                            await ctx.RespondAsync($"{ctx.User.Mention}, você não tem nada equipado na primeira mão.");
                            return;
                        }

                        // Tenta guardar na mochila
                        if (personagem.Mochila.TryAddItem(personagem.MaoPrincipal))
                        {
                            // Remove da mão
                            RemoverItemAtributos(personagem, personagem.MaoPrincipal);
                            personagem.MaoPrincipal = null;
                        }
                        else
                        {
                            await ctx.RespondAsync($"{ctx.User.Mention}, você não tem espaço o suficiente na mochila para desequipar!");
                            return;
                        }

                        break;
                    }
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
