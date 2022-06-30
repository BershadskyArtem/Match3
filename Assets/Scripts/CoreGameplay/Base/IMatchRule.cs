using CoreGameplay.Matches;

namespace CoreGameplay.Base
{
    public interface IMatchRule
    {
        public bool TryGetMatchAtPoint(NodeObject[,] board, int xPos, int yPos, ref Match match);
    }
}