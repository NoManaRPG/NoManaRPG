using DragonsDiscordRPG.Extensoes;
using DragonsDiscordRPG.Game.Entidades;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using static DragonsDiscordRPG.Entidades.Extras;

namespace DragonsDiscordRPG.Entidades
{
    public class ModuloCliente
    {
        public static DiscordClient Client { get; private set; }
        public static BotCore Bot { get; private set; }
        public ModuloCliente(DiscordConfiguration discordConfiguration)
        {
            Client = new DiscordClient(discordConfiguration);
            Client.Ready += Client_Ready;
            Client.GuildAvailable += Client_GuildAvailable;
            Client.ClientErrored += Client_ClientError;
            Client.GuildMemberAdded += Client_GuildMemberAdded;
            Client.MessageCreated += Client_MessageCreated;
            Client.VoiceStateUpdated += Client_VoiceStateUpdated;
        }

        private async Task Client_VoiceStateUpdated(VoiceStateUpdateEventArgs e)
        {
            DiscordMember member;
            DiscordRole role;
            Regiao reg;

            if (e.Channel != null)
            {
                reg = await ModuloBanco.ColecaoRegiao.Find(x => x.IdVoz == e.Channel.Id).FirstAsync();
                if (reg != null)
                {
                    member = await e.Guild.GetMemberAsync(e.User.Id);
                    role = e.Guild.GetRole(reg.IdCargoTexto);
                    await member.GrantRoleAsync(role);
                    await ModuloBanco.ColecaoJogador.UpdateOneAsync(x => x.Id == e.User.Id,
                        new UpdateDefinitionBuilder<Jogador>().Set(x => x.IdVoz, reg.IdVoz),
                        new UpdateOptions() { IsUpsert = true });
                }
                return;
            }

            member = await e.Guild.GetMemberAsync(e.User.Id);
            Jogador jogador = await ModuloBanco.ColecaoJogador.Find(x => x.Id == e.User.Id).FirstAsync();
            reg = await ModuloBanco.ColecaoRegiao.Find(x => x.IdVoz == jogador.IdVoz).FirstAsync();
            role = e.Guild.GetRole(reg.IdCargoTexto);
            await member.RevokeRoleAsync(role);
            return;
        }

        private async Task Client_MessageCreated(MessageCreateEventArgs e)
        {
            if (e.Channel.Id != 742793406417338468)
            {

                return;
            }
            DiscordMember membro = await e.Guild.GetMemberAsync(e.Author.Id);
            DiscordRole role = e.Guild.GetRole(742785152933036174);
            await membro.GrantRoleAsync(role, "Avisou que estava sem ver os canais de vozes");
            await e.Message.DeleteAsync();
            return;
        }

        private Task Client_GuildMemberAdded(GuildMemberAddEventArgs e)
        {
            DiscordRole role = e.Guild.GetRole(742785152933036174);
            e.Member.GrantRoleAsync(role, "Entrou no servidor");
            return Task.CompletedTask;
        }

        private Task Client_Ready(ReadyEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "Dragon", "Cliente está pronto.", DateTime.Now);
            Bot = BotCore.LoadFromFile(EntrarPasta("") + "BotCore.json");
            if (Bot == null)
                Bot = new BotCore();
            Client.UpdateStatusAsync(new DiscordActivity($"!ajuda", ActivityType.Playing), UserStatus.Online);
#if DEBUG
            Bot.VersaoRevisao++;
            Bot.SaveToFile(EntrarPasta("") + "BotCore.json");
#endif
            return Task.CompletedTask;
        }

        private Task Client_GuildAvailable(GuildCreateEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "Dragon", $"Guilda {e.Guild.Name.RemoverAcentos()}", DateTime.Now);
            Bot.QuantidadeMembros += e.Guild.MemberCount;
            return Task.CompletedTask;
        }

        private Task Client_ClientError(ClientErrorEventArgs e)
        {
            string erro = $"Um erro aconteceu no client: {e.Exception.GetType()}: {e.Exception.Message}";
            e.Client.DebugLogger.LogMessage(LogLevel.Error, "Dragon", erro, DateTime.Now);
            DiscordChannel channel = e.Client.GetChannelAsync(742778666509008956).Result;
            e.Client.SendMessageAsync(channel, erro);
            return Task.CompletedTask;
        }
    }
}
