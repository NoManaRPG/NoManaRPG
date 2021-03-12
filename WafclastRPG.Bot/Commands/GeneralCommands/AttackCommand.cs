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
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Database;
using WafclastRPG.Bot.Entidades;
using WafclastRPG.Bot.Extensions;
using WafclastRPG.Game;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Bot.Commands.GeneralCommands
{
    class AttackCommand : BaseCommandModule
    {
        public BotDatabase banco;
        public Formulas formulas;

        [Command("atacar")]
        [Aliases("at")]
        [Description("Permite atacar um monstro do local atual.")]
        [Usage("atacar [ #ID ]")]
        [Example("atacar 1", "Você ataca o monstro de ID 1.")]
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
                        var playerDamage = formulas.Sortear(player.Character.Ataque);
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
                    await ctx.ResponderAsync($"parece que {BotJogador.UserMention(_response.TargetId)} não está em {ctx.Channel.Name}! Não é possível atacar.");
                    return;
                }

                embed.WithColor(DiscordColor.DarkRed);
                embed.WithTitle("Combate PvP");
                embed.WithDescription(_response.BattleResult.ToString());
                timer.Stop();
                embed.WithFooter($"Tempo de resposta: {timer.Elapsed.Seconds}.{timer.ElapsedMilliseconds + ctx.Client.Ping}s.");
                await ctx.RespondAsync($"{ctx.User.Mention} {Emojis.Adaga} { BotJogador.UserMention(_response.TargetId)}", embed: embed.Build());
            }
            else if (alvo.TryParseID(out ulong id))
            {
                var player = await banco.FindPlayerAsync(ctx.User.Id);
                if (!await ctx.HasPlayerAsync(player))
                    return;

                if (player.Character.LocalId != ctx.Channel.Id)
                {
                    await ctx.ResponderAsync(Strings.LocalDiferente(ctx.Channel.Name));
                    return;
                }

                //if (!alvo.TryParseID(out ulong id))
                //{
                //    await ctx.ResponderAsync(Strings.IdInvalido);
                //    return;
                //}

                Task<Response> result;
                using (var session = await this.banco.StartDatabaseSessionAsync())
                {
                    result = await session.WithTransactionAsync(async (s, ct) =>
                    {
                        var r = new Response();
                        var player = await session.FindPlayerAsync(ctx.User);

                        var monster = await session.FindMonsterAsync(player.Character.LocalId + id);
                        if (monster == null)
                            return Task.FromResult(r);
                        r.IsMonstroEncontrado = true;


                        if (monster.DateSpawn > DateTime.UtcNow)
                        {
                            r.IsMonstroJaMorto = true;
                            r.monster = monster;
                            return Task.FromResult(r);
                        }
                        r.IsMonstroJaMorto = false;

                        //Calc monster
                        var str = new StringBuilder();
                        var playerDamage = formulas.Sortear(player.Character.Ataque);
                        str.AppendLine($"{monster.Nome} recebeu {playerDamage:N2} de dano.");
                        r.IsMonstroMortoEmCombate = monster.ReceberDano(playerDamage);
                        if (r.IsMonstroMortoEmCombate)
                        {
                            str.AppendLine($"{monster.Nome} morreu!");
                            str.AppendLine($"+{monster.Exp} exp");
                            if (player.Character.Karma < 0)
                                player.Character.Karma += 1;
                            player.Character.ReceberExperiencia(monster.Exp);
                        }

                        //Calc player
                        var monsterDamage = formulas.Sortear(monster.Ataque);
                        str.AppendLine($"{ctx.User.Mention} recebeu {monsterDamage:N2} de dano.");
                        r.IsJogadorMortoEmCombate = player.Character.ReceberDano(monsterDamage);
                        if (r.IsJogadorMortoEmCombate)
                        {
                            str.AppendLine($"{ctx.User.Mention} morreu!");
                            player.Character.ReceberVida(decimal.MaxValue);
                        }

                        r.BattleResult = str;
                        await player.SaveAsync();
                        await session.SaveMonsterAsync(monster);

                        return Task.FromResult(r);
                    });
                };
                var _response = await result;

                if (_response.IsMonstroEncontrado == false)
                {
                    await ctx.ResponderAsync(Strings.MonstroNaoEncontrado(id));
                    return;
                }

                if (_response.IsMonstroJaMorto)
                {
                    await ctx.ResponderAsync($"não é possivel atacar {Formatter.Bold(_response.monster.Nome)} pois o mesmo se encontra {Formatter.Bold("morto")}!");
                    return;
                }

                var embed = new DiscordEmbedBuilder();
                embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}]", iconUrl: ctx.User.AvatarUrl);
                embed.WithColor(DiscordColor.DarkRed);
                embed.WithTitle("Combate");
                embed.WithDescription(_response.BattleResult.ToString());
                timer.Stop();
                embed.WithFooter($"Tempo de resposta: {timer.Elapsed.Seconds}.{timer.ElapsedMilliseconds + ctx.Client.Ping}s.");
                await ctx.ResponderAsync(embed.Build());
            }
            else
                await ctx.ResponderAsync(Strings.MonstroNaoEncontrado(id));
        }

        private class Response
        {
            public StringBuilder BattleResult;
            public ulong TargetId = 0;
            public bool IsPlayerFound = true;
            public bool IsChannelValid = true;
            public bool IsCommandInWrongChannel = false;
            public bool IsMapCity = false;
            public bool IsTargetFound = true;
            public bool IsTargetDiffentChannel = false;

            public bool IsMonstroEncontrado = false;
            public bool IsMonstroJaMorto = false;
            public bool IsMonstroMortoEmCombate = false;
            public bool IsJogadorMortoEmCombate = false;

            public WafclastMonster monster;

            public Response()
            {
            }

            public Response(StringBuilder battleResult, ulong id)
            {
                BattleResult = battleResult;
                TargetId = id;
            }
        }
    }
}
