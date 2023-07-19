// This file is part of NoManaRPG project.

using System.Collections.Concurrent;
using DSharpPlus.CommandsNext;

namespace NoManaRPG.Interactivity;

public class UsersBlocked
{
    private readonly ConcurrentDictionary<ulong, bool> _usersBlocked;

    public UsersBlocked()
    {
        this._usersBlocked = new ConcurrentDictionary<ulong, bool>();
    }

    public bool IsUserBlocked(ulong userId) => this._usersBlocked.TryGetValue(userId, out _);
    public void UnblockUser(ulong userId) => this._usersBlocked.TryRemove(userId, out _);
    public void UnblockUser(CommandContext ctx) => this._usersBlocked.TryRemove(ctx.User.Id, out _);
    public void BlockUser(ulong userId) => this._usersBlocked.TryAdd(userId, true);
    public void BlockUser(CommandContext ctx) => this._usersBlocked.TryAdd(ctx.User.Id, true);
}
