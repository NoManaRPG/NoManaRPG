using TorreRPG.Enuns;

namespace TorreRPG.Entidades.Itens
{
    public class RPFrascoVida : RPBaseFrasco
    {
        public RPFrascoVida(int dropLevel, string tipoBase, RPClasse classe, int espaco, double regen,
            double tempo, double cargasUso, double cargasMax) :
            base(dropLevel, tipoBase, classe, espaco, regen, tempo, cargasUso, cargasMax)
        {
        }

        public virtual string Descricao()
        {
            return $"Recupera {Regen} de Vida por {Tempo} Segundo\n" +
                $"Consome {CargasUso} de {CargasMax} Carga na utilização\n" +
                $"Atualmente tem {CargasAtual} Carga\n" +
                $"---------------\n" +
                $"Só é possível manter cargas no cinto. Recarrega conforme você mata monstros.";
        }
    }
}
