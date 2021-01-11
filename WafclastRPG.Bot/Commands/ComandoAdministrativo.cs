using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Collections.Concurrent;
using System.Threading;
using WafclastRPG.Game.Entidades;
using System.Collections.Generic;
using System.Linq;
using WafclastRPG.Bot.Extensions;
using System.Text;
using DSharpPlus.Interactivity.Extensions;
using System.Security.Cryptography;

namespace WafclastRPG.Bot.Comandos
{
    public class ComandoAdministrativo : BaseCommandModule
    {
        public Banco banco;
        public Config config;

        // This command only work on Wafclast Guild.
        [Command("ban")]
        public async Task BanAsync(CommandContext ctx, DiscordMember membro = null, [RemainingText] string razao = "")
        {
            if (ctx.Guild.Id == 732102804654522470)
            {
                // Verify if the member has "Moderator" role.
                ulong modRole = 775094267831255110;
                var isMod = ctx.Member.Roles.FirstOrDefault(x => x.Id == modRole);
                if (isMod != null)
                {
                    if (membro == null)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, você precisa informar um membro válido!");
                        return;
                    }

                    isMod = membro.Roles.FirstOrDefault(x => x.Id == modRole);
                    if (isMod == null)
                    {
                        DiscordEmbedBuilder embed = new DiscordEmbedBuilder().Inicializar(ctx);
                        embed.WithTitle("Ban");
                        embed.WithTimestamp(DateTime.Now);
                        if (string.IsNullOrWhiteSpace(razao))
                        {
                            embed.WithDescription($"{membro.Mention}, foi banido sem nenhuma razão especifica.");
                        }
                        else
                            embed.WithDescription($"{membro.Mention}, foi banido.\n{razao}");
                        // Role banido.
                        DiscordRole banido = ctx.Guild.GetRole(775098106823704617);
                        await membro.GrantRoleAsync(banido);
                        var channel = ctx.Guild.GetChannel(775099480928026656);
                        await channel.SendMessageAsync(embed: embed.Build());
                    }
                    else return;
                }
                else return;
            }
        }

