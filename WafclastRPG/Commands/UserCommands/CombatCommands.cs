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
            var interactivity = new Interactivity(this._usersBlocked, ctx, new TimeSpan(0,0,5));
            using (interactivity.BlockUser())
            {
                var player = await this._playerRepository.FindPlayerAsync(ctx);

                //var room = await this._roomRepository.FindRoomOrDefaultAsync(player);
                //if(room.Monster == null)
                //{
                //             this._usersBlocked.UnblockUser(ctx);
                //    await ctx.RespondAsync("Parece que aqui não tem monstros!");
                //    return;
                //}

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

                    await interactivity.WaitForMessageAsync();

                    // Player skill
                    var playerUseSkill = combat.PlayerUseSkill(basicSkill);
                    continueCombat = playerUseSkill.ContinueCombat;
                    if (continueCombat == false)
                        goto EndCombat;

                    // Monster skill
                    var monsterUseSkill = combat.MonsterUseSkill();

                    EndCombat:
                    await this.RespostaBasicAsync(ctx, player, monster, strSkills, playerUseSkill.CombatDescription, continueCombat);
                }
                return;
            }

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
            emb.WithDescription(strDescription.ToString());
            emb.AddField(ctx.User.Username, player.Character.LifePointsStatus, true);
            emb.AddField(player.Character.ResourcePointsName, player.Character.ResourcePointsStatus, true);
            emb.AddField(monster.NameLevel, $"{Emojis.DinamicHeartEmoji(monster.LifePoints)} {monster.LifePoints.Current:N2} ");
            if (showOptions)
            {
                emb.AddField("[Ataques disponíveis]", strSkills.ToString());
                emb.WithFooter("Você tem 2 minutos para responder.");
            }
            emb.WithColor(DiscordColor.Red);
            await ctx.RespondAsync(ctx.User.Mention, emb);
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
    }
}
