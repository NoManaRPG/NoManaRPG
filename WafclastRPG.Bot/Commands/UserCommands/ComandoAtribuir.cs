using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;

namespace WafclastRPG.Bot.Commands.UserCommands
{
    public class ComandoAtribuir : BaseCommandModule
    {
        public Banco banco;

        [Command("atribuir")]
        [Description("Permite atribuir pontos que se ganha ao evoluir em atributos.")]
        [Example("atribuir força 1", "Atribui 1 ponto em força.")]
        [Usage("atribuir <atributo> [ quantidade ] ")]
        public async Task ComandoAtribuirAsync(CommandContext ctx, string atributo, string stringQuant = "1")
        {
            // Verifica se existe o jogador e faz o jogador esperar antes de começar outro comando
            var (isJogadorCriado, sessao) = await banco.ExisteJogadorAsync(ctx, true);
            if (!isJogadorCriado) return;

            if (string.IsNullOrEmpty(atributo))
            {
                sessao.Soltar();
                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa informar um atributo para atribuir pontos.");
                return;
            }

            if (!int.TryParse(stringQuant, out int quantidade))
            {
                sessao.Soltar();
                await ctx.RespondAsync($"{ctx.User.Mention}, você informou uma quantidade inválida!");
                return;
            }

            var personagem = sessao.Jogador.Personagem;

            if (quantidade > personagem.Pontos)
            {
                sessao.Soltar();
                await ctx.RespondAsync($"{ctx.User.Mention}, você não tem essa quantia informada de pontos!");
                return;
            }
            personagem.Pontos -= quantidade;
            atributo = atributo.ToLower().RemoverAcentos();

            switch (atributo)
            {
                case "forca":
                    personagem.Forca += quantidade;
                    personagem.CalcVida();
                    break;
                case "inteligencia":
                    personagem.Inteligencia += quantidade;
                    personagem.CalcMana();
                    break;
                case "destreza":
                    personagem.Destreza += quantidade;
                    personagem.CalcPrecisao();
                    personagem.CalcEvasao();
                    break;
                case "fome":
                    personagem.Fome.AddExtra(quantidade);
                    break;
                case "sede":
                    personagem.Sede.AddExtra(quantidade);
                    break;
                case "vigor":
                    personagem.Vigor.AddExtra(quantidade);
                    break;
                default:
                    sessao.Soltar();
                    await ctx.RespondAsync($"{ctx.User.Mention}, este atributo não existe!");
                    return;
            }
            await sessao.Salvar();
            await ctx.RespondAsync($"{ctx.User.Mention}, pontos atribuidos!");
        }
    }
}
