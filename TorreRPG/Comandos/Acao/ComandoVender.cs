﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TorreRPG.Atributos;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Entidades.Itens.Currency;
using TorreRPG.Enuns;
using TorreRPG.Extensoes;

namespace TorreRPG.Comandos.Acao
{
    class ComandoVender : BaseCommandModule
    {
        [Command("vender")]
        [Description("Permite vender um item.")]
        [ComoUsar("vender [#ID]")]
        [Exemplo("vender #1")]
        [Cooldown(1, 2, CooldownBucketType.User)]
        public async Task ComandoVenderAsync(CommandContext ctx, string stringId = "")
        {
            // Verifica se existe o jogador,
            // Caso não exista avisar no chat e finaliza o metodo.
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            if (string.IsNullOrEmpty(stringId))
            {
                await ctx.RespondAsync($"{ctx.Member.Mention}, você precisa informar um `#ID` para vender. Digite `!mochila` para encontra-los!");
                return;
            }

            // Converte o id informado.
            if (!stringId.TryParseID(out int index))
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, o #ID é numérico!");
                return;
            }

            using (var session = await ModuloBanco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

                //Somente pode vender itens na base. (andar 0)
                if (personagem.Zona.Nivel != 0)
                {
                    await ctx.RespondAsync($"{ctx.Member.Mention}, você precisa estar fora da torre para poder vender itens.");
                    return;
                }

                if (personagem.Mochila.TryRemoveItem(index, out RPBaseItem item))
                {
                    bool adicionou = false;
                    switch (item)
                    {
                        case RPCurrencyPergaminho rcp:
                            if (rcp.Classe == RPClasse.FragmentoPergaminho)
                            {
                                await ctx.RespondAsync($"{ctx.User.Mention}, você não pode vender este item!");
                                return;
                            }
                            adicionou = personagem.Mochila.TryAddItem(new Metadata.Itens.Currency.CurrencyPergaminho().PergaminhoFragmento1());
                            break;
                        case RPBaseItem rbi:
                            //Verificar a raridade, se for normal, vender por 1 fragmento de pergaminho.
                            switch (rbi.Raridade)
                            {
                                case RPRaridade.Normal:
                                    adicionou = personagem.Mochila.TryAddItem(new Metadata.Itens.Currency.CurrencyPergaminho().PergaminhoFragmento1());
                                    break;
                            }

                            if (adicionou)
                            {
                                await banco.EditJogadorAsync(jogador);
                                await session.CommitTransactionAsync();

                                await ctx.RespondAsync($"{ctx.User.Mention}, o item {rbi.TipoBaseModificado.Titulo().Bold()} foi vendido!");
                                return;
                            }
                            await ctx.RespondAsync($"{ctx.Member.Mention}, você não tem espaço o suficiente na mochila.");
                            return;
                    }
                }

                await ctx.RespondAsync($"{ctx.Member.Mention}, não existe item com este `#ID` na mochila.");
                return;

            }
        }
    }
}
