using System.Collections.Generic;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Armas.UmaMaoArmas
{
    public class MacasUmaMao
    {
        public List<RPArmaMacaUmaMao> MacasUmaMaoAb()
        {
            var maca = new List<RPArmaMacaUmaMao>();
            maca.Add(Maca1());
            maca.Add(Maca2());
            maca.Add(Maca3());
            return maca;
        }

        public RPArmaMacaUmaMao Maca1()
        => new RPArmaMacaUmaMao(1, "Clava de Madeira Balsa", RPClasse.UmaMao, 6, new RPDano(6, 8), 0.05, 1.45, 14);

        public RPArmaMacaUmaMao Maca2()
        => new RPArmaMacaUmaMao(5, "Clava Tribal", RPClasse.UmaMao, 6, new RPDano(8, 13), 0.05, 1.4, 26);

        public RPArmaMacaUmaMao Maca3()
        => new RPArmaMacaUmaMao(10, "Clava Cravada", RPClasse.UmaMao, 6, new RPDano(13, 16), 0.05, 1.45, 41);
    }
}
