using System.ComponentModel;

namespace WafclastRPG.Enums
{
    public enum StanceType : int
    {
        /// <summary>
        /// Modo desviar.
        /// </summary>
        [Description("Desviar")]
        Parry,

        /// <summary>
        /// Modo defender.
        /// </summary>
        [Description("Defender")]
        Defend
    }
}
