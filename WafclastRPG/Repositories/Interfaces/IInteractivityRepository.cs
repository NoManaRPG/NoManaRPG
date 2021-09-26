using DSharpPlus.CommandsNext;

namespace WafclastRPG.Repositories.Interfaces {
  public interface IInteractivityRepository {
    bool IsBlocked(ulong userId);
    void Unblock(ulong userId);
    void Unblock(CommandContext ctx);
    void Block(ulong userId);
    void Block(CommandContext ctx);
  }
}
