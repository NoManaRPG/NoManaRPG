using DSharpPlus.Entities;

namespace WafclastRPG.Commands.CommandResponse {
  public class EmbedResponse : IResponse {
    public DiscordEmbedBuilder Response { get; }

    public EmbedResponse(DiscordEmbedBuilder response) {
      Response = response;
    }
  }
}
