using DragonsDiscordRPG.Entidades;
using System.Collections.Generic;
using static DragonsDiscordRPG.Itens.Pocoes.PocoesVida;

namespace DragonsDiscordRPG.Itens.Pocoes
{
    public static class RPPocoes
    {
        public static List<RPItem> PocoesVida = new List<RPItem>();
        public static List<RPItem> PocoesMana = new List<RPItem>();

        public static void Carregar()
        {
            PocoesVida.Add(Frasco());
        }
    }
}
