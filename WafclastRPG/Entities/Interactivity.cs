using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Threading.Tasks;
using WafclastRPG.Commands;
using WafclastRPG.Exceptions;
using WafclastRPG.Extensions;
using WafclastRPG.Repositories.Interfaces;

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
    private readonly IInteractivityRepository _interac;
    private CommandContext _ctx;
    private InteractivityExtension _interactivityExtension;
    private TimeSpan _timeOutOverride;

    public Interactivity(IInteractivityRepository blockerRepository, CommandContext commandContext) {
      _interac = blockerRepository;
      _ctx = commandContext;
      _interactivityExtension = _ctx.Client.GetInteractivity();
    }

    public Interactivity(IInteractivityRepository blockerRepository, CommandContext commandContext, TimeSpan timeOutOverride) {
      _interac = blockerRepository;
      _ctx = commandContext;
      _interactivityExtension = _ctx.Client.GetInteractivity();
      _timeOutOverride = timeOutOverride;
    }

    public async Task<InteractivityResult<DiscordMessage>> WaitForMessageAsync(string message, DiscordEmbed embed) {

      await _ctx.ResponderAsync(message, embed);
      return await _interactivityExtension.WaitForMessageAsync(x => x.Author.Id == _ctx.User.Id && x.ChannelId == _ctx.Channel.Id, timeoutoverride: _timeOutOverride);
    }

    public async Task<AnswerResult<T>> WaitForEnumAsync<T>(string message) where T : Enum {
      _interac.Block(_ctx);

      while (true) {
        var embed = new DiscordEmbedBuilder();
        embed.WithDescription(message);
        embed.WithFooter("Digite um numero ou 'sair' para fechar | Somente numeros inteiros");

        var wait = await WaitForMessageAsync(_ctx.User.Mention, embed.Build());

        if (wait.TimedOut) {
          await _ctx.ResponderAsync("tempo de resposta expirado!");
          _interac.Unblock(_ctx);
          return new AnswerResult<T>(true, default);
        }

        if (Enum.TryParse(typeof(T), wait.Result.Content, out object result)) {
          _interac.Unblock(_ctx);
          return new AnswerResult<T>(false, (T) result);
        }

        if (wait.Result.Content.ToLower().Trim() == "sair") {
          _interac.Unblock(_ctx);
          return new AnswerResult<T>(true, default);
        }
      }
    }

    public async Task<AnswerResult<int>> WaitForIntAsync(string message, int? minValue = null, int? maxValue = null) {
      _interac.Block(_ctx);

      while (true) {
        var embed = new DiscordEmbedBuilder();
        embed.WithDescription(message);
        embed.WithFooter("Digite um numero ou 'sair' para fechar.");

        var wait = await WaitForMessageAsync(_ctx.User.Mention, embed.Build());

        if (wait.TimedOut) {
          await _ctx.ResponderAsync("tempo de resposta expirado!");
          _interac.Unblock(_ctx);
          return new AnswerResult<int>(true, 0);
        }

        if (int.TryParse(wait.Result.Content, out int result)) {
          if (minValue != null)
            if (result < minValue)
              continue;
          if (maxValue != null)
            if (result > maxValue)
              continue;
          _interac.Unblock(_ctx);
          return new AnswerResult<int>(false, result);
        }

        if (wait.Result.Content.ToLower().Trim() == "sair") {
          _interac.Unblock(_ctx);
          return new AnswerResult<int>(true, 0);
        }
      }
    }

    public async Task<AnswerResult<double>> WaitForDoubleAsync(string message, double? minValue = null, double? maxValue = null) {
      _interac.Block(_ctx);

      while (true) {
        var embed = new DiscordEmbedBuilder();
        embed.WithDescription(message);
        embed.WithFooter("Digite um numero ou 'sair' para fechar.");

        var wait = await WaitForMessageAsync(_ctx.User.Mention, embed.Build());

        if (wait.TimedOut) {
          await _ctx.ResponderAsync("tempo de resposta expirado!");
          _interac.Unblock(_ctx);
          return new AnswerResult<double>(true, 0);
        }

        if (double.TryParse(wait.Result.Content, out double result)) {
          if (minValue != null)
            if (result < minValue)
              continue;
          if (maxValue != null)
            if (result > maxValue)
              continue;
          _interac.Unblock(_ctx);
          return new AnswerResult<double>(false, result);
        }

        if (wait.Result.Content.ToLower().Trim() == "sair") {
          _interac.Unblock(_ctx);
          return new AnswerResult<double>(true, 0);
        }
      }
    }

    public async Task<AnswerResult<ulong>> WaitForUlongAsync(string message, ulong? minValue = null, ulong? maxValue = null) {
      _interac.Block(_ctx);

      while (true) {
        var embed = new DiscordEmbedBuilder();
        embed.WithDescription(message);
        embed.WithFooter("Digite um numero ou 'sair' para fechar | Somente numeros inteiros");

        var wait = await WaitForMessageAsync(_ctx.User.Mention, embed.Build());

        if (wait.TimedOut) {
          await _ctx.ResponderAsync("tempo de resposta expirado!");
          _interac.Unblock(_ctx);
          return new AnswerResult<ulong>(true, 0);
        }

        if (ulong.TryParse(wait.Result.Content, out ulong result)) {
          if (minValue != null)
            if (result < minValue)
              continue;
          if (maxValue != null)
            if (result > maxValue)
              continue;
          _interac.Unblock(_ctx);
          return new AnswerResult<ulong>(false, result);
        }

        if (wait.Result.Content.ToLower().Trim() == "sair") {
          _interac.Unblock(_ctx);
          return new AnswerResult<ulong>(true, 0);
        }
      }
    }

    public async Task<bool> WaitForBoolAsync(DiscordEmbed embed ) {
      bool isWrong = false;
      _interac.Block(_ctx.User.Id);

      while (true) {
        InteractivityResult<DiscordMessage> wait;
        if (isWrong)
          wait = await WaitForMessageAsync($"{_ctx.User.Mention}, você informou uma resposta inválida! Responda com 'Sim' ou 'Não'.", embed);
        else
          wait = await WaitForMessageAsync( "", embed);

        if (wait.TimedOut) {
          await _ctx.ResponderAsync("tempo de resposta expirado!");
          _interac.Unblock(_ctx.User.Id);
          return false;
        }

        switch (wait.Result.Content.ToLower().Trim()) {
          case "sim":
            _interac.Unblock(_ctx.User.Id);
            return true;
          case "nao":
          case "não":
            _interac.Unblock(_ctx.User.Id);
            return false;
          default:
            isWrong = true;
            break;
        }
      }
    }

    public async Task<AnswerResult<string>> WaitForStringAsync(string message) {
      _interac.Block(_ctx.User.Id);

      var embed = new DiscordEmbedBuilder();
      embed.WithDescription(message);
      embed.WithFooter("Digite 'sair' para fechar.");

      var wait = await WaitForMessageAsync(_ctx.User.Mention, embed.Build());
      if (wait.TimedOut)
        throw new AnswerTimeoutException();

      if (wait.Result.Content.ToLower().Trim() == "sair") {
        _interac.Unblock(_ctx.User.Id);
        return new AnswerResult<string>(true, null);
      }

      _interac.Unblock(_ctx.User.Id);
      return new AnswerResult<string>(false, wait.Result.Content);
    }
  }
}
