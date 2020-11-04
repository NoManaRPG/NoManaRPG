using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WafclastRPG.Game.Enuns
{
    public enum WafclastTipo
    {
        [Description("Frasco")]
        Frasco,
        [Description("Arma de Duas Mãos")]
        DuasMao,
        [Description("Arma de Uma Mão")]
        UmaMao,
        [Description("Fragmento de Pergaminho")]
        FragmentoPergaminho,
        [Description("Pergaminho de Sabedoria")]
        PergaminhoSabedoria,
        [Description("Pergaminho de Portal")]
        PergaminhoPortal,
    }
}
