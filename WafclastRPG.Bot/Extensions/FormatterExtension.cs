using DSharpPlus;

namespace WafclastRPG.Bot.Extensions
{
    public static class FormatterExtension
    {
        public static string Bold(this string text)
            => Formatter.Bold(text);

        public static string Bold(this int numero)
         => Formatter.Bold(numero.ToString());

        public static string Code(this string text)
            => Formatter.InlineCode(text);
    }
}
