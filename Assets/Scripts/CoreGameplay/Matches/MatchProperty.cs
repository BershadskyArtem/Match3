using CoreGameplay.Kinds;

namespace CoreGameplay.Matches
{
    public class MatchProperty : IMatchable
    {
        private int _id;
        private bool _canMatch;
        public int GetID() => _id;
        public bool CanMatch() => _canMatch;

        public MatchProperty(NodeColor color , bool canMatch = true)
        {
            _id = (int) color;
            _canMatch = canMatch;
        }
    }
}