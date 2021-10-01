namespace WafclastRPG.Commands.CommandResponse {
  public class StringResponse : IResponse {
    public string Response { get; }

    public StringResponse(string response) {
      Response = response;
    }
  }
}
