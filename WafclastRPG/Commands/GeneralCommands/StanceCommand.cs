using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using MongoDB.Driver;
using System.Threading.Tasks;
using WafclastRPG.DataBases;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using WafclastRPG.Enums;
using WafclastRPG.DataBases;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class StanceCommand : BaseCommandModule
    {
        public Database banco;

        [Command("postura")]
        [Description("Permite alterar a sua postura para melhor combater o inimigo a frente.")]
        [Usage("postura")]
        public async Task TemplateAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var player = await banco.FindPlayerAsync(ctx.User.Id);
            if (player == null)
            {
                await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            var interactivity = ctx.Client.GetInteractivity();
            banco.StartExecutingInteractivity(ctx.User.Id);
            var embed = new DiscordEmbedBuilder();
            embed.AddField("Desviar".Titulo(), "Ao ser atacado pelo inimigo, você sempre tentara desviar dos ataques " +
                "indo para trás ou para o lado caso ainda tenha estamina o suficiente.\n\n" +
                $"{Emojis.DiamanteLaranjaPequeno} Falhar em desviar resultara em dano.\n\n" +
                $"{Emojis.Aviso} Tentativa bem sucedida ou falha ainda consumirá vigor.", true);

            embed.AddField("Defender".Titulo(), "Ao ser atacado pelo inimigo, você sempre defenderá dos ataques com " +
                "as mãos ou com uma arma equipada.\n\n" +
                $"{Emojis.DiamanteLaranjaPequeno} Receber ataques consomem vigor.\n\n" +
                $"{Emojis.DiamanteLaranjaPequeno} Quanto mais Resistência você tiver menos vigor você perde.\n\n" +
                $"{Emojis.Aviso} Se sua agilidade for baixa, você falhará em defender.", true);

            embed.AddField("Sair".Titulo(), "Gostei da postura atual e não desejo trocar.");

            var mensagem = await ctx.RespondAsync($"{ctx.User.Mention}, atualmente sua postura é: {Formatter.Bold(player.Character.Stance.GetEnumDescription())}.\n " +
                $"Escolha uma postura digitando o nome dela.", embed.Build());

            bool continuar = true;
            while (continuar)
            {
                var msg = await interactivity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.ChannelId == ctx.Channel.Id);
                if (msg.TimedOut)
                {
                    await ctx.ResponderAsync("o tempo de resposta expirou.");
                    continuar = false;
                    banco.StopExecutingInteractivity(ctx.User.Id);
                }
                else
                    switch (msg.Result.Content.ToLower())
                    {
                        case "sair":
                            continuar = false;
                            break;
                        case "desviar":
                            await ctx.ResponderAsync($"você escolheu {Formatter.Bold("desviar")}.");
                            continuar = false;
                            player.Character.Stance = StanceType.Parry;

                            break;
                        case "defender":
                            await ctx.ResponderAsync($"você escolheu {Formatter.Bold("defender")}.");
                            continuar = false;
                            player.Character.Stance = StanceType.Defend;

                            break;
                        default:
                            await ctx.ResponderAsync("isso não é uma reposta, por favor escolha entre:", embed.Build());
                            break;
                    }
            }

            banco.StopExecutingInteractivity(ctx.User.Id);
            await banco.CollectionJogadores.ReplaceOneAsync(x => x.Id == player.Id, player);
        }
    }
}
