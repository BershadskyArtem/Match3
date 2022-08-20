using CoreGameplay.Base;
using CoreGameplay.Kinds;
using UnityEngine;

namespace CoreGameplay.Matches.Rules
{
    public class VerticalMatchRule : IMatchRule
    {
        public bool TryGetMatchAtPoint(NodeObject[,] board, int xPos, int yPos, ref Match match)
        {
            int height = board.GetLength(1);
            var swap = board[xPos, yPos].GetMatchable();
            if (!swap.CanMatch()) return false;
            var id = swap.GetID();
            
            //bottom to the top
            for (int y = yPos; y < height; y++)
            {
                if(board[xPos, y] == null) break;
                var swap2 = board[xPos, y].GetMatchable();
                if (!swap2.CanMatch()) break;
                if (swap2.GetID() != id) break;
                
                match.AddPosition(new Vector2Int(xPos , y));
            }
            //from the top
            for (int y = yPos; y >= 0; y--)
            {
                if(board[xPos, y] == null) break;
                var swap2 = board[xPos, y].GetMatchable();
                if (!swap2.CanMatch()) break;
                if (swap2.GetID() != id) break;
                
                match.AddPosition(new Vector2Int(xPos , y));
            }
            match.Kind = MatchKind.Vertical;
            return match.Rank > 2;
        }
    }
}