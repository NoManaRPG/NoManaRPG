using System;
using WafclastRPG.Properties;

namespace WafclastRPG.Exceptions {
  public class PlayerNotCreated : Exception {
    public new string Message = Messages.AindaNaoCriouPersonagem;
  }
}
