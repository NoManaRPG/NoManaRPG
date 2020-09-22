using System.Collections.Generic;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Armas.UmaMaoArmas
{
    public class MachadosUmaMao
    {
        public List<RPArmaMachadoUmaMao> MachadosUmaMaoAb()
        {
            var machados = new List<RPArmaMachadoUmaMao>();
            machados.Add(Machado1());
            machados.Add(Machado2());
            machados.Add(Machado3());
            machados.Add(Machado4());
            machados.Add(Machado5());
            machados.Add(Machado6());
            machados.Add(Machado7());
            return machados;
        }

        public RPArmaMachadoUmaMao Machado1()
        => new RPArmaMachadoUmaMao(2, "Machadinha Enferrujada", RPClasse.UmaMao, 6, new RPDano(6, 11), 0.05, 1.5, 12, 6);

        public RPArmaMachadoUmaMao Machado2()
        => new RPArmaMachadoUmaMao(6, "Machadinha de Jade", RPClasse.UmaMao, 6, new RPDano(10, 15), 0.05, 1.45, 21, 10);

        public RPArmaMachadoUmaMao Machado3()
        => new RPArmaMachadoUmaMao(11, "Machado Revestido", RPClasse.UmaMao, 6, new RPDano(11, 21), 0.05, 1.5, 28, 19);

        public RPArmaMachadoUmaMao Machado4()
        => new RPArmaMachadoUmaMao(16, "Cutelo", RPClasse.UmaMao, 6, new RPDano(12, 35), 0.05, 1.30, 48, 14);

        public RPArmaMachadoUmaMao Machado5()
        => new RPArmaMachadoUmaMao(21, "Machado Largo", RPClasse.UmaMao, 6, new RPDano(19, 34), 0.05, 1.35, 54, 25);

        public RPArmaMachadoUmaMao Machado6()
        => new RPArmaMachadoUmaMao(25, "Machado Armado", RPClasse.UmaMao, 6, new RPDano(14, 42), 0.05, 1.4, 58, 33);

        public RPArmaMachadoUmaMao Machado7()
        => new RPArmaMachadoUmaMao(29, "Machado Decorativo", RPClasse.UmaMao, 6, new RPDano(27, 50), 0.05, 1.2, 80, 23);
    }
}
