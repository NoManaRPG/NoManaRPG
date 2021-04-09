using System.Globalization;

namespace WafclastRPG.Extensions
{
    public static class UlongExtensions
    {
        public static string UserMention(this ulong id)
            => $"<@{id.ToString(CultureInfo.InvariantCulture)}>";
    }
}
