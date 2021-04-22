using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using WafclastRPG.Entities;
using WafclastRPG.DataBases;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.AdminCommands
{
    public class CreateFabricationCommand : BaseCommandModule
    {
        public DataBase database;
        public TimeSpan timeoutoverride = TimeSpan.FromMinutes(2);

        [Command("criarfabricacao")]
        [Description("Permite criar uma nova fabricação para um item.")]
        [Usage("criarfabricacao")]
        [RequireOwner]
        public async Task CreateFabricationCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var fabricacao = new WafclastFabrication();
                    var name = await ctx.WaitForStringAsync("Nome da Fabricação", database, timeoutoverride);
                    if (name.TimedOut)
                        return new Response(Messages.ComandoCancelado);
                    fabricacao.Name = name.Value;

                    var quantity = await ctx.WaitForIntAsync("Quantidade fabricada", database, timeoutoverride);
                    if (quantity.TimedOut)
                        return new Response(Messages.ComandoCancelado);
                    fabricacao.Quantity = (ulong)quantity.Value;

                    var experience = await ctx.WaitForDoubleAsync("Quantidade de experiencia", database, timeoutoverride);
                    if (experience.TimedOut)
                        return new Response(Messages.ComandoCancelado);
                    fabricacao.Experience = experience.Value;

                    var tipo = await ctx.WaitForStringAsync("Qual tipo? `Culinária`", database, timeoutoverride);
                    if (tipo.TimedOut)
                        return new Response(Messages.ComandoCancelado);
                    switch (tipo.Value)
                    {
                        case "culinária":
                        case "culinaria":
                            fabricacao.Type = FabricationType.Cook;
                            break;
                        default:
                            return new Response(Messages.ComandoCancelado);
                    }

                    var requiredLevel = await ctx.WaitForIntAsync("Nível requerido", database, timeoutoverride);
                    if (requiredLevel.TimedOut)
                        return new Response(Messages.ComandoCancelado);
                    fabricacao.RequiredLevel = requiredLevel.Value;

                    var chance = await ctx.WaitForDoubleAsync("Chance", database, timeoutoverride);
                    if (chance.TimedOut)
                        return new Response(Messages.ComandoCancelado);
                    fabricacao.Chance = chance.Value / 100;

                    bool continuar = true;
                    List<Requirement> reqs = new List<Requirement>();
                    while (continuar)
                    {
                        var req = new Requirement();

                        var reqname = await ctx.WaitForStringAsync("Nome do item necessario para fazer o item", database, timeoutoverride);
                        if (reqname.TimedOut)
                            return new Response(Messages.ComandoCancelado);
                        req.Name = reqname.Value;

                        var reqQuantity = await ctx.WaitForIntAsync("Quantos deste item?", database, timeoutoverride);
                        if (reqQuantity.TimedOut)
                            return new Response(Messages.ComandoCancelado);
                        req.Quantity = (ulong)reqQuantity.Value;
                        reqs.Add(req);

                        var querContinuar = await ctx.WaitForStringAsync("Deseja adicionar outro requisito para fabricar este item?", database, timeoutoverride);
                        if (querContinuar.TimedOut)
                            return new Response(Messages.ComandoCancelado);

                        if (querContinuar.Value != "sim")
                            continuar = false;
                    }
                    fabricacao.RequiredItems = reqs;

                    await session.ReplaceAsync(fabricacao);


                    return new Response("Fabricação criada!");
                });

            if (!string.IsNullOrWhiteSpace(response.Message))
                await ctx.ResponderAsync(response.Message);

            if (response.Embed != null)
                await ctx.ResponderAsync(response.Embed?.Build());
        }
    }
}
