using System;
using WafclastRPG.Game.Properties;

namespace WafclastRPG.Database.Exceptions {
  public class PlayerNotCreatedException : Exception {
    public new string Message = Messages.AindaNaoCriouPersonagem;
  }
}
