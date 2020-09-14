using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Atributos
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ComoUsarAttribute : CheckBaseAttribute
    {
        public string Mensagem { get; }
        public ComoUsarAttribute(string uso) => Mensagem = uso;
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help) => Task.FromResult(true);
    }
}
