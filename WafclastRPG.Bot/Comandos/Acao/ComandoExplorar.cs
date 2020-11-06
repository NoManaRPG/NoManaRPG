using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Net.Models;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades;

namespace WafclastRPG.Bot.Comandos.Acao
{
    public class ComandoExplorar : BaseCommandModule
    {
        public Banco banco;

        [Command("explorar")]
        [Aliases("ex")]
        [Description("Permite explorar a região, se encontrar um monstro você o ataca.")]
        [ComoUsar("explorar")]
        // [Cooldown(1, 4, CooldownBucketType.User)]
        public async Task ComandoExplorarAsync(CommandContext ctx, string codigo = "")
        {
            // Verifica se existe o jogador e faz o jogador esperar antes de começar outro comando
            var (isJogadorCriado, sessao) = await banco.ExisteJogadorAsync(ctx, true);
            if (!isJogadorCriado) return;

            var personagem = sessao.Jogador.Personagem;

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder().Criar(ctx.User);

            codigo.TryParseID(out var ataque);
            Math.Clamp(ataque, 0, 1);

            var batalha = personagem.AtacarMonstro(out var resultado, ataque);

            var str = new StringBuilder();
            str.AppendLine($":poultry_leg: **{personagem.Fome.Atual:N2}**");
            str.AppendLine($":cup_with_straw: **{personagem.Sede.Atual:N2}**");
            str.AppendLine($":zap: **{personagem.Vigor.Atual:N2}**");
            embed.WithDescription(str.ToString());
            embed.AddField("Resumo da Batalha".Titulo(), batalha.ToString());

            if (resultado == Resultado.Evoluiu)
            {
                embed.AddField("Evolução".Titulo(), $"{Emoji.Up} Você evoluiu de nível!");
                if (ctx.Guild.Id == 732102804654522470)
                {
                    try
                    {
                        var name = $"{ ctx.Member.Username } [Lvl.{ personagem.Nivel.Atual}]";
                        await ctx.Member.ModifyAsync(x => x.Nickname = name);
                    }
                    catch (Exception) { }
                }
            }

            switch (resultado)
            {
                case Resultado.PersonagemAbatido:
                    embed.WithColor(DiscordColor.Red);
                    embed.WithImageUrl("https://cdn.discordapp.com/attachments/758139402219159562/769397084284649472/kisspng-headstone-drawing-royalty-free-clip-art-5afd7eb3d84cb3.625146071526562483886.png");
                    break;
                case Resultado.SemVigor:
                case Resultado.InimigoAbatido:
                    var porcentagemVida = personagem.Vida.Atual / personagem.Vida.Maximo;
                    var porcenagemMana = personagem.Mana.Atual / personagem.Mana.Maximo;
                    embed.AddField(Formatter.Underline("Vida atual"), $"{WafclastPersonagem.VidaEmoji(porcentagemVida)} {(porcentagemVida * 100):N2}%", true);
                    embed.AddField(Formatter.Underline("Mana atual"), $"{WafclastPersonagem.ManaEmoji(porcenagemMana)} {(porcenagemMana * 100):N2}%", true);
                    break;
            }
            embed.WithColor(DiscordColor.Blue);
            embed.WithThumbnail("https://cdn.discordapp.com/attachments/758139402219159562/758425473541341214/sword-and-shield-icon-17.png");

            await sessao.Salvar();

            await ctx.RespondAsync(ctx.User.Mention, embed: embed.Build());
        }
    }
}
