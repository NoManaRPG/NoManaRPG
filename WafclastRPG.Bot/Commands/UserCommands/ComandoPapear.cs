using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades.Itens;

namespace WafclastRPG.Bot.Commands.UserCommands
{
    public class ComandoPapear : BaseCommandModule
    {
        public Banco banco;


        [Command("papear")]
        [Description("Permite conversar com um NPC")]
        [Example("papear Wilk", "Abre o menu de conversa do NPC Wilk")]
        [Usage("papear <NPC>")]
        public async Task ComandoPapearAsync(CommandContext ctx, string npc = "")
        {
            //// Verifica se existe o jogador e faz o jogador esperar antes de começar outro comando
            //var (isJogadorCriado, sessao) = await banco.GetSessaoAsync(ctx);
            //if (!isJogadorCriado) return;

            //var interactivity = ctx.Client.GetInteractivity();
            //banco.StartExecutingInteractivity(ctx.User.Id);
            //var mensagem = await ctx.RespondAsync("Conversando com Wafclast\nDigite 1 para sair\nDigite 2 para elogiar");

            //bool continuar = true;
            //while (continuar)
            //{
            //    var msg = await interactivity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.ChannelId == ctx.Channel.Id);
            //    if (msg.TimedOut)
            //        await ctx.RespondAsync("Você é chato, tchau!");
            //    else
            //        switch (msg.Result.Content)
            //        {
            //            case "1":
            //                continuar = false;
            //                banco.StopExecutingInteractivity(ctx.User.Id);
            //                break;
            //            case "2":
            //                await ctx.RespondAsync("Eu sei eu sei, sou muito bonita, obrigada.");
            //                break;
            //            default:
            //                await ctx.RespondAsync("Pelo jeito, você não gosta de conversar comigo! :rage:");
            //                break;
            //        }
            //}
            //sessao.Soltar();
        }
    }
}
