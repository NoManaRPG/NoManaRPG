// This file is part of the WafclastRPG project.

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities
{
    public enum FabricationType
    {
        Cook,
    }

    public class Requirement
    {
        public string Name { get; set; }
        public ulong Quantity { get; set; }
    }

    public class WafclastFabrication
    {
        /// <summary>
        /// Nome do item que será fabricado.
        /// </summary>
        [BsonId]
        public string Name { get; set; }

        /// <summary>
        /// Quantidade de itens que será fabricado.
        /// </summary>
        public ulong Quantity { get; set; }

        /// <summary>
        /// Quantos de experiencia ganha ao fabricar o item.
        /// <para>Depende do tipo</para>
        /// </summary>
        public double Experience { get; set; }

        /// <summary>
        /// Seus requisitos em item para fabricar o item.
        /// </summary>
        public List<Requirement> RequiredItems { get; set; }

        /// <summary>
        /// O tipo de fabricação para determinar o nível necessário.
        /// <para>Também determina o metodo de fabricação</para>
        /// </summary>
        public FabricationType Type { get; set; }

        /// <summary>
        /// Nível necessario da habilidade, depende do tipo.
        /// </summary>
        public int RequiredLevel { get; set; }

        /// <summary>
        /// A chance para fabricar o item.
        /// </summary>
        public double Chance { get; set; }
    }

}
