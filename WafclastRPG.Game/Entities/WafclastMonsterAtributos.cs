namespace WafclastRPG.Game.Entities
{
    public class WafclastMonsterAtributos
    {
        public int ForcaMin { get; set; }
        public int ForcaMax { get; set; }
        public int Forca { get; set; }

        public int ResistenciaMin { get; set; }
        public int ResistenciaMax { get; set; }
        public int Resistencia { get; set; }

        public int AgilidadeMin { get; set; }
        public int AgilidadeMax { get; set; }
        public int Agilidade { get; set; }

        public decimal ExpMin { get; set; }
        public decimal ExpMax { get; set; }

        public WafclastMonsterAtributos(
            int forcaMin,
            int forcaMax,
            int resistenciaMin,
            int resistenciaMax,
            int agilidadeMin,
            int agilidadeMax,
            decimal expMin,
            decimal expMax)
        {
            ForcaMin = forcaMin;
            ForcaMax = forcaMax;
            ResistenciaMin = resistenciaMin;
            ResistenciaMax = resistenciaMax;
            AgilidadeMin = agilidadeMin;
            AgilidadeMax = agilidadeMax;
            ExpMin = expMin;
            ExpMax = expMax;
        }
    }
}
