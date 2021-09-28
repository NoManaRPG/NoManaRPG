using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Threading.Tasks;
using WafclastRPG.Context;
using WafclastRPG.Exceptions;
using WafclastRPG.Extensions;

namespace WafclastRPG.Entities {
  public struct AnswerResult<T> {
    public bool TimedOut;
    public T Value;

    public AnswerResult(bool timedOut, T result) {
      TimedOut = timedOut;
      Value = result;
    }
  }

  public class Interactivity {
    private readonly UsersBlocked _usersBlocked;
    private readonly CommandContext _ctx;
    private readonly InteractivityExtension _interactivityExtension;
    private readonly TimeSpan? _timeOut;

    public Interactivity(UsersBlocked usersBlocked, CommandContext commandContext, TimeSpan? timeOutOverride = null) {
      _usersBlocked = usersBlocked;
      _ctx = commandContext;
      _interactivityExtension = _ctx.Client.GetInteractivity();
      _timeOut = timeOutOverride;
    }

    private void BlockUser() => _usersBlocked.BlockUser(_ctx);
    private void UnblockUser() => _usersBlocked.UnblockUser(_ctx);

    public async Task<InteractivityResult<DiscordMessage>> WaitForMessageAsync(string message, DiscordEmbed embed) {
      await CommandContextExtension.RespondAsync(_ctx, message, embed);
      return await _interactivityExtension.WaitForMessageAsync(x => x.Author.Id == _ctx.User.Id && x.ChannelId == _ctx.Channel.Id, timeoutoverride: _timeOut);
    }

    public async Task<AnswerResult<T>> WaitForEnumAsync<T>(string message) where T : Enum {
      BlockUser();

      while (true) {
        var embed = new DiscordEmbedBuilder();
        embed.WithDescription(message);
        embed.WithFooter("Digite um numero ou 'sair' para fechar | Somente numeros inteiros");

        var wait = await WaitForMessageAsync(_ctx.User.Mention, embed.Build());

        if (wait.TimedOut) {
          await _ctx.ResponderAsync("tempo de resposta expirado!");
          UnblockUser();
          return new AnswerResult<T>(true, default);
        }

        if (Enum.TryParse(typeof(T), wait.Result.Content, out object result)) {
          UnblockUser();
          return new AnswerResult<T>(false, (T) result);
        }

        if (wait.Result.Content.ToLower().Trim() == "sair") {
          UnblockUser();
          return new AnswerResult<T>(true, default);
        }
      }
    }

    public async Task<AnswerResult<int>> WaitForIntAsync(string message, int? minValue = null, int? maxValue = null) {
      BlockUser();

      while (true) {
        var embed = new DiscordEmbedBuilder();
        embed.WithDescription(message);
        embed.WithFooter("Digite um numero ou 'sair' para fechar.");

        var wait = await WaitForMessageAsync(_ctx.User.Mention, embed.Build());

        if (wait.TimedOut) {
          await _ctx.ResponderAsync("tempo de resposta expirado!");
          UnblockUser();
          return new AnswerResult<int>(true, 0);
        }

        if (int.TryParse(wait.Result.Content, out int result)) {
          if (minValue != null)
            if (result < minValue)
              continue;
          if (maxValue != null)
            if (result > maxValue)
              continue;
          UnblockUser();
          return new AnswerResult<int>(false, result);
        }

        if (wait.Result.Content.ToLower().Trim() == "sair") {
          UnblockUser();
          return new AnswerResult<int>(true, 0);
        }
      }
    }

    public async Task<AnswerResult<double>> WaitForDoubleAsync(string message, double? minValue = null, double? maxValue = null) {
      BlockUser();

      while (true) {
        var embed = new DiscordEmbedBuilder();
        embed.WithDescription(message);
        embed.WithFooter("Digite um numero ou 'sair' para fechar.");

        var wait = await WaitForMessageAsync(_ctx.User.Mention, embed.Build());

        if (wait.TimedOut) {
          await _ctx.ResponderAsync("tempo de resposta expirado!");
          UnblockUser();
          return new AnswerResult<double>(true, 0);
        }

        if (double.TryParse(wait.Result.Content, out double result)) {
          if (minValue != null)
            if (result < minValue)
              continue;
          if (maxValue != null)
            if (result > maxValue)
              continue;
          UnblockUser();
          return new AnswerResult<double>(false, result);
        }

        if (wait.Result.Content.ToLower().Trim() == "sair") {
          UnblockUser();
          return new AnswerResult<double>(true, 0);
        }
      }
    }

    public async Task<AnswerResult<ulong>> WaitForUlongAsync(string message, ulong? minValue = null, ulong? maxValue = null) {
      BlockUser();

      while (true) {
        var embed = new DiscordEmbedBuilder();
        embed.WithDescription(message);
        embed.WithFooter("Digite um numero ou 'sair' para fechar | Somente numeros inteiros");

        var wait = await WaitForMessageAsync(_ctx.User.Mention, embed.Build());

        if (wait.TimedOut) {
          await _ctx.ResponderAsync("tempo de resposta expirado!");
          UnblockUser();
          return new AnswerResult<ulong>(true, 0);
        }

        if (ulong.TryParse(wait.Result.Content, out ulong result)) {
          if (minValue != null)
            if (result < minValue)
              continue;
          if (maxValue != null)
            if (result > maxValue)
              continue;
          UnblockUser();
          return new AnswerResult<ulong>(false, result);
        }

        if (wait.Result.Content.ToLower().Trim() == "sair") {
          UnblockUser();
          return new AnswerResult<ulong>(true, 0);
        }
      }
    }

    public async Task<bool> WaitForBoolAsync(DiscordEmbed embed) {
      bool isWrong = false;
      BlockUser();

      while (true) {
        InteractivityResult<DiscordMessage> wait;
        if (isWrong)
          wait = await WaitForMessageAsync($"{_ctx.User.Mention}, você informou uma resposta inválida! Responda com 'Sim' ou 'Não'.", embed);
        else
          wait = await WaitForMessageAsync("", embed);

        if (wait.TimedOut) {
          await _ctx.ResponderAsync("tempo de resposta expirado!");
          UnblockUser();
          return false;
        }

        switch (wait.Result.Content.ToLower().Trim()) {
          case "sim":
            UnblockUser();
            return true;
          case "nao":
          case "não":
            UnblockUser();
            return false;
          default:
            isWrong = true;
            break;
        }
      }
    }

    public async Task<AnswerResult<string>> WaitForStringAsync(string message) {
      BlockUser();

      var embed = new DiscordEmbedBuilder();
      embed.WithDescription(message);
      embed.WithFooter("Digite 'sair' para fechar.");

      var wait = await WaitForMessageAsync(_ctx.User.Mention, embed.Build());
      if (wait.TimedOut)
        throw new AnswerTimeoutException();

      if (wait.Result.Content.ToLower().Trim() == "sair") {
        UnblockUser();
        return new AnswerResult<string>(true, null);
      }

      UnblockUser();
      return new AnswerResult<string>(false, wait.Result.Content);
    }
  }
}
