using DSharpPlus;

namespace WafclastRPG.DataBases
{
    public static class Strings
    {
        public static string MonstroNaoEncontrado(ulong id) => $"não foi encontrado nenhum monstro com o {Formatter.Bold($"#ID {id}")}!";
        public static string MonstroNaoEncontrado(int id) => $"não foi encontrado nenhum monstro com o {Formatter.Bold($"#ID {id}")}!";
        public static string LocalDiferente(string nome) => $"você não está em {Formatter.Bold(nome)}!";

        public static string NovoJogador = $"você precisa usar o comando {Formatter.InlineCode("comecar")} antes!";
        public static string IdInvalido = $"você informou um {Formatter.Bold("#ID")} inválido!";

    }
}
