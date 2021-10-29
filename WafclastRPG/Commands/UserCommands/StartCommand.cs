// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using WafclastRPG.Attributes;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Database.Response;
using WafclastRPG.Extensions;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Commands.UserCommands
{
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class StartCommand : BaseCommandModule
    {
        private IResponse _res;
        private readonly IPlayerRepository _playerRepository;
        private readonly IMongoSession _session;

        public StartCommand(IPlayerRepository playerRepository, IMongoSession session)
        {
            this._playerRepository = playerRepository;
            this._session = session;
        }

        [Command("comecar")]
        [Aliases("start")]
        [Description("Permite criar um personagem.")]
        [Usage("comecar")]
        public async Task StartCommandAsync(CommandContext ctx)
        {
            using (await this._session.StartSessionAsync())
                this._res = await this._session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await this._playerRepository.FindPlayerOrDefaultAsync(ctx);
                    if (player != null)
                        return new StringResponse("você já criou um personagem! Se estiver com dúvidas ou problemas, consulte o nosso Servidor Oficial do Discord.");

                    player = new WafclastPlayer(ctx.User.Id);

                    await this._playerRepository.SavePlayerAsync(player);
                    return new StringResponse("personagem criado com sucesso! Obrigado por escolher Wafclast!");
                });
            await ctx.RespondAsync(this._res);
        }
    }
}