        [Command("purge")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task PurgeAsync(CommandContext ctx, int quantidade)
            => await ctx.Channel.DeleteMessagesAsync(await ctx.Channel.GetMessagesAsync(quantidade + 1));

        [Command("emojis")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task ASD(CommandContext ctx)
        {
            var str = new StringBuilder();
            foreach (var item in ctx.Guild.Emojis)
                str.AppendLine($"{item.Key} - {item.Value}");
            await ctx.RespondAsync(str.ToString());
        }

        [Command("dm")]
        [RequireOwner]
        public async Task DmAsync(CommandContext ctx, DiscordGuild server, DiscordUser user, [RemainingText] string mensagem)
        {
            var serv = await ctx.Client.GetGuildAsync(server.Id);
            var sv = await serv.GetMemberAsync(user.Id);
            var dm = await sv.CreateDmChannelAsync();
            await dm.SendMessageAsync(mensagem);
        }


        [Command("mensagem")]
        [RequireOwner]
        public async Task MensagemAsync(CommandContext ctx, [RemainingText] string mensagem)
        {
            var embed = new DiscordEmbedBuilder();
            embed.WithDescription(mensagem);
            embed.WithImageUrl("https://cdn.discordapp.com/attachments/758139402219159562/773918198524936252/Wafclast.png");
            await ctx.RespondAsync(embed: embed.Build());
        }

        [Command("sudo")]
        [RequireOwner]
        public async Task Sudo(CommandContext ctx, DiscordUser member, [RemainingText] string command)
        {
            await ctx.TriggerTypingAsync();
            var cmd = ctx.CommandsNext.FindCommand(command, out var args);
            if (cmd == null)
            {
                await ctx.RespondAsync("Comando não encontrado");
                return;
            }

            var cfx = ctx.CommandsNext.CreateFakeContext(member, ctx.Channel, "", config.PrefixRelease, cmd, args);
            await ctx.CommandsNext.ExecuteCommandAsync(cfx);
        }

        [Command("sudo")]
        [RequireOwner]
        public async Task Sudo(CommandContext ctx, ulong id, [RemainingText] string command)
        {
            await ctx.TriggerTypingAsync();
            var cmd = ctx.CommandsNext.FindCommand(command, out var args);
            if (cmd == null)
            {
                await ctx.RespondAsync("Comando não encontrado");
                return;
            }
            var member = await ctx.Client.GetUserAsync(id);
            var cfx = ctx.CommandsNext.CreateFakeContext(member, ctx.Channel, "", config.PrefixRelease, cmd, args);
            await ctx.CommandsNext.ExecuteCommandAsync(cfx);
        }

        [Command("deletar-user")]
        [RequireOwner]
        public async Task DeletarUsuarioAsync(CommandContext ctx, [RemainingText] DiscordUser user = null)
        {
            await ctx.TriggerTypingAsync();
            if (user == null) user = ctx.User;
            await banco.Jogadores.DeleteOneAsync(x => x.Id == user.Id);
            await ctx.RespondAsync($"{user} deletado do banco de dados!");
        }

        public static ConcurrentDictionary<ulong, SemaphoreSlim> Buckets = new ConcurrentDictionary<ulong, SemaphoreSlim>();

        [Command("testetravar")]
        [RequireOwner]
        public async Task ff(CommandContext ctx)
        {
            if (!Buckets.TryGetValue(ctx.User.Id, out var bucket))
            {
                bucket = new SemaphoreSlim(1, 1);
                Buckets.AddOrUpdate(ctx.User.Id, bucket, (k, v) => bucket);
            }

            await bucket.WaitAsync();
            await ctx.RespondAsync($"{ctx.User.Username} aguardando 5 segundos");
            await Task.Delay(TimeSpan.FromSeconds(5));
            bucket.Release();
        }


        [Command("kill")]
        [RequireOwner]
        public async Task Kill(CommandContext ctx)
        {
            var jogador = await banco.GetJogadorAsync(ctx);
            var per = jogador.Personagem;

            per.InimigoMonstro.Vida = 1;
            await jogador.Salvar();
            await ctx.RespondAsync("inimigo está agora com 1 de vida!");
        }

        [Command("atualizar")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task Atualizar(CommandContext ctx)
        {
            FilterDefinition<WafclastJogador> filter = FilterDefinition<WafclastJogador>.Empty;
            FindOptions<WafclastJogador> options = new FindOptions<WafclastJogador>
            {
                BatchSize = 8,
                NoCursorTimeout = false
            };

            using (IAsyncCursor<WafclastJogador> cursor = await banco.Jogadores.FindAsync(filter, options))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<WafclastJogador> usuarios = cursor.Current;

                    foreach (WafclastJogador user in usuarios)
                    {
                        if (user.Personagem.Equipamentos == null)
                            user.Personagem.Equipamentos = new Dictionary<Game.Enums.EquipamentoType, Game.Entidades.Itens.WafclastItem>();
                        await banco.Jogadores.ReplaceOneAsync(x => x.Id == user.Id, user);
                    }
                }
            }
            await ctx.RespondAsync("Banco foi atualizado com sucesso!");
        }

        [Command("code")]
        [RequireOwner]
        public async Task WaitForCode(CommandContext ctx, DiscordChannel channel)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var codebytes = new byte[8];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(codebytes);

            var code = BitConverter.ToString(codebytes).ToLower();

            await channel.SendMessageAsync($"O primeiro que digitar o seguinte código ganha uma recompensa: `{code}`");

            var msg = await interactivity.WaitForMessageAsync(xm => xm.Content.Contains(code) && xm.ChannelId == channel.Id, TimeSpan.FromSeconds(60));
            if (!msg.TimedOut)
            {
                await channel.SendMessageAsync($"O ganhador é: {msg.Result.Author.Mention}");
                var jogador = await banco.Jogadores.Find(x => x.Id == msg.Result.Author.Id).FirstOrDefaultAsync();
                if (jogador == null)
                {
                    await ctx.RespondAsync("Mas parece que ele ainda não criou um personagem!");
                    return;
                }

                await banco.Jogadores.ReplaceOneAsync(x => x.Id == msg.Result.Author.Id, jogador);
            }
            else
                await channel.SendMessageAsync("Ninguém? Serio?!?");
        }

