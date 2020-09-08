using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DragonsDiscordRPG.Extensoes
{
    public static class StringExtension
    {
        /// <summary>
        /// Permite entrar em uma pasta, após a pasta raiz.
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public static string EntrarPasta(string nome)
        {
            StringBuilder raizProjeto = new StringBuilder();
#if DEBUG
            raizProjeto.Append(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\")));
            raizProjeto.Replace(@"/", @"\");
            return raizProjeto + nome + @"\";
#else
            raizProjeto.Append(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../../")));
            raizProjeto.Replace(@"\", @"/");
            return raizProjeto + nome + @"/";
#endif
        }

        public static string Titulo(this string titulo)
            => "**⌈" + titulo + "⌋**";

        public static string FirstUpper(this string texto)
            => texto.First().ToString().ToUpper() + texto.Substring(1);

        public static string Underline(this string texto)
            => $"__{texto}__";

        public static string Bold(this string texto)
           => $"**{texto}**";

        public static string Bold(this double texto)
          => $"**{texto}**";

        public static string Bold(this int texto)
       => $"**{texto}**";

        public static string Italic(this string texto)
            => $"*{texto}*";

        public static string RemoverAcentos(this string texto)
          => Regex.Replace(texto, @"[^\u0000-\u007F]+", string.Empty);
    }
}