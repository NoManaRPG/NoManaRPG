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
            maca.Add(MacaUmaMao1());
            maca.Add(MacaUmaMao2());
            maca.Add(MacaUmaMao3());
            return maca;
        }

        public RPMacaUmaMao MacaUmaMao1()
        => new RPMacaUmaMao(1, "Clava de Madeira Balsa", RPClasse.UmaMaoArma, 6, new RPDano(6, 8), 0.05, 1.45, 14);

        public RPMacaUmaMao MacaUmaMao2()
        => new RPMacaUmaMao(5, "Clava Tribal", RPClasse.UmaMaoArma, 6, new RPDano(8, 13), 0.05, 1.4, 26);

        public RPMacaUmaMao MacaUmaMao3()
        => new RPMacaUmaMao(10, "Clava Cravada", RPClasse.UmaMaoArma, 6, new RPDano(13, 16), 0.05, 1.45, 41);
    }
}
