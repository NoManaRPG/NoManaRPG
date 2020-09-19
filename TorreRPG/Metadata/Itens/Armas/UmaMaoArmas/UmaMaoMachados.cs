using System.Collections.Generic;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Armas.UmaMaoArmas
{
    public class UmaMaoMachados
    {
        public List<RPMachadoUmaMao> UmaMaoMachadosAb()
        {
            var machados = new List<RPMachadoUmaMao>();
            machados.Add(UmaMaoMachado1());
            machados.Add(UmaMaoMachado2());
            machados.Add(UmaMaoMachado3());
            machados.Add(UmaMaoMachado4());
            machados.Add(UmaMaoMachado5());
            machados.Add(UmaMaoMachado6());
            machados.Add(UmaMaoMachado7());
            return machados;
        }

        public RPMachadoUmaMao UmaMaoMachado1()
        => new RPMachadoUmaMao(2, "Machadinha Enferrujada", RPClasse.MachadoUmaMao, 6, new RPDano(6, 11), 0.05, 1.5, 12, 6);

        public RPMachadoUmaMao UmaMaoMachado2()
        => new RPMachadoUmaMao(6, "Machadinha de Jade", RPClasse.MachadoUmaMao, 6, new RPDano(10, 15), 0.05, 1.45, 21, 10);

        public RPMachadoUmaMao UmaMaoMachado3()
        => new RPMachadoUmaMao(11, "Machado Revestido", RPClasse.MachadoUmaMao, 6, new RPDano(11, 21), 0.05, 1.5, 28, 19);

        public RPMachadoUmaMao UmaMaoMachado4()
        => new RPMachadoUmaMao(16, "Cutelo", RPClasse.MachadoUmaMao, 6, new RPDano(12, 35), 0.05, 1.30, 48, 14);

        public RPMachadoUmaMao UmaMaoMachado5()
        => new RPMachadoUmaMao(21, "Machado Largo", RPClasse.MachadoUmaMao, 6, new RPDano(19, 34), 0.05, 1.35, 54, 25);

        public RPMachadoUmaMao UmaMaoMachado6()
        => new RPMachadoUmaMao(25, "Machado Armado", RPClasse.MachadoUmaMao, 6, new RPDano(14, 42), 0.05, 1.4, 58, 33);

        public RPMachadoUmaMao UmaMaoMachado7()
        => new RPMachadoUmaMao(29, "Machado Decorativo", RPClasse.MachadoUmaMao, 6, new RPDano(27, 50), 0.05, 1.2, 80, 23);
    }
}
