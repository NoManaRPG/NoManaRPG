using System.ComponentModel;

namespace WafclastRPG.Enums
{
    public enum ItemType : int
    {
        [Description("Nada")]
        Nothing,
        [Description("Livro")]
        Book,
        [Description("Bebida")]
        Drink,
        [Description("Comida")]
        Food,
        [Description("Chave")]
        Key,
        [Description("Gazua")]
        LockPick,
        [Description("Poção")]
        Potion,
        [Description("Arma")]
        Weapon,
        [Description("Lixo")]
        Trash,
        [Description("Livro de Projeto")]
        CraftingBook,
        [Description("Mão primária")]
        PrimaryHand,
        [Description("Mão secundária")]
        SecondaryHand,
    }
}
