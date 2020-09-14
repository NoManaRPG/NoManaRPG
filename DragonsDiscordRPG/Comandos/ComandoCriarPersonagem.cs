using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
{

    public class ComandoCriarPersonagem : BaseCommandModule
    {
        [Command("criar-personagem")]
        [Cooldown(1, 10, CooldownBucketType.User)]
        public async Task CriarPersonagemAsync(CommandContext ctx, string classe = null, [RemainingText] string nomePersonagem = null)
        {
            var jogadorExiste = await JogadorExisteAsync(ctx);
            if (jogadorExiste) return;

            if (string.IsNullOrWhiteSpace(classe))
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
                embed.WithColor(DiscordColor.Gold);
                embed.AddField("Caçadora".Titulo(), "Foco em: Destreza", true);
                embed.AddField("Berserker".Titulo(), "Foco em: Força", true);
                embed.AddField("Bruxa".Titulo(), "Foco em: Inteligência", true);
                embed.AddField("Duelista".Titulo(), "Foco em: Força e Destreza", true);
                embed.AddField("Templário".Titulo(), "Foco em: Força e Inteligência", true);
                embed.AddField("Sombra".Titulo(), "Foco em: Destreza e Inteligência", true);
                embed.AddField("Herdeira".Titulo(), "Foco em : Força, Destreza e Inteligência", true);
                await ctx.RespondAsync("Escolha uma classe com o comando `criar-personagem [classe] [nome do personagem]`. Não utilize colchetes!", embed: embed.Build());
                return;
            }

            if (string.IsNullOrWhiteSpace(nomePersonagem))
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa informar o nome do seu personagem!");
                return;
            }

            if (nomePersonagem.Length > 12)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, o nome do seu personagem precisa ter menos de 12 caracteres!");
                return;
            }

            string classeFormatada = RemoveDiacritics(classe).ToLower();

            switch (classeFormatada)
            {
                case "cacadora":
                    await CriarJogador(ctx, new RPPersonagem("Caçadora", nomePersonagem, new RPAtributo(14, 32, 14), new RPDano(2, 5)));
                    break;
                case "berserker":
                    await CriarJogador(ctx, new RPPersonagem("Berserker", nomePersonagem, new RPAtributo(14, 14, 32), new RPDano(2, 8)));
                    break;
                case "bruxa":
                    await CriarJogador(ctx, new RPPersonagem("Bruxa", nomePersonagem, new RPAtributo(32, 14, 14), new RPDano(2, 5)));
                    break;
                case "duelista":
                    await CriarJogador(ctx, new RPPersonagem("Duelista", nomePersonagem, new RPAtributo(14, 23, 23), new RPDano(2, 6)));
                    break;
                case "templario":
                    await CriarJogador(ctx, new RPPersonagem("Templário", nomePersonagem, new RPAtributo(23, 14, 23), new RPDano(2, 6)));
                    break;
                case "sombra":
                    await CriarJogador(ctx, new RPPersonagem("Sombra", nomePersonagem, new RPAtributo(23, 23, 14), new RPDano(2, 5)));
                    break;
                case "herdeira":
                    await CriarJogador(ctx, new RPPersonagem("Herdeira", nomePersonagem, new RPAtributo(20, 20, 20), new RPDano(2, 6)));
                    break;
                default:
                    await ctx.RespondAsync($"{ctx.User.Mention}, você informou uma classe que não existe!");
                    break;
            }
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }


        private async Task<bool> JogadorExisteAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            RPJogador jogador = await ModuloBanco.GetJogadorAsync(ctx);
            if (jogador == null) return false;
            await ctx.RespondAsync($"{ctx.User.Mention}, você já criou um personagem e por isso não pode usar este comando novamente!");
            return true;
        }

        private async Task CriarJogador(CommandContext ctx, RPPersonagem personagem)
        {
            using (var session = await ModuloBanco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = new RPJogador(ctx, personagem);
                await banco.AddJogadorAsync(jogador);
                await session.CommitTransactionAsync();
            }
            await ctx.RespondAsync($"{ctx.User.Mention}, o seu personagem foi criado!");
        }
    }
}
