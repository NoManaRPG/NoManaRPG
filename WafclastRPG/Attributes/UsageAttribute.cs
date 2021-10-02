﻿// This file is part of the WafclastRPG project.

using System;

namespace WafclastRPG.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class UsageAttribute : Attribute
    {
        public string Command { get; }
        public UsageAttribute(string command)
        {
            this.Command = command;
        }
    }
}
