// This file is part of NoManaRPG project.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NoManaRPG.Entidades.Items;

[BsonIgnoreExtraElements]
public class Item : BaseItem
{
    // Way to find the item
    [BsonId]
    public ObjectId Id { get; private set; } = ObjectId.GenerateNewId();
    public ulong PlayerId { get; set; }

    public bool IsOnInventory { get; set; }

    public Item() { }

    public Item(Item item)
    {
        this.Name = item.Name;
        this.Quantity = item.Quantity;
        this.Volume = item.Volume;
        this.Id = item.Id;
        this.PlayerId = item.PlayerId;
        this.IsOnInventory = item.IsOnInventory;
    }
}
