using System.Globalization;
using System.Linq;
using System.Text;

namespace WafclastRPG.Bot.Extensoes
{
    public static class StringExtension
    {
        public static string RemoverAcentos(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static bool TryParseID(this string texto, out int id)
            => int.TryParse(texto.Replace("#", string.Empty), out id);

        public static string Titulo(this string titulo)
            => "**⌈" + titulo + "⌋**";

        public static string FirstUpper(this string texto)
            => texto.First().ToString().ToUpper() + texto.Substring(1);
    }
}