using System.Collections.Generic;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Armas.DuasMaoArmas
{
    public class Arcos
    {
        public List<RPArmaArco> ArcosAb()
        {
            var arcos = new List<RPArmaArco>();
            arcos.Add(Arco1());
            arcos.Add(Arco2());
            arcos.Add(Arco3());
            arcos.Add(Arco4());
            arcos.Add(Arco5());
            arcos.Add(Arco6());
            return arcos;
        }

        public RPArmaArco Arco1()
        => new RPArmaArco(1, "Arco Bruto", RPClasse.DuasMao, 6, new RPDano(5, 13), 0.05, 1.4, 14);

        public RPArmaArco Arco2()
        => new RPArmaArco(5, "Arco Curto", RPClasse.DuasMao, 6, new RPDano(6, 16), 0.05, 1.5, 26);

        public RPArmaArco Arco3()
        => new RPArmaArco(9, "Arco Longo", RPClasse.DuasMao, 6, new RPDano(6, 25), 0.06, 1.3, 38);

        public RPArmaArco Arco4()
        => new RPArmaArco(14, "Arco Composto", RPClasse.DuasMao, 6, new RPDano(12, 26), 0.06, 1.3, 53);

        public RPArmaArco Arco5()
        => new RPArmaArco(18, "Arco Recurvo", RPClasse.DuasMao, 6, new RPDano(11, 34), 0.067, 1.25, 65);

        public RPArmaArco Arco6()
        => new RPArmaArco(23, "Arco Ósseo", RPClasse.DuasMao, 6, new RPDano(11, 34), 0.065, 1.4, 80);
    }
}
