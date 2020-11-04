using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Game;
using WafclastRPG.Bot.Atributos;

namespace WafclastRPG.Bot.Comandos.Acao
{
    public class ComandoDesequipar : BaseCommandModule
    {
        public Banco banco;

        [Command("desequipar")]
        [Description("Permite desequipar um item. Veja no equipamentos os `⌈SLOTS⌋` disponíveis.")]
        [ComoUsar("desequipar <slot>")]
        [Exemplo("desequipar mão principal")]
        [Exemplo("desequipar segunda mão")]
        public async Task ComandoDesequiparAsync(CommandContext ctx, [RemainingText] string itemString = "")
        {
            //// Verifica se existe o jogador,
            //var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            //if (naoCriouPersonagem) return;

            //using (var session = await banco.Client.StartSessionAsync())
            //{
            //    BancoSession banco = new BancoSession(session);
            //    RPJogador jogador = await banco.GetJogadorAsync(ctx);
            //    RPPersonagem personagem = jogador.Personagem;

            //    RPBaseItem item = null;
            //    switch (itemString.RemoverAcentos().ToLower())
            //    {
            //        case "0":
            //        case "#0":
            //            item = await RemoverFrascoAsync(personagem, ctx, 0);
            //            if (item == null)
            //                return;
            //            break;
            //        case "1":
            //        case "#1":
            //            item = await RemoverFrascoAsync(personagem, ctx, 1);
            //            if (item == null)
            //                return;
            //            break;
            //        case "2":
            //        case "#2":
            //            item = await RemoverFrascoAsync(personagem, ctx, 2);
            //            if (item == null)
            //                return;
            //            break;
            //        case "3":
            //        case "#3":
            //            item = await RemoverFrascoAsync(personagem, ctx, 3);
            //            if (item == null)
            //                return;
            //            break;
            //        case "4":
            //        case "#4":
            //            item = await RemoverFrascoAsync(personagem, ctx, 4);
            //            if (item == null)
            //                return;
            //            break;
            //        case "primeira mao":
            //        case "mao principal":
            //        case "primeira":
            //        case "principal":
            //        case "mao1":
            //        case "m1":
            //            // Verifica se tem algo equipado
            //            if (personagem.MaoPrincipal == null)
            //            {
            //                // Avisa
            //                await ctx.RespondAsync($"{ctx.User.Mention}, você não tem nada equipado na primeira mão.");
            //                return;
            //            }
            //            // Desequipa
            //            item = personagem.MaoPrincipal;
            //            personagem.MaoPrincipal = null;
            //            break;
            //        case "segunda mao":
            //        case "segunda":
            //        case "mao secundaria":
            //        case "mao2":
            //        case "m2":
            //            // Verifica se tem algo equipado
            //            if (personagem.MaoSecundaria == null)
            //            {
            //                // Avisa
            //                await ctx.RespondAsync($"{ctx.User.Mention}, você não tem nada equipado na segunda mão.");
            //                return;
            //            }
            //            // Desequipa
            //            item = personagem.MaoSecundaria;
            //            personagem.MaoSecundaria = null;
            //            break;
            //        default:
            //            await ctx.RespondAsync($"{ctx.User.Mention}, não foi encontrado o {"SLOT".Titulo().Bold()} que você pediu! Digite `!equipamentos` para ver os {"SLOT".Titulo().Bold()} disponíveis.");
            //            return;
            //    }

            //    // Tenta guardar na mochila
            //    if (personagem.Mochila.TryAddItem(item))
            //    {
            //        // Remove os atributos
            //        switch (item)
            //        {
            //            case RPBaseItemArma arma:
            //                personagem.DanoFisicoExtra.Subtrair(arma.DanoFisicoModificado);
            //                personagem.CalcDano();
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        await ctx.RespondAsync($"{ctx.User.Mention}, você não tem espaço o suficiente na mochila para desequipar {item.TipoBaseModificado.Titulo().Bold()}!");
            //        return;
            //    }

            //    await banco.EditJogadorAsync(jogador);
            //    await session.CommitTransactionAsync();

            //    await ctx.RespondAsync($"{ctx.User.Mention}, você desequipou {item.TipoBaseModificado.Titulo().Bold()}.");
            //}
        }

        //public async Task<RPBaseItem> RemoverFrascoAsync(RPPersonagem personagem, CommandContext ctx, int index)
        //{
        //    // Verifica se tem algo equipado
        //    index = Math.Clamp(index, 0, 4);
        //    var item = personagem.Frascos.ElementAtOrDefault(index);
        //    if (item == null)
        //    {
        //        // Avisa
        //        await ctx.RespondAsync($"{ctx.User.Mention}, você não tem nada equipado no slot `#{index}` do cinto de poções.");
        //        return null;
        //    }
        //    // Desequipa
        //    personagem.Frascos.RemoveAt(index);
        //    return item;
        //}
    }
}
