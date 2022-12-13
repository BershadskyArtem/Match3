using System.Collections.Generic;
using CoreGameplay.Matches;

namespace CoreGameplay.Base
{
    public interface IMatchDiagnoser<T>
    {
        public IEnumerable<Match> GetMatchesFromBoard(T board);
        public Match GetMatchAtPoint(T board , int xPos , int yPos);
        public IMatchDiagnoser<T> AddMatchRule(IMatchRule<T> rule);
        public IMatchDiagnoser<T> ResetRules();
    }
}