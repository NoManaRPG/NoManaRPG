using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class LookCommand : BaseCommandModule
    {
        public Database banco;

        [Command("olhar")]
        [Aliases("look", "l")]
        [Description("Permite ver os atributos de algum monstro ou a vida de algum jogador.")]
        [Usage("olhar [ #ID | @menção")]
        [Example("olhar #1", "Permite olhar os atributos do monstro de #ID 1.")]
        public async Task LookCommandAsync(CommandContext ctx, string targetString = "")
        {
            //var timer = new Stopwatch();
            //timer.Start();

            //await ctx.TriggerTypingAsync();
            //var player = await banco.FindPlayerAsync(ctx);
            //if (player == null)
            //{
            //    await ctx.ResponderAsync(Strings.NovoJogador);
            //    return;
            //}

            //if (player.Character.Localization.ChannelId != ctx.Channel.Id)
            //{
            //    await ctx.ResponderAsync(Strings.LocalDiferente(ctx.Channel.Name));
            //    return;
            //}

            //if (string.IsNullOrWhiteSpace(alvo))
            //{
            //    await ctx.ResponderAsync($"você precisa informar {Formatter.Bold("alguém")} para olhar");
            //    return;
            //}

            ////Checks if is user or monster.
            //var regex = new Regex(@"<@!?(\d+)>", RegexOptions.ECMAScript);
            //var isUser = regex.IsMatch(alvo);
            //if (isUser)
            //{
            //    var match = regex.Match(alvo);
            //    var userId = ulong.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            //    var playerTarget = await banco.FindPlayerAsync(userId);
            //    if (playerTarget == null)
            //    {
            //        await ctx.ResponderAsync("o jogador ainda não criou um personagem!");
            //        return;
            //    }

            //    if (playerTarget.Character.Localization.ChannelId != player.Character.Localization.ChannelId)
            //    {
            //        await ctx.ResponderAsync("vocês não estão no mesmo lugar!");
            //        return;
            //    }

            //    var porcentagemLife = Convert.ToInt32((playerTarget.Character.LifePoints.CurrentValue / playerTarget.Character.LifePoints.MaxValue) * 100);

            //    await ctx.RespondAsync($"Estado do {playerTarget.Id.Mention()}: {VidaEmTexto(porcentagemLife)}");
            //    return;
            //}

            //if (alvo.TryParseID(out ulong id))
            //{
            //    var monster = await banco.CollectionMonsters.Find(x => x.Id == ctx.Channel.Id + id).FirstOrDefaultAsync();
            //    if (monster != null)
            //    {
            //        if (monster.DateSpawn > DateTime.UtcNow)
            //            await ctx.ResponderAsync($"o monstro {Formatter.Bold(monster.Nome)} está morto!");
            //        else
            //        {
            //            var porcentagemLife = Convert.ToInt32((monster.Life.CurrentValue / monster.Life.MaxValue) * 100);
            //            await ctx.RespondAsync($"Estado do {Formatter.Bold(monster.Nome)}: {VidaEmTexto(porcentagemLife)}");
            //        }
            //    }
            //    else
            //        await ctx.ResponderAsync(Strings.MonstroNaoEncontrado(id));
            //}
            //else
            //    await ctx.ResponderAsync(Strings.IdInvalido);


            //timer.Stop();
        }

        [Command("olhar")]
        [Example("olhar @Talion", "Permite olhar a vida do jogador mencionado.")]
        [Priority(1)]
        public async Task LookCommandAsync(CommandContext ctx, DiscordUser target)
        {
            //var timer = new Stopwatch();
            //timer.Start();
            //await ctx.ResponderAsync(targetString + " é uma string!");
            //await ctx.TriggerTypingAsync();
            //var player = await banco.FindPlayerAsync(ctx);
            //if (player == null)
            //{
            //    await ctx.ResponderAsync(Strings.NovoJogador);
            //    return;
            //}



            //await ctx.TriggerTypingAsync();
            //var player = await banco.FindPlayerAsync(ctx.User.Id);
            //if (!await ctx.HasPlayerAsync(player))
            //    return;

            //if (player.Character.Localization.ChannelId != ctx.Channel.Id)
            //{
            //    await ctx.ResponderAsync(Strings.LocalDiferente(ctx.Channel.Name));
            //    return;
            //}

            //if (string.IsNullOrWhiteSpace(alvo))
            //{
            //    await ctx.ResponderAsync($"você precisa informar {Formatter.Bold("alguém")} para olhar");
            //    return;
            //}

            ////Checks if is user or monster.
            //var regex = new Regex(@"<@!?(\d+)>", RegexOptions.ECMAScript);
            //var isUser = regex.IsMatch(alvo);
            //if (isUser)
            //{
            //    var match = regex.Match(alvo);
            //    var userId = ulong.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            //    var playerTarget = await banco.FindPlayerAsync(userId);
            //    if (playerTarget == null)
            //    {
            //        await ctx.ResponderAsync("o jogador ainda não criou um personagem!");
            //        return;
            //    }

            //    if (playerTarget.Character.Localization.ChannelId != player.Character.Localization.ChannelId)
            //    {
            //        await ctx.ResponderAsync("vocês não estão no mesmo lugar!");
            //        return;
            //    }

            //    var porcentagemLife = Convert.ToInt32((playerTarget.Character.LifePoints.CurrentValue / playerTarget.Character.LifePoints.MaxValue) * 100);

            //    await ctx.RespondAsync($"Estado do {playerTarget.Id.Mention()}: {VidaEmTexto(porcentagemLife)}");
            //    return;
            //}

            //if (alvo.TryParseID(out ulong id))
            //{
            //    var monster = await banco.CollectionMonsters.Find(x => x.Id == ctx.Channel.Id + id).FirstOrDefaultAsync();
            //    if (monster != null)
            //    {
            //        if (monster.DateSpawn > DateTime.UtcNow)
            //            await ctx.ResponderAsync($"o monstro {Formatter.Bold(monster.Nome)} está morto!");
            //        else
            //        {
            //            var porcentagemLife = Convert.ToInt32((monster.Life.CurrentValue / monster.Life.MaxValue) * 100);
            //            await ctx.RespondAsync($"Estado do {Formatter.Bold(monster.Nome)}: {VidaEmTexto(porcentagemLife)}");
            //        }
            //    }
            //    else
            //        await ctx.ResponderAsync(Strings.MonstroNaoEncontrado(id));
            //}
            //else
            //    await ctx.ResponderAsync(Strings.IdInvalido);


            //timer.Stop();
        }
    }
}
