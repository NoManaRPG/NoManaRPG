using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Game.Services;
using System;

namespace WafclastRPG.Game.Comandos.Acao
{
    public class ComandoExplorar : BaseCommandModule
    {
        public Banco banco;

        [Command("explorar")]
        [Description("Permite procurar por novos inimigos no andar atual!")]
        public async Task ComandoExplorarAsync(CommandContext ctx)
        {
            // Verifica se existe o jogador,
            var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            if (naoCriouPersonagem) return;

            if (personagemNaoModificar.Zona.Monstros.Count != 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa eliminar todos os montros para explorar!");
                return;
            }

            if (personagemNaoModificar.Zona.Nivel == 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você somente pode explorar os níveis inferiores da torre!");
                return;
            }

            int inimigos = 0;
            using (var session = await banco.Client.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

                inimigos = personagem.Zona.TrocarZona(personagem.VelocidadeAtaque.Modificado, personagem.Zona.Nivel);

                await banco.EditJogadorAsync(jogador);
                await session.CommitTransactionAsync();
                await ctx.RespondAsync($"{ctx.User.Mention}, apareceu [{inimigos.Bold()}] monstro na sua frente!");
            }
        }
    }
}
