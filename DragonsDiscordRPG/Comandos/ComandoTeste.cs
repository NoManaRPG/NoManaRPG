using DragonsDiscordRPG.Entidades;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
{
    public class ComandoTeste : BaseCommandModule
    {
        [Command("dolar")]
        [Aliases("d")]
        public async Task ComandoAtacarAb(CommandContext ctx)
        {
            //try
            //{
            //    using (var session = await ModuloBanco.Cliente.StartSessionAsync())
            //    {

            //        //var result = await session.WithTransactionAsync((s, ct) =>
            //        //{
            //        session.StartTransaction();
            //        var database = session.Client.GetDatabase("zaynbot");
            //        var collection = database.GetCollection<Jogador>("Jogador");
            //        var f = collection.Find(session, x => x.Id == ctx.User.Id).FirstOrDefault();
            //        if (f == null)
            //        {
            //            Jogador j = new Jogador()
            //            {
            //                Id = ctx.User.Id,
            //                //dolar = 1
            //            };
            //            collection.InsertOne(session, j);
            //        }
            //        else
            //        {
            //           // f.dolar++;
            //            collection.ReplaceOne(session, x => x.Id == f.Id, f);

            //        }
            //        await Task.Delay(3000);
            //        await ctx.RespondAsync($"{collection.Find(session, x => x.Id == ctx.User.Id).FirstOrDefault()} dolares");
            //        await session.CommitTransactionAsync();
            //        //try
            //        //{
            //        //    await session.AbortTransactionAsync(token);
            //        //}
            //        //catch (Exception ex)
            //        //{
            //        //    await ctx.RespondAsync($"TRY ABORT, {ex.Message}");
            //        //}
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await ctx.RespondAsync($"O ultimo comando não foi processado! Digite mais lentamente!");
            //}
            //// await ctx.RespondAsync($"{ModuloBanco.ColecaoJogador.Find(x => x.Id == ctx.User.Id).FirstOrDefault().dolar} dolares");
        }

        //[Command("m")]
        //public async Task fasd(CommandContext ctx)
        //{
        //    await ctx.RespondAsync($"{ModuloBanco.ColecaoJogador.Find(x => x.Id == ctx.User.Id).FirstOrDefault().dolar} dolares");
        //}

        //[Command("r")]
        //public async Task remover(CommandContext ctx)
        //{
        //    try
        //    {
        //        using (var session = await ModuloBanco.Cliente.StartSessionAsync())
        //        {
        //            session.StartTransaction();
        //            var database = session.Client.GetDatabase("Dragon");
        //            var collection = database.GetCollection<Jogador>("Jogador");
        //            var jogador = await collection.Find(session, x => x.Id == ctx.User.Id).FirstOrDefaultAsync();
        //            if (jogador == null)
        //                jogador = new Jogador(ctx.User.Id)
        //                {
        //                    dolar = 0,
        //                };
        //            jogador.dolar++;
        //            await collection.ReplaceOneAsync(session, x => x.Id == jogador.Id, jogador, new ReplaceOptions { IsUpsert = true });

        //            await Task.Delay(10000);
        //            await session.CommitTransactionAsync();
        //            //try
        //            //{
        //            //    await session.AbortTransactionAsync(token);
        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //    await ctx.RespondAsync($"TRY ABORT, {ex.Message}");
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await ctx.RespondAsync($"O ultimo comando ainda não foi processado! Aguarde! {ctx.User.Mention}");
        //        return;
        //    }
        //    await ctx.RespondAsync($"{ModuloBanco.ColecaoJogador.Find(x => x.Id == ctx.User.Id).FirstOrDefault().dolar} dolares {ctx.User.Mention}");
        //}

        [Command("t")]
        public async Task gg(CommandContext ctx)
        {
            await ctx.RespondAsync((3 < 3).ToString());
            //ModuloBanco.ColecaoJogador.UpdateOne(x => x.Id == ctx.User.Id,
            //    new UpdateDefinitionBuilder<Jogador>().Inc(x => x.RoleId, 5));
        }

        [Command("addvida")]
        [RequireUserPermissions(DSharpPlus.Permissions.Administrator)]
        public async Task add(CommandContext ctx)
        {
            var g = await ModuloBanco.GetJogadorAsync(ctx);
            g.Personagem.Vida.Adicionar(double.MaxValue);
            ModuloBanco.ColecaoJogador.ReplaceOne(x => x.Id == g.Id, g);
            //ModuloBanco.ColecaoJogador.UpdateOne(x => x.Id == ctx.User.Id,
            //    new UpdateDefinitionBuilder<Jogador>().Inc(x => x.RoleId, 5));
        }

        [Command("xp-efetiva")]
        public async Task XpEfetiva(CommandContext ctx, int levelJogador, int levelInimigo)
        {
            await ctx.RespondAsync(Calculo.CalcularEfetividadeXP(levelJogador, levelInimigo).ToString());
        }
    }
}
