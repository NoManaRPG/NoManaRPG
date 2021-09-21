using System;
using WafclastRPG.Properties;

namespace WafclastRPG.Exceptions {
  public class PlayerNotCreatedException : Exception {
    public new string Message = Messages.AindaNaoCriouPersonagem;
  }
}
