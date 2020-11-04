using System.Collections.Generic;
using WafclastRPG.Game.Entidades.Itens;
using WafclastRPG.Game.Enuns;

namespace WafclastRPG.Game.Metadata.Itens.MoedasEmpilhaveis
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

        public RPMoedaEmpilhavel PergaminhoFragmento()
        => new RPMoedaEmpilhavel(0, "Fragmento de Pergaminho", RPClasse.FragmentoPergaminho, 1, 5);
        public RPMoedaEmpilhavel PergaminhoSabedoria()
        => new RPMoedaEmpilhavel(1, "Pergaminho de Sabedoria", RPClasse.PergaminhoSabedoria, 1, 40);
        public RPMoedaEmpilhavel PergaminhoPortal()
        => new RPMoedaEmpilhavel(4, "Pergaminho de Portal", RPClasse.PergaminhoPortal, 1, 40);
    }
}
