using System.Collections.Generic;
using CoreGameplay.Base;
using CoreGameplay.Kinds;
using CoreGameplay.Matches;
using UnityEngine;

namespace CoreGameplay.Implementations
{
    public class MatchDiagnoser : IMatchDiagnoser
    {
        private readonly List<IMatchRule> _matchRules;

        private bool[,] _matchedMap;

        public MatchDiagnoser()
        {
            _matchRules = new List<IMatchRule>();
        }
        
        public IEnumerable<Match> GetMatchesFromBoard(NodeObject[,] board)
        {
            if (board == null) return new List<Match>();
            
            int width = board.GetLength(0);
            int height = board.GetLength(1);
            
            _matchedMap = new bool[width, height];
            
            List<Match> result = new List<Match>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if(_matchedMap[x,y]) continue;
                    
                    if(board[x,y] == null) continue;
                    
                    foreach (var matchRule in _matchRules)
                    {
                        var m = new Match(new Vector2Int(x , y) , board[x,y].GetColor());
                        if (!matchRule.TryGetMatchAtPoint(board, x, y, ref m)) continue;
                        
                        result.Add(m);
                        foreach (var position in m.Positions)
                        {
                            _matchedMap[position.x, position.y] = true;
                        }
                        break;
                    }
                }
            }

            return result;
        }

        public Match GetMatchAtPoint(NodeObject[,] board, int xPos, int yPos)
        {
            var pos = new Vector2Int(xPos , yPos);
            var nodeColor = board[xPos, yPos].GetColor();
            if(nodeColor == NodeColor.Unknown) return Match.Zero;
            
            foreach (var matchRule in _matchRules)
            {
                var m = new Match(pos, nodeColor);
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