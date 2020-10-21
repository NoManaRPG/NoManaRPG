using TorreRPG.Atributos;
using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using TorreRPG.Services;

namespace TorreRPG.Comandos.Acao
{
    public class ComandoIdentificar : BaseCommandModule
    {
        public Banco banco;

        [Command("identificar")]
        [Description("Permite identificar um item.")]
        [ComoUsar("identificar <#ID>")]
        [Exemplo("identificar #1")]
        [Cooldown(1, 10, CooldownBucketType.Channel)]
        public async Task ComandoIdentificarAsync(CommandContext ctx, string stringId = "")
        {
            await ctx.RespondAsync("Comando em construção!");
        }
    }
}
