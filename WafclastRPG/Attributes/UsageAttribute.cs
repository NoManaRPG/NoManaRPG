using System;

namespace WafclastRPG.Attributes
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
