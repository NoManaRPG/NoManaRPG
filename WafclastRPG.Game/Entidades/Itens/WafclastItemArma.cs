using MongoDB.Bson.Serialization.Attributes;
using System;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Game.Entidades.Itens
{
    [BsonIgnoreExtraElements]
    public class WafclastItemArma : WafclastItemNormal
    {
        public int NivelAtaque { get; set; } //Para equipar
        public int DanoMax { get; set; }
        public int AtaqueVelocidade { get; set; } = 0;
        public int AtaqueVelocidadeMax { get; set; }
        public int Precisao { get; set; }
        public EquipamentoType Slot { get; set; }

        public WafclastItemArma(int itemId, string nome, double precoCompra, int nivelAtaque, int ataqueVelocidadeMax, EquipamentoType slot) : base(itemId, nome, precoCompra)
        {
            this.NivelAtaque = nivelAtaque;
            this.AtaqueVelocidadeMax = ataqueVelocidadeMax;
            this.Slot = slot;
        }

        public void CalcularPrecisao()
        {
            this.Precisao = (int)Math.Truncate(2.5 * ((0.0008 * Math.Pow(this.NivelAtaque, 3)) + (4 * this.NivelAtaque) + 40));
        }

        public void CalcularDanoArma()
        {
            switch (Slot)
            {
                case EquipamentoType.PrimeiraMao:
                    switch (AtaqueVelocidadeMax)
                    {
                        case int n when n <= 4:
                            this.DanoMax = (int)Math.Truncate(9.6 * NivelAtaque);
                            break;
                        case int n when n <= 5:
                            this.DanoMax = (int)Math.Truncate(12.25 * NivelAtaque);
                            break;
                        case int n when n >= 6:
                            this.DanoMax = (int)Math.Truncate(14.9 * NivelAtaque);
                            break;
                    }
                    break;
                case EquipamentoType.SegundaMao:
                    switch (AtaqueVelocidadeMax)
                    {
                        case int n when n <= 4:
                            this.DanoMax = (int)Math.Truncate(4.8 * NivelAtaque);
                            break;
                        case int n when n <= 5:
                            this.DanoMax = (int)Math.Truncate(6.125 * NivelAtaque);
                            break;
                        case int n when n >= 6:
                            this.DanoMax = (int)Math.Truncate(7.45 * NivelAtaque);
                            break;
                    }
                    break;
            }
        }
    }
}
