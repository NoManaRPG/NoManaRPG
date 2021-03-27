using DSharpPlus;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WafclastRPG.Extensions
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

        public static bool TryParseID(this string texto, out ulong id)
            => ulong.TryParse(texto.Replace("#", string.Empty), out id);

        public static string Titulo(this string titulo)
            => "⌈" + titulo + "⌋";

        public static string FirstUpper(this string texto)
            => texto.First().ToString().ToUpper() + texto.Substring(1);

        public static string Url(this string texto, string site)
            => Formatter.MaskedUrl($"`{texto}` ", new Uri(site));

        public static string FormatarURLComando(string prefixo, string texto, string hover, string site = "https://discord.gg/zjvb5kUc8q")
           => Formatter.MaskedUrl($"`{prefixo}{texto}` ", new Uri(site), hover);
    }
}