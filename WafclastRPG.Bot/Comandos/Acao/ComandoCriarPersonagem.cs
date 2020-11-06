using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Enums;
using WafclastRPG.Game.Extensoes;

namespace WafclastRPG.Bot.Comandos.Acao
{
    public class ComandoCriarPersonagem : BaseCommandModule
    {
        public Banco banco;

        [Command("criar-personagem")]
        [Aliases("cp")]
        [Description("Permite criar um personagem com uma das 7 classes disponíveis.")]
        [ComoUsar("criar-personagem <classe>")]
        [Exemplo("criar-personagem caçadora")]
        [Cooldown(1, 10, CooldownBucketType.User)]
        public async Task CriarPersonagemAsync(CommandContext ctx, string classe = "")
        {
            var jogadorExiste = await JogadorExisteAsync(ctx);
            if (jogadorExiste) return;

            if (string.IsNullOrWhiteSpace(classe))
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder().Criar(ctx);
                embed.WithColor(DiscordColor.Gold);
                embed.WithTitle("Classes disponíveis");
                embed.WithDescription($"Escolha uma classe digitando `w.criar-personagem <classe>`.");
                embed.AddField("Caçadora", "Foco em: Destreza", true);
                embed.AddField("Berserker", "Foco em: Força", true);
                embed.AddField("Bruxa", "Foco em: Inteligência", true);
                embed.AddField("Duelista", "Foco em: Força e Destreza", true);
                embed.AddField("Templário", "Foco em: Força e Inteligência", true);
                embed.AddField("Sombra", "Foco em: Destreza e Inteligência", true);
                embed.AddField("Herdeira", "Foco em : Força, Destreza e Inteligência", true);
                await ctx.RespondAsync(embed: embed.Build());
                return;
            }

            string classeFormatada = classe.RemoverAcentos().ToLower();
            WafclastPersonagem per;
            switch (classeFormatada)
            {
                case "cacadora":
                    per = new WafclastPersonagem(WafclastClasse.Cacadora, new WafclastDano(2, 5), 14, 32, 14);
                    break;
                case "berserker":
                    per = new WafclastPersonagem(WafclastClasse.Berserker, new WafclastDano(2, 8), 14, 14, 32);
                    break;
                case "sombra":
                    per = new WafclastPersonagem(WafclastClasse.Sombra, new WafclastDano(2, 5), 23, 23, 14);
                    break;
                case "duelista":
                    per = new WafclastPersonagem(WafclastClasse.Duelista, new WafclastDano(2, 6), 14, 23, 23);
                    break;
                case "bruxa":
                    per = new WafclastPersonagem(WafclastClasse.Bruxa, new WafclastDano(2, 5), 32, 14, 14);
                    break;
                case "templario":
                    per = new WafclastPersonagem(WafclastClasse.Templario, new WafclastDano(2, 6), 23, 14, 23);
                    break;
                case "herdeira":
                    per = new WafclastPersonagem(WafclastClasse.Herdeira, new WafclastDano(2, 6), 20, 20, 20);
                    break;
                default:
                    await ctx.RespondAsync($"{ctx.User.Mention}, você informou uma classe que não existe!");
                    return;
            }

            await CriarJogador(ctx, per);
        }

        private async Task<bool> JogadorExisteAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var jogador = await banco.GetJogadorAsync(ctx);
            if (jogador == null)
                return false;
            await ctx.RespondAsync($"{ctx.User.Mention}, você já tem um personagem!");
            return true;
        }

        private async Task CriarJogador(CommandContext ctx, WafclastPersonagem personagem)
        {
            var jogador = new WafclastJogador(ctx.User.Id, personagem);
            await banco.AddJogadorAsync(jogador);
            await ctx.RespondAsync($"{ctx.User.Mention}, o seu personagem {personagem.Classe.GetEnumDescription()} criado. Divirta-se!");
        }
    }
}
