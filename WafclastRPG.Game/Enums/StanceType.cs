using System.ComponentModel;

namespace WafclastRPG.Game.Enums
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
