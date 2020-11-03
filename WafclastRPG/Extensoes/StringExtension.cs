using DSharpPlus.CommandsNext;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WafclastRPG.Extensoes
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

        public static string Underline(this string texto)
            => $"__{texto}__";

        public static string Bold(this string texto)
           => $"**{texto}**";

        public static string Bold(this long texto)
           => $"**{texto}**";

        public static string Bold(this double texto)
          => $"**{texto}**";

        public static string Bold(this int texto)
       => $"**{texto}**";

        public static string Italic(this string texto)
            => $"*{texto}*";
    }
}