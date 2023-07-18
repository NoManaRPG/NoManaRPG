// This file is part of NoManaRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using NoManaRPG.Attributes;

namespace NoManaRPG.Commands.UserCommands;

public class StartCommand : ApplicationCommandModule
{
    //private readonly PlayerRepository _playerRepository;

    //public StartCommand(PlayerRepository playerRepository, MongoSession session)
    //{
    //    this._playerRepository = playerRepository;
    //}

    [Command("comecar")]
    [Aliases("start")]
    [Description("Permite criar um personagem.")]
    [Usage("comecar")]
    [SlashCommand("cade", "Sumiu?")]
    public async Task StartCommandAsync(InteractionContext ctx)
    {
        var d = new DiscordEmbedBuilder();
        d.WithDescription("É o que parece neh.. mas não, o bot não sumiu e em breve deve voltar com coisas novas! " +
            "Foi recriado o nosso servidor de suporte e caso você queria ficar por" +
            " dentro das novidades que estão chegando, não deixe de acompanhar aqui: https://discord.gg/Ht2uvSr4NW");

        await ctx.CreateResponseAsync(d, true);


        //var player = await this._playerRepository.FindPlayerOrDefaultAsync(ctx.Member.Id);
        //if (player != null)
        //{
        //    await ctx.RespondAsync("você já criou um personagem! Se estiver com dúvidas ou problemas, consulte o nosso Servidor Oficial do Discord.");
        //    return;
        //}

        //context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        //// do your stuff here
        //context.EditResponseAsync(new DiscordWebhookBuilder());
        // you cannot use an InteractionResponseBuilder here.
        // If you need to send ephemeral responses, pass an empty
        // DiscordInteractionResponseBuilder to the CreateResponseAsync
        // and just call .AsEphemeral() on it

        //player = new Player(ctx.User.Id);

        //await this._playerRepository.SavePlayerAsync(player);
        //await ctx.RespondAsync("personagem criado com sucesso! Obrigado por escolher Wafclast!");
    }
}
