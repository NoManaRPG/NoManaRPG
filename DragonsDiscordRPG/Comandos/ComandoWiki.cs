using DragonsDiscordRPG.Entidades;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
{
    [Group("wiki")]
    [Cooldown(1, 5, CooldownBucketType.User)]
    public class ComandoWiki : BaseCommandModule
    {

        [GroupCommand()]
        public async Task ExecuteGroupAsync(CommandContext ctx)
        {
            int pageSize = 10;
            int currentPage = (int)1;
            if (currentPage != 0)
                currentPage = currentPage - 1;
            double totalDocuments = await ModuloBanco.ColecaoWiki.CountDocumentsAsync(FilterDefinition<Wiki>.Empty);
            double totalPages = Math.Ceiling(totalDocuments / pageSize);

            List<Wiki> list = new List<Wiki>
                (ModuloBanco.ColecaoWiki.Find(FilterDefinition<Wiki>.Empty)
               .Skip(currentPage * pageSize)
               .Limit(pageSize).ToList());
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
                str.AppendLine($"{list[i].Nome} - ID: {list[i].Id}");
            await ctx.RespondAsync(str.ToString());
        }

        [Command("ler")]
        [Cooldown(1, 5, CooldownBucketType.User)]
        public async Task WikiLerAsync(CommandContext ctx, string wiki = null, int pagina = 0)
        {
            if (string.IsNullOrEmpty(wiki))
            {
                await ctx.RespondAsync("Você precisa informar uma wiki pelo o ID");
                return;
            }
            var wikipedia = await ModuloBanco.ColecaoWiki.Find(x => x.Id == wiki).FirstOrDefaultAsync();
            if (wikipedia == null)
            {
                await ctx.RespondAsync("Wiki não encontrada, você informou o ID corretamente?");
                return;
            }

            await ctx.RespondAsync(wikipedia.Texto[pagina]);
        }

        [Command("criar")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task WikiCriarAsync(CommandContext ctx)
        {
            Wiki wiki = new Wiki()
            {
                Id = "asdxz",
                Nome = "Wiki",
            };
            wiki.Texto = new List<string>();
            wiki.Texto.Add("wiki");
            wiki.Texto.Add("wiki");
            await ModuloBanco.ColecaoWiki.InsertOneAsync(wiki);
            await ctx.RespondAsync("Criado!");
        }
    }
}
