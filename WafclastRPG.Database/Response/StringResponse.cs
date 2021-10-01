namespace WafclastRPG.Database.Response {
  public class StringResponse : IResponse {
    public string Response { get; }

    public StringResponse(string response) {
      Response = response;
    }
  }
}
