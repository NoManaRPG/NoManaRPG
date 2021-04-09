using System;
using System.Diagnostics.CodeAnalysis;
using WafclastRPG.Entities.Maps;

namespace WafclastRPG.Entities
{
    public class WafclastLocalization : IEquatable<WafclastLocalization>
    {
        /// <summary>
        /// TextChannelId
        /// </summary>
        public ulong ChannelId { get; set; }
        public ulong ServerId { get; set; }

        public WafclastLocalization(ulong localId, ulong serverId)
        {
            ChannelId = localId;
            ServerId = serverId;
        }

        public bool Equals([DisallowNull] WafclastLocalization other)
            => ChannelId == other.ChannelId;

        public static bool operator !=(WafclastLocalization loc, WafclastLocalization loc1)
            => loc.ChannelId != loc1.ChannelId;

        public static bool operator ==(WafclastLocalization loc, WafclastLocalization loc1)
            => loc.ChannelId == loc1.ChannelId;

        public static bool operator !=(WafclastLocalization loc, WafclastMap map)
            => loc.ChannelId != map.Id;

        public static bool operator ==(WafclastLocalization loc, WafclastMap map)
            => loc.ChannelId == map.Id;
    }
}
