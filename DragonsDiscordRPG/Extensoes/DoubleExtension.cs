namespace DragonsDiscordRPG.Extensoes
{
    public static class DoubleExtension
    {
        public static string Text(this double numero)
          => string.Format("{0:N2}", numero);
    }
}
