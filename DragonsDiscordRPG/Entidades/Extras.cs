using System;
using System.IO;
using System.Text;

namespace DragonsDiscordRPG.Entidades
{
    public static class Extras
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
    }
}
