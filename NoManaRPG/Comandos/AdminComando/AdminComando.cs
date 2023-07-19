// This file is part of NoManaRPG project.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using NoManaRPG.Extensions;

namespace NoManaRPG.Comandos.AdminCommands;

[SlashCommandGroup("Admin", "Comandos de administrador")]
public class AdminComando : ApplicationCommandModule
{
    [SlashRequireOwner]
    [SlashCommand("reset", "Admin")]
    public async Task ResetarCommandosAsync(InteractionContext ctx)
    {
        await ctx.Client.BulkOverwriteGuildApplicationCommandsAsync(1118002801046474773, new List<DiscordApplicationCommand>());
        await ctx.CreateResponseAsync("Comandos neste servidor foram apagados! Reinicie o cliente para adicionar novamente os comandos deste servidor", true);
    }

    [SlashRequireOwner]
    [SlashCommand("teste", "Admin")]
    public async Task TesteCommandosAsync(InteractionContext ctx)
    {
        Random r = new ();
        await ctx.CreateResponseAsync($"{r.Sortear(1)}", true);
    }
}
