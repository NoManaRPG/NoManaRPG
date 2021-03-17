namespace WafclastRPG.Game.Entities
{
    public class WafclastLocalization
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
    }
}
