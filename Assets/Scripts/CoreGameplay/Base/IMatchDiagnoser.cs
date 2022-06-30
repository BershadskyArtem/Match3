using System.Collections.Generic;
using CoreGameplay.Matches;

namespace CoreGameplay.Base
{
    public interface IMatchDiagnoser
    {
        public IEnumerable<Match> GetMatchesFromBoard(NodeObject[,] board);
        public Match GetMatchAtPoint(NodeObject[,] board , int xPos , int yPos);
        public IMatchDiagnoser AddMatchRule(IMatchRule rule);
        public IMatchDiagnoser ResetRules();
    }
}