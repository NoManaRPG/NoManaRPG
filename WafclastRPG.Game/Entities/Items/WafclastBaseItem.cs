// This file is part of WafclastRPG project.

namespace WafclastRPG.Game.Entities.Items
{
    public class WafclastBaseItem
    {
        public string Name { get; set; }
        public ulong Quantity { get; set; }

        public ulong Volume { get; set; }

        public WafclastBaseItem() { }

        public WafclastBaseItem(WafclastBaseItem baseItem)
        {
            this.Name = baseItem.Name;
            this.Quantity = baseItem.Quantity;
            this.Volume = baseItem.Volume;
        }
    }
}
