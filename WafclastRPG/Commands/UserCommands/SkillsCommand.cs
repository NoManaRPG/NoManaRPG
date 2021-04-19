using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class SkillsCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("habilidades")]
        [Aliases("skills")]
        [Description("Exibe suas habilidades, níveis e experiencia atual/máxima.")]
        [Usage("habilidades")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task UseCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    var embed = new DiscordEmbedBuilder();
                    embed.WithColor(DiscordColor.Brown);

                    var mine = player.Character.MineSkill;
                    var cook = player.Character.CookingSkill;

                    embed.AddField(":pick: Mineração", $"Nível {mine.Level} ({mine.CurrentExperience:N2} / {mine.ExperienceForNextLevel:N2})");
                    embed.AddField(":cook: Culinária", $"Nível {cook.Level} ({cook.CurrentExperience:N2} / {cook.ExperienceForNextLevel:N2})");

                    return new Response(embed);
                });

            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                await ctx.ResponderAsync(response.Message);
                return;
            }

            await ctx.ResponderAsync(response.Embed.Build());
        }
    }
}