        private static ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
                if (codec.FormatID == format.Guid)
                    return codec;
            return null;
        }

        //[Command("teste")]
        //[RequireOwner]
        //public async Task testeCalc(CommandContext ctx, [RemainingText] string text)
        //{
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        ImageCodecInfo jpgEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);
        //        Encoder myEncoder = Encoder.Quality;
        //        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 70L);
        //        EncoderParameters myEncoderParameters = new EncoderParameters(1);

        //        myEncoderParameters.Param[0] = myEncoderParameter;

        //        PointF firstLocation = new PointF(90f, 289f);

        //        string imageFilePath = @"C:\Users\Talion\Desktop\template.png";
        //        Bitmap bitmap = (Bitmap)Image.FromFile(imageFilePath);//load the image file

        //        using (Graphics graphics = Graphics.FromImage(bitmap))
        //        {
        //            graphics.SmoothingMode = SmoothingMode.AntiAlias;
        //            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        //            using (Font arialFont = new Font("Arial", 89, FontStyle.Italic | FontStyle.Bold))
        //            {
        //                graphics.DrawString(text, arialFont, Brushes.White, firstLocation);
        //            }
        //        }
        //        bitmap.Save(@"C:\Users\Talion\Desktop\teste.png");

        //        new Bitmap(bitmap).Save(memoryStream, jpgEncoder, myEncoderParameters); //save the image file
        //        memoryStream.Position = 0;
        //        await ctx.RespondWithFileAsync("Perfil.jpeg", memoryStream);
        //    }

        //var numeroRandom = Calculo.SortearValor(0, 100.0);
        //switch (numeroRandom)
        //{
        //    case double n when (n > 0.9):
        //        await ctx.RespondAsync($"N maior que 0.9 {numeroRandom}");
        //        break;
        //    case double n when (n > 0.8):
        //        await ctx.RespondAsync($"N maior que 0.8 {numeroRandom}");
        //        break;
        //    case double n when (n > 0.7):
        //        await ctx.RespondAsync($"N maior que 0.7 {numeroRandom}");
        //        break;
        //    case double n when (n > 0.6):
        //        await ctx.RespondAsync($"N maior que 0.6 {numeroRandom}");
        //        break;
        //    case double n when (n > 0.5):
        //        await ctx.RespondAsync($"N maior que 0.5 {numeroRandom}");
        //        break;
        //    case double n when (n > 0.4):
        //        await ctx.RespondAsync($"N maior que 0.4 {numeroRandom}");
        //        break;
        //    case double n when (n > 0.3):
        //        await ctx.RespondAsync($"N maior que 0.3 {numeroRandom}");
        //        break;
        //    case double n when (n > 0.2):
        //        await ctx.RespondAsync($"N maior que 0.2 {numeroRandom}");
        //        break;
        //}

        //var frutas = new List<Fruta> {
        //new Fruta { Cor = 1, Nome = "Morango" },
        //    new Fruta { Cor = 1, Nome = "Framboesa" },
        //    new Fruta { Cor = 1, Nome = "Cereja" },
        //    new Fruta { Cor = 2, Nome = "Limão" },
        //    new Fruta { Cor = 2, Nome = "Melancia" }        };

        //var grupos = frutas.GroupBy(f => f.Cor);

        //Console.WriteLine("Imprimindo grupos");
        //Console.WriteLine("-----------------------");
        //foreach (var nomeGrupo in grupos.Select(g => g.Key))
        //{
        //    Console.WriteLine(nomeGrupo);
        //}

        //Console.WriteLine("-----------------------");
        //var pesquisa = grupos.Where(x => x.Key <= 2).ToList();
        //foreach (var listaDeFrutasPorCor in pesquisa)
        //{
        //    Console.WriteLine("Imprimindo frutas da cor: " + listaDeFrutasPorCor.Key);
        //    Console.WriteLine("-----------------------");
        //    foreach (var fruta in listaDeFrutasPorCor)
        //    {
        //        Console.WriteLine(fruta.Nome);
        //    }
        //    Console.WriteLine("-----------------------");
        //}
        //}
    }
}
