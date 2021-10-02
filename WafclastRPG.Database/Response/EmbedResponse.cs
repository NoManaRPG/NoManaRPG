﻿// This file is part of the WafclastRPG project.

using DSharpPlus.Entities;

namespace WafclastRPG.Database.Response
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
