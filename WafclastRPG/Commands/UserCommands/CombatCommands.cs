// This file is part of the WafclastRPG project.

using System;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using WafclastRPG.Attributes;
using WafclastRPG.Database;
using WafclastRPG.Database.Repositories;
using WafclastRPG.Database.Response;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.UserCommands
{
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class CombatCommands : BaseCommandModule
    {
        private IResponse _res;
        private readonly IPlayerRepository _playerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMongoSession _session;
        private readonly UsersBlocked _usersBlocked;

        public CombatCommands(IPlayerRepository playerRepository, IRoomRepository roomRepository, IMongoSession session, UsersBlocked usersBlocked)
        {
            this._playerRepository = playerRepository;
            this._roomRepository = roomRepository;
            this._session = session;
            this._usersBlocked = usersBlocked;
        }

        [Command("explorar")]
        [Aliases("ex", "explore")]
        [Description("Permite explorar uma região, podendo encontrar monstros.")]
        [Usage("explorar")]
        public async Task ExploreCommandAsync(CommandContext ctx)
        {
            this._usersBlocked.BlockUser(ctx);
            Interactivity intera = new Interactivity(this._usersBlocked, ctx);
            var player = await this._playerRepository.FindPlayerAsync(ctx);
            var room = await this._roomRepository.FindRoomOrDefaultAsync(player);
            var monster = room.Monster;
            if (monster == null)
                throw new Exception("para que essa area não tem nada de interessante para explorar.");



            var str = new StringBuilder();

            str.AppendLine($"Você encontrou **[{room.Monster.Mention}]!**");
            str.AppendLine("O que deseja fazer?");
            str.AppendLine("Ataque 1: Atacar com as mãos");
            str.AppendLine("Ataque 2: Magia Congelante");
            str.AppendLine("Ataque 3: Magia de Administrador");
            str.AppendLine("Defender");
            str.AppendLine("Fugir");
            var emb = new DiscordEmbedBuilder();
            emb.WithDescription(str.ToString());
            emb.WithColor(DiscordColor.Azure);
            await ctx.RespondAsync(emb);

            var character = player.Character;
            int turn = 1;
            bool cont = true;
            Random rd = new Random();
            while (cont)
            {
                var monsterDamage = rd.Next(1, 10);

                var message = await intera.WaitForMessageAsync();
                if (message.TimedOut)
                {
                    await ctx.RespondAsync("O monstro fugiu!");
                    return;
                }

                int? choose = message.Result switch
                {
                    "ataque 1" => 1,
                    "1" => 1,
                    "ataque 2" => 2,
                    "2" => 2,
                    "ataque 3" => 3,
                    "3" => 3,
                    "fugir" => 4,
                    _ => 0,
                };

                if (choose == 4)
                {
                    await ctx.RespondAsync("Você fugiu da batalha igual um patinho feio 🦆🦆🦆!");
                    return;
                }

                var playerDamage = 0;

                _ = choose switch
                {
                    1 => playerDamage = rd.Next(1, 10),
                    2 => playerDamage = rd.Next(20, 30),
                    3 => playerDamage = 100000,
                };

                var embed = new DiscordEmbedBuilder();

                embed.WithColor(DiscordColor.Red);
                embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}]", iconUrl: ctx.User.AvatarUrl);
                embed.WithTitle("Relatório de Combate");
                if ((player.Character.LifePoints.Current -= monsterDamage) <= 0)
                    cont = false;
                if ((monster.LifePoints.Current -= playerDamage) <= 0)
                    cont = false;

                var strf = new StringBuilder();
                strf.AppendLine($"Monster causou {monsterDamage} de dano!");
                strf.AppendLine($"Jogador causou {playerDamage} de dano!");
                embed.WithDescription(strf.ToString());
                embed.AddField(ctx.User.Username, $"{Emojis.TesteEmoji(player.Character.LifePoints)} {character.LifePoints}", true);
                embed.AddField(monster.Mention, $"{Emojis.GerarVidaEmoji(monster.LifePoints)} {monster.LifePoints.Current:N2} ", true);

                if (cont)
                {
                    str = new StringBuilder();
                    str.AppendLine("Ataque 1: Atacar com as mãos");
                    str.AppendLine("Ataque 2: Magia Congelante");
                    str.AppendLine("Ataque 3: Magia de Administrador");
                    str.AppendLine("Defender");
                    str.AppendLine("Fugir");

                    embed.AddField("O que deseja fazer?", str.ToString());
                }
                await ctx.RespondAsync(embed);
            }

            this._usersBlocked.UnblockUser(ctx);
            return;


            //using (await this._session.StartSessionAsync())
            //    this._res = await this._session.WithTransactionAsync(async (s, ct) =>
            //    {
            //        var player = await this._playerRepository.FindPlayerAsync(ctx);

            //        //Combat
            //        var combatResult = player.BasicAttackMonster();
            //        if (combatResult == "você não está visualizando nenhum monstro para atacar!")
            //            return new StringResponse(combatResult);

            //        await this._playerRepository.SavePlayerAsync(player);

            //        var embed = new DiscordEmbedBuilder();
            //        embed.AddField(ctx.User.Username, $"{Emojis.TesteEmoji(player.Character.LifePoints)} {character.LifePoints}", true);

            //        embed.WithColor(DiscordColor.Red);
            //        embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}]", iconUrl: ctx.User.AvatarUrl);
            //        embed.WithTitle("Relatório de Combate");
            //        embed.WithDescription(combatResult.ToString());

            //        var monster = player.Character.Room.Monster;

            //        embed.AddField(monster.Mention, $"{Emojis.GerarVidaEmoji(monster.LifePoints)} {monster.LifePoints.Current:N2} ", true);

            //        //loot em outro comando!

            //        return new EmbedResponse(embed);
            //    });
            //await ctx.RespondAsync(this._res);
        }






        public async Task BasicAttackCommandAsync(CommandContext ctx)
        {
            //using (await this._session.StartSessionAsync())
            //    this._res = await this._session.WithTransactionAsync(async (s, ct) =>
            //    {
            //        var player = await this._playerRepository.FindPlayerAsync(ctx);

            //        //Combat
            //        var combatResult = player.BasicAttackMonster();
            //        if (combatResult == "você não está visualizando nenhum monstro para atacar!")
            //            return new StringResponse(combatResult);

            //        await this._playerRepository.SavePlayerAsync(player);

            //        var embed = new DiscordEmbedBuilder();
            //        //embed.AddField(ctx.User.Username, $"{Emojis.TesteEmoji(player.Character.LifePoints)} {character.LifePoints}", true);

            //        embed.WithColor(DiscordColor.Red);
            //        embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}]", iconUrl: ctx.User.AvatarUrl);
            //        embed.WithTitle("Relatório de Combate");
            //        embed.WithDescription(combatResult.ToString());

            //        var monster = player.Character.Room.Monster;

            //        embed.AddField(monster.Mention, $"{Emojis.GerarVidaEmoji(monster.LifePoints)} {monster.LifePoints.Current:N2} ", true);

            //        //loot em outro comando!

            //        return new EmbedResponse(embed);
            //    });
            //await ctx.RespondAsync(this._res);
        }

        //public async Task ExploreCommandAsync(CommandContext ctx) {
        //  using (var sessionHandler = await _session.StartSession())
        //    _res = await sessionHandler.WithTransactionAsync(async (s, ct) => {
        //      var player = await _playerRepository.FindPlayerAsync(ctx);

        //      var character = player.Character;
        //      character.Room = await _roomRepository.FindRoomOrDefaultAsync(character.Room.Id);

        //      if (character.Room.Monster == null) {
        //        return new Response("você procura monstros para atacar.. mas parece que não você não encontrará nada aqui.");
        //      }

        //      await _playerRepository.SavePlayerAsync(player);

        //      return new Response($"você encontrou **[{character.Room.Monster.Mention}]!**");
        //    });
        //  await ctx.RespondAsync(_res);
        //}
    }
}
