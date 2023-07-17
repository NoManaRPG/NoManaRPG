﻿// This file is part of WafclastRPG project.

using System;

namespace NoManaRPG.Attributes
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