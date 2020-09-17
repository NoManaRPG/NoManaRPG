using System;
using System.Collections.Generic;
using System.Text;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Armas.DuasMaoArmas.Arcos
{
    public class Arcos
    {
        public List<RPArco> ArcosAb()
        {
            var arcos = new List<RPArco>();
            arcos.Add(Arco1());
            arcos.Add(Arco2());
            arcos.Add(Arco3());
            arcos.Add(Arco4());
            arcos.Add(Arco5());
            arcos.Add(Arco6());
            return arcos;
        }

        public RPArco Arco1()
        => new RPArco(1, "Arco Bruto", RPClasse.Arcos, 6, new RPDano(5, 13), 0.05, 1.4, 14);

        public RPArco Arco2()
        => new RPArco(5, "Arco Curto", RPClasse.Arcos, 6, new RPDano(6, 16), 0.05, 1.5, 26);

        public RPArco Arco3()
        => new RPArco(9, "Arco Longo", RPClasse.Arcos, 6, new RPDano(6, 25), 0.06, 1.3, 38);

        public RPArco Arco4()
        => new RPArco(14, "Arco Composto", RPClasse.Arcos, 6, new RPDano(12, 26), 0.06, 1.3, 53);

        public RPArco Arco5()
        => new RPArco(18, "Arco Recurvo", RPClasse.Arcos, 6, new RPDano(11, 34), 0.067, 1.25, 65);

        public RPArco Arco6()
        => new RPArco(23, "Arco Ósseo", RPClasse.Arcos, 6, new RPDano(11, 34), 0.065, 1.4, 80);
    }
}
