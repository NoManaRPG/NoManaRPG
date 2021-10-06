// This file is part of the WafclastRPG project.

using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using WafclastRPG.Database;
using WafclastRPG.Exceptions;
using WafclastRPG.Extensions;

namespace WafclastRPG
{
    public struct AnswerResult<T>
    {
        public bool TimedOut { get; }
        public T Result { get; }

        public AnswerResult(bool timedOut, T result)
        {
            this.TimedOut = timedOut;
            this.Result = result;
        }
    }

    public class Interactivity
    {
        private readonly UsersBlocked _usersBlocked;
        private readonly CommandContext _ctx;
        private readonly InteractivityExtension _interactivityExtension;
        private readonly TimeSpan? _timeOut;
        private readonly Func<DiscordMessage, bool> _waitMessage;

        public Interactivity(UsersBlocked usersBlocked, CommandContext commandContext, TimeSpan? timeOutOverride = null)
        {
            this._usersBlocked = usersBlocked;
            this._ctx = commandContext;
            this._interactivityExtension = this._ctx.Client.GetInteractivity();
            this._timeOut = timeOutOverride;
            this._waitMessage = new Func<DiscordMessage, bool>(x => x.Author.Id == this._ctx.User.Id && x.ChannelId == this._ctx.Channel.Id);
        }

        private void BlockUser() => this._usersBlocked.BlockUser(this._ctx);
        private void UnblockUser() => this._usersBlocked.UnblockUser(this._ctx);

        public async Task<InteractivityResult<DiscordMessage>> WaitForMessageAsync(string message, DiscordEmbedBuilder embed)
        {
            await CommandContextExtension.RespondAsync(this._ctx, embed);
            return await this._interactivityExtension.WaitForMessageAsync(x => x.Author.Id == this._ctx.User.Id && x.ChannelId == this._ctx.Channel.Id, timeoutoverride: this._timeOut);
        }

        public async Task<AnswerResult<string>> WaitForMessageAsync(string message)
        {
            await this._ctx.RespondAsync(message);
            var msg = await this._interactivityExtension.WaitForMessageAsync(this._waitMessage, timeoutoverride: this._timeOut);
            return new AnswerResult<string>(msg.TimedOut, msg.Result.Content);
        }

        public async Task<AnswerResult<string>> WaitForMessageAsync()
        {
            var msg = await this._interactivityExtension.WaitForMessageAsync(this._waitMessage, timeoutoverride: this._timeOut);
            return new AnswerResult<string>(msg.TimedOut, msg.Result.Content);
        }

        public async Task<AnswerResult<T>> WaitForEnumAsync<T>(string message) where T : Enum
        {
            this.BlockUser();

            while (true)
            {
                var embed = new DiscordEmbedBuilder();
                embed.WithDescription(message);
                embed.WithFooter("Digite um numero ou 'sair' para fechar | Somente numeros inteiros");

                var wait = await this.WaitForMessageAsync(this._ctx.User.Mention, embed);

                if (wait.TimedOut)
                {
                    await this._ctx.RespondAsync("tempo de resposta expirado!");
                    this.UnblockUser();
                    return new AnswerResult<T>(true, default);
                }

                if (Enum.TryParse(typeof(T), wait.Result.Content, out object result))
                {
                    this.UnblockUser();
                    return new AnswerResult<T>(false, (T)result);
                }

                if (wait.Result.Content.ToLower().Trim() == "sair")
                {
                    this.UnblockUser();
                    return new AnswerResult<T>(true, default);
                }
            }
        }

        public async Task<AnswerResult<int>> WaitForIntAsync(string message, int? minValue = null, int? maxValue = null)
        {
            this.BlockUser();

            while (true)
            {
                var embed = new DiscordEmbedBuilder();
                embed.WithDescription(message);
                embed.WithFooter("Digite um numero ou 'sair' para fechar.");

                var wait = await this.WaitForMessageAsync(this._ctx.User.Mention, embed);

                if (wait.TimedOut)
                {
                    await this._ctx.RespondAsync("tempo de resposta expirado!");
                    this.UnblockUser();
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
                    this.UnblockUser();
                    return new AnswerResult<int>(false, result);
                }

                if (wait.Result.Content.ToLower().Trim() == "sair")
                {
                    this.UnblockUser();
                    return new AnswerResult<int>(true, 0);
                }
            }
        }

