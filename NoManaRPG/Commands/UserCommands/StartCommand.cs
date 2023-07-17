// This file is part of NoManaRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using NoManaRPG.Attributes;
using NoManaRPG.Database.Repositories;
using NoManaRPG.Game.Entities;

namespace NoManaRPG.Commands.UserCommands;

[ModuleLifespan(ModuleLifespan.Transient)]
public class StartCommand : BaseCommandModule
{
    private readonly PlayerRepository _playerRepository;

    public StartCommand(PlayerRepository playerRepository, MongoSession session)
    {
        this._playerRepository = playerRepository;
    }

    [Command("comecar")]
    [Aliases("start")]
    [Description("Permite criar um personagem.")]
    [Usage("comecar")]
    public async Task StartCommandAsync(CommandContext ctx)
    {
        var player = await this._playerRepository.FindPlayerOrDefaultAsync(ctx.Member.Id);
        if (player != null)
        {
            await ctx.RespondAsync("você já criou um personagem! Se estiver com dúvidas ou problemas, consulte o nosso Servidor Oficial do Discord.");
            return;
        }

        player = new Player(ctx.User.Id);

        await this._playerRepository.SavePlayerAsync(player);
        await ctx.RespondAsync("personagem criado com sucesso! Obrigado por escolher Wafclast!");
    }
}
