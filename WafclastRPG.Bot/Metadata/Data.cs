using WafclastRPG.Bot;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Entidades.Itens;
using WafclastRPG.Game.Metadata.Itens;

namespace WafclastRPG.Game.Metadata
{
    public sealed class Data
    {
        private readonly Banco banco;

        public Data(Banco banco)
        {
            this.banco = banco;
            CarregarRegioes();
            CarregarArmas();
            CarregarComidas();
        }

        private void CarregarRegioes()
        {
            var regioes = typeof(Regioes).GetMethods();
            for (int i = 0; i < regioes.Length - 4; i++)
            {
                var reg = (WafclastRegiao)regioes[i].Invoke(null, null);
                banco.ReplaceRegiaoAsync(reg);
            }
        }

        private void CarregarArmas()
        {
            var itens = typeof(Armas).GetMethods();
            for (int i = 0; i < itens.Length - 4; i++)
            {
                var reg = (WafclastItemArma)itens[i].Invoke(null, null);
                banco.ReplaceItemAsync(reg);
            }
        }

        private void CarregarComidas()
        {
            var itens = typeof(Comidas).GetMethods();
            for (int i = 0; i < itens.Length - 4; i++)
            {
                var reg = (WafclastItemComida)itens[i].Invoke(null, null);
                banco.ReplaceItemAsync(reg);
            }
        }
    }
}
