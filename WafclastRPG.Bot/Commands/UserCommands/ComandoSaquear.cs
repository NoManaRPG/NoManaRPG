using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;
using WafclastRPG.Game.Entidades.Itens;

namespace WafclastRPG.Bot.Commands.UserCommands
{
    public class ComandoSaquear : BaseCommandModule
    {
        public Banco banco;

        [Command("saquear")]
        [Description("Permite pegar os itens que caiu de um monstro após ser abatido.")]
        [Usage("saquear")]
        public async Task ComandoSaquearAsync(CommandContext ctx)
        {
            /*
            * Comando interativo.
            * Mostramos os itens disponível com 1 numeração na frente.
            * Esta númeração será a opção para o jogador escolher.
            * Ele pode pegar todos os itens, ou pegar somente 1.
            * Após pegar os itens, salvar o jogador e liberar os outros comandos.
            */

            // Trava o jogador de inicializar outro comando.
            using (await banco.LockAsync(ctx.User.Id))
            {
                var jogador = await banco.GetJogadorAsync(ctx);
                var per = jogador.Personagem;

                // Sempre precisa ter inimigo mortos e com itens para pegar.
                if (per.InimigoMonstro == null || per.InimigoMonstro.Vida > 1)
                {
                    await ctx.ResponderAsync("você precisa abater monstros para saquear itens!");
                    return;
                }

                if (per.InimigoMonstro.Drops.Count == 0)
                {
                    await ctx.ResponderAsync("você já saqueou tudo! Abate outro monstro.");
                    return;
                }

                var str = new StringBuilder();
                var listItens = new List<WafclastItem>();

                // Get all drops.
                for (int i = 0; i < per.InimigoMonstro.Drops.Count; i++)
                {
                    var item = await banco.GetItemAsync(per.InimigoMonstro.Drops[i].ItemId);
                    listItens.Add(item);

                    str.AppendLine($"`#{i}` **{item.Nome.Titulo()}** x_{per.InimigoMonstro.Drops[i].QuantidadeMin}_");
                }
                str.AppendLine($"`#{per.InimigoMonstro.Drops.Count}` **Saquear tudo.**");
                str.AppendLine($"`#{per.InimigoMonstro.Drops.Count + 1}` **Sair.**");


                var tempo = TimeSpan.FromSeconds(15);
                banco.StartExecutingInteractivity(ctx.User.Id);
                var interactivity = ctx.Client.GetInteractivity();

                var embed = new DiscordEmbedBuilder().Inicializar(ctx);
                embed.WithFooter("Você tem 15 segundos para responder!");
                embed.WithDescription("_Escolha o `#ID` para saquear._\n" + str.ToString());

                await ctx.ResponderAsync(embed.Build());

                int id = 0;
                while (true)
                {
                    var mensagem = await interactivity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.ChannelId == ctx.Channel.Id, tempo);
                    if (mensagem.TimedOut)
                        break;

                    if (!mensagem.Result.Content.TryParseID(out id))
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, o #ID precisa ser númerico!");
                        // Return to start of the loop
                        continue;
                    }

                    // Get all itens
                    if (id == per.InimigoMonstro.Drops.Count)
                    {
                        var strf = new StringBuilder();
                        for (int i = 0; i < per.InimigoMonstro.Drops.Count; i++)
                        {
                            bool tentar = per.Mochila.TryAddItem(listItens[i], per.InimigoMonstro.Drops[i].QuantidadeMin);
                            if (tentar)
                            {
                                strf.AppendLine($"{per.InimigoMonstro.Drops[i].QuantidadeMin} **{listItens[i].Nome.Titulo()}**");
                                per.InimigoMonstro.Drops.RemoveAt(i);
                            }
                            else
                                break;
                        }
                        await jogador.Salvar();
                        await ctx.ResponderAsync($"você saqueou: {strf.ToString()}");

                        break;
                    } //Exit
                    else if (id == per.InimigoMonstro.Drops.Count + 1)
                    {
                        break;
                    } //Invalid option
                    else if (id > per.InimigoMonstro.Drops.Count + 1)
                    {
                        await ctx.ResponderAsync("Opção inválida!");
                        continue;
                    }
                    // One item

                    var item = listItens[id];
                    int quantidade = 1;
                    if (per.InimigoMonstro.Drops[id].QuantidadeMin > 1)
                    // Select quantity
                    {
                        await ctx.ResponderAsync("quantos item deseja pegar? *(valor númerico)*");
                        mensagem = await interactivity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.ChannelId == ctx.Channel.Id, tempo);
                        if (mensagem.TimedOut)
                            break;

                        if (!int.TryParse(mensagem.Result.Content, out quantidade))
                            await ctx.RespondAsync($"{ctx.User.Mention}, informe uma quantidade válida!");

                        quantidade = Math.Clamp(quantidade, 0, per.InimigoMonstro.Drops[id].QuantidadeMin);
                    }

                    per.Mochila.TryAddItem(item, quantidade);
                    per.InimigoMonstro.Drops[id].QuantidadeMin -= quantidade;
                    if (per.InimigoMonstro.Drops[id].QuantidadeMin == 0)
                    {
                        per.InimigoMonstro.Drops.RemoveAt(id);
                        listItens.RemoveAt(id);
                    }
                    await jogador.Salvar();

                    await ctx.ResponderAsync($"você saqueou {quantidade} **{item.Nome.Titulo()}**!");
                    if (per.InimigoMonstro.Drops.Count == 0)
                        break;

                    str.Clear();
                    for (int i = 0; i < listItens.Count; i++)
                    {
                        str.AppendLine($"`#{i}` **{listItens[i].Nome.Titulo()}** x_{per.InimigoMonstro.Drops[i].QuantidadeMin}_");
                    }

                    str.AppendLine($"`#{per.InimigoMonstro.Drops.Count }` **Saquear tudo.**");
                    str.AppendLine($"`#{per.InimigoMonstro.Drops.Count + 1}` **Sair.**");
                }

                banco.StopExecutingInteractivity(ctx.User.Id);
            }
        }
    }
}