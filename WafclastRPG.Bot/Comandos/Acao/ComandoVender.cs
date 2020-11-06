using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Game;

namespace WafclastRPG.Bot.Comandos.Acao
{
    class ComandoVender : BaseCommandModule
    {
        public Banco banco;

        [Command("vender")]
        [Description("Permite vender um item.")]
        [ComoUsar("vender [#ID]")]
        [Exemplo("vender #1")]
        [Cooldown(1, 2, CooldownBucketType.User)]
        public async Task ComandoVenderAsync(CommandContext ctx, string stringId = "")
        {
            // Verifica se existe o jogador,
            //var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            //if (naoCriouPersonagem) return;

            //// Somente pode vender itens na base. (andar 0)
            //if (personagemNaoModificar.Zona.Nivel != 0)
            //{
            //    await ctx.RespondAsync($"{ctx.Member.Mention}, você precisa estar fora da torre para vender itens.");
            //    return;
            //}

            //if (string.IsNullOrEmpty(stringId))
            //{
            //    await ctx.RespondAsync($"{ctx.Member.Mention}, você precisa informar um `#ID` de um item para poder estar vendendo. Digite `!mochila` para encontra-los, eles geralmente são os primeiros números de um item!");
            //    return;
            //}

            //// Converte o id informado.
            //if (!stringId.TryParseID(out int index))
            //{
            //    await ctx.RespondAsync($"{ctx.User.Mention}, o `#ID` é numérico!");
            //    return;
            //}

            //using (var session = await banco.Client.StartSessionAsync())
            //{
            //    BancoSession banco = new BancoSession(session);
            //    RPJogador jogador = await banco.GetJogadorAsync(ctx);
            //    RPPersonagem personagem = jogador.Personagem;

            //    if (personagem.Mochila.TryRemoveItem(index, out RPBaseItem item))
            //    {
            //        bool adicionou = false;
            //        switch (item)
            //        {
            //            case RPMoedaEmpilhavel me:
            //                switch (me.Classe)
            //                {
            //                    case RPClasse.FragmentoPergaminho:
            //                        await ctx.RespondAsync($"{ctx.User.Mention}, você não pode vender este item!");
            //                        return;
            //                    case RPClasse.PergaminhoSabedoria:
            //                        adicionou = personagem.Mochila.TryAddItem(new MoedasEmpilhaveis().PergaminhoFragmento());
            //                        break;
            //                    case RPClasse.PergaminhoPortal:
            //                        for (int i = 0; i < 3; i++)
            //                            adicionou = personagem.Mochila.TryAddItem(new MoedasEmpilhaveis().PergaminhoSabedoria());
            //                        break;
            //                }
            //                break;
            //            case RPBaseItem bi:
            //                //Verificar a raridade, se for normal, vender por 1 fragmento de pergaminho.
            //                switch (bi.Raridade)
            //                {
            //                    case RPRaridade.Normal:
            //                        adicionou = personagem.Mochila.TryAddItem(new MoedasEmpilhaveis().PergaminhoFragmento());
            //                        break;
            //                }
            //                break;
            //        }

            //        if (adicionou)
            //        {
            //            await banco.EditJogadorAsync(jogador);
            //            await session.CommitTransactionAsync();

            //            await ctx.RespondAsync($"{ctx.User.Mention}, você vendeu {1.Bold()} {item.TipoBaseModificado.Titulo()}!");
            //            return;
            //        }

            //        await ctx.RespondAsync($"{ctx.Member.Mention}, você não tem espaço o suficiente na mochila.");
            //        return;
            //    }

            //    await ctx.RespondAsync($"{ctx.Member.Mention}, não existe item com este `#ID` na mochila.");
            //    return;
            //}
        }
    }

}

