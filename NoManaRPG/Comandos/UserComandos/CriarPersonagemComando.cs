// This file is part of NoManaRPG project.

using System.Threading.Tasks;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using NoManaRPG.Database.Repositories;
using NoManaRPG.Entidades;

namespace NoManaRPG.Comandos.UserComandos;

public class CriarPersonagemComando : ApplicationCommandModule
{
    private readonly PlayerRepository _playerRepository;

    public CriarPersonagemComando(PlayerRepository playerRepository)
    {
        this._playerRepository = playerRepository;
    }

    [SlashCommand("comecar", "Cria um novo personagem para a sua conta. É aqui onde você começa.")]
    [SlashCooldown(1, 86400, SlashCooldownBucketType.User)]
    public async Task StatusCommandAsync(InteractionContext ctx)
    {
        Jogador player = await this._playerRepository.FindPlayerOrNullAsync(ctx.User.Id);
        if (player != null)
        {
            await ctx.CreateResponseAsync($"{ctx.User.Mention}, você já criou um personagem! " +
                $"Se estiver com dúvidas ou problemas, consulte o nosso Servidor Oficial.");
            return;
        }

        player = new(ctx.User.Id);

        await this._playerRepository.SavePlayerAsync(player);
        await ctx.CreateResponseAsync($"{ctx.User.Mention}, seu personagem foi criado com sucesso! Obrigado por nos escolher!");
    }
}
