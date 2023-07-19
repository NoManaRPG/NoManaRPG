// This file is part of NoManaRPG project.

using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using NoManaRPG.Database.Repositories;
using NoManaRPG.Entidades;
using NoManaRPG.Extensions;

namespace NoManaRPG.Comandos.UserCommands;

public class StatusComando : ApplicationCommandModule
{
    private readonly PlayerRepository _playerRepository;
    private readonly ZoneRepository _zoneRepository;

    public StatusComando(PlayerRepository playerRepository, ZoneRepository zoneRepository)
    {
        this._playerRepository = playerRepository;
        this._zoneRepository = zoneRepository;
    }

    [SlashCommand("status", "Permite visualizar dados do seu personagem.")]
    [SlashCooldown(1, 5, SlashCooldownBucketType.User)]
    public async Task StatusCommandAsync(InteractionContext ctx)
    {
        var player = await this._playerRepository.FindPlayerOrErrorAsync(ctx);
        var character = player.Personagem;
        var str = new StringBuilder();

        str.AppendLine($"**{player.MonstersKills}** monstros abatidos.");
        str.AppendLine($"**{player.PlayersKills}** jogadores abatidos.");
        str.AppendLine($"**{player.Deaths}** vezes eliminado.");
        str.AppendLine($"Conta criada {Formatter.Timestamp(player.DateAccountCreation)}.");

        var embed = new DiscordEmbedBuilder();
        embed.WithAuthor($"{ctx.User.Username} [Nvl C. {character.NivelDeCombate}] ", iconUrl: ctx.User.AvatarUrl);
        embed.WithThumbnail(ctx.User.AvatarUrl);
        embed.WithColor(DiscordColor.Blue);
        embed.WithDescription(str.ToString());

        embed.AddField($"Vida {Emojis.CoracaoVermelho}", $"{character.PontosDeVida.ValorAtual:N2}/{character.PontosDeVida.ValorMaximo:N2}", true);
        embed.AddField($"Mana {Emojis.Mago}", $"{character.PontosDeMana.ValorAtual:N2}/{character.PontosDeMana.ValorMaximo:N2}", true);

        //var zone = await this._zoneRepository.FindPlayerHighestZoneAsync(player.DiscordId);
        //if (zone != null)
        //    embed.AddField("Maior Zona", $"{Emojis.Mapa} {zone.Level}");
        await ctx.CreateResponseAsync(embed);
    }
}
