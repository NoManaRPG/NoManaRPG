using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WafclastRPG.Bot.Attributes;
using WafclastRPG.Bot.Entities;
using WafclastRPG.Bot.Extensions;

namespace WafclastRPG.Bot.Commands.UserCommands
{
    public class LookCommand : BaseCommandModule
    {
        public Database banco;

        [Command("olhar")]
        [Aliases("o", "look")]
        [Description("Permiter ver o estado atual de monstros ou de alguém .")]
        [Usage("olhar [ #id | @menção ]")]
        [Example("olhar #1", "Permite olhar o estado do monstro com o ID 1.")]
        [Example("olhar @Talion", "Permite olhar o estado do jogador mencionado.")]
        public async Task TemplateAsync(CommandContext ctx, string alvo = "")
        {
            await ctx.TriggerTypingAsync();
            var player = await banco.FindPlayerAsync(ctx.User.Id);
            if (!await ctx.HasPlayerAsync(player))
                return;

            if (player.Character.LocalId != ctx.Channel.Id)
            {
                await ctx.ResponderAsync(Strings.LocalDiferente(ctx.Channel.Name));
                return;
            }

            if (string.IsNullOrWhiteSpace(alvo))
            {
                await ctx.ResponderAsync($"você precisa informar {Formatter.Bold("alguém")} para olhar");
                return;
            }

            //Checks if is user or monster.
            var regex = new Regex(@"<@!?(\d+)>", RegexOptions.ECMAScript);
            var isUser = regex.IsMatch(alvo);
            if (isUser)
            {
                var match = regex.Match(alvo);
                var userId = ulong.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var playerTarget = await banco.FindPlayerAsync(userId);
                if (playerTarget == null)
                {
                    await ctx.ResponderAsync("o jogador ainda não criou um personagem!");
                    return;
                }

                if (playerTarget.Character.LocalId != player.Character.LocalId)
                {
                    await ctx.ResponderAsync("vocês não estão no mesmo lugar!");
                    return;
                }

                var porcentagemLife = Convert.ToInt32((playerTarget.Character.VidaAtual / playerTarget.Character.VidaMaxima) * 100);

                await ctx.RespondAsync($"Estado do {playerTarget.Id.Mention()}: {VidaEmTexto(porcentagemLife)}");
                return;
            }

            if (alvo.TryParseID(out ulong id))
            {
                var monster = await banco.CollectionMonsters.Find(x => x.Id == ctx.Channel.Id + id).FirstOrDefaultAsync();
                if (monster != null)
                {
                    if (monster.DateSpawn > DateTime.UtcNow)
                        await ctx.ResponderAsync($"o monstro {Formatter.Bold(monster.Nome)} está morto!");
                    else
                    {
                        var porcentagemLife = Convert.ToInt32((monster.VidaAtual / monster.VidaMaxima) * 100);
                        await ctx.RespondAsync($"Estado do {Formatter.Bold(monster.Nome)}: {VidaEmTexto(porcentagemLife)}");
                    }
                }
                else
                    await ctx.ResponderAsync(Strings.MonstroNaoEncontrado(id));
            }
            else
                await ctx.ResponderAsync(Strings.IdInvalido);
        }

        public string VidaEmTexto(int vidaPorcentagem)
        {
            switch (vidaPorcentagem)
            {
                case int x when x == 100:
                    return "Em excelente condição.";
                case int x when x >= 95:
                    return "Tem alguns aranhões.";
                case int x when x >= 85:
                    return "Tem algumas pequenas feridas e hematomas.";
                case int x when x >= 75:
                    return "Tem alguns ferimentos leves.";
                case int x when x >= 63:
                    return "Tem alguns ferimentos.";
                case int x when x >= 50:
                    return "Tem algumas feridas grandes e arranhões desagradavéis.";
                case int x when x >= 40:
                    return "Parece muito machucado.";
                case int x when x >= 20:
                    return "Em má condição.";
                case int x when x >= 10:
                    return "Está quase morto.";
                case int x when x >= 1:
                    return "Está com um corte feio e sangrando muito por causa das gigantes feridas.";
            };
            return "Parece morto.";
        }
    }
}
