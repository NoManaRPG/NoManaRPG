using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Enuns;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
{
    [Group("olhar")]
    public class ComandoOlhar : BaseCommandModule
    {
        [GroupCommand]
        public async Task ComandoOlharAsync(CommandContext ctx)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            RPJogador jogador = await ModuloBanco.GetJogadorAsync(ctx);
            RPPersonagem personagem = jogador.Personagem;

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe}", iconUrl: ctx.User.AvatarUrl);
            embed.WithDescription($"Batalhando contra {personagem.Zona.Monstros.Count} monstros.\n" +
                $"Onda {personagem.Zona.OndaAtual.Bold()}/{personagem.Zona.OndaTotal.Bold()}.\n" +
                $"Nivel {personagem.Zona.Nivel}\n" +
                $"Tem {(personagem.Zona.ItensNoChao == null ? 0 : personagem.Zona.ItensNoChao.Count)} no chão\n\n" +
                $"*Use `!olhar inimigo` para ver os inimigos*\n" +
                $"*Use `!olhar item` para ver os itens no chão*");

            await ctx.RespondAsync(embed: embed.Build());
        }

        [Command("inimigo")]
        public async Task ComandoOlharInimigoAsync(CommandContext ctx)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            RPJogador jogador = await ModuloBanco.GetJogadorAsync(ctx);
            RPPersonagem personagem = jogador.Personagem;

            if (personagem.Zona.Monstros == null)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você não está em combate!");
                return;
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe}", iconUrl: ctx.User.AvatarUrl);

            for (int i = 0; i < personagem.Zona.Monstros.Count; i++)
            {
                var monstro = personagem.Zona.Monstros[i];
                embed.AddField($"{monstro.Nome.Titulo().Bold()} *#{i}*", $"{monstro.Vida.Text()} vida.", true);
            }
            await ctx.RespondAsync(embed: embed.Build());
        }

        [Command("inimigo")]
        [Cooldown(1, 50, CooldownBucketType.User)]
        public async Task ComandoOlharInimigoAsync(CommandContext ctx, string id)
        {
            await ctx.RespondAsync($"{ctx.User.Mention}, este comando ainda está em desenvolvimento!");
        }

        [Command("item")]
        public async Task ComandoOlharItemAsync(CommandContext ctx)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            RPJogador jogador = await ModuloBanco.GetJogadorAsync(ctx);
            RPPersonagem personagem = jogador.Personagem;

            if (personagem.Zona.ItensNoChao.Count == 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você não tem itens para olhar!");
                return;
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe}", iconUrl: ctx.User.AvatarUrl);

            StringBuilder str = new StringBuilder();
            for (int i = 0; i < personagem.Zona.ItensNoChao.Count; i++)
            {
                var item = personagem.Zona.ItensNoChao[i];
                str.AppendLine($"*#{i}* - {item.Nome.Titulo().Bold()} ");
            }
            embed.WithDescription(str.ToString());
            await ctx.RespondAsync(embed: embed.Build());
        }

        [Command("item")]
        [Priority(1)]
        public async Task ComandoOlharItemAsync(CommandContext ctx, string idEscolhido)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            RPJogador jogador = await ModuloBanco.GetJogadorAsync(ctx);
            RPPersonagem personagem = jogador.Personagem;

            if (personagem.Zona.ItensNoChao.Count == 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você não tem itens para olhar!");
                return;
            }

            // Converte o id informado.
            bool converteu = int.TryParse(idEscolhido.Replace("#", string.Empty), out int id);
            if (!converteu)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você informou um #ID válido?");
                return;
            }

            var item = personagem.Zona.ItensNoChao.ElementAtOrDefault(id);
            if (item != null)
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
                embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe}", iconUrl: ctx.User.AvatarUrl);

                embed.WithTitle($"*#{id}* - {item.Nome.Titulo().Bold()}");
                switch (item.Tipo)
                {
                    case RPItemTipo.PocaoVida:
                        embed.WithDescription("Frascos de Vida\n" +
                            $"Ocupa {item.Espaco} de espaço\n" +
                            $"Recupera {item.LifeRegen} de vida por {item.Tempo} segundos\n" +
                            $"Consome {item.CargasUso} de {item.CargasMax} cargas na utilização\n" +
                            $"Atualmente tem 0 carga\n" +
                            $"------\n" +
                            "Só é possível manter cargas no cinto. Recarrega conforme você mata monstros.");
                        break;
                    case RPItemTipo.Arco:
                        StringBuilder str = new StringBuilder();
                        if (item.Destreza != 0)
                            str.Append($"{item.Destreza} des, ");
                        if (item.Inteligencia != 0)
                            str.Append($"{item.Inteligencia} int, ");
                        if (item.Forca != 0)
                            str.Append($"{item.Forca} for");
                        embed.WithDescription("Arcos\n" +
                            $"Ocupa {item.Espaco} de espaço\n" +
                            $"Dano físico: {item.DanoFisico.Minimo}-{item.DanoFisico.Maximo}\n" +
                            $"Chance de crítico: {item.ChanceCritico * 100}%\n" +
                            $"Ataques por segundo: {item.VelocidadeAtaque}\n" +
                            $"------\n" +
                            $"Requer nível {item.Nivel}, {str}");
                        break;
                }
                await ctx.RespondAsync(embed: embed.Build());
            }
            else
                await ctx.RespondAsync($"{ctx.User.Mention}, #ID não encontrado.");
        }
    }
}
