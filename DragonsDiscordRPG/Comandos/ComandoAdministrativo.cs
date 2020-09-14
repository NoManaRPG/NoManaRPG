using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.BancoItens;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
{
    public class ComandoAdministrativo : BaseCommandModule
    {
        [Command("purge")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task PurgeAsync(CommandContext ctx, int quantidade)
            => await ctx.Channel.DeleteMessagesAsync(await ctx.Channel.GetMessagesAsync(quantidade + 1));

        [Command("atualizar")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task Atualizar(CommandContext ctx)
        {
            FilterDefinition<RPJogador> filter = FilterDefinition<RPJogador>.Empty;
            FindOptions<RPJogador> options = new FindOptions<RPJogador>
            {
                BatchSize = 8,
                NoCursorTimeout = false
            };

            using (IAsyncCursor<RPJogador> cursor = await ModuloBanco.ColecaoJogador.FindAsync(filter, options))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<RPJogador> usuarios = cursor.Current;

                    foreach (RPJogador user in usuarios)
                    {
                        user.Personagem.Zona = new RPZona();
                        await ModuloBanco.ColecaoJogador.ReplaceOneAsync(x => x.Id == user.Id, user);
                    }
                }
            }
            await ctx.RespondAsync("Banco foi atualizado com sucesso!");
        }

        [Command("sudo")]
        [RequireOwner]
        public async Task Sudo(CommandContext ctx, DiscordUser member, [RemainingText] string command)
        {
            await ctx.TriggerTypingAsync();
            var invocation = command.Substring(1);
            var cmd = ctx.CommandsNext.FindCommand(invocation, out var args);
            if (cmd == null)
            {
                await ctx.RespondAsync("Comando não encontrado");
                return;
            }

            var cfx = ctx.CommandsNext.CreateFakeContext(member, ctx.Channel, "", "!", cmd, args);
            await ctx.CommandsNext.ExecuteCommandAsync(cfx);
        }

        [Command("restaurarpot")]
        [RequireOwner]
        public async Task GivePot(CommandContext ctx, DiscordUser member)
        {
            using (var session = await ModuloBanco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(member);
                RPPersonagem personagem = jogador.Personagem;

                personagem.Pocoes[0].AddCarga(double.MaxValue);

                await banco.EditJogadorAsync(jogador);
                await session.CommitTransactionAsync();
                await ctx.RespondAsync($"Poções restaurada para {member.Mention}!");

            }
        }

        [Command("matarinimigos")]
        [RequireOwner]
        public async Task matar(CommandContext ctx, DiscordUser member)
        {
            using (var session = await ModuloBanco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(member);
                RPPersonagem personagem = jogador.Personagem;

                personagem.Zona.Monstros = new List<RPMonstro>();

                await banco.EditJogadorAsync(jogador);
                await session.CommitTransactionAsync();
                await ctx.RespondAsync($"Inimigos mortos para {member.Mention}!");

            }
        }

        [Command("teste")]
        [RequireOwner]
        public async Task testeCalc(CommandContext ctx)
        {

            var jog = await ModuloBanco.GetJogadorAsync(ctx);
            jog.Personagem.Zona = null;
            ModuloBanco.ColecaoJogador.ReplaceOne(x => x.Id == ctx.User.Id, jog);
            await ctx.RespondAsync("Atualiado");
            //var numeroRandom = Calculo.SortearValor(0, 100.0);
            //switch (numeroRandom)
            //{
            //    case double n when (n > 0.9):
            //        await ctx.RespondAsync($"N maior que 0.9 {numeroRandom}");
            //        break;
            //    case double n when (n > 0.8):
            //        await ctx.RespondAsync($"N maior que 0.8 {numeroRandom}");
            //        break;
            //    case double n when (n > 0.7):
            //        await ctx.RespondAsync($"N maior que 0.7 {numeroRandom}");
            //        break;
            //    case double n when (n > 0.6):
            //        await ctx.RespondAsync($"N maior que 0.6 {numeroRandom}");
            //        break;
            //    case double n when (n > 0.5):
            //        await ctx.RespondAsync($"N maior que 0.5 {numeroRandom}");
            //        break;
            //    case double n when (n > 0.4):
            //        await ctx.RespondAsync($"N maior que 0.4 {numeroRandom}");
            //        break;
            //    case double n when (n > 0.3):
            //        await ctx.RespondAsync($"N maior que 0.3 {numeroRandom}");
            //        break;
            //    case double n when (n > 0.2):
            //        await ctx.RespondAsync($"N maior que 0.2 {numeroRandom}");
            //        break;
            //}

            //var frutas = new List<Fruta> {
            //new Fruta { Cor = 1, Nome = "Morango" },
            //    new Fruta { Cor = 1, Nome = "Framboesa" },
            //    new Fruta { Cor = 1, Nome = "Cereja" },
            //    new Fruta { Cor = 2, Nome = "Limão" },
            //    new Fruta { Cor = 2, Nome = "Melancia" }        };

            //var grupos = frutas.GroupBy(f => f.Cor);

            //Console.WriteLine("Imprimindo grupos");
            //Console.WriteLine("-----------------------");
            //foreach (var nomeGrupo in grupos.Select(g => g.Key))
            //{
            //    Console.WriteLine(nomeGrupo);
            //}

            //Console.WriteLine("-----------------------");
            //var pesquisa = grupos.Where(x => x.Key <= 2).ToList();
            //foreach (var listaDeFrutasPorCor in pesquisa)
            //{
            //    Console.WriteLine("Imprimindo frutas da cor: " + listaDeFrutasPorCor.Key);
            //    Console.WriteLine("-----------------------");
            //    foreach (var fruta in listaDeFrutasPorCor)
            //    {
            //        Console.WriteLine(fruta.Nome);
            //    }
            //    Console.WriteLine("-----------------------");
            //}
        }
    }
}
