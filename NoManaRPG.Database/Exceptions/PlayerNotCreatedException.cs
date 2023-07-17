// This file is part of WafclastRPG project.

using System;
using NoManaRPG.Game.Properties;

namespace NoManaRPG.Database.Exceptions
{
    public class PlayerNotCreatedException : Exception
    {
        public new string Message = Messages.AindaNaoCriouPersonagem;
    }
}
