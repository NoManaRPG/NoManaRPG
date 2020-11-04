using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Game.Atributos;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Extensoes;
using WafclastRPG.Game.Services;

namespace WafclastRPG.Game.Comandos.Acao
{
    public class ComandoAtacar : BaseCommandModule
    {
        public Banco banco;

        [Command("atacar")]
        [Aliases("at")]
        [Description("Permite atacar um monstro.")]
        [ComoUsar("atacar [#ID]")]
        [ComoUsar("atacar")]
        [Exemplo("atacar #1")]
        [Cooldown(1, 10, CooldownBucketType.User)]
        public async Task ComandoAtacarAsync(CommandContext ctx)
        {
        }
            //    // Verifica se existe o jogador,
            //    var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            //    if (naoCriouPersonagem) return;

            //    // Inicia uma sessão do Mongo para não ter alteração duplicada.
            //    using (var session = await banco.StartSessionAsync())
            //    {
            //        BancoSession banco = new BancoSession(session);
            //        RPJogador jogador = await banco.GetJogadorAsync(ctx);
            //        RPPersonagem personagem = jogador.Personagem;


            //        StringBuilder resumoBatalha = new StringBuilder();
            //        bool monstroMorreu = false;

            //        // Executa os efeitos ativos no personagem.
            //        personagem.CalcEfeitos(resumoBatalha);

            //        // Executa os ataques do inimigo.
            //        personagem.Zona.CalcAtaquesInimigos(personagem, resumoBatalha);

            //        // Exibimos a vida/mana do personagem no começo da mensagem.
            //        DiscordEmbedBuilder embed = new DiscordEmbedBuilder();


            //        // Verifica se o personagem vai acertar o monstro
            //        bool chanceAcertoPersonagem = Calculo.DanoFisicoChanceAcerto(personagem.Precisao.Modificado, personagem.Zona.Monstros[indexAlvo].Evasao);
            //        if (chanceAcertoPersonagem)
            //        {
            //            // Randomizamos um dano médio com base no minimo e max da arma equipada.
            //            double danoPersonagem = personagem.DanoFisicoModificado.Sortear;
            //            personagem.Zona.Monstros[indexAlvo].Vida -= danoPersonagem;
            //            resumoBatalha.AppendLine($"\n{Emoji.Adaga} Você causou {danoPersonagem.Text()} de dano no {personagem.Zona.Monstros[indexAlvo].Nome}!");

            //            // Se o monstro morrer.
            //            if (personagem.Zona.Monstros[indexAlvo].Vida <= 0)
            //            {
            //                DiscordEmoji xp = DiscordEmoji.FromGuildEmote(ctx.Client, 758439721016885308);

            //                double expGanha = Calculo.CalcularEfetividadeXP(personagem.Nivel.Atual, personagem.Zona.Monstros[indexAlvo].Nivel) * personagem.Zona.Monstros[indexAlvo].Exp;
            //                resumoBatalha.AppendLine($"{Emoji.CrossBone} {personagem.Zona.Monstros[indexAlvo].Nome.Bold()} ️{Emoji.CrossBone}\n" +
            //                    $"{xp}+{expGanha.Text()}.");
            //                monstroMorreu = true;
            //                //Evento halloween
            //                int evoluiu = personagem.AddExp(expGanha);
            //                if (evoluiu != 0)
            //                    embed.AddField("Evolução".Titulo(), $"{Emoji.Up} Você evoluiu de nível!");

            //                // Recarrega os frascos
            //                foreach (var item in personagem.Frascos)
            //                    item.AddCarga(1);
            //                // Dropa itens
            //                personagem.Zona.SortearItem(personagem.Zona.Monstros[indexAlvo], personagem.ChanceDrop);
            //            }
            //        }
            //        else // Caso ele erre o ataque
            //            resumoBatalha.AppendLine($"\n{Emoji.Desapontado} Você errou o ataque!");

            //        // Informações complementares
            //        embed.WithDescription($"**Turno {personagem.Zona.Turno}.**\n" +
            //          $"**Inimigo restante {personagem.Zona.Monstros.Count} .**");

            //        embed.WithColor(DiscordColor.Blue);
            //        // Se o personagem morrer, reseta ele.
            //        bool jogadorMorreu = false;
            //        if (personagem.Vida.Atual <= 0)
            //        {
            //            personagem.Resetar();

            //            resumoBatalha.AppendLine($"{Emoji.CrossBone} Você morreu!!! {Emoji.CrossBone}".Bold());
            //            embed.WithColor(DiscordColor.Red);
            //            embed.WithImageUrl("https://cdn.discordapp.com/attachments/758139402219159562/769397084284649472/kisspng-headstone-drawing-royalty-free-clip-art-5afd7eb3d84cb3.625146071526562483886.png");
            //            jogadorMorreu = true;
            //        }

            //        // Exibe o resumo da batalha.. Regens e ataques.
            //        embed.AddField("Resumo da batalha".Titulo(), resumoBatalha.ToString());
            //        embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe} - {personagem.Nome}", iconUrl: ctx.User.AvatarUrl);
            //        embed.WithThumbnail("https://cdn.discordapp.com/attachments/758139402219159562/758425473541341214/sword-and-shield-icon-17.png");

            //        if (personagem.Zona.ItensNoChao.Count != 0)
            //            embed.AddField("Itens no chão".Titulo().Bold(), $":mag: {personagem.Zona.ItensNoChao.Count.Bold()} item. ");

            //        // Adiciona nova onda.
            //        if (monstroMorreu)
            //        {
            //            if (personagem.Zona.NovaOnda(personagem.VelocidadeAtaque.Modificado, out int inimigosNovos))
            //                embed.AddField("Nova onda".Titulo(), $"{Emoji.ExplacamaoDupla} Apareceu {inimigosNovos} monstros!");
            //        }
            //        if (!jogadorMorreu)
            //        {
            //            var porcentagemVida = personagem.Vida.Atual / personagem.Vida.Maximo;
            //            var porcenagemMana = personagem.Mana.Atual / personagem.Mana.Maximo;
            //            embed.AddField($"{"Vida atual".Underline()}", $"{ConverterVida(porcentagemVida)} {(porcentagemVida * 100).Text()}%", true);
            //            embed.AddField($"{"Mana atual".Underline()}", $"{ConverterMana(porcenagemMana)} {(porcenagemMana * 100).Text()}%", true);
            //        }

            //        // Salvamos.
            //        await banco.EditJogadorAsync(jogador);
            //        await session.CommitTransactionAsync();

            //        await ctx.RespondAsync(ctx.User.Mention, embed: embed.Build());
            //    }
            //}

            //public static string ConverterVida(double porcentagem)
            //{
            //    switch (porcentagem)
            //    {
            //        case double n when (n > 0.75):
            //            return Emoji.CoracaoVerde;
            //        case double n when (n > 0.50):
            //            return Emoji.CoracaoAmarelo;
            //        case double n when (n > 0.25):
            //            return Emoji.CoracaoLaranja;
            //    }
            //    return Emoji.CoracaoVermelho;
            //}

            //public static string ConverterMana(double porcentagem)
            //{
            //    switch (porcentagem)
            //    {
            //        case double n when (n > 0.75):
            //            return Emoji.CirculoVerde;
            //        case double n when (n > 0.50):
            //            return Emoji.CirculoAmarelo;
            //        case double n when (n > 0.25):
            //            return Emoji.CirculoLaranja;
            //    }
            //    return Emoji.CirculoVermelho;
            //}
        }
}
