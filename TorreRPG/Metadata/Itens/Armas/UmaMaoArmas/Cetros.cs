using System.Collections.Generic;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Armas.UmaMaoArmas
{
    public class Cetros
    {
        public List<RPArmaCetro> CetrosAb()
        {
            var arma = new List<RPArmaCetro>();
            arma.Add(Cetro1());
            arma.Add(Cetro2());
            arma.Add(Cetro3());
            return arma;
        }

        public RPArmaCetro Cetro1()
        => new RPArmaCetro(1, "Cetro de Madeira Balsa", RPClasse.UmaMao, 3, new RPDano(5, 8), 0.06, 1.55, 8, 8);

        public RPArmaCetro Cetro2()
        => new RPArmaCetro(5, "Cetro de Lenhanegra", RPClasse.UmaMao, 3, new RPDano(8, 12), 0.06, 1.5, 14, 14);

        public RPArmaCetro Cetro3()
        => new RPArmaCetro(10, "Cetro de Bronze", RPClasse.UmaMao, 3, new RPDano(10, 19), 0.06, 1.4, 22, 22);

    }
}
