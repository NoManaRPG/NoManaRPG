using System.Collections.Generic;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Armas.UmaMaoArmas
{
    public class Cetros
    {
        public List<RPCetro> CetrosAb()
        {
            var arma = new List<RPCetro>();
            arma.Add(Cetro1());
            arma.Add(Cetro2());
            arma.Add(Cetro3());
            return arma;
        }

        public RPCetro Cetro1()
        => new RPCetro(1, "Cetro de Madeira Balsa", RPClasse.UmaMaoArma, 3, new RPDano(5, 8), 0.06, 1.55, 8, 8);

        public RPCetro Cetro2()
        => new RPCetro(5, "Cetro de Lenhanegra", RPClasse.UmaMaoArma, 3, new RPDano(8, 12), 0.06, 1.5, 14, 14);

        public RPCetro Cetro3()
        => new RPCetro(10, "Cetro de Bronze", RPClasse.UmaMaoArma, 3, new RPDano(10, 19), 0.06, 1.4, 22, 22);

    }
}
