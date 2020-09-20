using System.Collections.Generic;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Armas.UmaMaoArmas
{
    public class MacasUmaMao
    {
        public List<RPMacaUmaMao> MacasUmaMaoAb()
        {
            var maca = new List<RPMacaUmaMao>();
            maca.Add(Maca1());
            maca.Add(Maca2());
            maca.Add(Maca3());
            return maca;
        }

        public RPMacaUmaMao Maca1()
        => new RPMacaUmaMao(1, "Clava de Madeira Balsa", RPClasse.UmaMaoArma, 6, new RPDano(6, 8), 0.05, 1.45, 14);

        public RPMacaUmaMao Maca2()
        => new RPMacaUmaMao(5, "Clava Tribal", RPClasse.UmaMaoArma, 6, new RPDano(8, 13), 0.05, 1.4, 26);

        public RPMacaUmaMao Maca3()
        => new RPMacaUmaMao(10, "Clava Cravada", RPClasse.UmaMaoArma, 6, new RPDano(13, 16), 0.05, 1.45, 41);
    }
}
