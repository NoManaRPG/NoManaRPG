using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades.Itens;
using WafclastRPG.Game.Extensoes;

namespace WafclastRPG.Bot.Comandos.Exibir
{
    public class ComandoExaminar : BaseCommandModule
    {
        public Banco banco;

        [Command("examinar")]
        [Description("Permite examinar um item, mostrando seus atributos.")]
        [Example("examinar #0", "Examina o item de id *#0* que é o primeiro item da mochila.")]
        [Usage("examinar <#ID>")]
        public async Task ComandoExaminarAsync(CommandContext ctx, string stringIndex = "#0")
        {
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
                    await ctx.RespondAsync($"{ctx.User.Mention}, você precisa de itens na mochila para poder equipar!");
                    return;
                }
            }
            //    stringId.TryParseID(out int index);
            //    Math.Clamp(index, 0, personagem.Mochila.Itens.Count - 1);
            //    if (personagem.Mochila.TryGetItem(index, out var item))
            //    {
            //        var descricao = ItemDescricao(item).Criar(ctx);
            //        await ctx.RespondAsync(embed: descricao.Build());
            //    }
            //    else
            //        await ctx.RespondAsync($"{ctx.User.Mention}, item não encontrado na mochila!");
            //}

            //public static DiscordEmbedBuilder ItemDescricao(WafclastItem item)
            //{
            //    DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            //    embed.WithTitle($" {item.Nome.Titulo()}");
            //    embed.WithColor(DiscordColor.Azure);
            //    StringBuilder str = new StringBuilder();

            //    str.AppendLine($"Ocupa {item.OcupaEspaco} espaço.");
            //    str.AppendLine($"{Emoji.Coins} {item.PrecoVenda / 2}V.");

            //    switch (item)
            //    {
            //        case WafclastItemFrasco wif:
            //            if (wif.Tipo.HasFlag(FrascoTipo.Vida) & wif.Tipo.HasFlag(FrascoTipo.Mana))
            //                str.AppendLine($"Recupera {wif.VidaRestaura} Vida e {wif.ManaRestaura} Mana.");
            //            else if (wif.Tipo.HasFlag(FrascoTipo.Vida))
            //                str.AppendLine($"Recupera {wif.VidaRestaura} Vida.");
            //            else
            //                str.AppendLine($"Recupera {wif.ManaRestaura} Mana.");
            //            str.AppendLine($"Você tem {wif.Pilha}.");
            //            str.AppendLine("Consumível.");
            //            break;
            //        case WafclastItemArma wia:
            //            str.AppendLine($"Nível: {wia.Nivel}.");
            //            //str.AppendLine(wia.Classe.GetEnumDescription());
            //            if (wia.IsDuasMao)
            //                str.AppendLine("Precisa das duas mãos.");
            //            var dano = wia.DanoFisicoCalculado;
            //            str.AppendLine($"Dano Físico: {dano.Minimo}-{dano.Maximo}");
            //            str.AppendLine($"Chance de Crítico: { wia.DanoFisicoCriticoChance * 100}% ");
            //            break;
            //        case WafclastItemComida wic:
            //            str.AppendLine($"{Emoji.Fome} +{wic.FomeRestaura}");
            //            str.AppendLine($"Você tem {wic.Pilha}.");
            //            str.AppendLine("Consumível.");
            //            break;
            //        case WafclastItemBebida wic:
            //            str.AppendLine($"{Emoji.Sede} +{wic.SedeRestaura}");
            //            str.AppendLine($"Você tem {wic.Pilha}.");
            //            str.AppendLine("Consumível.");
            //            break;
            //    }
            //    embed.WithDescription(str.ToString());
            //    return embed;
        }
    }

}
