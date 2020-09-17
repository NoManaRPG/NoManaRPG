using MongoDB.Bson.Serialization.Attributes;

namespace TorreRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPNivel
    {
        public int Atual { get; private set; }
        public double ExpAtual { get; private set; }
        public double ExpMax { get; private set; }

        public RPNivel()
        {
            ExpAtual = 0;
            Atual = 1;
            ExpMax = 525;
        }

        public void PersonagemMorreu() => ExpAtual *= 0.9;

        public int AddExp(double exp)
        {
            double expResultante = ExpAtual + exp;
            int quantEvo = 0;
            if (expResultante >= ExpMax)
            {
                do
                {
                    double quantosPrecisaProxNivel = expResultante - ExpMax;
                    Evoluir();
                    quantEvo++;
                    expResultante = quantosPrecisaProxNivel;
                } while (expResultante >= ExpMax);
                ExpAtual += expResultante;
                return quantEvo;
            }
            ExpAtual += exp;
            return quantEvo;
        }

        private void Evoluir()
        {
            Atual++;
            ExpMax *= 1.0777;
            ExpAtual = 0;
        }
    }
}
