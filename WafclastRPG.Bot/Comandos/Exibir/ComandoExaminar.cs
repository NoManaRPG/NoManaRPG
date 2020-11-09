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
        [Description("Permite examinar um item pelo o `#ID`.")]
        [ComoUsar("examinar [#ID]")]
        [Exemplo("examinar #1")]
        public async Task ComandoExaminarAsync(CommandContext ctx, string stringId = "#0")
        {
            // Verifica se existe o jogador e faz o jogador esperar antes de começar outro comando
            var (isJogadorCriado, sessao) = await banco.ExisteJogadorAsync(ctx);
            if (!isJogadorCriado) return;

            var personagem = sessao.Jogador.Personagem;

            if (personagem.Mochila.Itens.Count == 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa de algum itens na mochila para poder estar examinando!");
                return;
            }

            stringId.TryParseID(out int index);
            Math.Clamp(index, 0, personagem.Mochila.Itens.Count - 1);
            if (personagem.Mochila.TryGetItem(index, out var item))
            {
                var descricao = ItemDescricao(item).Criar(ctx);
                await ctx.RespondAsync(embed: descricao.Build());
            }
            else
                await ctx.RespondAsync($"{ctx.User.Mention}, item não encontrado na mochila!");
        }

        public static DiscordEmbedBuilder ItemDescricao(WafclastItem item)
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithTitle($" {item.Nome.Titulo()}");
            embed.WithColor(DiscordColor.Azure);
            StringBuilder str = new StringBuilder();

            str.AppendLine($"Ocupa {item.OcupaEspaco} espaço.");
            str.AppendLine($"{Emoji.Coins} {item.PrecoVenda / 2}V.");

            switch (item)
            {
                case WafclastItemFrasco wif:
                    if (wif.Tipo.HasFlag(FrascoTipo.Vida) & wif.Tipo.HasFlag(FrascoTipo.Mana))
                        str.AppendLine($"Recupera {wif.VidaRestaura} Vida e {wif.ManaRestaura} Mana.");
                    else if (wif.Tipo.HasFlag(FrascoTipo.Vida))
                        str.AppendLine($"Recupera {wif.VidaRestaura} Vida.");
                    else
                        str.AppendLine($"Recupera {wif.ManaRestaura} Mana.");
                    str.AppendLine($"Você tem {wif.Pilha}.");
                    str.AppendLine("Consumível.");
                    break;
                case WafclastItemArma wia:
                    str.AppendLine($"Nível: {wia.Nivel}.");
                    str.AppendLine(wia.Classe.GetEnumDescription());
                    if (wia.IsDuasMao)
                        str.AppendLine("Precisa das duas mãos.");
                    var dano = wia.DanoFisicoCalculado;
                    str.AppendLine($"Dano Físico: {dano.Minimo}-{dano.Maximo}");
                    str.AppendLine($"Chance de Crítico: { wia.DanoFisicoCriticoChance * 100}% ");
                    break;
                case WafclastItemComida wic:
                    str.AppendLine($"{Emoji.Fome} +{wic.FomeRestaura}");
                    str.AppendLine($"Você tem {wic.Pilha}.");
                    str.AppendLine("Consumível.");
                    break;
                case WafclastItemBebida wic:
                    str.AppendLine($"{Emoji.Sede} +{wic.SedeRestaura}");
                    str.AppendLine($"Você tem {wic.Pilha}.");
                    str.AppendLine("Consumível.");
                    break;
            }
            embed.WithDescription(str.ToString());
            return embed;
        }
    }

}
