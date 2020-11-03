using WafclastRPG.Atributos;
using WafclastRPG.Entidades;
using WafclastRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Services;

namespace WafclastRPG.Comandos.Acao
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
