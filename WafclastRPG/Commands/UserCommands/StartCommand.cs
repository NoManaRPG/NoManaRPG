// This file is part of WafclastRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using WafclastRPG.Attributes;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Extensions;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Commands.UserCommands
{
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class StartCommand : BaseCommandModule
    {
        private readonly IPlayerRepository _playerRepository;

        public StartCommand(IPlayerRepository playerRepository, IMongoSession session)
        {
            this._playerRepository = playerRepository;
        }

        [Command("comecar")]
        [Aliases("start")]
        [Description("Permite criar um personagem.")]
        [Usage("comecar")]
        public async Task StartCommandAsync(CommandContext ctx)
        {
            var player = await this._playerRepository.FindPlayerOrDefaultAsync(ctx);
            if (player != null)
            {
                await ctx.RespondAsync("você já criou um personagem! Se estiver com dúvidas ou problemas, consulte o nosso Servidor Oficial do Discord.");
                return;
            }

            player = new WafclastPlayer(ctx.User.Id);

            await this._playerRepository.SavePlayerAsync(player);
            await ctx.RespondAsync("personagem criado com sucesso! Obrigado por escolher Wafclast!");
        }
    }
}
