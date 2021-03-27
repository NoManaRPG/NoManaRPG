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
        {
            throw new NotImplementedException();
        }
    }
}
