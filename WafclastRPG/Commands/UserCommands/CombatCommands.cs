// This file is part of the WafclastRPG project.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using WafclastRPG.Attributes;
using WafclastRPG.Database;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Database.Response;
using WafclastRPG.Extensions;
using WafclastRPG.Game.Entities;
using WafclastRPG.Game.Entities.Monsters;
using WafclastRPG.Game.Entities.Skills;
using WafclastRPG.Game.Entities.Skills.Effects;

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
            var player = await this._playerRepository.FindPlayerAsync(ctx);

            //var room = await this._roomRepository.FindRoomOrDefaultAsync(player);
            //if(room.Monster == null)
            //{
            //    await ctx.RespondAsync("Parece que aqui não tem monstros!");
            //    return;
            //}

            var intera = new Interactivity(this._usersBlocked, ctx, new TimeSpan(0,2,0));
            var character = player.Character;

            // Auto Gerado!
            var monster = new WafclastMonster(1,"Boneco de teste",7,7,7,7,7,7,7,7);
            var basicSkill = new WafclastPlayerSkill("Ataque cortante", ctx.User.Id);
            var damageEffect = new  WafclastSkillDamageEffect(50);
            basicSkill.Effects.Add(damageEffect);
            // Auto Gerado!

            var combat = new WafclastCombat(player,monster);
            var strSkills = new StringBuilder();
            var strDescription = new StringBuilder();

            strSkills.AppendLine("Ataque cortante");
            strSkills.AppendLine("Usar ..");
            strDescription.AppendLine("Você encontrou um monstro!");

            await this.RespostaBasicAsync(ctx, player, monster, strSkills, strDescription);

            bool continueCombat = true;
            while (continueCombat)
            {
                //Input

                await intera.WaitForMessageAsync();

                // Player skill
                var playerUseSkill = combat.PlayerUseSkill(basicSkill);
                continueCombat = playerUseSkill.ContinueCombat;
                if (continueCombat == false)
                    goto EndCombat;

                // Monster skill

                EndCombat:
                await this.RespostaBasicAsync(ctx, player, monster, strSkills, playerUseSkill.CombatDescription, continueCombat);
            }
            this._usersBlocked.UnblockUser(ctx);
            return;


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
                embed.AddField(ctx.User.Username, $"{Emojis.DinamicHeartEmoji(player.Character.LifePoints)} {character.LifePoints}", true);
                embed.AddField(monster.Mention, $"{Emojis.GerarVidaEmoji(monster.LifePoints)} {monster.LifePoints.Current:N2} ", true);

                if (cont)
                {
                    //str = new StringBuilder();
                    //str.AppendLine("Ataque 1: Atacar com as mãos");
                    //str.AppendLine("Ataque 2: Magia Congelante");
                    //str.AppendLine("Ataque 3: Magia de Administrador");
                    //str.AppendLine("Defender");
                    //str.AppendLine("Fugir");

                    //embed.AddField("O que deseja fazer?", str.ToString());
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


        public async Task RespostaBasicAsync(CommandContext ctx, WafclastPlayer player, WafclastMonster monster, StringBuilder strSkills, StringBuilder strDescription, bool showOptions = true)
        {
            var emb = new DiscordEmbedBuilder();
            emb.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}]", iconUrl: ctx.User.AvatarUrl);
            emb.WithDescription(strDescription.ToString());
            emb.AddField(ctx.User.Username, player.Character.Life, true);
            var mana = player.Character.Energia;
            emb.AddField(mana.Nome, mana.Info, true);
            emb.AddField(monster.Mention, $"{Emojis.DinamicHeartEmoji(monster.LifePoints)} {monster.LifePoints.Current:N2} ");
            if (showOptions)
            {
                emb.AddField("[Ataques/Ações disponíveis]", strSkills.ToString());
                emb.WithFooter("Você tem 2 minutos para responder.");
            }
            emb.WithColor(DiscordColor.Red);
            await ctx.RespondAsync(emb);
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
