using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Database;
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
            var player = await banco.FindPlayerAsync(ctx.User.Id);
            if (!await ctx.HasPlayerAsync(player))
                return;

            if (player.Character.LocalId != ctx.Channel.Id)
            {
                await ctx.ResponderAsync(Strings.LocalDiferente(ctx.Channel.Name));
                return;
            }

            if (!alvo.TryParseID(out ulong id))
            {
                await ctx.ResponderAsync(Strings.IdInvalido);
                return;
            }

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

        private class Response
        {
            public bool IsMonstroEncontrado = false;
            public bool IsMonstroJaMorto = false;
            public bool IsMonstroMortoEmCombate = false;
            public bool IsJogadorMortoEmCombate = false;
            public StringBuilder BattleResult;
            public WafclastMonster monster;
        }
    }
}
