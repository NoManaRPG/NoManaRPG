using System;
using System.Collections.Generic;
using System.Text;

namespace WafclastRPG.Game.Entities
{
    public class WafclastCharacterAtributos
    {
        public int Forca { get; private set; } = 4;
        public int Agilidade { get; private set; } = 4;
        public int Resistencia { get; private set; } = 4;
        public int Inteligencia { get; private set; } = 4;
        public int Vitalidade { get; private set; } = 4;
        public int PontosLivreAtributo { get; private set; } = 4;
    }
}
