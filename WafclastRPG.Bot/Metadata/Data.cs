﻿using WafclastRPG.Bot;

namespace WafclastRPG.Game.Metadata
{
    public sealed class Data
    {
        private readonly Banco banco;

        public Data(Banco banco)
        {
            this.banco = banco;
         //   CarregarComidas();
        }

        //private void CarregarComidas()
        //{
        //    var itens = typeof(Comidas).GetMethods();
        //    for (int i = 0; i < itens.Length - 4; i++)
        //    {
        //        var reg = (WafclastItemComida)itens[i].Invoke(null, null);
        //        banco.ReplaceItemAsync(reg);
        //    }
        //}
    }
}
