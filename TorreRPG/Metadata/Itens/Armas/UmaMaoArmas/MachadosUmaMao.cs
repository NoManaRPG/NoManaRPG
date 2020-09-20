using System.Collections.Generic;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Armas.UmaMaoArmas
{
    public class MachadosUmaMao
    {
        public List<RPMachadoUmaMao> MachadosUmaMaoAb()
        {
            var machados = new List<RPMachadoUmaMao>();
            machados.Add(Machado1());
            machados.Add(Machado2());
            machados.Add(Machado3());
            machados.Add(Machado4());
            machados.Add(Machado5());
            machados.Add(Machado6());
            machados.Add(Machado7());
            return machados;
        }

        public RPMachadoUmaMao Machado1()
        => new RPMachadoUmaMao(2, "Machadinha Enferrujada", RPClasse.UmaMaoArma, 6, new RPDano(6, 11), 0.05, 1.5, 12, 6);

        public RPMachadoUmaMao Machado2()
        => new RPMachadoUmaMao(6, "Machadinha de Jade", RPClasse.UmaMaoArma, 6, new RPDano(10, 15), 0.05, 1.45, 21, 10);

        public RPMachadoUmaMao Machado3()
        => new RPMachadoUmaMao(11, "Machado Revestido", RPClasse.UmaMaoArma, 6, new RPDano(11, 21), 0.05, 1.5, 28, 19);

        public RPMachadoUmaMao Machado4()
        => new RPMachadoUmaMao(16, "Cutelo", RPClasse.UmaMaoArma, 6, new RPDano(12, 35), 0.05, 1.30, 48, 14);

        public RPMachadoUmaMao Machado5()
        => new RPMachadoUmaMao(21, "Machado Largo", RPClasse.UmaMaoArma, 6, new RPDano(19, 34), 0.05, 1.35, 54, 25);

        public RPMachadoUmaMao Machado6()
        => new RPMachadoUmaMao(25, "Machado Armado", RPClasse.UmaMaoArma, 6, new RPDano(14, 42), 0.05, 1.4, 58, 33);

        public RPMachadoUmaMao Machado7()
        => new RPMachadoUmaMao(29, "Machado Decorativo", RPClasse.UmaMaoArma, 6, new RPDano(27, 50), 0.05, 1.2, 80, 23);
    }
}
