using System.Collections.Generic;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.MoedasEmpilhaveis
{
    public class MoedasEmpilhaveis
    {
        public List<RPMoedaEmpilhavel> MoedaEmpilhaveisAb()
        {
            var pergaminho = new List<RPMoedaEmpilhavel>();
            pergaminho.Add(PergaminhoSabedoria());
            pergaminho.Add(PergaminhoPortal());
            return pergaminho;
        }

        public RPMoedaEmpilhavel PergaminhoSabedoria()
        => new RPMoedaEmpilhavel(1, "Pergaminho de Sabedoria", RPClasse.PergaminhoSabedoria, 1, 40);
        public RPMoedaEmpilhavel PergaminhoFragmento()
        => new RPMoedaEmpilhavel(0, "Fragmento de Pergaminho", RPClasse.FragmentoPergaminho, 1, 5);
        public RPMoedaEmpilhavel PergaminhoPortal()
        => new RPMoedaEmpilhavel(4, "Pergaminho de Portal", RPClasse.PergaminhoPortal, 1, 40);
    }
}
