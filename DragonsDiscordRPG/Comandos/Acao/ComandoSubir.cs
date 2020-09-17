using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos.Acao
{
    public class ComandoSubir : BaseCommandModule
    {
        [Command("subir")]
        public async Task ComandoSubirAsync(CommandContext ctx)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            int inimigos = 0;
            using (var session = await ModuloBanco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

                if (personagem.Zona.Monstros.Count != 0)
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, você precisa eliminar todos os montros para subir!");
                    return;
                }

                bool temMonstros = ModuloBanco.MonstrosNomes.ContainsKey(personagem.Zona.Nivel - 1);
                if (temMonstros)
                {

                    inimigos = personagem.Zona.TrocarZona(personagem.VelocidadeAtaque.Atual, personagem.Zona.Nivel - 1);

                    await banco.EditJogadorAsync(jogador);
                    await session.CommitTransactionAsync();
                    await ctx.RespondAsync($"{ctx.User.Mention}, apareceu {inimigos} monstro na sua frente!");
                }
                else if (personagem.Zona.Nivel - 1 == 0)
                {
                    foreach (var item in personagem.Pocoes)
                        item.AddCarga(double.MaxValue);
                    personagem.Vida.Adicionar(double.MaxValue);
                    personagem.Mana.Adicionar(double.MaxValue);
                    personagem.Efeitos = new List<RPEfeito>();
                    personagem.Zona.Nivel = 0;
                    personagem.Zona.ItensNoChao = new List<RPItem>();
                    await banco.EditJogadorAsync(jogador);
                    await session.CommitTransactionAsync();
                    await ctx.RespondAsync($"{ctx.User.Mention}, você saiu da torre!");
                }
                else
                    await ctx.RespondAsync($"{ctx.User.Mention}, você só pode subir para o céu morrendo!");
            }
        }
    }
}
