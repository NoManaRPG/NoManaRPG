using System;
using WafclastRPG.Properties;

namespace WafclastRPG.Exceptions {
  public class AnswerTimeoutException : Exception {
    public new string Message = Messages.TempoExpirado;
  }
}
