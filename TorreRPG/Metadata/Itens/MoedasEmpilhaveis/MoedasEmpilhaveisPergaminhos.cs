using System.Collections.Generic;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.MoedasEmpilhaveis
{
    public class MoedasEmpilhaveisPergaminhos
    {
        public List<RPMoedaEmpilhavel> MoedaEmpilhaveisPergaminhoAb()
        {
            var pergaminho = new List<RPMoedaEmpilhavel>();
            pergaminho.Add(PergaminhoFragmento1());
            pergaminho.Add(Pergaminho1());
            return pergaminho;
        }

        public RPMoedaEmpilhavel Pergaminho1()
        => new RPMoedaEmpilhavel(0, "Pergaminho", RPClasse.PergaminhoSabedoria, 1, 40);
        public RPMoedaEmpilhavel PergaminhoFragmento1()
        => new RPMoedaEmpilhavel(0, "Fragmento de Pergaminho", RPClasse.FragmentoPergaminho, 1, 5);
    }
}
