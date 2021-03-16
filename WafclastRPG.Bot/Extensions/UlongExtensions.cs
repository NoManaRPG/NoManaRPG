using System.Globalization;

namespace WafclastRPG.Bot.Extensions
{
    public static class UlongExtensions
    {
        public static string Mention(this ulong id)
            => $"<@{id.ToString(CultureInfo.InvariantCulture)}>";
    }
}
