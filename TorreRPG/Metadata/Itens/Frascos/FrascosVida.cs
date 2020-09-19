using System;
using System.Collections.Generic;
using System.Text;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Frascos
{
    public class FrascosVida
    {
        public List<RPFrascoVida> FrascosVidaAb()
        {
            var frask = new List<RPFrascoVida>();
            frask.Add(Frasco1());
            frask.Add(Frasco2());
            frask.Add(Frasco3());
            frask.Add(Frasco4());
            frask.Add(Frasco5());
            return frask;
        }

        public RPFrascoVida Frasco1()
        => new RPFrascoVida(1, "Frasco de Vida Pequeno", RPClasse.Frasco, 2, 70, 6, 7, 21);
        public RPFrascoVida Frasco2()
        => new RPFrascoVida(3, "Frasco de Vida Médio", RPClasse.Frasco, 2, 150, 6.5, 8, 28);
        public RPFrascoVida Frasco3()
        => new RPFrascoVida(6, "Frasco de Vida Grande", RPClasse.Frasco, 2, 250, 7, 9, 30);
        public RPFrascoVida Frasco4()
        => new RPFrascoVida(12, "Frasco de Vida Maior", RPClasse.Frasco, 2, 360, 7, 10, 32);
        public RPFrascoVida Frasco5()
        => new RPFrascoVida(18, "Frasco de Vida Grandioso", RPClasse.Frasco, 2, 640, 6, 10, 25);

    }
}
