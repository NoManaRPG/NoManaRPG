// This file is part of NoManaRPG project.

using System.Threading.Tasks;
using DSharpPlus.SlashCommands;
using NoManaRPG.Database.Repositories;
using NoManaRPG.Entidades;
using NoManaRPG.Exceptions;

namespace NoManaRPG.Extensions;

public static class PlayerRepositoryExtension
{

    public static async Task<Jogador> FindPlayerOrErrorAsync(this PlayerRepository playerRep, InteractionContext ctx)
    {
        var jogador = await playerRep.FindPlayerOrNullAsync(ctx.User.Id) ?? throw new PlayerNotCreatedException();
        return jogador;
    }
}
