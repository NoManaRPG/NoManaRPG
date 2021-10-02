// This file is part of the WafclastRPG project.

using System;
using WafclastRPG.Game.Properties;

namespace WafclastRPG.Exceptions
{
    public class AnswerTimeoutException : Exception
    {
        public new string Message = Messages.TempoExpirado;
    }
}
