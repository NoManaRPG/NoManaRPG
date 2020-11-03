using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;

namespace WafclastRPG.Atributos
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ExemploAttribute : CheckBaseAttribute
    {
        public string Mensagem { get; }
        public ExemploAttribute(string exemplo) => Mensagem = exemplo;
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help) => Task.FromResult(true);
    }
}