        public async Task<AnswerResult<double>> WaitForDoubleAsync(string message, double? minValue = null, double? maxValue = null)
        {
            this.BlockUser();

            while (true)
            {
                var embed = new DiscordEmbedBuilder();
                embed.WithDescription(message);
                embed.WithFooter("Digite um numero ou 'sair' para fechar.");

                var wait = await this.WaitForMessageAsync(this._ctx.User.Mention, embed);

                if (wait.TimedOut)
                {
                    await this._ctx.RespondAsync("tempo de resposta expirado!");
                    this.UnblockUser();
                    return new AnswerResult<double>(true, 0);
                }

                if (double.TryParse(wait.Result.Content, out double result))
                {
                    if (minValue != null)
                        if (result < minValue)
                            continue;
                    if (maxValue != null)
                        if (result > maxValue)
                            continue;
                    this.UnblockUser();
                    return new AnswerResult<double>(false, result);
                }

                if (wait.Result.Content.ToLower().Trim() == "sair")
                {
                    this.UnblockUser();
                    return new AnswerResult<double>(true, 0);
                }
            }
        }

        public async Task<AnswerResult<ulong>> WaitForUlongAsync(string message, ulong? minValue = null, ulong? maxValue = null)
        {
            this.BlockUser();

            while (true)
            {
                var embed = new DiscordEmbedBuilder();
                embed.WithDescription(message);
                embed.WithFooter("Digite um numero ou 'sair' para fechar | Somente numeros inteiros");

                var wait = await this.WaitForMessageAsync(this._ctx.User.Mention, embed);

                if (wait.TimedOut)
                {
                    await this._ctx.RespondAsync("tempo de resposta expirado!");
                    this.UnblockUser();
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
                    this.UnblockUser();
                    return new AnswerResult<ulong>(false, result);
                }

                if (wait.Result.Content.ToLower().Trim() == "sair")
                {
                    this.UnblockUser();
                    return new AnswerResult<ulong>(true, 0);
                }
            }
        }

        public async Task<bool> WaitForBoolAsync(DiscordEmbedBuilder embed)
        {
            bool isWrong = false;
            this.BlockUser();

            while (true)
            {
                InteractivityResult<DiscordMessage> wait;
                if (isWrong)
                    wait = await this.WaitForMessageAsync($"{this._ctx.User.Mention}, você informou uma resposta inválida! Responda com 'Sim' ou 'Não'.", embed);
                else
                    wait = await this.WaitForMessageAsync("", embed);

                if (wait.TimedOut)
                {
                    await this._ctx.RespondAsync("tempo de resposta expirado!");
                    this.UnblockUser();
                    return false;
                }

                switch (wait.Result.Content.ToLower().Trim())
                {
                    case "sim":
                        this.UnblockUser();
                        return true;
                    case "nao":
                    case "não":
                        this.UnblockUser();
                        return false;
                    default:
                        isWrong = true;
                        break;
                }
            }
        }

        public async Task<AnswerResult<string>> WaitForStringAsync(string message)
        {
            this.BlockUser();

            var embed = new DiscordEmbedBuilder();
            embed.WithDescription(message);
            embed.WithFooter("Digite 'sair' para fechar.");

            var wait = await this.WaitForMessageAsync(this._ctx.User.Mention, embed);
            if (wait.TimedOut)
                throw new AnswerTimeoutException();

            if (wait.Result.Content.ToLower().Trim() == "sair")
            {
                this.UnblockUser();
                return new AnswerResult<string>(true, null);
            }

            this.UnblockUser();
            return new AnswerResult<string>(false, wait.Result.Content);
        }
    }
}
