using System;
using System.Collections.Generic;
using System.Text;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Game.Entidades
{
    public class WafclastRegiao
    {
        public int Id { get; set; }
        public WafclastRegioes Reino { get; set; }
        public string Local { get; set; }
        public List<int> Saidas { get; set; }
        public List<WafclastMonstro> Monstros { get; set; }

        public WafclastRegiao(int id, WafclastRegioes reino, string local, List<int> saidas, List<WafclastMonstro> monstros)
        {
            this.Id = id;
            this.Reino = reino;
            this.Local = local;
            this.Saidas = saidas;
            this.Monstros = monstros;
        }
    }
}
