using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades;

namespace WafclastRPG.Bot.Comandos.Acao
{
    public class ComandoAtacar : BaseCommandModule
    {
        public Banco banco;

        [Command("atacar")]
        [Aliases("at")]
        [Description("Permite atacar um monstro.")]
        [ComoUsar("atacar")]
        public async Task ComandoAtacarAsync(CommandContext ctx)
        {
            // Verifica se existe o jogador e faz o jogador esperar antes de começar outro comando
            var (isJogadorCriado, sessao) = await banco.ExisteJogadorAsync(ctx, true);
            if (!isJogadorCriado) return;

            var personagem = sessao.Jogador.Personagem;

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder().Criar(ctx.User);

            var batalha = personagem.AtacarMonstro(out var resultado);

            embed.AddField("Resumo da Batalha".Titulo(), batalha.ToString());

            if (resultado == WafclastBatalha.Evoluiu)
                embed.AddField("Evolução".Titulo(), $"{Emoji.Up} Você evoluiu de nível!");

            switch (resultado)
            {
                case WafclastBatalha.PersonagemAbatido:
                    embed.WithColor(DiscordColor.Red);
                    embed.WithImageUrl("https://cdn.discordapp.com/attachments/758139402219159562/769397084284649472/kisspng-headstone-drawing-royalty-free-clip-art-5afd7eb3d84cb3.625146071526562483886.png");
                    break;
                case WafclastBatalha.InimigoAbatido:
                    var porcentagemVida = personagem.Vida.Atual / personagem.Vida.Maximo;
                    var porcenagemMana = personagem.Mana.Atual / personagem.Mana.Maximo;
                    embed.AddField(Formatter.Underline("Vida atual"), $"{WafclastPersonagem.VidaEmoji(porcentagemVida)} {(porcentagemVida * 100):N2}%", true);
                    embed.AddField(Formatter.Underline("Mana atual"), $"{WafclastPersonagem.ManaEmoji(porcenagemMana)} {(porcenagemMana * 100):N2}%", true);
                    break;
            }

            embed.WithDescription($"**Turno {personagem.Zona.Turno}.**");
            embed.WithColor(DiscordColor.Blue);
            embed.WithThumbnail("https://cdn.discordapp.com/attachments/758139402219159562/758425473541341214/sword-and-shield-icon-17.png");

            await sessao.Salvar();

            await ctx.RespondAsync(ctx.User.Mention, embed: embed.Build());
        }
    }
}
