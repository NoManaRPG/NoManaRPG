using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Game.Atributos;
using WafclastRPG.Game.Services;

namespace WafclastRPG.Game.Comandos.Acao
{
    public class ComandoCriarPersonagem : BaseCommandModule
    {
        public Banco banco;

        [Command("criar-personagem")]
        [Aliases("cp")]
        [Description("Permite criar um personagem com uma das 7 classes disponíveis e com um nome personalizado.")]
        [ComoUsar("criar-personagem <classe> <nome do personagem>")]
        [Exemplo("criar-personagem caçadora Arqueiro Verde")]
        [Cooldown(1, 10, CooldownBucketType.User)]
        public async Task CriarPersonagemAsync(CommandContext ctx, string classe = "")
        {
            var jogadorExiste = await JogadorExisteAsync(ctx);
            if (jogadorExiste) return;

            if (string.IsNullOrWhiteSpace(classe))
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
                embed.WithColor(DiscordColor.Gold);
                embed.WithTitle("Classes disponíveis");
                embed.WithDescription($"Escolha uma classe digitando `!criar-personagem <classe> <nome do personagem>`.");
                embed.AddField("Caçadora", "Foco em: Destreza", true);
                embed.AddField("Berserker", "Foco em: Força", true);
                embed.AddField("Bruxa", "Foco em: Inteligência", true);
                embed.AddField("Duelista", "Foco em: Força e Destreza", true);
                embed.AddField("Templário", "Foco em: Força e Inteligência", true);
                embed.AddField("Sombra", "Foco em: Destreza e Inteligência", true);
                embed.AddField("Herdeira", "Foco em : Força, Destreza e Inteligência", true);
                embed.WithFooter("Se estiver perdido digite `!ajuda`.", "https://cdn.discordapp.com/attachments/736163626934861845/742671714386968576/help_animated_x4_1.gif");
                await ctx.RespondAsync(embed: embed.Build());
                return;
            }

            string classeFormatada = classe.RemoverAcentos().ToLower();

            switch (classeFormatada)
            {
                case "cacadora":
                    var per1 = new RPPersonagem("Caçadora", new RPAtributo(14, 32, 14), new RPDano(2, 5));
                    per1.Mochila.TryAddItem(new Metadata.Itens.Armas.DuasMaoArmas.Arcos().Arco1());
                    per1.Mochila.TryAddItem(new Metadata.Itens.Frascos.FrascosVida().Frasco1());
                    await CriarJogador(ctx, per1);
                    break;
                case "berserker":
                    var per2 = new RPPersonagem("Berserker", new RPAtributo(14, 14, 32), new RPDano(2, 8));
                    per2.Mochila.TryAddItem(new Metadata.Itens.Armas.UmaMaoArmas.MacasUmaMao().Maca1());
                    per2.Mochila.TryAddItem(new Metadata.Itens.Frascos.FrascosVida().Frasco1());
                    await CriarJogador(ctx, per2);
                    break;
                case "sombra":
                    var per6 = new RPPersonagem("Sombra", new RPAtributo(23, 23, 14), new RPDano(2, 5));
                    per6.Mochila.TryAddItem(new Metadata.Itens.Armas.UmaMaoArmas.Adagas().Adaga1());
                    per6.Mochila.TryAddItem(new Metadata.Itens.Frascos.FrascosVida().Frasco1());
                    await CriarJogador(ctx, per6);
                    break;
                case "duelista":
                    var per4 = new RPPersonagem("Duelista", new RPAtributo(14, 23, 23), new RPDano(2, 6));
                    per4.Mochila.TryAddItem(new Metadata.Itens.Armas.UmaMaoArmas.EspadasUmaMao().Espada1());
                    per4.Mochila.TryAddItem(new Metadata.Itens.Frascos.FrascosVida().Frasco1());
                    await CriarJogador(ctx, per4);
                    break;
                case "bruxa":
                    var per3 = new RPPersonagem("Bruxa", new RPAtributo(32, 14, 14), new RPDano(2, 5));
                    per3.Mochila.TryAddItem(new Metadata.Itens.Armas.UmaMaoArmas.Varinhas().Varinha1());
                    per3.Mochila.TryAddItem(new Metadata.Itens.Frascos.FrascosVida().Frasco1());
                    await CriarJogador(ctx, per3);
                    break;
                case "templario":
                    var per5 = new RPPersonagem("Templário", new RPAtributo(23, 14, 23), new RPDano(2, 6));
                    per5.Mochila.TryAddItem(new Metadata.Itens.Armas.UmaMaoArmas.Cetros().Cetro1());
                    per5.Mochila.TryAddItem(new Metadata.Itens.Frascos.FrascosVida().Frasco1());
                    await CriarJogador(ctx, per5);
                    break;
                case "herdeira":
                    var per7 = new RPPersonagem("Herdeira", new RPAtributo(20, 20, 20), new RPDano(2, 6));
                    per7.Mochila.TryAddItem(new Metadata.Itens.Armas.UmaMaoArmas.EspadasUmaMao().Espada1());
                    per7.Mochila.TryAddItem(new Metadata.Itens.Frascos.FrascosVida().Frasco1());
                    await CriarJogador(ctx, per7);
                    break;
                default:
                    await ctx.RespondAsync($"{ctx.User.Mention}, você informou uma classe que não existe!");
                    break;
            }
        }

        private async Task<bool> JogadorExisteAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            RPJogador jogador = await banco.GetJogadorAsync(ctx);
            if (jogador == null) return false;
            await ctx.RespondAsync($"{ctx.User.Mention}, você já criou um personagem e por isso não pode usar este comando novamente!");
            return true;
        }

        private async Task CriarJogador(CommandContext ctx, RPPersonagem personagem)
        {
            using (var session = await banco.Client.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = new RPJogador(ctx, personagem);
                await banco.AddJogadorAsync(jogador);
                await session.CommitTransactionAsync();

                await ctx.RespondAsync($"{ctx.User.Mention}, o seu personagem {jogador.Personagem.Classe.Titulo()} foi criado!");
            }
        }
    }
}
