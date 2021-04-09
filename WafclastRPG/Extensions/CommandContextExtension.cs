using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Threading.Tasks;
using WafclastRPG.DataBases;

namespace WafclastRPG.Extensions
{
    public struct AnswerResult<T>
    {
        public bool TimedOut;
        public T Value;

        public AnswerResult(bool timedOut, T result)
        {
            TimedOut = timedOut;
            Value = result;
        }
    }

    public static class CommandContextExtension
    {
        public static Task<DiscordMessage> ResponderAsync(this CommandContext ctx, string mensagem)
            => ctx.RespondAsync($"{ctx.User.Mention}, {mensagem}");

        public static Task ResponderAsync(this CommandContext ctx, string mensagem, DiscordEmbed embed)
            => ctx.RespondAsync($"{ctx.User.Mention}, {mensagem}", embed);

        public static Task ResponderAsync(this CommandContext ctx, DiscordEmbed embed)
            => ctx.RespondAsync(ctx.User.Mention, embed: embed);

        public static async Task<InteractivityResult<DiscordMessage>> WaitForMessageAsync(this CommandContext ctx, string message, DiscordEmbed embed, TimeSpan? timeoutoverride = null)
        {
            var vity = ctx.Client.GetInteractivity();
            await ctx.ResponderAsync(message, embed);
            return await vity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.ChannelId == ctx.Channel.Id, timeoutoverride: timeoutoverride);
        }

        public static async Task<AnswerResult<T>> WaitForEnumAsync<T>(this CommandContext ctx, string message, DataBase database, TimeSpan? timeoutoverride = null) where T : Enum
        {
            database.StartExecutingInteractivity(ctx);

            while (true)
            {
                var embed = new DiscordEmbedBuilder();
                embed.WithDescription(message);
                embed.WithFooter("Digite um numero ou 'sair' para fechar | Somente numeros inteiros");

                var wait = await WaitForMessageAsync(ctx, ctx.User.Mention, embed.Build(), timeoutoverride);

                if (wait.TimedOut)
                {
                    await ctx.ResponderAsync("tempo de resposta expirado!");
                    database.StopExecutingInteractivity(ctx);
                    return new AnswerResult<T>(true, default(T));
                }

                if (Enum.TryParse(typeof(T), wait.Result.Content, out object result))
                {
                    database.StopExecutingInteractivity(ctx);
                    return new AnswerResult<T>(false, (T)result);
                }

                if (wait.Result.Content.ToLower().Trim() == "sair")
                {
                    database.StopExecutingInteractivity(ctx);
                    return new AnswerResult<T>(true, default(T));
                }
            }
        }

        public static async Task<AnswerResult<int>> WaitForIntAsync(this CommandContext ctx, string message, DataBase database, TimeSpan? timeoutoverride = null, int? minValue = null, int? maxValue = null)
        {
            database.StartExecutingInteractivity(ctx);

            while (true)
            {
                var embed = new DiscordEmbedBuilder();
                embed.WithDescription(message);
                embed.WithFooter("Digite um numero ou 'sair' para fechar.");

                var wait = await WaitForMessageAsync(ctx, ctx.User.Mention, embed.Build(), timeoutoverride);

                if (wait.TimedOut)
                {
                    await ctx.ResponderAsync("tempo de resposta expirado!");
                    database.StopExecutingInteractivity(ctx);
                    return new AnswerResult<int>(true, 0);
                }

                if (int.TryParse(wait.Result.Content, out int result))
                {
                    if (minValue != null)
                        if (result < minValue)
                            continue;
                    if (maxValue != null)
                        if (result > maxValue)
                            continue;
                    database.StopExecutingInteractivity(ctx);
                    return new AnswerResult<int>(false, result);
                }

                if (wait.Result.Content.ToLower().Trim() == "sair")
                {
                    database.StopExecutingInteractivity(ctx);
                    return new AnswerResult<int>(true, 0);
                }
            }   
        }

        public static async Task<AnswerResult<ulong>> WaitForUlongAsync(this CommandContext ctx, string message, DataBase database, TimeSpan? timeoutoverride = null, ulong? minValue = null, ulong? maxValue = null)
        {
            database.StartExecutingInteractivity(ctx);

            while (true)
            {
                var embed = new DiscordEmbedBuilder();
                embed.WithDescription(message);
                embed.WithFooter("Digite um numero ou 'sair' para fechar | Somente numeros inteiros");

                var wait = await WaitForMessageAsync(ctx, ctx.User.Mention, embed.Build(), timeoutoverride);

                if (wait.TimedOut)
                {
                    await ctx.ResponderAsync("tempo de resposta expirado!");
                    database.StopExecutingInteractivity(ctx);
                    return new AnswerResult<ulong>(true, 0);
                }

                if (ulong.TryParse(wait.Result.Content, out ulong result))
                {
                    if (minValue != null)
                        if (result < minValue)
                            continue;
                    if (maxValue != null)
                        if (result > maxValue)
                            continue;
                    database.StopExecutingInteractivity(ctx);
                    return new AnswerResult<ulong>(false, result);
                }

                if (wait.Result.Content.ToLower().Trim() == "sair")
                {
                    database.StopExecutingInteractivity(ctx);
                    return new AnswerResult<ulong>(true, 0);
                }
            }
        }

        public static async Task<bool> WaitForBoolAsync(this CommandContext ctx, DataBase banco, DiscordEmbed embed, TimeSpan? timeoutoverride = null)
        {
            bool isWrong = false;
            banco.StartExecutingInteractivity(ctx.User.Id);

            while (true)
            {
                InteractivityResult<DiscordMessage> wait;
                if (isWrong)
                    wait = await WaitForMessageAsync(ctx, $"{ctx.User.Mention}, você informou uma resposta inválida! Responda com 'Sim' ou 'Não'.", embed, timeoutoverride);
                else
                    wait = await WaitForMessageAsync(ctx, ctx.User.Mention, embed, timeoutoverride);
                isWrong = false;

                if (wait.TimedOut)
                {
                    await ctx.ResponderAsync("tempo de resposta expirado!");
                    banco.StopExecutingInteractivity(ctx.User.Id);
                    return false;
                }

                switch (wait.Result.Content.ToLower().Trim())
                {
                    case "sim":
                        banco.StopExecutingInteractivity(ctx.User.Id);
                        return true;
                    case "nao":
                    case "não":
                        banco.StopExecutingInteractivity(ctx.User.Id);
                        return false;
                    default:
                        isWrong = true;
                        break;
                }
            }
        }

        public static async Task<AnswerResult<string>> WaitForStringAsync(this CommandContext ctx, string message, DataBase banco, TimeSpan? timeoutoverride = null)
        {
            banco.StartExecutingInteractivity(ctx.User.Id);

            var embed = new DiscordEmbedBuilder();
            embed.WithDescription(message);
            embed.WithFooter("Digite 'sair' para fechar.");

            var wait = await WaitForMessageAsync(ctx, ctx.User.Mention, embed.Build(), timeoutoverride);
            if (wait.TimedOut)
            {
                await ctx.ResponderAsync("tempo de resposta expirado!");
                banco.StopExecutingInteractivity(ctx.User.Id);
                return new AnswerResult<string>(true, null);
            }

            if (wait.Result.Content.ToLower().Trim() == "sair")
            {
                banco.StopExecutingInteractivity(ctx.User.Id);
                return new AnswerResult<string>(true, null);
            }

            return new AnswerResult<string>(false, wait.Result.Content);
        }
    }
}
