// This file is part of WafclastRPG project.

using System;
using System.Globalization;
using System.Text;
using DSharpPlus;

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

        public static string Title(this string titulo)
            => "⌈" + titulo + "⌋";

        public static string Url(this string texto, string site)
            => Formatter.MaskedUrl($"`{texto}` ", new Uri(site));
    }
}