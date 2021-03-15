using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WafclastRPG.Bot.Attributes;
using WafclastRPG.Bot.Entities;
using WafclastRPG.Bot.Extensions;
using WafclastRPG.Game;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Bot.Commands.GeneralCommands
{
    class AttackCommand : BaseCommandModule
    {
        public Database banco;

        [Command("atacar")]
        [Aliases("at")]
        [Description("Permite atacar um monstro ou um jogador, note que os dois precisam estar na mesma localização.")]
        [Usage("atacar [ #ID 1 | @menção ]")]
        [Example("atacar #1", "Você ataca o monstro de #ID 1.")]
        [Example("atacar @Talion", "Você ataca o jogador mencionado.")]
        public async Task AttackCommandAsync(CommandContext ctx, string alvo = "")
        {
            var timer = new Stopwatch();
            timer.Start();

            await ctx.TriggerTypingAsync();

            if (string.IsNullOrWhiteSpace(alvo))
            {
                await ctx.ResponderAsync("não é possível atacar o vento! Por favor especifique um alvo.");
                return;
            }

            //Checks if is user or monster.
            var regex = new Regex(@"<@!?(\d+)>", RegexOptions.ECMAScript);
            if (regex.IsMatch(alvo))
            {
                var match = regex.Match(alvo);
                var targetId = ulong.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

                if (targetId == ctx.User.Id)
                {
                    await ctx.ResponderAsync("por que você se atacaria?");
                    return;
                }

                #region PvP

                var rd = new Random();
                var embed = new DiscordEmbedBuilder();
                Task<Response> result;
                using (var session = await this.banco.StartDatabaseSessionAsync())
                    result = await session.WithTransactionAsync(async (s, ct) =>
                    {
                        //Find player
                        var player = await session.FindPlayerAsync(ctx.User.Id);
                        if (player == null)
                            return Task.FromResult(new Response() { IsPlayerFound = false });

                        //Is the channel is a map
                        var map = await session.FindMapAsync(ctx.Channel.Id);
                        if (map == null)
                            return Task.FromResult(new Response() { IsChannelValid = false });

                        //Is using command in wrong channel
                        if (map.Id != player.Character.LocalId)
                            return Task.FromResult(new Response() { IsCommandInWrongChannel = true });

                        //Is the map is a city
                        if (map.Tipo == WafclastMapaType.Cidade)
                            return Task.FromResult(new Response() { IsMapCity = true });

                        //Find target
                        var target = await session.FindPlayerAsync(targetId);
                        if (target == null)
                            return Task.FromResult(new Response() { IsTargetFound = false });

                        //Is target different channel
                        if (target.Character.LocalId != player.Character.LocalId)
                            return Task.FromResult(new Response() { IsTargetDiffentChannel = true, TargetId = target.Id });

                        //Combat
                        var str = new StringBuilder();
                        embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}]", iconUrl: ctx.User.AvatarUrl);

                        if (target.Character.Karma == 0)
                            player.Character.Karma -= 1;
                        var playerDamage = rd.Sortear(player.Character.Ataque);
                        var _isTargetDead = target.Character.ReceberDano(playerDamage);
                        str.AppendLine($"{target.Mention()} recebeu {playerDamage:N2}({Emojis.Adaga}) de dano.");

                        if (_isTargetDead)
                        {
                            if (target.Character.Karma == 0)
                                player.Character.Karma -= 10;
                            str.AppendLine($"{Emojis.CrossBone} {target.Mention()} morreu! {Emojis.CrossBone}");
                            var exp = target.Character.Level * 3;
                            str.AppendLine($"+{exp} exp");
                            if (player.Character.ReceberExperiencia(exp))
                                str.AppendLine($"{Emojis.Up} {player.Mention()} evoluiu de nível!");
                        }

                        await player.SaveAsync();
                        await target.SaveAsync();
                        return Task.FromResult(new Response(str, target.Id));
                    });
                var _response = await result;

                if (_response.IsPlayerFound == false)
                {
                    await ctx.ResponderAsync(Strings.NovoJogador);
                    return;
                }

                if (_response.IsChannelValid == false)
                {
                    await ctx.ResponderAsync("você não está em um canal jogável!");
                    return;
                }

                if (_response.IsCommandInWrongChannel)
                {
                    await ctx.ResponderAsync("você precisa estar no mesmo canal da sua localização para usar este comando!");
                    return;
                }

                if (_response.IsMapCity)
                {
                    await ctx.ResponderAsync("você não pode causar problemas dentro dos muros da cidade, os guardas lhe matariam!");
                    return;
                }

                if (_response.IsTargetFound == false)
                {
                    await ctx.ResponderAsync("parece que o seu alvo ainda não criou um personagem!");
                    return;
                }

                if (_response.IsTargetDiffentChannel)
                {
                    await ctx.ResponderAsync($"parece que {Player.UserMention(_response.TargetId)} não está em {ctx.Channel.Name}! Não é possível atacar.");
                    return;
                }

                embed.WithColor(DiscordColor.DarkRed);
                embed.WithTitle("Combate PvP");
                embed.WithDescription(_response.BattleResult.ToString());
                timer.Stop();
                embed.WithFooter($"Tempo de resposta: {timer.Elapsed.Seconds}.{timer.ElapsedMilliseconds + ctx.Client.Ping}s.");
                await ctx.RespondAsync($"{ctx.User.Mention} {Emojis.Adaga} { Player.UserMention(_response.TargetId)}", embed: embed.Build());

                #endregion
            }
            else if (alvo.TryParseID(out ulong id))
            {
                #region PvM

                var rd = new Random();
                var embed = new DiscordEmbedBuilder();
                Task<Response> result;
                using (var session = await this.banco.StartDatabaseSessionAsync())
                    result = await session.WithTransactionAsync(async (s, ct) =>
                    {
                        //Find player
                        var player = await session.FindPlayerAsync(ctx.User.Id);
                        if (player == null)
                            return Task.FromResult(new Response() { IsPlayerFound = false });

                        //Is the channel is a map
                        var map = await session.FindMapAsync(ctx.Channel.Id);
                        if (map == null)
                            return Task.FromResult(new Response() { IsChannelValid = false });

                        //Is using command in wrong channel
                        if (map.Id != player.Character.LocalId)
                            return Task.FromResult(new Response() { IsCommandInWrongChannel = true });

                        //Is the map is a city
                        if (map.Tipo == WafclastMapaType.Cidade)
                            return Task.FromResult(new Response() { IsMapCity = true });

                        //Find target
                        var target = await session.FindMonsterAsync(player.Character.LocalId + id);
                        if (target == null)
                            return Task.FromResult(new Response() { IsTargetFound = false });

                        if (target.DateSpawn > DateTime.UtcNow)
                            return Task.FromResult(new Response() { IsTargetAlreadyDead = true, TargetName = target.Nome });

                        //Combat
                        var str = new StringBuilder();
                        embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}]", iconUrl: ctx.User.AvatarUrl);

                        var playerDamage = rd.Sortear(player.Character.Ataque);
                        var _isTargetDead = target.ReceberDano(playerDamage);
                        str.AppendLine($"{target.Nome} recebeu {playerDamage:N2} de dano.");

                        if (_isTargetDead)
                        {
                            player.Character.ReceberExperiencia(target.Exp);
                            if (player.Character.Karma < 0)
                                player.Character.Karma += 1;

                            str.AppendLine($"{Emojis.CrossBone} {target.Nome} morreu! {Emojis.CrossBone}");
                            str.AppendLine($"+{target.Exp} exp");


                            await player.SaveAsync();
                            await session.SaveMonsterAsync(target);
                            return Task.FromResult(new Response(str, target.Nome));
                        }

                        var targetDamage = rd.Sortear(target.Ataque);
                        str.AppendLine($"{ctx.User.Mention} recebeu {targetDamage:N2} de dano.");

                        if (player.Character.ReceberDano(targetDamage))
                            str.AppendLine($"{ctx.User.Mention} morreu!");

                        await player.SaveAsync();
                        await session.SaveMonsterAsync(target);

                        return Task.FromResult(new Response(str, target.Nome));
                    });
                var _response = await result;


                if (_response.IsPlayerFound == false)
                {
                    await ctx.ResponderAsync(Strings.NovoJogador);
                    return;
                }

                if (_response.IsChannelValid == false)
                {
                    await ctx.ResponderAsync("você não está em um canal jogável!");
                    return;
                }

                if (_response.IsCommandInWrongChannel)
                {
                    await ctx.ResponderAsync("você precisa estar no mesmo canal da sua localização para usar este comando!");
                    return;
                }

                if (_response.IsMapCity)
                {
                    await ctx.ResponderAsync("você não pode causar problemas dentro dos muros da cidade, os guardas lhe matariam!");
                    return;
                }

                if (_response.IsTargetFound == false)
                {
                    await ctx.ResponderAsync("você procura mais não encontra nenhum monstro!");
                    return;
                }

                if (_response.IsTargetAlreadyDead)
                {
                    await ctx.ResponderAsync($"{Formatter.Bold(_response.TargetName)} já está morto! Tente atacar outro.");
                    return;
                }

                embed.WithColor(DiscordColor.DarkRed);
                embed.WithTitle("Combate PvM");
                embed.WithDescription(_response.BattleResult.ToString());
                timer.Stop();
                embed.WithFooter($"Tempo de resposta: {timer.Elapsed.Seconds}.{timer.ElapsedMilliseconds + ctx.Client.Ping}s.");
                await ctx.RespondAsync($"{ctx.User.Mention} {Emojis.Adaga} {Formatter.Bold(_response.TargetName)}", embed: embed.Build());

                #endregion PvM
            }
            else
                await ctx.ResponderAsync(Strings.IdInvalido);
        }

        private class Response
        {
            public StringBuilder BattleResult;
            public ulong TargetId = 0;
            public string TargetName;
            public bool IsPlayerFound = true;
            public bool IsChannelValid = true;
            public bool IsCommandInWrongChannel = false;
            public bool IsMapCity = false;
            public bool IsTargetFound = true;
            public bool IsTargetDiffentChannel = false;
            public bool IsTargetAlreadyDead = false;

            public Response()
            {
            }

            public Response(StringBuilder battleResult, string targetName)
            {
                BattleResult = battleResult;
                TargetName = targetName;
            }

            public Response(StringBuilder battleResult, ulong id)
            {
                BattleResult = battleResult;
                TargetId = id;
            }
        }
    }
}
