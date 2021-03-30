using System.ComponentModel;

namespace WafclastRPG.Enums
{
    public enum MapType : int
    {
        [Description("Cidade")]
        Cidade,
        [Description("Vila")]
        Vila,
        [Description("Aldeia")]
        Aldeia,
        [Description("Floresta")]
        Floresta
    }
}
