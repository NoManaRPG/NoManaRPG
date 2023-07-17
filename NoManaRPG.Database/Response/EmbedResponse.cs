// This file is part of WafclastRPG project.

using DSharpPlus.Entities;

namespace NoManaRPG.Database.Response
{
    public class EmbedResponse : IResponse
    {
        public DiscordEmbedBuilder Response { get; }

        public EmbedResponse(DiscordEmbedBuilder response)
        {
            this.Response = response;
        }
    }
}
