using System;

namespace WafclastRPG.Entities.Itens
{
    public class WafclastLevelItem : WafclastBaseItem
    {
        public int Level { get; set; } = 1;
        public double CurrentExperience { get; set; }
        public double ExperienceForNextLevel { get; set; }

        public WafclastLevelItem(WafclastBaseItem baseItem) : base(baseItem)
        {
            ExperienceForNextLevel = this.ExperienceTotalLevel(2);
        }

        public int AddExperience(double experience)
        {
            int niveisEv = 0;
            double expResultante = this.CurrentExperience + experience;
            if (expResultante >= this.ExperienceForNextLevel)
            {
                do
                {
                    expResultante = expResultante - this.ExperienceForNextLevel;
                    this.Evolve();
                    niveisEv++;
                } while (expResultante >= this.ExperienceForNextLevel);
                this.CurrentExperience += expResultante;
                return niveisEv;
            }
            this.CurrentExperience += experience;
            return niveisEv;
        }

        private void Evolve()
        {
            this.Level++;
            this.ExperienceForNextLevel = this.ExperienceTotalLevel(Level + 1);
        }

        private int ExperienceTotalLevel(int level)
        {
            double v1 = 1.0 / 8.0 * level * (level - 1.0) + 75.0;
            double pow1 = Math.Pow(3, (level - 1.0) / 7.0) - 1;
            double pow2 = 1 - Math.Pow(2, -1 / 7.0);
            return (int)Math.Truncate(v1 * (pow1 / pow2));
        }
    }
}
