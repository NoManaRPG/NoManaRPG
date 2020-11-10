using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Extensoes;

namespace WafclastRPG.Bot.Commands.UserCommands
{
    public class ComandoAtributos : BaseCommandModule
    {
        public Banco banco;

        [Command("atributos")]
        [Description("Exibe todos atributos e seus pontos.")]
        public async Task ComandoAtributosAsync(CommandContext ctx)
        {
            // Verifica se existe o jogador e faz o jogador esperar antes de começar outro comando
            var (isJogadorCriado, sessao) = await banco.ExisteJogadorAsync(ctx);
            if (!isJogadorCriado) return;

            var personagem = sessao.Jogador.Personagem;

            var embed = new DiscordEmbedBuilder().Criar(ctx);

            var str = new StringBuilder();
            str.AppendLine(Formatter.Bold($"Força: {personagem.Forca}."));
            str.AppendLine(Formatter.Bold($"Inteligência: {personagem.Inteligencia}."));
            str.AppendLine(Formatter.Bold($"Destreza: {personagem.Destreza}."));
            str.AppendLine(Formatter.Bold($"Fome: {personagem.Fome.Maximo}."));
            str.AppendLine(Formatter.Bold($"Sede: {personagem.Sede.Maximo}."));
            str.AppendLine(Formatter.Bold($"Vigor: {personagem.Vigor.Maximo}."));
            str.AppendLine();
            str.AppendLine(Formatter.Bold($"Pontos disponíveis: {personagem.Pontos}."));
            embed.WithDescription(str.ToString());
            embed.WithTitle("Seus atributos");
            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
