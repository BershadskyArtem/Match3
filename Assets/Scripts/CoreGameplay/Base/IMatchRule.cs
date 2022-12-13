using CoreGameplay.Matches;

namespace CoreGameplay.Base
{
    public interface IMatchRule<T>
    {
        public bool TryGetMatchAtPoint(T board, int xPos, int yPos, ref Match match);
    }
}