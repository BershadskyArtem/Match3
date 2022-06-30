using System.Collections.Generic;
using CoreGameplay.Base;
using CoreGameplay.Matches;
using UnityEngine;

namespace CoreGameplay.Implementations
{
    public class MatchDiagnoser : IMatchDiagnoser
    {
        private readonly List<IMatchRule> _matchRules;

        public MatchDiagnoser()
        {
            _matchRules = new List<IMatchRule>();
        }
        
        public IEnumerable<Match> GetMatchesFromBoard(NodeObject[,] board)
        {
            List<Match> result = new List<Match>();

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    foreach (var matchRule in _matchRules)
                    {
                        var m = new Match(new Vector2Int(x , y) , board[x,y].GetColor());
                        if (matchRule.TryGetMatchAtPoint(board, x, y, ref m))
                        {
                            result.Add(m);
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public Match GetMatchAtPoint(NodeObject[,] board, int xPos, int yPos)
        {
            foreach (var matchRule in _matchRules)
            {
                var m = new Match(new Vector2Int(xPos , yPos) , board[xPos , yPos].GetColor());
                if (matchRule.TryGetMatchAtPoint(board, xPos , yPos, ref m))
                {
                    return m;
                }
            }
            return Match.Zero;
        }

        public IMatchDiagnoser AddMatchRule(IMatchRule rule)
        {
            if(_matchRules.Contains(rule))return this;
            _matchRules.Add(rule);
            return this;
        }

        public IMatchDiagnoser ResetRules()
        {
            _matchRules.Clear();
            return this;
        }
    }
}