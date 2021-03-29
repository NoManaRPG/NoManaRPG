using DSharpPlus.CommandsNext;
using System;
using System.Diagnostics.CodeAnalysis;

namespace WafclastRPG.Entities
{
    public class WafclastLocalization : IEquatable<WafclastLocalization>
    {
        public ulong ChannelId { get; set; }
        public ulong ServerId { get; set; }

        public WafclastLocalization()
        {
        }

        public WafclastLocalization(ulong localId, ulong serverId)
        {
            ChannelId = localId;
            ServerId = serverId;
        }

        public bool Equals([DisallowNull] WafclastLocalization other)
            => ChannelId == other.ChannelId;

        public static bool operator !=(WafclastLocalization loc1, WafclastLocalization loc2)
            => loc1.ChannelId != loc2.ChannelId;

        public static bool operator ==(WafclastLocalization loc1, WafclastLocalization loc2)
            => loc1.ChannelId == loc2.ChannelId;

        public static bool operator !=(WafclastLocalization loc1, CommandContext ctx)
            => loc1.ChannelId != ctx.Channel.Id;

        public static bool operator ==(WafclastLocalization loc1, CommandContext ctx)
            => loc1.ChannelId == ctx.Channel.Id;
    }
}
