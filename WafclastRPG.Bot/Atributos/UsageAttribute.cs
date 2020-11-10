using System;

namespace WafclastRPG.Bot.Atributos
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class UsageAttribute : Attribute
    {
        public string Command { get; }
        public UsageAttribute(string command)
        {
            Command = command;
        }
    }
}
