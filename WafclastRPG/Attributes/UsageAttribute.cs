using System;

namespace WafclastRPG.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class UsageAttribute : Attribute
    {
        public string Command { get; }
        public UsageAttribute(string command)
        {
            Command = command;
        }
    }
}
