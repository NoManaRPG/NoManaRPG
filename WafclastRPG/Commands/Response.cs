using DSharpPlus.Entities;

namespace WafclastRPG.Commands {
  public class Response {
    public DiscordEmbedBuilder Embed { get; set; }
    public string Message { get; set; }

    public Response(DiscordEmbedBuilder embed) => Embed = embed;
    public Response(string message) => Message = message;
    public Response() {
    }
  }
}
