using DragonsDiscordRPG.Enuns;
using MongoDB.Bson.Serialization.Attributes;

namespace DragonsDiscordRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPItem
    {
        public RPItem(RPTipo tipo, string nome, int nivel, string urlImage)
        {
            Tipo = tipo;
            Nome = nome;
            Nivel = nivel;
            UrlImage = urlImage;
        }

        public RPTipo Tipo { get; set; }
        public string Nome { get; set; }
        public int Nivel { get; set; }
        public string UrlImage { get; set; }

        #region Poção

        public double LifeRegen { get; set; }
        public double ManaRegen { get; set; }
        public double Tempo { get; set; }
        public double CargasMax { get; set; }
        public double CargasAtual { get; set; }
        public double CargasUso { get; set; }

        public void AddCarga(double valor)
        {
            CargasAtual += valor;
            if (CargasAtual > CargasMax) CargasAtual = CargasMax;
        }

        public bool RemoverCarga(double valor)
        {
            if (CargasAtual >= valor)
            {
                CargasAtual -= valor;
                return true;
            }
            return false;
        }


        #endregion
    }
}
