// This file is part of WafclastRPG project.

namespace NoManaRPG.Game.Entities.Items
{
    public class BaseItem
    {
        public string Name { get; set; }
        public ulong Quantity { get; set; }

        public ulong Volume { get; set; }

        public BaseItem() { }

        public BaseItem(BaseItem baseItem)
        {
            this.Name = baseItem.Name;
            this.Quantity = baseItem.Quantity;
            this.Volume = baseItem.Volume;
        }
    }
}
