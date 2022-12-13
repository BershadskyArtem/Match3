using CoreGameplay.Base;
using CoreGameplay.Kinds;
using CoreGameplay.Nodes;
using UnityEngine;

namespace CoreGameplay.Matches.Rules
{
    public class InfoVerticalMatchRule : IMatchRule<NodeInfo[,]>
    {
        public bool TryGetMatchAtPoint(NodeInfo[,] board, int xPos, int yPos, ref Match match)
        {
            int height = board.GetLength(1);
            bool isMatchable = board[xPos, yPos].IsMatchable;
            
            if (!isMatchable) return false;
            var id = board[xPos, yPos].Color;
            
            //bottom to the top
            for (int y = yPos; y < height; y++)
            {
                if(board[xPos, y] == null) break;
                var isMatchable2 = board[xPos, y].IsMatchable;
                if (!isMatchable2) break;
                if (board[xPos, y].Color != id) break;
                
                match.AddPosition(new Vector2Int(xPos , y));
            }
            //from the top
            for (int y = yPos; y >= 0; y--)
            {
                if(board[xPos, y] == null) break;
                var isMatchable2 = board[xPos, y].IsMatchable;
                if (!isMatchable2) break;
                if (board[xPos, y].Color != id) break;
                
                match.AddPosition(new Vector2Int(xPos , y));
            }
            match.Kind = MatchKind.Vertical;
            return match.Rank > 2;
        }
    }
}