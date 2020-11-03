using WafclastRPG.Atributos;
using WafclastRPG.Entidades;
using WafclastRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Services;
using WafclastRPG.Enuns;
using WafclastRPG.Entidades.Itens;

namespace WafclastRPG.Comandos.Acao
{
    public class ComandoPortal : BaseCommandModule
    {
        // Abrir um portal de qualquer lugar que não seja a base/nivel 0
        // para a base/nivel 0
        // Portal precisa ficar aberto
        // Pode voltar para o mesmo lugar em que foi aberto o portal
        // Quando voltar para o mesmo lugar, o portal se fecha.
        // Se ele não voltar para o portal, os itens do chão se perdem para sempre.
        // Obs:
        // Com portal os itens devem continuar no chão.
        // Consome 1 portal.


        /*
         * !portal para ir a base
         * !portal para voltar
         * 
         * Adicionar no personagem:
         * bool IsPortal; case true portal está aberto
         *                case false portal está fechado
         *                
         * Comandos chao, pegar, atacar, monstros, zona, não podem funcionar com portal aberto
         */

        public Banco banco;

        [Command("portal")]
        [Description("Permite voltar para a base, perde-se um pergaminho de portal no processo. " +
            "Utilize novamente para voltar a posição em que foi usado o portal.")]
        [ComoUsar("portal")]
        public async Task ComandoPortalAsync(CommandContext ctx)
        {
            // Verifica se existe o jogador,
            var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            if (naoCriouPersonagem) return;

            if (personagemNaoModificar.IsPortalAberto)
            {
                // Voltar para zona em que ele estava
                using (var session = await banco.Cliente.StartSessionAsync())
                {
                    BancoSession banco = new BancoSession(session);
                    RPJogador jogador = await banco.GetJogadorAsync(ctx);
                    RPPersonagem personagem = jogador.Personagem;

                    personagem.IsPortalAberto = false;
                    personagem.Zona.Nivel = personagem.Zona.NivelAnterior;
                    await banco.EditJogadorAsync(jogador);
                    await session.CommitTransactionAsync();

                    await ctx.RespondAsync($"{ctx.User.Mention}, portal fechado, você voltou para a zona {personagem.Zona.Nivel}!");
                }
                return;
            }

            if (personagemNaoModificar.Zona.Nivel == 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você não pode usar um portal na base!");
                return;
            }

            using (var session = await banco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

                if (personagem.Mochila.TryRemoveItemCurrency(RPClasse.PergaminhoPortal, out RPBaseItem _))
                {

                    personagem.IsPortalAberto = true;

                    personagem.Zona.NivelAnterior = personagem.Zona.Nivel;
                    personagem.Zona.Nivel = 0;
                    personagem.Vida.Adicionar(double.MaxValue);
                    personagem.Mana.Adicionar(double.MaxValue);

                    await banco.EditJogadorAsync(jogador);
                    await session.CommitTransactionAsync();

                    await ctx.RespondAsync($"{ctx.User.Mention}, portal aberto, você saiu da torre!");
                }
                else
                    await ctx.RespondAsync($"{ctx.User.Mention}, você não tem {"Pergaminho de Portal".Titulo()}!");
            }
        }
    }
}
