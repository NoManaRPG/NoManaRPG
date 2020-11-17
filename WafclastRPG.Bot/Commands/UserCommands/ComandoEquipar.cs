using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades.Itens;

namespace WafclastRPG.Bot.Comandos.Acao
{
    public class ComandoEquipar : BaseCommandModule
    {
        public Banco banco;

        [Command("equipar")]
        [Description("Permite equipar um item que está na mochila.")]
        [Example("equipar #0", "Equipa o item de id *#0* que é o primeiro item da mochila.")]
        [Usage("equipar <#ID>")]
        public async Task ComandoEquiparAsync(CommandContext ctx, string stringIndex = "0")
        {
            await Task.CompletedTask;
            using (await banco.LockAsync(ctx.User.Id))
            {
                if (!stringIndex.TryParseID(out int index))
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, o ID precisa ser númerico!");
                    return;
                }

                var jogador = await banco.GetJogadorAsync(ctx);
                var per = jogador.Personagem;

                if (per.Mochila.Itens.Count == 0)
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, você precisa de itens na mochila para poder equipar algo!");
                    return;
                }

                if (per.Mochila.TryRemoveItem(index, 1, out var itemId))
                {
                    var item = await banco.GetItemAsync(itemId.ItemId);
                    if (per.TryEquiparItem(item))
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, você equipou {item.Nome.Titulo().Bold()}!");
                        await jogador.Salvar();
                    }
                    else
                        await ctx.RespondAsync($"{ctx.User.Mention}, você precisa reniver o item equipado antes!");

                }
            }
        }
    }
}
