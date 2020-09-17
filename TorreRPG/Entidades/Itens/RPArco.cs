using System;
using System.Collections.Generic;
using System.Text;
using TorreRPG.Enuns;

namespace TorreRPG.Entidades.Itens
{
    public class RPArco : RPArma
    {
        public RPArco(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque, int destreza) :
            base(dropLevel, tipoBase, classe, espaco, danoFisico, chanceCritico, velocidadeAtaque)
        {
            Destreza = destreza;
        }

        public override string Descricao()
        {
            return "Arcos\n" + base.Descricao();
        }
    }
}
