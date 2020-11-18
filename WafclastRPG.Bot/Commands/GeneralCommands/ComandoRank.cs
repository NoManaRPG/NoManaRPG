using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades;

namespace WafclastRPG.Bot.Comandos.Exibir
{
    public class ComandoRank : BaseCommandModule
    {
        public Banco banco;

        //Está dando erro
        [Command("top-hab")]
        [Description("Exibe os 10 jogadores com determinada habilidades mais fortes.")]
        [Usage("top-hab <Habilidade>")]
        [Example("top-hab ataque", "Exibe os 10 jogadores com o maior nível em ataque")]
        [Cooldown(1, 30, CooldownBucketType.User)]
        public async Task ComandoMochilaAsync(CommandContext ctx, string hab = "")
        {
            //StringBuilder str = new StringBuilder();

            //var filter = Builders<WafclastJogador>.Filter.Gt(x => x.Personagem.Habilidades[0].ExperienciaAtual, 50);
            //var res = await banco.Jogadores.FindAsync(filter);

            await Task.CompletedTask;


            //var embed = new DiscordEmbedBuilder();

            ////foreach (var item in top)
            ////{
            ////    var habi = item.Personagem.Habilidades[Game.Enums.ProficienciaType.Ataque];
            ////    str.AppendLine($"{habi.Nivel}|{habi.ExperienciaAtual} - <@{item.Id}>");
            ////}

            //embed.WithDescription(str.ToString());
            //embed.WithTitle("Jogadores com ataque maior");

            //await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
