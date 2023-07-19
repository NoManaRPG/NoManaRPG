// This file is part of NoManaRPG project.

using System;
using NoManaRPG.Properties;

namespace NoManaRPG.Exceptions;

public class PlayerNotCreatedException : Exception
{
    public new string Message = Messages.AindaNaoCriouPersonagem;
}
