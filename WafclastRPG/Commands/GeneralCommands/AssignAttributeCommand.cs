using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Maps;
using WafclastRPG.Enums;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class AssignAttributeCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("evoluir-atributo")]
        [Description("Permite atribuir pontos de atributos")]
        [Usage("evoluir-atributo")]
        public async Task AssignAttributeCommandAsync(CommandContext ctx)
        {
            
        }
    }
}
