namespace CoreGameplay.Matches
{
    public class SwappableProperty : ISwappable
    {
        private bool _canSwap;
        public bool CanSwap()
        {
            return _canSwap;
        }

        public static ISwappable AlwaysTrue => new SwappableProperty(true);
        public static ISwappable AlwaysFalse => new SwappableProperty(false);

        public SwappableProperty(bool canSwap)
        {
            _canSwap = canSwap;
        }
    }
}