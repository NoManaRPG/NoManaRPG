using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TorreRPG.Atributos;
using TorreRPG.Extensoes;

namespace TorreRPG.Comandos.Acao
{
    class ComandoVender : BaseCommandModule
    {
        [Command("vender")]
        [Description("Permite vender um item.")]
        [ComoUsar("vender [#ID]")]
        [Exemplo("vender #1")]
        public async Task ComandoVenderAsync(CommandContext ctx, string stringId = "")
        {
            // Verifica se existe o jogador,
            // Caso não exista avisar no chat e finaliza o metodo.
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            if (string.IsNullOrEmpty(stringId))
            {
                await ctx.RespondAsync($"{ctx.Member.Mention}, você precisa informar um item para vender.");
                return;
            }

            //Somente pode vender itens na base. (andar 0)
            //Verificar a raridade, se for normal, vender por 1 fragmento de pergaminho.
            //Se tiver 20 fragmentos, transformar em pergaminho. Pilha máxima de pergaminhos é 40.
            
        }
    }
}
