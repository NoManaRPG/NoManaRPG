using System.Collections.Generic;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Armas.UmaMaoArmas
{
    public class Varinhas
    {
        public List<RPVarinha> VarinhasAb()
        {
            var varinha = new List<RPVarinha>();
            varinha.Add(Varinha1());
            varinha.Add(Varinha2());
            varinha.Add(Varinha3());
            return varinha;
        }

        public RPVarinha Varinha1()
        => new RPVarinha(1, "Varinha de Madeira Balsa", RPClasse.UmaMaoArma, 3, new RPDano(5, 9), 0.07, 1.4, 14);

        public RPVarinha Varinha2()
        => new RPVarinha(6, "Chifre de Cabra", RPClasse.UmaMaoArma, 3, new RPDano(9, 16), 0.07, 1.2, 29);

        public RPVarinha Varinha3()
        => new RPVarinha(12, "Varinha Gravada", RPClasse.UmaMaoArma, 3, new RPDano(9, 17), 0.07, 1.5, 47);
    }
}
